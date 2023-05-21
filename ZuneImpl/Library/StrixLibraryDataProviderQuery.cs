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

namespace Microsoft.Zune.Library
{
    public class StrixLibraryDataProviderQuery : DataProviderQuery
    {
        private LibraryVirtualList m_virtualListResultSet;
        private bool m_disposed = false;
        private int m_requestGeneration;
        private string m_thumbnailFallbackImageUrl;
        private MarkupTypeSchema m_dataType;

        private readonly IStrixDataRoot m_dataRoot;

        public StrixLibraryDataProviderQuery(object typeCookie, IStrixDataRoot dataRoot) : base(typeCookie)
        {
            m_dataRoot = dataRoot;
            m_dataType = (MarkupTypeSchema)((MarkupTypeSchema)ResultTypeCookie).Properties[0].AlternateType;

            if (Application.DebugSettings.GenerateDataMappingModels)
                GenerateModelCode(m_dataType);
        }

        private static bool IsBasicType(TypeSchema type) =>
            type == null || type.RuntimeType.Assembly.FullName.Contains("CoreLib") || type.Properties.Length == 0;

        private void GenerateModelCode(TypeSchema dataType)
        {
            string code =
                "using Microsoft.Iris.Markup;\r\n\r\n" +
                "namespace Microsoft.Zune.Schemas;\r\n\r\n" +
                "public class " + dataType.Name;

            if (!IsBasicType(dataType.Base))
            {
                code += $" : {dataType.Base.Name}";
                GenerateModelCode(dataType.Base);
            }
            else
            {
                code += $" : MarkupDataType";
            }

            code += "\r\n{\r\n";
            code += $"    public {dataType.Name}(MarkupTypeSchema schema) : base(schema)\r\n";
            code += "    {\r\n";
            code += "    }\r\n\r\n";

            foreach (var prop in dataType.Properties)
            {
                string propType = prop.PropertyType.AlternateName ?? prop.PropertyType.Name;

                code += $"    public {propType} {prop.Name}\r\n";
                code += "    {\r\n";
                code += $"        get => GetProperty<{propType}>();\r\n";
                code += $"        set => SetProperty(value);\r\n";
                code += "    }\r\n\r\n";

                if (!IsBasicType(prop.PropertyType) && !Application.DebugSettings.DataMappingModels.Any(m => m.Type == propType))
                    GenerateModelCode(prop.PropertyType);
            }

            code += "}";

            Application.DebugSettings.DataMappingModels.Add(new(dataType.RuntimeType.Name, dataType.Name, code));
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

                    var stArtists = m_dataRoot.Library.GetArtistItemsAsync(int.MaxValue, 0).ToEnumerable();
                    foreach (var stPlayableArtist in stArtists)
                    {
                        var stArtist = stPlayableArtist as IArtist;

                        UriImage image = null;
                        var stImage = stArtist?.GetImagesAsync(1, 0).ToEnumerable().FirstOrDefault();
                        if (stImage != null && stImage.Sources.Count > 0)
                        {
                            var imageSource = stImage.Sources[0];
                            using var imageStream = AsyncHelper.Run(imageSource.OpenStreamAsync);
                            var imageBytes = AsyncHelper.Run(imageStream.ToBytesAsync);

                            BytesResource imageResource = new(stArtist.Id, imageBytes, stArtist.Id, false);
                            image = new(imageResource, stArtist.Id + "_img", Inset.Zero, new Size(int.MaxValue, int.MaxValue), true);
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

                    equeryType = EQueryType.eQueryTypeAllGenres;

                    // Strix doesn't support getting a list of genres, so we'll
                    // instead get a list of all children and create a set of
                    // genres from that.
                    var stChildren = m_dataRoot.Library.GetTracksAsync(int.MaxValue, 0).ToEnumerable();
                    HashSet<string> knownGenres = new();
                    foreach (var stChild in stChildren)
                    {
                        if (stChild is not IGenreCollection stGenreCollection)
                            continue;

                        var genreNames = stGenreCollection.GetGenresAsync(int.MaxValue, 0)
                            .ToEnumerable().Select(g => g.Name);
                        foreach (var genreName in genreNames)
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
                    int artistId = -1, genreId = -1;
                    if (TryGetProperty("ArtistId", out artistId) && artistId != -1)// || queryPropertyBag.IsSet("ArtistIds"))
                        equeryType = EQueryType.eQueryTypeAlbumsForAlbumArtistId;
                    else if ((TryGetProperty("GenreId", out genreId) && genreId != -1))// || queryPropertyBag.IsSet("GenreIds"))
                        equeryType = EQueryType.eQueryTypeAlbumsByGenreId;
                    else
                        equeryType = EQueryType.eQueryTypeAllAlbums;

                    retainedList = true;

                    var stAlbums = m_dataRoot.Library.GetAlbumItemsAsync(int.MaxValue, 0).ToEnumerable();
                    foreach (var stAlbumItem in stAlbums)
                    {
                        var stAlbum = stAlbumItem as IAlbum;
                        var stAlbumArtist = stAlbum?.GetArtistItemsAsync(int.MaxValue, 0).ToEnumerable().FirstOrDefault();

                        // Handle filters
                        var currentArtistId = stAlbumArtist?.Id.HashToInt32() ?? -1;
                        if (equeryType == EQueryType.eQueryTypeAlbumsForAlbumArtistId && currentArtistId != artistId)
                        {
                            // Ignore all other artists
                            continue;
                        }
                        else if (equeryType == EQueryType.eQueryTypeAlbumsByGenreId)
                        {
                            var containsCurrentGenre = stAlbum.GetGenresAsync(int.MaxValue, 0)
                                .ToEnumerable()
                                .Select(g => g.Name.HashToInt32())
                                .Any(g => g == genreId);

                            if (!containsCurrentGenre)
                                continue;
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
                        var stImage = stAlbum?.GetImagesAsync(1, 0).ToEnumerable().FirstOrDefault();
                        if (stImage != null && stImage.Sources.Count > 0)
                        {
                            var imageSource = stImage.Sources[0];
                            using var imageStream = AsyncHelper.Run(imageSource.OpenStreamAsync);
                            var imageBytes = AsyncHelper.Run(imageStream.ToBytesAsync);

                            BytesResource imageResource = new(stAlbum.Id, imageBytes, stAlbum.Id, false);
                            image = new(imageResource, stAlbum.Id + "_img", Inset.Zero, new Size(int.MaxValue, int.MaxValue), true);
                        }
                        album.AlbumArtSmall = album.AlbumArtLarge = album.AlbumArtSuperLarge = image;

                        queryList.Add(album.Item);
                    }
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
                        };

                        var stAlbumArtist = stTrack.Album != null
                            ? AsyncHelper.Run(() => stTrack.Album.GetArtistItemsAsync(1, 0).FirstOrDefaultAsync().AsTask())
                            : null;
                        track.AlbumArtistName = stAlbumArtist?.Name;
                        track.AlbumArtistLibraryId = stAlbumArtist?.Id.HashToInt32() ?? 0;

                        var stArtists = stTrack.GetArtistItemsAsync(int.MaxValue, 0).ToEnumerable();
                        bool setPrimaryArtist = false;
                        List<string> contributingArtists = new();
                        foreach (var stArtist in stArtists)
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

                        var stGenre = AsyncHelper.Run(() => stTrack.GetGenresAsync(1, 0).FirstOrDefaultAsync().AsTask());
                        track.Genre = stGenre?.Name;

                        queryList.Add(track.Item);
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
                    queryList.Add(pin.Item);
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
