// Decompiled with JetBrains decompiler
// Type: ZuneUI.SelectBillingOfferStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System.Collections;
using System.ComponentModel;

namespace ZuneUI
{
    public class SelectBillingOfferStep : AccountManagementStep
    {
        private EBillingOfferType _offerTypes;
        private BillingOffer _selectedBillingOffer;
        private BillingOfferHelper _helper;

        public SelectBillingOfferStep(Wizard owner, AccountManagementWizardState state)
          : base(owner, state, false)
        {
            this._helper = new BillingOfferHelper();
            this._helper.PropertyChanged += new PropertyChangedEventHandler(this.HelperPropertyChanged);
            this._offerTypes = EBillingOfferType.Unknown;
            this.SetDescription();
            this.RequireSignIn = true;
            this.Initialize(null);
        }

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (this._helper == null || !disposing)
                return;
            this._helper.PropertyChanged -= new PropertyChangedEventHandler(this.HelperPropertyChanged);
            this._helper.Dispose();
            this._helper = null;
        }

        public override string UI => "res://ZuneShellResources!AccountInfo.uix#SelectBillingOfferStep";

        public override bool IsEnabled
        {
            get
            {
                bool flag = base.IsEnabled;
                if (flag)
                    flag = this._owner.CurrentPage != this || this.Subscriptions != null && this.SubscriptionsOnly || this.PointsOffers != null && this.PointsOffersOnly;
                return flag;
            }
        }

        public bool TrialOnly => this._offerTypes == EBillingOfferType.Trial;

        public bool SubscriptionsOnly => this._offerTypes == EBillingOfferType.Subscription || this._offerTypes == EBillingOfferType.Trial || this._offerTypes == EBillingOfferType.Renewal;

        public bool PointsOffersOnly => this._offerTypes == EBillingOfferType.Points;

        public EBillingOfferType ShowOffers
        {
            get => this._offerTypes;
            set
            {
                if (this._offerTypes == value)
                    return;
                this._offerTypes = value;
                this.SetDescription();
                this.FirePropertyChanged(nameof(ShowOffers));
                this.FirePropertyChanged("TrialOnly");
                this.FirePropertyChanged("SubscriptionsOnly");
                this.FirePropertyChanged("PointsOffersOnly");
                this.FirePropertyChanged("IsEnabled");
            }
        }

        public BillingOffer TrialOffer
        {
            get
            {
                BillingOffer billingOffer = null;
                if (this._helper.Subscriptions != null)
                {
                    foreach (BillingOffer subscription in _helper.Subscriptions)
                    {
                        if (subscription.Trial)
                        {
                            billingOffer = subscription;
                            break;
                        }
                    }
                }
                return billingOffer;
            }
        }

        public IList Subscriptions => this._helper.Subscriptions;

        public IList PointsOffers => this._helper.PointsOffers;

        public BillingOffer SelectedBillingOffer
        {
            get => this._selectedBillingOffer;
            set
            {
                if (this._selectedBillingOffer == value)
                    return;
                this._selectedBillingOffer = value;
                this.FirePropertyChanged(nameof(SelectedBillingOffer));
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            if (this.Subscriptions == null && this.SubscriptionsOnly)
            {
                this._helper.GetSubscriptionOffers();
            }
            else
            {
                if (this.PointsOffers != null || !this.PointsOffersOnly)
                    return;
                this._helper.GetPointsOffers();
            }
        }

        private void HelperPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (this.IsDisposed || this._helper == null)
                return;
            if (args.PropertyName == "Subscriptions")
            {
                if (this.TrialOnly)
                {
                    this.SelectedBillingOffer = this.TrialOffer;
                    if (this.SelectedBillingOffer == null)
                        this.ShowOffers = EBillingOfferType.Subscription;
                }
                this.FirePropertyChanged("TrialOffer");
                this.FirePropertyChanged("Subscriptions");
                this.FirePropertyChanged("IsEnabled");
            }
            else if (args.PropertyName == "PointsOffers")
            {
                this.FirePropertyChanged("PointsOffers");
                this.FirePropertyChanged("IsEnabled");
            }
            else
            {
                if (!(args.PropertyName == "ErrorCode") || !this._helper.ErrorCode.IsError)
                    return;
                this.SetError(this._helper.ErrorCode, null);
            }
        }

        private void SetDescription()
        {
            if (this._offerTypes == EBillingOfferType.Trial)
                this.Description = Shell.LoadString(StringId.IDS_BILLING_TRIAL_TITLE);
            else if (this._offerTypes == EBillingOfferType.Subscription)
                this.Description = Shell.LoadString(StringId.IDS_BILLING_ZUNE_PASS_HEADER);
            else if (this._offerTypes == EBillingOfferType.Points)
                this.Description = Shell.LoadString(StringId.IDS_BILLING_PURCHASE_POINTS_HEADER);
            else
                this.Description = null;
        }
    }
}
