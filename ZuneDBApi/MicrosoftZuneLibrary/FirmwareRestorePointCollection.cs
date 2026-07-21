using System;

namespace MicrosoftZuneLibrary;

// Original wraps a native IFirmwareRestorePointCollection*. Not reverse engineered —
// see logs/MicrosoftZuneLibrary/FirmwareUpdater.md.
public class FirmwareRestorePointCollection : IDisposable
{
    internal FirmwareRestorePointCollection()
    {
    }

    public int Count => 0;

    public FirmwareRestorePoint GetRestorePoint(int index)
    {
        throw new IndexOutOfRangeException(nameof(index));
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
