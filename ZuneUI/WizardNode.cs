// Decompiled with JetBrains decompiler
// Type: ZuneUI.WizardNode
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class WizardNode : Node
    {
        private Wizard _wizard;

        public WizardNode(Experience owner, Wizard wizard, SQMDataId sqmDataID)
          : base(owner, null, sqmDataID)
          => this._wizard = wizard;

        protected override void Execute(Shell shell)
        {
            WizardZunePage wizardZunePage = new WizardZunePage(this, this._wizard);
            shell.NavigateToPage(wizardZunePage);
        }
    }
}
