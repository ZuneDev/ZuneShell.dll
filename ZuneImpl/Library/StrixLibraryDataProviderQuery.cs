using Microsoft.Iris;
using Microsoft.Iris.Markup;
using Microsoft.Iris.UI;
using Microsoft.Zune.Playlist;
using MicrosoftZuneLibrary;
using StrixMusic.Sdk.AppModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Zune.Library
{
    public class StrixLibraryDataProviderQuery : DataProviderQuery
    {
        private LibraryVirtualList m_virtualListResultSet;
        private bool m_disposed = false;
        private int m_requestGeneration;
        private string m_thumbnailFallbackImageUrl;
        private MarkupDataTypeSchema m_dataType;

        private readonly IStrixDataRoot m_dataRoot;

        public StrixLibraryDataProviderQuery(object typeCookie, IStrixDataRoot dataRoot) : base(typeCookie)
        {
            m_dataRoot = dataRoot;
            m_dataType = (MarkupDataTypeSchema)((MarkupDataTypeSchema)ResultTypeCookie).Properties[0].AlternateType;
        }

        public bool GetSortAttributes(out string[] sorts, out bool[] ascendings)
        {
            return LibraryDataProvider.GetSortAttributes((string)GetProperty("Sort"), out sorts, out ascendings);
        }

        protected override void BeginExecute()
        {
            LibraryVirtualList virtualListResultSet = m_virtualListResultSet;
            if (virtualListResultSet != null)
            {
                ((IDisposable)virtualListResultSet).Dispose();
                m_virtualListResultSet = null;
            }
            m_thumbnailFallbackImageUrl = null;
            m_requestGeneration++;
            Status = DataProviderQueryStatus.RequestingData;
            BeginExecuteWorker(m_requestGeneration);
        }

        private void BeginExecuteWorker(object state)
        {
            bool[] sortAscendings = null;
            string[] sortStrings = null;
            string str = null;
            int num = (int)state;

            if (num == this.m_requestGeneration)
            {
                ZuneQueryList queryList = new();

                //QueryPropertyBag queryPropertyBag = new QueryPropertyBag();
                //IQueryPropertyBag* iqueryPropertyBag = queryPropertyBag.GetIQueryPropertyBag();
                bool retainedList = false;
                if (GetSortAttributes(out sortStrings, out sortAscendings))
                {

                }
                object property = GetProperty("ArtistIds");
                if (property != null)
                {

                }
                object property2 = GetProperty("GenreIds");
                if (property2 != null)
                {

                }
                object property3 = GetProperty("AlbumIds");
                if (property3 != null)
                {

                }
                object property4 = GetProperty("UserCardIds");
                if (property4 != null)
                {

                }
                object property5 = GetProperty("DeviceId");
                if (property5 != null)
                {

                }
                else
                {

                }
                object property6 = GetProperty("SyncMappedError");
                if (property6 != null)
                {

                }
                property6 = GetProperty("UserId");
                if (property6 != null)
                {

                }
                else
                {

                }
                object property7 = GetProperty("InLibrary");
                if (property7 != null)
                {

                }
                //object obj13 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvCdecl)(System.IntPtr, EQueryPropertyBagProp, System.Int32), iqueryPropertyBag, 2, 0, *(*(long*)iqueryPropertyBag + 56L));
                EQueryTypeView equeryTypeView = EQueryTypeView.eQueryTypeLibraryView;
                object property8 = GetProperty("ShowDeviceContents");
                if (property8 != null)
                {
                    equeryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeDeviceView : equeryTypeView);
                }
                property8 = GetProperty("DiscMediaView");
                if (property8 != null)
                {
                    equeryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeDiscMediaView : equeryTypeView);
                }
                property8 = GetProperty("Remaining");
                if (property8 != null)
                {
                    equeryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeSyncRemaining : equeryTypeView);
                }
                property8 = GetProperty("Complete");
                if (property8 != null)
                {
                    equeryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeSyncSucceeded : equeryTypeView);
                }
                property8 = GetProperty("Failed");
                if (property8 != null)
                {
                    equeryTypeView = (((bool)property8) ? EQueryTypeView.eQueryTypeSyncFailed : equeryTypeView);
                }
                property8 = GetProperty("MultiSelect");
                if (property8 != null && (bool)property8)
                {
                    equeryTypeView = ((EQueryTypeView.eQueryTypeDeviceView == equeryTypeView) ? EQueryTypeView.eQueryTypeDeviceMultiSelectView : EQueryTypeView.eQueryTypeLibraryMultiSelectView);
                }
                object property9 = GetProperty("RulesOnly");
                if (property9 != null)
                {
                    equeryTypeView = (((bool)property9) ? EQueryTypeView.eQueryTypeDeviceSyncRuleView : equeryTypeView);
                }
                //object obj14 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvCdecl)(System.IntPtr, EQueryPropertyBagProp, System.Int32), iqueryPropertyBag, 15, equeryTypeView, *(*(long*)iqueryPropertyBag + 56L));
                object keywordsProp = GetProperty("Keywords");
                if (keywordsProp != null)
                {

                }
                object property11 = GetProperty("ContributingArtistId");
                if (property11 != null)
                {

                }
                property11 = GetProperty("ArtistId");
                if (property11 != null)
                {

                }
                property11 = GetProperty("GenreId");
                if (property11 != null)
                {

                }
                property11 = GetProperty("AlbumId");
                if (property11 != null)
                {

                }
                property11 = GetProperty("FolderId");
                if (property11 != null)
                {

                }
                property11 = GetProperty("RecurseIntoFolders");
                if (property11 != null && (int)property11 != 0)
                {

                }
                object property12 = GetProperty("FolderMediaType");
                if (property12 != null)
                {
                    //EMediaTypes emediaTypes = LibraryDataProvider.NameToMediaType((string)property12);

                }
                object property13 = GetProperty("MediaType");
                if (property13 != null)
                {

                }
                object property14 = GetProperty("TOC");
                if (property14 != null)
                {

                }
                object property15 = GetProperty("SeriesId");
                if (property15 != null)
                {

                }
                property15 = GetProperty("WatchType");
                if (property15 != null)
                {

                }
                if (GetProperty("ExpiresOnly") != null)
                {

                }
                object property16 = GetProperty("Operation");
                if (property16 != null)
                {

                }
                property16 = GetProperty("InitTime");
                if (property16 != null)
                {

                }
                object property17 = GetProperty("PlaylistId");
                if (property17 != null)
                {

                }
                property17 = GetProperty("CategoryId");
                if (property17 != null)
                {

                }
                property17 = GetProperty("PlaylistType");
                if (property17 != null && !string.IsNullOrEmpty((string)property17))
                {
                    PlaylistType playlistType = (PlaylistType)Enum.Parse(typeof(PlaylistType), (string)property17);
                    //object obj32 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvCdecl)(System.IntPtr, EQueryPropertyBagProp, System.Int32), iqueryPropertyBag, 24, playlistType, *(*(long*)iqueryPropertyBag + 56L));
                }
                object property18 = GetProperty("PlaylistTypeMask");
                if (property18 != null)
                {
                    int num9 = (int)property18;
                    if (num9 != 0)
                    {

                    }
                }
                if (!TryGetProperty("MaxResultCount", out int maxResultCount))
                {
                    maxResultCount = int.MaxValue;
                }
                var property19 = GetProperty("DrmStateMask");
                if (property19 != null)
                {
                    ulong num10 = (ulong)((long)property19);
                    if (num10 != 0UL)
                    {

                    }
                }
                string text = (string)GetProperty("QueryType");
                EQueryType equeryType = EQueryType.eQueryTypeInvalid;
                if (keywordsProp != null)//queryPropertyBag.IsSet("Keywords"))
                {
                    if (text == "Artist")
                    {
                        equeryType = EQueryType.eQueryTypeArtistsWithKeyword;
                    }
                    else if (text == "Album")
                    {
                        equeryType = EQueryType.eQueryTypeAlbumsWithKeyword;
                    }
                    else if (text == "Track")
                    {
                        equeryType = EQueryType.eQueryTypeTracksWithKeyword;
                    }
                    else if (text == "Playlist")
                    {
                        equeryType = EQueryType.eQueryTypePlaylistsWithKeyword;
                    }
                    else if (text == "Photo")
                    {
                        equeryType = EQueryType.eQueryTypePhotosWithKeyword;
                    }
                    else if (text == "PodcastSeries")
                    {
                        equeryType = EQueryType.eQueryTypeSubscriptionsSeriesWithKeyword;
                    }
                    else if (text == "PodcastEpisode")
                    {
                        equeryType = EQueryType.eQueryTypeSubscriptionsEpisodesWithKeyword;
                    }
                    else if (text == "Video")
                    {
                        equeryType = EQueryType.eQueryTypeVideoWithKeyword;
                    }
                }
                else if (text == "Artist")
                {
                    equeryType = EQueryType.eQueryTypeAllAlbumArtists;
                }
                else if (text == "Genres")
                {
                    if (GetProperty("MediaType") is not int genreMediaType)
                        genreMediaType = 3;

                    equeryType = EQueryType.eQueryTypeAllGenres;

                    object genre = new Class(m_dataType);
                    m_dataType.FindPropertyDeep("LibraryId").SetValue(ref genre, 1);
                    m_dataType.FindPropertyDeep("Title").SetValue(ref genre, "OpenZune");
                    m_dataType.FindPropertyDeep("SyncState").SetValue(ref genre, 0);
                    m_dataType.FindPropertyDeep("Type").SetValue(ref genre, "genre");
                    m_dataType.FindPropertyDeep("DeviceFileSize").SetValue(ref genre, 0L);
                    m_dataType.FindPropertyDeep("Copyright").SetValue(ref genre, "Copyright no-one");
                    m_dataType.FindPropertyDeep("DateLastPlayed").SetValue(ref genre, DateTime.Now);
                    queryList.Add(genre);
                }
                else if (text == "Album")
                {
                    if (TryGetProperty("ArtistId", out int artistId) && artistId != -1)// || queryPropertyBag.IsSet("ArtistIds"))
                    {
                        equeryType = EQueryType.eQueryTypeAlbumsForAlbumArtistId;
                    }
                    else
                    {
                        if ((TryGetProperty("GenreId", out int genreId) && genreId != -1))// || queryPropertyBag.IsSet("GenreIds"))
                            equeryType = EQueryType.eQueryTypeAlbumsByGenreId;
                        else
                            equeryType = EQueryType.eQueryTypeAllAlbums;
                    }
                    retainedList = true;
                }
                else if (text == "Track")
                {
                    if (GetProperty("RulesOnly") is bool queryAllTracks)
                        equeryType = EQueryType.eQueryTypeAllTracks;

                    if ((TryGetProperty<int>("AlbumId", out var albumId) && albumId != -1))// || queryPropertyBag.IsSet("AlbumIds"))
                    {
                        if ((TryGetProperty<int>("ArtistId", out var artistId) && artistId != -1))// || queryPropertyBag.IsSet("ArtistIds"))
                            equeryType = EQueryType.eQueryTypeTracksForAlbumArtistId;
                        else
                            equeryType = EQueryType.eQueryTypeTracksForAlbumId;
                    }
                    else
                    {
                        if ((TryGetProperty<int>("ArtistId", out var artistId) && artistId != -1))// || queryPropertyBag.IsSet("ArtistIds"))
                        {
                            equeryType = EQueryType.eQueryTypeTracksForAlbumArtistId;
                        }
                        else
                        {
                            if ((TryGetProperty<int>("GenreId", out var genreId) && genreId != -1))// || queryPropertyBag.IsSet("GenreIds"))
                            {
                                equeryType = EQueryType.eQueryTypeTracksByGenreId;
                            }
                            else
                            {
                                if (TryGetProperty<bool>("Detailed", out var detailed) && detailed)
                                {
                                    equeryType = EQueryType.eQueryTypeAllTracksDetailed;
                                }
                                else
                                {
                                    if (TryGetProperty<string>("TOC", out var toc) && !string.IsNullOrEmpty(toc))
                                    {
                                        equeryType = EQueryType.eQueryTypeTracksForTOC;
                                    }
                                    else
                                    {
                                        equeryType = EQueryType.eQueryTypeAllTracks;
                                    }
                                }
                            }
                        }
                    }

                    var stTracks = m_dataRoot.Library.GetTracksAsync(int.MaxValue, 0).ToEnumerable();
                    foreach (var stTrack in stTracks)
                    {
                        object track = new Class(m_dataType);

                        m_dataType.FindProperty("AlbumName").SetValue(ref track, stTrack.Album?.Name);
                        m_dataType.FindProperty("AlbumLibraryId").SetValue(ref track, stTrack.Album?.Id.HashToInt32() ?? 0);

                        var stAlbumArtist = stTrack.Album != null
                            ? AsyncHelper.Run(() => stTrack.Album.GetArtistItemsAsync(1, 0).FirstOrDefaultAsync().AsTask())
                            : null;
                        m_dataType.FindProperty("AlbumArtistName").SetValue(ref track, stAlbumArtist?.Name);
                        m_dataType.FindProperty("AlbumArtistLibraryId").SetValue(ref track, stAlbumArtist?.Id.HashToInt32() ?? 0);

                        var stArtists = stTrack.GetArtistItemsAsync(int.MaxValue, 0).ToEnumerable();
                        bool setPrimaryArtist = false;
                        List<string> contributingArtists = new();
                        foreach (var stArtist in stArtists)
                        {
                            if (!setPrimaryArtist)
                            {
                                m_dataType.FindProperty("ArtistLibraryId").SetValue(ref track, stArtist.Id.HashToInt32());
                                m_dataType.FindProperty("ArtistName").SetValue(ref track, stArtist.Name);
                                m_dataType.FindProperty("ArtistNameYomi").SetValue(ref track, stArtist.Name);
                                setPrimaryArtist = true;
                            }
                            else if (!string.IsNullOrEmpty(stArtist.Name))
                            {
                                contributingArtists.Add(stArtist.Name);
                            }
                        }
                        m_dataType.FindProperty("ContributingArtistCount").SetValue(ref track, contributingArtists.Count);
                        m_dataType.FindProperty("ContributingArtistNames").SetValue(ref track, contributingArtists);

                        // No artists
                        if (!setPrimaryArtist)
                        {
                            m_dataType.FindProperty("ArtistLibraryId").SetValue(ref track, 0);
                            m_dataType.FindProperty("ArtistName").SetValue(ref track, null);
                            m_dataType.FindProperty("ArtistNameYomi").SetValue(ref track, null);
                        }

                        var stGenre = AsyncHelper.Run(() => stTrack.GetGenresAsync(1, 0).FirstOrDefaultAsync().AsTask());
                        m_dataType.FindProperty("Genre").SetValue(ref track, stGenre?.Name);

                        m_dataType.FindProperty("TitleYomi").SetValue(ref track, stTrack.Name);
                        m_dataType.FindProperty("ReleaseDate").SetValue(ref track, DateTime.Now);
                        m_dataType.FindProperty("ZuneMediaId").SetValue(ref track, stTrack.Id.HashToGuid());
                        m_dataType.FindProperty("TrackNumber").SetValue(ref track, stTrack.TrackNumber);
                        m_dataType.FindProperty("Duration").SetValue(ref track, stTrack.Duration);
                        m_dataType.FindProperty("ComponentId").SetValue(ref track, "strix");
                        m_dataType.FindProperty("FileName").SetValue(ref track, System.IO.Path.GetFileName(stTrack.Id));
                        m_dataType.FindProperty("FolderName").SetValue(ref track, System.IO.Path.GetDirectoryName(stTrack.Id));
                        m_dataType.FindProperty("FilePath").SetValue(ref track, stTrack.Id);
                        m_dataType.FindProperty("NowPlaying").SetValue(ref track, false);
                        m_dataType.FindProperty("InLibrary").SetValue(ref track, true);
                        m_dataType.FindProperty("DateLastPlayed").SetValue(ref track, stTrack.LastPlayed);
                        m_dataType.FindProperty("PlayCount").SetValue(ref track, 0);
                        m_dataType.FindProperty("FileSize").SetValue(ref track, 0);
                        m_dataType.FindProperty("ComposerName").SetValue(ref track, (string)null);
                        m_dataType.FindProperty("ConductorName").SetValue(ref track, (string)null);
                        m_dataType.FindProperty("IsProtected").SetValue(ref track, 0);
                        m_dataType.FindProperty("DateAdded").SetValue(ref track, stTrack.AddedAt);
                        m_dataType.FindProperty("Bitrate").SetValue(ref track, 0);
                        m_dataType.FindProperty("MediaType").SetValue(ref track, "track");
                        m_dataType.FindProperty("DiscNumber").SetValue(ref track, stTrack.DiscNumber);
                        m_dataType.FindProperty("DateAlbumAdded").SetValue(ref track, DateTime.Now);
                        m_dataType.FindProperty("FileCount").SetValue(ref track, 1L);
                        m_dataType.FindProperty("DrmState").SetValue(ref track, 0);
                        m_dataType.FindProperty("DrmStateMask").SetValue(ref track, 0L);
                        m_dataType.FindProperty("QuickMixState").SetValue(ref track, 0);

                        m_dataType.FindPropertyDeep("LibraryId").SetValue(ref track, stTrack.Id.HashToInt32());
                        m_dataType.FindPropertyDeep("Title").SetValue(ref track, stTrack.Name);
                        m_dataType.FindPropertyDeep("SyncState").SetValue(ref track, 0);
                        m_dataType.FindPropertyDeep("Type").SetValue(ref track, "track");
                        m_dataType.FindPropertyDeep("DeviceFileSize").SetValue(ref track, 0L);
                        m_dataType.FindPropertyDeep("DateAdded").SetValue(ref track, DateTime.Now);

                        m_dataType.FindPropertyDeep("UserRating").SetValue(ref track, 0);

                        queryList.Add(track);
                    }
                }
                else if (text == "AlbumByTOC")
                {
                    equeryType = EQueryType.eQueryTypeAlbumsByTOC;
                    //if (!queryPropertyBag.IsSet("TOC"))
                    //{
                    //	equeryType = EQueryType.eQueryTypeInvalid;
                    //}
                }
                else if (text == "Photo")
                {
                    equeryType = ((equeryTypeView == EQueryTypeView.eQueryTypeDeviceSyncRuleView) ? EQueryType.eQueryTypeAllPhotos : EQueryType.eQueryTypePhotosByFolderId);
                }
                else if (text == "MediaFolder")
                {
                    equeryType = EQueryType.eQueryTypeMediaFolders;
                }
                else if (text == "Video")
                {
                    property19 = GetProperty("CategoryId");
                    if (property19 != null && (int)property19 != -1)
                    {
                        equeryType = EQueryType.eQueryTypeVideosByCategoryId;
                    }
                    else
                    {
                        equeryType = EQueryType.eQueryTypeAllVideos;
                    }
                }
                else if (text == "PodcastSeries")
                {
                    equeryType = EQueryType.eQueryTypeAllPodcastSeries;
                }
                else if (text == "PodcastEpisode")
                {
                    property19 = GetProperty("SeriesId");
                    if (property19 != null && (int)property19 != -1)
                    {
                        equeryType = EQueryType.eQueryTypeEpisodesForSeriesId;
                    }
                    else
                    {
                        equeryType = EQueryType.eQueryTypeAllPodcastEpisodes;
                    }
                }
                else if (text == "SyncItem")
                {
                    equeryType = EQueryType.eQueryTypeSyncProgress;
                }
                else if (text == "Playlist")
                {
                    equeryType = EQueryType.eQueryTypeAllPlaylists;
                }
                else if (text == "PlaylistContent")
                {
                    equeryType = EQueryType.eQueryTypePlaylistContentByPlaylistId;
                }
                else if (text == "UserCard")
                {
                    equeryType = EQueryType.eQueryTypeUserCards;
                }
                else if (text == "Person")
                {
                    equeryType = EQueryType.eQueryTypePersonsByTypeId;
                    EMediaTypes emediaTypes2 = EMediaTypes.eMediaTypePersonArtist;
                    emediaTypes2 = (((string)GetProperty("PersonType") == "Composer") ? EMediaTypes.eMediaTypePersonComposer : emediaTypes2);

                }
                else if (text == "ArtistsRanking")
                {
                    equeryType = EQueryType.eQueryTypeArtistsRanking;
                }
                else if (text == "TVSeries")
                {
                    equeryType = EQueryType.eQueryTypeVideoSeriesTitles;
                }
                else if (text == "Pin")
                {
                    equeryType = EQueryType.eQueryTypePinsByPinType;
                    EPinType epinType = EPinType.ePinTypeGeneric;
                    property19 = GetProperty("PinType");
                    if (property19 != null)
                    {
                        epinType = (EPinType)property19;
                    }

                    object pin = new Class(m_dataType);
                    m_dataType.FindProperty("PinId").SetValue(ref pin, 42);
                    m_dataType.FindProperty("PinType").SetValue(ref pin, 1);
                    m_dataType.FindProperty("Ordinal").SetValue(ref pin, 1);
                    m_dataType.FindProperty("Description").SetValue(ref pin, "Haha! Get rekt scrub, not even UIB will stop OpenZune.");
                    m_dataType.FindProperty("MediaId").SetValue(ref pin, 1);
                    m_dataType.FindProperty("MediaType").SetValue(ref pin, 1);
                    m_dataType.FindProperty("ZuneMediaRef").SetValue(ref pin, "ZuneSomethingHehe");
                    m_dataType.FindProperty("UserId").SetValue(ref pin, 0);
                    m_dataType.FindProperty("DateModified").SetValue(ref pin, DateTime.Now);
                    m_dataType.FindPropertyDeep("ZuneMediaType").SetValue(ref pin, 0);
                    queryList.Add(pin);
                }
                else
                {
                    equeryType = ((text == "App") ? EQueryType.eQueryTypeAllApps : equeryType);
                }

                queryList.QueryType = equeryType;
                DeferredSetResult(new DeferredSetResultArgs(num, queryList, false));// retainedList));
            }
        }

        private void DeferredSetResult(object state)
        {
            DeferredSetResultArgs deferredSetResultArgs = (DeferredSetResultArgs)state;
            bool isEmpty = true;
            if (deferredSetResultArgs.QueryList != null)
            {
                object property = GetProperty("AutoRefresh");
                bool autoRefresh = property == null || (bool)property;

                object property2 = GetProperty("AntialiasImageEdges");
                bool antialiasEdges = property2 != null && (bool)property2;

                m_thumbnailFallbackImageUrl = (string)GetProperty("ThumbnailFallbackImageUrl");

                LibraryVirtualList virtualListResultSet = m_virtualListResultSet;
                (virtualListResultSet as IDisposable)?.Dispose();

                LibraryVirtualList libraryVirtualList = new(this, deferredSetResultArgs.QueryList, autoRefresh, antialiasEdges);
                m_virtualListResultSet = libraryVirtualList;

                ReleaseBehavior visualReleaseBehavior = deferredSetResultArgs.RetainedList ? ReleaseBehavior.KeepReference : ReleaseBehavior.ReleaseReference;
                libraryVirtualList.VisualReleaseBehavior = visualReleaseBehavior;

                isEmpty = deferredSetResultArgs.QueryList.IsEmpty;
            }

            StrixLibraryDataProviderQueryResult libraryDataProviderQueryResult = new(this, m_virtualListResultSet, base.ResultTypeCookie);
            libraryDataProviderQueryResult.SetIsEmpty(isEmpty);
            Result = libraryDataProviderQueryResult;
            Status = DataProviderQueryStatus.Complete;
        }
    }

    internal class DeferredSetResultArgs
    {
        public DeferredSetResultArgs(int requestGeneration, ZuneQueryList queryList, bool retainedList)
        {
            RequestGeneration = requestGeneration;
            QueryList = queryList;
            RetainedList = retainedList;
        }

        public int RequestGeneration { get; }

        public ZuneQueryList QueryList { get; }

        public bool RetainedList { get; }
    }
}
