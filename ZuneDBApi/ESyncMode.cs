// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum ESyncMode
{
    eSyncModeInvalid = -1,
    eSyncModeSyncAll = 0,
    eSyncModeSyncSelected = 1,
    eSyncModeManual = 2,
    eSyncModeCount = 3,
}
