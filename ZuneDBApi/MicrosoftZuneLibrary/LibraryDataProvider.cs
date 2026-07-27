using System;
using System.Collections;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary;

public class LibraryDataProvider
{
    public static void Register()
    {
        Application.RegisterDataProvider("Library", ConstructQuery);
    }

    public static DataProviderQuery ConstructQuery(object queryTypeCookie) => new LibraryDataProviderQuery(queryTypeCookie);

    // Original also updates sync rules / performs the actual library mutation for each
    // BulkItemAction via the native database — not reverse engineered (see
    // logs/MicrosoftZuneLibrary/ZuneLibrary.md), so this always reports failure.
    public static bool ActOnItems(IList mediaList, BulkItemAction action, EventArgs args) => false;

    // Pure switch over the Iris markup type-schema name; transcribed verbatim from the
    // original decompiled body (no native dependency).
    public static EMediaTypes NameToMediaType(string typeName) => typeName switch
    {
        "Artist" => EMediaTypes.eMediaTypePersonArtist,
        "Album" => EMediaTypes.eMediaTypeAudioAlbum,
        "Track" => EMediaTypes.eMediaTypeAudio,
        "Photo" => EMediaTypes.eMediaTypeImage,
        "Video" => EMediaTypes.eMediaTypeVideo,
        "PodcastSeries" => EMediaTypes.eMediaTypePodcastSeries,
        "PodcastEpisode" => EMediaTypes.eMediaTypePodcastEpisode,
        "Playlist" => EMediaTypes.eMediaTypePlaylist,
        "MediaFolder" => EMediaTypes.eMediaTypeFolder,
        "Genre" => EMediaTypes.eMediaTypeGenre,
        "UserCard" => EMediaTypes.eMediaTypeUserCard,
        "App" => EMediaTypes.eMediaTypeApp,
        _ => (EMediaTypes)(-1),
    };

    public static bool GetSortAttributes(string sortString, out string[] sorts, out bool[] ascendings)
    {
        sorts = Array.Empty<string>();
        ascendings = Array.Empty<bool>();
        return false;
    }
}
