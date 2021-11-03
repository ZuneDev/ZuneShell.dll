// Decompiled with JetBrains decompiler
// Type: ZuneUI.SongMatchData
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class SongMatchData
    {
        private AlbumMetadata _mergedAlbum;
        private TrackMetadata _track;
        private GroupedList _matchOptions;
        private int _originalMergedIndex;
        private int _selectedMatchIndex;

        internal SongMatchData(
          AlbumMetadata mergedAlbum,
          TrackMetadata track,
          GroupedList matchOptions,
          int originalMergedIndex,
          int selectedMatchIndex)
        {
            this._mergedAlbum = mergedAlbum;
            this._track = track;
            this._matchOptions = matchOptions;
            this._originalMergedIndex = originalMergedIndex;
            this._selectedMatchIndex = selectedMatchIndex;
        }

        public AlbumMetadata MergedAlbum => this._mergedAlbum;

        public TrackMetadata Track => this._track;

        public GroupedList MatchOptions => this._matchOptions;

        public int OriginalMergedIndex => this._originalMergedIndex;

        public int SelectedMatchIndex
        {
            get => this._selectedMatchIndex;
            set => this._selectedMatchIndex = value;
        }
    }
}
