// Decompiled with JetBrains decompiler
// Type: ZuneUI.AddCreditCardToAccountWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class AddCreditCardToAccountWizard : AccountManagementWizard
    {
        private AccountManagementFinishStep _finishStep;
        private AccountManagementErrorPage _errorStep;

        public AddCreditCardToAccountWizard()
        {
            this.State.ContactInfoStep.LightWeightOnly = true;
            this._finishStep = new AccountManagementFinishStep((Wizard)this, this.State, Shell.LoadString(StringId.IDS_ACCOUNT_FINISHED_DESCRIPTION));
            this._errorStep = new AccountManagementErrorPage((Wizard)this, Shell.LoadString(StringId.IDS_ACCOUNT_ADD_CC_TO_ACCOUNT_ERROR_TITLE), Shell.LoadString(StringId.IDS_ACCOUNT_ADD_CC_TO_ACCOUNT_ERROR_DESC));
            PaymentInstrumentStep paymentInstrumentStep = this.State.PaymentInstrumentStep;
            paymentInstrumentStep.NextTextOverride = Shell.LoadString(StringId.IDS_OK_BUTTON);
            paymentInstrumentStep.DetailDescription = Shell.LoadString(StringId.IDS_BILLING_ADD_CC_TO_ACCOUNT_DESC);
            this.AddPage((WizardPage)this.State.ContactInfoStep);
            this.AddPage((WizardPage)this.State.ListAndAddPaymentInstrumentStep);
            this.AddPage((WizardPage)paymentInstrumentStep);
            this.AddPage((WizardPage)this._finishStep);
            this.AddPage((WizardPage)this._errorStep);
        }

        public bool HideOnComplete
        {
            get => this._finishStep.HideOnComplete;
            set
            {
                if (this._finishStep.HideOnComplete == value)
                    return;
                this._finishStep.HideOnComplete = value;
                this.FirePropertyChanged(nameof(HideOnComplete));
            }
        }

        protected override void OnAsyncCommitCompleted(bool success)
        {
            base.OnAsyncCommitCompleted(success);
            if (!success)
                return;
            this._finishStep.ClosingMessage = Shell.LoadString(StringId.IDS_ACCOUNT_ADD_CC_TO_ACCOUNT_SUCCESS_DESC);
        }
    }
}
