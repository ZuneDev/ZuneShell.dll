// Decompiled with JetBrains decompiler
// Type: ZuneUI.AccountManagementStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.ErrorMapperApi;
using Microsoft.Zune.Service;
using System.Collections.Generic;
using System.Threading;

namespace ZuneUI
{
    public abstract class AccountManagementStep : WizardPropertyEditorPage
    {
        private AccountManagementWizardState _state;
        private bool _parentAccount;
        private bool _serviceActivationRequestsDone;
        private bool _serviceDeactivationRequestsDone;
        private bool _serviceRequestsWorking;
        private bool _requireSignIn;
        private string _nextTextOverride;
        private string _finishTextOverride;

        public AccountManagementStep(
          Wizard owner,
          AccountManagementWizardState state,
          bool parentAccount)
          : base(owner)
        {
            this._parentAccount = parentAccount;
            this._state = state;
            this._serviceActivationRequestsDone = true;
            this._serviceDeactivationRequestsDone = true;
            this._serviceRequestsWorking = false;
            this.EnableVerticalScrolling = true;
        }

        public override bool IsEnabled => (!this.RequireSignIn || SignIn.Instance.SignedIn) && (this._owner.CurrentPage != this || !this.ServiceRequestWorking);

        protected bool RequireSignIn
        {
            get => this._requireSignIn;
            set
            {
                if (this._requireSignIn == value)
                    return;
                this._requireSignIn = value;
                this.FirePropertyChanged(nameof(RequireSignIn));
                this.FirePropertyChanged("IsEnabled");
            }
        }

        public AccountManagementWizardState State => this._state;

        public string NextTextOverride
        {
            get => this._nextTextOverride;
            set
            {
                if (!(this._nextTextOverride != value))
                    return;
                this._nextTextOverride = value;
                this.FirePropertyChanged(nameof(NextTextOverride));
            }
        }

        public string FinishTextOverride
        {
            get => this._finishTextOverride;
            set
            {
                if (!(this._finishTextOverride != value))
                    return;
                this._finishTextOverride = value;
                this.FirePropertyChanged(nameof(FinishTextOverride));
            }
        }

        public bool ServiceActivationRequestsDone
        {
            get => this._serviceActivationRequestsDone;
            protected set
            {
                if (this._serviceActivationRequestsDone == value)
                    return;
                this._serviceActivationRequestsDone = value;
                this.FirePropertyChanged("ServiceDeactivationRequestsDone");
            }
        }

        public bool ServiceDeactivationRequestsDone
        {
            get => this._serviceDeactivationRequestsDone;
            protected set
            {
                if (this._serviceDeactivationRequestsDone == value)
                    return;
                this._serviceDeactivationRequestsDone = value;
                this.FirePropertyChanged(nameof(ServiceDeactivationRequestsDone));
            }
        }

        public bool ServiceRequestWorking
        {
            get => this._serviceRequestsWorking;
            private set
            {
                if (this._serviceRequestsWorking == value)
                    return;
                this._serviceRequestsWorking = value;
                this.FirePropertyChanged(nameof(ServiceRequestWorking));
                if (this._owner.CurrentPage != this)
                    return;
                this.FirePropertyChanged("IsEnabled");
            }
        }

        public bool ParentAccount => this._parentAccount;

        internal virtual Dictionary<int, PropertyDescriptor> ErrorPropertyMappings => (Dictionary<int, PropertyDescriptor>)null;

        internal virtual ErrorMapperResult GetMappedErrorDescriptionAndUrl(HRESULT hr) => ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hr.Int, eErrorCondition.eEC_None);

        internal void SetError(HRESULT hr, ServiceError serviceError) => this._owner.SetError(hr, new AccountManagementErrorState(this.ParentAccount, serviceError));

        internal bool HandleError()
        {
            HRESULT error = this._owner.Error;
            AccountManagementErrorState errorState = null;
            if (this._owner is AccountManagementWizard)
                errorState = ((AccountManagementWizard)this._owner).LastErrorState;
            bool flag = error.IsSuccess && errorState == null;
            if (!flag)
            {
                flag = this.HandleError(error, errorState);
                if (flag)
                    this._owner.ResetError();
            }
            return flag;
        }

        internal virtual bool HandleError(HRESULT hr, AccountManagementErrorState errorState)
        {
            bool flag1 = hr.IsSuccess && errorState == null;
            bool flag2 = (errorState == null || errorState.ParentAccount == this.ParentAccount) && this.IsEnabled;
            if (!flag1 && flag2)
            {
                if (this.SetExternalPropertyError(hr, null))
                    flag1 = true;
                if (errorState != null && errorState.ServiceError != null)
                {
                    if (this.SetExternalPropertyError(errorState.ServiceError.RootError, null))
                        flag1 = true;
                    if (errorState.ServiceError.PropertyErrors != null)
                    {
                        foreach (PropertyError propertyError in errorState.ServiceError.PropertyErrors)
                        {
                            if (this.SetExternalPropertyError(propertyError.Hr, propertyError.Name))
                                flag1 = true;
                        }
                    }
                }
                if (flag1)
                {
                    this.ShowGenericErrorStatus();
                    this.ShowValidation();
                }
            }
            return flag1;
        }

        internal bool NavigateToErrorHandler()
        {
            bool flag = false;
            if (this._owner is AccountManagementWizard)
            {
                ((AccountManagementWizard)this._owner).NavigateToErrorHandler();
                flag = true;
            }
            return flag;
        }

        internal override sealed void Activate()
        {
            base.Activate();
            if (!this.HandleError() && this.NavigateToErrorHandler())
                return;
            this.OnActivate();
        }

        internal override bool OnMovingNext()
        {
            if (this.ServiceDeactivationRequestsDone)
            {
                this.StatusMessage = null;
                return base.OnMovingNext();
            }
            this.StartDeactivationRequests(null);
            return false;
        }

        protected bool SetExternalPropertyError(HRESULT hr, string propertyName)
        {
            string str = null;
            bool flag = false;
            if (hr.IsError && this.ErrorPropertyMappings != null)
            {
                ErrorMapperResult descriptionAndUrl = this.GetMappedErrorDescriptionAndUrl(hr);
                if (descriptionAndUrl != null && this.ErrorPropertyMappings.ContainsKey(descriptionAndUrl.Hr))
                {
                    flag = true;
                    PropertyDescriptor errorPropertyMapping = this.ErrorPropertyMappings[descriptionAndUrl.Hr];
                    if (errorPropertyMapping == null || !this.SetExternalError(errorPropertyMapping, new HRESULT(descriptionAndUrl.Hr)))
                        str = descriptionAndUrl.Description;
                }
                else if (!string.IsNullOrEmpty(propertyName) && descriptionAndUrl != null)
                    flag = this.SetExternalError(propertyName, new HRESULT(descriptionAndUrl.Hr));
            }
            if (flag)
                this.StatusMessage = str;
            return flag;
        }

        protected virtual void OnActivate()
        {
            if (this.ServiceActivationRequestsDone)
                return;
            this.StartActivationRequests(null);
        }

        protected void StartActivationRequests(object state)
        {
            if (this.ServiceRequestWorking)
                return;
            this.ServiceRequestWorking = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.AsyncStartActivationRequests), state);
        }

        private void AsyncStartActivationRequests(object state) => this.OnStartActivationRequests(state);

        protected virtual void OnStartActivationRequests(object state) => this.EndActivationRequests(null);

        protected void EndActivationRequests(object args) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredEndActivationRequests), args);

        private void DeferredEndActivationRequests(object args)
        {
            this.OnEndActivationRequests(args);
            this.ServiceActivationRequestsDone = true;
            this.ServiceRequestWorking = false;
        }

        protected virtual void OnEndActivationRequests(object args)
        {
        }

        protected void StartDeactivationRequests(object state)
        {
            if (this.ServiceRequestWorking)
                return;
            this.StatusMessage = null;
            this.ServiceRequestWorking = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.AsyncStartDeactivationRequests), state);
        }

        private void AsyncStartDeactivationRequests(object state) => this.OnStartDeactivationRequests(state);

        protected virtual void OnStartDeactivationRequests(object state) => this.EndDeactivationRequests(null);

        protected void EndDeactivationRequests(object args) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DerferredEndDeactivationRequests), args);

        private void DerferredEndDeactivationRequests(object args)
        {
            this.ServiceDeactivationRequestsDone = true;
            this.ServiceRequestWorking = false;
            this.OnEndDeactivationRequests(args);
            this._owner.MoveNext();
        }

        protected virtual void OnEndDeactivationRequests(object args)
        {
        }
    }
}
