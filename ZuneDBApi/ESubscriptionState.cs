// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum ESubscriptionState
{
    eSubscriptionStateSubscribed = 0,
    eSubscriptionStateUnsubscribed = 1,
    eSubscriptionStateDormant_MigrationOnly = 3,
}
