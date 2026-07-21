// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum ESyncCategory
{
    eSyncCategoryInvalid = -1,
    eSyncCategoryMusic = 0,
    eSyncCategoryVideo = 1,
    eSyncCategoryPhotos = 2,
    eSyncCategoryPodcasts = 3,
    eSyncCategoryFriends = 4,
    eSyncCategoryAudiobooks = 5,
    eSyncCategoryChannels = 6,
    eSyncCategoryApps = 7,
    eSyncCategoryGuest = 8,
    eSyncCategoryInbox = 9,
    eSyncCategoryArtists = 10,
    eSyncCategoryCount = 11,
}
