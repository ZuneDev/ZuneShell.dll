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
            this.AddPage((WizardPage)new DeviceRestoreConfirmationPage(this));
            this.AddPage((WizardPage)new DeviceRestoreBatteryPowerPage(this));
            this.AddPage((WizardPage)new DeviceRestoreSyncingPage(this));
            this.AddPage((WizardPage)new DeviceRestoreProgressPage(this));
            this.AddPage((WizardPage)new DeviceRestoreSummaryPage(this));
            this.AddPage((WizardPage)new DeviceRestoreErrorPage(this));
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
