// Decompiled with JetBrains decompiler
// Type: ZuneUI.PlaylistManager
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Playlist;
using Microsoft.Zune.QuickMix;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Subscription;
using Microsoft.Zune.Util;
using MicrosoftZuneInterop;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UIXControls;
using ZuneXml;

namespace ZuneUI
{
    public class PlaylistManager : ModelItem
    {
        private const int DefaultAutoRefreshFreq = 7;
        private const PlaylistLimitType DefaultPlaylistLimitType = PlaylistLimitType.Count;
        private static object s_invalidMediaId = -1;
        private static object s_invalidMediaType = MediaType.Undefined;
        private static char[] s_autoPlaylistSplitChars = new char[1]
        {
      ';'
        };
        private static PlaylistManager _instance;
        private QueryPropertyBag _queryContext;
        private int _defaultPlaylistId = int.MinValue;
        private Microsoft.Zune.Playlist.PlaylistManager _playlistManagerInterop;
        private Notification _notification;

        public static PlaylistManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PlaylistManager();
                return _instance;
            }
        }

        private PlaylistManager()
        {
            this._playlistManagerInterop = Microsoft.Zune.Playlist.PlaylistManager.Instance;
            this._queryContext = new QueryPropertyBag();
            this._queryContext.SetValue("QueryView", 0);
        }

        protected override void OnDispose(bool fDisposing)
        {
            base.OnDispose(fDisposing);
            this._queryContext = null;
        }

        public PlaylistResult CreatePlaylist(string title) => this.CreatePlaylist(title, CreatePlaylistOption.None);

        public PlaylistResult CreateAutoPlaylist(
          string title,
          CreatePlaylistOption options)
        {
            SQMLog.Log(SQMDataId.AutoPlaylistCreations, 1);
            return this.CreatePlaylist(title, CreatePlaylistOption.AutoPlaylist | options);
        }

        public PlaylistResult CreateComplexSyncRulePlaylist(string title) => this.CreatePlaylist(title, CreatePlaylistOption.SyncRule | CreatePlaylistOption.AutoPlaylist);

        internal PlaylistResult CreatePlaylist(string title, bool privatePlaylist) => this.CreatePlaylist(title, privatePlaylist ? CreatePlaylistOption.PrivatePlaylist : CreatePlaylistOption.None);

        public PlaylistResult CreatePlaylist(string title, CreatePlaylistOption options)
        {
            int playlistId;
            HRESULT playlist = this._playlistManagerInterop.CreatePlaylist(title, null, null, options, out playlistId);
            return new PlaylistResult(playlistId, playlist);
        }

        public PlaylistResult SavePlaylistAsStatic(
          int playlistId,
          PlaylistAsyncOperationCompleted completedDelegate)
        {
            HRESULT hr = this._playlistManagerInterop.SavePlaylistAsStatic(playlistId, completedDelegate);
            return new PlaylistResult(playlistId, hr);
        }

        public PlaylistResult GetPlaylistByServiceMediaId(Guid serviceMediaId)
        {
            int playlistId = InvalidPlaylistId;
            HRESULT byServiceMediaId = this._playlistManagerInterop.GetPlaylistByServiceMediaId(serviceMediaId, out playlistId);
            return new PlaylistResult(playlistId, byServiceMediaId);
        }

        public PlaylistResult CreateAndAddToUniquePlaylist(
          string title,
          Guid? serviceMediaId,
          IList items)
        {
            int playlistId;
            HRESULT playlist1 = this._playlistManagerInterop.CreatePlaylist(title, null, serviceMediaId, CreatePlaylistOption.RenameOnConflict, out playlistId);
            if (playlist1.IsError)
                return new PlaylistResult(playlistId, playlist1);
            PlaylistError playlist2 = this.AddToPlaylist(playlistId, items);
            return new PlaylistResult(playlistId, playlist2);
        }

        public PlaylistResult CreateAndAddToUniquePlaylist(string title, IList items) => this.CreateAndAddToUniquePlaylist(title, new Guid?(), items);

        public PlaylistError AddToPlaylist(int playlistId, IList items) => this.AddToPlaylist(playlistId, items, true);

        public PlaylistError AddToPlaylist(
          int playlistId,
          IList items,
          bool rememberAsDefault)
        {
            return this.AddToPlaylist(playlistId, items, rememberAsDefault, true);
        }

        public PlaylistError AddToPlaylist(
          int playlistId,
          IList items,
          bool rememberAsDefault,
          bool notify)
        {
            int capacity = items != null ? items.Count : 0;
            int count = 0;
            if (capacity > 0)
            {
                int startIndex = 0;
                List<PlaybackTrack> playbackTrackList = new List<PlaybackTrack>(capacity);
                bool flag = playlistId == NowPlayingId;
                bool materializeMarketplaceTracks = !flag;
                bool allowVideo = playlistId == CDAccess.Instance.BurnListId;
                bool allowPictures = allowVideo;
                AddItemsToTrackList(items, playbackTrackList, ref startIndex, allowVideo, allowPictures, materializeMarketplaceTracks, null);
                count = playbackTrackList.Count;
                if (count > 0)
                {
                    if (!flag)
                    {
                        int[] mediaIds = new int[count];
                        int[] mediaTypeIds = new int[count];
                        for (int index = 0; index < count; ++index)
                        {
                            if (playbackTrackList[index] is LibraryPlaybackTrack libraryPlaybackTrack)
                            {
                                mediaIds[index] = libraryPlaybackTrack.MediaId;
                                mediaTypeIds[index] = (int)libraryPlaybackTrack.MediaType;
                            }
                        }
                        if (this._playlistManagerInterop.AddMediaToPlaylist(playlistId, mediaIds.Length, mediaIds, mediaTypeIds, -1, null).IsError)
                        {
                            if (playlistId == this.DefaultPlaylistId)
                                this.DefaultPlaylistId = InvalidPlaylistId;
                            return PlaylistError.InvalidId;
                        }
                    }
                    else
                        SingletonModelItem<TransportControls>.Instance.AddToNowPlaying(items);
                }
            }
            if (rememberAsDefault)
                this.DefaultPlaylistId = playlistId;
            if (notify)
                this.NotifyItemsAdded(playlistId, count);
            return PlaylistError.Success;
        }

        public void NotifyItemsAdded(int playlistId, int count)
        {
            string playlistName = GetPlaylistName(playlistId);
            this.ShowNewNotification(count <= 0 ? string.Format(Shell.LoadString(StringId.IDS_PLAYLIST_NEW_DEFAULT), playlistName) : (count != 1 ? string.Format(Shell.LoadString(StringId.IDS_PLAYLIST_ADDED_N_ITEMS), count, playlistName) : string.Format(Shell.LoadString(StringId.IDS_PLAYLIST_ADDED_1_ITEM), playlistName)));
        }

        public void NotifyAutoPlaylistCreated() => this.ShowNewNotification(Shell.LoadString(StringId.IDS_AUTOPLAYLIST_CREATED));

        private void ShowNewNotification(string message)
        {
            if (this._notification != null && !this._notification.IsDisposed)
                NotificationArea.Instance.Remove(this._notification);
            this._notification = new MessageNotification(message, NotificationTask.EditPlaylist, NotificationState.OneShot, 5000);
            NotificationArea.Instance.Add(this._notification);
        }

        public void RemoveFromPlaylist(int playlistId, IList items)
        {
            int count = items.Count;
            int[] playlistContentIds = new int[count];
            for (int index = 0; index < count; ++index)
                playlistContentIds[index] = (int)((DataProviderObject)items[index]).GetProperty("LibraryId");
            this._playlistManagerInterop.RemoveMediaFromPlaylist(playlistId, count, playlistContentIds);
        }

        public void ReorderInPlaylist(int playlistId, IList items, int newIndex)
        {
            int count = items.Count;
            int[] playlistContentIds = new int[count];
            for (int index = 0; index < count; ++index)
                playlistContentIds[index] = (int)((DataProviderObject)items[index]).GetProperty("LibraryId");
            this._playlistManagerInterop.UpdateMediaPositionInPlaylist(playlistId, count, playlistContentIds, newIndex + 1);
        }

        public PlaylistResult RenamePlaylist(int playlistId, string newTitle)
        {
            HRESULT hr = this._playlistManagerInterop.RenamePlaylist(playlistId, newTitle);
            return new PlaylistResult(playlistId, hr);
        }

        public void SetSubType(int playlistId, int subtype)
        {
            SQMLog.LogToStream(SQMDataId.PlaylistSubType, (uint)subtype);
            SetFieldValue(playlistId, EListType.ePlaylistList, 324, subtype);
        }

        public void SetLimitType(int playlistId, int type)
        {
            SQMLog.LogToStream(SQMDataId.PlaylistLimitType, (uint)type);
            SetFieldValue(playlistId, EListType.ePlaylistList, 220, type);
        }

        public void SetLimitValue(int playlistId, int value, int type)
        {
            SQMLog.LogToStream((SQMDataId)(type == 0 ? 205 : 206), (uint)value);
            SetFieldValue(playlistId, EListType.ePlaylistList, 221, value);
        }

        public void SetAutoRefresh(int playlistId, bool enable)
        {
            SQMLog.Log(SQMDataId.PlaylistAutoRefresh, enable ? 1 : 0);
            SetFieldValue(playlistId, EListType.ePlaylistList, 25, enable);
        }

        public void SetAutoRefreshFreq(int playlistId, int freq) => SetFieldValue(playlistId, EListType.ePlaylistList, 26, freq);

        public PlaylistResult RefreshAutoPlaylist(int playlistId)
        {
            HRESULT hr = this._playlistManagerInterop.RefreshAutoPlaylist(playlistId);
            return new PlaylistResult(playlistId, hr);
        }

        public PlaylistResult FreezeAutoPlaylist(int playlistId)
        {
            HRESULT hr = this._playlistManagerInterop.DisableAutomaticRefresh(playlistId);
            return new PlaylistResult(playlistId, hr);
        }

        public PlaylistResult UnfreezeAutoPlaylist(int playlistId)
        {
            HRESULT hr = this._playlistManagerInterop.EnableAutomaticRefresh(playlistId);
            return new PlaylistResult(playlistId, hr);
        }

        public int DefaultPlaylistId
        {
            get => this._defaultPlaylistId;
            private set
            {
                if (this._defaultPlaylistId == value)
                    return;
                this._defaultPlaylistId = value;
                this.FirePropertyChanged(nameof(DefaultPlaylistId));
                this.FirePropertyChanged("DefaultPlaylistName");
            }
        }

        public string DefaultPlaylistName => GetPlaylistName(this._defaultPlaylistId);

        public void ValidateDefaultPlaylist()
        {
            if (this.DefaultPlaylistId < 0)
                return;
            if (string.IsNullOrEmpty(GetFieldValue(this.DefaultPlaylistId, EListType.ePlaylistList, 317, (string)null)))
                this.DefaultPlaylistId = InvalidPlaylistId;
            else
                this.FirePropertyChanged("DefaultPlaylistName");
        }

        public static string GetPlaylistName(int playlistId)
        {
            if (playlistId == NowPlayingId)
                return Shell.LoadString(StringId.IDS_NOW_PLAYING);
            return playlistId >= 0 ? GetFieldValue(playlistId, EListType.ePlaylistList, 344, string.Empty) : null;
        }

        public static bool GetPlaylistAutoRefresh(int playlistId) => playlistId >= 0 && GetFieldValue(playlistId, EListType.ePlaylistList, 25, false);

        public static int GetPlaylistAutoRefreshFreq(int playlistId) => playlistId >= 0 ? GetFieldValue(playlistId, EListType.ePlaylistList, 26, 7) : 7;

        public Choice GetSubTypeChoice(QuickMixSessionManager manager)
        {
            List<Command> commandList = new List<Command>();
            commandList.Add(new QuickMixSubTypeCommand(Shell.LoadString(StringId.IDS_QUICKMIXPLAYLIST_LOCAL_CONTENT_ONLY), EQuickMixType.eQuickMixTypeLocal, manager.QuickMixSession));
            if (FeatureEnablement.IsFeatureEnabled(Features.eQuickMixZmp))
            {
                commandList.Add(new QuickMixSubTypeCommand(Shell.LoadString(StringId.IDS_QUICKMIXPLAYLIST_MIXED_CONTENT), EQuickMixType.eQuickMixTypeMixed, manager.QuickMixSession));
                commandList.Add(new QuickMixSubTypeCommand(Shell.LoadString(StringId.IDS_QUICKMIXPLAYLIST_MP_CONTENT_ONLY), EQuickMixType.eQuickMixTypeRadio, manager.QuickMixSession));
            }
            return new Choice() { Options = commandList };
        }

        public static int GetSubType(int playlistId) => playlistId >= 0 ? GetFieldValue(playlistId, EListType.ePlaylistList, 324, 0) : -1;

        public static int GetLimitType(int playlistId) => playlistId >= 0 ? GetFieldValue(playlistId, EListType.ePlaylistList, 220, 2) : 2;

        public static int GetLimitValue(int playlistId) => playlistId >= 0 ? GetFieldValue(playlistId, EListType.ePlaylistList, 221, 0) : 0;

        public static int GetPlaylistType(int playlistId) => playlistId >= 0 ? GetFieldValue(playlistId, EListType.ePlaylistList, 265, 0) : 0;

        public static bool IsChannel(int playlistId)
        {
            if (playlistId >= 0)
            {
                switch ((PlaylistType)GetPlaylistType(playlistId))
                {
                    case PlaylistType.Channel:
                    case PlaylistType.PersonalChannel:
                        return true;
                }
            }
            return false;
        }

        public static MediaType GetAutoPlaylistSchema(int playlistId)
        {
            EMediaTypes schema;
            return playlistId >= 0 && Microsoft.Zune.Playlist.PlaylistManager.Instance.GetAutoPlaylistSchema(playlistId, out schema).IsSuccess ? (MediaType)schema : MediaType.Undefined;
        }

        public static void AddItemsToTrackList(
          IList items,
          IList tracks,
          ref int startIndex,
          bool allowVideo,
          bool allowPictures,
          bool materializeMarketplaceTracks,
          ContainerPlayMarker containerPlayMarkerOverride)
        {
            int num1 = startIndex;
            if (items == null || tracks == null)
                return;
            int count = items.Count;
            if (count <= 0)
                return;
            MediaType mediaType1 = MediaType.Undefined;
            if (items[0] is LibraryDataProviderItemBase providerItemBaseA)
            {
                if (providerItemBaseA.TypeName == "Artist")
                    mediaType1 = MediaType.Artist;
                else if (providerItemBaseA.TypeName == "Album")
                    mediaType1 = MediaType.Album;
                else if (providerItemBaseA.TypeName == "Genre")
                    mediaType1 = MediaType.Genre;
            }
            if (mediaType1 == MediaType.Artist || mediaType1 == MediaType.Genre || mediaType1 == MediaType.Album)
            {
                IList list = new ArrayList(count);
                foreach (LibraryDataProviderItemBase providerItemBaseB in items)
                    list.Add((int)providerItemBaseB.GetProperty("LibraryId"));
                bool singleAlbum = list.Count == 1;
                if ((mediaType1 == MediaType.Artist || mediaType1 == MediaType.Genre) && list.Count == 1)
                {
                    ZuneQueryList zuneQueryList = mediaType1 != MediaType.Artist ? ZuneApplication.ZuneLibrary.GetAlbumsByGenres(list, null) : ZuneApplication.ZuneLibrary.GetAlbumsByArtists(list, null);
                    int num2 = (int)zuneQueryList.AddRef();
                    singleAlbum = zuneQueryList.Count == 1;
                    int num3 = (int)zuneQueryList.Release();
                    zuneQueryList.Dispose();
                }
                string sort = null;
                if (ZuneShell.DefaultInstance.CurrentPage is MusicLibraryPage currentPage)
                    sort = currentPage.GetSort(singleAlbum, mediaType1);
                if (sort == null)
                    sort = "+WM/TrackNumber";
                ZuneQueryList zuneQueryList1 = null;
                switch (mediaType1)
                {
                    case MediaType.Album:
                        zuneQueryList1 = ZuneApplication.ZuneLibrary.GetTracksByAlbums(list, sort);
                        break;
                    case MediaType.Genre:
                        zuneQueryList1 = ZuneApplication.ZuneLibrary.GetTracksByGenres(list, sort);
                        break;
                    case MediaType.Artist:
                        zuneQueryList1 = ZuneApplication.ZuneLibrary.GetTracksByArtists(list, sort);
                        break;
                }
                int num4 = (int)zuneQueryList1.AddRef();
                ArrayList uniqueIds = zuneQueryList1.GetUniqueIds();
                int num5 = uniqueIds != null ? uniqueIds.Count : 0;
                for (int index = 0; index < num5; ++index)
                {
                    ContainerPlayMarker containerPlayMarker;
                    if (containerPlayMarkerOverride != null)
                    {
                        containerPlayMarker = containerPlayMarkerOverride;
                    }
                    else
                    {
                        containerPlayMarker = new ContainerPlayMarker();
                        containerPlayMarker.MediaType = mediaType1;
                    }
                    LibraryPlaybackTrack libraryPlaybackTrack = new LibraryPlaybackTrack((int)uniqueIds[index], MediaType.Track, containerPlayMarker);
                    tracks.Add(libraryPlaybackTrack);
                }
                int num6 = (int)zuneQueryList1.Release();
                zuneQueryList1.Dispose();
            }
            else
            {
                bool flag1 = CanFastAddList(items);
                ArrayList arrayList = null;
                int num2 = count;
                bool flag2 = false;
                if (flag1)
                {
                    arrayList = GetUniqueIdsFromList(items);
                    if (arrayList != null)
                        num2 = arrayList.Count;
                    else
                        flag1 = false;
                }
                for (int index = 0; index < num2; ++index)
                {
                    if (index == num1)
                        startIndex = tracks.Count;
                    if (flag1)
                    {
                        AddLibraryDataProviderItemToTrackList((int)arrayList[index], MediaType.Track, tracks, allowVideo, allowPictures, containerPlayMarkerOverride);
                    }
                    else
                    {
                        object obj = items[index];
                        switch (obj)
                        {
                            case LibraryDataProviderItemBase _:
                                int mediaId = -1;
                                EMediaTypes mediaType2 = EMediaTypes.eMediaTypeInvalid;
                                (obj as LibraryDataProviderItemBase).GetMediaIdAndType(out mediaId, out mediaType2);
                                AddLibraryDataProviderItemToTrackList(mediaId, (MediaType)mediaType2, tracks, allowVideo, allowPictures, containerPlayMarkerOverride);
                                continue;
                            case SubscriptionDataProviderItem _:
                                AddSubscriptionDataProviderItemToTrackList((SubscriptionDataProviderItem)obj, tracks, allowVideo, allowPictures);
                                continue;
                            case DataProviderObject _:
                                bool blockedExplicitContent = false;
                                AddDataProviderObjectToTrackList((DataProviderObject)obj, tracks, materializeMarketplaceTracks, containerPlayMarkerOverride, out blockedExplicitContent);
                                flag2 |= blockedExplicitContent;
                                continue;
                            case FileEntry _:
                                AddFileEntryToTrackList((FileEntry)obj, tracks, allowVideo, allowPictures, materializeMarketplaceTracks, containerPlayMarkerOverride);
                                continue;
                            case LibraryPlaybackTrack _:
                            case VideoPlaybackTrack _:
                                AddLibraryOrVideoPlaybackTrackToTrackList(obj, tracks);
                                continue;
                            case MarketplacePlaybackTrack _:
                                AddMarketplacePlaybackTrackToTrackList((MarketplacePlaybackTrack)obj, tracks, materializeMarketplaceTracks);
                                continue;
                            case QuickMixItem _:
                                AddQuickMixItemToTrackList((QuickMixItem)obj, tracks, containerPlayMarkerOverride);
                                continue;
                            default:
                                continue;
                        }
                    }
                }
                if (!flag2)
                    return;
                if (SignIn.Instance.SignedIn)
                    MessageBox.Show(Shell.LoadString(StringId.IDS_ExplicitErrorHeading), Shell.LoadString(StringId.IDS_ExplicitNeedAdult), null);
                else
                    MessageBox.Show(Shell.LoadString(StringId.IDS_ExplicitErrorHeading), Shell.LoadString(StringId.IDS_ExplicitMustLogin), null);
            }
        }

        private static bool CanFastAddList(IList list)
        {
            bool flag = false;
            if (list != null && list is LibraryVirtualList && (list[0] is LibraryDataProviderItemBase && ((DataProviderObject)list[0]).TypeName == "Track"))
                flag = true;
            return flag;
        }

        private static ArrayList GetUniqueIdsFromList(IList list)
        {
            ArrayList arrayList = null;
            if (list is LibraryVirtualList libraryVirtualList)
                arrayList = libraryVirtualList.GetUniqueIds();
            return arrayList;
        }

        private static void AddLibraryDataProviderItemToTrackList(
          int libraryId,
          MediaType mediaType,
          IList tracks,
          bool allowVideo,
          bool allowPictures,
          ContainerPlayMarker containerPlayMarkerOverride)
        {
            if (libraryId == -1)
                return;
            switch (mediaType)
            {
                case MediaType.Track:
                case MediaType.Video:
                case MediaType.Photo:
                case MediaType.PodcastEpisode:
                    if (!CanEnqueue(libraryId, mediaType, allowVideo, allowPictures))
                        break;
                    ContainerPlayMarker containerPlayMarker1 = null;
                    if (mediaType == MediaType.Track)
                    {
                        if (containerPlayMarkerOverride != null)
                        {
                            containerPlayMarker1 = containerPlayMarkerOverride;
                        }
                        else
                        {
                            containerPlayMarker1 = new ContainerPlayMarker();
                            containerPlayMarker1.MediaType = MediaType.Album;
                            containerPlayMarker1.LibraryId = -1;
                        }
                    }
                    tracks.Add(new LibraryPlaybackTrack(libraryId, mediaType, containerPlayMarker1));
                    break;
                case MediaType.Playlist:
                    ZuneQueryList tracksByPlaylist = ZuneApplication.ZuneLibrary.GetTracksByPlaylist(0, libraryId, EQuerySortType.eQuerySortOrderAscending, 437U);
                    int num1 = (int)tracksByPlaylist.AddRef();
                    ContainerPlayMarker containerPlayMarker2 = new ContainerPlayMarker();
                    containerPlayMarker2.LibraryId = libraryId;
                    containerPlayMarker2.MediaType = MediaType.Playlist;
                    containerPlayMarker2.PlaylistType = (PlaylistType)GetPlaylistType(libraryId);
                    containerPlayMarker2.PlaylistSubType = GetSubType(libraryId);
                    int count = tracksByPlaylist.Count;
                    for (uint index = 0; index < count; ++index)
                    {
                        int fieldValue1 = (int)tracksByPlaylist.GetFieldValue(index, typeof(int), 233U, s_invalidMediaId);
                        int fieldValue2 = (int)tracksByPlaylist.GetFieldValue(index, typeof(int), 234U, s_invalidMediaType);
                        if (fieldValue1 != -1)
                            tracks.Add(new LibraryPlaybackTrack(fieldValue1, (MediaType)fieldValue2, containerPlayMarker2));
                    }
                    int num2 = (int)tracksByPlaylist.Release();
                    tracksByPlaylist.Dispose();
                    break;
            }
        }

        private static void AddSubscriptionDataProviderItemToTrackList(
          SubscriptionDataProviderItem podcastEpisode,
          IList tracks,
          bool allowVideo,
          bool allowPictures)
        {
            int property = (int)podcastEpisode.GetProperty("LibraryId");
            if (!CanEnqueue(property, MediaType.PodcastEpisode, allowVideo, allowPictures))
                return;
            tracks.Add(new LibraryPlaybackTrack(property, MediaType.PodcastEpisode, null));
        }

        private static void AddDataProviderObjectToTrackList(
          DataProviderObject dpItem,
          IList tracks,
          bool materializeMarketplaceTracks,
          ContainerPlayMarker containerPlayMarkerOverride,
          out bool blockedExplicitContent)
        {
            blockedExplicitContent = false;
            if (dpItem == null)
                return;
            if (dpItem is Track)
            {
                Track track = (Track)dpItem;
                int dbMediaId = -1;
                if (track.IsParentallyBlocked && !track.InCollection)
                {
                    blockedExplicitContent = true;
                }
                else
                {
                    if (!ZuneApplication.Service.InCompleteCollection(track.Id, Microsoft.Zune.Service.EContentType.MusicTrack, out dbMediaId, out bool _) && materializeMarketplaceTracks && (track.IsDownloading || track.CanDownload || track.CanPurchase))
                        dbMediaId = ZuneApplication.ZuneLibrary.AddTrack(track.Id, track.AlbumId, track.TrackNumber, track.Title, track.Duration, track.AlbumTitle, track.Artist, track.PrimaryGenre.Title);
                    if (dbMediaId >= 0)
                    {
                        tracks.Add(new LibraryPlaybackTrack(dbMediaId, MediaType.Track, null));
                    }
                    else
                    {
                        if (!track.CanPlay)
                            return;
                        tracks.Add(new MarketplacePlaybackTrack(track.CanSubscriptionPlay, track.Id, track.Title, track.Duration, track.AlbumTitle, track.Artist, track.TrackNumber, track.PrimaryGenre.Title, track.AlbumId, track.ReferrerContext));
                    }
                }
            }
            else if (dpItem is Video)
            {
                Video video = (Video)dpItem;
                if (video.IsParentallyBlocked && !video.InCollection)
                    blockedExplicitContent = true;
                else
                    tracks.Add(new VideoPlaybackTrack(video.Id, video.Title, null, null, false, false, false, true, false, VideoDefinitionEnum.None));
            }
            else
            {
                if (!(dpItem.TypeName == "RadioStation"))
                    return;
                tracks.Add(new StreamingRadioPlaybackTrack((string)dpItem.GetProperty("SourceURL"), (string)dpItem.GetProperty("Title"), MediaType.Track));
            }
        }

        private static void AddFileEntryToTrackList(
          FileEntry file,
          IList tracks,
          bool allowVideo,
          bool allowPictures,
          bool materializeMarketplaceTracks,
          ContainerPlayMarker containerPlayMarkerOverride)
        {
            if (file == null)
                return;
            int dbMediaId;
            bool fFileAlreadyExists;
            bool fTimedout;
            bool flag1 = AddTransientMediaTask.AddTransientMediaWithTimeout(file.Path, (MediaType)file.MediaType, TimeSpan.FromSeconds(ClientConfiguration.GeneralSettings.AccessMediaHangTimeoutSec), out dbMediaId, out fFileAlreadyExists, out fTimedout);
            if (fTimedout)
                MessageBox.Show(Shell.LoadString(StringId.IDS_GENERIC_ERROR), Shell.LoadString(StringId.IDS_PLAYLIST_MEDIA_NOTACCESSIBLE), null);
            else if (flag1 && !fFileAlreadyExists)
            {
                if (!CanEnqueue(dbMediaId, (MediaType)file.MediaType, allowVideo, allowPictures))
                    return;
                tracks.Add(new LibraryPlaybackTrack(dbMediaId, (MediaType)file.MediaType, null));
            }
            else
            {
                bool flag2 = false;
                if (file.MediaType == EMediaTypes.eMediaTypeVideo && fFileAlreadyExists)
                {
                    MediaType type = MediaType.Undefined;
                    Guid zuneMediaId = Guid.Empty;
                    string title = string.Empty;
                    bool isHD = false;
                    GetVideoValues(dbMediaId, out type, out zuneMediaId, out title, out isHD);
                    if (type == MediaType.VideoMBR)
                    {
                        tracks.Add(new VideoPlaybackTrack(zuneMediaId, title, null, null, false, true, false, false, false, isHD ? VideoDefinitionEnum.HD : VideoDefinitionEnum.SD));
                        flag2 = true;
                    }
                }
                if (flag2)
                    return;
                string str = file.Path;
                try
                {
                    str = Path.GetFileName(str);
                }
                catch (ArgumentException ex)
                {
                }
                tracks.Add(new StreamingPlaybackTrack(file.Path, str, (MediaType)file.MediaType));
            }
        }

        private static void AddLibraryOrVideoPlaybackTrackToTrackList(object item, IList tracks) => tracks.Add(item);

        private static void AddMarketplacePlaybackTrackToTrackList(
          MarketplacePlaybackTrack marketplacePlaybackTrack,
          IList tracks,
          bool materializeMarketplaceTracks)
        {
            if (marketplacePlaybackTrack == null)
                return;
            int dbMediaId;
            if (!ZuneApplication.Service.InCompleteCollection(marketplacePlaybackTrack.ZuneMediaId, Microsoft.Zune.Service.EContentType.MusicTrack, out dbMediaId, out bool _))
                dbMediaId = ZuneApplication.ZuneLibrary.AddTrack(marketplacePlaybackTrack.ZuneMediaId, marketplacePlaybackTrack.AlbumId, marketplacePlaybackTrack.TrackNumber, marketplacePlaybackTrack.Title, marketplacePlaybackTrack.Duration, marketplacePlaybackTrack.Album, marketplacePlaybackTrack.Artist, marketplacePlaybackTrack.Genre);
            if (dbMediaId < 0)
                return;
            tracks.Add(new LibraryPlaybackTrack(dbMediaId, MediaType.Track, null));
        }

        private static void AddQuickMixItemToTrackList(
          QuickMixItem quickMixItem,
          IList tracks,
          ContainerPlayMarker containerPlayMarkerOverride)
        {
            if (quickMixItem == null)
                return;
            tracks.Add(new LibraryPlaybackTrack(quickMixItem.MediaId, MediaType.Track, containerPlayMarkerOverride));
        }

        internal QueryPropertyBag QueryContext
        {
            get
            {
                if (this._queryContext != null)
                    this._queryContext.SetValue("UserId", SignIn.Instance.LastSignedInUserId);
                return this._queryContext;
            }
        }

        public static T GetFieldValue<T>(int mediaId, EListType listType, int atom, T defaultValue)
        {
            int[] columnIndexes = new int[1] { atom };
            object[] fieldValues = new object[1]
            {
         defaultValue
            };
            ZuneLibrary.GetFieldValues(mediaId, listType, 1, columnIndexes, fieldValues, Instance.QueryContext);
            return (T)fieldValues[0];
        }

        public static void SetFieldValue<T>(int mediaId, EListType listType, int atom, T value)
        {
            int[] columnIndexes = new int[1] { atom };
            object[] fieldValues = new object[1] { value };
            ZuneLibrary.SetFieldValues(mediaId, listType, 1, columnIndexes, fieldValues, Instance.QueryContext);
        }

        public static bool IsVideo(int mediaId, MediaType mediaType)
        {
            switch (mediaType)
            {
                case MediaType.Video:
                    return true;
                case MediaType.PodcastEpisode:
                    if (GetFieldValue(mediaId, EListType.ePodcastEpisodeList, 161, MediaType.Undefined) == MediaType.Video)
                        return true;
                    break;
            }
            return false;
        }

        public static void GetVideoValues(
          int dbMediaId,
          out MediaType type,
          out Guid zuneMediaId,
          out string title,
          out bool isHD)
        {
            int[] columnIndexes = new int[4] { 177, 451, 344, 440 };
            object[] fieldValues = new object[4]
            {
         MediaType.Undefined,
         Guid.Empty,
         string.Empty,
         VideoDefinition.Unknown
            };
            ZuneLibrary.GetFieldValues(dbMediaId, EListType.eVideoList, columnIndexes.Length, columnIndexes, fieldValues, Instance.QueryContext);
            type = (MediaType)fieldValues[0];
            zuneMediaId = (Guid)fieldValues[1];
            title = (string)fieldValues[2];
            isHD = VideoDefinitionHelper.IsHD((VideoDefinition)fieldValues[3]);
        }

        public static bool CanEnqueue(
          int mediaId,
          MediaType mediaType,
          bool allowVideo,
          bool allowPictures)
        {
            switch (mediaType)
            {
                case MediaType.Photo:
                    return allowPictures;
                case MediaType.PodcastEpisode:
                    if (GetFieldValue(mediaId, EListType.ePodcastEpisodeList, 145, EItemDownloadState.eDownloadStateNone) != EItemDownloadState.eDownloadStateDownloaded)
                        return false;
                    break;
            }
            return allowVideo || !IsVideo(mediaId, mediaType);
        }

        public static bool IsInCollection(int mediaId, MediaType mediaType)
        {
            EListType listType = MediaTypeToListType(mediaType);
            if (listType == EListType.eListTypeCount)
                return false;
            string fieldValue = GetFieldValue(mediaId, listType, 317, (string)null);
            bool flag = false;
            if (!string.IsNullOrEmpty(fieldValue) && !fieldValue.StartsWith("zunecd://"))
                flag = true;
            return flag;
        }

        public static bool IsInVisibleCollection(int mediaId, MediaType mediaType)
        {
            bool flag = false;
            if (mediaId != -1)
            {
                EListType listType = MediaTypeToListType(mediaType);
                if (listType != EListType.eListTypeCount)
                    flag = GetFieldValue(mediaId, listType, 204, false);
            }
            return flag;
        }

        public static EListType MediaTypeToListType(MediaType mediaType)
        {
            switch (mediaType)
            {
                case MediaType.Track:
                    return EListType.eTrackList;
                case MediaType.Video:
                    return EListType.eVideoList;
                case MediaType.Photo:
                    return EListType.ePhotoList;
                case MediaType.Playlist:
                    return EListType.ePlaylistList;
                case MediaType.Album:
                    return EListType.eAlbumList;
                case MediaType.PodcastEpisode:
                    return EListType.ePodcastEpisodeList;
                case MediaType.Podcast:
                    return EListType.ePodcastList;
                case MediaType.Genre:
                    return EListType.eGenreList;
                case MediaType.Artist:
                    return EListType.eArtistList;
                default:
                    return EListType.eListTypeCount;
            }
        }

        public static int GetPlaylistId(int contentItemId) => GetFieldValue(contentItemId, EListType.ePlaylistContentList, 263, -1);

        public static List<string> SplitAutoPlaylistValue(string value)
        {
            List<string> stringList = null;
            if (value != null)
            {
                if (value.IndexOf(';') >= 0)
                {
                    string[] strArray = value.Split(s_autoPlaylistSplitChars, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str1 in strArray)
                    {
                        string str2 = str1.Trim();
                        if (str2.Length > 0)
                        {
                            if (stringList == null)
                                stringList = new List<string>(strArray.Length);
                            stringList.Add(str2);
                        }
                    }
                }
                else
                {
                    value = value.Trim();
                    if (value.Length > 0)
                    {
                        stringList = new List<string>(1);
                        stringList.Add(value);
                    }
                }
            }
            return stringList;
        }

        [Conditional("DEBUG_PLAYLIST")]
        private static void _DEBUG_Trace(string message, params object[] args)
        {
        }

        public static int InvalidPlaylistId => int.MinValue;

        public static int NowPlayingId => -1;

        public static int ImportErrorCode => HRESULT._NS_E_WMP_PLAYLIST_IMPORT_ERROR.Int;
    }
}
