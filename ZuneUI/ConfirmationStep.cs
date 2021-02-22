// Decompiled with JetBrains decompiler
// Type: ZuneUI.ConfirmationStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System.Collections;

namespace ZuneUI
{
    public class ConfirmationStep : AccountManagementStep
    {
        private ArrayList _bulletItems;
        private EBillingOfferType _offerType;
        private ulong _offerId;

        public ConfirmationStep(Wizard owner, AccountManagementWizardState state, bool parent)
          : base(owner, state, parent)
        {
            this.Description = Shell.LoadString(StringId.IDS_CONFIRM_SUBSCRIPTION_HEADER);
            this.NextTextOverride = Shell.LoadString(StringId.IDS_BILLING_CONTINUE);
            this._bulletItems = null;
            this._offerType = EBillingOfferType.Unknown;
            this._offerId = 0UL;
            this.RequireSignIn = true;
        }

        public override string UI => "res://ZuneShellResources!AccountInfo.uix#ConfirmationStep";

        public EBillingOfferType OfferType
        {
            get => this._offerType;
            set
            {
                this._offerType = value;
                this.FirePropertyChanged(nameof(OfferType));
            }
        }

        public ArrayList BulletItems
        {
            get => this._bulletItems;
            set
            {
                this._bulletItems = value;
                this.FirePropertyChanged(nameof(BulletItems));
            }
        }

        public override bool IsEnabled
        {
            get
            {
                bool flag = base.IsEnabled;
                if (flag)
                    flag = this.State.IsPurchaseConfirmationNeeded && (this.OfferType == EBillingOfferType.Subscription || this.OfferType == EBillingOfferType.Renewal || this.OfferType == EBillingOfferType.Trial);
                return flag;
            }
        }

        protected override void OnActivate()
        {
            if ((long)this._offerId != (long)this.State.SelectBillingOfferStep.SelectedBillingOffer.Id || this.BulletItems == null)
                this.ServiceActivationRequestsDone = false;
            base.OnActivate();
        }

        protected override void OnStartActivationRequests(object state)
        {
            ArrayList bulletStrings;
            HRESULT subscriptionDetails = this.State.AccountManagement.GetSubscriptionDetails(this.State.SelectBillingOfferStep.SelectedBillingOffer.Id.ToString(), out bulletStrings);
            if (subscriptionDetails.IsError)
            {
                bulletStrings = null;
                this.SetError(subscriptionDetails, null);
            }
            this._offerId = this.State.SelectBillingOfferStep.SelectedBillingOffer.Id;
            this.EndActivationRequests(bulletStrings);
        }

        protected override void OnEndActivationRequests(object args)
        {
            if (args == null)
                this.NavigateToErrorHandler();
            else
                this.BulletItems = (ArrayList)args;
        }
    }
}
