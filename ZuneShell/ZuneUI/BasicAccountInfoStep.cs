// Decompiled with JetBrains decompiler
// Type: ZuneUI.BasicAccountInfoStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.ErrorMapperApi;
using Microsoft.Zune.Service;
using System;
using System.Collections.Generic;

namespace ZuneUI
{
    public class BasicAccountInfoStep : RegionInfoStep
    {
        private AccountUserType _newAccountType;
        private string _termsOfService;
        private string _termsOfServiceUrl;
        private string _privacyUrl;
        private Dictionary<int, PropertyDescriptor> _errorMappings;

        public BasicAccountInfoStep(
          Wizard owner,
          AccountManagementWizardState state,
          bool parentAccount)
          : base(owner, state, parentAccount)
        {
            this.EnableVerticalScrolling = false;
            this.LoadLanguages = true;
            this.NextTextOverride = Shell.LoadString(StringId.IDS_I_ACCEPT_BUTTON);
            this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ACCOUNT_INFO_STEP);
            this.Initialize(new BasicAccountInfoPropertyEditor());
        }

        public override bool IsEnabled
        {
            get
            {
                bool flag = base.IsEnabled;
                if (flag)
                    flag = ((this._owner.CurrentPage != this ? (true ? 1 : 0) : (this.TermsOfService != null ? 1 : 0)) & (this.State.CreatePassportStep.IsEnabled || this.State.PassportPasswordStep.CanCreateAccount ? 1 : (this.State.PassportPasswordStep.IsUpgradeNeeded ? 1 : 0))) != 0;
                return flag;
            }
        }

        protected override PropertyDescriptor CountryDescriptor => BasicAccountInfoPropertyEditor.Country;

        protected override PropertyDescriptor LanguageDescriptor => BasicAccountInfoPropertyEditor.Language;

        public bool IsParentAccountNeeded => this._newAccountType != AccountUserType.Adult;

        public AccountUserType NewAccounType
        {
            get => this._newAccountType;
            private set
            {
                if (this._newAccountType == value)
                    return;
                this._newAccountType = value;
                this.FirePropertyChanged(nameof(NewAccounType));
                this.FirePropertyChanged("IsParentAccountNeeded");
            }
        }

        public string TermsOfService
        {
            get => this._termsOfService;
            private set
            {
                if (!(this._termsOfService != value))
                    return;
                this._termsOfService = value;
                this.FirePropertyChanged(nameof(TermsOfService));
                this.FirePropertyChanged("IsEnabled");
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

        public override string UI => "res://ZuneShellResources!AccountInfo.uix#BasicAccountInfoStep";

        internal override Dictionary<int, PropertyDescriptor> ErrorPropertyMappings
        {
            get
            {
                if (this._errorMappings == null)
                {
                    this._errorMappings = new Dictionary<int, PropertyDescriptor>(4);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_BIRTH_YEAR_INVALID.Int, BasicAccountInfoPropertyEditor.Birthday);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_BIRTH_DAY_INVALID.Int, BasicAccountInfoPropertyEditor.Birthday);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_BIRTH_MONTH_INVALID.Int, BasicAccountInfoPropertyEditor.Birthday);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_BIRTH_DATE_FUTURE.Int, BasicAccountInfoPropertyEditor.Birthday);
                    this._errorMappings.Add(HRESULT._ZEST_E_INVALID_ARG_LANGUAGE.Int, BasicAccountInfoPropertyEditor.Language);
                }
                return this._errorMappings;
            }
        }

        protected override void OnCountryChanged()
        {
            this.SetPropertyState(BasicAccountInfoPropertyEditor.PostalCode, SelectedCountry);
            this.SetPropertyState(BasicAccountInfoPropertyEditor.Birthday, SelectedLocale);
            this.TermsOfService = null;
            this.TermsOfServiceUrl = TermsOfServiceStep.GetTermsOfServiceUrl(this.SelectedLocale);
            this.PrivacyUrl = TermsOfServiceStep.GetPrivacyUrl(this.SelectedLocale);
            this.SetUncommittedValue(BasicAccountInfoPropertyEditor.Birthday, this.GetCommittedValue(BasicAccountInfoPropertyEditor.Birthday));
            if (!this.ServiceActivationRequestsDone)
                return;
            this.Activate();
        }

        protected override void OnLanguageChanged()
        {
            this.SetPropertyState(BasicAccountInfoPropertyEditor.Birthday, SelectedLocale);
            this.TermsOfService = null;
            this.TermsOfServiceUrl = TermsOfServiceStep.GetTermsOfServiceUrl(this.SelectedLocale);
            this.PrivacyUrl = TermsOfServiceStep.GetPrivacyUrl(this.SelectedLocale);
            this.SetUncommittedValue(BasicAccountInfoPropertyEditor.Birthday, this.GetCommittedValue(BasicAccountInfoPropertyEditor.Birthday));
            if (!this.ServiceActivationRequestsDone)
                return;
            this.Activate();
        }

        protected override void OnActivate()
        {
            this.ServiceDeactivationRequestsDone = false;
            this.SetUncommittedValue(BasicAccountInfoPropertyEditor.Birthday, this.GetCommittedValue(BasicAccountInfoPropertyEditor.Birthday));
            this.ServiceActivationRequestsDone = this.TermsOfService != null;
            base.OnActivate();
        }

        internal override bool OnMovingNext()
        {
            if (this.ServiceDeactivationRequestsDone)
                return base.OnMovingNext();
            DateTime? uncommittedValue = (DateTime?)this.GetUncommittedValue(BasicAccountInfoPropertyEditor.Birthday);
            this.StartDeactivationRequests(new WorkerThreadData()
            {
                Birthday = uncommittedValue.Value,
                SelectedCountry = this.SelectedCountry
            });
            return false;
        }

        internal override ErrorMapperResult GetMappedErrorDescriptionAndUrl(HRESULT hr) => ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hr.Int, eErrorCondition.eEC_WinLive);

        protected override void OnStartActivationRequests(object state)
        {
            base.OnStartActivationRequests(state);
            if (!(state is RegionServiceData regionServiceData) || string.IsNullOrEmpty(regionServiceData.SelectedCountry))
                return;
            this.EndActivationRequests(this.ObtainTermsOfService(regionServiceData.SelectedLanguage, regionServiceData.SelectedCountry));
        }

        protected override void OnEndActivationRequests(object args)
        {
            if (args == null)
                this.NavigateToErrorHandler();
            else if (args is string)
                this.TermsOfService = (string)args;
            else
                base.OnEndActivationRequests(args);
        }

        protected override void OnStartDeactivationRequests(object state) => this.EndDeactivationRequests(this.ObtainNewAccountType((WorkerThreadData)state));

        protected override void OnEndDeactivationRequests(object args)
        {
            this.NewAccounType = (AccountUserType)args;
            this.State.SetPrivacySettings(this.NewAccounType);
        }

        private AccountUserType ObtainNewAccountType(
          WorkerThreadData data)
        {
            AccountUserType accountUserType = AccountUserType.Unknown;
            AccountCountry country = AccountCountryList.Instance.GetCountry(data.SelectedCountry);
            if (country != null)
            {
                int age = this.ObtainAge(data.Birthday);
                accountUserType = age < country.AdultAge ? (age < country.TeenagerAge ? AccountUserType.ChildWithoutSocial : AccountUserType.ChildWithSocial) : AccountUserType.Adult;
            }
            return accountUserType;
        }

        private int ObtainAge(DateTime birthday)
        {
            DateTime today = DateTime.Today;
            int num = today.Year - birthday.Year;
            if (today.Month < birthday.Month || today.Month == birthday.Month && today.Day < birthday.Day)
                --num;
            return num;
        }

        private string ObtainTermsOfService(string language, string country)
        {
            string termsOfService1 = null;
            HRESULT termsOfService2 = this.State.AccountManagement.GetTermsOfService(language, country, out termsOfService1);
            if (termsOfService2.IsError)
                this.SetError(termsOfService2, null);
            return termsOfService1;
        }

        public class WorkerThreadData
        {
            public DateTime Birthday;
            public string SelectedCountry;
        }
    }
}
