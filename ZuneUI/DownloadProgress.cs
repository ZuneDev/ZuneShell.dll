// Decompiled with JetBrains decompiler
// Type: ZuneUI.DownloadProgress
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System;

namespace ZuneUI
{
    public class DownloadProgress : ProgressCommand
    {
        private EDownloadTaskState m_downloadState = EDownloadTaskState.DLTaskNone;
        private DownloadTask m_downloadTask;

        public DownloadProgress(DownloadTask downloadTask)
          : base(downloadTask)
        {
            this.m_downloadTask = downloadTask;
            this.m_downloadState = this.m_downloadTask.GetState();
            this.m_downloadTask.OnProgressChanged += new DownloadProgressHandler(this.OnProgressChanged);
            this.Available = false;
            this.UpdateState((object)null);
        }

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (!disposing || this.m_downloadTask == null)
                return;
            this.m_downloadTask.OnProgressChanged -= new DownloadProgressHandler(this.OnProgressChanged);
            this.m_downloadTask = (DownloadTask)null;
        }

        protected Microsoft.Zune.Service.EContentType ContentType
        {
            get
            {
                Microsoft.Zune.Service.EContentType econtentType = Microsoft.Zune.Service.EContentType.Unknown;
                string property = this.m_downloadTask.GetProperty("Type");
                if (property == "type:musictrack")
                    econtentType = Microsoft.Zune.Service.EContentType.MusicTrack;
                else if (property == "type:musicvideo")
                    econtentType = Microsoft.Zune.Service.EContentType.Video;
                else if (property == "type:podcast")
                    econtentType = Microsoft.Zune.Service.EContentType.PodcastEpisode;
                else if (property == "type:app")
                    econtentType = Microsoft.Zune.Service.EContentType.App;
                return econtentType;
            }
        }

        protected Guid ServiceId
        {
            get
            {
                Guid guid = Guid.Empty;
                string property = this.m_downloadTask.GetProperty(nameof(ServiceId));
                if (!string.IsNullOrEmpty(property))
                    guid = new Guid(property);
                return guid;
            }
        }

        protected int PodcastEpisodeId => this.m_downloadTask.GetPropertyInt("MediaId");

        protected int CollectionId
        {
            get
            {
                int dbMediaId;
                if (!Microsoft.Zune.Service.Service.Instance.InCompleteCollection(this.ServiceId, this.ContentType, out dbMediaId, out bool _))
                    dbMediaId = -1;
                return dbMediaId;
            }
        }

        protected Microsoft.Zune.Service.EDownloadFlags DownloadFlags => (Microsoft.Zune.Service.EDownloadFlags)this.m_downloadTask.GetPropertyInt(nameof(DownloadFlags));

        protected int SubscriptionMediaId => this.m_downloadTask.GetPropertyInt(nameof(SubscriptionMediaId));

        protected int SubscriptionItemMediaId => this.m_downloadTask.GetPropertyInt(nameof(SubscriptionItemMediaId));

        protected int PlaylistId => this.m_downloadTask.GetPropertyInt(nameof(PlaylistId));

        public void FindInCollection()
        {
            Microsoft.Zune.Service.EContentType contentType = this.ContentType;
            if (contentType == Microsoft.Zune.Service.EContentType.PodcastEpisode)
                PodcastLibraryPage.FindInCollection(-1, this.PodcastEpisodeId);
            else if ((this.DownloadFlags & Microsoft.Zune.Service.EDownloadFlags.Channel) != Microsoft.Zune.Service.EDownloadFlags.None)
            {
                if (this.PlaylistId > 0)
                    MusicLibraryPage.FindPlaylistInCollection(this.PlaylistId, this.CollectionId, true);
                else
                    ChannelLibraryPage.FindInCollection(this.SubscriptionMediaId, this.SubscriptionItemMediaId);
            }
            else
            {
                if (this.CollectionId < 0)
                    return;
                switch (contentType)
                {
                    case Microsoft.Zune.Service.EContentType.MusicTrack:
                        MusicLibraryPage.FindInCollection(-1, -1, this.CollectionId);
                        break;
                    case Microsoft.Zune.Service.EContentType.Video:
                        VideoLibraryPage.FindInCollection(this.CollectionId);
                        break;
                    case Microsoft.Zune.Service.EContentType.App:
                        ApplicationLibraryPage.FindInCollection(this.CollectionId);
                        break;
                }
            }
        }

        private void OnProgressChanged(DownloadEventArguments args) => Application.DeferredInvoke(new DeferredInvokeHandler(this.UpdateState), (object)args, DeferredInvokePriority.Normal);

        private void UpdateState(object args)
        {
            this.Progress = -1f;
            float progress;
            if (args != null)
            {
                DownloadEventArguments downloadEventArguments = (DownloadEventArguments)args;
                this.m_downloadState = downloadEventArguments.State;
                progress = downloadEventArguments.Progress;
            }
            else
            {
                this.m_downloadState = this.m_downloadTask.GetState();
                progress = this.m_downloadTask.GetProgress();
            }
            switch (this.m_downloadState)
            {
                case EDownloadTaskState.DLTaskPendingAttach:
                case EDownloadTaskState.DLTaskPending:
                    this.Description = Shell.LoadString(StringId.IDS_PENDING);
                    this.Available = false;
                    break;
                case EDownloadTaskState.DLTaskDownloading:
                    this.UpdateProgress(Guid.Empty, progress);
                    if (this.SecondsToProgressivePlayback == 0)
                        this.Description = Shell.LoadString(StringId.IDS_PLAY_SONG);
                    else
                        this.Description = string.Format(Shell.LoadString(StringId.IDS_DOWNLOAD_PROGRESS), (object)(int)progress);
                    this.Available = true;
                    break;
                case EDownloadTaskState.DLTaskPaused:
                    this.Description = string.Format(Shell.LoadString(StringId.IDS_DOWNLOAD_PROGRESS), (object)(int)progress);
                    this.Progress = progress / 100f;
                    this.Available = false;
                    break;
                case EDownloadTaskState.DLTaskCancelled:
                    this.Description = Shell.LoadString(StringId.IDS_CANCELLED);
                    this.Available = false;
                    break;
                case EDownloadTaskState.DLTaskFailed:
                    this.Description = Shell.LoadString(StringId.IDS_FAILED);
                    this.Available = false;
                    break;
                case EDownloadTaskState.DLTaskComplete:
                    this.Description = Shell.LoadString(StringId.IDS_INCOLLECTION);
                    this.Available = true;
                    break;
                default:
                    this.Description = Shell.LoadString(StringId.IDS_PENDING);
                    this.Available = false;
                    break;
            }
        }

        protected override void OnInvoked()
        {
            if (!this.Available)
                return;
            if (this.SecondsToProgressivePlayback == 0)
                this.InvokeProgressivePlayback();
            else
                this.FindInCollection();
        }
    }
}
