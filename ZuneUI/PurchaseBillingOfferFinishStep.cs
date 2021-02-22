// Decompiled with JetBrains decompiler
// Type: ZuneUI.PurchaseBillingOfferFinishStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneUI
{
    public class PurchaseBillingOfferFinishStep : AccountManagementFinishStep
    {
        public PurchaseBillingOfferFinishStep(Wizard owner, AccountManagementWizardState state)
          : base(owner, state, Shell.LoadString(StringId.IDS_ACCOUNT_FINISHED_DESCRIPTION))
          => this.RequireSignIn = true;

        protected override bool OnCommitChanges() => this.State.PurchaseBillingOffer(this.State.SelectBillingOfferStep.SelectedBillingOffer, this.State.SelectPaymentInstrumentStep.CommittedCreditCard != null ? State.SelectPaymentInstrumentStep.CommittedCreditCard : State.PaymentInstrumentStep.CommittedCreditCard);
    }
}
