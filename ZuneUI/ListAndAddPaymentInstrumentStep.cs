// Decompiled with JetBrains decompiler
// Type: ZuneUI.ListAndAddPaymentInstrumentStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class ListAndAddPaymentInstrumentStep : SelectPaymentInstrumentStep
    {
        public ListAndAddPaymentInstrumentStep(
          Wizard owner,
          AccountManagementWizardState state,
          bool parent)
          : base(owner, state, parent)
        {
            this.NextTextOverride = Shell.LoadString(StringId.IDS_BILLING_EDIT_CC_ADD_BTN);
        }

        public override string UI => "res://ZuneShellResources!AccountInfo.uix#ListAndAddPaymentInstrumentStep";

        protected override void ResetNextTextOverride()
        {
        }
    }
}
