// Decompiled with JetBrains decompiler
// Type: ZuneUI.PaymentInstrumentStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneUI
{
    public class PaymentInstrumentStep : PaymentInstrumentStepBase
    {
        private bool _firstView;

        public PaymentInstrumentStep(Wizard owner, AccountManagementWizardState state, bool parent)
          : base(owner, state, parent)
        {
            this._firstView = true;
            this.RequireSignIn = true;
        }

        public override bool IsEnabled => base.IsEnabled && this.State.SelectPaymentInstrumentStep.CommittedCreditCard == null;

        protected override bool OnCommitChanges()
        {
            bool flag = !this.IsEnabled;
            if (!flag && this.CommittedCreditCard != null)
            {
                string paymentId = (string)null;
                ServiceError serviceError;
                HRESULT hr = (HRESULT)Microsoft.Zune.Service.Service.Instance.AddPaymentInstrument((PaymentInstrument)this.CommittedCreditCard, out paymentId, out serviceError);
                if (hr.IsError)
                {
                    this.SetError(hr, serviceError);
                }
                else
                {
                    this.CommittedCreditCard.Id = paymentId;
                    flag = true;
                }
            }
            return flag;
        }

        protected override void OnActivate()
        {
            this.UpdateButtonText();
            if (this.State.ContactInfoStep.IsEnabled && this._firstView)
            {
                this._firstView = false;
                string committedValue1 = this.State.ContactInfoStep.GetCommittedValue(BaseContactInfoPropertyEditor.FirstName) as string;
                string committedValue2 = this.State.ContactInfoStep.GetCommittedValue(BaseContactInfoPropertyEditor.LastName) as string;
                this.SetCommittedValue(PaymentInstrumentPropertyEditor.AccountHolderName, (object)string.Format(Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_NAME_FORMAT), (object)committedValue1, (object)committedValue2));
                this.SetCommittedValue(PaymentInstrumentPropertyEditor.City, this.State.ContactInfoStep.GetCommittedValue(ContactInfoPropertyEditor.City));
                this.SetCommittedValue(PaymentInstrumentPropertyEditor.Country, this.State.ContactInfoStep.GetCommittedValue(ContactInfoPropertyEditor.Country));
                this.SetCommittedValue(PaymentInstrumentPropertyEditor.District, this.State.ContactInfoStep.GetCommittedValue(ContactInfoPropertyEditor.District));
                this.SetCommittedValue(PaymentInstrumentPropertyEditor.PhoneExtension, this.State.ContactInfoStep.GetCommittedValue(BaseContactInfoPropertyEditor.PhoneExtension));
                this.SetCommittedValue(PaymentInstrumentPropertyEditor.PhoneNumber, this.State.ContactInfoStep.GetCommittedValue(BaseContactInfoPropertyEditor.PhoneNumber));
                this.SetPropertyState(PaymentInstrumentPropertyEditor.PostalCode, this.State.ContactInfoStep.GetCommittedValue(ContactInfoPropertyEditor.Country));
                this.SetCommittedValue(PaymentInstrumentPropertyEditor.PostalCode, this.State.ContactInfoStep.GetCommittedValue(ContactInfoPropertyEditor.PostalCode));
                this.SetPropertyState(PaymentInstrumentPropertyEditor.State, this.State.ContactInfoStep.GetCommittedValue(ContactInfoPropertyEditor.Country));
                this.SetCommittedValue(PaymentInstrumentPropertyEditor.State, this.State.ContactInfoStep.GetCommittedValue(ContactInfoPropertyEditor.State));
                this.SetCommittedValue(PaymentInstrumentPropertyEditor.Street1, this.State.ContactInfoStep.GetCommittedValue(ContactInfoPropertyEditor.Street1));
                this.SetCommittedValue(PaymentInstrumentPropertyEditor.Street2, this.State.ContactInfoStep.GetCommittedValue(ContactInfoPropertyEditor.Street2));
            }
            base.OnActivate();
        }

        internal override bool OnMovingNext()
        {
            this.CommittedCreditCard = this.CreateCreditCard();
            if (this.State.ContactInfoStep.IsEnabled)
            {
                this.CommittedCreditCard.ContactFirstName = this.State.ContactInfoStep.GetCommittedValue(BaseContactInfoPropertyEditor.FirstName) as string;
                this.CommittedCreditCard.ContactLastName = this.State.ContactInfoStep.GetCommittedValue(BaseContactInfoPropertyEditor.LastName) as string;
                this.CommittedCreditCard.Email = this.State.ContactInfoStep.GetCommittedValue(BaseContactInfoPropertyEditor.Email) as string;
            }
            return base.OnMovingNext();
        }

        private void UpdateButtonText()
        {
            if (this.State.SelectBillingOfferStep.SubscriptionsOnly && !this.State.IsPurchaseConfirmationNeeded)
            {
                this.NextTextOverride = Shell.LoadString(StringId.IDS_BILLING_SIGN_UP);
            }
            else
            {
                if (!this.State.SelectBillingOfferStep.PointsOffersOnly)
                    return;
                this.NextTextOverride = Shell.LoadString(StringId.IDS_BILLING_BUY_BTN);
            }
        }
    }
}
