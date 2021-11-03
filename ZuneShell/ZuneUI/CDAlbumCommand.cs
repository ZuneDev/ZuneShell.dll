// Decompiled with JetBrains decompiler
// Type: ZuneUI.CDAlbumCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Configuration;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class CDAlbumCommand : Node
    {
        private CDAction _autoPlayAction;
        private int _libraryID = -1;
        private bool _insertedDuringSession;
        private string _TOC;
        private bool _isRipping;
        private int _ripCount;
        private CDAlbumTrack[] _trackList;
        private int _trackCount;
        private CDAccess _cdAccess;
        private ZuneLibraryCDDevice _device;
        private BurnableCD _burnCD;

        public CDAlbumCommand(Experience owner, StringId id)
          : base(owner, id, null, SQMDataId.DiscTitleClicks)
        {
        }

        public CDAlbumCommand(
          Experience owner,
          CDAccess cdAccess,
          ZuneLibraryCDDevice device,
          bool insertedDuringSession)
          : base(owner, null, SQMDataId.DiscTitleClicks)
        {
            this._cdAccess = cdAccess;
            this._device = device;
            this._insertedDuringSession = insertedDuringSession;
            this._TOC = device.TOC;
        }

        protected override void OnDispose(bool fDisposing)
        {
            base.OnDispose(fDisposing);
            if (!fDisposing)
                return;
            if (this._burnCD != null)
            {
                this._burnCD.Dispose();
                this._burnCD = null;
            }
            if (this._device == null)
                return;
            this._device.Dispose();
            this._device = null;
        }

        public int RipCount
        {
            get => this._ripCount;
            private set
            {
                if (this._ripCount == value)
                    return;
                this._ripCount = value;
                if (this._ripCount == 0)
                    this.IsRipping = false;
                this.FirePropertyChanged(nameof(RipCount));
            }
        }

        public CDAction AutoPlayAction
        {
            get => this._autoPlayAction;
            internal set
            {
                if (this._autoPlayAction == value)
                    return;
                this._autoPlayAction = value;
                this.FirePropertyChanged(nameof(AutoPlayAction));
            }
        }

        public void ClearAutoPlayAction() => this._autoPlayAction = CDAction.None;

        public int TrackCount
        {
            get => this._trackCount;
            set
            {
                if (this._trackCount == value)
                    return;
                this._trackCount = value;
                if (value == 0)
                {
                    this._trackList = null;
                    this.RipCount = 0;
                }
                else
                {
                    this._trackList = new CDAlbumTrack[value];
                    for (int index = 0; index < value; ++index)
                        this._trackList[index] = new CDAlbumTrack(this, (uint)index);
                }
                this.FirePropertyChanged(nameof(TrackCount));
            }
        }

        public bool InsertedDuringSession => this._insertedDuringSession;

        public bool IsRipping
        {
            get => this._isRipping;
            set
            {
                if (this._isRipping == value)
                    return;
                this._isRipping = value;
                this.FirePropertyChanged(nameof(IsRipping));
            }
        }

        public CDAlbumTrack[] TrackList => this._trackList;

        public ZuneLibraryCDDevice CDDevice => this._device;

        public string TOC => this._TOC;

        public bool IsMediaLoaded => this._device != null && this._device.IsMediaLoaded;

        public bool CanErase => this._device != null && this._device.IsCDRW;

        public bool CanWrite => this._device != null && this._device.IsBlank;

        public BurnableCD BurnCD
        {
            get
            {
                if (this._burnCD == null && this._device != null)
                    this._burnCD = new BurnableCD(this._cdAccess, this._device);
                return this._burnCD;
            }
        }

        public int LibraryID
        {
            get => this._libraryID;
            set
            {
                if (this._libraryID == value || value == -1)
                    return;
                this._libraryID = value;
                this.TrackCount = 0;
                this.FirePropertyChanged(nameof(LibraryID));
            }
        }

        public CDAlbumTrack GetTrack(int trackIndex) => this._trackList == null ? null : this._trackList[trackIndex];

        public void ToggleRipAll()
        {
            int trackCount = this.TrackCount;
            bool flag = this.RipCount < trackCount;
            for (int index = 0; index < trackCount; ++index)
            {
                CDAlbumTrack track = this.TrackList[index];
                if (track.RipState != RipState.InProgress)
                    track.RipTrack = flag;
            }
        }

        internal void AddTrackToRip(CDAlbumTrack track)
        {
            ++this.RipCount;
            if (!this.IsRipping)
                return;
            if (this._cdAccess.Recorder.AsyncAddRecordingRequest(this.CDDevice, track.TrackIndex) == 0)
                track.RipState = RipState.Pending;
            this._cdAccess.StartRip(1);
        }

        internal void RemoveTrackToRip(CDAlbumTrack track)
        {
            --this.RipCount;
            if (!this.IsRipping || track.RipState != RipState.Pending)
                return;
            if (this._cdAccess.Recorder.AsyncRemoveRecordingRequest(this.CDDevice, track.TrackIndex) == 0)
                track.RipState = RipState.NotInLibrary;
            this._cdAccess.StopRip(1);
        }

        public void StartRip()
        {
            if (this.IsRipping || this.RipCount <= 0)
                return;
            ZuneLibraryCDDevice cdDevice = this.CDDevice;
            if (this._cdAccess.Recorder == null || cdDevice == null)
                return;
            int trackCount = 0;
            foreach (CDAlbumTrack track in this.TrackList)
            {
                if (track.RipTrack)
                {
                    ++trackCount;
                    if (this._cdAccess.Recorder.AsyncAddRecordingRequest(cdDevice, track.TrackIndex) == 0)
                        track.RipState = RipState.Pending;
                }
            }
            this._cdAccess.StartRip(trackCount);
            this.IsRipping = true;
            SQMLog.LogToStream(SQMDataId.RipRecordMode, (uint)ClientConfiguration.Recorder.RecordMode);
        }

        public void StopRip()
        {
            int trackCount = 0;
            for (int index = this.TrackList.Length - 1; index >= 0; --index)
            {
                CDAlbumTrack track = this.TrackList[index];
                if (track.RipTrack)
                {
                    ++trackCount;
                    if (this._cdAccess.Recorder.AsyncRemoveRecordingRequest(this.CDDevice, track.TrackIndex) == 0)
                        track.RipState = RipState.Incomplete;
                }
            }
            this._cdAccess.StopRip(trackCount);
            this.IsRipping = false;
        }

        protected override void Execute(Shell shell) => shell.NavigateToPage(new CDLand(this));
    }
}
