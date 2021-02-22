// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceUpdateGuestWarningPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class DeviceUpdateGuestWarningPage : DeviceUpdatePage
    {
        internal DeviceUpdateGuestWarningPage(DeviceUpdateWizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_PHONE_UPDATE_GUEST_WARNING_TITLE);

        public override string UI => "res://ZuneShellResources!DeviceUpdate.uix#DeviceUpdateGuestWarningPage";

        public override bool IsEnabled => this.Wizard.ActiveDevice.SupportsBrandingType(DeviceBranding.WindowsPhone) && this.Wizard.ActiveDevice.IsGuest && !this.Wizard.IsChainedUpdate;

        public override bool CanCancel => true;
    }
}
