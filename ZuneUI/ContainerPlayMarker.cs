// Decompiled with JetBrains decompiler
// Type: ZuneUI.ContainerPlayMarker
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Playlist;
using System;

namespace ZuneUI
{
    [Serializable]
    public class ContainerPlayMarker
    {
        private int _libraryId = -1;
        private MediaType _mediaType;
        private PlaylistType _playlistType;
        private int _playlistSubType;
        private bool _marked;

        public int LibraryId
        {
            get => this._libraryId;
            set => this._libraryId = value;
        }

        public MediaType MediaType
        {
            get => this._mediaType;
            set => this._mediaType = value;
        }

        public PlaylistType PlaylistType
        {
            get => this._playlistType;
            set => this._playlistType = value;
        }

        public int PlaylistSubType
        {
            get => this._playlistSubType;
            set => this._playlistSubType = value;
        }

        public bool Marked
        {
            get => this._marked;
            set => this._marked = value;
        }
    }
}
