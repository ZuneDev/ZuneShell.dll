// Decompiled with JetBrains decompiler
// Type: ZuneUI.MobileWirelessSyncSummaryPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class MobileWirelessSyncSummaryPage : MobileWirelessSyncWizardPage
    {
        public MobileWirelessSyncSummaryPage(Wizard owner)
          : base(owner)
          => this.Description = Shell.LoadString(StringId.IDS_WIRELESS_MOBILE_WIZARD_CONFIRM_TITLE);

        public override string UI => "res://ZuneShellResources!MobileWirelessSync.uix#MobileWirelessSyncSummaryPage";

        public override void OnCancel() => SQMLog.Log(SQMDataId.WirelessSyncWizardComplete, 1);
    }
}
