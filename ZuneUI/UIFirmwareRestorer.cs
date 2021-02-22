// Decompiled with JetBrains decompiler
// Type: ZuneUI.UIFirmwareRestorer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;

namespace ZuneUI
{
    public class UIFirmwareRestorer : ModelItem
    {
        private FirmwareRestorer _restorer;
        private FirmwareRestorePoint _restorePoint;
        private bool _restoreInProgress;
        private bool _restorePointCollectionRefreshInProgress;
        private HRESULT _lastRestoreResult;
        private HRESULT _lastRefreshRestorePointResult;
        private UpdateStep _currentStep;
        private FirmwareUpdateErrorInfo _lastFirmwareRestoreErrorInfo;
        private bool _inRecoveryMode;
        private bool _needsCollectionRefresh = true;
        private bool _fCancelInProgress;

        internal UIFirmwareRestorer(FirmwareRestorer restorer, bool inRecoveryMode)
        {
            this._restorer = restorer;
            this._lastFirmwareRestoreErrorInfo = null;
            this._inRecoveryMode = inRecoveryMode;
        }

        internal FirmwareRestorer FirmwareRestorer => this._restorer;

        public int EstimatedRestoreTime
        {
            get
            {
                int num = 0;
                if (this._restorePoint != null && this._restorePoint.EstimatedRestoreTime != TimeSpan.Zero)
                    num = (int)this._restorePoint.EstimatedRestoreTime.TotalMinutes;
                return num;
            }
        }

        public void StartRefreshRestorePointCollection()
        {
            this.IsCheckingForRestorePoints = true;
            this.LastRefreshRestorePointResult = HRESULT._E_PENDING;
            if (!this._restorer.StartGetRestorePointCollection(new DeferredInvokeHandler(this.OnRefreshRestorePointCollectionComplete)).IsError)
                return;
            this.LastRefreshRestorePointResult = HRESULT._E_FAIL;
            this.IsCheckingForRestorePoints = false;
        }

        private void OnRefreshRestorePointCollectionComplete(object data) => Application.DeferredInvoke(delegate
       {
           FirmwareRestorePointCollection restorePointCollection = (FirmwareRestorePointCollection)data;
           if (restorePointCollection != null && restorePointCollection.Count > 0)
           {
               this._restorePoint = restorePointCollection.GetRestorePoint(0);
               this.LastRefreshRestorePointResult = HRESULT._S_OK;
           }
           else
           {
               this._restorePoint = null;
               this.LastRefreshRestorePointResult = HRESULT._ZUNE_E_NO_AVAILABLE_RESTORE_POINT;
           }
           this.NeedsCollectionRefresh = false;
           this.IsCheckingForRestorePoints = false;
       }, data);

        public FirmwareRestorePoint RestorePoint => this._restorePoint;

        public bool IsCheckingForRestorePoints
        {
            get => this._restorePointCollectionRefreshInProgress;
            private set
            {
                if (this._restorePointCollectionRefreshInProgress == value)
                    return;
                this._restorePointCollectionRefreshInProgress = value;
                this.FirePropertyChanged(nameof(IsCheckingForRestorePoints));
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

        public bool RestoreInProgress
        {
            get => this._restoreInProgress;
            private set
            {
                if (this._restoreInProgress == value)
                    return;
                this._restoreInProgress = value;
                this.FirePropertyChanged(nameof(RestoreInProgress));
                if (this._restoreInProgress)
                {
                    if (this._restorer.EnterContinuousPowerMode())
                        ;
                }
                else
                    this._restorer.LeaveContinuousPowerMode();
            }
        }

        public HRESULT LastRestoreResult
        {
            get => this._lastRestoreResult;
            private set
            {
                if (!(this._lastRestoreResult != value))
                    return;
                this._lastRestoreResult = value;
                this.FirePropertyChanged(nameof(LastRestoreResult));
            }
        }

        public HRESULT LastRefreshRestorePointResult
        {
            get => this._lastRefreshRestorePointResult;
            private set
            {
                if (!(this._lastRefreshRestorePointResult != value))
                    return;
                this._lastRefreshRestorePointResult = value;
                this.FirePropertyChanged(nameof(LastRefreshRestorePointResult));
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
            }
        }

        public string LastFirmwareRestoreErrorDescription
        {
            get
            {
                string str = string.Empty;
                if (this._lastFirmwareRestoreErrorInfo != null && !string.IsNullOrEmpty(this._lastFirmwareRestoreErrorInfo.Description))
                    str = this._lastFirmwareRestoreErrorInfo.Description;
                else if (this.RestorePoint == null)
                    str = !this._inRecoveryMode ? Shell.LoadString(StringId.IDS_DEVICE_RESTORE_ERROR_NO_POINTS) : Shell.LoadString(StringId.IDS_DEVICE_RECOVERY_ERROR_NO_POINTS);
                return str;
            }
        }

        public string LastFirmwareRestoreErrorWebHelpUrl => this._lastFirmwareRestoreErrorInfo == null || string.IsNullOrEmpty(this._lastFirmwareRestoreErrorInfo.Url) ? null : this._lastFirmwareRestoreErrorInfo.Url;

        public bool IsOnBatteryPower
        {
            get
            {
                bool fOnBatteryPower;
                return this._restorer.CheckPowerRequirements(out fOnBatteryPower).IsError || fOnBatteryPower;
            }
        }

        public void CancelFirmwareRestore()
        {
            Shell.IgnoreAppNavigationsArgs = false;
            if (this._fCancelInProgress || !this.RestoreInProgress && !this.IsCheckingForRestorePoints)
                return;
            this._fCancelInProgress = true;
            this._restorer.Cancel();
        }

        public void StartRestore()
        {
            if (this.RestoreInProgress)
                return;
            this.RestoreInProgress = true;
            this._lastFirmwareRestoreErrorInfo = null;
            this.LastRestoreResult = this._restorer.StartFirmwareRestore(this.RestorePoint, new DeferredInvokeHandler(this.OnFirmwareRestoreStepBegin), new DeferredInvokeHandler(this.OnFirmwareRestoreStepProgress), new DeferredInvokeHandler(this.OnFirmwareRestoreCompleted));
            if (this.LastRestoreResult.IsError)
                this.RestoreInProgress = false;
            else
                Shell.IgnoreAppNavigationsArgs = true;
        }

        private void OnFirmwareRestoreStepBegin(object data) => Application.DeferredInvoke(delegate
       {
           if (!this.RestoreInProgress)
               return;
           this.CurrentStep = ((FirmwareUpdateBeginArgs)data).Step;
       }, data);

        private void OnFirmwareRestoreStepProgress(object data) => Application.DeferredInvoke(delegate
       {
           if (!this.RestoreInProgress)
               return;
           this.CurrentStep = ((FirmwareUpdateProgressArgs)data).Step;
       }, data);

        private void OnFirmwareRestoreCompleted(object data) => Application.DeferredInvoke(delegate
       {
           if (!this.RestoreInProgress)
               return;
           FirmwareProcessCompleteArgs processCompleteArgs = (FirmwareProcessCompleteArgs)data;
           switch (processCompleteArgs.Action)
           {
               case CompletionAction.Complete:
                   this.CurrentStep = null;
                   this.NeedsCollectionRefresh = true;
                   this.RestoreInProgress = false;
                   this._fCancelInProgress = false;
                   this.LastRestoreResult = processCompleteArgs.ErrorInfo.HrStatus;
                   this._lastFirmwareRestoreErrorInfo = processCompleteArgs.ErrorInfo;
                   Shell.IgnoreAppNavigationsArgs = false;
                   break;
           }
       }, data);
    }
}
