using System;

namespace Microsoft.Zune.Util;

// Original wraps a native IThumbBar* (Windows 7 taskbar thumbnail toolbar). Not
// reverse engineered — see logs/Microsoft.Zune/Util/DownloadManager.md.
public class ThumbBar : IDisposable
{
    internal ThumbBar()
    {
    }

    public int CreateButton(out ThumbBarButton button)
    {
        button = null;
        return 0;
    }

    public int UpdateThumbBar() => 0;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
