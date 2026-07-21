using System;

namespace Microsoft.Zune.Util;

// Original wraps a native IAsyncCallback* advised for feature-flag change
// notifications. Not reverse engineered — see logs/Microsoft.Zune/Util/FeatureEnablement.md.
public class FeaturesChangedApi : IDisposable
{
    private static FeaturesChangedApi m_singletonInstance;

    public static FeaturesChangedApi Instance => m_singletonInstance ??= new FeaturesChangedApi();

    public static bool HasInstance => m_singletonInstance != null;

    public event FeaturesChangedHandler OnFeaturesChangedEvent;

    private FeaturesChangedApi()
    {
    }

    public void FeaturesHaveChanged(bool fFeaturesHaveChanged)
    {
        OnFeaturesChangedEvent?.Invoke(fFeaturesHaveChanged);
    }

    public void FeaturesHaveChangedAsync(object args)
    {
        FeaturesHaveChanged(args is bool b && b);
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
