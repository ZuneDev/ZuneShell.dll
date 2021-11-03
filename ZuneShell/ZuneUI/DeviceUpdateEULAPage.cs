// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceUpdateEULAPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class DeviceUpdateEULAPage : DeviceUpdatePage
    {
        internal DeviceUpdateEULAPage(DeviceUpdateWizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(SyncControls.Instance.CurrentDeviceOverride.RequiresFirmwareUpdate ? StringId.IDS_EULA_DIALOG_TITLE_REQUIRED : StringId.IDS_EULA_DIALOG_TITLE);

        public override string UI => "res://ZuneShellResources!DeviceUpdate.uix#DeviceUpdateEULAPage";

        public override bool IsEnabled
        {
            get
            {
                bool flag = true;
                if (this.Wizard.ActiveDevice.SupportsBrandingType(DeviceBranding.WindowsPhone))
                    flag = !this.Wizard.IsChainedUpdate && (this.Wizard.UIFirmwareUpdater != null && !string.IsNullOrEmpty(this.Wizard.UIFirmwareUpdater.NewFirmwareEULAContent));
                return flag;
            }
        }
    }
}
