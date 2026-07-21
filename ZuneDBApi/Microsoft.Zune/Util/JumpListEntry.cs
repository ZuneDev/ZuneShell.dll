using System;

namespace Microsoft.Zune.Util;

// Original wraps a native IJumpListEntry*. Not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md.
public class JumpListEntry : IDisposable
{
    public int IconIndex { get; set; }
    public string CommandLineArguments { get; set; }
    public string Name { get; set; }

    internal JumpListEntry()
    {
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
