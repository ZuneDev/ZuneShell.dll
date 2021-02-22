// Decompiled with JetBrains decompiler
// Type: ZuneUI.MetadataNotifications
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;
using System.Runtime.InteropServices;

namespace ZuneUI
{
    public class MetadataNotifications : ModelItem
    {
        private static MetadataNotifications _singletonInstance;
        private MetadataMgrNotifications _metadataMgr;
        private MessageNotification _libraryNotification;
        private MessageNotification _updateMetadataNotification;
        private bool _updatingMetadata;
        private Timer _timerOnEndUpdates;
        private object _lock = new object();
        private bool _deferredUpdatePending;
        private int _addCount;
        private static string _libraryActiveImportMessage = Shell.LoadString(StringId.IDS_LIBRARY_IMPORTING_MEDIA);
        private static string _libraryAddFileMessage = Shell.LoadString(StringId.IDS_LIBRARY_ADD_FILE);
        private static string _libraryScanCompleteMessage = Shell.LoadString(StringId.IDS_LIBRARY_SCAN_COMPLETE);
        private static string _metadataUpdateMessage = Shell.LoadString(StringId.IDS_LIBRARY_METADATA_UPDATE);
        private static string _metadataUpdateInfoMessage = Shell.LoadString(StringId.IDS_LIBRARY_METADATA_UPDATE_INFO);
        private static string _libraryFileChangeCompleteMessage = Shell.LoadString(StringId.IDS_LIBRARY_FILE_CHANGE_COMPLETE);
        private static string _libraryFileDeleteFailedMessage = Shell.LoadString(StringId.IDS_LIBRARY_FILE_DELETE_FAILED);

        private MetadataNotifications(IModelItemOwner owner)
          : base(owner)
        {
            this._timerOnEndUpdates = new Timer();
            this._timerOnEndUpdates.Interval = 15000;
            this._timerOnEndUpdates.AutoRepeat = false;
            this._timerOnEndUpdates.Tick += new EventHandler(this.OnEndUpdatesTick);
        }

        protected override void OnDispose(bool fDisposing)
        {
            base.OnDispose(fDisposing);
            if (!fDisposing)
                return;
            if (this._metadataMgr != null)
            {
                this._metadataMgr.FileAdded -= new OnFileAddedHandler(this.OnFileAdded);
                this._metadataMgr.Dispose();
                this._metadataMgr = null;
            }
            if (this._timerOnEndUpdates == null)
                return;
            this._timerOnEndUpdates.Tick -= new EventHandler(this.OnEndUpdatesTick);
            this._timerOnEndUpdates.Dispose();
            this._timerOnEndUpdates = null;
        }

        public static MetadataNotifications Instance
        {
            get
            {
                if (_singletonInstance == null)
                    _singletonInstance = new MetadataNotifications(ZuneShell.DefaultInstance);
                return _singletonInstance;
            }
        }

        public bool Importing => this._libraryNotification != null;

        public void Phase2Init()
        {
            this._metadataMgr = new MetadataMgrNotifications();
            if (this._metadataMgr == null)
                return;
            this._metadataMgr.FileAdded += new OnFileAddedHandler(this.OnFileAdded);
        }

        private void OnFileAdded(IntPtr pszPath, EMediaTypes MediaType) => this.NotifyFileAddedStatus();

        private void NotifyFileAddedStatus()
        {
            bool flag = false;
            int milliseconds = 250;
            lock (this._lock)
            {
                if (this._addCount == 0)
                    milliseconds = 0;
                ++this._addCount;
                if (!this._deferredUpdatePending)
                {
                    this._deferredUpdatePending = true;
                    flag = true;
                }
            }
            if (!flag)
                return;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredFileAddedStatus), null, new TimeSpan(0, 0, 0, 0, milliseconds));
        }

        private void DeferredFileAddedStatus(object obj)
        {
            if (this.IsDisposed)
                return;
            int addCount;
            lock (this._lock)
            {
                this._deferredUpdatePending = false;
                addCount = this._addCount;
            }
            this._timerOnEndUpdates.Stop();
            this._timerOnEndUpdates.Start();
            if (Download.Instance.Notification != null)
                return;
            if (this._libraryNotification == null)
            {
                this._libraryNotification = new MessageNotification(_libraryActiveImportMessage, NotificationTask.Library, NotificationState.Normal);
                NotificationArea.Instance.RemoveAll(NotificationTask.Library, NotificationState.Completed);
                NotificationArea.Instance.Add(_libraryNotification);
                this.FirePropertyChanged("Importing");
            }
            this._libraryNotification.SubMessage = string.Format(_libraryAddFileMessage, addCount);
        }

        private void OnEndUpdatesTick(object sender, EventArgs args)
        {
            int addCount;
            lock (this._lock)
            {
                addCount = this._addCount;
                this._addCount = 0;
            }
            NotificationArea.Instance.Replace(_libraryNotification, new MessageNotification(_libraryFileChangeCompleteMessage, string.Format(_libraryAddFileMessage, addCount), NotificationTask.Library, NotificationState.Completed));
            this._libraryNotification = null;
            this.FirePropertyChanged("Importing");
        }

        private void OnBeginMetadataLifecycle() => Application.DeferredInvoke(delegate
       {
           if (this.IsDisposed)
               return;
           this._updatingMetadata = true;
       }, null);

        private void OnEndMetadataLifecycle() => Application.DeferredInvoke(delegate
       {
           if (this.IsDisposed)
               return;
           if (this._updateMetadataNotification != null && this._updateMetadataNotification.Type != NotificationState.Completed)
           {
               NotificationArea.Instance.Replace(_updateMetadataNotification, new MessageNotification("Metadata update completed", NotificationTask.Library, NotificationState.Completed));
               this._updateMetadataNotification = null;
           }
           this._updatingMetadata = false;
       }, null);

        private void OnMetadataUpdate(IntPtr artistPtr, IntPtr albumPtr)
        {
            string artist = Marshal.PtrToStringUni(artistPtr);
            string album = Marshal.PtrToStringUni(albumPtr);
            Application.DeferredInvoke(delegate
           {
               if (this.IsDisposed || !this._updatingMetadata)
                   return;
               if (this._updateMetadataNotification == null)
               {
                   this._updateMetadataNotification = new MessageNotification(_metadataUpdateMessage, NotificationTask.Library, NotificationState.Normal);
                   NotificationArea.Instance.RemoveAll(NotificationTask.Library, NotificationState.Completed);
                   NotificationArea.Instance.Add(_updateMetadataNotification);
               }
               this._updateMetadataNotification.SubMessage = string.Format(_metadataUpdateInfoMessage, artist, album);
           }, null);
        }

        private void OnFileDeleteFailed(IntPtr fileUrlPtr)
        {
            string strFileUrl = Marshal.PtrToStringUni(fileUrlPtr);
            Application.DeferredInvoke(delegate
           {
               if (this.IsDisposed || !this._updatingMetadata)
                   return;
               if (this._updateMetadataNotification == null)
               {
                   this._updateMetadataNotification = new MessageNotification(_metadataUpdateMessage, NotificationTask.Library, NotificationState.Normal);
                   NotificationArea.Instance.RemoveAll(NotificationTask.Library, NotificationState.Completed);
                   NotificationArea.Instance.Add(_updateMetadataNotification);
               }
               this._updateMetadataNotification.SubMessage = string.Format(_libraryFileDeleteFailedMessage, strFileUrl);
           }, null);
        }
    }
}
