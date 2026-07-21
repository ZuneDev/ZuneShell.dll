using System;

namespace Microsoft.Zune.Util;

// Original wraps a native IThumbBarButton* (Windows 7 taskbar thumbnail toolbar
// button). Not reverse engineered — see logs/Microsoft.Zune/Util/DownloadManager.md.
public class ThumbBarButton : IDisposable
{
    public bool ShowBackground { get; set; }
    public bool IsHidden { get; set; }
    public bool IsEnabled { get; set; }
    public ThumbBarButtonIcons Icon { get; set; }
    public string Tooltip { get; set; }
    public uint UniqueID { get; set; }

    internal ThumbBarButton()
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
