// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace, like EMediaTypes.cs — see that
// file's header comment for the full rationale).
public enum EDeviceSyncRuleType
{
    eDeviceSyncRuleTypeInvalid = -1,
    eDeviceSyncRuleTypeIncludeAll = 0,
    eDeviceSyncRuleTypeExclude = 1,
    eDeviceSyncRuleTypeAllUnplayed = 2,
    eDeviceSyncRuleTypeFirstUnplayed = 3,
    eDeviceSyncRuleTypeNone = 4,
    eDeviceSyncRuleTypeSyncEpisodesCount = 5,
    eDeviceSyncRuleTypeCount = 6,
}
