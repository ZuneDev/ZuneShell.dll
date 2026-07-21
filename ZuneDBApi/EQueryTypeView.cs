// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale). Placed at the project root rather
// than under a namespace-mapped folder for that reason.
public enum EQueryTypeView
{
    eQueryTypeInvalidView = -1,
    eQueryTypeLibraryView = 0,
    eQueryTypeDeviceView = 16777216,
    eQueryTypeRemovableMediaView = 33554432,
    eQueryTypeDeviceSyncRuleView = 50331648,
    eQueryTypeDiscMediaView = 67108864,
    eQueryTypeSyncRemaining = 83886080,
    eQueryTypeSyncSucceeded = 100663296,
    eQueryTypeSyncFailed = 117440512,
    eQueryTypeX360View = 134217728,
    eQueryTypeLibraryMultiSelectView = 150994944,
    eQueryTypeDeviceMultiSelectView = 167772160,
}
