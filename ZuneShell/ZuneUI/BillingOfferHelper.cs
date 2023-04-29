// Decompiled with JetBrains decompiler
// Type: ZuneUI.BillingOfferHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.ErrorMapperApi;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace ZuneUI
{
    public class BillingOfferHelper : ModelItem
    {
        private BillingOfferCollection m_pointsOffers;
        private BillingOfferCollection m_subscriptionOffers;
        private HRESULT m_errorCode;
        private BillingOffer m_currentSubscription;
        private BillingOffer m_renewalSubscription;

        public event EventHandler PurchaseComplete;

        public event EventHandler PurchaseFailed;

        public void GetSubscriptionOffers()
        {
            this.ErrorCode = HRESULT._S_OK;
            ZuneApplication.Service2.GetSubscriptionOffers(new GetBillingOffersCompleteCallback(this.OnGetSubscriptionsComplete), new GetBillingOffersErrorCallback(this.OnGetSubscriptionsError));
        }

        public void GetPointsOffers()
        {
            this.ErrorCode = HRESULT._S_OK;
            ZuneApplication.Service2.GetPointsOffers(new GetBillingOffersCompleteCallback(this.OnGetPointsOffersComplete), new GetBillingOffersErrorCallback(this.OnGetPointsOffersError));
        }

        public void GetCurrentSubscription()
        {
            this.ErrorCode = HRESULT._S_OK;
            if (SignIn.Instance.SignedInWithSubscription)
                ZuneApplication.Service2.GetSubscriptionDetails(SignIn.Instance.SubscriptionId, new GetBillingOffersCompleteCallback(this.OnGetCurrentSubscriptionComplete), new GetBillingOffersErrorCallback(this.OnGetCurrentSubscriptionError));
            else
                this.CurrentSubscription = null;
        }

        public void GetRenewalSubscription()
        {
            this.ErrorCode = HRESULT._S_OK;
            ulong subscriptionRenewalId = SignIn.Instance.SubscriptionRenewalId;
            if (SignIn.Instance.SignedInWithSubscription && subscriptionRenewalId != 0UL)
                ZuneApplication.Service2.GetSubscriptionDetails(subscriptionRenewalId, new GetBillingOffersCompleteCallback(this.OnGetRenewalSubscriptionComplete), new GetBillingOffersErrorCallback(this.OnGetRenewalSubscriptionError));
            else
                this.RenewalSubscription = null;
        }

        public void Purchase(BillingOffer billingOffer, PaymentInstrument paymentInstrument)
        {
            this.ErrorCode = HRESULT._S_OK;
            if (billingOffer == null)
                return;
            AsyncCompleteHandler callback = billingOffer.OfferType != EBillingOfferType.Points ? new AsyncCompleteHandler(this.OnPurchaseSubscriptionComplete) : new AsyncCompleteHandler(this.OnPurchasePointsComplete);
            ZuneApplication.Service2.PurchaseBillingOffer(billingOffer, paymentInstrument, callback);
        }

        public static bool IsSubscribed(BillingOffer offer)
        {
            bool flag = false;
            if (offer != null && ZuneApplication.Service2.IsSignedInWithSubscription())
                flag = (long)SignIn.Instance.SubscriptionId == (long)offer.Id;
            return flag;
        }

        public static bool IsSubscriptionChanging()
        {
            bool flag = false;
            if (ZuneApplication.Service2.IsSignedInWithSubscription())
                flag = (long)SignIn.Instance.SubscriptionRenewalId != (long)SignIn.Instance.SubscriptionId;
            return flag;
        }

        public static bool IsLightWeightError(HRESULT hr)
        {
            if (HRESULT._NS_E_BILLING_LIGHTWEIGHT_ACCOUNT == hr)
                return true;
            ErrorMapperResult descriptionAndUrl = ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hr.Int);
            return HRESULT._NS_E_BILLING_LIGHTWEIGHT_ACCOUNT.Int == descriptionAndUrl.Hr;
        }

        public IList PointsOffers => this.m_pointsOffers == null ? null : this.m_pointsOffers.Items;

        public IList Subscriptions => this.m_subscriptionOffers == null ? null : this.m_subscriptionOffers.Items;

        public BillingOffer CurrentSubscription
        {
            get => this.m_currentSubscription;
            private set
            {
                if (this.m_currentSubscription == value)
                    return;
                this.m_currentSubscription = value;
                this.FirePropertyChanged(nameof(CurrentSubscription));
            }
        }

        public BillingOffer RenewalSubscription
        {
            get => this.m_renewalSubscription;
            private set
            {
                if (this.m_renewalSubscription == value)
                    return;
                this.m_renewalSubscription = value;
                this.FirePropertyChanged(nameof(RenewalSubscription));
            }
        }

        public HRESULT ErrorCode
        {
            get => this.m_errorCode;
            set
            {
                if (!(this.m_errorCode != value))
                    return;
                this.m_errorCode = value;
                this.FirePropertyChanged(nameof(ErrorCode));
            }
        }

        private void SetError(HRESULT hrError) => this.ErrorCode = hrError;

        private void OnGetSubscriptionsComplete(BillingOfferCollection subscriptions) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetSubscriptionsComplete), subscriptions);

        private void OnGetSubscriptionsError(HRESULT hrError) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetError), hrError);

        private void OnGetCurrentSubscriptionComplete(BillingOfferCollection subscriptions) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetSubsciptionComplete), new object[3]
        {
       EBillingOfferType.Subscription,
       SignIn.Instance.SubscriptionRenewalId,
       subscriptions
        });

        private void OnGetCurrentSubscriptionError(HRESULT hrError)
        {
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetError), hrError);
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetSubsciptionComplete), new object[3]
            {
         EBillingOfferType.Subscription,
         SignIn.Instance.SubscriptionRenewalId,
        null
            });
        }

        private void OnGetRenewalSubscriptionComplete(BillingOfferCollection subscriptions) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetSubsciptionComplete), new object[3]
        {
       EBillingOfferType.Renewal,
       SignIn.Instance.SubscriptionRenewalId,
       subscriptions
        });

        private void OnGetRenewalSubscriptionError(HRESULT hrError)
        {
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetError), hrError);
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetSubsciptionComplete), new object[3]
            {
         EBillingOfferType.Renewal,
         SignIn.Instance.SubscriptionRenewalId,
        null
            });
        }

        private void OnGetPointsOffersComplete(BillingOfferCollection pointsOffers) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredGetPointsOffersComplete), pointsOffers);

        private void OnGetPointsOffersError(HRESULT hrError) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredSetError), hrError);

        private void OnPurchaseSubscriptionComplete(HRESULT hrError) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredPurchaseComplete), new object[2]
        {
       hrError,
       true
        });

        private void OnPurchasePointsComplete(HRESULT hrError) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredPurchaseComplete), new object[2]
        {
       hrError,
       false
        });

        private void DeferredGetSubscriptionsComplete(object args)
        {
            BillingOfferCollection billingOfferCollection = (BillingOfferCollection)args;
            if (this.IsDisposed)
            {
                billingOfferCollection?.Dispose();
            }
            else
            {
                if (this.m_subscriptionOffers != null)
                    this.m_subscriptionOffers.Dispose();
                this.m_subscriptionOffers = billingOfferCollection;
                this.FirePropertyChanged("Subscriptions");
            }
        }

        private void DeferredGetSubsciptionComplete(object args)
        {
            object[] objArray = (object[])args;
            EBillingOfferType eBillingOfferType = (EBillingOfferType)objArray[0];
            ulong id = (ulong)objArray[1];
            BillingOfferCollection billingOfferCollection = objArray[2] as BillingOfferCollection;
            BillingOffer billingOffer = null;
            if (billingOfferCollection != null && billingOfferCollection.Items != null && billingOfferCollection.Items.Count > 0)
                billingOffer = billingOfferCollection.Items[0] as BillingOffer;
            if (billingOffer == null && SignIn.Instance.SignedInWithSubscription)
                billingOffer = new BillingOffer(id, eBillingOfferType, string.Empty);
            if (eBillingOfferType == EBillingOfferType.Subscription)
            {
                this.CurrentSubscription = billingOffer;
            }
            else
            {
                if (eBillingOfferType != EBillingOfferType.Renewal)
                    return;
                this.RenewalSubscription = billingOffer;
            }
        }

        private void DeferredGetPointsOffersComplete(object args)
        {
            BillingOfferCollection billingOfferCollection = (BillingOfferCollection)args;
            if (this.IsDisposed)
            {
                billingOfferCollection?.Dispose();
            }
            else
            {
                if (this.m_pointsOffers != null)
                    this.m_pointsOffers.Dispose();
                this.m_pointsOffers = billingOfferCollection;
                this.FirePropertyChanged("PointsOffers");
            }
        }

        private void DeferredPurchaseComplete(object args)
        {
            HRESULT hrError = (HRESULT)((object[])args)[0];
            bool flag = (bool)((object[])args)[1];
            if (hrError.IsError)
            {
                this.SetError(hrError);
                if (this.PurchaseFailed != null)
                    this.PurchaseFailed(this, null);
                this.FirePropertyChanged("PurchaseFailed");
            }
            else
            {
                if (flag)
                    SignIn.Instance.RefreshAccount();
                if (this.PurchaseComplete != null)
                    this.PurchaseComplete(this, null);
                this.FirePropertyChanged("PurchaseComplete");
            }
        }

        private void DeferredSetError(object args) => this.SetError((HRESULT)args);

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (!disposing)
                return;
            if (this.m_subscriptionOffers != null)
            {
                this.m_subscriptionOffers.Dispose();
                this.m_subscriptionOffers = null;
            }
            if (this.m_pointsOffers == null)
                return;
            this.m_pointsOffers.Dispose();
            this.m_pointsOffers = null;
        }
    }
}
