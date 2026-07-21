using System;

namespace MicrosoftZuneLibrary;

// Original wraps a native IGasGauge*/GasGaugeMediator pair reporting device storage
// usage. Not reverse engineered — see logs/MicrosoftZuneLibrary/Device.md.
public class GasGauge : IDisposable
{
    internal GasGauge()
    {
    }

    public ulong Capacity => 0;

    public long SpaceAvailable => 0;

    public long SpaceReserved => 0;

    public event DeviceOverflowHandler DeviceOverflowEvent;
    public event ReservedSpaceUpdatedHandler ReservedSpaceUpdatedEvent;
    public event CategorySpaceUsedUpdatedHandler CategorySpaceUsedUpdatedEvent;

    internal void CategorySpaceUsedUpdated(ESyncCategory syncCategory, long llNewSchemaSpace, long llNewFreeSpace)
    {
        CategorySpaceUsedUpdatedEvent?.Invoke(this, syncCategory, llNewSchemaSpace, llNewFreeSpace);
    }

    internal void ReservedSpaceUpdated(long llNewReservedSpace, long llNewFreeSpace)
    {
        ReservedSpaceUpdatedEvent?.Invoke(this, llNewReservedSpace, llNewFreeSpace);
    }

    internal void DeviceOverflow()
    {
        DeviceOverflowEvent?.Invoke(this);
    }

    public long GetSpaceUsedByCategory(ESyncCategory syncCategory) => 0;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
