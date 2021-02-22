// Decompiled with JetBrains decompiler
// Type: ZuneUI.Deviceland
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class Deviceland : ZunePage, IDeviceContentsPage, IPage
    {
        public Deviceland()
        {
            this.PivotPreference = Shell.MainFrame.Device.Status;
            this.UI = "res://ZuneShellResources!DeviceSummary.uix#DeviceSummary";
            this.UIPath = "Device\\Summary";
            this.IsRootPage = true;
            Deviceland.InitDevicePage((ZunePage)this);
            this.ShowComputerIcon = ComputerIconState.Show;
        }

        public static void InitDevicePage(ZunePage page)
        {
            page.BackgroundUI = "res://ZuneShellResources!DevicelandElements.uix#DeviceBackground";
            page.TransportControlStyle = TransportControlStyle.None;
            page.ShowCDIcon = false;
            page.ShowDeviceIcon = false;
            page.ShowPlaylistIcon = false;
            page.ShowComputerIcon = ComputerIconState.ShowAsDropTarget;
            page.NotificationAreaVisible = false;
            page.TransportControlsVisible = false;
            page.BottomBarUI = "res://ZuneShellResources!DevicelandElements.uix#GasGauge";
            page.ShowAppBackground = false;
            page.ShowNowPlayingBackgroundOnIdle = false;
        }

        public override void InvokeSettings() => Shell.SettingsFrame.Settings.Device.Invoke();

        protected override void OnNavigatedToWorker()
        {
            base.OnNavigatedToWorker();
            Management management = ZuneShell.DefaultInstance.Management;
            if (management.AlertedDeviceCategory == null)
                return;
            management.AlertedDeviceCategory = (Category)null;
            ZuneShell.DefaultInstance.NavigateToPage((ZunePage)new FirstConnectLandPage());
        }

        public override IPageState SaveAndRelease() => (IPageState)new DevicePivotManagingPageState((IDeviceContentsPage)this);

        bool IDeviceContentsPage.ShowDeviceContents => true;
    }
}
