// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplaceArtistPageState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class MarketplaceArtistPageState
    {
        private string _pivot;
        private string _album;
        private bool _showSongs;

        public MarketplaceArtistPageState(
          string selectedPivotId,
          string selectedAlbumId,
          bool showSongs)
        {
            this._pivot = selectedPivotId;
            this._album = selectedAlbumId;
            this._showSongs = showSongs;
        }

        public string SelectedPivotId
        {
            get => this._pivot;
            set => this._pivot = value;
        }

        public string SelectedAlbumId
        {
            get => this._album;
            set => this._album = value;
        }

        public bool ShowSongs
        {
            get => this._showSongs;
            set => this._showSongs = value;
        }
    }
}
