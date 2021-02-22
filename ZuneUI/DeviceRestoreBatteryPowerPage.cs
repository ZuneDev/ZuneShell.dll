// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceRestoreBatteryPowerPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class DeviceRestoreBatteryPowerPage : WizardPage
    {
        internal DeviceRestoreBatteryPowerPage(DeviceRestoreWizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_AC_POWER_WARNING_TITLE);

        public override bool IsEnabled
        {
            get
            {
                DeviceRestoreWizard owner = (DeviceRestoreWizard)this._owner;
                return owner.UIFirmwareRestorer == null || owner.UIFirmwareRestorer.IsOnBatteryPower;
            }
        }

        public override string UI => "res://ZuneShellResources!DeviceRestore.uix#DeviceRestoreBatteryPowerPage";
    }
}
