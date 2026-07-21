using System;

namespace MicrosoftZuneLibrary;

// Original wraps a native ISyncRulesView*. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/Device.md.
public class SyncRulesView : IDisposable
{
    internal SyncRulesView(GasGauge predictedGasGauge)
    {
        PredictedGasGauge = predictedGasGauge;
    }

    public GasGauge PredictedGasGauge { get; }

    public int Count => 0;

    public event SyncRulesViewItemUpdatedHandler ItemUpdatedEvent;
    public event SyncRulesViewItemAddedHandler ItemAddedEvent;

    public SyncRuleDetails GetItem(int index)
    {
        throw new ArgumentOutOfRangeException(nameof(index));
    }

    public void UpdateItem(int index, bool included)
    {
    }

    internal void InvokeItemAdded(uint iItem)
    {
        ItemAddedEvent?.Invoke(this, (int)iItem);
    }

    internal void InvokeItemUpdated(uint iItem)
    {
        ItemUpdatedEvent?.Invoke(this, (int)iItem);
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
