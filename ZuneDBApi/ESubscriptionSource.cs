// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum ESubscriptionSource
{
    eSubscriptionSourceZMP = 0,
    eSubscriptionSourceProtocolHandler = 1,
    eSubscriptionSourceUrl = 2,
    eSubscriptionSourceInbox = 3,
    eSubscriptionSourceInternal = 4,
    eSubscriptionSourceUnitTest = 5,
}
