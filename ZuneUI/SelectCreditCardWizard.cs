// Decompiled with JetBrains decompiler
// Type: ZuneUI.SelectCreditCardWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneUI
{
    public class SelectCreditCardWizard : AccountManagementWizard
    {
        private AccountManagementFinishStep _finishStep;
        private AccountManagementErrorPage _errorStep;
        private CreditCard _selectedCreditCard;

        public SelectCreditCardWizard()
        {
            this.State.ContactInfoStep.LightWeightOnly = true;
            this._finishStep = (AccountManagementFinishStep)new SelectCreditCardFinishStep((Wizard)this, this.State, Shell.LoadString(StringId.IDS_ACCOUNT_FINISHED_DESCRIPTION));
            this._errorStep = new AccountManagementErrorPage((Wizard)this, Shell.LoadString(StringId.IDS_ACCOUNT_ADD_CC_TO_ACCOUNT_ERROR_TITLE), Shell.LoadString(StringId.IDS_ACCOUNT_ADD_CC_TO_ACCOUNT_ERROR_DESC));
            this._finishStep.FinishTextOverride = Shell.LoadString(StringId.IDS_PURCHASE_BUTTON);
            PaymentInstrumentStep paymentInstrumentStep = this.State.PaymentInstrumentStep;
            paymentInstrumentStep.NextTextOverride = Shell.LoadString(StringId.IDS_OK_BUTTON);
            this.AddPage((WizardPage)this.State.ContactInfoStep);
            this.AddPage((WizardPage)this.State.SelectPaymentInstrumentStep);
            this.AddPage((WizardPage)paymentInstrumentStep);
            this.AddPage((WizardPage)this._finishStep);
            this.AddPage((WizardPage)this._errorStep);
        }

        public CreditCard SelectedCreditCard
        {
            get => this._selectedCreditCard;
            private set
            {
                if (this._selectedCreditCard == value)
                    return;
                this._selectedCreditCard = value;
                this.FirePropertyChanged(nameof(SelectedCreditCard));
            }
        }

        protected override void OnAsyncCommitCompleted(bool success)
        {
            base.OnAsyncCommitCompleted(success);
            if (!success)
                return;
            if (this.State.PaymentInstrumentStep.CommittedCreditCard != null)
            {
                this.SelectedCreditCard = this.State.PaymentInstrumentStep.CommittedCreditCard;
                this._finishStep.ClosingMessage = Shell.LoadString(StringId.IDS_PURCHASE_PAYMENT_METHOD_ADDED);
            }
            else
            {
                if (this.State.SelectPaymentInstrumentStep.CommittedCreditCard == null)
                    return;
                this.SelectedCreditCard = this.State.SelectPaymentInstrumentStep.CommittedCreditCard;
                this._finishStep.ClosingMessage = string.Format(Shell.LoadString(StringId.IDS_PURCHASE_SELECTED_PAYMENT_METHOD), (object)this.State.SelectPaymentInstrumentStep.CommittedCreditCard.ToString());
            }
        }
    }
}
