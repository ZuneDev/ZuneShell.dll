// Decompiled with JetBrains decompiler
// Type: ZuneUI.DownloadCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;

namespace ZuneUI
{
    public abstract class DownloadCommand : ProgressCommand
    {
        private DownloadProgressHandler m_progressHandler;
        private EDownloadTaskState m_downloadState = EDownloadTaskState.DLTaskNone;
        private DownloadTask m_downloadTask;
        private string m_taskId;
        private int m_progress;
        private volatile object m_myLock;

        public string TaskId
        {
            set
            {
                this.m_taskId = value;
                this.Refresh();
            }
        }

        public DownloadCommand(IModelItem owner)
          : base(owner)
        {
            this.Progress = -1f;
            this.m_progressHandler = new DownloadProgressHandler(this.OnProgressChanged);
            this.m_myLock = new object();
        }

        public void Refresh()
        {
            lock (this.m_myLock)
            {
                this.m_downloadState = this.GetDownloadState();
                if (this.m_downloadState == EDownloadTaskState.DLTaskDownloading)
                    this.UpdateDownloadTask();
            }
            this.UpdateCommandState(null);
        }

        protected abstract EDownloadTaskState GetDownloadState();

        protected virtual string GetDownloadString(EDownloadTaskState downloadState)
        {
            string str;
            switch (downloadState)
            {
                case EDownloadTaskState.DLTaskPendingAttach:
                case EDownloadTaskState.DLTaskPending:
                    str = Shell.LoadString(StringId.IDS_PENDING);
                    break;
                case EDownloadTaskState.DLTaskDownloading:
                case EDownloadTaskState.DLTaskPaused:
                    str = string.Format(Shell.LoadString(StringId.IDS_DOWNLOAD_PROGRESS), this.m_progress.ToString());
                    break;
                case EDownloadTaskState.DLTaskComplete:
                    str = Shell.LoadString(StringId.IDS_INCOLLECTION);
                    break;
                default:
                    str = Shell.LoadString(StringId.IDS_DOWNLOAD);
                    break;
            }
            return str;
        }

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (!disposing)
                return;
            lock (this.m_myLock)
            {
                if (this.m_downloadTask == null)
                    return;
                this.m_downloadTask.OnProgressChanged -= this.m_progressHandler;
                this.m_downloadTask = null;
            }
        }

        protected virtual void OnProgressChanged(DownloadEventArguments args)
        {
            lock (this.m_myLock)
            {
                this.m_downloadState = args.State;
                this.m_progress = (int)args.Progress;
                if (this.m_downloadState != EDownloadTaskState.DLTaskDownloading)
                {
                    if (this.m_downloadState != EDownloadTaskState.DLTaskPaused)
                    {
                        if (this.m_downloadState != EDownloadTaskState.DLTaskPending)
                        {
                            if (this.m_downloadState != EDownloadTaskState.DLTaskPendingAttach)
                            {
                                this.m_downloadTask = null;
                                this.m_downloadState = this.GetDownloadState();
                            }
                        }
                    }
                }
            }
            Application.DeferredInvoke(new DeferredInvokeHandler(this.UpdateCommandState), DeferredInvokePriority.Normal);
        }

        private void UpdateCommandState(object args)
        {
            this.Description = this.GetDownloadString(this.m_downloadState);
            this.Available = this.m_downloadState != EDownloadTaskState.DLTaskComplete;
            if (this.m_downloadState == EDownloadTaskState.DLTaskDownloading || this.m_downloadState == EDownloadTaskState.DLTaskPaused)
                this.Progress = m_progress / 100f;
            else
                this.Progress = -1f;
        }

        private void UpdateDownloadTask()
        {
            if (this.m_downloadTask == null)
            {
                if (this.m_taskId == null)
                    return;
                this.m_downloadTask = DownloadManager.Instance.GetTask(this.m_taskId);
                if (this.m_downloadTask == null)
                    return;
                if (this.m_downloadState == EDownloadTaskState.DLTaskPendingAttach || this.m_downloadState == EDownloadTaskState.DLTaskPending || (this.m_downloadState == EDownloadTaskState.DLTaskDownloading || this.m_downloadState == EDownloadTaskState.DLTaskPaused))
                    this.m_downloadTask.OnProgressChanged += this.m_progressHandler;
                this.m_progress = (int)this.m_downloadTask.GetProgress();
                this.m_downloadState = this.m_downloadTask.GetState();
            }
            else
                this.m_downloadState = this.m_downloadTask.GetState();
        }
    }
}
