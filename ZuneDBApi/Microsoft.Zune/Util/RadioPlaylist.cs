using System;

namespace Microsoft.Zune.Util;

// Original wraps a native IRadioPlaylist*, not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md. No public constructor exists in the
// original either (only ever handed out by native radio/QuickMix code), so this is
// internal-only until that native layer exists.
public class RadioPlaylist : IDisposable
{
    internal RadioPlaylist()
    {
    }

    public string GetNextUri() => null;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~RadioPlaylist()
    {
        Dispose(false);
    }
}
