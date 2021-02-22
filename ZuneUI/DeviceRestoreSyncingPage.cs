// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceRestoreSyncingPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class DeviceRestoreSyncingPage : DeviceRestorePage
    {
        private bool _isLockedAgainstSyncing;

        internal DeviceRestoreSyncingPage(DeviceRestoreWizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_DEVICE_RESTORE_YOUR_PHONE);

        public override string UI => "res://ZuneShellResources!DeviceRestore.uix#DeviceRestoreSyncingPage";

        internal override void Activate()
        {
            base.Activate();
            this._isLockedAgainstSyncing = this.Wizard.ActiveDevice.IsLockedAgainstSyncing;
        }

        internal override void Deactivate()
        {
            this.Wizard.ActiveDevice.EndSync();
            this.Wizard.ActiveDevice.IsLockedAgainstSyncing = this._isLockedAgainstSyncing;
            base.Deactivate();
        }

        public override bool IsEnabled => this.Wizard.ActiveDevice.SupportsBrandingType(DeviceBranding.WindowsPhone) && !this.Wizard.ActiveDevice.IsGuest && this.Wizard.ActiveDevice.InStandardMode;

        public override bool CanCancel => true;

        public bool BeginSync()
        {
            if (!this.Wizard.ActiveDevice.IsReadyForSync)
                return false;
            this.Wizard.ActiveDevice.IsLockedAgainstSyncing = false;
            this.Wizard.ActiveDevice.BeginSync(true, false);
            return true;
        }
    }
}
