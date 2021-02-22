// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceUpdateDiskSpaceErrorPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class DeviceUpdateDiskSpaceErrorPage : DeviceUpdatePage
    {
        internal DeviceUpdateDiskSpaceErrorPage(DeviceUpdateWizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_FIRMWARE_UPDATE_IN_PROGRESS_HEADER);

        public override string UI => "res://ZuneShellResources!DeviceUpdate.uix#DeviceUpdateDiskSpaceErrorPage";

        public override bool IsEnabled => this.Wizard.ActiveDevice.SupportsBrandingType(DeviceBranding.WindowsPhone) && this.Wizard.UIFirmwareUpdater != null;

        internal override void Activate()
        {
            base.Activate();
            if (this.Wizard.ActiveDevice.SkipFutureBackupRequests)
            {
                this.Wizard.UIFirmwareUpdater.UpdateOption = FirmwareUpdateOption.NoBackup;
                this.Wizard.MoveNext();
            }
            else
                this.Wizard.UIFirmwareUpdater.StartCheckForDiskSpace();
        }

        public override bool CanCancel => true;
    }
}
