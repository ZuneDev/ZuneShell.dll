// Decompiled with JetBrains decompiler
// Type: ZuneUI.PlaybackTrack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZunePlayback;
using System;
using System.Runtime.Serialization;

namespace ZuneUI
{
    [Serializable]
    public abstract class PlaybackTrack
    {
        public const long c_TicksPerSecond = 10000000;
        public const long c_TicksPerMinute = 600000000;
        public const long c_incrementPlayCountAfter = 200000000;
        private static int s_nextPlaybackID;
        protected ContainerPlayMarker _containerPlayMarker;
        [NonSerialized]
        private int _playbackID;
        [NonSerialized]
        private Command _ratingChanged;

        public PlaybackTrack() => this.AcquirePlaybackID();

        public int PlaybackID => this._playbackID;

        public Command RatingChanged
        {
            get
            {
                if (this._ratingChanged == null)
                    this._ratingChanged = new Command();
                return this._ratingChanged;
            }
        }

        public abstract string Title { get; }

        public abstract TimeSpan Duration { get; }

        public abstract Guid ZuneMediaId { get; }

        public virtual Guid ZuneMediaInstanceId => Guid.Empty;

        public virtual bool CanRate => false;

        public virtual int UserRating
        {
            get => 0;
            set
            {
            }
        }

        public virtual bool IsInCollection => false;

        public virtual bool IsInVisibleCollection => false;

        public virtual bool IsMusic => false;

        public virtual bool IsVideo => false;

        public virtual bool IsStreaming => false;

        public virtual bool IsHD => false;

        public virtual MediaType MediaType => MediaType.Undefined;

        public virtual string ServiceContext => (string)null;

        public abstract HRESULT GetURI(out string uri);

        internal virtual void OnBeginPlayback(PlayerInterop playbackWrapper) => SingletonModelItem<TransportControls>.Instance.ClearError(this);

        internal virtual void OnPositionChanged(long position)
        {
        }

        internal virtual void OnSkip()
        {
        }

        internal virtual void OnEndPlayback(bool endOfMedia)
        {
        }

        public override string ToString() => string.Format("TRACK[{0}]: {1}", (object)this._playbackID, (object)this.Title);

        private void AcquirePlaybackID()
        {
            this._playbackID = ++PlaybackTrack.s_nextPlaybackID;
            if (this._playbackID != 0)
                return;
            this._playbackID = ++PlaybackTrack.s_nextPlaybackID;
        }

        [OnDeserialized]
        internal void HandleDeserialization(StreamingContext context) => this.AcquirePlaybackID();
    }
}
