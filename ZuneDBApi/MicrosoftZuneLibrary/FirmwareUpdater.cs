using System;
using Microsoft.Iris;
using ZuneUI;

namespace MicrosoftZuneLibrary;

// Original wraps a native IFirmwareUpdater*. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/FirmwareUpdater.md.
public class FirmwareUpdater : FirmwareOperationBase
{
    public FirmwareRestorer Restorer { get; } = new();

    internal FirmwareUpdater()
    {
    }

    public UpdateAction IsUpdateInProgress() => UpdateAction.None;

    public bool RequiresSyncBeforeUpdate() => false;

    public HRESULT StartCheckForDiskSpace(DeferredInvokeHandler checkForDiskSpaceComplete)
    {
        checkForDiskSpaceComplete?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL;
    }

    public HRESULT StartCheckForUpdates(bool fForceServerRequest, DeferredInvokeHandler checkForUpdatesComplete)
    {
        checkForUpdatesComplete?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL;
    }

    public HRESULT StartFirmwareUpdate(UpdatePackageCollection updates, DeferredInvokeHandler firmwareUpdateBegin, DeferredInvokeHandler firmwareUpdateProgress, DeferredInvokeHandler firmwareUpdateComplete, FirmwareUpdateOption updateOption)
    {
        firmwareUpdateComplete?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL;
    }

    public HRESULT Cancel() => HRESULT._E_FAIL;
}
