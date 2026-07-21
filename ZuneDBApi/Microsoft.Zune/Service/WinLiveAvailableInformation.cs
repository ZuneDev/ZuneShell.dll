using System;
using System.Collections;

namespace Microsoft.Zune.Service;

// Original wraps a native IWinLiveAvailableInformation*. Not reverse engineered — see
// logs/Microsoft.Zune/Service/WinLiveSignup.md.
public class WinLiveAvailableInformation : IDisposable
{
    internal WinLiveAvailableInformation()
    {
    }

    public IList SuggestedNames { get; } = new ArrayList();

    public bool Available => false;

    public string SigninName => null;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
