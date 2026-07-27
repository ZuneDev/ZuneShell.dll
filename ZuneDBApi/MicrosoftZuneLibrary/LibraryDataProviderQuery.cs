using System;
using System.Collections;
using Microsoft.Iris;
using Microsoft.Zune.Playlist;
using MicrosoftZuneInterop;

namespace MicrosoftZuneLibrary;

// Property-bag population and EQueryType derivation are pure C#/Iris business logic with
// no native dependency of their own, so they're transcribed as real logic (verbatim from
// the decompiled original, translated off raw IQueryPropertyBag vtable-offset calls onto
// QueryPropertyBag's managed SetValue/SetIDList/SetMultiSortAttributes/IsSet wrappers, per
// CLAUDE.md's COM-objects rule). The actual native database query
// (ZuneLibraryExports.QueryDatabase) is not reverse engineered — no native database engine
// exists in this codebase, see logs/MicrosoftZuneLibrary/ZuneLibrary.md — so it always
// "succeeds" with an empty result set, matching ZuneLibrary.QueryDatabase's existing stub.
// Only the ETW PERFTRACE_COLLECTIONEVENT calls were dropped (diagnostic-only, same
// treatment as VirtualDatabaseList/LibraryDataProviderItemBase).
internal class LibraryDataProviderQuery : DataProviderQuery
{
    protected LibraryVirtualList m_virtualListResultSet;

    private bool m_disposed;

    private int m_requestGeneration;

    private string m_thumbnailFallbackImageUrl;

    private static readonly WorkerQueue m_libQueriesQueue = WorkerQueue.CreateInstance();

    public string ThumbnailFallbackImageUrl => m_thumbnailFallbackImageUrl;

    internal LibraryDataProviderQuery(object queryTypeCookie)
        : base(queryTypeCookie)
    {
        LibraryDataProviderQueryResult result = new(this, null, ResultTypeCookie);
        result.SetIsEmpty(true);
        Result = result;
    }

    public bool GetSortAttributes(out string[] sorts, out bool[] ascendings)
        => LibraryDataProvider.GetSortAttributes((string)GetProperty("Sort"), out sorts, out ascendings);

    protected override void BeginExecute()
    {
        if (m_disposed)
            return;

        m_virtualListResultSet?.Dispose();
        m_virtualListResultSet = null;
        m_thumbnailFallbackImageUrl = null;
        m_requestGeneration++;
        Status = DataProviderQueryStatus.RequestingData;
        m_libQueriesQueue.QueueSequentialWorkItem(BeginExecuteWorker, m_requestGeneration);
    }

    protected override void OnDispose()
    {
        m_disposed = true;
        m_virtualListResultSet?.Dispose();
        m_virtualListResultSet = null;
    }

    private void BeginExecuteWorker(object state)
    {
        int requestGeneration = (int)state;
        if (requestGeneration != m_requestGeneration)
            return;

        using QueryPropertyBag queryPropertyBag = new();
        bool retainedList = false;

        if (GetSortAttributes(out string[] sorts, out bool[] ascendings))
            queryPropertyBag.SetMultiSortAttributes(sorts, ascendings);

        if (GetProperty("ArtistIds") is IList artistIds)
            queryPropertyBag.SetIDList("ArtistIds", artistIds);
        if (GetProperty("GenreIds") is IList genreIds)
            queryPropertyBag.SetIDList("GenreIds", genreIds);
        if (GetProperty("AlbumIds") is IList albumIds)
            queryPropertyBag.SetIDList("AlbumIds", albumIds);
        if (GetProperty("UserCardIds") is IList userCardIds)
            queryPropertyBag.SetIDList("UserCardIds", userCardIds);

        queryPropertyBag.SetValue("DeviceId", GetProperty("DeviceId") ?? 1);

        if (GetProperty("SyncMappedError") is { } syncMappedError)
            queryPropertyBag.SetValue("SyncMappedError", syncMappedError);

        queryPropertyBag.SetValue("UserId", GetProperty("UserId") ?? 1);

        if (GetProperty("InLibrary") is { } inLibrary)
            queryPropertyBag.SetValue("InLibrary", inLibrary);

        queryPropertyBag.SetValue("RuleTypeId", 0);

        EQueryTypeView queryView = EQueryTypeView.eQueryTypeLibraryView;
        if (GetProperty("ShowDeviceContents") is bool showDeviceContents)
            queryView = showDeviceContents ? EQueryTypeView.eQueryTypeDeviceView : queryView;
        if (GetProperty("DiscMediaView") is bool discMediaView)
            queryView = discMediaView ? EQueryTypeView.eQueryTypeDiscMediaView : queryView;
        if (GetProperty("Remaining") is bool remaining)
            queryView = remaining ? EQueryTypeView.eQueryTypeSyncRemaining : queryView;
        if (GetProperty("Complete") is bool complete)
            queryView = complete ? EQueryTypeView.eQueryTypeSyncSucceeded : queryView;
        if (GetProperty("Failed") is bool failed)
            queryView = failed ? EQueryTypeView.eQueryTypeSyncFailed : queryView;
        if (GetProperty("MultiSelect") is true)
        {
            queryView = queryView == EQueryTypeView.eQueryTypeDeviceView
                ? EQueryTypeView.eQueryTypeDeviceMultiSelectView
                : EQueryTypeView.eQueryTypeLibraryMultiSelectView;
        }
        if (GetProperty("RulesOnly") is bool rulesOnlyView)
            queryView = rulesOnlyView ? EQueryTypeView.eQueryTypeDeviceSyncRuleView : queryView;

        queryPropertyBag.SetValue("QueryView", (int)queryView);

        if (GetProperty("Keywords") is string keywords)
            queryPropertyBag.SetValue("Keywords", keywords);

        if (GetProperty("ContributingArtistId") is { } contributingArtistId)
            queryPropertyBag.SetValue("ContributingArtistId", contributingArtistId);
        if (GetProperty("ArtistId") is { } artistId)
            queryPropertyBag.SetValue("ArtistId", artistId);
        if (GetProperty("GenreId") is { } genreId)
            queryPropertyBag.SetValue("GenreId", genreId);
        if (GetProperty("AlbumId") is { } albumId)
            queryPropertyBag.SetValue("AlbumId", albumId);
        if (GetProperty("FolderId") is { } folderId)
            queryPropertyBag.SetValue("FolderId", folderId);

        if (GetProperty("RecurseIntoFolders") is int recurseIntoFolders && recurseIntoFolders != 0)
            queryPropertyBag.SetValue("Recursive", 1);

        if (GetProperty("FolderMediaType") is string folderMediaType)
            queryPropertyBag.SetValue("MediaType", (int)LibraryDataProvider.NameToMediaType(folderMediaType));
        if (GetProperty("MediaType") is { } mediaType)
            queryPropertyBag.SetValue("MediaType", mediaType);

        if (GetProperty("TOC") is string toc)
            queryPropertyBag.SetValue("TOC", toc);

        if (GetProperty("SeriesId") is { } seriesId)
            queryPropertyBag.SetValue("SeriesId", seriesId);
        if (GetProperty("WatchType") is { } watchType)
            queryPropertyBag.SetValue("WatchType", watchType);

        if (GetProperty("ExpiresOnly") != null)
            queryPropertyBag.SetValue("ExpiresOnly", 1);

        if (GetProperty("Operation") is { } operation)
            queryPropertyBag.SetValue("Operation", operation);
        if (GetProperty("InitTime") is string initTime)
            queryPropertyBag.SetValue("InitTime", initTime);

        if (GetProperty("PlaylistId") is { } playlistId)
            queryPropertyBag.SetValue("PlaylistId", playlistId);
        if (GetProperty("CategoryId") is { } categoryId)
            queryPropertyBag.SetValue("CategoryId", categoryId);

        if (GetProperty("PlaylistType") is string playlistTypeName && !string.IsNullOrEmpty(playlistTypeName))
        {
            PlaylistType playlistType = (PlaylistType)Enum.Parse(typeof(PlaylistType), playlistTypeName);
            queryPropertyBag.SetValue("PlaylistType", (int)playlistType);
        }

        if (GetProperty("PlaylistTypeMask") is int playlistTypeMask && playlistTypeMask != 0)
            queryPropertyBag.SetValue("PlaylistTypeMask", playlistTypeMask);

        if (GetProperty("MaxResultCount") is { } maxResultCount)
            queryPropertyBag.SetValue("MaxResultCount", maxResultCount);

        if (GetProperty("DrmStateMask") is long drmStateMaskRaw)
        {
            ulong drmStateMask = (ulong)drmStateMaskRaw;
            if (drmStateMask != 0)
                queryPropertyBag.SetValue("DrmStateMask", drmStateMask);
        }

        string queryTypeName = (string)GetProperty("QueryType");
        EQueryType queryType = EQueryType.eQueryTypeInvalid;

        if (queryPropertyBag.IsSet("Keywords"))
        {
            queryType = queryTypeName switch
            {
                "Artist" => EQueryType.eQueryTypeArtistsWithKeyword,
                "Album" => EQueryType.eQueryTypeAlbumsWithKeyword,
                "Track" => EQueryType.eQueryTypeTracksWithKeyword,
                "Playlist" => EQueryType.eQueryTypePlaylistsWithKeyword,
                "Photo" => EQueryType.eQueryTypePhotosWithKeyword,
                "PodcastSeries" => EQueryType.eQueryTypeSubscriptionsSeriesWithKeyword,
                "PodcastEpisode" => EQueryType.eQueryTypeSubscriptionsEpisodesWithKeyword,
                "Video" => EQueryType.eQueryTypeVideoWithKeyword,
                _ => queryType,
            };
        }
        else
        {
            switch (queryTypeName)
            {
                case "Artist":
                    queryType = EQueryType.eQueryTypeAllAlbumArtists;
                    break;

                case "Genres":
                {
                    object genreMediaType = GetProperty("MediaType");
                    queryPropertyBag.SetValue("MediaType", genreMediaType is null ? 3 : (int)genreMediaType);
                    queryType = EQueryType.eQueryTypeAllGenres;
                    break;
                }

                case "Album":
                {
                    object albumArtistId = GetProperty("ArtistId");
                    if ((albumArtistId != null && (int)albumArtistId != -1) || queryPropertyBag.IsSet("ArtistIds"))
                    {
                        queryType = EQueryType.eQueryTypeAlbumsForAlbumArtistId;
                    }
                    else
                    {
                        object albumGenreId = GetProperty("GenreId");
                        queryType = (albumGenreId == null || (int)albumGenreId == -1) && !queryPropertyBag.IsSet("GenreIds")
                            ? EQueryType.eQueryTypeAllAlbums
                            : EQueryType.eQueryTypeAlbumsByGenreId;
                    }
                    retainedList = true;
                    break;
                }

                case "Track":
                {
                    if (GetProperty("RulesOnly") is true)
                    {
                        queryType = EQueryType.eQueryTypeAllTracks;
                        break;
                    }

                    object trackAlbumId = GetProperty("AlbumId");
                    if ((trackAlbumId != null && (int)trackAlbumId != -1) || queryPropertyBag.IsSet("AlbumIds"))
                    {
                        object albumTrackArtistId = GetProperty("ArtistId");
                        queryType = (albumTrackArtistId == null || (int)albumTrackArtistId == -1) && !queryPropertyBag.IsSet("ArtistIds")
                            ? EQueryType.eQueryTypeTracksForAlbumId
                            : EQueryType.eQueryTypeTracksForAlbumArtistId;
                        break;
                    }

                    object trackArtistId = GetProperty("ArtistId");
                    if ((trackArtistId != null && (int)trackArtistId != -1) || queryPropertyBag.IsSet("ArtistIds"))
                    {
                        queryType = EQueryType.eQueryTypeTracksForAlbumArtistId;
                        break;
                    }

                    object trackGenreId = GetProperty("GenreId");
                    if ((trackGenreId != null && (int)trackGenreId != -1) || queryPropertyBag.IsSet("GenreIds"))
                    {
                        queryType = EQueryType.eQueryTypeTracksByGenreId;
                        break;
                    }

                    if (GetProperty("Detailed") is true)
                    {
                        queryType = EQueryType.eQueryTypeAllTracksDetailed;
                        break;
                    }

                    queryType = GetProperty("TOC") is string trackToc && !string.IsNullOrEmpty(trackToc)
                        ? EQueryType.eQueryTypeTracksForTOC
                        : EQueryType.eQueryTypeAllTracks;
                    break;
                }

                case "AlbumByTOC":
                    queryType = queryPropertyBag.IsSet("TOC") ? EQueryType.eQueryTypeAlbumsByTOC : EQueryType.eQueryTypeInvalid;
                    break;

                case "Photo":
                    queryType = queryView == EQueryTypeView.eQueryTypeDeviceSyncRuleView
                        ? EQueryType.eQueryTypeAllPhotos
                        : EQueryType.eQueryTypePhotosByFolderId;
                    break;

                case "MediaFolder":
                    queryType = EQueryType.eQueryTypeMediaFolders;
                    break;

                case "Video":
                {
                    object videoCategoryId = GetProperty("CategoryId");
                    queryType = (videoCategoryId == null || (int)videoCategoryId == -1)
                        ? EQueryType.eQueryTypeAllVideos
                        : EQueryType.eQueryTypeVideosByCategoryId;
                    break;
                }

                case "PodcastSeries":
                    queryType = EQueryType.eQueryTypeAllPodcastSeries;
                    break;

                case "PodcastEpisode":
                {
                    object episodeSeriesId = GetProperty("SeriesId");
                    queryType = (episodeSeriesId == null || (int)episodeSeriesId == -1)
                        ? EQueryType.eQueryTypeAllPodcastEpisodes
                        : EQueryType.eQueryTypeEpisodesForSeriesId;
                    break;
                }

                case "SyncItem":
                    queryType = EQueryType.eQueryTypeSyncProgress;
                    break;

                case "Playlist":
                    queryType = EQueryType.eQueryTypeAllPlaylists;
                    break;

                case "PlaylistContent":
                    queryType = EQueryType.eQueryTypePlaylistContentByPlaylistId;
                    break;

                case "UserCard":
                    queryType = EQueryType.eQueryTypeUserCards;
                    break;

                case "Person":
                {
                    queryType = EQueryType.eQueryTypePersonsByTypeId;
                    EMediaTypes personType = (string)GetProperty("PersonType") == "Composer"
                        ? EMediaTypes.eMediaTypePersonComposer
                        : EMediaTypes.eMediaTypePersonArtist;
                    queryPropertyBag.SetValue("PersonType", (int)personType);
                    break;
                }

                case "ArtistsRanking":
                    queryType = EQueryType.eQueryTypeArtistsRanking;
                    break;

                case "TVSeries":
                    queryType = EQueryType.eQueryTypeVideoSeriesTitles;
                    break;

                case "Pin":
                {
                    queryType = EQueryType.eQueryTypePinsByPinType;
                    EPinType pinType = GetProperty("PinType") is { } pinTypeValue ? (EPinType)pinTypeValue : EPinType.ePinTypeGeneric;
                    queryPropertyBag.SetValue("PinType", (int)pinType);
                    break;
                }

                case "App":
                    queryType = EQueryType.eQueryTypeAllApps;
                    break;
            }
        }

        if (requestGeneration != m_requestGeneration)
            return;

        ZuneQueryList queryList = null;
        if (queryType != EQueryType.eQueryTypeInvalid)
            queryList = new ZuneQueryList();

        Application.DeferredInvoke(DeferredSetResult, new DeferredSetResultArgs(requestGeneration, queryList, retainedList));
    }

    private void DeferredSetResult(object state)
    {
        DeferredSetResultArgs args = (DeferredSetResultArgs)state;
        if (m_disposed || args.RequestGeneration != m_requestGeneration)
        {
            args.QueryList?.Dispose();
            return;
        }

        bool isEmpty = true;
        if (args.QueryList != null)
        {
            bool autoRefresh = GetProperty("AutoRefresh") is not bool autoRefreshValue || autoRefreshValue;
            bool antialiasEdges = GetProperty("AntialiasImageEdges") is true;
            m_thumbnailFallbackImageUrl = (string)GetProperty("ThumbnailFallbackImageUrl");

            m_virtualListResultSet?.Dispose();
            m_virtualListResultSet = new LibraryVirtualList(args.QueryList, autoRefresh, antialiasEdges)
            {
                VisualReleaseBehavior = args.RetainedList ? ReleaseBehavior.KeepReference : ReleaseBehavior.ReleaseReference,
            };

            isEmpty = args.QueryList.IsEmpty;
        }

        LibraryDataProviderQueryResult result = new(this, m_virtualListResultSet, ResultTypeCookie);
        result.SetIsEmpty(isEmpty);
        Result = result;
        Status = DataProviderQueryStatus.Complete;
    }
}
