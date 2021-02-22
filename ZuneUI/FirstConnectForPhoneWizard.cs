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
            this.AddPage(new FirstConnectPhoneIntroPage(this), Shell.LoadString(StringId.IDS_BREADCRUMB_INTRODUCTION));
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
