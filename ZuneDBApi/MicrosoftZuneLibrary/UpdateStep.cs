using System;

namespace MicrosoftZuneLibrary;

// Original wraps a native IFirmwareUpdateCallbackData*. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/FirmwareUpdater.md.
public class UpdateStep : IDisposable
{
    internal UpdateStep()
    {
    }

    public int TotalSteps => 0;

    public int StepNumber => 0;

    public int Progress => 0;

    public bool HasProgress => false;

    public bool Cancelable => false;

    public string Name => null;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
