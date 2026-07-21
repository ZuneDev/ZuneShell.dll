using System;

namespace MicrosoftZuneLibrary;

// Original wraps a native IWMPCDDeviceList*. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/ZuneLibrary.md.
public class ZuneLibraryCDDeviceList : IDisposable
{
    internal ZuneLibraryCDDeviceList()
    {
    }

    public event OnMediaChangedHandler MediaChangedHandler;

    public int Count => 0;

    public uint AddRef() => 1;

    public uint Release() => 0;

    public ZuneLibraryCDDevice GetItem(int idx)
    {
        throw new ArgumentOutOfRangeException(nameof(idx));
    }

    internal void OnMediaChanged(char driveLetter, bool fMediaPresent)
    {
        MediaChangedHandler?.Invoke(driveLetter, fMediaPresent);
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
