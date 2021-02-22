// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProgressCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using System;
using ZuneXml;

namespace ZuneUI
{
    public class ProgressCommand : Command
    {
        private const int PlaybackBufferSecs = 200;
        private const int PlaybackThresholdSecs = 10;
        private bool m_allowPlayback = true;
        private float m_progress;
        private float m_progressivePlaybackPoint;
        private int m_secondsToDownload = -1;
        private int m_secondsToProgressivePlayback = -1;
        private int m_secondsToPlaybackThreshold = 5;
        private DownloadTask m_downloadTask;

        public ProgressCommand()
        {
        }

        public ProgressCommand(IModelItem owner)
          : base(owner)
        {
        }

        public ProgressCommand(DownloadTask downloadTask) => this.m_downloadTask = downloadTask;

        public bool AllowProgressivePlayback
        {
            get => this.m_allowPlayback;
            set
            {
                if (this.m_allowPlayback == value)
                    return;
                this.m_allowPlayback = value;
                this.FirePropertyChanged(nameof(AllowProgressivePlayback));
            }
        }

        public float Progress
        {
            get => this.m_progress;
            set
            {
                if (m_progress == (double)value)
                    return;
                this.m_progress = value;
                this.FirePropertyChanged(nameof(Progress));
            }
        }

        public float ProgressivePlaybackPoint
        {
            get => this.m_progressivePlaybackPoint;
            private set
            {
                if (m_progressivePlaybackPoint == (double)value)
                    return;
                this.m_progressivePlaybackPoint = value;
                this.FirePropertyChanged(nameof(ProgressivePlaybackPoint));
            }
        }

        public int SecondsToProgressivePlayback
        {
            get => this.m_secondsToProgressivePlayback;
            private set
            {
                if (this.m_secondsToProgressivePlayback == value)
                    return;
                this.m_secondsToProgressivePlayback = value;
                this.FirePropertyChanged(nameof(SecondsToProgressivePlayback));
            }
        }

        public int SecondsToDownload
        {
            get => this.m_secondsToDownload;
            private set
            {
                if (this.m_secondsToDownload == value)
                    return;
                this.m_secondsToDownload = value;
                this.FirePropertyChanged(nameof(SecondsToDownload));
            }
        }

        public string TimeToProgressivePlayback => TimeFormattingHelper.FormatSeconds(this.SecondsToProgressivePlayback);

        public string TimeToDownload => TimeFormattingHelper.FormatSeconds(this.SecondsToDownload);

        protected void UpdateProgress(Guid taskId, float percent)
        {
            if (this.m_downloadTask == null)
                this.m_downloadTask = DownloadManager.Instance.GetTask(taskId.ToString());
            else if (taskId == Guid.Empty && percent < 0.0)
                this.m_downloadTask = null;
            float num1 = 0.0f;
            int num2 = -1;
            int num3 = -1;
            if (this.m_downloadTask != null)
            {
                num2 = this.m_downloadTask.GetDownloadSecondsRemaining();
                int propertyInt = this.m_downloadTask.GetPropertyInt("PlaybackDuration");
                if (propertyInt > 0)
                {
                    int secondsRemaining = this.m_downloadTask.GetDownloadFileSecondsRemaining(0);
                    if (secondsRemaining >= 0)
                    {
                        int num4 = secondsRemaining - propertyInt;
                        long bytesDownloaded = (long)this.m_downloadTask.GetBytesDownloaded();
                        long fileSizeForPlayback = (long)this.CalculateMinimumFileSizeForPlayback(this.m_downloadTask.GetFinalFileSize(0), propertyInt);
                        int num5 = this.m_downloadTask.GetDownloadBytesPerSecond();
                        if (num5 <= 0)
                            num5 = 1;
                        if (this.AllowProgressivePlayback)
                        {
                            int num6 = (int)((fileSizeForPlayback - bytesDownloaded) / num5);
                            num3 = (num4 > num6 ? num4 : num6) + this.m_secondsToPlaybackThreshold;
                            if (num3 > 0)
                            {
                                num1 = percent + (100f - percent) * num3 / num2;
                                this.m_secondsToPlaybackThreshold = 10;
                            }
                            else
                            {
                                num3 = 0;
                                this.m_secondsToPlaybackThreshold = 0;
                            }
                        }
                    }
                }
            }
            this.Progress = percent / 100f;
            this.ProgressivePlaybackPoint = num1 / 100f;
            this.SecondsToProgressivePlayback = num3;
            this.SecondsToDownload = num2;
        }

        public void InvokeProgressivePlayback()
        {
            if (this.m_downloadTask == null || this.SecondsToProgressivePlayback != 0)
                return;
            string tempFileName = this.m_downloadTask.GetTempFileName(0);
            if (string.IsNullOrEmpty(tempFileName))
                return;
            int propertyInt = this.m_downloadTask.GetPropertyInt("PlaybackDuration");
            if (propertyInt == 0)
                return;
            ulong finalFileSize = this.m_downloadTask.GetFinalFileSize(0);
            ulong bytesDownloaded = this.m_downloadTask.GetBytesDownloaded();
            if (finalFileSize <= 0UL || bytesDownloaded <= this.CalculateMinimumFileSizeForPlayback(finalFileSize, propertyInt))
                return;
            string uri = string.Format("zuneprogdl://{0}?duration={1}&size={2}", tempFileName, propertyInt, finalFileSize);
            string property1 = this.m_downloadTask.GetProperty("Title");
            string property2 = this.m_downloadTask.GetProperty("Artist");
            SingletonModelItem<TransportControls>.Instance.PlayItem(new VideoPlaybackTrack(new Guid(this.m_downloadTask.GetProperty("ServiceId")), property1, property2, uri, true, false, false, false, false, VideoDefinitionEnum.None), PlayNavigationOptions.NavigateToNowPlaying);
        }

        private ulong CalculateMinimumFileSizeForPlayback(ulong finalSize, int finalDuration)
        {
            ulong num = 0;
            if (this.m_downloadTask != null)
                num = finalSize / (ulong)finalDuration * 200UL;
            return num;
        }
    }
}
