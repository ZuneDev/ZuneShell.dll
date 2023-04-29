// Decompiled with JetBrains decompiler
// Type: ZuneUI.LibraryPlaybackTrack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Playlist;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using MicrosoftZunePlayback;
using System;
using System.Threading;

#if OPENZUNE
using StrixMusic.Sdk.AppModels;
using StrixMusic.Sdk.BaseModels;
using StrixMusic.Sdk.MediaPlayback;
using System.Globalization;
using System.Threading.Tasks;
#endif

namespace ZuneUI
{
    [Serializable]
    public class LibraryPlaybackTrack : PlaybackTrack
#if OPENZUNE
        , ITrackBase
#endif
    {
        private const long c_bookmarkInterval = 10000000;
        private int _mediaId;
        private Guid _zuneMediaInstanceId;
        private Guid? _zuneMediaId;
        private MediaType _mediaType;
        private EListType _listType;
        private Streaming _isStreaming;
        private bool _isInCollection;
        private bool? _isVideo;
        private int? _userRating;
        private long _markPlayedAt;
        private bool _hasMarkedPlayed;
        private bool _hasIncrementedPlayCount;
        private bool _hasIncrementedSkipCount;
        private long _lastStoredBookmark;
        private long _duration;
        private static long[] c_podcastVideoLengths = new long[3]
        {
            0L,
            3000000000L,
            6000000000L
        };
        private static long[] c_podcastVideoMarkPlayedAtEOFMinus = new long[3]
        {
            300000000L,
            600000000L,
            1200000000L
        };

        public LibraryPlaybackTrack(int mediaId, MediaType mediaType, ContainerPlayMarker containerPlayMarker)
        {
            this._mediaId = mediaId;
            this._mediaType = mediaType;
            this._listType = PlaylistManager.MediaTypeToListType(mediaType);
            this._containerPlayMarker = containerPlayMarker;
            ThreadPool.QueueUserWorkItem(args =>
            {
                this._isInCollection = PlaylistManager.IsInCollection(this._mediaId, this._mediaType);
                Application.DeferredInvoke(delegate
                {
                    this.RatingChanged.Invoke();
                }, null);
            }, null);
        }

        public int MediaId => this._mediaId;

        public override Guid ZuneMediaInstanceId => this._zuneMediaInstanceId;

        public override MediaType MediaType => this._mediaType;

        public EListType ListType => this._listType;

        public override bool IsVideo
        {
            get
            {
                if (!this._isVideo.HasValue)
                    this._isVideo = new bool?(PlaylistManager.IsVideo(this._mediaId, this._mediaType));
                return this._isVideo.Value;
            }
        }

        public override bool IsMusic => this._mediaType == MediaType.Track;

        public override bool IsHD => this.IsVideo && VideoDefinitionHelper.IsHD((VideoDefinition)PlaylistManager.GetFieldValue(this._mediaId, this._listType, 440, -1));

        public override bool IsStreaming
        {
            get
            {
                if (this._isStreaming == Streaming.Unknown)
                    this._isStreaming = PlaylistManager.GetFieldValue(this._mediaId, this._listType, 177, -1) == 43 ? Streaming.Yes : Streaming.No;
                return this._isStreaming == Streaming.Yes;
            }
        }

        public override Guid ZuneMediaId
        {
            get
            {
                if (!this._zuneMediaId.HasValue)
                    this._zuneMediaId = new Guid?(PlaylistManager.GetFieldValue(this._mediaId, this._listType, 451, Guid.Empty));
                return this._zuneMediaId.Value;
            }
        }

        public override string Title => PlaylistManager.GetFieldValue(_mediaId, _listType, 344, string.Empty);

        public override TimeSpan Duration => PlaylistManager.GetFieldValue(_mediaId, _listType, 151, TimeSpan.Zero);

        public int AlbumLibraryId => PlaylistManager.GetFieldValue(_mediaId, _listType, 11, -1);

        public int AlbumArtistLibraryId
        {
            get
            {
                int albumLibraryId = this.AlbumLibraryId;
                return albumLibraryId >= 0 ? PlaylistManager.GetFieldValue(albumLibraryId, EListType.eAlbumList, 78, -1) : -1;
            }
        }

        public override bool CanRate
        {
            get
            {
                bool flag1 = this._containerPlayMarker != null && this._containerPlayMarker.PlaylistType == PlaylistType.QuickMix;
                bool flag2 = false;
                if (this.MediaType == MediaType.Track)
                    flag2 = flag1 || this.IsInCollection;
                return flag2;
            }
        }

        public override int UserRating
        {
            get
            {
                if (!this.CanRate)
                    return 0;
                if (!this._userRating.HasValue)
                    this._userRating = new int?(PlaylistManager.GetFieldValue(this._mediaId, this._listType, 372, 0));
                return this._userRating.Value;
            }
            set
            {
                if (!this.CanRate)
                    return;
                this._userRating = new int?(value);
                PlaylistManager.SetFieldValue(this._mediaId, this._listType, 372, value);
                this.RatingChanged.Invoke();
            }
        }

        public override bool IsInCollection => this._isInCollection;

        public override bool IsInVisibleCollection => PlaylistManager.IsInVisibleCollection(this._mediaId, this._mediaType);

        private long Bookmark => this._mediaType == MediaType.PodcastEpisode || this._mediaType == MediaType.Video
            ? PlaylistManager.GetFieldValue(_mediaId, _listType, 35, 0L) : 0L;

        private string Album => PlaylistManager.GetFieldValue(_mediaId, _listType, _mediaType == MediaType.PodcastEpisode
            ? FieldAtom.TrackList_PodcastEpisodeAlbumName : FieldAtom.TrackList_AlbumName, string.Empty);

        public string DisplayArtist => PlaylistManager.GetFieldValue(_mediaId, _listType, _mediaType == MediaType.PodcastEpisode
            ? FieldAtom.TrackList_PodcastEpisodeArtistName : FieldAtom.TrackList_ArtistName, string.Empty);

        public override string ServiceContext
        {
            get
            {
                int num = 0;
                if (this.MediaType != MediaType.PodcastEpisode)
                    num = PlaylistManager.GetFieldValue(this._mediaId, this._listType, 358, 0);
                return num != 0 ? num.ToString() : null;
            }
        }

        private int TrackNumber => _mediaType != MediaType.PodcastEpisode ?
            PlaylistManager.GetFieldValue(_mediaId, _listType, FieldAtom.TrackList_TrackNumber, 0) : 0;

        public override HRESULT GetURI(out string uri)
        {
            string uriOut = null;
            HRESULT hresult = HRESULT._S_OK;
            Microsoft.Zune.Service.EContentType eContentType;
            if (PlaylistManager.GetFieldValue(this._mediaId, this._listType, FieldAtom.TrackList_UnkIsVideo, -1) == 43)
            {
                eContentType = Microsoft.Zune.Service.EContentType.Video;
            }
            else
            {
                eContentType = Microsoft.Zune.Service.EContentType.MusicTrack;
                uriOut = PlaylistManager.GetFieldValue(this._mediaId, this._listType, 317, (string)null);
                if (!string.IsNullOrEmpty(uriOut))
                {
                    try
                    {
                        if (new Uri(uriOut).Scheme == Uri.UriSchemeFile)
                        {
                            if (!ZuneLibrary.DoesFileExist(uriOut))
                                uriOut = null;
                        }
                    }
                    catch (UriFormatException ex)
                    {
                    }
                }
            }
            if (string.IsNullOrEmpty(uriOut) && this.ZuneMediaId != Guid.Empty)
                hresult = ZuneApplication.Service2.GetContentUri(this.ZuneMediaId, eContentType, Microsoft.Zune.Service.EContentUriFlags.None, out uriOut, out this._zuneMediaInstanceId);
            uri = uriOut;
            return hresult;
        }

        internal override void OnBeginPlayback(PlayerInterop playbackWrapper)
        {
            base.OnBeginPlayback(playbackWrapper);
            this._hasMarkedPlayed = false;
            this._markPlayedAt = 0L;
            this._hasIncrementedPlayCount = false;
            this._hasIncrementedSkipCount = false;
            this._lastStoredBookmark = 0L;
            this._duration = playbackWrapper.Duration;
            if (this._mediaType == ZuneUI.MediaType.PodcastEpisode || this._mediaType == ZuneUI.MediaType.Video)
            {
                PodcastVideoLengthGroup videoLengthGroup = PodcastVideoLengthGroup.Long;
                while (videoLengthGroup > PodcastVideoLengthGroup.Short && this._duration <= c_podcastVideoLengths[(int)videoLengthGroup])
                    --videoLengthGroup;
                this._markPlayedAt = this._duration - c_podcastVideoMarkPlayedAtEOFMinus[(int)videoLengthGroup];
                if (this._markPlayedAt < 1L)
                    this._markPlayedAt = 1L;
                this._lastStoredBookmark = this.Bookmark;
                if (this._lastStoredBookmark != 0L)
                {
                    playbackWrapper.SeekToAbsolutePosition(this._lastStoredBookmark);
                    if (this._lastStoredBookmark > 200000000L)
                        this._hasIncrementedPlayCount = true;
                }
                if (this._mediaType == ZuneUI.MediaType.PodcastEpisode)
                {
                    if (this.IsVideo)
                        SQMLog.Log(SQMDataId.PodcastVideoEpisodePlayed, 1);
                    else
                        SQMLog.Log(SQMDataId.PodcastAudioEpisodePlayed, 1);
                }
            }
            EMediaTypes MediaType;
            switch (this.MediaType)
            {
                case ZuneUI.MediaType.Track:
                    MediaType = EMediaTypes.eMediaTypeAudio;
                    break;
                case ZuneUI.MediaType.Video:
                    MediaType = EMediaTypes.eMediaTypeVideo;
                    break;
                case ZuneUI.MediaType.PodcastEpisode:
                    MediaType = EMediaTypes.eMediaTypePodcastEpisode;
                    break;
                default:
                    MediaType = EMediaTypes.eMediaTypeInvalid;
                    break;
            }
            Microsoft.Zune.Util.Notification.BroadcastNowPlaying(MediaType, this.Album, this.DisplayArtist, this.Title, this.TrackNumber, this.ZuneMediaId);

#if OPENZUNE
            m_playbackState = PlaybackState.Playing;
            PlaybackStateChanged?.Invoke(this, m_playbackState);
#endif
        }

        internal override void OnPositionChanged(long position)
        {
            bool markPlayed = false;
            bool incrementPlayCount = false;
            if (position <= 0L || position >= this._duration)
                return;
            if (!this._hasMarkedPlayed && this._markPlayedAt > 0L && position >= this._markPlayedAt)
            {
                this._hasMarkedPlayed = true;
                markPlayed = true;
            }
            if (!this._hasIncrementedPlayCount && position >= 200000000L)
            {
                this._hasIncrementedPlayCount = true;
                incrementPlayCount = true;
            }
            long num = 0;
            if (!this._hasMarkedPlayed)
                num = position / 10000000L * 10000000L;
            if (this._lastStoredBookmark != num)
                this._lastStoredBookmark = num;
            if (!markPlayed && !incrementPlayCount)
                return;
            this.UpdatePlayedStates(markPlayed, incrementPlayCount, false);
        }

        internal override void OnSkip()
        {
            if (this._hasIncrementedPlayCount || this._hasIncrementedSkipCount || this.MediaType == MediaType.PodcastEpisode)
                return;
            this._hasIncrementedSkipCount = true;
            this.UpdatePlayedStates(false, false, true);
        }

        internal override void OnEndPlayback(bool endOfMedia)
        {
            base.OnEndPlayback(endOfMedia);
            Microsoft.Zune.Util.Notification.ResetNowPlaying();
            if (endOfMedia)
            {
                this._lastStoredBookmark = 0L;
                if (!this._hasIncrementedPlayCount)
                    this.UpdatePlayedStates(false, true, false);
            }
            this.CommitLastStoredBookmark();
        }

        private void UpdatePlayedStates(
          bool markPlayed,
          bool incrementPlayCount,
          bool incrementSkipCount)
        {
            int mediaId = this.MediaId;
            if (mediaId == -1)
                return;
            EListType listType = this.ListType;
            ContainerPlayMarker containerPlayMarker = this._containerPlayMarker;
            ThreadPool.QueueUserWorkItem(new WaitCallback(UpdatePlayedStatesWorker), new UpdatePlayedStatesTask(markPlayed, incrementPlayCount, incrementSkipCount, mediaId, listType, containerPlayMarker));
            bool flag = this._containerPlayMarker != null && this._containerPlayMarker.PlaylistType == PlaylistType.QuickMix;
            if (incrementPlayCount)
            {
                if (this._mediaType == MediaType.Track)
                    ++Shell.MainFrame.Social.PlayCount;
                if (flag)
                    SQMLog.Log(SQMDataId.QuickMixTrackPlays, 1);
            }
            if (!incrementSkipCount || !flag)
                return;
            if (this.IsInVisibleCollection)
                SQMLog.Log(SQMDataId.QuickMixLocalSkips, 1);
            else
                SQMLog.Log(SQMDataId.QuickMixRemoteSkips, 1);
        }

        private static void UpdatePlayedStatesWorker(object o)
        {
            if (!(o is UpdatePlayedStatesTask playedStatesTask))
                return;
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int[] columnIndexes = new int[7];
            object[] fieldValues = new object[7];
            if (playedStatesTask.IncrementPlayCount)
            {
                columnIndexes[0] = 367;
                fieldValues[0] = 0;
                ZuneLibrary.GetFieldValues(playedStatesTask.MediaID, playedStatesTask.ListType, 1, columnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
                num2 = (int)fieldValues[0];
                columnIndexes[0] = 366;
                fieldValues[0] = 0;
                ZuneLibrary.GetFieldValues(playedStatesTask.MediaID, playedStatesTask.ListType, 1, columnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
                num1 = (int)fieldValues[0];
            }
            if (playedStatesTask.IncrementSkipCount)
            {
                columnIndexes[0] = 374;
                fieldValues[0] = 0;
                ZuneLibrary.GetFieldValues(playedStatesTask.MediaID, playedStatesTask.ListType, 1, columnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
                num3 = (int)fieldValues[0];
            }
            int cValues = 0;
            if (playedStatesTask.MarkPlayed)
            {
                columnIndexes[cValues] = 262;
                fieldValues[cValues] = 1;
                ++cValues;
            }
            if (playedStatesTask.IncrementPlayCount)
            {
                int num4 = num2 + 1;
                columnIndexes[cValues] = 367;
                fieldValues[cValues] = num4;
                int index1 = cValues + 1;
                int num5 = num1 + 1;
                columnIndexes[index1] = 366;
                fieldValues[index1] = num5;
                int index2 = index1 + 1;
                columnIndexes[index2] = 363;
                fieldValues[index2] = DateTime.UtcNow;
                cValues = index2 + 1;
            }
            if (playedStatesTask.IncrementSkipCount)
            {
                int num4 = num3 + 1;
                columnIndexes[cValues] = 374;
                fieldValues[cValues] = num4;
                int index = cValues + 1;
                columnIndexes[index] = 365;
                fieldValues[index] = DateTime.UtcNow;
                cValues = index + 1;
            }
            if (cValues > 0)
                ZuneLibrary.SetFieldValues(playedStatesTask.MediaID, playedStatesTask.ListType, cValues, columnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
            if (!playedStatesTask.IncrementPlayCount || playedStatesTask.ContainerPlayMarker == null)
                return;
            bool flag = false;
            lock (playedStatesTask.ContainerPlayMarker)
            {
                if (!playedStatesTask.ContainerPlayMarker.Marked)
                {
                    playedStatesTask.ContainerPlayMarker.Marked = true;
                    flag = true;
                }
            }
            if (!flag)
                return;
            if (playedStatesTask.ContainerPlayMarker.LibraryId == -1 && playedStatesTask.ListType == EListType.eTrackList)
            {
                fieldValues[0] = -1;
                if (playedStatesTask.ContainerPlayMarker.MediaType == MediaType.Album)
                {
                    columnIndexes[0] = 11;
                    ZuneLibrary.GetFieldValues(playedStatesTask.MediaID, playedStatesTask.ListType, 1, columnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
                    playedStatesTask.ContainerPlayMarker.LibraryId = (int)fieldValues[0];
                }
                else if (playedStatesTask.ContainerPlayMarker.MediaType == MediaType.Genre)
                {
                    columnIndexes[0] = 399;
                    ZuneLibrary.GetFieldValues(playedStatesTask.MediaID, playedStatesTask.ListType, 1, columnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
                    playedStatesTask.ContainerPlayMarker.LibraryId = (int)fieldValues[0];
                }
                else if (playedStatesTask.ContainerPlayMarker.MediaType == MediaType.Artist)
                {
                    columnIndexes[0] = 11;
                    ZuneLibrary.GetFieldValues(playedStatesTask.MediaID, playedStatesTask.ListType, 1, columnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
                    int iMediaId = (int)fieldValues[0];
                    fieldValues[0] = -1;
                    columnIndexes[0] = 78;
                    ZuneLibrary.GetFieldValues(iMediaId, EListType.eAlbumList, 1, columnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
                    playedStatesTask.ContainerPlayMarker.LibraryId = (int)fieldValues[0];
                }
            }
            if (playedStatesTask.ContainerPlayMarker.LibraryId == -1)
                return;
            columnIndexes[0] = 363;
            fieldValues[0] = DateTime.UtcNow;
            EListType listType = PlaylistManager.MediaTypeToListType(playedStatesTask.ContainerPlayMarker.MediaType);
            ZuneLibrary.SetFieldValues(playedStatesTask.ContainerPlayMarker.LibraryId, listType, 1, columnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
        }

        private void CommitLastStoredBookmark()
        {
            if (this.MediaType != MediaType.PodcastEpisode && this.MediaType != MediaType.Video)
                return;
            int[] columnIndexes = new int[1];
            object[] fieldValues = new object[1];
            columnIndexes[0] = 35;
            fieldValues[0] = _lastStoredBookmark;
            ZuneLibrary.SetFieldValues(this.MediaId, this.ListType, 1, columnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
        }

        public void FindInCollection()
        {
            if (this._mediaType != MediaType.Track)
                return;
            MusicLibraryPage.FindInCollection(this.AlbumArtistLibraryId, this.AlbumLibraryId, this.MediaId);
        }

        public void RatingUpdatedExternally(int newRating)
        {
            this._userRating = new int?(newRating);
            this.RatingChanged.Invoke();
        }

        private class UpdatePlayedStatesTask
        {
            public readonly bool MarkPlayed;
            public readonly bool IncrementPlayCount;
            public readonly bool IncrementSkipCount;
            public readonly int MediaID;
            public readonly EListType ListType;
            public readonly ContainerPlayMarker ContainerPlayMarker;

            public UpdatePlayedStatesTask(
              bool markPlayed,
              bool incrementPlayCount,
              bool incrementSkipCount,
              int mediaID,
              EListType listType,
              ContainerPlayMarker containerPlayMarker)
            {
                this.MarkPlayed = markPlayed;
                this.IncrementPlayCount = incrementPlayCount;
                this.IncrementSkipCount = incrementSkipCount;
                this.MediaID = mediaID;
                this.ListType = listType;
                this.ContainerPlayMarker = containerPlayMarker;
            }
        }

        public enum Streaming
        {
            Unknown,
            Yes,
            No,
        }

        private enum PodcastVideoLengthGroup
        {
            Short,
            Medium,
            Long,
        }

#if OPENZUNE
        private PlaybackState m_playbackState = PlaybackState.None;

        public event EventHandler<int?> TrackNumberChanged;
        public event EventHandler<CultureInfo> LanguageChanged;
        public event EventHandler<bool> IsExplicitChanged;
        public event EventHandler<bool> IsPlayArtistCollectionAsyncAvailableChanged;
        public event EventHandler<bool> IsPauseArtistCollectionAsyncAvailableChanged;
        public event EventHandler<int> ArtistItemsCountChanged;
        public event EventHandler<PlaybackState> PlaybackStateChanged;
        public event EventHandler<string> NameChanged;
        public event EventHandler<string> DescriptionChanged;
        public event EventHandler<TimeSpan> DurationChanged;
        public event EventHandler<DateTime?> LastPlayedChanged;
        public event EventHandler<bool> IsChangeNameAsyncAvailableChanged;
        public event EventHandler<bool> IsChangeDescriptionAsyncAvailableChanged;
        public event EventHandler<bool> IsChangeDurationAsyncAvailableChanged;
        public event EventHandler<int> ImagesCountChanged;
        public event EventHandler<int> UrlsCountChanged;
        public event EventHandler<int> GenresCountChanged;

        TrackType ITrackBase.Type => MediaType switch
        {
            MediaType.Podcast or
            MediaType.PodcastEpisode => TrackType.PodcastEpisode,

            MediaType.PlaylistChannel => TrackType.RadioOrStream,

            _ => TrackType.Song
        };

        int? ITrackBase.TrackNumber => TrackNumber;

        int? ITrackBase.DiscNumber => null;

        CultureInfo ITrackBase.Language => CultureInfo.CurrentCulture;

        bool ITrackBase.IsExplicit => false;

        bool ITrackBase.IsChangeAlbumAsyncAvailable => false;
        bool ITrackBase.IsChangeTrackNumberAsyncAvailable => false;
        bool ITrackBase.IsChangeLanguageAsyncAvailable => false;
        bool ITrackBase.IsChangeLyricsAsyncAvailable => false;
        bool ITrackBase.IsChangeIsExplicitAsyncAvailable => false;

        int IArtistCollectionBase.TotalArtistItemsCount => DisplayArtist != null ? 1 : 0;

        bool IArtistCollectionBase.IsPlayArtistCollectionAsyncAvailable => false;
        bool IArtistCollectionBase.IsPauseArtistCollectionAsyncAvailable => false;

        DateTime? IPlayableCollectionItem.AddedAt => null;

        string IPlayableBase.Id => ZuneMediaId.ToString();

        string IPlayableBase.Name => Title;

        string IPlayableBase.Description => null;

        DateTime? IPlayableBase.LastPlayed => null;

        PlaybackState IPlayableBase.PlaybackState => m_playbackState;

        bool IPlayableBase.IsChangeNameAsyncAvailable => false;

        bool IPlayableBase.IsChangeDescriptionAsyncAvailable => false;

        bool IPlayableBase.IsChangeDurationAsyncAvailable => false;

        int IImageCollectionBase.TotalImageCount => 0;

        int IUrlCollectionBase.TotalUrlCount => 1;

        int IGenreCollectionBase.TotalGenreCount => 0;

        Task ITrackBase.ChangeTrackNumberAsync(int? trackNumber, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ITrackBase.ChangeLanguageAsync(CultureInfo language, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ITrackBase.ChangeIsExplicitAsync(bool isExplicit, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IArtistCollectionBase.PlayArtistCollectionAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IArtistCollectionBase.PauseArtistCollectionAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IArtistCollectionBase.RemoveArtistItemAsync(int index, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<bool> IArtistCollectionBase.IsAddArtistItemAvailableAsync(int index, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<bool> IArtistCollectionBase.IsRemoveArtistItemAvailableAsync(int index, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IPlayableBase.ChangeNameAsync(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IPlayableBase.ChangeDescriptionAsync(string description, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IPlayableBase.ChangeDurationAsync(TimeSpan duration, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<bool> IImageCollectionBase.IsAddImageAvailableAsync(int index, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }

        Task<bool> IImageCollectionBase.IsRemoveImageAvailableAsync(int index, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }

        Task IImageCollectionBase.RemoveImageAsync(int index, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IUrlCollectionBase.RemoveUrlAsync(int index, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUrlCollectionBase.IsAddUrlAvailableAsync(int index, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }

        Task<bool> IUrlCollectionBase.IsRemoveUrlAvailableAsync(int index, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }

        Task IGenreCollectionBase.RemoveGenreAsync(int index, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenreCollectionBase.IsAddGenreAvailableAsync(int index, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }

        Task<bool> IGenreCollectionBase.IsRemoveGenreAvailableAsync(int index, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }
#endif

    }
}
