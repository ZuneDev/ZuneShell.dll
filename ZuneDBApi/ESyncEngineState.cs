public enum ESyncEngineState
{
	sesComplete = 20,
	sesMetering = 19,
	sesDirectSyncComplete = 18,
	sesDirectSyncDownloading = 17,
	sesDirectSyncCalculating = 16,
	sesDirectSyncNotStarted = 15,
	sesWaitingForDownload = 14,
	sesWaitingForServiceNotification = 13,
	sesWaitingForLicenseAcquisition = 12,
	sesWaitingForTranscode = 11,
	sesDeletingFilesByUserRequest = 10,
	sesDeletingFilesNotInSyncSet = 9,
	sesAcquiredContentRetrieval = 8,
	sesInboxSync = 7,
	sesUpdatingContent = 6,
	sesTransferringFileFromDevice = 5,
	sesTransferringFile = 4,
	sesBuildingList = 3,
	sesRunningRules = 2,
	sesBeginning = 1,
	sesInitial = 0
}
