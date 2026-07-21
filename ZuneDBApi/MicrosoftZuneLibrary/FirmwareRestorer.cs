using System;
using Microsoft.Iris;
using ZuneUI;

namespace MicrosoftZuneLibrary;

// Original wraps a native IFirmwareRestorer*. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/FirmwareUpdater.md.
public class FirmwareRestorer : FirmwareOperationBase
{
    internal FirmwareRestorer()
    {
    }

    public bool IsRestoreInProgress() => false;

    public HRESULT StartGetRestorePointCollection(DeferredInvokeHandler getCollectionComplete)
    {
        getCollectionComplete?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL;
    }

    public HRESULT StartFirmwareRestore(FirmwareRestorePoint restorePoint, DeferredInvokeHandler restoreBegin, DeferredInvokeHandler restoreProgress, DeferredInvokeHandler restoreComplete)
    {
        restoreComplete?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL;
    }

    public HRESULT Cancel() => HRESULT._E_FAIL;
}
