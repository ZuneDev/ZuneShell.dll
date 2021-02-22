// Decompiled with JetBrains decompiler
// Type: ZuneUI.SyncNotification
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class SyncNotification : ProgressNotification
    {
        private bool _syncCanceled;
        private UIDevice _device;
        private static string _syncing = Shell.LoadString(StringId.IDS_SYNC_STATUS);
        private static string _transcoding = Shell.LoadString(StringId.IDS_TRANSCODE_STATUS);
        private static string _deleting = Shell.LoadString(StringId.IDS_DELETING_STATUS);
        private static string _updating = Shell.LoadString(StringId.IDS_UPDATING_STATUS);
        private static string _tunnelling = Shell.LoadString(StringId.IDS_TUNNELLING_STATUS);
        private static string _group = Shell.LoadString(StringId.IDS_SYNC_STATUS_GROUP);
        private static string _ellipsis = Shell.LoadString(StringId.IDS_GENERIC_ELLIPSIS);

        public SyncNotification(UIDevice device)
          : base(Shell.LoadString(StringId.IDS_SYNC_HEADER), NotificationTask.Sync, NotificationState.Normal)
        {
            this._device = device;
            this.UpdateProgress(0, 0, 0, "", "", ESyncEngineState.sesInitial);
        }

        public void UpdateProgress(
          int percent,
          int percentItem,
          int percentTranscode,
          string group,
          string title,
          ESyncEngineState engineState)
        {
            this.Percentage = percent;
            string format;
            switch (engineState)
            {
                case ESyncEngineState.sesUpdatingContent:
                    format = _updating;
                    break;
                case ESyncEngineState.sesDeletingFilesNotInSyncSet:
                case ESyncEngineState.sesDeletingFilesByUserRequest:
                    format = _deleting;
                    break;
                case ESyncEngineState.sesWaitingForTranscode:
                    format = _transcoding;
                    break;
                case ESyncEngineState.sesDirectSyncNotStarted:
                case ESyncEngineState.sesDirectSyncCalculating:
                case ESyncEngineState.sesDirectSyncDownloading:
                    format = !string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(group) ? _syncing : _tunnelling;
                    break;
                default:
                    format = _syncing;
                    break;
            }
            string str1;
            if (string.IsNullOrEmpty(group))
                str1 = title ?? string.Empty;
            else if (string.IsNullOrEmpty(title))
            {
                str1 = group;
            }
            else
            {
                string str2 = group;
                if (str2.Length > 15)
                    str2 = string.Format(_ellipsis, str2.Substring(0, 15).Trim());
                str1 = string.Format(_group, str2, title);
            }
            this.SubMessage = string.Format(format, str1);
        }

        public void Complete(ESyncEventReason reason)
        {
            this.Percentage = 100;
            if (reason == ESyncEventReason.eSyncEventFailed || reason == ESyncEventReason.eSyncEventInvalid)
            {
                this.Message = Shell.LoadString(StringId.IDS_SYNC_ERROR_NOTIFICATION);
                this.SubMessage = null;
            }
            else
            {
                if (this.SyncCanceled)
                    this.Message = Shell.LoadString(StringId.IDS_SYNC_CANCELLED);
                else
                    this.Message = Shell.LoadString(StringId.IDS_SYNC_COMPLETED);
                long freeSpace = this._device.ActualGasGauge.FreeSpace;
                if (freeSpace <= 0L)
                    return;
                if (freeSpace < SyncControls.DevicelandGigabyte)
                    this.SubMessage = string.Format(Shell.LoadString(StringId.IDS_FREE_SPACE_REMAINING_IN_MB), (float)(freeSpace / (double)SyncControls.DevicelandMegabyte));
                else
                    this.SubMessage = string.Format(Shell.LoadString(StringId.IDS_FREE_SPACE_REMAINING_IN_GB), (float)(freeSpace / (double)SyncControls.DevicelandGigabyte));
            }
        }

        public bool SyncCanceled
        {
            get => this._syncCanceled;
            set
            {
                if (this._syncCanceled == value)
                    return;
                this._syncCanceled = value;
            }
        }

        public UIDevice Device => this._device;
    }
}
