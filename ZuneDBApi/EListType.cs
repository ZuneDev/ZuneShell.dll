// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale). Placed at the project root rather
// than under a namespace-mapped folder for that reason.
public enum EListType
{
    eListInvalid = -1,
    eArtistList = 0,
    eAlbumList = 1,
    eTrackList = 2,
    ePhotoList = 3,
    eVideoList = 4,
    eDeviceList = 5,
    ePodcastList = 6,
    ePodcastEpisodeList = 7,
    eDeviceFirmwareList = 8,
    eFileList = 9,
    eFolderList = 10,
    eDeviceContentList = 11,
    ePlaylistList = 12,
    ePlaylistContentList = 13,
    eDiscMediaContentList = 14,
    eGenreList = 15,
    eSubscriptionList = 16,
    eUserList = 17,
    eUserCardList = 18,
    eUserCardPlaylistList = 19,
    eAppList = 20,
    ePinList = 21,
    eLicenseList = 22,
    eListTypeCount = 23,
}
