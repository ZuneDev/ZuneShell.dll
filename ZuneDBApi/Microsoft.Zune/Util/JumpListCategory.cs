using System;

namespace Microsoft.Zune.Util;

// Original wraps a native IJumpListCategory* (Windows 7 taskbar jump list). Not
// reverse engineered — see logs/Microsoft.Zune/Util/DownloadManager.md.
public class JumpListCategory : IDisposable
{
    public string Name { get; set; }

    internal JumpListCategory()
    {
    }

    public int CreateDestination(out JumpListEntry destination)
    {
        destination = null;
        return 0;
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
