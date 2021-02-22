// Decompiled with JetBrains decompiler
// Type: ZuneUI.PassportPasswordStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System;
using System.Collections.Generic;

namespace ZuneUI
{
    public class PassportPasswordStep : AccountManagementStep
    {
        private bool _skipOnce;
        private bool _forceEnable;
        private bool _isUpgradeNeeded;
        private bool _isZuneAccount;
        private bool _isUnsupportedAccount;
        private bool _isUnsupportedRegion;
        private bool _lockEmail;
        private PassportIdentity _passportIdentity;
        private AccountUser _existingAccountUser;
        private Dictionary<int, PropertyDescriptor> _errorMappings;

        public PassportPasswordStep(
          Wizard owner,
          AccountManagementWizardState state,
          bool parentAccount)
          : base(owner, state, parentAccount)
        {
            if (parentAccount)
            {
                this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_START_PARENT_HEADER);
                this.DetailDescription = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_CREDENTIALS_PARENT);
            }
            else
            {
                this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_PASSPORT_STEP);
                this.DetailDescription = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_CREDENTIALS_HEADER);
            }
            this.DetailDescription = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_CREDENTIALS_HEADER);
            this.Initialize((WizardPropertyEditor)new PassportPasswordPropertyEditor());
        }

        public override string UI => "res://ZuneShellResources!CreatePassport.uix#PassportPasswordStep";

        public override bool IsEnabled
        {
            get
            {
                bool flag = base.IsEnabled && !this.CreatePassportStep.CreatedPassport;
                if (flag)
                {
                    flag = this.ForceEnable;
                    if (!flag)
                        flag = SignIn.Instance.SignedIn && this.ParentAccount && SignIn.Instance.IsParentallyControlled;
                    if (!flag)
                        flag = ((this.EmailSelectionStep.IsEnabled || this.EmailSelectionStep.PageLocked ? (!this.EmailSelectionStep.NeedsPassportId ? 1 : 0) : (false ? 1 : 0)) & (this._owner.CurrentPage != this || this.ParentAccount ? 1 : (!SignIn.Instance.SigningIn ? 1 : 0))) != 0;
                }
                return flag;
            }
        }

        public bool ForceEnable
        {
            get => this._forceEnable;
            set
            {
                if (this._forceEnable == value)
                    return;
                this._forceEnable = value;
                this.FirePropertyChanged(nameof(ForceEnable));
            }
        }

        internal bool SkipOnce
        {
            get => this._skipOnce;
            set
            {
                if (this._skipOnce == value)
                    return;
                this._skipOnce = value;
                this.FirePropertyChanged(nameof(SkipOnce));
            }
        }

        public bool LockEmail
        {
            get => this._lockEmail;
            internal set
            {
                if (this._lockEmail == value)
                    return;
                this._lockEmail = value;
                this.FirePropertyChanged(nameof(LockEmail));
            }
        }

        public string CommittedEmail
        {
            get => this.GetCommittedValue(PassportPasswordPropertyEditor.Email) as string;
            internal set
            {
                this.SetCommittedValue(PassportPasswordPropertyEditor.Email, (object)value);
                this.FirePropertyChanged(nameof(CommittedEmail));
            }
        }

        public string CommittedPassword
        {
            get => this.GetCommittedValue(PassportPasswordPropertyEditor.Password) as string;
            internal set
            {
                this.SetCommittedValue(PassportPasswordPropertyEditor.Password, (object)value);
                this.FirePropertyChanged(nameof(CommittedPassword));
            }
        }

        internal string UncommittedEmail => this.GetUncommittedValue(PassportPasswordPropertyEditor.Email) as string;

        internal string UncommittedPassword => this.GetUncommittedValue(PassportPasswordPropertyEditor.Password) as string;

        public bool CanCreateAccount
        {
            get
            {
                bool flag = this.ParentAccount && (this.State.CreatePassportParentStep.IsEnabled || this.State.CreatePassportParentStep.CreatedPassport) || !this.ParentAccount && (this.State.CreatePassportStep.IsEnabled || this.State.CreatePassportStep.CreatedPassport);
                if (!flag)
                    flag = !this.IsUnsupportedAccount && !this.IsUnsupportedRegion && !this.IsUpgradeNeeded && !this.IsZuneAccount;
                return flag;
            }
        }

        public bool IsUpgradeNeeded
        {
            get => this._isUpgradeNeeded;
            private set
            {
                if (this._isUpgradeNeeded == value)
                    return;
                this._isUpgradeNeeded = value;
                this.FirePropertyChanged(nameof(IsUpgradeNeeded));
                this.FirePropertyChanged("CanCreateAccount");
            }
        }

        public bool IsZuneAccount
        {
            get => this._isZuneAccount;
            private set
            {
                if (this._isZuneAccount == value)
                    return;
                this._isZuneAccount = value;
                this.FirePropertyChanged(nameof(IsZuneAccount));
                this.FirePropertyChanged("CanCreateAccount");
            }
        }

        public bool IsUnsupportedAccount
        {
            get => this._isUnsupportedAccount;
            private set
            {
                if (this._isUnsupportedAccount == value)
                    return;
                this._isUnsupportedAccount = value;
                this.FirePropertyChanged(nameof(IsUnsupportedAccount));
                this.FirePropertyChanged("CanCreateAccount");
            }
        }

        public bool IsUnsupportedRegion
        {
            get => this._isUnsupportedRegion;
            private set
            {
                if (this._isUnsupportedRegion == value)
                    return;
                this._isUnsupportedRegion = value;
                this.FirePropertyChanged(nameof(IsUnsupportedRegion));
                this.FirePropertyChanged("CanCreateAccount");
            }
        }

        public bool IsParentAccountNeeded
        {
            get
            {
                bool flag = false;
                if (this._existingAccountUser != null)
                    flag = this._existingAccountUser.AccountUserType != AccountUserType.Adult;
                return flag;
            }
        }

        public AccountUser ExistingAccountUser
        {
            get => this._existingAccountUser;
            private set
            {
                if (this._existingAccountUser == value)
                    return;
                this._existingAccountUser = value;
                this.FirePropertyChanged(nameof(ExistingAccountUser));
                this.FirePropertyChanged("IsParentAccountNeeded");
            }
        }

        public PassportIdentity PassportIdentity
        {
            get => this._passportIdentity;
            internal set
            {
                if (this._passportIdentity == value)
                    return;
                this._passportIdentity = value;
                this.FirePropertyChanged(nameof(PassportIdentity));
            }
        }

        internal override Dictionary<int, PropertyDescriptor> ErrorPropertyMappings
        {
            get
            {
                if (this._errorMappings == null)
                {
                    this._errorMappings = new Dictionary<int, PropertyDescriptor>(1);
                    this._errorMappings.Add(HRESULT._NS_E_PASSPORT_LOGIN_FAILED.Int, PassportPasswordPropertyEditor.Password);
                }
                return this._errorMappings;
            }
        }

        private EmailSelectionStep EmailSelectionStep => !this.ParentAccount ? this.State.EmailSelectionStep : this.State.EmailSelectionParentStep;

        private CreatePassportStep CreatePassportStep => !this.ParentAccount ? this.State.CreatePassportStep : this.State.CreatePassportParentStep;

        protected override void OnActivate()
        {
            this.ServiceDeactivationRequestsDone = false;
            string committedValue = this.EmailSelectionStep.GetCommittedValue(EmailSelectionPropertyEditor.Email) as string;
            string committedPassword = this.CommittedPassword;
            string committedEmail = this.CommittedEmail;
            if (committedValue != committedEmail || string.IsNullOrEmpty(committedPassword))
            {
                this.PassportIdentity = (PassportIdentity)null;
                this.CommittedEmail = committedValue;
                this.CommittedPassword = string.Empty;
            }
            if (!this.SkipOnce)
                return;
            this.SkipOnce = false;
            this._owner.MoveNext();
        }

        internal override bool OnMovingNext()
        {
            if (!this.ServiceDeactivationRequestsDone)
            {
                this.IsZuneAccount = false;
                this.IsUnsupportedAccount = false;
                this.IsUnsupportedRegion = false;
                this.IsUpgradeNeeded = false;
                this.StartDeactivationRequests((object)new PassportPasswordStep.ServiceData()
                {
                    Password = this.UncommittedPassword,
                    Email = this.UncommittedEmail,
                    PassportIdentity = (this.UncommittedPassword != this.CommittedPassword || this.UncommittedEmail != this.CommittedEmail ? (PassportIdentity)null : this.PassportIdentity)
                });
                this.LoadStatus = Shell.LoadString(StringId.IDS_ACCOUNT_SIGNING_INTO_WINDOWS_LIVE);
                return false;
            }
            if (!SignIn.Instance.SigningIn || this.ParentAccount)
            {
                this.LoadStatus = (string)null;
                return base.OnMovingNext();
            }
            this.LoadStatus = Shell.LoadString(StringId.IDS_PLEASE_WAIT);
            return false;
        }

        protected override void OnStartDeactivationRequests(object state)
        {
            PassportPasswordStep.ServiceData serviceData = (PassportPasswordStep.ServiceData)state;
            this.ValidatePassportAccount(ref serviceData);
            this.EndDeactivationRequests((object)serviceData);
        }

        protected override void OnEndDeactivationRequests(object args)
        {
            PassportPasswordStep.ServiceData serviceData = (PassportPasswordStep.ServiceData)args;
            this.PassportIdentity = serviceData.PassportIdentity;
            if (this.PassportIdentity == null || this.ParentAccount)
                return;
            SignIn.Instance.SignOut();
            SignIn.Instance.SignInStatusUpdatedEvent += new EventHandler(this.OnSignInStatusUpdatedEvent);
            SignIn.Instance.SignInUser(serviceData.Email, serviceData.Password);
            this.ExistingAccountUser = serviceData.ExistingAccountUser;
            if (this.ExistingAccountUser == null)
                return;
            this.State.SetPrivacySettings(this.ExistingAccountUser.AccountUserType);
        }

        private void ValidatePassportAccount(ref PassportPasswordStep.ServiceData serviceData)
        {
            HRESULT hr = HRESULT._S_OK;
            if (serviceData.PassportIdentity == null)
                hr = AccountManagementHelper.GetPassportIdentity(serviceData.Email, serviceData.Password, out serviceData.PassportIdentity);
            if (hr.IsSuccess)
            {
                ServiceError serviceError = (ServiceError)null;
                this.State.AccountManagement.GetAccount(serviceData.PassportIdentity, out serviceData.ExistingAccountUser, out serviceError);
            }
            if (!hr.IsError)
                return;
            this.SetError(hr, (ServiceError)null);
        }

        private void OnSignInStatusUpdatedEvent(object sender, EventArgs e)
        {
            if (!SignIn.Instance.SigningIn && (SignIn.Instance.SignInError.IsError || SignIn.Instance.SignedIn))
            {
                bool flag = false;
                if (SignIn.Instance.SignInError.IsError)
                {
                    if (SignIn.Instance.SignInError == HRESULT._NS_E_SIGNIN_TERMS_OF_SERVICE)
                        this.IsUpgradeNeeded = true;
                    else if (SignIn.Instance.SignInError == HRESULT._NS_E_SIGNIN_ACCOUNTS_NOT_XENON_USER)
                    {
                        flag = true;
                        this.IsUnsupportedAccount = true;
                    }
                    else if (SignIn.Instance.SignInError == HRESULT._NS_E_SIGNIN_WCMUSIC_ACCOUNT_NOT_ELIGIBLE)
                    {
                        flag = true;
                        this.IsUnsupportedRegion = true;
                    }
                    else if (SignIn.Instance.SignInError == HRESULT._NS_E_SIGNIN_INVALID_REGION)
                        this.IsZuneAccount = true;
                }
                else if (SignIn.Instance.SignedIn)
                    this.IsZuneAccount = true;
                SignIn.Instance.SignInStatusUpdatedEvent -= new EventHandler(this.OnSignInStatusUpdatedEvent);
                if (flag)
                {
                    this.SetError(SignIn.Instance.SignInError, (ServiceError)null);
                    this.NavigateToErrorHandler();
                }
                else if (this._owner.CurrentPage == this)
                    this._owner.MoveNext();
            }
            this.FirePropertyChanged("Enabled");
        }

        private struct ServiceData
        {
            public string Email;
            public string Password;
            public PassportIdentity PassportIdentity;
            public AccountUser ExistingAccountUser;
        }
    }
}
