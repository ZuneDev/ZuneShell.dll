using System;
using ZuneUI;

namespace MicrosoftZuneLibrary;

// Shared base of FirmwareUpdater/FirmwareRestorer, wrapping a native IEndpointHost*
// firmware-process state machine. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/FirmwareUpdater.md. Only the public surface UIFirmwareUpdater.cs/
// UIFirmwareRestorer.cs actually call is reconstructed; the original's internal
// process-continuation plumbing (ContinueFirmwareProcess, ReleaseNativeObject,
// InternalReset, etc.) is dropped since nothing outside MicrosoftZuneLibrary reaches it.
public abstract class FirmwareOperationBase : IDisposable
{
    public bool EnterContinuousPowerMode() => false;

    public bool LeaveContinuousPowerMode() => false;

    public virtual bool IsValid() => false;

    public HRESULT CheckPowerRequirements(out bool fOnBatteryPower)
    {
        fOnBatteryPower = false;
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
