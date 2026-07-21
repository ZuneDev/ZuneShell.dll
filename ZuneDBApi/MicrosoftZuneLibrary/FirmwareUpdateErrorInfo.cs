using System;
using ZuneUI;

namespace MicrosoftZuneLibrary;

// Original wraps a native IFirmwareUpdateErrorInfo*. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/FirmwareUpdater.md.
public class FirmwareUpdateErrorInfo : IDisposable
{
    internal FirmwareUpdateErrorInfo()
    {
    }

    public string Url => null;

    public string Description => null;

    public HRESULT HrStatus => HRESULT._E_FAIL;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
