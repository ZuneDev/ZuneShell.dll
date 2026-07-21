// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum ESyncEngineState
{
    sesInitial = 0,
    sesBeginning = 1,
    sesRunningRules = 2,
    sesBuildingList = 3,
    sesTransferringFile = 4,
    sesTransferringFileFromDevice = 5,
    sesUpdatingContent = 6,
    sesInboxSync = 7,
    sesAcquiredContentRetrieval = 8,
    sesDeletingFilesNotInSyncSet = 9,
    sesDeletingFilesByUserRequest = 10,
    sesWaitingForTranscode = 11,
    sesWaitingForLicenseAcquisition = 12,
    sesWaitingForServiceNotification = 13,
    sesWaitingForDownload = 14,
    sesDirectSyncNotStarted = 15,
    sesDirectSyncCalculating = 16,
    sesDirectSyncDownloading = 17,
    sesDirectSyncComplete = 18,
    sesMetering = 19,
    sesComplete = 20,
}
