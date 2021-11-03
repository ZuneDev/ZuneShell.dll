// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstLaunchForPhoneWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Configuration;

namespace ZuneUI
{
    public class FirstLaunchForPhoneWizard : DeviceUpdateWizard
    {
        private bool _isSoftwareSettingsEnabled = true;
        private bool _wasPreviouslyNotPaired;

        public FirstLaunchForPhoneWizard()
        {
            this._wasPreviouslyNotPaired = ZuneShell.DefaultInstance.Management.DeviceManagement.DevicePartnership == DeviceRelationship.None;
            ZuneShell.DefaultInstance.Management.DeviceManagement.DevicePartnership = DeviceRelationship.Permanent;
        }

        protected override void AddPages()
        {
            this.AddPage(new FirstConnectPhoneIntroPage(this), Shell.LoadString(StringId.IDS_BREADCRUMB_INTRODUCTION));
            this.AddPage(new FirstLaunchForPhoneWelcomePage(this), Shell.LoadString(StringId.IDS_BREADCRUMB_SOFTWARE_SETTINGS));
            this.AddPage(new FirstLaunchMonitoredFoldersPage(this));
            this.AddPage(new FirstLaunchDownloadFoldersPage(this));
            this.AddPage(new FirstLaunchFileTypesPage(this));
            this.AddPage(new FirstLaunchPrivacyPage(this));
            this.AddPage(new FirstConnectDeviceNamePage(this), Shell.LoadString(StringId.IDS_BREADCRUMB_PHONE_NAME));
            this.AddPage(new DeviceUpdateCheckingPage(this), Shell.LoadString(StringId.IDS_BREADCRUMB_PHONE_UPDATE));
            this.AddPage(new DeviceUpdateEULAPage(this));
            this.AddPage(new DeviceUpdateBatteryPowerPage(this));
            this.AddPage(new DeviceUpdateGuestWarningPage(this));
            this.AddPage(new DeviceUpdateSyncingPage(this));
            this.AddPage(new DeviceUpdateDiskSpaceErrorPage(this));
            this.AddPage(new DeviceUpdateProgressPage(this));
            this.AddPage(new DeviceUpdateSummaryPage(this));
            this.AddPage(new DeviceUpdateErrorPage(this));
        }

        public bool IsSoftwareSettingsEnabled
        {
            get => this._isSoftwareSettingsEnabled;
            set
            {
                if (this._isSoftwareSettingsEnabled == value)
                    return;
                this._isSoftwareSettingsEnabled = value;
                this.FirePropertyChanged(nameof(IsSoftwareSettingsEnabled));
            }
        }

        public override bool CanCommitChanges => this.IsValid && (this.CurrentPage is DeviceUpdateCheckingPage && this.ActiveDevice.UIFirmwareUpdater != null && !this.ActiveDevice.UIFirmwareUpdater.IsUpdateAvailable || (!this.CanAdvancePageIndex || this.CurrentPage is FirstLaunchWelcomePage || this.CurrentPage is FirstConnectDeviceNamePage));

        protected override bool OnCommitChanges()
        {
            bool navigateToLandingPage = this._wasPreviouslyNotPaired && !(this.CurrentPage is FirstConnectDeviceNamePage);
            if (!this.IsSoftwareSettingsEnabled)
                ClientConfiguration.FUE.AcceptedPrivacyStatement = true;
            ZuneShell.DefaultInstance.Management.CommitListSave();
            ZuneShell.DefaultInstance.Management.DeviceManagement.SetupComplete(navigateToLandingPage);
            if (navigateToLandingPage || this.CurrentPage is DeviceUpdatePage || this.CurrentPage is DeviceUpdateErrorPage)
            {
                Fue.Instance.MigrateLegacyConfiguration();
                Fue.Instance.CompleteFUE();
                if (!this.ActiveDevice.AllowChainedUpdates)
                {
                    Shell.MainFrame.Device.Invoke();
                }
                else
                {
                    this.ActiveDevice.NavigateToDeviceSummaryAfterUpdate = true;
                    this.ActiveDevice.UIFirmwareUpdater.StartCheckForUpdates(false, true);
                }
            }
            return base.OnCommitChanges();
        }

        public bool RequiredStepsComplete => this.CurrentPage is DeviceUpdatePage || this.CurrentPage is DeviceUpdateErrorPage;
    }
}
