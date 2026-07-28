using System;
using System.Collections;
using MicrosoftZuneInterop;
using ZuneUI;

namespace MicrosoftZuneLibrary;

// Original wraps the native Zune media library database engine in its entirety —
// every query, add/delete, folder, and CD-burning entry point the whole shell is built
// on. Not reverse engineered — see logs/MicrosoftZuneLibrary/ZuneLibrary.md. Every
// member below has its exact original signature but is a "pure no-op stub" per
// CLAUDE.md's stage 2 convention, except for the two pure string-comparison statics
// (CompareWithoutArticles is not itself pure-computable without knowing the original's
// article list, so it too stays a stub) — none of the string/file-system utility
// methods depend on the native database, but their original implementations are not
// visible to ILSpy either way (native-only), so all are left as documented stubs.
public class ZuneLibrary : IDisposable
{
    public int Initialize(string path, out bool dbRebuilt)
    {
        dbRebuilt = false;
        // return unchecked((int)0x80004005);
        return HRESULT._S_OK;
    }

    public bool Phase2Initialization(out int hr)
    {
        // hr = unchecked((int)0x80004005);
        hr = HRESULT._S_OK;
        return false;
    }

    public static string LoadStringFromResource(uint dwResourceNumber)
    {
        ZuneLibraryResources.Strings.TryGetValue(dwResourceNumber, out var result);
        return result ?? "STRING NOT FOUND!!!!";
    }

    public ZuneQueryList QueryDatabase(EQueryType queryType, int libraryView, EQuerySortType sortType, uint sortAtom, QueryPropertyBag propertyBag) => new();

    public ZuneLibraryCDDeviceList GetCDDeviceList() => new();

    public ZuneLibraryCDRecorder GetRecorder() => new();

    public ZuneQueryList GetTracksByArtist(int libraryView, int artistId, EQuerySortType sortOrder, uint sortAtom) => new();

    public ZuneQueryList GetTracksByAlbum(int libraryView, int albumId, EQuerySortType sortOrder, uint sortAtom) => new();

    public ZuneQueryList GetTracksByArtists(IList artistIds, string sort) => new();

    public ZuneQueryList GetTracksByGenres(IList genreIds, string sort) => new();

    public ZuneQueryList GetTracksByAlbums(IList albumIds, string sort) => new();

    public ZuneQueryList GetTracksByPlaylist(int libraryView, int playlistId, EQuerySortType sortOrder, uint sortAtom) => new();

    public ZuneQueryList GetAlbumsByArtists(IList artistIds, string sort) => new();

    public ZuneQueryList GetAlbumsByGenres(IList genreIds, string sort) => new();

    public AlbumMetadata GetAlbumMetadata(int iAlbumId) => null;

    public void UpdateAlbumMetadata(int iAlbumId, AlbumMetadata albumMetadata)
    {
    }

    public void GetAlbumMetadataForAlbumId(long wmisAlbumId, int wmisVolume, AlbumMetadata dbAlbumMetadata, GetAlbumForAlbumIdCompleteHandler handler)
    {
        handler?.Invoke(wmisAlbumId, wmisVolume, unchecked((int)0x80004005), dbAlbumMetadata);
    }

    public static void SplitAudioTrack(int iTrackMediaId)
    {
    }

    public bool CanAddFromFolder(string folder) => false;

    public bool CanAddMedia(string filename, EMediaTypes mediaType) => false;

    public bool AddGrovelerScanDirectory(string path, EMediaTypes mediaType) => false;

    public int AddMedia(string filename) => -1;

    public int AddTrack(Guid guidTrackServiceMediaId, Guid guidAlbumServiceMediaId, int iTrackNumber, string strTitle, TimeSpan duration, string strAlbum, string strArtist, string strGenre) => -1;

    public int AddVideo(Guid guidVideoMediaId, string strTitle, TimeSpan duration) => -1;

    public int AddAlbum(Guid guidServiceMediaId, string strAlbum, string strArtist) => -1;

    public bool AddTransientMedia(string filename, EMediaTypes mediaType, out int libraryID, out bool fFileAlreadyExists)
    {
        libraryID = -1;
        fFileAlreadyExists = false;
        return false;
    }

    public void CleanupTransientMedia()
    {
    }

    public void MarkAllDRMFilesAsNeedingLicenseRefresh()
    {
    }

    public bool DeleteMedia(int[] mediaIds, EMediaTypes mediaType, bool fDeleteFileOnDisk) => false;

    public bool DeleteRootFolder(string folderName, EMediaTypes mediaType) => false;

    public bool DeleteFilesystemFolder(int folderId, EMediaTypes mediaType) => false;

    public static void ScanAndClearDeletedMedia()
    {
    }

    public static bool ImportSharedRatingsForUser(int iUserId, EMediaTypes mediaType) => false;

    public static bool ExportUserRatings(int iUserId, EMediaTypes mediaType) => false;

    public static HRESULT GetFieldValues(int iMediaId, EListType eList, int cValues, int[] columnIndexes, object[] fieldValues, QueryPropertyBag propertyBag) => HRESULT._E_FAIL;

    public static HRESULT GetFieldValues(int iMediaId, EListType eList, int cValues, int[] columnIndexes, object[] fieldValues, bool[] isEmptyValues, QueryPropertyBag propertyBag) => HRESULT._E_FAIL;

    public static void SetFieldValues(int iMediaId, EListType eList, int cValues, int[] columnIndexes, object[] fieldValues, QueryPropertyBag propertyBag)
    {
    }

    public int GetKnownFolders(out string[] music, out string[] videos, out string[] pictures, out string[] podcasts, out string[] applications, out string ripFolder, out string videoMediaFolder, out string photoMediaFolder, out string podcastMediaFolder, out string applicationsFolder)
    {
        music = [
            Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic),
            Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
        ];
        videos = [
            Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos),
            Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
        ];
        pictures = [
            Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures),
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
        ];
        podcasts = Array.Empty<string>();
        applications = Array.Empty<string>();
        ripFolder = null;
        videoMediaFolder = null;
        photoMediaFolder = null;
        podcastMediaFolder = null;
        applicationsFolder = null;
        // return unchecked((int)0x80004005);
        return HRESULT._S_OK;
    }

    public int GetLocalizedPathOfFolder(string physicalPath, bool fNetworkPathsAllowed, out string localizedPath)
    {
        localizedPath = null;
        return unchecked((int)0x80004005);
    }

    public static int CompareWithoutArticles(string prefix, string value) => string.Compare(prefix, value, StringComparison.CurrentCultureIgnoreCase);

    public static bool DoesFileExist(string path) => File.Exists(path);

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
