// Decompiled with JetBrains decompiler
// Type: ZuneUI.ZunePage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ZuneUI
{
    public class ZunePage : Page, IZunePage, IPage, INotifyPropertyChanged
    {
        private string _pageUI;
        private string _pageUIPath;
        private string _backgroundUI;
        private string _bottomBarUI;
        private string _overlayUI;
        private Hashtable _overlayState;
        private IDictionary _navigationArguments;
        private string _navigationCommand;
        private ICommandHandler _commandHandler;
        private ComputerIconState _showComputerIcon;
        private TransportControlStyle _transportStyle;
        private PlaybackContext _playbackContext;
        private Node _pivotPreference;
        private Command _releaseCommand;
        private Command _navigateAwayCommand;
        private Command _navigateToCommand;
        private object _temporaryPageState;
        private BitVector32 _bits;

        public ZunePage()
        {
            this.SetBit(Bits.ShowDeviceIcon, true);
            this.SetBit(Bits.ShowPlaylistIcon, true);
            this.SetBit(Bits.ShowCDIcon, true);
            this.SetBit(Bits.ShowBackArrow, true);
            this.SetBit(Bits.NotificationAreaVisible, true);
            this.SetBit(Bits.TransportControlsVisible, true);
            this.SetBit(Bits.ShowLogo, true);
            this.SetBit(Bits.ShowPivots, true);
            this.SetBit(Bits.ShowSearch, true);
            this.SetBit(Bits.ShowSettings, true);
            this.SetBit(Bits.ShowAppBackground, true);
            this.SetBit(Bits.TakeFocusOnNavigate, true);
            this.SetBit(Bits.ShowNowPlayingBackgroundOnIdle, true);
            this.SetBit(Bits.CanEnterCompactMode, true);
            this.SetBit(Bits.NoStackPage, false);
            this._showComputerIcon = ComputerIconState.Hide;
            this._transportStyle = TransportControlStyle.Music;
            this._playbackContext = PlaybackContext.None;
        }

        public string UI
        {
            get => this._pageUI;
            set
            {
                if (!(this._pageUI != value))
                    return;
                this._pageUI = value;
                this.FirePropertyChanged(nameof(UI));
            }
        }

        public string UIPath
        {
            get => this._pageUIPath;
            set
            {
                if (!(this._pageUIPath != value))
                    return;
                this._pageUIPath = value;
                this.FirePropertyChanged(nameof(UIPath));
            }
        }

        public string BackgroundUI
        {
            get => this._backgroundUI;
            set
            {
                if (!(this._backgroundUI != value))
                    return;
                this._backgroundUI = value;
                this.FirePropertyChanged(nameof(BackgroundUI));
            }
        }

        public string BottomBarUI
        {
            get => this._bottomBarUI;
            set
            {
                if (!(this._bottomBarUI != value))
                    return;
                this._bottomBarUI = value;
                this.FirePropertyChanged(nameof(BottomBarUI));
            }
        }

        public string OverlayUI
        {
            get => this._overlayUI;
            set
            {
                if (!(this._overlayUI != value))
                    return;
                this._overlayUI = value;
                this.FirePropertyChanged(nameof(OverlayUI));
            }
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                this.UI = null;
                this.BackgroundUI = null;
                this.BottomBarUI = null;
                this.OverlayUI = null;
            }
            base.OnDispose(disposing);
        }

        public void SetOverlayState(string overlayKey, object state)
        {
            if (this._overlayState == null)
                this._overlayState = new Hashtable(1);
            this._overlayState[overlayKey] = state;
        }

        public object GetOverlayState(string overlayKey) => this._overlayState != null ? this._overlayState[overlayKey] : null;

        public IDictionary NavigationArguments
        {
            get => this._navigationArguments;
            set => this._navigationArguments = value;
        }

        public string NavigationCommand
        {
            get => this._navigationCommand;
            set => this._navigationCommand = value;
        }

        public ICommandHandler CommandHandler
        {
            get => this._commandHandler;
            set => this._commandHandler = value;
        }

        public bool ShowBackArrow
        {
            get => this.GetBit(Bits.ShowBackArrow);
            set
            {
                if (!this.ChangeBit(Bits.ShowBackArrow, value))
                    return;
                this.FirePropertyChanged(nameof(ShowBackArrow));
            }
        }

        public bool ShowDeviceIcon
        {
            get => this.GetBit(Bits.ShowDeviceIcon);
            set
            {
                if (!this.ChangeBit(Bits.ShowDeviceIcon, value))
                    return;
                this.FirePropertyChanged(nameof(ShowDeviceIcon));
            }
        }

        public bool ShowPlaylistIcon
        {
            get => this.GetBit(Bits.ShowPlaylistIcon);
            set
            {
                if (!this.ChangeBit(Bits.ShowPlaylistIcon, value))
                    return;
                this.FirePropertyChanged(nameof(ShowPlaylistIcon));
            }
        }

        public bool ShowCDIcon
        {
            get => this.GetBit(Bits.ShowCDIcon);
            set
            {
                if (!this.ChangeBit(Bits.ShowCDIcon, value))
                    return;
                this.FirePropertyChanged(nameof(ShowCDIcon));
            }
        }

        public bool ShowNowPlayingX
        {
            get => this.GetBit(Bits.ShowNowPlayingX);
            set
            {
                if (!this.ChangeBit(Bits.ShowNowPlayingX, value))
                    return;
                this.FirePropertyChanged(nameof(ShowNowPlayingX));
            }
        }

        public bool ShowNowPlayingBackgroundOnIdle
        {
            get => this.GetBit(Bits.ShowNowPlayingBackgroundOnIdle);
            set
            {
                if (!this.ChangeBit(Bits.ShowNowPlayingBackgroundOnIdle, value))
                    return;
                this.FirePropertyChanged(nameof(ShowNowPlayingBackgroundOnIdle));
            }
        }

        public bool CanEnterCompactMode
        {
            get => this.GetBit(Bits.CanEnterCompactMode);
            set
            {
                if (!this.ChangeBit(Bits.CanEnterCompactMode, value))
                    return;
                this.FirePropertyChanged(nameof(CanEnterCompactMode));
            }
        }

        public bool NoStackPage
        {
            get => this.GetBit(Bits.NoStackPage);
            set
            {
                if (!this.ChangeBit(Bits.NoStackPage, value))
                    return;
                this.FirePropertyChanged(nameof(NoStackPage));
            }
        }

        public bool NotificationAreaVisible
        {
            get => this.GetBit(Bits.NotificationAreaVisible);
            set
            {
                if (!this.ChangeBit(Bits.NotificationAreaVisible, value))
                    return;
                this.FirePropertyChanged(nameof(NotificationAreaVisible));
            }
        }

        public bool TransportControlsVisible
        {
            get => this.GetBit(Bits.TransportControlsVisible);
            set
            {
                if (!this.ChangeBit(Bits.TransportControlsVisible, value))
                    return;
                this.FirePropertyChanged(nameof(TransportControlsVisible));
            }
        }

        public bool AutoHideToolbars
        {
            get => this.GetBit(Bits.AutoHideToolbars);
            set
            {
                if (!this.ChangeBit(Bits.AutoHideToolbars, value))
                    return;
                this.FirePropertyChanged(nameof(AutoHideToolbars));
            }
        }

        public bool ShowAppBackground
        {
            get => this.GetBit(Bits.ShowAppBackground);
            set
            {
                if (!this.ChangeBit(Bits.ShowAppBackground, value))
                    return;
                this.FirePropertyChanged(nameof(ShowAppBackground));
            }
        }

        public ComputerIconState ShowComputerIcon
        {
            get => this._showComputerIcon;
            set
            {
                if (this._showComputerIcon == value)
                    return;
                this._showComputerIcon = value;
                this.FirePropertyChanged(nameof(ShowComputerIcon));
            }
        }

        public bool ShowingVideoPreview
        {
            get => this.GetBit(Bits.ShowingVideoPreview);
            set
            {
                if (!this.ChangeBit(Bits.ShowingVideoPreview, value))
                    return;
                this.FirePropertyChanged(nameof(ShowingVideoPreview));
            }
        }

        public bool ShowLogo
        {
            get => this.GetBit(Bits.ShowLogo);
            set => this.SetBit(Bits.ShowLogo, value);
        }

        public bool ShowPivots
        {
            get => this.GetBit(Bits.ShowPivots);
            set => this.ChangeBit(Bits.ShowPivots, value);
        }

        public bool ShowSearch
        {
            get => this.GetBit(Bits.ShowSearch);
            set => this.SetBit(Bits.ShowSearch, value);
        }

        public bool ShowSettings
        {
            get => this.GetBit(Bits.ShowSettings);
            set => this.SetBit(Bits.ShowSettings, value);
        }

        public bool TakeFocusOnNavigate
        {
            get => this.GetBit(Bits.TakeFocusOnNavigate);
            set => this.SetBit(Bits.TakeFocusOnNavigate, value);
        }

        public TransportControlStyle TransportControlStyle
        {
            get => this._transportStyle;
            set
            {
                if (this._transportStyle == value)
                    return;
                this._transportStyle = value;
                this.FirePropertyChanged(nameof(TransportControlStyle));
            }
        }

        public PlaybackContext PlaybackContext
        {
            get => this._playbackContext;
            set
            {
                if (this._playbackContext == value)
                    return;
                this._playbackContext = value;
                this.FirePropertyChanged(nameof(PlaybackContext));
            }
        }

        public Node PivotPreference
        {
            get => this._pivotPreference;
            set => this._pivotPreference = value;
        }

        public bool IsRootPage
        {
            get => this.GetBit(Bits.IsRootPage);
            set => this.SetBit(Bits.IsRootPage, value);
        }

        public object TemporaryPageState
        {
            get => this._temporaryPageState;
            set
            {
                if (this._temporaryPageState == value)
                    return;
                this._temporaryPageState = value;
                this.FirePropertyChanged(nameof(TemporaryPageState));
            }
        }

        public virtual void InvokeSettings() => Shell.SettingsFrame.Settings.Invoke();

        public bool ShouldHandleBack
        {
            get => this.GetBit(Bits.ShouldHandleBack);
            set
            {
                if (!this.ChangeBit(Bits.ShouldHandleBack, value))
                    return;
                this.FirePropertyChanged(nameof(ShouldHandleBack));
            }
        }

        public bool ShouldHandleEscape
        {
            get => this.GetBit(Bits.ShouldHandleEscape);
            set
            {
                if (!this.ChangeBit(Bits.ShouldHandleEscape, value))
                    return;
                this.FirePropertyChanged(nameof(ShouldHandleEscape));
            }
        }

        public event EventHandler BackHandled;

        public event EventHandler EscapeHandled;

        public virtual bool HandleBack()
        {
            if (!this.ShouldHandleBack)
                return false;
            if (this.BackHandled != null)
                this.BackHandled(this, EventArgs.Empty);
            this.FirePropertyChanged("BackHandled");
            return true;
        }

        public virtual bool HandleEscape()
        {
            if (!this.ShouldHandleEscape)
                return false;
            if (this.EscapeHandled != null)
                this.EscapeHandled(this, EventArgs.Empty);
            this.FirePropertyChanged("EscapeHandled");
            return true;
        }

        public virtual bool CanNavigateForwardTo(IZunePage destination) => true;

        public event EventHandler Refresh;

        public void RefreshPage()
        {
            if (this.Refresh != null)
                this.Refresh(this, EventArgs.Empty);
            this.FirePropertyChanged("Refresh");
        }

        public override void Release()
        {
            if (this._releaseCommand != null)
                this._releaseCommand.Invoke();
            base.Release();
        }

        protected override void OnNavigatedToWorker()
        {
            if (this._navigateToCommand != null)
                this._navigateToCommand.Invoke();
            base.OnNavigatedToWorker();
            Telemetry.Instance.ReportNavigation(this._pageUIPath, this._navigationArguments);
        }

        protected override void OnNavigatedAwayWorker(IPage destination)
        {
            if (this._navigateAwayCommand != null)
                this._navigateAwayCommand.Invoke();
            this.TemporaryPageState = null;
            base.OnNavigatedAwayWorker(destination);
        }

        public override IPageState SaveAndRelease()
        {
            if (!this.NoStackPage)
                return base.SaveAndRelease();
            this.Release();
            return null;
        }

        public Command ReleaseCommand
        {
            get => this._releaseCommand;
            set
            {
                if (this._releaseCommand == value)
                    return;
                this._releaseCommand = value;
                this.FirePropertyChanged(nameof(ReleaseCommand));
            }
        }

        public Command NavigateToCommand
        {
            get => this._navigateToCommand;
            set
            {
                if (this._navigateToCommand == value)
                    return;
                this._navigateToCommand = value;
                this.FirePropertyChanged(nameof(NavigateToCommand));
            }
        }

        public Command NavigateAway
        {
            get => this._navigateAwayCommand;
            set
            {
                if (this._navigateAwayCommand == value)
                    return;
                this._navigateAwayCommand = value;
                this.FirePropertyChanged("NavigateAwayCommand");
            }
        }

        private bool GetBit(Bits lookupBit) => this._bits[(int)lookupBit];

        private void SetBit(Bits changeBit, bool value) => this._bits[(int)changeBit] = value;

        private bool ChangeBit(Bits bit, bool value)
        {
            if (this._bits[(int)bit] == value)
                return false;
            this._bits[(int)bit] = value;
            return true;
        }

        private enum Bits : uint
        {
            ShowBackArrow = 1,
            ShowDeviceIcon = 2,
            ShowPlaylistIcon = 4,
            ShowCDIcon = 8,
            ShowNowPlayingX = 16, // 0x00000010
            NotificationAreaVisible = 32, // 0x00000020
            AutoHideToolbars = 64, // 0x00000040
            ShowAppBackground = 128, // 0x00000080
            IsRootPage = 256, // 0x00000100
            ShouldHandleBack = 512, // 0x00000200
            ShowLogo = 1024, // 0x00000400
            ShowPivots = 2048, // 0x00000800
            ShowSearch = 4096, // 0x00001000
            ShowSettings = 8192, // 0x00002000
            TakeFocusOnNavigate = 16384, // 0x00004000
            ShowingVideoPreview = 32768, // 0x00008000
            ShowNowPlayingBackgroundOnIdle = 65536, // 0x00010000
            ShouldHandleEscape = 131072, // 0x00020000
            CanEnterCompactMode = 262144, // 0x00040000
            NoStackPage = 524288, // 0x00080000
            TransportControlsVisible = 1048576, // 0x00100000
        }
    }
}
