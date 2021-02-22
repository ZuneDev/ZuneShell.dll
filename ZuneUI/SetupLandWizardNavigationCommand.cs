// Decompiled with JetBrains decompiler
// Type: ZuneUI.SetupLandWizardNavigationCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    public class SetupLandWizardNavigationCommand : Command
    {
        private SetupLandPage _page;

        public SetupLandWizardNavigationCommand(SetupLandPage page)
          : base(null, null, null)
          => this._page = page;

        protected override void OnInvoked()
        {
            ZuneShell.DefaultInstance.NavigateToPage(_page);
            base.OnInvoked();
        }
    }
}
