// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceUpdateBatteryPowerPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class DeviceUpdateBatteryPowerPage : DeviceUpdatePage
    {
        internal DeviceUpdateBatteryPowerPage(DeviceUpdateWizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_AC_POWER_WARNING_TITLE);

        public override bool IsEnabled
        {
            get
            {
                DeviceUpdateWizard owner = (DeviceUpdateWizard)this._owner;
                return !owner.IsChainedUpdate && (owner.UIFirmwareUpdater == null || owner.UIFirmwareUpdater.IsOnBatteryPower);
            }
        }

        public override string UI => "res://ZuneShellResources!DeviceUpdate.uix#DeviceUpdateBatteryPowerPage";
    }
}
