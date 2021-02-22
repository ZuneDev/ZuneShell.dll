// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceUpdateSummaryPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class DeviceUpdateSummaryPage : DeviceUpdatePage
    {
        internal DeviceUpdateSummaryPage(DeviceUpdateWizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_FIRMWARE_UPDATE_SUCCESSFUL_HEADER);

        public override string UI => "res://ZuneShellResources!DeviceUpdate.uix#DeviceUpdateSummaryPage";
    }
}
