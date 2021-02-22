// Decompiled with JetBrains decompiler
// Type: ZuneUI.CDAlbumTrack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class CDAlbumTrack : ModelItem
    {
        private RipState _ripState = RipState.NotInLibrary;
        private bool _ripTrack;
        private int _percentComplete;
        private uint _trackIndex;
        private CDAlbumCommand _album;
        private int _ripErrorCode;

        internal CDAlbumTrack(CDAlbumCommand album, uint trackIndex)
        {
            this._album = album;
            this._trackIndex = trackIndex;
        }

        public bool RipTrack
        {
            get => this._ripTrack;
            set
            {
                if (this._ripTrack == value)
                    return;
                this._ripTrack = value;
                if (this._ripTrack)
                    this._album.AddTrackToRip(this);
                else
                    this._album.RemoveTrackToRip(this);
                this.FirePropertyChanged(nameof(RipTrack));
            }
        }

        public RipState RipState
        {
            get => this._ripState;
            set
            {
                if (this._ripState == value)
                    return;
                this._ripState = value;
                this.FirePropertyChanged(nameof(RipState));
            }
        }

        public int PercentComplete
        {
            get => this._percentComplete;
            set
            {
                if (this._percentComplete == value)
                    return;
                this._percentComplete = value;
                this.FirePropertyChanged(nameof(PercentComplete));
            }
        }

        public int RipErrorCode
        {
            get => this._ripErrorCode;
            internal set
            {
                if (this._ripErrorCode == value)
                    return;
                this._ripErrorCode = value;
                this.FirePropertyChanged(nameof(RipErrorCode));
            }
        }

        public CDAlbumCommand Album => this._album;

        public uint TrackIndex => this._trackIndex;
    }
}
