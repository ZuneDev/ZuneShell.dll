using System;
using System.Collections;
using Microsoft.Iris;

namespace Microsoft.Zune.Util;

// Original wraps a native DownloadManagerProxy* (itself wrapping a native
// IDownloadManager COM interface) with worker-thread-driven queue management. Neither
// native type has been reverse engineered — see logs/Microsoft.Zune/Util/DownloadManager.md.
// Public surface preserved so callers keep compiling; the download queue is always
// empty since nothing can populate it yet.
public class DownloadManager : IDisposable
{
    private static DownloadManager sm_downloadManager;

    private ArrayListDataSet m_activeDownloadTasks = new();
    private ArrayListDataSet m_completedDownloadTasks = new();
    private ArrayListDataSet m_cancelledDownloadTasks = new();
    private ArrayListDataSet m_failedDownloadTasks = new();

    public event DownloadManagerUpdateHandler OnProgressChanged;

    public static DownloadManager Instance => sm_downloadManager ??= CreateInstance();

    public ArrayListDataSet CancelledDownloads => m_cancelledDownloadTasks;

    public ArrayListDataSet FailedDownloads => m_failedDownloadTasks;

    public ArrayListDataSet CompletedDownloads => m_completedDownloadTasks;

    public ArrayListDataSet ActiveDownloads => m_activeDownloadTasks;

    public float Percentage => 0f;

    public bool HadFailures => false;

    public bool HadCancellations => false;

    public bool IsQueuePaused { get; private set; }

    public bool Finished => true;

    public int ActiveItem => 0;

    public int TotalInProgressItems => 0;

    public int TotalItems => 0;

    private DownloadManager()
    {
    }

    public static DownloadManager CreateInstance() => new();

    public DownloadTask GetTask(string taskId) => null;

    public int SetPosition(IList list, int position) => position;

    public void DownloadNext(DownloadTask task)
    {
    }

    public void RemoveFailed(DownloadTask task)
    {
    }

    public void PauseQueue()
    {
        IsQueuePaused = true;
    }

    public void ResumeQueue()
    {
        IsQueuePaused = false;
    }

    public bool SignInRequired() => false;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~DownloadManager()
    {
        Dispose(false);
    }
}
