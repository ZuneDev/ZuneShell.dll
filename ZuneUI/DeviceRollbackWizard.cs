// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceRollbackWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class DeviceRollbackWizard : Wizard
    {
        private UIFirmwareUpdater _updater;

        public DeviceRollbackWizard()
        {
            this.AddPage(new DeviceRollbackProgressPage(this));
            this.AddPage(new DeviceRollbackSummaryPage(this));
            this.AddPage(new DeviceRollbackErrorPage(this));
        }

        public UIFirmwareUpdater UIFirmwareUpdater
        {
            get => this._updater;
            set
            {
                if (this._updater == value)
                    return;
                this._updater = value;
                this.FirePropertyChanged(nameof(UIFirmwareUpdater));
            }
        }
    }
}
