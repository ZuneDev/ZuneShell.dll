// Decompiled with JetBrains decompiler
// Type: ZuneUI.SelectPaymentInstrumentStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System.Collections;
using System.ComponentModel;

namespace ZuneUI
{
    public class SelectPaymentInstrumentStep : AccountManagementStep
    {
        private PaymentInstrumentHelper _helper;
        private bool _loadedCreditCards;
        private CreditCard _committedCreditCard;

        public SelectPaymentInstrumentStep(
          Wizard owner,
          AccountManagementWizardState state,
          bool parent)
          : base(owner, state, parent)
        {
            this.Description = Shell.LoadString(StringId.IDS_BILLING_EDIT_CC_EDIT_HEADER);
            this._helper = new PaymentInstrumentHelper();
            this._helper.PropertyChanged += new PropertyChangedEventHandler(this.HelperPropertyChanged);
            this.RequireSignIn = true;
        }

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (this._helper == null || !disposing)
                return;
            this._helper.PropertyChanged -= new PropertyChangedEventHandler(this.HelperPropertyChanged);
            this._helper.Dispose();
            this._helper = (PaymentInstrumentHelper)null;
        }

        public override string UI => "res://ZuneShellResources!AccountInfo.uix#SelectPaymentInstrumentStep";

        public override bool IsEnabled
        {
            get
            {
                bool flag = base.IsEnabled;
                if (flag)
                    flag = this._owner.CurrentPage == this ? this._loadedCreditCards : !this._loadedCreditCards || this.CreditCards != null && this.CreditCards.Count > 0;
                return flag;
            }
        }

        public CreditCard CommittedCreditCard
        {
            get => this._committedCreditCard;
            set
            {
                if (this._committedCreditCard == value)
                    return;
                this._committedCreditCard = value;
                this.ResetNextTextOverride();
                this.FirePropertyChanged(nameof(CommittedCreditCard));
            }
        }

        protected virtual void ResetNextTextOverride()
        {
            if (this._committedCreditCard != null && this.State.SelectBillingOfferStep.SubscriptionsOnly && !this.State.IsPurchaseConfirmationNeeded)
                this.NextTextOverride = Shell.LoadString(StringId.IDS_BILLING_SIGN_UP);
            else if (this._committedCreditCard != null && this.State.SelectBillingOfferStep.PointsOffersOnly)
                this.NextTextOverride = Shell.LoadString(StringId.IDS_BILLING_BUY_BTN);
            else
                this.NextTextOverride = (string)null;
        }

        public IList CreditCards => this._helper.CreditCards;

        protected override void OnActivate()
        {
            base.OnActivate();
            if (this._loadedCreditCards || this.CreditCards != null && this.CreditCards.Count != 0)
                return;
            this._helper.GetPaymentInstruments();
        }

        private void HelperPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (this.IsDisposed || this._helper == null)
                return;
            if (args.PropertyName == "CreditCards")
            {
                this._loadedCreditCards = true;
                this.FirePropertyChanged("PaymentInstruments");
                this.FirePropertyChanged("IsEnabled");
                if (this._helper.CreditCards != null && this._helper.CreditCards.Count != 0)
                    return;
                this._owner.MoveNext();
            }
            else if (args.PropertyName == "Default")
            {
                this.CommittedCreditCard = this._helper.Default as CreditCard;
            }
            else
            {
                if (!(args.PropertyName == "ErrorCode") || !this._helper.ErrorCode.IsError)
                    return;
                this.SetError(this._helper.ErrorCode, (ServiceError)null);
            }
        }
    }
}
