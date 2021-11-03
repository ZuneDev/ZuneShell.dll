// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceRestoreWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class DeviceRestoreWizard : DeviceWizard
    {
        private UIFirmwareRestorer _restorer;

        public DeviceRestoreWizard()
        {
            this.AddPage(new DeviceRestoreConfirmationPage(this));
            this.AddPage(new DeviceRestoreBatteryPowerPage(this));
            this.AddPage(new DeviceRestoreSyncingPage(this));
            this.AddPage(new DeviceRestoreProgressPage(this));
            this.AddPage(new DeviceRestoreSummaryPage(this));
            this.AddPage(new DeviceRestoreErrorPage(this));
        }

        public UIFirmwareRestorer UIFirmwareRestorer
        {
            get => this._restorer;
            set
            {
                if (this._restorer == value)
                    return;
                this._restorer = value;
                this.FirePropertyChanged(nameof(UIFirmwareRestorer));
            }
        }

        public override void Cancel()
        {
            if (this._restorer != null && this._restorer.RestoreInProgress)
                this._restorer.CancelFirmwareRestore();
            base.Cancel();
        }
    }
}
