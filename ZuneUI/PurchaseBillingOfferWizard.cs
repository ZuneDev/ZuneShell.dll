// Decompiled with JetBrains decompiler
// Type: ZuneUI.PurchaseBillingOfferWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneUI
{
    public class PurchaseBillingOfferWizard : AccountManagementWizard
    {
        private AccountManagementFinishStep _finishStep;
        private AccountManagementErrorPage _errorStep;

        public PurchaseBillingOfferWizard(EBillingOfferType offerTypes)
        {
            this.State.ContactInfoStep.LightWeightOnly = true;
            this.State.SelectBillingOfferStep.ShowOffers = offerTypes;
            this.State.ConfirmationStep.OfferType = offerTypes;
            this._finishStep = (AccountManagementFinishStep)new PurchaseBillingOfferFinishStep((Wizard)this, this.State);
            this._errorStep = new AccountManagementErrorPage((Wizard)this, Shell.LoadString(StringId.IDS_ACCOUNT_PURCHASE_ERROR_TITLE), Shell.LoadString(StringId.IDS_ACCOUNT_PURCHASE_ERROR_DESC));
            this.AddPage((WizardPage)this.State.SelectBillingOfferStep);
            this.AddPage((WizardPage)this.State.ContactInfoStep);
            this.AddPage((WizardPage)this.State.SelectPaymentInstrumentStep);
            this.AddPage((WizardPage)this.State.PaymentInstrumentStep);
            this.AddPage((WizardPage)this.State.ConfirmationStep);
            this.AddPage((WizardPage)this._finishStep);
            this.AddPage((WizardPage)this._errorStep);
        }

        protected override void OnAsyncCommitCompleted(bool success)
        {
            base.OnAsyncCommitCompleted(success);
            if (!success)
                return;
            BillingOffer selectedBillingOffer = this.State.SelectBillingOfferStep.SelectedBillingOffer;
            if (selectedBillingOffer != null)
            {
                if (selectedBillingOffer.OfferType == EBillingOfferType.Points)
                    this._finishStep.ClosingMessage = string.Format(Shell.LoadString(StringId.IDS_BILLING_BUY_POINTS_SUCCESS), (object)selectedBillingOffer.Points);
                else
                    this._finishStep.ClosingMessage = string.Format(Shell.LoadString(StringId.IDS_BILLING_BUY_PASS_SUCCESS), (object)selectedBillingOffer.OfferName);
            }
            else
                this._finishStep.ClosingMessage = Shell.LoadString(StringId.IDS_ACCOUNT_PURCHASE_SUCCESS_DESC);
        }
    }
}
