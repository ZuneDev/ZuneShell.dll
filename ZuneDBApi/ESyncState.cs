// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum ESyncState
{
    eSyncStateInvalid = -1,
    eSyncStateCurrentlySyncing = 0,
    eSyncStateOnDevice = 1,
    eSyncStateExcluded = 2,
    eSyncStateNone = 3,
    eSyncStateCount = 4,
}
