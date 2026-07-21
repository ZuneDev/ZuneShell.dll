using System;

namespace Microsoft.Zune.Util;

// Original wraps a native IUpdateManager*. Not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md.
public class UpdateManager : IDisposable
{
    private static UpdateManager sm_updateManager;

    public static UpdateManager Instance => sm_updateManager ??= new UpdateManager();

    private UpdateManager()
    {
    }

    public void BeginUpdateCheck(UpdateProgressHandler updateProgressHandler)
    {
        updateProgressHandler?.Invoke(new UpdateCheckEventArguments(false, false, ZuneUI.HRESULT._E_FAIL.Int));
    }

    public void CancelUpdateCheck()
    {
    }

    public void InstallUpdate(UpdateProgressHandler updateProgressHandler)
    {
        updateProgressHandler?.Invoke(new UpdateCheckEventArguments(false, false, ZuneUI.HRESULT._E_FAIL.Int));
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
