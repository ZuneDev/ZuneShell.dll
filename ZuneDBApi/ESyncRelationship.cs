// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum ESyncRelationship
{
    srNone = 0,
    srManual_Deprecated = 1,
    srSyncWithThisMachine = 2,
    srSyncWithOtherMachine = 3,
}
