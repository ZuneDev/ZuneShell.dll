// Decompiled with JetBrains decompiler
// Type: ZuneUI.MobileWirelessSyncErrorPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class MobileWirelessSyncErrorPage : WizardErrorPage
    {
        public MobileWirelessSyncErrorPage(Wizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_WIRELESS_MOBILE_WIZARD_TROUBLESHOOT);

        public override string UI => "res://ZuneShellResources!MobileWirelessSync.uix#MobileWirelessSyncErrorPage";

        public void OnCancel() => SQMLog.Log(SQMDataId.WirelessSyncWizardCancelFail, 1);
    }
}
