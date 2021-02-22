// Decompiled with JetBrains decompiler
// Type: ZuneUI.PlaybackPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public abstract class PlaybackPage : ZunePage
    {
        private bool _showingArtistBio;
        private TransportControlStyle _activeStyle;
        private bool _showMixOnEntry;
        private bool _exitOnPlaybackStopped;
        private int _initialPlaybackID;

        public PlaybackPage()
        {
            this.UI = PlaybackPage.LandUI;
            this.ShowAppBackground = false;
            this.ShowBackArrow = true;
            this.ShowCDIcon = false;
            this.ShowDeviceIcon = false;
            this.ShowComputerIcon = ComputerIconState.Hide;
            this.ShowNowPlayingBackgroundOnIdle = false;
            this.ShowNowPlayingX = true;
            this.ShowPivots = false;
            this.ShowPlaylistIcon = false;
            this.ShowSearch = false;
            this.ShowSettings = false;
            this.ShowLogo = false;
            if (SingletonModelItem<TransportControls>.Instance.CurrentTrack == null)
                return;
            this._initialPlaybackID = SingletonModelItem<TransportControls>.Instance.CurrentTrack.PlaybackID;
        }

        public TransportControlStyle ActiveTransportControlStyle
        {
            get => this._activeStyle;
            set
            {
                if (this._activeStyle == value)
                    return;
                this._activeStyle = value;
                this.FirePropertyChanged(nameof(ActiveTransportControlStyle));
            }
        }

        public bool ShowingArtistBio
        {
            get => this._showingArtistBio;
            set
            {
                if (this._showingArtistBio == value)
                    return;
                this._showingArtistBio = value;
                this.FirePropertyChanged(nameof(ShowingArtistBio));
            }
        }

        public bool ShowMixOnEntry
        {
            get => this._showMixOnEntry;
            set
            {
                if (this._showMixOnEntry == value)
                    return;
                this._showMixOnEntry = value;
                this.FirePropertyChanged(nameof(ShowMixOnEntry));
            }
        }

        public bool ExitOnPlaybackStopped
        {
            get => this._exitOnPlaybackStopped;
            set
            {
                if (this._exitOnPlaybackStopped == value)
                    return;
                this._exitOnPlaybackStopped = value;
                this.FirePropertyChanged(nameof(ExitOnPlaybackStopped));
            }
        }

        public int InitialPlaybackID => this._initialPlaybackID;

        public override bool HandleEscape()
        {
            if (this.ShouldHandleEscape)
                return base.HandleEscape();
            ZuneShell.DefaultInstance.NavigateBack();
            return true;
        }

        public override bool CanNavigateForwardTo(IZunePage destination) => !(destination is CDLand) && !(destination is Deviceland);

        private static string LandUI => "res://ZuneShellResources!NowPlayingLand.uix#NowPlayingLand";
    }
}
