// Decompiled with JetBrains decompiler
// Type: ZuneUI.MarketplacePlaybackTrack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using MicrosoftZunePlayback;
using System;

namespace ZuneUI
{
    [Serializable]
    public class MarketplacePlaybackTrack : PlaybackTrack
    {
        private bool _subscriptionPlay;
        private Guid _zuneMediaId;
        private string _title;
        private string _album;
        private string _artist;
        private TimeSpan _duration;
        private int _trackNumber;
        private string _genre;
        private Guid _albumId;
        private string _context;
        private long _incrementPlayCountAfter;
        private bool _hasReportedStreamPlayback;
        private bool _hasReportedStreamPlaySkip;

        public MarketplacePlaybackTrack(
          bool subscriptionPlay,
          Guid zuneMediaId,
          string title,
          TimeSpan duration,
          string album,
          string artist,
          int trackNumber,
          string genre,
          Guid albumId,
          string context)
        {
            this._subscriptionPlay = subscriptionPlay;
            this._zuneMediaId = zuneMediaId;
            this._title = title;
            this._duration = duration;
            this._album = album;
            this._artist = artist;
            this._trackNumber = trackNumber;
            this._genre = genre;
            this._albumId = albumId;
            if (!string.IsNullOrEmpty(context))
                this._context = context;
            this._incrementPlayCountAfter = subscriptionPlay ? 200000000L : 1L;
        }

        public override HRESULT GetURI(out string uri)
        {
            string uriOut = (string)null;
            if (ZuneApplication.Service.InCompleteCollection(this._zuneMediaId, Microsoft.Zune.Service.EContentType.MusicTrack))
                ZuneApplication.Service.GetContentUri(this._zuneMediaId, Microsoft.Zune.Service.EContentType.MusicTrack, Microsoft.Zune.Service.EContentUriFlags.FallbackToPreview, out uriOut, out Guid _);
            if (string.IsNullOrEmpty(uriOut))
                uriOut = "vnd.ms.zunecp://CP/?ContentPartnerKeyName=zune&StreamType=Music&TrackID=" + this._zuneMediaId.ToString();
            uri = uriOut;
            return HRESULT._S_OK;
        }

        internal override void OnBeginPlayback(PlayerInterop playbackWrapper)
        {
            base.OnBeginPlayback(playbackWrapper);
            this._hasReportedStreamPlayback = false;
            this._hasReportedStreamPlaySkip = false;
            Microsoft.Zune.Util.Notification.BroadcastNowPlaying(EMediaTypes.eMediaTypeAudio, this.Album, this.Artist, this.Title, this.TrackNumber, this.ZuneMediaId);
        }

        internal override void OnEndPlayback(bool endOfMedia)
        {
            base.OnEndPlayback(endOfMedia);
            Microsoft.Zune.Util.Notification.ResetNowPlaying();
        }

        internal override void OnPositionChanged(long position)
        {
            if (this._hasReportedStreamPlayback || position < this._incrementPlayCountAfter)
                return;
            this._hasReportedStreamPlayback = true;
            if (this._subscriptionPlay)
            {
                UsageDataService.ReportTrackSubscriptionPlayback(this._zuneMediaId, this._context);
                if (this.MediaType != MediaType.Track)
                    return;
                ++ZuneUI.Shell.MainFrame.Social.PlayCount;
            }
            else
                UsageDataService.ReportTrackPreviewPlayback(this._zuneMediaId, this._context);
        }

        internal override void OnSkip()
        {
            if (this._hasReportedStreamPlayback || this._hasReportedStreamPlaySkip)
                return;
            this._hasReportedStreamPlaySkip = true;
            if (this._subscriptionPlay)
                UsageDataService.ReportTrackSubscriptionSkipPlay(this._zuneMediaId, this._context);
            else
                UsageDataService.ReportTrackPreviewSkipPlay(this._zuneMediaId, this._context);
        }

        public override string Title => this._title;

        public string Album => this._album;

        public string Artist => this._artist;

        public override TimeSpan Duration => this._duration;

        public override Guid ZuneMediaId => this._zuneMediaId;

        public override bool IsMusic => true;

        public override bool IsStreaming => true;

        public int TrackNumber => this._trackNumber;

        public string Genre => this._genre;

        public Guid AlbumId => this._albumId;

        public override MediaType MediaType => MediaType.Track;

        public override string ServiceContext => this._context;
    }
}
