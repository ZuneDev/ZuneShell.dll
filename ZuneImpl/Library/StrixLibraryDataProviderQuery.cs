using Microsoft.Iris;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Zune.Playlist;
using OwlCore.Extensions;
using StrixMusic.Sdk.AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Zune.Library
{
    public class StrixLibraryDataProviderQuery : DataProviderQuery
    {
        private LibraryVirtualList m_virtualListResultSet;
        private bool m_disposed = false;
        private int m_requestGeneration;
        private string m_thumbnailFallbackImageUrl;

        private readonly MarkupTypeSchema m_dataType;
        private readonly IStrixDataRoot m_dataRoot;
        private readonly Dictionary<int, string> m_albumIdMap = new();
        private readonly Dictionary<int, string> m_artistIdMap = new();
        private readonly Dictionary<int, string> m_trackIdMap = new();

        public StrixLibraryDataProviderQuery(object typeCookie, IStrixDataRoot dataRoot) : base(typeCookie)
        {
            m_dataRoot = dataRoot;
            m_dataType = (MarkupTypeSchema)((MarkupTypeSchema)ResultTypeCookie).Properties[0].AlternateType;

            if (Application.DebugSettings.GenerateDataMappingModels)
                TypeSchema.GenerateModelCode(m_dataType);
        }

        public bool GetSortAttributes(out string[] sorts, out bool[] ascendings)
        {
            if (!TryGetProperty("Sort", out string sortStr) || sortStr == null)
            {
                sorts = null;
                ascendings = null;
                return false;
            }

            var options = sortStr.Split(',');
            sorts = new string[options.Length];
            ascendings = new bool[options.Length];

            for (int i = 0; i < options.Length; i++)
            {
                sorts[i] = options[i].Substring(1);
                ascendings[i] = options[i][0] != '-';
            }

            return true;
        }

        protected override void BeginExecute()
        {
            if (m_virtualListResultSet is IDisposable disposableResultSet)
                disposableResultSet.Dispose();

            m_virtualListResultSet = null;
            m_thumbnailFallbackImageUrl = null;
            m_requestGeneration++;

            Status = DataProviderQueryStatus.RequestingData;

            ExecuteWorker(m_requestGeneration).ConfigureAwait(true);
        }

        private async Task ExecuteWorker(object state)
        {
            bool[] sortAscendings = null;
            string[] sortStrings = null;
            int num = (int)state;

            if (num == m_requestGeneration)
            {
                ZuneQueryList queryList = new();

                //QueryPropertyBag queryPropertyBag = new QueryPropertyBag();
                //IQueryPropertyBag* iqueryPropertyBag = queryPropertyBag.GetIQueryPropertyBag();
                bool retainedList = false;
                if (GetSortAttributes(out sortStrings, out sortAscendings))
                {

                }

                if (TryGetProperty<object>("ArtistIds", out var artistIds))
                {
                }
                if (TryGetProperty<object>("GenreIds", out var genreIds))
                {
                }
                if (TryGetProperty<object>("AlbumIds", out var albumIds))
                {
                }
                if (TryGetProperty<object>("UserCardIds", out var userCardIds))
                {
                }
                if (TryGetProperty<object>("DeviceId", out var deviceId))
                {
                }
                else
                {
                }
                if (TryGetProperty<object>("SyncMappedError", out var syncMappedError))
                {
                }
                if (TryGetProperty<object>("UserId", out var userId))
                {
                }
                else
                {
                }
                if (TryGetProperty<bool>("InLibrary", out var inLibrary))
                {
                }

                //object obj13 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvCdecl)(System.IntPtr, EQueryPropertyBagProp, System.Int32), iqueryPropertyBag, 2, 0, *(*(long*)iqueryPropertyBag + 56L));
                EQueryTypeView equeryTypeView = EQueryTypeView.eQueryTypeLibraryView;
                if (TryGetProperty("ShowDeviceContents", out bool showDeviceContents))
                    equeryTypeView = showDeviceContents ? EQueryTypeView.eQueryTypeDeviceView : equeryTypeView;
                if (TryGetProperty("DiscMediaView", out bool discMediaView))
                    equeryTypeView = discMediaView ? EQueryTypeView.eQueryTypeDiscMediaView : equeryTypeView;
                if (TryGetProperty("Remaining", out bool remaining))
                    equeryTypeView = remaining ? EQueryTypeView.eQueryTypeSyncRemaining : equeryTypeView;
                if (TryGetProperty("Complete", out bool complete))
                    equeryTypeView = complete ? EQueryTypeView.eQueryTypeSyncSucceeded : equeryTypeView;
                if (TryGetProperty("Failed", out bool failed))
                    equeryTypeView = failed ? EQueryTypeView.eQueryTypeSyncFailed : equeryTypeView;
                if (TryGetProperty("MultiSelect", out bool multiSelect))
                {
                    equeryTypeView = (EQueryTypeView.eQueryTypeDeviceView == equeryTypeView)
                        ? EQueryTypeView.eQueryTypeDeviceMultiSelectView
                        : EQueryTypeView.eQueryTypeLibraryMultiSelectView;
                }
                if (TryGetProperty("RulesOnly", out bool rulesOnly))
                    equeryTypeView = rulesOnly ? EQueryTypeView.eQueryTypeDeviceSyncRuleView : equeryTypeView;

                #region Unhandled properties
                //object obj14 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvCdecl)(System.IntPtr, EQueryPropertyBagProp, System.Int32), iqueryPropertyBag, 15, equeryTypeView, *(*(long*)iqueryPropertyBag + 56L));
                if (TryGetProperty("Keywords", out string keywords))
                {
                }
                if (TryGetProperty("ContributingArtistId", out int contributingArtistId))
                {
                }
                if (TryGetProperty("ArtistId", out int artistId))
                {
                }
                if (TryGetProperty("GenreId", out int genreId))
                {
                }
                if (TryGetProperty("AlbumId", out int albumId))
                {
                }
                if (TryGetProperty("FolderId", out int folderId))
                {
                }
                if (TryGetProperty("RecurseIntoFolder", out int recurseIntoFolders) && recurseIntoFolders != 0)
                {
                }
                if (TryGetProperty("FolderMediaType", out string folderMediaType))
                {
                    //EMediaTypes emediaTypes = LibraryDataProvider.NameToMediaType(folderMediaType);
                }
                if (TryGetProperty("MediaType", out string mediaType))
                {
                }
                if (TryGetProperty("TOC", out string toc))
                {
                }
                if (TryGetProperty("SeriesId", out int seriesId))
                {
                }
                if (TryGetProperty("WatchType", out string watchType))
                {
                }
                if (TryGetProperty("ExpiresOnly", out object expiresOnly) && expiresOnly != null)
                {
                }
                if (TryGetProperty("Operation", out object operation))
                {
                }
                if (TryGetProperty("InitTime", out object initTime))
                {
                }
                if (TryGetProperty("PlaylistId", out int playlistId))
                {
                }
                if (TryGetProperty("CategoryId", out int categoryId))
                {
                }
                if (TryGetProperty("PlaylistType", out string playlistTypeStr) && !string.IsNullOrEmpty(playlistTypeStr))
                {
                    PlaylistType playlistType = (PlaylistType)Enum.Parse(typeof(PlaylistType), playlistTypeStr);
                    //object obj32 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvCdecl)(System.IntPtr, EQueryPropertyBagProp, System.Int32), iqueryPropertyBag, 24, playlistType, *(*(long*)iqueryPropertyBag + 56L));
                }
                if (TryGetProperty("PlaylistTypeMask", out int playlistTypeMask))
                {
                    if (playlistTypeMask != 0)
                    {
                    }
                }
                if (!TryGetProperty("MaxResultCount", out int maxResultCount))
                {
                    maxResultCount = int.MaxValue;
                }
                var property19 = GetProperty("DrmStateMask");
                if (TryGetProperty("DrmStateMask", out ulong drmStateMask))
                {
                    if (drmStateMask != 0UL)
                    {
                    }
                }
#endregion

                string text = (string)GetProperty("QueryType");
                EQueryType queryType = EQueryType.eQueryTypeInvalid;

                if (keywords != null)//queryPropertyBag.IsSet("Keywords"))
                {
                    if (text == "Artist")
                    {
                        queryType = EQueryType.eQueryTypeArtistsWithKeyword;
                    }
                    else if (text == "Album")
                    {
                        queryType = EQueryType.eQueryTypeAlbumsWithKeyword;
                    }
                    else if (text == "Track")
                    {
                        queryType = EQueryType.eQueryTypeTracksWithKeyword;
                    }
                    else if (text == "Playlist")
                    {
                        queryType = EQueryType.eQueryTypePlaylistsWithKeyword;
                    }
                    else if (text == "Photo")
                    {
                        queryType = EQueryType.eQueryTypePhotosWithKeyword;
                    }
                    else if (text == "PodcastSeries")
                    {
                        queryType = EQueryType.eQueryTypeSubscriptionsSeriesWithKeyword;
                    }
                    else if (text == "PodcastEpisode")
                    {
                        queryType = EQueryType.eQueryTypeSubscriptionsEpisodesWithKeyword;
                    }
                    else if (text == "Video")
                    {
                        queryType = EQueryType.eQueryTypeVideoWithKeyword;
                    }
                }
                else if (text == "Artist")
                {
                    queryType = EQueryType.eQueryTypeAllAlbumArtists;

                    var stArtists = m_dataRoot.Library.GetArtistItemsAsync(int.MaxValue, 0);
                    await foreach (var stPlayableArtist in stArtists)
                    {
                        var stArtist = stPlayableArtist as IArtist;

                        UriImage image = null;
                        if (stArtist != null)
                        {
                            var stImage = await stArtist.GetImagesAsync(1, 0).FirstOrDefaultAsync();
                            if (stImage != null && stImage.Sources.Count > 0)
                            {
                                var imageSource = stImage.Sources[0];
                                using var imageStream = await imageSource.OpenStreamAsync();
                                var imageBytes = await imageStream.ToBytesAsync();

                                BytesResource imageResource = new(stArtist.Id, imageBytes, stArtist.Id, false);
                                image = new(imageResource, stArtist.Id + "_img", Inset.Zero, new Size(int.MaxValue, int.MaxValue), true);
                            }
                        }

                        Schemas.Artist artist = new(m_dataType)
                        {
                            Copyright = "©",
                            DateLastPlayed = stPlayableArtist.LastPlayed ?? DateTime.Now,
                            DeviceFileSize = 0L,
                            DrmStateMask = 0,
                            Image = image,
                            LibraryId = stPlayableArtist.Id.HashToInt32(),
                            NumberOfAlbums = stArtist?.TotalAlbumItemsCount ?? 0,
                            QuickMixState = 0,
                            SyncState = 0,
                            Title = stPlayableArtist.Name,
                            Type = "artist",
                            UserRating = 0,
                            ZuneMediaId = stPlayableArtist.Id.HashToGuid()
                        };

                        queryList.Add(artist.Item);
                    }
                }
                else if (text == "Genres")
                {
                    if (GetProperty("MediaType") is not int genreMediaType)
                        genreMediaType = 3;

                    queryType = EQueryType.eQueryTypeAllGenres;

                    // Strix doesn't support getting a list of genres, so we'll
                    // instead get a list of all children and create a set of
                    // genres from that.
                    var stChildren = m_dataRoot.Library.GetTracksAsync(int.MaxValue, 0);
                    HashSet<string> knownGenres = new();
                    await foreach (var stChild in stChildren)
                    {
                        if (stChild is not IGenreCollection stGenreCollection)
                            continue;

                        var genreNames = stGenreCollection.GetGenresAsync(int.MaxValue, 0)
                            .Select(g => g.Name);
                        await foreach (var genreName in genreNames)
                        {
                            if (knownGenres.Contains(genreName))
                                continue;

                            knownGenres.Add(genreName);
                            Schemas.Genre genre = new(m_dataType)
                            {
                                LibraryId = genreName.HashToInt32(),
                                Title = genreName,
                                SyncState = 0,
                                Type = "genre",
                                DeviceFileSize = 0L,
                                Copyright = "©",
                                DateLastPlayed = DateTime.Now
                            };

                            queryList.Add(genre.Item);
                        }
                    }
                }
                else if (text == "Album")
                {
                    if (TryGetProperty("ArtistId", out artistId) && artistId != -1)
                        queryType = EQueryType.eQueryTypeAlbumsForAlbumArtistId;
                    else if (TryGetProperty("GenreId", out genreId) && genreId != -1)
                        queryType = EQueryType.eQueryTypeAlbumsByGenreId;
                    else
                        queryType = EQueryType.eQueryTypeAllAlbums;

                    retainedList = true;

                    var stAlbums = m_dataRoot.Library.GetAlbumItemsAsync(int.MaxValue, 0);
                    await foreach (var stAlbumItem in stAlbums)
                    {
                        var stAlbum = stAlbumItem as IAlbum;
                        var stAlbumArtist = stAlbum != null
                            ? await stAlbum.GetArtistItemsAsync(int.MaxValue, 0).FirstOrDefaultAsync()
                            : null;

                        // Handle filters
                        var currentArtistId = stAlbumArtist?.Id.HashToInt32() ?? -1;
                        if (queryType == EQueryType.eQueryTypeAlbumsForAlbumArtistId && currentArtistId != artistId)
                        {
                            // Ignore all other artists
                            continue;
                        }
                        else if (queryType == EQueryType.eQueryTypeAlbumsByGenreId)
                        {
                            var containsCurrentGenre = await stAlbum.GetGenresAsync(int.MaxValue, 0)
                                .Select(g => g.Name.HashToInt32())
                                .AnyAsync(g => g == genreId);

                            if (!containsCurrentGenre)
                            {
                                // If the album doesn't have the genre, the tracks might.
                                containsCurrentGenre = await stAlbum.GetTracksAsync(int.MaxValue, 0)
                                    .SelectMany(t => t.GetGenresAsync(int.MaxValue, 0))
                                    .Select(g => g.Name.HashToInt32())
                                    .AnyAsync(g => g == genreId);

                                if (!containsCurrentGenre) continue;
                            }
                        }

                        Schemas.Album album = new(m_dataType)
                        {
                            ArtistLibraryId = currentArtistId,
                            ArtistName = stAlbumArtist?.Name,
                            DateAdded = DateTime.Now,
                            ContributingArtistCount = 0,
                            Copyright = "",
                            DateLastPlayed = stAlbumArtist.LastPlayed ?? DateTime.Now,
                            DeviceFileSize = 0L,
                            DisplayArtistCount = 0,
                            DrmStateMask = 0,
                            ReleaseDate = stAlbum.DatePublished ?? DateTime.Now,
                            HasAlbumArt = true,
                            LibraryId = stAlbumItem.Id.HashToInt32(),
                            QuickMixState = 0,
                            SyncState = 0,
                            ThumbnailPath = null,
                            Title = stAlbumItem.Name,
                            Type = "album",
                            ZuneMediaId = stAlbumItem.Id.HashToGuid(),
                        };

                        UriImage image = null;
                        if (stAlbum != null)
                        {
                            var stImage = await stAlbum.GetImagesAsync(1, 0).FirstOrDefaultAsync();
                            if (stImage != null && stImage.Sources.Count > 0)
                            {
                                var imageSource = stImage.Sources[0];
                                using var imageStream = await imageSource.OpenStreamAsync();
                                var imageBytes = await imageStream.ToBytesAsync();

                                BytesResource imageResource = new(stAlbum.Id, imageBytes, stAlbum.Id, false);
                                image = new(imageResource, stAlbum.Id + "_img", Inset.Zero, new Size(int.MaxValue, int.MaxValue), true);
                            }
                        }
                        album.AlbumArtSmall = album.AlbumArtLarge = album.AlbumArtSuperLarge = image;

                        queryList.Add(album.Item);
                    }
                }
                else if (text == "Track")
                {
                    if (GetProperty("RulesOnly") is bool queryAllTracks)
                        queryType = EQueryType.eQueryTypeAllTracks;

                    int albumArtistId = -1;
                    bool detailed = false;
                    if (TryGetProperty("AlbumId", out albumId) && albumId != -1)
                    {
                        if (TryGetProperty("ArtistId", out albumArtistId) && albumArtistId != -1)
                            queryType = EQueryType.eQueryTypeTracksForAlbumArtistId;
                        else
                            queryType = EQueryType.eQueryTypeTracksForAlbumId;
                    }
                    else
                    {
                        if (TryGetProperty("ArtistId", out artistId) && artistId != -1)
                        {
                            queryType = EQueryType.eQueryTypeTracksForAlbumArtistId;
                        }
                        else
                        {
                            if (TryGetProperty("GenreId", out genreId) && genreId != -1)
                            {
                                queryType = EQueryType.eQueryTypeTracksByGenreId;
                            }
                            else
                            {
                                if (TryGetProperty("Detailed", out detailed) && detailed)
                                {
                                    queryType = EQueryType.eQueryTypeAllTracksDetailed;
                                }
                                else
                                {
                                    if (TryGetProperty("TOC", out toc) && !string.IsNullOrEmpty(toc))
                                        queryType = EQueryType.eQueryTypeTracksForTOC;
                                    else
                                        queryType = EQueryType.eQueryTypeAllTracks;
                                }
                            }
                        }
                    }

                    var stTracks = m_dataRoot.Library.GetTracksAsync(int.MaxValue, 0);
                    
                }
                else if (text == "AlbumByTOC")
                {
                    queryType = EQueryType.eQueryTypeAlbumsByTOC;
                    //if (!queryPropertyBag.IsSet("TOC"))
                    //{
                    //	equeryType = EQueryType.eQueryTypeInvalid;
                    //}
                }
                else if (text == "Photo")
                {
                    queryType = (equeryTypeView == EQueryTypeView.eQueryTypeDeviceSyncRuleView)
                        ? EQueryType.eQueryTypeAllPhotos
                        : EQueryType.eQueryTypePhotosByFolderId;
                }
                else if (text == "MediaFolder")
                {
                    queryType = EQueryType.eQueryTypeMediaFolders;
                }
                else if (text == "Video")
                {
                    if (TryGetProperty("CategoryId", out categoryId) && categoryId != -1)
                        queryType = EQueryType.eQueryTypeVideosByCategoryId;
                    else
                        queryType = EQueryType.eQueryTypeAllVideos;
                }
                else if (text == "PodcastSeries")
                {
                    queryType = EQueryType.eQueryTypeAllPodcastSeries;
                }
                else if (text == "PodcastEpisode")
                {
                    if (TryGetProperty("SeriesId", out seriesId) && seriesId != -1)
                        queryType = EQueryType.eQueryTypeEpisodesForSeriesId;
                    else
                        queryType = EQueryType.eQueryTypeAllPodcastEpisodes;
                }
                else if (text == "SyncItem")
                {
                    queryType = EQueryType.eQueryTypeSyncProgress;
                }
                else if (text == "Playlist")
                {
                    queryType = EQueryType.eQueryTypeAllPlaylists;
                }
                else if (text == "PlaylistContent")
                {
                    queryType = EQueryType.eQueryTypePlaylistContentByPlaylistId;
                }
                else if (text == "UserCard")
                {
                    queryType = EQueryType.eQueryTypeUserCards;
                }
                else if (text == "Person")
                {
                    queryType = EQueryType.eQueryTypePersonsByTypeId;

                    EMediaTypes personType = EMediaTypes.eMediaTypePersonArtist;
                    personType = ((string)GetProperty("PersonType") == "Composer")
                        ? EMediaTypes.eMediaTypePersonComposer
                        : personType;
                }
                else if (text == "ArtistsRanking")
                {
                    queryType = EQueryType.eQueryTypeArtistsRanking;
                }
                else if (text == "TVSeries")
                {
                    queryType = EQueryType.eQueryTypeVideoSeriesTitles;
                }
                else if (text == "Pin")
                {
                    queryType = EQueryType.eQueryTypePinsByPinType;

                    if (!TryGetProperty("PinType", out EPinType pinType))
                        pinType = EPinType.ePinTypeGeneric;

                    var pinnedTracks = m_dataRoot.Library.GetTracksAsync(3, 0);

                    Schemas.Pin pin = new(m_dataType)
                    {
                        PinId = 42,
                        PinType = 1,
                        Ordinal = 1,
                        Description = "An OpenZune pin",
                        MediaId = 1,
                        MediaType = 1,
                        ZuneMediaRef = "ZuneSomethingHehe",
                        UserId = 0,
                        DateModified = DateTime.Now,
                        ZuneMediaType = 0,
                    };
                    //queryList.Add(pin.Item);
                }
                else if (text == "App")
                {
                    queryType = EQueryType.eQueryTypeAllApps;
                }

                queryList.QueryType = queryType;
                DeferredSetResult(new DeferredSetResultArgs(num, queryList, false));// retainedList));
            }
        }

        private void DeferredSetResult(object state)
        {
            DeferredSetResultArgs deferredSetResultArgs = (DeferredSetResultArgs)state;
            bool isEmpty = true;
            if (deferredSetResultArgs.QueryList != null)
            {
                if (!TryGetProperty("AutoRefresh", out bool autoRefresh))
                    autoRefresh = true;

                if (!TryGetProperty("AntialiasImageEdges", out bool antialiasEdges))
                    antialiasEdges = false;
                
                m_thumbnailFallbackImageUrl = (string)GetProperty("ThumbnailFallbackImageUrl");

                (m_virtualListResultSet as IDisposable)?.Dispose();

                m_virtualListResultSet = new(this, deferredSetResultArgs.QueryList, autoRefresh, antialiasEdges)
                {
                    VisualReleaseBehavior = deferredSetResultArgs.RetainedList
                        ? ReleaseBehavior.KeepReference
                        : ReleaseBehavior.ReleaseReference
                };

                isEmpty = deferredSetResultArgs.QueryList.IsEmpty;
            }

            StrixLibraryDataProviderQueryResult libraryDataProviderQueryResult = new(this, m_virtualListResultSet, ResultTypeCookie);
            libraryDataProviderQueryResult.SetIsEmpty(isEmpty);
            Result = libraryDataProviderQueryResult;
            Status = DataProviderQueryStatus.Complete;
        }

        private Schemas.Track CreateTrack(ZuneQueryList queryList, )
        {
            await foreach (var stTrack in stTracks)
            {
                var stAlbum = stTrack.Album;
                var stAlbumArtist = stAlbum != null
                    ? await stAlbum.GetArtistItemsAsync(1, 0).FirstOrDefaultAsync()
                    : null;

                // Handle filters
                if (queryType == EQueryType.eQueryTypeTracksForAlbumId)
                {
                    var currentAlbumId = stAlbum?.Id.HashToInt32();
                    if (currentAlbumId != albumId) continue;
                }
                else if (queryType == EQueryType.eQueryTypeTracksForAlbumArtistId)
                {
                    var currentAlbumArtistId = stAlbumArtist?.Id.HashToInt32();
                    if (currentAlbumArtistId != albumArtistId) continue;
                }
                else if (queryType == EQueryType.eQueryTypeTracksByGenreId)
                {
                    var containsCurrentGenre = await stTrack.GetGenresAsync(int.MaxValue, 0)
                        .Select(g => g.Name.HashToInt32())
                        .AnyAsync(g => g == genreId);

                    if (!containsCurrentGenre && stAlbum != null)
                    {
                        // If the track doesn't have the genre, the album might.
                        containsCurrentGenre = await stAlbum.GetGenresAsync(int.MaxValue, 0)
                            .Select(g => g.Name.HashToInt32())
                            .AnyAsync(g => g == genreId);
                    }

                    if (!containsCurrentGenre) continue;
                }

                Schemas.Track track = new(m_dataType)
                {
                    AlbumName = stTrack.Album?.Name,
                    AlbumLibraryId = stTrack.Album?.Id.HashToInt32() ?? 0,

                    TitleYomi = stTrack.Name,
                    ReleaseDate = DateTime.Now,
                    ZuneMediaId = stTrack.Id.HashToGuid(),
                    TrackNumber = stTrack.TrackNumber ?? 0,
                    Duration = stTrack.Duration,
                    ComponentId = "strix",
                    FileName = System.IO.Path.GetFileName(stTrack.Id),
                    FolderName = System.IO.Path.GetDirectoryName(stTrack.Id),
                    FilePath = stTrack.Id,
                    NowPlaying = false,
                    InLibrary = true,
                    DateLastPlayed = stTrack.LastPlayed ?? DateTime.MinValue,
                    PlayCount = 0,
                    FileSize = 0,
                    ComposerName = null,
                    ConductorName = null,
                    IsProtected = 0,
                    DateAdded = stTrack.AddedAt ?? DateTime.Now,
                    Bitrate = 0,
                    MediaType = "track",
                    DiscNumber = stTrack.DiscNumber ?? 0,
                    DateAlbumAdded = DateTime.Now,
                    FileCount = 1L,
                    DrmState = 0,
                    DrmStateMask = 0L,
                    QuickMixState = 0,

                    LibraryId = stTrack.Id.HashToInt32(),
                    Title = stTrack.Name,
                    SyncState = 0,
                    Type = "track",
                    DeviceFileSize = 0L,

                    UserRating = 0,

                    AlbumArtistName = stAlbumArtist?.Name,
                    AlbumArtistLibraryId = stAlbumArtist?.Id.HashToInt32() ?? 0
                };

                var stArtists = stTrack.GetArtistItemsAsync(int.MaxValue, 0);
                bool setPrimaryArtist = false;
                List<string> contributingArtists = new();
                await foreach (var stArtist in stArtists)
                {
                    if (!setPrimaryArtist)
                    {
                        track.ArtistLibraryId = stArtist.Id.HashToInt32();
                        track.ArtistName = stArtist.Name;
                        track.ArtistNameYomi = stArtist.Name;
                        setPrimaryArtist = true;
                    }
                    else if (!string.IsNullOrEmpty(stArtist.Name))
                    {
                        contributingArtists.Add(stArtist.Name);
                    }
                }
                track.ContributingArtistCount = contributingArtists.Count;
                track.ContributingArtistNames = contributingArtists;

                // No artists
                if (!setPrimaryArtist)
                {
                    track.ArtistLibraryId = 0;
                    track.ArtistName = null;
                    track.ArtistNameYomi = null;
                }

                var stGenre = await stTrack.GetGenresAsync(1, 0).FirstOrDefaultAsync();
                track.Genre = stGenre?.Name;

                track.Item.Storage["StrixItem"] = stTrack;
                queryList.Add(track.Item);
            }
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
