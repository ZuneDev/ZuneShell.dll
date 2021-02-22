// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstLaunchLandPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using System;
using System.IO;
using ZuneXml;

namespace ZuneUI
{
    public class FirstLaunchLandPage : SetupLandPage
    {
        private Timer _deviceArrivalTimer;
        private int _deviceArrivalTimerInterval = 10000;
        private bool _phoneDisconnected;
        private static string _commonAppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ZuneUI.Shell.LoadString(StringId.IDS_APPDATAFOLDERNAME).TrimStart('\\'));
        private static string _customFirstLaunchMovieUri = ClientConfiguration.FUE.FirstLaunchVideo;
        private bool _playbackFailed;
        private FirstLaunchWizardType _wizardType = FirstLaunchWizardType.Undefined;

        public FirstLaunchLandPage()
        {
            this.UI = "res://ZuneShellResources!SetupLand.uix#FirstLaunch";
            if (Fue.Instance.AutoFUE)
                this.BackgroundUI = "res://ZuneShellResources!SetupLand.uix#SetupLandBackground";
            else if (FeatureEnablement.IsFeatureEnabled(Features.eFirstLaunchIntroVideo))
                this.BackgroundUI = "res://ZuneShellResources!SetupLand.uix#FirstLaunchBackground";
            else
                this.BackgroundUI = "res://ZuneShellResources!SetupLand.uix#FirstLaunchNoVideoBackground";
        }

        protected override void SetConnectionRule() => SingletonModelItem<UIDeviceList>.Instance.AllowDeviceConnections = true;

        protected override void OnDispose(bool disposing)
        {
            SingletonModelItem<UIDeviceList>.Instance.DeviceDisconnectedEvent -= new DeviceListEventHandler(this.OnDeviceDisconnected);
            base.OnDispose(disposing);
        }

        public void WaitForPhoneArrival()
        {
            UIDevice connectedPhone = this.FindConnectedPhone();
            if (connectedPhone == UIDeviceList.NullDevice)
            {
                if (this._deviceArrivalTimer == null)
                {
                    this._deviceArrivalTimer = new Timer();
                    this._deviceArrivalTimer.Enabled = true;
                    this._deviceArrivalTimer.Tick += new EventHandler(this.OnDeviceConnectedTimeout);
                    this._deviceArrivalTimer.Interval = this._deviceArrivalTimerInterval;
                }
                SingletonModelItem<UIDeviceList>.Instance.DeviceConnectedEvent += new DeviceListEventHandler(this.OnDeviceConnected);
            }
            else
                this.ProcessPhoneForWizard(connectedPhone);
        }

        private UIDevice FindConnectedPhone()
        {
            UIDevice uiDevice1 = UIDeviceList.NullDevice;
            foreach (UIDevice uiDevice2 in SingletonModelItem<UIDeviceList>.Instance)
            {
                if (uiDevice2.IsConnectedToClientPhysically && uiDevice2.Class == DeviceClass.WindowsPhone)
                {
                    uiDevice1 = uiDevice2;
                    break;
                }
            }
            return uiDevice1;
        }

        private void OnDeviceConnected(object sender, DeviceListEventArgs args)
        {
            if (args.Device.Class != DeviceClass.WindowsPhone)
                return;
            this.ProcessPhoneForWizard(args.Device);
        }

        private void OnDeviceDisconnected(object sender, DeviceListEventArgs args)
        {
            if (SyncControls.Instance.CurrentDeviceOverride != args.Device)
                return;
            this.PhoneDisconnected = true;
        }

        private void OnDeviceConnectedTimeout(object sender, EventArgs args) => this.ProcessPhoneForWizard(UIDeviceList.NullDevice);

        private void ProcessPhoneForWizard(UIDevice device)
        {
            if (this._deviceArrivalTimer != null)
                this._deviceArrivalTimer.Stop();
            SingletonModelItem<UIDeviceList>.Instance.DeviceConnectedEvent -= new DeviceListEventHandler(this.OnDeviceConnected);
            SingletonModelItem<UIDeviceList>.Instance.AllowDeviceConnections = false;
            if (device.Class == DeviceClass.WindowsPhone && device.Relationship == DeviceRelationship.None && !device.RequiresAutoRestore)
            {
                if (device.SupportsOOBECompleted && !device.OOBECompleted)
                {
                    SingletonModelItem<UIDeviceList>.Instance.HideDevice(device);
                    DeviceManagement.ShowDeviceOOBEIncompleteDialog();
                    this.WizardType = FirstLaunchWizardType.Standard;
                }
                else
                {
                    SingletonModelItem<UIDeviceList>.Instance.DeviceDisconnectedEvent += new DeviceListEventHandler(this.OnDeviceDisconnected);
                    this.WizardType = FirstLaunchWizardType.PhoneFirstConnect;
                }
            }
            else
                this.WizardType = FirstLaunchWizardType.Standard;
        }

        public FirstLaunchWizardType WizardType
        {
            get => this._wizardType;
            set
            {
                if (this._wizardType == value)
                    return;
                this._wizardType = value;
                this.FirePropertyChanged(nameof(WizardType));
            }
        }

        public bool PhoneDisconnected
        {
            get => this._phoneDisconnected;
            set
            {
                if (this._phoneDisconnected == value)
                    return;
                this._phoneDisconnected = value;
                this.FirePropertyChanged(nameof(PhoneDisconnected));
            }
        }

        public void InvokePlayback()
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(FirstLaunchLandPage._customFirstLaunchMovieUri))
                str = FirstLaunchLandPage._customFirstLaunchMovieUri;
            else
                this.PlaybackFailed = true;
            if (!string.IsNullOrEmpty(str))
            {
                Uri uri = new Uri(str);
                PlaybackTrack playbackTrack = (PlaybackTrack)null;
                if (uri.IsLoopback)
                {
                    if (this.VideoExists(str))
                    {
                        int mediaId = ZuneApplication.ZuneLibrary.AddMedia(str);
                        if (mediaId != -1)
                            playbackTrack = (PlaybackTrack)new LibraryPlaybackTrack(mediaId, MediaType.Video, (ContainerPlayMarker)null);
                    }
                    else
                        this.PlaybackFailed = true;
                }
                else
                    playbackTrack = (PlaybackTrack)new VideoPlaybackTrack(Guid.Empty, "", (string)null, str, false, true, false, false, false, VideoDefinitionEnum.HD);
                if (playbackTrack == null)
                    return;
                SingletonModelItem<TransportControls>.Instance.PlayItem((object)playbackTrack, PlayNavigationOptions.None);
            }
            else
                this.PlaybackFailed = true;
        }

        public bool PlaybackFailed
        {
            get => this._playbackFailed;
            set
            {
                if (this._playbackFailed == value)
                    return;
                this._playbackFailed = value;
                this.FirePropertyChanged(nameof(PlaybackFailed));
            }
        }

        private bool VideoExists(string uri) => !string.IsNullOrEmpty(uri) && File.Exists(uri);
    }
}
