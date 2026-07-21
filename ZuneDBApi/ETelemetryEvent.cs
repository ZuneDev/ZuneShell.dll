// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum ETelemetryEvent
{
    eTelemetryEventUndefined = -1,
    eTelemetryEventCDBurn = 0,
    eTelemetryEventBuildPlaylist = 1,
    eTelemetryEventMixView = 2,
    eTelemetryEventQuickMix = 3,
    eTelemetryEventNowPlaying = 4,
    eTelemetryEventDeviceConnect = 5,
    eTelemetryEventSyncStart = 6,
    eTelemetryEventMediaSyncComplete = 7,
    eTelemetryEventPlayback = 8,
    eTelemetryEventDTODownload = 9,
    eTelemetryEventRentalDownload = 10,
    eTelemetryEventManualDownload = 11,
    eTelemetryEventZunePassDownload = 12,
    eTelemetryEventSignIn = 13,
    eTelemetryEventAccountCreate = 14,
    eTelemetryEventZunePassPurchase = 15,
    eTelemetryEventSubscriptionAdd = 16,
    eTelemetryEventDownloadSuccess = 17,
    eTelemetryEventDownloadFailure = 18,
    eTelemetryEventFulfillmentFailure = 19,
    eTelemetryEventConcurrentStreamingDenied = 20,
}
