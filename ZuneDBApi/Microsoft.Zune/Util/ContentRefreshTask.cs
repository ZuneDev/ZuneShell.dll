using System;
using ZuneUI;

namespace Microsoft.Zune.Util;

// Original wraps a native IContentRefreshTask*. Not reverse engineered — see
// logs/Microsoft.Zune/Util/DownloadManager.md.
public class ContentRefreshTask : IDisposable
{
    private static ContentRefreshTask sm_ContentRefreshTask;

    public static bool HasInstance => sm_ContentRefreshTask != null;

    public static ContentRefreshTask Instance => sm_ContentRefreshTask ??= new ContentRefreshTask();

    private ContentRefreshTask()
    {
    }

    public HRESULT StartContentRefresh(AsyncCompleteHandler completeHandler)
    {
        completeHandler?.Invoke(HRESULT._E_FAIL);
        return HRESULT._E_FAIL;
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
