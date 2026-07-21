using System;

namespace MicrosoftZuneLibrary;

// Original wraps a native IFirmwareMetadata*. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/FirmwareUpdater.md.
public class FirmwareUpdatePackage : IDisposable
{
    internal bool Selected { get; set; }

    public TimeSpan UpdateEstimatedTime => TimeSpan.Zero;

    public string MoreInfoURL => null;

    public string EULAContent => null;

    public string Version => null;

    public string Description => null;

    public string Name => null;

    public FirmwareUpdateType Type => FirmwareUpdateType.Undefined;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
