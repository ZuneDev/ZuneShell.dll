// Decompiled with JetBrains decompiler
// Type: ZuneUI.SelectCreditCardFinishStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class SelectCreditCardFinishStep : AccountManagementFinishStep
    {
        public SelectCreditCardFinishStep(
          Wizard owner,
          AccountManagementWizardState state,
          string description)
          : base(owner, state, description, null)
        {
        }

        public override string UI => "res://ZuneShellResources!AccountCreation.uix#SelectCreditCardFinishStep";
    }
}
