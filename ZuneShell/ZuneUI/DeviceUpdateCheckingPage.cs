// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceUpdateCheckingPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class DeviceUpdateCheckingPage : DeviceUpdatePage
    {
        internal DeviceUpdateCheckingPage(DeviceUpdateWizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_PHONE_UPDATE_CHECKING_TITLE);

        public override bool IsEnabled => this.Wizard.ActiveDevice.SupportsBrandingType(DeviceBranding.WindowsPhone);

        public override string UI => "res://ZuneShellResources!DeviceUpdate.uix#DeviceUpdateCheckingPage";

        internal override bool OnMovingNext()
        {
            if (SoftwareUpdates.Instance.LastUpdateCheckResult == null || !SoftwareUpdates.Instance.LastUpdateCheckResult.UpdateFound)
                return base.OnMovingNext();
            SoftwareUpdates.Instance.InstallUpdates();
            return false;
        }
    }
}
