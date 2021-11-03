// Decompiled with JetBrains decompiler
// Type: ZuneUI.AsyncOperation
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.ErrorMapperApi;

namespace ZuneUI
{
    internal abstract class AsyncOperation
    {
        protected const int IdleState = -1;
        protected const int FinishedState = -2;
        protected UIDevice _device;
        protected string _error;
        protected string _detailedError;
        protected HRESULT _hr;
        private AOComplete _completeFunc;
        private WirelessStates[] _states;
        private bool _fListening;
        private bool _fTryToCancel;
        private bool _fCanceled;
        protected static int _iCurrentState = -1;

        public string Error => this.Finished ? this._error : null;

        public string DetailedError => this.Finished ? this._detailedError : null;

        public HRESULT Hr => this.Finished ? this._hr : HRESULT._E_PENDING;

        public bool Canceled => this.Finished && this._fCanceled;

        protected abstract WirelessStateResults DoStep(WirelessStates state);

        public virtual void Cancel()
        {
            if (this.Idle || this.Finished)
                return;
            this._fTryToCancel = true;
        }

        protected virtual void EndOperation(WirelessStateResults result)
        {
        }

        protected virtual void AddListeners()
        {
        }

        protected virtual void RemoveListeners()
        {
        }

        protected bool Idle => _iCurrentState == -1;

        protected bool Finished => _iCurrentState == -2;

        protected WirelessStateResults StartOperation(
          UIDevice device,
          AOComplete completeFunc,
          WirelessStates[] states)
        {
            WirelessStateResults result = WirelessStateResults.Error;
            if (!this.Idle)
                return WirelessStateResults.NotAvailable;
            this.ResetState();
            this._device = device;
            this._states = states;
            this._completeFunc = completeFunc;
            _iCurrentState = 0;
            if (device.IsConnectedToClient)
            {
                this.AddListenersInternal();
                result = this.DoNextStep();
            }
            if (result != WirelessStateResults.Success)
                this.EndOperationInternal(result);
            return WirelessStateResults.Success;
        }

        protected void StepComplete(WirelessStateResults result)
        {
            ++_iCurrentState;
            if (result == WirelessStateResults.Success)
                result = this.DoNextStep();
            if (result == WirelessStateResults.Success)
                return;
            this.EndOperationInternal(result);
        }

        protected void ClearResult()
        {
            this._hr = HRESULT._S_OK;
            this._detailedError = null;
        }

        protected void SetResult(HRESULT hr)
        {
            this._hr = hr;
            if (!hr.IsError)
                return;
            ErrorMapperResult descriptionAndUrl = ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hr.Int);
            if (descriptionAndUrl.Hr == (uint)HRESULT._E_FAIL.Int || descriptionAndUrl.Hr == (uint)HRESULT._NS_E_WMP_UNKNOWN_ERROR.Int)
                return;
            this._detailedError = descriptionAndUrl.Description;
        }

        private void EndOperationInternal(WirelessStateResults result)
        {
            this.RemoveListenersInternal();
            if (result == WirelessStateResults.Canceled)
                this._fCanceled = true;
            this.EndOperation(result);
            _iCurrentState = -2;
            if (this._completeFunc != null)
                this._completeFunc(result == WirelessStateResults.Finished);
            _iCurrentState = -1;
        }

        private WirelessStateResults DoNextStep()
        {
            if (_iCurrentState == this._states.Length)
                return WirelessStateResults.Finished;
            if (_iCurrentState > this._states.Length)
                return WirelessStateResults.Error;
            if (this._fTryToCancel)
                return WirelessStateResults.Canceled;
            WirelessStateResults result = this.DoStep(this._states[_iCurrentState]);
            if (result != WirelessStateResults.Success)
                this.EndOperationInternal(result);
            return result;
        }

        private void ResetState()
        {
            if (!this.Idle && !this.Finished)
                return;
            this._error = null;
            this._detailedError = null;
            this._fTryToCancel = false;
            this._fCanceled = false;
            this._fListening = false;
            this._hr = HRESULT._S_OK;
        }

        private void AddListenersInternal()
        {
            if (this._fListening)
                return;
            this.AddListeners();
            this._fListening = true;
        }

        private void RemoveListenersInternal()
        {
            if (!this._fListening)
                return;
            this.RemoveListeners();
            this._fListening = false;
        }

        public delegate void AOComplete(bool success);
    }
}
