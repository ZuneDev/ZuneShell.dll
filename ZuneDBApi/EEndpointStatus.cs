// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum EEndpointStatus
{
    eEndpointStatusUndefined = 0,
    eEndpointStatusNotPresent = 1,
    eEndpointStatusHidden = 2,
    eEndpointStatusConnected = 3,
    eEndpointStatusInUse = 4,
    eEndpointStatusAvailabilityPending = 5,
    eEndpointStatusAuthenticationFailed = 6,
    eEndpointStatusAuthenticationRequired = 7,
    eEndpointStatusAuthenticationCompleted = 8,
    eEndpointStatusDetectingProxy = 9,
    eEndpointStatusProxyDetectionCompleted = 10,
    eEndpointStatusAvailable = 11,
    eEndpointStatusEnumeratingContents = 12,
    eEndpointStatusEnumerationCompleted = 13,
    eEndpointStatusReadyForSync = 14,
    eEndpointStatusWPhoneIPDisabled = 15,
}
