using System;
using ZuneUI;

namespace Microsoft.Zune.Util;

// Original wraps a native IFamilySettingsProvider*. Not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md.
public class FamilySettingsManager : IDisposable
{
    private static FamilySettingsManager sm_manager;

    public static FamilySettingsManager Instance => sm_manager ??= new FamilySettingsManager();

    private FamilySettingsManager()
    {
    }

    public HRESULT AddSetting(int nSettingId, int nUserId, string szRatingSystem, int nRatingLevel, bool fBlockUnrated, out int settingId)
    {
        settingId = -1;
        return HRESULT._E_FAIL;
    }

    public HRESULT GetSettingIdsForUser(int nUserId, out int[] rgSettingIds)
    {
        rgSettingIds = Array.Empty<int>();
        return HRESULT._E_FAIL;
    }

    public HRESULT GetSetting(int nSettingId, out string szRatingSystem, out int nRatingLevel, out bool fBlockUnrated)
    {
        szRatingSystem = null;
        nRatingLevel = 0;
        fBlockUnrated = false;
        return HRESULT._E_FAIL;
    }

    public HRESULT GetSettingForSystem(int nUserId, string szRatingSystem, out int nRatingLevel, out bool fBlockUnrated)
    {
        nRatingLevel = 0;
        fBlockUnrated = false;
        return HRESULT._E_FAIL;
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
