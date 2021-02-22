// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstConnectForPhoneWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class FirstConnectForPhoneWizard : DeviceUpdateWizard
    {
        private bool _wasPreviouslyNotPaired;

        public FirstConnectForPhoneWizard()
        {
            this._wasPreviouslyNotPaired = ZuneShell.DefaultInstance.Management.DeviceManagement.DevicePartnership == DeviceRelationship.None;
            ZuneShell.DefaultInstance.Management.DeviceManagement.DevicePartnership = DeviceRelationship.Permanent;
        }

        protected override void AddPages()
        {
            this.AddPage((WizardPage)new FirstConnectPhoneIntroPage((Wizard)this), Shell.LoadString(StringId.IDS_BREADCRUMB_INTRODUCTION));
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

        public override void Cancel()
        {
            if (this.CurrentPage is DeviceUpdateProgressPage)
            {
                if (this.UIFirmwareUpdater == null || !this.UIFirmwareUpdater.UpdateInProgress)
                    return;
                this.UIFirmwareUpdater.CancelFirmwareUpdate();
            }
            else
            {
                if (this.RequiredStepsComplete)
                    return;
                base.Cancel();
            }
        }

        public override bool MoveBack()
        {
            if (!base.MoveBack())
                return false;
            if (this.CurrentPage is FirstConnectPage)
                ((FirstConnectPage)this.CurrentPage).IsPageComplete = false;
            return true;
        }

        public bool RequiredStepsComplete => !(this.CurrentPage is FirstConnectPhoneIntroPage) && !(this.CurrentPage is FirstConnectDeviceNamePage) || ((FirstConnectPage)this.CurrentPage).IsPageComplete;

        public override bool CanCommitChanges => this.IsValid && this.RequiredStepsComplete;

        protected override bool OnCommitChanges()
        {
            bool navigateToLandingPage = this._wasPreviouslyNotPaired && !(this.CurrentPage is FirstConnectDeviceNamePage);
            ZuneShell.DefaultInstance.Management.CommitListSave();
            ZuneShell.DefaultInstance.Management.DeviceManagement.SetupComplete(navigateToLandingPage);
            return base.OnCommitChanges();
        }
    }
}
