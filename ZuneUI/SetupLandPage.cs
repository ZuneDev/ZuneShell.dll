// Decompiled with JetBrains decompiler
// Type: ZuneUI.SetupLandPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class SetupLandPage : NoStackPage
    {
        public SetupLandPage()
        {
            this.SetConnectionRule();
            this.ShowComputerIcon = ComputerIconState.Hide;
            this.ShowSearch = false;
            this.ShowSettings = false;
            this.ShowLogo = false;
            this.ShowBackArrow = false;
            this.ShowCDIcon = false;
            this.ShowDeviceIcon = false;
            this.ShowPlaylistIcon = false;
            this.ShowPivots = false;
            this.CanEnterCompactMode = false;
            this.TransportControlStyle = TransportControlStyle.None;
            this.NotificationAreaVisible = false;
            this.TransportControlsVisible = false;
            this.PivotPreference = Shell.SettingsFrame.Wizard.FUE;
        }

        protected override void OnNavigatedToWorker()
        {
            SyncControls.Instance.CurrentDevice.IsLockedAgainstSyncing = true;
            base.OnNavigatedToWorker();
        }

        protected override void OnNavigatedAwayWorker(IPage destination)
        {
            if (!(destination is SetupLandPage))
                SyncControls.Instance.CurrentDevice.IsLockedAgainstSyncing = false;
            base.OnNavigatedAwayWorker(destination);
        }

        protected virtual void SetConnectionRule() => SingletonModelItem<UIDeviceList>.Instance.AllowDeviceConnections = false;

        protected override void OnDispose(bool disposing)
        {
            DeviceManagement.SetupDevice = null;
            SingletonModelItem<UIDeviceList>.Instance.AllowDeviceConnections = true;
            base.OnDispose(disposing);
        }
    }
}
