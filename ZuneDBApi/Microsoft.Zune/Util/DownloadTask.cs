using System;

namespace Microsoft.Zune.Util;

// Original wraps a native IDownloadTask* behind a DownloadTaskProxy helper (itself a
// native class with its own vtable, per ILSpy's decompiled call sites). Neither native
// type has been reverse engineered — see logs/Microsoft.Zune/Util/DownloadManager.md.
// Public surface preserved so callers keep compiling; state defaults reflect "no
// download exists" since nothing can construct a real one yet.
public class DownloadTask : IDisposable
{
    private string m_taskId;

    public event DownloadProgressHandler OnProgressChanged;

    internal DownloadTask(string taskId)
    {
        m_taskId = taskId;
    }

    public string GetTaskId() => m_taskId;

    public EDownloadTaskState GetState() => EDownloadTaskState.DLTaskNone;

    public float GetProgress() => 0f;

    public ulong GetBytesDownloaded() => 0;

    public int GetDownloadSecondsRemaining() => 0;

    public int GetDownloadFileSecondsRemaining(int iFile) => 0;

    public string GetTempFileName(int iFile) => null;

    public ulong GetFinalFileSize(int iFile) => 0;

    public int GetDownloadBytesPerSecond() => 0;

    public string GetProperty(string propertyName) => null;

    public int GetPropertyInt(string propertyName) => 0;

    public void SetProperty(string propertyName, string propertyValue)
    {
    }

    public bool CanCancel() => false;

    public void Cancel()
    {
    }

    public bool CanReorder() => false;

    public override bool Equals(object obj) => ReferenceEquals(this, obj);

    public override int GetHashCode() => base.GetHashCode();

    internal void DownloadNext()
    {
    }

    internal bool SetPosition(int position) => false;

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~DownloadTask()
    {
        Dispose(false);
    }
}
