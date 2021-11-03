// Decompiled with JetBrains decompiler
// Type: ZuneUI.UIFirmwareUpdater
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using MicrosoftZuneLibrary;
using System;
using System.Threading;

namespace ZuneUI
{
    public class UIFirmwareUpdater : ModelItem
    {
        private FirmwareUpdater _updater;
        private bool _installFirmware;
        private bool _installGames;
        private UpdatePackageCollection _updateCollection;
        private bool _isUpdateAvailable;
        private bool _rollbackStarted;
        private bool _isCheckingForUpdates;
        private bool _updateInProgress;
        private UpdateStep _currentStep;
        private HRESULT _lastCheckForUpdatesResult;
        private HRESULT _lastCheckForDiskSpaceResult;
        private FirmwareUpdateErrorInfo _lastCheckForUpdateErrorInfo;
        private HRESULT _lastUpdateResult;
        private FirmwareUpdateErrorInfo _lastFirmwareUpdateErrorInfo;
        private bool _isSoftFailure;
        private bool _canCancel;
        private bool _needsCollectionRefresh;
        private bool _launchWizardIfUpdatesFound;
        private bool _fCancelInProgress;
        private int _estimatedUpdateTime;
        private bool _estimatedUpdateTimeInProgress;
        private UIDevice _device;
        private bool _requiresSyncBeforeUpdate;
        private FirmwareUpdateOption _updateOption;
        private CheckDiskSpaceArgs _diskSpaceInfo;

        internal UIFirmwareUpdater(UIDevice device, FirmwareUpdater updater)
        {
            this._device = device;
            this._updater = updater;
            this.ResetState();
        }

        private void ResetState()
        {
            this._installFirmware = false;
            this._installGames = false;
            this._updateCollection = null;
            this._isUpdateAvailable = false;
            this._isCheckingForUpdates = false;
            this._updateInProgress = false;
            this._currentStep = null;
            this._lastCheckForUpdatesResult = HRESULT._S_OK;
            this._lastCheckForDiskSpaceResult = HRESULT._S_OK;
            this._lastCheckForUpdateErrorInfo = null;
            this._lastUpdateResult = HRESULT._S_OK;
            this._lastFirmwareUpdateErrorInfo = null;
            this._isSoftFailure = false;
            this._canCancel = true;
            this._needsCollectionRefresh = true;
            this._launchWizardIfUpdatesFound = false;
            this._fCancelInProgress = false;
            this._rollbackStarted = false;
            this._estimatedUpdateTime = 0;
            this._estimatedUpdateTimeInProgress = false;
            this._requiresSyncBeforeUpdate = false;
            this._updateOption = FirmwareUpdateOption.None;
            this._diskSpaceInfo = null;
            this.FirePropertyChanged("IsUpdateAvailable");
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                this._updater.Dispose();
                this._updater = null;
                this.UpdateCollection = null;
                this.CurrentStep = null;
            }
            base.OnDispose(disposing);
        }

        internal FirmwareUpdater FirmwareUpdater => this._updater;

        public UpdatePackageCollection UpdateCollection
        {
            get => this._updateCollection;
            private set
            {
                if (this._updateCollection == value)
                    return;
                if (this._updateCollection != null)
                    this._updateCollection.Dispose();
                this.UpdateEstimatedTime = 0;
                this._updateCollection = value;
                this.FirePropertyChanged(nameof(UpdateCollection));
            }
        }

        public UpdateStep CurrentStep
        {
            get => this._currentStep;
            private set
            {
                if (this._currentStep == value)
                    return;
                if (this._currentStep != null)
                    this._currentStep.Dispose();
                this._currentStep = value;
                this.FirePropertyChanged(nameof(CurrentStep));
                this.CanCancel = this._currentStep != null && this._currentStep.Cancelable;
            }
        }

        public bool CanCancel
        {
            get => this._canCancel;
            private set
            {
                if (this._canCancel == value)
                    return;
                this._canCancel = value;
                this.FirePropertyChanged(nameof(CanCancel));
            }
        }

        public bool IsCheckingForUpdates
        {
            get => this._isCheckingForUpdates;
            private set
            {
                if (this._isCheckingForUpdates == value)
                    return;
                this._isCheckingForUpdates = value;
                this.FirePropertyChanged(nameof(IsCheckingForUpdates));
            }
        }

        public bool NeedsCollectionRefresh
        {
            get => this._needsCollectionRefresh;
            private set
            {
                if (this._needsCollectionRefresh == value)
                    return;
                this._needsCollectionRefresh = value;
                this.FirePropertyChanged(nameof(NeedsCollectionRefresh));
            }
        }

        public FirmwareUpdateOption UpdateOption
        {
            get => this._updateOption;
            set
            {
                if (this._updateOption == value)
                    return;
                this._updateOption = value;
                this.FirePropertyChanged(nameof(UpdateOption));
            }
        }

        public CheckDiskSpaceArgs DiskSpaceInfo
        {
            get => this._diskSpaceInfo;
            private set
            {
                if (this._diskSpaceInfo == value)
                    return;
                this._diskSpaceInfo = value;
                this.FirePropertyChanged(nameof(DiskSpaceInfo));
            }
        }

        public void StartEstimatedUpdateTimeCalculation()
        {
            if (this.IsCheckingForUpdates || this._estimatedUpdateTimeInProgress || (this._updateCollection == null || this._updateCollection.FirmwarePackage == null))
                return;
            this._estimatedUpdateTimeInProgress = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.EstimatedUpdateTimeCalculationWorker), null);
        }

        private void EstimatedUpdateTimeCalculationWorker(object args)
        {
            int estimatedTime = 0;
            if (this._updateCollection != null)
            {
                FirmwareUpdatePackage firmwarePackage = this._updateCollection.FirmwarePackage;
                if (firmwarePackage != null)
                {
                    TimeSpan updateEstimatedTime = firmwarePackage.UpdateEstimatedTime;
                    this._estimatedUpdateTime = -1;
                    estimatedTime = (int)updateEstimatedTime.TotalMinutes;
                }
            }
            Application.DeferredInvoke(delegate
           {
               this.UpdateEstimatedTime = estimatedTime;
               this._estimatedUpdateTimeInProgress = false;
           }, null);
        }

        public void StartCheckForUpdates(bool forceServerRequest, bool launchWizardIfUpdatesFound)
        {
            if (this.IsCheckingForUpdates || !SyncControls.Instance.CurrentDeviceOverride.IsConnectedToClientPhysically)
                return;
            if (this._device.AllowChainedUpdates && !forceServerRequest)
            {
                if (this.IsUpdateAvailable && launchWizardIfUpdatesFound)
                    Application.DeferredInvoke(delegate
                   {
                       ZuneShell.DefaultInstance.NavigateToPage(new DeviceUpdateLandPage());
                   }, null);
                else
                    Application.DeferredInvoke(delegate
                   {
                       this._device.AllowChainedUpdates = false;
                       this._device.NavigateToDeviceSummaryAfterUpdate = false;
                       Shell.MainFrame.Device.Invoke();
                   }, null);
            }
            else
            {
                this.ResetState();
                this.IsCheckingForUpdates = true;
                this._lastCheckForUpdateErrorInfo = null;
                this._launchWizardIfUpdatesFound = launchWizardIfUpdatesFound;
                this.LastCheckForUpdatesResult = this._updater.StartCheckForUpdates(forceServerRequest, new DeferredInvokeHandler(this.OnCheckForUpdatesComplete));
                if (!this.LastCheckForUpdatesResult.IsError)
                    return;
                this.IsCheckingForUpdates = false;
            }
        }

        private void OnCheckForUpdatesComplete(object data) => Application.DeferredInvoke(delegate
       {
           CheckForUpdatesArgs checkForUpdatesArgs = (CheckForUpdatesArgs)data;
           this.LastCheckForUpdatesResult = checkForUpdatesArgs.ErrorInfo.HrStatus;
           this._lastCheckForUpdateErrorInfo = checkForUpdatesArgs.ErrorInfo;
           this._lastFirmwareUpdateErrorInfo = checkForUpdatesArgs.ErrorInfo;
           this.RequiresSyncBeforeUpdate = checkForUpdatesArgs.RequiresSyncBeforeUpdate;
           if (this.LastCheckForUpdatesResult.IsSuccess)
           {
               this.UpdateCollection = checkForUpdatesArgs.UpdatePackages;
               this.IsUpdateAvailable = this.UpdateCollection != null && this.UpdateCollection.Count > 0;
           }
           else
               this.UpdateCollection = null;
           this.NeedsCollectionRefresh = false;
           this.IsCheckingForUpdates = false;
           if (!this.IsUpdateAvailable || !this._launchWizardIfUpdatesFound || ZuneShell.DefaultInstance.CurrentPage is QuickplayPage && ClientConfiguration.FUE.ShowArtistChooser || (ZuneShell.DefaultInstance.Management.CurrentCategoryPage != null && ZuneShell.DefaultInstance.Management.CurrentCategoryPage.IsWizard || (Shell.SettingsFrame.IsCurrent || ZuneShell.DefaultInstance.CurrentPage is SetupLandPage)))
               return;
           ZuneShell.DefaultInstance.NavigateToPage(new DeviceUpdateLandPage());
       }, data);

        public void StartCheckForDiskSpace()
        {
            this._diskSpaceInfo = null;
            this.LastCheckForDiskSpaceResult = this._updater.StartCheckForDiskSpace(new DeferredInvokeHandler(this.OnCheckForDiskSpaceComplete));
        }

        private void OnCheckForDiskSpaceComplete(object data) => Application.DeferredInvoke(delegate
       {
           CheckDiskSpaceArgs checkDiskSpaceArgs = (CheckDiskSpaceArgs)data;
           this.LastCheckForDiskSpaceResult = checkDiskSpaceArgs.HrStatus;
           this.DiskSpaceInfo = checkDiskSpaceArgs;
       }, data);

        public void StartFirmwareUpdate()
        {
            if (this.UpdateInProgress)
                return;
            this.UpdateInProgress = true;
            this._currentStep = null;
            this._lastUpdateResult = HRESULT._S_OK;
            this._lastFirmwareUpdateErrorInfo = null;
            this._canCancel = true;
            this._rollbackStarted = false;
            this.LastUpdateResult = this._updater.StartFirmwareUpdate(this._updateCollection, new DeferredInvokeHandler(this.OnFirmwareUpdateStepBegin), new DeferredInvokeHandler(this.OnFirmwareUpdateStepProgress), new DeferredInvokeHandler(this.OnFirmwareUpdateCompleted), this.UpdateOption);
            if (this.LastUpdateResult.IsError)
                this.UpdateInProgress = false;
            else
                Shell.IgnoreAppNavigationsArgs = true;
        }

        private void OnFirmwareUpdateStepBegin(object data) => Application.DeferredInvoke(delegate
       {
           if (!this.UpdateInProgress)
               return;
           this.CurrentStep = ((FirmwareUpdateBeginArgs)data).Step;
       }, data);

        private void OnFirmwareUpdateStepProgress(object data) => Application.DeferredInvoke(delegate
       {
           if (!this.UpdateInProgress)
               return;
           this.CurrentStep = ((FirmwareUpdateProgressArgs)data).Step;
       }, data);

        private void OnFirmwareUpdateCompleted(object data) => Application.DeferredInvoke(delegate
       {
           if (!this.UpdateInProgress)
               return;
           FirmwareProcessCompleteArgs processCompleteArgs = (FirmwareProcessCompleteArgs)data;
           switch (processCompleteArgs.Action)
           {
               case CompletionAction.Reboot:
                   if (!this.IsLegacyZuneDevice())
                       break;
                   this.CanCancel = true;
                   break;
               case CompletionAction.Rollback:
                   this.CanCancel = false;
                   this.RollbackStarted = true;
                   break;
               case CompletionAction.Complete:
                   this.CurrentStep = null;
                   this.CanCancel = true;
                   this.NeedsCollectionRefresh = true;
                   this.UpdateInProgress = false;
                   this._fCancelInProgress = false;
                   this.LastUpdateResult = processCompleteArgs.ErrorInfo.HrStatus;
                   this._lastFirmwareUpdateErrorInfo = processCompleteArgs.ErrorInfo;
                   this.IsUpdateAvailable = this.LastUpdateResult.IsError && !this.IsSoftFailure;
                   Shell.IgnoreAppNavigationsArgs = false;
                   break;
           }
       }, data);

        public HRESULT LastCheckForUpdatesResult
        {
            get => this._lastCheckForUpdatesResult;
            private set
            {
                if (!(this._lastCheckForUpdatesResult != value))
                    return;
                this._lastCheckForUpdatesResult = value;
                this.FirePropertyChanged(nameof(LastCheckForUpdatesResult));
            }
        }

        public HRESULT LastCheckForDiskSpaceResult
        {
            get => this._lastCheckForDiskSpaceResult;
            private set
            {
                if (!(this._lastCheckForDiskSpaceResult != value))
                    return;
                this._lastCheckForDiskSpaceResult = value;
                this.FirePropertyChanged(nameof(LastCheckForDiskSpaceResult));
            }
        }

        public string LastCheckForUpdateErrorOverrideDescription => this._lastCheckForUpdateErrorInfo == null || string.IsNullOrEmpty(this._lastCheckForUpdateErrorInfo.Description) ? null : this._lastCheckForUpdateErrorInfo.Description;

        public string LastCheckForUpdateErrorWebHelpUrl => this._lastCheckForUpdateErrorInfo == null || string.IsNullOrEmpty(this._lastCheckForUpdateErrorInfo.Url) ? null : this._lastCheckForUpdateErrorInfo.Url;

        public HRESULT LastUpdateResult
        {
            get => this._lastUpdateResult;
            private set
            {
                if (!(this._lastUpdateResult != value))
                    return;
                this._lastUpdateResult = value;
                this.FirePropertyChanged(nameof(LastUpdateResult));
                this.IsSoftFailure = this._lastUpdateResult == HRESULT._NS_E_FIRMWARE_UPDATE_DISK_FULL;
            }
        }

        public string LastFirmwareUpdateErrorOverrideDescription => this._lastFirmwareUpdateErrorInfo == null || string.IsNullOrEmpty(this._lastFirmwareUpdateErrorInfo.Description) ? null : this._lastFirmwareUpdateErrorInfo.Description;

        public string LastFirmwareUpdateErrorWebHelpUrl => this._lastFirmwareUpdateErrorInfo == null || string.IsNullOrEmpty(this._lastFirmwareUpdateErrorInfo.Url) ? null : this._lastFirmwareUpdateErrorInfo.Url;

        public bool IsSoftFailure
        {
            get => this._isSoftFailure;
            private set
            {
                if (this._isSoftFailure == value)
                    return;
                this._isSoftFailure = value;
                this.FirePropertyChanged(nameof(IsSoftFailure));
            }
        }

        public bool UpdateInProgress
        {
            get => this._updateInProgress;
            private set
            {
                if (this._updateInProgress == value)
                    return;
                this._updateInProgress = value;
                this.FirePropertyChanged(nameof(UpdateInProgress));
                if (this._updateInProgress)
                {
                    if (this._updater.EnterContinuousPowerMode())
                        ;
                }
                else
                    this._updater.LeaveContinuousPowerMode();
            }
        }

        public bool RollbackStarted
        {
            get => this._rollbackStarted;
            set
            {
                if (this._rollbackStarted == value)
                    return;
                this._rollbackStarted = value;
                this.FirePropertyChanged(nameof(RollbackStarted));
            }
        }

        public bool IsUpdateAvailable
        {
            get => this._isUpdateAvailable;
            private set
            {
                if (this._isUpdateAvailable == value)
                    return;
                this._isUpdateAvailable = value;
                this.FirePropertyChanged(nameof(IsUpdateAvailable));
            }
        }

        public string AvailableFirmwareDescription
        {
            get
            {
                string str = null;
                if (this._updateCollection != null)
                {
                    FirmwareUpdatePackage firmwarePackage = this._updateCollection.FirmwarePackage;
                    if (firmwarePackage != null)
                        str = !SyncControls.Instance.CurrentDeviceOverride.SupportsBrandingType(DeviceBranding.WindowsPhone) ? StringParserHelper.FormatFirmwareVersion(firmwarePackage.Version) : firmwarePackage.Name;
                }
                return str;
            }
        }

        public string NewFirmwareEULAContent
        {
            get
            {
                string str = null;
                if (this._updateCollection != null)
                {
                    FirmwareUpdatePackage firmwarePackage = this._updateCollection.FirmwarePackage;
                    if (firmwarePackage != null)
                        str = firmwarePackage.EULAContent;
                }
                return str;
            }
        }

        public string MoreInfoURL
        {
            get
            {
                string str = null;
                if (this._updateCollection != null)
                {
                    FirmwareUpdatePackage firmwarePackage = this._updateCollection.FirmwarePackage;
                    if (firmwarePackage != null)
                        str = firmwarePackage.MoreInfoURL;
                }
                return str;
            }
        }

        public int UpdateEstimatedTime
        {
            get => this._estimatedUpdateTime;
            set
            {
                if (this._estimatedUpdateTime == value)
                    return;
                this._estimatedUpdateTime = value;
                this.FirePropertyChanged(nameof(UpdateEstimatedTime));
            }
        }

        public string NewFirmwareDescription
        {
            get
            {
                string str = string.Empty;
                if (this._updateCollection != null)
                {
                    FirmwareUpdatePackage firmwarePackage = this._updateCollection.FirmwarePackage;
                    if (firmwarePackage != null)
                        str = firmwarePackage.Description;
                }
                return str;
            }
        }

        public bool GamesPackIsOnlyUpdateAvailable => this._updateCollection != null && this._updateCollection.FirmwarePackage == null && this._updateCollection.GamesPackage != null;

        public void CancelFirmwareUpdate()
        {
            Shell.IgnoreAppNavigationsArgs = false;
            if (this._fCancelInProgress || !this.UpdateInProgress && !this.IsCheckingForUpdates)
                return;
            this._fCancelInProgress = true;
            this._updater.Cancel();
        }

        public bool InstallFirmware
        {
            get => this._installFirmware;
            set
            {
                if (this._installFirmware == value)
                    return;
                this._installFirmware = value;
                this.FirePropertyChanged(nameof(InstallFirmware));
                for (int index = 0; index < this._updateCollection.Count; ++index)
                {
                    if (this._updateCollection.get_Item(index).Type == FirmwareUpdateType.Firmware)
                        this._updateCollection.set_Selected(index, this._installFirmware);
                }
            }
        }

        public bool InstallGames
        {
            get => this._installGames;
            set
            {
                if (this._installGames == value)
                    return;
                this._installGames = value;
                this.FirePropertyChanged(nameof(InstallGames));
                for (int index = 0; index < this._updateCollection.Count; ++index)
                {
                    if (this._updateCollection.get_Item(index).Type == FirmwareUpdateType.Games)
                        this._updateCollection.set_Selected(index, this._installGames);
                }
            }
        }

        private bool IsLegacyZuneDevice() => SyncControls.Instance.CurrentDeviceOverride.Class == DeviceClass.Classic;

        public bool IsOnBatteryPower
        {
            get
            {
                bool fOnBatteryPower;
                return this._updater.CheckPowerRequirements(out fOnBatteryPower).IsError || fOnBatteryPower;
            }
        }

        public bool RequiresSyncBeforeUpdate
        {
            get => this._requiresSyncBeforeUpdate;
            private set
            {
                if (this._requiresSyncBeforeUpdate == value)
                    return;
                this._requiresSyncBeforeUpdate = value;
                this.FirePropertyChanged(nameof(RequiresSyncBeforeUpdate));
            }
        }
    }
}
