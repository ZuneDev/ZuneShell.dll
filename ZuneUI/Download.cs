// Decompiled with JetBrains decompiler
// Type: ZuneUI.Download
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using ZuneXml;

namespace ZuneUI
{
    public class Download : ModelItem
    {
        private bool m_disposed;
        private bool m_updatePending;
        private ProgressNotification m_notification;
        private Dictionary<Guid, int> m_errors = new Dictionary<Guid, int>();
        private Dictionary<Guid, int> m_historyErrors = new Dictionary<Guid, int>();
        private Microsoft.Zune.Service.EDownloadContextEvent m_clientContextEvent;
        private string m_clientContextEventValue;
        private static Download s_instance;
        private static object s_cs = new object();
        private static string s_downloadCompleteMessage = ZuneUI.Shell.LoadString(StringId.IDS_DOWNLOAD_COMPLETE_NOTIFICATION);
        private static string s_downloadFailedMessage = ZuneUI.Shell.LoadString(StringId.IDS_DOWNLOAD_FAILED_NOTIFICATION);
        private static string s_downloadProgressMessage = ZuneUI.Shell.LoadString(StringId.IDS_DOWNLOAD_PROGRESS_NOTIFICATION);
        private static string s_downloadCurrentMessage = ZuneUI.Shell.LoadString(StringId.IDS_DOWNLOAD_CURRENT_NOTIFICATION);
        private static string s_downloadPausedMessage = ZuneUI.Shell.LoadString(StringId.IDS_DOWNLOAD_PAUSED_NOTIFICATION);
        private static string s_downloadMBRPausedMessage = ZuneUI.Shell.LoadString(StringId.IDS_DOWNLOAD_MBR_PAUSED_NOTIFICATION);
        private static string s_downloadSignInMessage = ZuneUI.Shell.LoadString(StringId.IDS_DOWNLOAD_SIGNIN_NOTIFICATION);

        public event DownloadEventHandler DownloadEvent;

        public event DownloadEventProgressHandler DownloadProgressEvent;

        public event EventHandler DownloadAllPendingEvent;

        public static Download Instance
        {
            get
            {
                if (Download.s_instance == null)
                {
                    lock (Download.s_cs)
                    {
                        if (Download.s_instance == null)
                            Download.s_instance = new Download();
                    }
                }
                return Download.s_instance;
            }
        }

        public static bool IsCreated => Download.s_instance != null;

        public void DownloadContent(IList items, Microsoft.Zune.Service.EDownloadFlags eDownloadFlags) => this.DownloadContent(items, eDownloadFlags, (string)null);

        internal void DownloadContent(
          IList items,
          Microsoft.Zune.Service.EDownloadFlags eDownloadFlags,
          string deviceEndpointId)
        {
            this.DownloadContent(items, eDownloadFlags, deviceEndpointId, new EventHandler(this.OnAllPending));
        }

        public void DownloadContent(
          IList items,
          Microsoft.Zune.Service.EDownloadFlags eDownloadFlags,
          string deviceEndpointId,
          EventHandler onAllPending)
        {
            ZuneApplication.Service.Download(items, eDownloadFlags, deviceEndpointId, this.ClientContextEvent, this.ClientContextEventValue, new DownloadEventHandler(this.OnDownloadEvent), new DownloadEventProgressHandler(this.OnDownloadProgressEvent), onAllPending);
            if ((eDownloadFlags & ~(Microsoft.Zune.Service.EDownloadFlags.CanBeOffline | Microsoft.Zune.Service.EDownloadFlags.Subscription)) != Microsoft.Zune.Service.EDownloadFlags.None)
                return;
            bool flag = ZuneShell.DefaultInstance.CurrentPage is InboxPage;
            if ((eDownloadFlags & Microsoft.Zune.Service.EDownloadFlags.Subscription) != Microsoft.Zune.Service.EDownloadFlags.None)
                SQMLog.Log(flag ? SQMDataId.InboxDownload : SQMDataId.MarketplaceDownload, 1);
            else
                SQMLog.Log(flag ? SQMDataId.InboxPurchase : SQMDataId.MarketplacePurchase, 1);
        }

        public void AddToCollection(IList items)
        {
            ZuneApplication.Service.Download(items, Microsoft.Zune.Service.EDownloadFlags.Subscription, (string)null, this.ClientContextEvent, this.ClientContextEventValue, new DownloadEventHandler(this.OnDownloadEvent), new DownloadEventProgressHandler(this.OnDownloadProgressEvent), new EventHandler(this.OnAllPending));
            foreach (object obj in (IEnumerable)items)
            {
                if (obj is DataProviderObject)
                {
                    DataProviderObject dataProviderObject = (DataProviderObject)obj;
                    if (dataProviderObject.TypeName == "PlaylistContentItem")
                    {
                        PlaylistManager.GetPlaylistId((int)dataProviderObject.GetProperty("LibraryId"));
                        UsageDataService.ReportTrackAddToCollection((Guid)dataProviderObject.GetProperty("ZuneMediaId"), PlaylistManager.GetFieldValue<int>((int)dataProviderObject.GetProperty("MediaId"), EListType.eTrackList, 358, 0).ToString());
                    }
                    else if (dataProviderObject is Track)
                    {
                        Track track = (Track)dataProviderObject;
                        UsageDataService.ReportTrackAddToCollection(track.Id, track.ReferrerContext);
                    }
                }
            }
        }

        public bool IsDownloadingOrPending(Guid mediaId, Microsoft.Zune.Service.EContentType eContentType)
        {
            bool fPending;
            return this.IsDownloading(mediaId, eContentType, out fPending) || fPending;
        }

        public bool IsDownloading(Guid mediaId, Microsoft.Zune.Service.EContentType eContentType, out bool fPending)
        {
            bool fIsHidden = false;
            return ZuneApplication.Service.IsDownloading(mediaId, eContentType, out fPending, out fIsHidden);
        }

        public void CancelDownload(Guid mediaId) => ZuneApplication.Service.CancelDownload(mediaId, Microsoft.Zune.Service.EContentType.MusicTrack);

        public int ErrorCount => this.m_errors.Count;

        public int GetErrorCode(Guid mediaId)
        {
            int num;
            if (!this.m_errors.TryGetValue(mediaId, out num))
                num = 0;
            return num;
        }

        internal void SetErrorCode(Guid mediaId, int errorCode)
        {
            this.m_errors[mediaId] = errorCode;
            this.FirePropertyChanged("ErrorCount");
        }

        internal void Phase2Init() => DownloadManager.CreateInstance();

        internal void Phase3Init()
        {
            DownloadManager.Instance.OnProgressChanged += new DownloadManagerUpdateHandler(this.OnProgressChanged);
            ZuneApplication.Service.RegisterForDownloadNotification(new DownloadEventHandler(this.OnDownloadEvent), new DownloadEventProgressHandler(this.OnDownloadProgressEvent), new EventHandler(this.OnAllPending));
            SingletonModelItem<TransportControls>.Instance.PropertyChanged += new PropertyChangedEventHandler(this.OnTransportControlPropertyChanged);
        }

        private void ClearErrorCode(Guid mediaId)
        {
            this.m_errors.Remove(mediaId);
            this.FirePropertyChanged("ErrorCount");
        }

        public int HistoryErrorCount => this.m_historyErrors.Count;

        public int GetHistoryErrorCode(Guid mediaId)
        {
            int num;
            if (!this.m_historyErrors.TryGetValue(mediaId, out num))
                num = 0;
            return num;
        }

        internal void SetHistoryErrorCode(Guid mediaId, int errorCode)
        {
            this.m_historyErrors[mediaId] = errorCode;
            this.FirePropertyChanged("HistoryErrorCount");
        }

        private void OnDownloadEvent(Guid mediaId, HRESULT hr)
        {
            if (this.m_disposed)
                return;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredDownloadEvent), (object)new object[2]
            {
        (object) mediaId,
        (object) hr
            });
        }

        private void OnAllPending(object sender, EventArgs e)
        {
            if (this.m_disposed)
                return;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DefferedAllPending), (object)null);
        }

        private void DeferredDownloadEvent(object arg)
        {
            object[] objArray = (object[])arg;
            Guid mediaId = (Guid)objArray[0];
            HRESULT hr = (HRESULT)objArray[1];
            if (hr.IsSuccess || hr == HRESULT._E_ABORT || (hr == HRESULT._E_PENDING || hr == HRESULT._E_ALREADY_EXISTS))
                this.ClearErrorCode(mediaId);
            else
                this.SetErrorCode(mediaId, hr.hr);
            if (this.DownloadEvent == null)
                return;
            this.DownloadEvent(mediaId, hr);
            this.FirePropertyChanged("DownloadEvent");
        }

        private void DefferedAllPending(object notUsed)
        {
            if (this.DownloadAllPendingEvent == null)
                return;
            this.DownloadAllPendingEvent((object)this, (EventArgs)null);
        }

        private void OnDownloadProgressEvent(Guid mediaId, float percent)
        {
            if (this.m_disposed)
                return;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredDownloadProgressEvent), (object)new object[2]
            {
        (object) mediaId,
        (object) percent
            });
        }

        private void DeferredDownloadProgressEvent(object arg)
        {
            if (this.DownloadProgressEvent == null)
                return;
            object[] objArray = (object[])arg;
            this.DownloadProgressEvent((Guid)objArray[0], (float)objArray[1]);
            this.FirePropertyChanged("DownloadProgressEvent");
        }

        public ProgressNotification Notification
        {
            get => this.m_notification;
            private set
            {
                if (this.m_notification == value)
                    return;
                if (this.m_notification != null)
                    NotificationArea.Instance.Remove((ZuneUI.Notification)this.m_notification);
                this.m_notification = value;
                if (this.m_notification != null)
                    NotificationArea.Instance.Add((ZuneUI.Notification)this.m_notification);
                this.FirePropertyChanged(nameof(Notification));
            }
        }

        public DownloadTaskList DownloadTaskList => DownloadTaskList.Instance;

        private void OnTransportControlPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "SupressDownloads"))
                return;
            if (SingletonModelItem<TransportControls>.Instance.SupressDownloads)
                DownloadManager.Instance.PauseQueue();
            else
                DownloadManager.Instance.ResumeQueue();
        }

        private void ShowCompletedMessage(bool cancellations, bool failures)
        {
            if (this.Notification == null || this.Notification.Type == NotificationState.Completed)
                return;
            this.Notification.Type = NotificationState.Completed;
            if (failures)
            {
                this.Notification.Message = Download.s_downloadFailedMessage;
            }
            else
            {
                this.Notification.Message = Download.s_downloadCompleteMessage;
                this.Notification.Percentage = 100;
            }
            this.Notification.SubMessage = (string)null;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.HideCompletedMessage), (object)null, TimeSpan.FromSeconds(5.0));
        }

        private void HideCompletedMessage(object args)
        {
            if (this.Notification == null || this.Notification.Type != NotificationState.Completed)
                return;
            this.Notification = (ProgressNotification)null;
        }

        private void OnProgressChanged(DownloadManagerUpdateArguments args)
        {
            if (this.m_updatePending)
                return;
            this.m_updatePending = true;
            Application.DeferredInvoke((DeferredInvokeHandler)delegate
           {
               this.m_updatePending = false;
               int totalItems = DownloadManager.Instance.TotalItems;
               int activeItem = DownloadManager.Instance.ActiveItem;
               if (totalItems == 0)
               {
                   this.Notification = (ProgressNotification)null;
               }
               else
               {
                   if (this.Notification == null)
                   {
                       this.Notification = new ProgressNotification(Download.s_downloadProgressMessage, NotificationTask.Download, NotificationState.Normal, 0);
                   }
                   else
                   {
                       this.Notification.Type = NotificationState.Normal;
                       this.Notification.Message = Download.s_downloadProgressMessage;
                   }
                   if (DownloadManager.Instance.Finished)
                   {
                       this.ShowCompletedMessage(DownloadManager.Instance.HadCancellations, DownloadManager.Instance.HadFailures);
                   }
                   else
                   {
                       this.Notification.Percentage = (int)DownloadManager.Instance.Percentage;
                       this.Notification.SubMessage = string.Format(Download.s_downloadCurrentMessage, (object)activeItem, (object)totalItems);
                       if (DownloadManager.Instance.IsQueuePaused)
                       {
                           if (SingletonModelItem<TransportControls>.Instance.IsStreamingVideo)
                               this.Notification.Message = Download.s_downloadMBRPausedMessage;
                           else
                               this.Notification.Message = Download.s_downloadPausedMessage;
                       }
                       else if (!SignIn.Instance.SignedIn && DownloadManager.Instance.SignInRequired())
                           this.Notification.Message = Download.s_downloadSignInMessage;
                   }
               }
               bool show = !DownloadManager.Instance.Finished || DownloadManager.Instance.FailedDownloads.Count > 0 || DownloadManager.Instance.HadFailures;
               ZuneUI.Shell.MainFrame.Marketplace.UpdateDownloadPivot(show);
               ZuneUI.Shell.MainFrame.Collection.UpdateDownloadPivot(show);
               if (show || !(ZuneShell.DefaultInstance.CurrentPage is DownloadsPage))
                   return;
               ZuneShell.DefaultInstance.NavigateBack();
           }, (object)null);
        }

        private Download()
        {
        }

        protected override void OnDispose(bool fDisposing)
        {
            base.OnDispose(fDisposing);
            if (fDisposing)
            {
                DownloadManager.Instance.OnProgressChanged -= new DownloadManagerUpdateHandler(this.OnProgressChanged);
                SingletonModelItem<TransportControls>.Instance.PropertyChanged -= new PropertyChangedEventHandler(this.OnTransportControlPropertyChanged);
            }
            this.m_disposed = true;
        }

        public Microsoft.Zune.Service.EDownloadContextEvent ClientContextEvent => !this.ShouldTrackUsage() ? Microsoft.Zune.Service.EDownloadContextEvent.Unknown : this.m_clientContextEvent;

        public string ClientContextEventValue => !this.ShouldTrackUsage() ? (string)null : this.m_clientContextEventValue;

        private bool ShouldTrackUsage() => ClientConfiguration.SQM.UsageTracking && ClientConfiguration.FUE.AcceptedPrivacyStatement;

        public void ReportClientContextEvent(
          Microsoft.Zune.Service.EDownloadContextEvent clientContextEvent,
          Guid clientContextEventValue)
        {
            Microsoft.Zune.Service.EDownloadContextEvent edownloadContextEvent = Microsoft.Zune.Service.EDownloadContextEvent.Unknown;
            string str = (string)null;
            if (clientContextEventValue != Guid.Empty)
            {
                edownloadContextEvent = clientContextEvent;
                str = clientContextEventValue.ToString();
            }
            bool flag1 = this.m_clientContextEvent != edownloadContextEvent;
            bool flag2 = this.m_clientContextEventValue != str;
            if (flag1)
            {
                this.m_clientContextEvent = edownloadContextEvent;
                this.FirePropertyChanged("ClientContextEvent");
            }
            if (!flag1 && !flag2)
                return;
            this.m_clientContextEventValue = str;
            this.FirePropertyChanged("ClientContextEventValue");
        }
    }
}
