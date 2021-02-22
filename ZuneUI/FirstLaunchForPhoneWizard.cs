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
            this.AddPage((WizardPage)new FirstConnectPhoneIntroPage((Wizard)this), Shell.LoadString(StringId.IDS_BREADCRUMB_INTRODUCTION));
            this.AddPage((WizardPage)new FirstLaunchForPhoneWelcomePage((Wizard)this), Shell.LoadString(StringId.IDS_BREADCRUMB_SOFTWARE_SETTINGS));
            this.AddPage((WizardPage)new FirstLaunchMonitoredFoldersPage((Wizard)this));
            this.AddPage((WizardPage)new FirstLaunchDownloadFoldersPage((Wizard)this));
            this.AddPage((WizardPage)new FirstLaunchFileTypesPage((Wizard)this));
            this.AddPage((WizardPage)new FirstLaunchPrivacyPage((Wizard)this));
            this.AddPage((WizardPage)new FirstConnectDeviceNamePage((Wizard)this), Shell.LoadString(StringId.IDS_BREADCRUMB_PHONE_NAME));
            this.AddPage((WizardPage)new DeviceUpdateCheckingPage((DeviceUpdateWizard)this), Shell.LoadString(StringId.IDS_BREADCRUMB_PHONE_UPDATE));
            this.AddPage((WizardPage)new DeviceUpdateEULAPage((DeviceUpdateWizard)this));
            this.AddPage((WizardPage)new DeviceUpdateBatteryPowerPage((DeviceUpdateWizard)this));
            this.AddPage((WizardPage)new DeviceUpdateGuestWarningPage((DeviceUpdateWizard)this));
            this.AddPage((WizardPage)new DeviceUpdateSyncingPage((DeviceUpdateWizard)this));
            this.AddPage((WizardPage)new DeviceUpdateDiskSpaceErrorPage((DeviceUpdateWizard)this));
            this.AddPage((WizardPage)new DeviceUpdateProgressPage((DeviceUpdateWizard)this));
            this.AddPage((WizardPage)new DeviceUpdateSummaryPage((DeviceUpdateWizard)this));
            this.AddPage((WizardPage)new DeviceUpdateErrorPage((DeviceUpdateWizard)this));
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
