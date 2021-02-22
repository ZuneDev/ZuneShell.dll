// Decompiled with JetBrains decompiler
// Type: ZuneUI.BurnableCD
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Service;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class BurnableCD : ModelItem
    {
        private CDAccess _cdAccess;
        private ZuneLibraryCDDevice _device;
        private bool _isBurning;
        private bool _hasDeviceStartedBurn;
        private bool _isErasing;
        private bool _burnCanceling;
        private List<BurnSessionItem> _items;
        private int _playlistId;
        private string _burnTitle;
        private bool _downloadsComplete;
        private HRESULT _burnError;
        private ProgressNotification _burnNotification;
        private int _sessionTotalTime;
        private int _sessionTimeRemaining;
        private int _sessionPercentComplete;
        private int _sessionCurrentTrackNumber;
        private bool _sessionFinalizing;
        private Timer _progressTimer;
        private int _simulatedTickInterval;
        private string _burnProgressMessage;
        private static string s_burnStartedMessage = Shell.LoadString(StringId.IDS_BURN_STARTED_NOTIFICATION);
        private static string s_burnPreparingMessage = Shell.LoadString(StringId.IDS_BURN_PREPARING);
        private static string s_burnProgressMessage = Shell.LoadString(StringId.IDS_BURN_PROGRESS_NOTIFICATION);
        private static string s_burnProgressDataMessage = Shell.LoadString(StringId.IDS_BURN_PROGRESS_NOTIFICATION_DATA);
        private static string s_burnFinalizingMessage = Shell.LoadString(StringId.IDS_BURN_FINALIZING);
        private static string s_burnSucceededMessage = Shell.LoadString(StringId.IDS_BURN_SUCCEEDED_NOTIFICATION);
        private static string s_burnFailedMessage = Shell.LoadString(StringId.IDS_BURN_FAILED_NOTIFICATION);
        private static string s_burnCanceledMessage = Shell.LoadString(StringId.IDS_BURN_CANCELED_NOTIFICATION);
        private static string s_burnCancelingMessage = Shell.LoadString(StringId.IDS_BURN_CANCELING_NOTIFICATION);
        private static string s_eraseStartedMessage = Shell.LoadString(StringId.IDS_ERASE_STARTED_NOTIFICATION);
        private static string s_eraseFinishedMessage = Shell.LoadString(StringId.IDS_ERASE_COMPLETED_NOTIFICATION);

        internal BurnableCD(CDAccess cdAccess, ZuneLibraryCDDevice device)
        {
            this._cdAccess = cdAccess;
            this._device = device;
        }

        protected override void OnDispose(bool fDisposing)
        {
            base.OnDispose(fDisposing);
            if (!fDisposing)
                return;
            this._device = (ZuneLibraryCDDevice)null;
            this._cdAccess = (CDAccess)null;
        }

        internal bool IsWriteable => this._device.IsWriteable;

        public bool CanBurnAudio => !this._device.IsDVD;

        public ZuneLibraryCDDevice Device => this._device;

        public char DriveLetter => this._device.DrivePath;

        public TimeSpan TimeAvailable => TimeSpan.FromSeconds((double)this._device.TimeAvailable);

        public long SpaceAvailable => this._device.SpaceAvailable;

        public bool IsBurning => this._isBurning;

        public bool IsErasing => this._isErasing;

        public bool IsBurnCanceling => this._burnCanceling;

        public void PrepareForBurn(string burnTitle, int playlistId, IList burnListItems)
        {
            this._isBurning = true;
            this._burnError = new HRESULT();
            this._items = new List<BurnSessionItem>(burnListItems.Count);
            this._playlistId = playlistId;
            this._burnTitle = burnTitle;
            this._downloadsComplete = false;
            this._burnProgressMessage = this._cdAccess.IsAudioBurn ? BurnableCD.s_burnProgressMessage : BurnableCD.s_burnProgressDataMessage;
            for (int index = 0; index < burnListItems.Count; ++index)
            {
                DataProviderObject burnListItem = (DataProviderObject)burnListItems[index];
                int property1 = (int)burnListItem.GetProperty("LibraryId");
                Guid property2 = (Guid)burnListItem.GetProperty("ZuneMediaId");
                this._items.Add(new BurnSessionItem(this, index, property1, property2));
            }
            this.BeginSession();
            this.UpdateMessage();
        }

        public void ClearBurnItems() => this._items = (List<BurnSessionItem>)null;

        public void StartBurn()
        {
            if (!this._isBurning)
                return;
            this.WaitForDownloads();
        }

        private void WaitForDownloads()
        {
            bool flag = false;
            if (this._cdAccess.IsAudioBurn)
            {
                foreach (BurnSessionItem burnSessionItem in this._items)
                {
                    bool fPending = false;
                    if (Download.Instance.IsDownloading(burnSessionItem.ZuneMediaId, EContentType.MusicTrack, out fPending))
                    {
                        burnSessionItem.Downloading = true;
                        flag = true;
                    }
                }
            }
            if (!flag)
            {
                this.StartBurnForReal();
            }
            else
            {
                Download.Instance.DownloadEvent += new DownloadEventHandler(this.OnDownloadEvent);
                Download.Instance.DownloadProgressEvent += new DownloadEventProgressHandler(this.OnDownloadProgress);
                this._downloadsComplete = false;
            }
        }

        private void OnDownloadEvent(Guid zuneMediaId, HRESULT hr)
        {
            if (this._downloadsComplete || hr == HRESULT._E_PENDING || hr == HRESULT._E_ALREADY_EXISTS)
                return;
            bool flag = true;
            int hrError = 0;
            foreach (BurnSessionItem burnSessionItem in this._items)
            {
                if (burnSessionItem.ZuneMediaId == zuneMediaId)
                {
                    burnSessionItem.Downloading = false;
                    burnSessionItem.ErrorCode = hr.Int;
                }
                else if (burnSessionItem.Downloading)
                    flag = false;
                if (hrError == 0 && burnSessionItem.ErrorCode != 0)
                    hrError = burnSessionItem.ErrorCode;
            }
            if (!flag)
                return;
            this._downloadsComplete = true;
            if (hrError != 0)
            {
                this.OnSessionError(hrError);
                this.NotifyBurnStopped();
            }
            else
                this.StartBurnForReal();
        }

        private void OnDownloadProgress(Guid zuneMediaId, float percent)
        {
            foreach (BurnSessionItem burnSessionItem in this._items)
            {
                if (burnSessionItem.ZuneMediaId == zuneMediaId)
                {
                    burnSessionItem.DownloadProgress = (int)percent;
                    break;
                }
            }
        }

        private void StartBurnForReal()
        {
            this._sessionCurrentTrackNumber = 0;
            HRESULT hr = this._device.SetBurnPlaylist(this._playlistId);
            if (hr.IsSuccess)
            {
                if (!string.IsNullOrEmpty(this._burnTitle))
                    hr = this._device.SetVolumeLabelW(this._burnTitle);
                if (hr.IsSuccess)
                    hr = this._device.StartBurn();
            }
            if (hr.IsError)
            {
                Application.DeferredInvoke((DeferredInvokeHandler)delegate
               {
                   this.OnSessionError(hr.Int);
                   this.NotifyBurnStopped();
               }, (object)null);
            }
            else
            {
                this._hasDeviceStartedBurn = true;
                this.UpdateMessage();
            }
        }

        internal void CancelBurn()
        {
            if (!this._isBurning || this._burnCanceling)
                return;
            this._burnCanceling = true;
            if (this._hasDeviceStartedBurn)
                this._device.StopBurn();
            if (this._progressTimer != null)
                this._progressTimer.Stop();
            this.UpdateMessage();
            if (!this._hasDeviceStartedBurn)
                this.NotifyBurnStopped();
            this._hasDeviceStartedBurn = false;
        }

        private void BeginSession()
        {
            this._device.ItemProgressHandler += new OnItemProgressHandler(this.OnItemProgress);
            this._device.ItemErrorHandler += new OnItemErrorHandler(this.OnItemError);
            this._device.SessionProgressHandler += new OnSessionProgressHandler(this.OnSessionProgress);
            this._device.BurnStateChangeHandler += new OnBurnStateChangeHandler(this.OnBurnStateChanged);
        }

        private void NotifyBurnStopped()
        {
            this.EndSession();
            if (this._burnCanceling && this._items != null)
            {
                foreach (BurnSessionItem burnSessionItem in this._items)
                    burnSessionItem.BurnCanceled = true;
                this._items = (List<BurnSessionItem>)null;
            }
            this._isBurning = false;
            this._burnCanceling = false;
            this._sessionFinalizing = false;
            this._playlistId = PlaylistManager.InvalidPlaylistId;
            this._burnTitle = (string)null;
            this._cdAccess.IsBurning = false;
        }

        private void EndSession()
        {
            this._device.ItemProgressHandler -= new OnItemProgressHandler(this.OnItemProgress);
            this._device.ItemErrorHandler -= new OnItemErrorHandler(this.OnItemError);
            this._device.SessionProgressHandler -= new OnSessionProgressHandler(this.OnSessionProgress);
            this._device.BurnStateChangeHandler -= new OnBurnStateChangeHandler(this.OnBurnStateChanged);
            Download.Instance.DownloadEvent -= new DownloadEventHandler(this.OnDownloadEvent);
            Download.Instance.DownloadProgressEvent -= new DownloadEventProgressHandler(this.OnDownloadProgress);
            this._downloadsComplete = false;
            this._sessionTotalTime = 0;
            this._sessionTimeRemaining = 0;
            this._sessionPercentComplete = 0;
            if (this._progressTimer != null)
            {
                this._progressTimer.Dispose();
                this._progressTimer = (Timer)null;
            }
            this.ShowCompletedMessage();
        }

        public void Erase()
        {
            if (this._isErasing)
                return;
            this._isErasing = true;
            this._cdAccess.ActiveDriveLetter = this.DriveLetter;
            this._cdAccess.IsErasing = true;
            this.BeginSession();
            if (!this._device.EraseDisc().IsError)
                return;
            this.CompleteErase();
        }

        private void CompleteErase()
        {
            this.EndSession();
            this._isErasing = false;
            this._cdAccess.IsErasing = false;
        }

        public BurnSessionItem GetBurnItemByPlaylistContentId(int playlistContentId)
        {
            if (this._items != null)
            {
                foreach (BurnSessionItem burnSessionItem in this._items)
                {
                    if (burnSessionItem.PlaylistContentId == playlistContentId)
                        return burnSessionItem;
                }
            }
            return (BurnSessionItem)null;
        }

        private void OnItemProgress(int index, EBurnProgressStatus eStatus, int nPercent) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           bool flag = false;
           if (0 <= index && index < this._items.Count)
           {
               if (eStatus == EBurnProgressStatus.ebpsComplete || eStatus == EBurnProgressStatus.ebpsFinalizing)
                   this._items[index].BurnComplete = true;
               else if (eStatus == EBurnProgressStatus.ebpsWritingImageToCD || eStatus == EBurnProgressStatus.ebpsTAOConvertingAndWriting)
               {
                   this._items[index].BurnProgress = nPercent;
                   int num = index + 1;
                   if (this._sessionCurrentTrackNumber != num)
                   {
                       this._sessionCurrentTrackNumber = num;
                       flag = true;
                   }
               }
           }
           if (eStatus == EBurnProgressStatus.ebpsFinalizing)
           {
               this._sessionFinalizing = true;
               flag = true;
           }
           if (!flag)
               return;
           this.UpdateMessage();
       }, (object)null);

        private void OnItemError(int index, int hrError) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (index == -1)
           {
               this.OnSessionError(hrError);
           }
           else
           {
               if (0 > index || index >= this._items.Count)
                   return;
               this._items[index].ErrorCode = hrError;
           }
       }, (object)null);

        private void OnSessionError(int hrError)
        {
            this._burnError = new HRESULT(hrError);
            bool flag = false;
            foreach (BurnSessionItem burnSessionItem in this._items)
            {
                if (burnSessionItem.ErrorCode != 0)
                {
                    flag = true;
                    break;
                }
            }
            foreach (BurnSessionItem burnSessionItem in this._items)
            {
                if (!flag)
                    burnSessionItem.ErrorCode = hrError;
                else if (burnSessionItem.ErrorCode == 0)
                    burnSessionItem.BurnCanceled = true;
            }
            Shell.ShowErrorDialog(hrError, StringId.IDS_BURN_FAILED);
        }

        private void OnSessionProgress(int lSessionTimeRemaining, int lSessionTotalTime) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           this._sessionTotalTime = lSessionTotalTime * 1000;
           this._sessionTimeRemaining = lSessionTimeRemaining * 1000;
           this.UpdateMessage();
           int num = 100 - this._sessionPercentComplete;
           if (num <= 0)
               return;
           this._simulatedTickInterval = this._sessionTimeRemaining / num;
           if (this._simulatedTickInterval <= 0)
               return;
           if (this._progressTimer == null)
           {
               this._progressTimer = new Timer();
               this._progressTimer.Enabled = true;
               this._progressTimer.Tick += new EventHandler(this.OnSimulatedSessionProgress);
           }
           this._progressTimer.Interval = this._simulatedTickInterval;
       }, (object)null);

        private void OnSimulatedSessionProgress(object sender, EventArgs args)
        {
            this._sessionTimeRemaining = Math.Max(this._sessionTimeRemaining - this._simulatedTickInterval, 0);
            this.UpdateMessage();
        }

        private void OnBurnStateChanged(EBurnState eBurnState) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (eBurnState != EBurnState.ebsStopped)
               return;
           if (this._isBurning)
           {
               this.NotifyBurnStopped();
           }
           else
           {
               if (!this._isErasing)
                   return;
               this.CompleteErase();
           }
       }, (object)null);

        private ProgressNotification Notification
        {
            get => this._burnNotification;
            set
            {
                this._burnNotification = value;
                this._cdAccess.BurnNotification = this._burnNotification;
            }
        }

        private void UpdateMessage()
        {
            string message = this._isBurning ? BurnableCD.s_burnStartedMessage : BurnableCD.s_eraseStartedMessage;
            if (this._burnNotification == null)
            {
                this.Notification = new ProgressNotification(message, NotificationTask.Burn, NotificationState.Normal, 0);
            }
            else
            {
                this._burnNotification.Type = NotificationState.Normal;
                this._burnNotification.Message = message;
            }
            int num = 0;
            if (this._sessionTotalTime > 0)
                num = (this._sessionTotalTime - this._sessionTimeRemaining) * 100 / this._sessionTotalTime;
            if (this._sessionPercentComplete < num)
                this._sessionPercentComplete = num;
            this._burnNotification.Percentage = this._sessionPercentComplete;
            if (!this._isBurning)
                return;
            if (this._burnCanceling)
                this._burnNotification.SubMessage = BurnableCD.s_burnCancelingMessage;
            else if (this._sessionFinalizing)
                this._burnNotification.SubMessage = BurnableCD.s_burnFinalizingMessage;
            else if (this._sessionCurrentTrackNumber == 0)
                this._burnNotification.SubMessage = BurnableCD.s_burnPreparingMessage;
            else
                this._burnNotification.SubMessage = string.Format(this._burnProgressMessage, (object)this._sessionCurrentTrackNumber, (object)this._items.Count);
        }

        private void ShowCompletedMessage()
        {
            if (this._burnNotification != null && this._burnNotification.Type != NotificationState.Completed)
            {
                this._burnNotification.Type = NotificationState.Completed;
                if (this._isBurning)
                {
                    if (this._burnCanceling)
                        this._burnNotification.Message = BurnableCD.s_burnCanceledMessage;
                    else if (this._burnError.IsSuccess)
                    {
                        this._burnNotification.Message = BurnableCD.s_burnSucceededMessage;
                        this._burnNotification.Percentage = 100;
                    }
                    else
                        this._burnNotification.Message = BurnableCD.s_burnFailedMessage;
                }
                else
                    this._burnNotification.Message = BurnableCD.s_eraseFinishedMessage;
                this._burnNotification.SubMessage = (string)null;
                this.Notification = (ProgressNotification)null;
            }
            SoundHelper.Play(SoundId.BurnComplete);
        }
    }
}
