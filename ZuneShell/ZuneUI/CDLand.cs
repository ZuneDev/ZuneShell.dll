// Decompiled with JetBrains decompiler
// Type: ZuneUI.CDLand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class CDLand : LibraryPage
    {
        private string _title = Shell.LoadString(StringId.IDS_NO_CD);
        private CDAlbumCommand _album;
        private PlaylistContentsPanel _burnListPanel;

        public CDLand(CDAlbumCommand album)
        {
            this._album = album;
            this.PivotPreference = album;
            this.IsRootPage = true;
            this.UI = "res://ZuneShellResources!CDView.uix#CDView";
            this.ShowCDIcon = false;
            this.ShowDeviceIcon = false;
            this.ShowPlaylistIcon = false;
            this.ShowNowPlayingBackgroundOnIdle = false;
            this.PlaybackContext = PlaybackContext.Music;
            this._burnListPanel = new PlaylistContentsPanel(this);
        }

        public override void InvokeSettings()
        {
            if (this.Album == Shell.MainFrame.Disc.BurnList)
                Shell.SettingsFrame.Settings.Software.Invoke(SettingCategories.Burn);
            else
                Shell.SettingsFrame.Settings.Software.Invoke(SettingCategories.Rip);
        }

        public override IPageState SaveAndRelease() => new CDLandPageState(this);

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
                this._burnListPanel.Dispose();
            base.OnDispose(disposing);
        }

        public PlaylistContentsPanel BurnListPanel => this._burnListPanel;

        public CDAlbumCommand Album => this._album;
    }
}
