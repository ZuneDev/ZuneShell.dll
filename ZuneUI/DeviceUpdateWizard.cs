// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceUpdateWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class DeviceUpdateWizard : DeviceWizard
    {
        private UIFirmwareUpdater _updater;
        private bool _isUpdateAvailable;
        private bool _isLiveIdSignInSuccess;
        private bool _chainedUpdate;

        public DeviceUpdateWizard() => this.AddPages();

        protected virtual void AddPages()
        {
            this.AddPage(new DeviceUpdateCheckingPage(this));
            this.AddPage(new DeviceUpdateEULAPage(this));
            this.AddPage(new DeviceUpdateBatteryPowerPage(this));
            this.AddPage(new DeviceUpdateGuestWarningPage(this));
            this.AddPage(new DeviceUpdateSyncingPage(this));
            this.AddPage(new DeviceUpdateDiskSpaceErrorPage(this));
            this.AddPage(new DeviceUpdateProgressPage(this));
            this.AddPage(new DeviceUpdateSummaryPage(this));
            this.AddPage(new DeviceUpdateErrorPage(this));
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
                this.IsUpdateAvailable = this._updater.IsUpdateAvailable;
            }
        }

        public bool IsUpdateAvailable
        {
            get => this._isUpdateAvailable;
            set
            {
                if (this._isUpdateAvailable == value)
                    return;
                this._isUpdateAvailable = value;
                this.FirePropertyChanged(nameof(IsUpdateAvailable));
            }
        }

        public bool IsChainedUpdate
        {
            get => this._chainedUpdate;
            set
            {
                if (this._chainedUpdate == value)
                    return;
                this._chainedUpdate = value;
                this.FirePropertyChanged(nameof(IsChainedUpdate));
            }
        }

        public bool IsLiveIdSignInSuccess
        {
            get => this._isLiveIdSignInSuccess;
            set
            {
                this._isLiveIdSignInSuccess = value;
                this.FirePropertyChanged(nameof(IsLiveIdSignInSuccess));
            }
        }

        public override void Cancel()
        {
            if (this._updater != null && this._updater.UpdateInProgress)
                this._updater.CancelFirmwareUpdate();
            if (this.ActiveDevice != null && this.ActiveDevice != UIDeviceList.NullDevice)
            {
                this.ActiveDevice.SequentialUpdatesInstalled = 0;
                this.ActiveDevice.SkipFutureBackupRequests = false;
            }
            if (this.CurrentPage is DeviceUpdatePage && (this.ActiveDevice == UIDeviceList.NullDevice || !this.ActiveDevice.RequiresFirmwareUpdate) || this.CurrentPage is DeviceUpdateErrorPage)
                this.OnCommitChanges();
            else
                base.Cancel();
        }

        public void SignInWithDeviceLiveId(UIDevice device, string password)
        {
            SignIn.Instance.SignOut();
            SignIn.Instance.CancelSignIn();
            if (string.IsNullOrEmpty(device.LiveId) || string.IsNullOrEmpty(password))
            {
                this.IsLiveIdSignInSuccess = false;
            }
            else
            {
                SignIn.Instance.SignInStatusUpdatedEvent += new EventHandler(this.OnSignInStatusUpdatedEvent);
                SignIn.Instance.SignInUser(device.LiveId, password);
            }
        }

        private void OnSignInStatusUpdatedEvent(object sender, EventArgs e)
        {
            if (SignIn.Instance.SigningIn || !SignIn.Instance.SignInError.IsError && !SignIn.Instance.SignedIn)
                return;
            SignIn.Instance.SignInStatusUpdatedEvent -= new EventHandler(this.OnSignInStatusUpdatedEvent);
            this.IsLiveIdSignInSuccess = SignIn.Instance.SignInError.IsSuccess;
        }
    }
}
