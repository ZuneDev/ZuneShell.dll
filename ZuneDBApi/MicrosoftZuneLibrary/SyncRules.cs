using System;

namespace MicrosoftZuneLibrary;

// Original wraps a native IEndpointHost* to manage a device's sync rule set. Not
// reverse engineered — see logs/MicrosoftZuneLibrary/Device.md.
public class SyncRules : IDisposable
{
    internal SyncRules()
    {
    }

    public int Add(int[] rgIds, EMediaTypes mediaType) => unchecked((int)0x80004005);

    public int Add(int[] rgIds, EMediaTypes mediaType, EDeviceSyncRuleType ruleType) => unchecked((int)0x80004005);

    public int AddDeviceSyncRuleWithValue(int[] rgIds, int value) => unchecked((int)0x80004005);

    public int Remove(int[] rgIds, EMediaTypes mediaType, bool fDeviceFolderIds) => unchecked((int)0x80004005);

    public int Exclude(int[] rgIds, EMediaTypes mediaType) => unchecked((int)0x80004005);

    public int Unexclude(int[] rgIds, EMediaTypes mediaType) => unchecked((int)0x80004005);

    public int GetSyncRuleForMedia(EMediaTypes mediaType, int iMediaItemId, ref EDeviceSyncRuleType ruleType)
    {
        ruleType = EDeviceSyncRuleType.eDeviceSyncRuleTypeInvalid;
        return unchecked((int)0x80004005);
    }

    public int GetSyncRuleValueForMedia(int iMediaItemId, ref int iValue)
    {
        iValue = 0;
        return unchecked((int)0x80004005);
    }

    public int GetCategorySyncMode(ESyncCategory cat, ref ESyncMode mode, bool fEstablishingPartnership)
    {
        mode = ESyncMode.eSyncModeInvalid;
        return unchecked((int)0x80004005);
    }

    public int SetCategorySyncMode(ESyncCategory cat, ESyncMode mode) => unchecked((int)0x80004005);

    public int GetDontSyncHatedContent(ref bool fDontSyncHatedContent)
    {
        fDontSyncHatedContent = false;
        return unchecked((int)0x80004005);
    }

    public int SetDontSyncHatedContent(bool fDontSyncHatedContent) => unchecked((int)0x80004005);

    public int GenerateSnapshot(bool expandSyncAll, ref SyncRulesView syncRulesView)
    {
        syncRulesView = null;
        return unchecked((int)0x80004005);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
