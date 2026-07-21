using System;

namespace MicrosoftZuneLibrary;

// Original wraps a native IFirmwareRestorePoint*. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/FirmwareUpdater.md.
public class FirmwareRestorePoint : IDisposable
{
    internal FirmwareRestorePoint()
    {
    }

    public TimeSpan EstimatedRestoreTime => TimeSpan.Zero;

    public DateTime CreationDate => DateTime.MinValue;

    public string OSVersion => null;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
