using System;

namespace Microsoft.Zune.Service;

// Original wraps a native IUriResourceTracker*. Not reverse engineered — see
// logs/Microsoft.Zune/Service/HttpWebRequest.md.
public class UriResourceTracker : IDisposable
{
    private static UriResourceTracker m_singletonInstance;

    public static UriResourceTracker Instance => m_singletonInstance ??= new UriResourceTracker();

    private UriResourceTracker()
    {
    }

    public bool SetResourceModified(string strUrlResource, bool fModified) => false;

    public bool IsResourceModified(string strUrlResource) => false;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
