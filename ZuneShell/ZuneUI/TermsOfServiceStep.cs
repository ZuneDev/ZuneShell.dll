// Decompiled with JetBrains decompiler
// Type: ZuneUI.TermsOfServiceStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System.Collections;

namespace ZuneUI
{
    public class TermsOfServiceStep : AccountManagementStep
    {
        private string _termsOfService;
        private string _termsOfServiceUrl;
        private string _privacyUrl;
        private string _username;
        private string _password;
        private PassportIdentity _passportIdentity;

        public TermsOfServiceStep(Wizard owner, AccountManagementWizardState state, bool parentAccount)
          : base(owner, state, parentAccount)
        {
            this.NextTextOverride = Shell.LoadString(StringId.IDS_I_ACCEPT_BUTTON);
            this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_TOS_STEP_TITLE);
            this.Initialize(null);
        }

        public override string UI => "res://ZuneShellResources!AccountInfo.uix#TermsOfServiceStep";

        public string TermsOfService
        {
            get => this._termsOfService;
            private set
            {
                if (!(this._termsOfService != value))
                    return;
                this._termsOfService = value;
                this.FirePropertyChanged(nameof(TermsOfService));
            }
        }

        public string TermsOfServiceUrl
        {
            get => this._termsOfServiceUrl;
            private set
            {
                if (!(this._termsOfServiceUrl != value))
                    return;
                this._termsOfServiceUrl = value;
                this.FirePropertyChanged(nameof(TermsOfServiceUrl));
            }
        }

        public string PrivacyUrl
        {
            get => this._privacyUrl;
            private set
            {
                if (!(this._privacyUrl != value))
                    return;
                this._privacyUrl = value;
                this.FirePropertyChanged(nameof(PrivacyUrl));
            }
        }

        public string Username
        {
            get => this._username;
            set
            {
                if (!(this._username != value))
                    return;
                this._username = value;
                this.FirePropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => this._password;
            set
            {
                if (value == null || value == SignIn.Instance.PseudoPassword)
                    value = string.Empty;
                if (!(this._password != value))
                    return;
                this._password = value;
                this.FirePropertyChanged(nameof(Password));
            }
        }

        public PassportIdentity PassportIdentity
        {
            get => this._passportIdentity;
            private set
            {
                if (this._passportIdentity == value)
                    return;
                this._passportIdentity = value;
                this.FirePropertyChanged(nameof(PassportIdentity));
            }
        }

        internal static string GetTermsOfServiceUrl(string locale) => CultureHelper.AppendFwlinkCulture(Shell.LoadString(StringId.IDS_TERMS_OF_SERVICE_URL), locale);

        internal static string GetPrivacyUrl(string locale) => CultureHelper.AppendFwlinkCulture(Shell.LoadString(StringId.IDS_PRIVACY_STATEMENT_URL), locale);

        protected override void OnActivate()
        {
            this.ServiceActivationRequestsDone = this.TermsOfService != null && this.PassportIdentity != null;
            if (this.ServiceActivationRequestsDone)
                return;
            this.StartActivationRequests(new TermsOfServiceData()
            {
                Username = this.Username,
                Password = this.Password
            });
        }

        protected override void OnStartActivationRequests(object state)
        {
            HRESULT hr = HRESULT._S_OK;
            TermsOfServiceData termsOfServiceData = (TermsOfServiceData)state;
            if (hr.IsSuccess)
                hr = AccountManagementHelper.GetPassportIdentity(termsOfServiceData.Username, termsOfServiceData.Password, out termsOfServiceData.PassportIdentity);
            ServiceError serviceError = null;
            if (hr.IsSuccess)
                hr = this.State.AccountManagement.GetAccount(termsOfServiceData.PassportIdentity, out termsOfServiceData.AccountUser, out serviceError);
            string language = null;
            string country = null;
            if (hr.IsSuccess)
            {
                RegionInfoStep.GetLanguageAndCountry(termsOfServiceData.AccountUser.Locale, out language, out country);
                if (string.IsNullOrEmpty(country))
                    hr = HRESULT._E_FAIL;
            }
            if (hr.IsSuccess)
                hr = this.State.AccountManagement.GetTermsOfService(language, country, out termsOfServiceData.TermsOfService);
            if (hr.IsError)
                this.SetError(hr, serviceError);
            this.EndActivationRequests(termsOfServiceData);
        }

        protected override void OnEndActivationRequests(object args)
        {
            TermsOfServiceData termsOfServiceData = (TermsOfServiceData)args;
            this.TermsOfService = termsOfServiceData.TermsOfService;
            this.PassportIdentity = termsOfServiceData.PassportIdentity;
            if (termsOfServiceData.AccountUser == null)
                return;
            this.TermsOfServiceUrl = GetTermsOfServiceUrl(termsOfServiceData.AccountUser.Locale);
            this.PrivacyUrl = GetPrivacyUrl(termsOfServiceData.AccountUser.Locale);
            this.State.PrivacyInfoParentStep.CommittedSettings = termsOfServiceData.AccountUser.AccountSettings;
            this.State.PrivacyInfoStep.CommittedSettings = termsOfServiceData.AccountUser.AccountSettings;
            this.State.SetPrivacySettings(termsOfServiceData.AccountUser.AccountUserType);
            if (termsOfServiceData.AccountUser.AccountUserType != AccountUserType.ChildWithoutSocial && termsOfServiceData.AccountUser.AccountUserType != AccountUserType.ChildWithSocial || this.State.PassportPasswordParentStep.IsEnabled || this._owner.Pages == null)
                return;
            this.State.PassportPasswordParentStep.ForceEnable = true;
            int num = -1;
            foreach (object page in _owner.Pages)
            {
                ++num;
                if (page == this.State.PassportPasswordParentStep)
                    break;
            }
            if (num >= this._owner.CurrentPageIndex || num >= this._owner.Pages.Count)
                return;
            this._owner.CurrentPageIndex = num;
        }

        private class TermsOfServiceData
        {
            public string Username;
            public string Password;
            public string TermsOfService;
            public PassportIdentity PassportIdentity;
            public AccountUser AccountUser;
        }
    }
}
