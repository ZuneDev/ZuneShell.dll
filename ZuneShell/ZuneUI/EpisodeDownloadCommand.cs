// Decompiled with JetBrains decompiler
// Type: ZuneUI.EpisodeDownloadCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Subscription;
using System;
using System.Collections;

namespace ZuneUI
{
    public class EpisodeDownloadCommand : DownloadCommand
    {
        private DataProviderObject m_episode;
        private EItemDownloadType m_downloadType;
        private bool m_requireSignIn;
        private bool m_explicit;

        public EpisodeDownloadCommand(IModelItem owner, DataProviderObject episode)
          : base(owner)
        {
            this.m_episode = episode;
            this.m_downloadType = (EItemDownloadType)this.m_episode.GetProperty("DownloadType");
            this.TaskId = (string)this.m_episode.GetProperty("EnclosureUrl");
        }

        public static void DownloadEpisode(DataProviderObject episode)
        {
            if (episode is SubscriptionDataProviderItem)
                ((SubscriptionDataProviderItem)episode).SaveToLibrary();
            DownloadEpisode((int)episode.GetProperty("SeriesId"), (int)episode.GetProperty("LibraryId"));
        }

        public static void DownloadEpisode(int seriesId, int episodeId)
        {
            try
            {
                SubscriptionManager.Instance.DownloadEpisode(seriesId, episodeId);
            }
            catch (ApplicationException ex)
            {
            }
        }

        protected override void OnInvoked()
        {
            switch (this.GetDownloadState())
            {
                case EDownloadTaskState.DLTaskPendingAttach:
                case EDownloadTaskState.DLTaskPending:
                case EDownloadTaskState.DLTaskDownloading:
                case EDownloadTaskState.DLTaskPaused:
                    ZuneShell.DefaultInstance.Execute("Marketplace\\Downloads\\Home", null);
                    break;
                case EDownloadTaskState.DLTaskComplete:
                    PodcastLibraryPage.FindInCollection((int)this.m_episode.GetProperty("SeriesId"), (int)this.m_episode.GetProperty("LibraryId"));
                    break;
                default:
                    if (this.m_requireSignIn && !SignIn.Instance.SignedIn || this.m_explicit && ZuneApplication.Service.BlockExplicitContent())
                        break;
                    DownloadEpisode(this.m_episode);
                    this.Refresh();
                    break;
            }
        }

        public bool RequireSignIn
        {
            get => this.m_requireSignIn;
            set => this.m_requireSignIn = value;
        }

        public bool Explicit
        {
            get => this.m_explicit;
            set => this.m_explicit = value;
        }

        protected override EDownloadTaskState GetDownloadState()
        {
            this.m_downloadType = (EItemDownloadType)this.m_episode.GetProperty("DownloadType");
            switch ((EItemDownloadState)this.m_episode.GetProperty("DownloadState"))
            {
                case EItemDownloadState.eDownloadStateNone:
                    return EDownloadTaskState.DLTaskNone;
                case EItemDownloadState.eDownloadStateDownloading:
                    return EDownloadTaskState.DLTaskDownloading;
                case EItemDownloadState.eDownloadStateDownloaded:
                    return EDownloadTaskState.DLTaskComplete;
                default:
                    return EDownloadTaskState.DLTaskNone;
            }
        }

        protected override string GetDownloadString(EDownloadTaskState downloadState)
        {
            if (downloadState != EDownloadTaskState.DLTaskComplete)
                return base.GetDownloadString(downloadState);
            return this.m_downloadType == EItemDownloadType.eDownloadTypeAutomatic ? Shell.LoadString(StringId.IDS_AUTOMATIC) : Shell.LoadString(StringId.IDS_INCOLLECTION);
        }

        public static int ConvertDownloadStatusToInt(EItemDownloadState type) => (int)type;

        public static int ConvertDownloadTypeToInt(EItemDownloadType type) => (int)type;
    }
}
