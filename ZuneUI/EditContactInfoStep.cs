// Decompiled with JetBrains decompiler
// Type: ZuneUI.EditContactInfoStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System.Collections.Generic;

namespace ZuneUI
{
    public class EditContactInfoStep : RegionInfoStep
    {
        private AccountUser _accountUser;
        private bool _lightWeightOnly;
        private Dictionary<int, PropertyDescriptor> _errorMappings;

        public EditContactInfoStep(Wizard owner, AccountManagementWizardState state)
          : base(owner, state, false)
        {
            this.LoadStates = true;
            this.LoadLanguages = true;
            this._lightWeightOnly = false;
            this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_CONTACT_HEAD);
            this.DetailDescription = Shell.LoadString(StringId.IDS_ACCOUNT_EDIT_CONTACT_INFO_DETAIL);
            WizardPropertyEditor wizardPropertyEditor = new ContactInfoPropertyEditor();
            this.RequireSignIn = true;
            this.Initialize(wizardPropertyEditor);
        }

        public override string UI => "res://ZuneShellResources!AccountInfo.uix#ContactInfoStep";

        public override bool IsEnabled
        {
            get
            {
                bool flag = base.IsEnabled;
                if (flag)
                    flag = this.LightWeightOnly && SignIn.Instance.IsLightWeight || !this.LightWeightOnly;
                return flag;
            }
        }

        public bool LightWeightOnly
        {
            get => this._lightWeightOnly;
            set
            {
                if (this._lightWeightOnly == value)
                    return;
                this._lightWeightOnly = value;
                this.FirePropertyChanged(nameof(LightWeightOnly));
                this.FirePropertyChanged("Enabled");
            }
        }

        internal override Dictionary<int, PropertyDescriptor> ErrorPropertyMappings
        {
            get
            {
                if (this._errorMappings == null)
                {
                    this._errorMappings = new Dictionary<int, PropertyDescriptor>(2);
                    this._errorMappings.Add(HRESULT._ZEST_E_INVALID_POSTALCODE.Int, ContactInfoPropertyEditor.PostalCode);
                    this._errorMappings.Add(HRESULT._ZEST_E_LIVEACCOUNT_INVALIDPHONE.Int, BaseContactInfoPropertyEditor.PhoneNumber);
                    this._errorMappings.Add(HRESULT._ZUNE_E_UPDATE_ACCOUNT_INFO_FAILED.Int, null);
                    this._errorMappings.Add(HRESULT._ZEST_E_LIVEACCOUNT_ADDRESS_INVALID.Int, null);
                    this._errorMappings.Add(HRESULT._ZEST_E_INVALID_ARG_CONTACT_INFO.Int, null);
                }
                return this._errorMappings;
            }
        }

        protected override PropertyDescriptor CountryDescriptor => ContactInfoPropertyEditor.Country;

        protected override PropertyDescriptor LanguageDescriptor => ContactInfoPropertyEditor.Language;

        protected override PropertyDescriptor StateDescriptor => ContactInfoPropertyEditor.State;

        public AccountUser AccountUser
        {
            get => this._accountUser;
            private set
            {
                if (this._accountUser == value)
                    return;
                this._accountUser = value;
                this.SetCommittedValue(ContactInfoPropertyEditor.City, AccountUser.Address.City);
                this.SetCommittedValue(ContactInfoPropertyEditor.District, AccountUser.Address.District);
                this.SetCommittedValue(ContactInfoPropertyEditor.PostalCode, AccountUser.Address.PostalCode);
                this.SetCommittedValue(ContactInfoPropertyEditor.Street1, AccountUser.Address.Street1);
                this.SetCommittedValue(ContactInfoPropertyEditor.Street2, AccountUser.Address.Street2);
                this.SetCommittedValue(BaseContactInfoPropertyEditor.FirstName, AccountUser.FirstName);
                this.SetCommittedValue(BaseContactInfoPropertyEditor.LastName, AccountUser.LastName);
                this.SetCommittedValue(BaseContactInfoPropertyEditor.Email, AccountUser.Email);
                this.SetCommittedValue(BaseContactInfoPropertyEditor.PhoneNumber, AccountUser.PhoneNumber);
                this.SelectedLocale = this.AccountUser.Locale;
                this.SelectedState = this.AccountUser.Address.State;
                this.FirePropertyChanged(nameof(AccountUser));
            }
        }

        protected override void OnCountryChanged()
        {
            if (this.WizardPropertyEditor == null)
                return;
            this.WizardPropertyEditor.SetPropertyState(BaseContactInfoPropertyEditor.FirstName, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(BaseContactInfoPropertyEditor.LastName, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(ContactInfoPropertyEditor.Street1, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(ContactInfoPropertyEditor.Street2, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(ContactInfoPropertyEditor.City, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(ContactInfoPropertyEditor.District, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(ContactInfoPropertyEditor.State, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(ContactInfoPropertyEditor.PostalCode, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(BaseContactInfoPropertyEditor.PhoneNumber, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(BaseContactInfoPropertyEditor.PhoneExtension, SelectedCountry);
        }

        protected override bool OnCommitChanges()
        {
            bool flag = true;
            if (this.AccountUser != null)
            {
                ServiceError serviceError = null;
                this.AccountUser.Address.City = this.GetCommittedValue(ContactInfoPropertyEditor.City) as string;
                this.AccountUser.Address.District = this.GetCommittedValue(ContactInfoPropertyEditor.District) as string;
                this.AccountUser.Address.PostalCode = this.GetCommittedValue(ContactInfoPropertyEditor.PostalCode) as string;
                this.AccountUser.Address.Street1 = this.GetCommittedValue(ContactInfoPropertyEditor.Street1) as string;
                this.AccountUser.Address.Street2 = this.GetCommittedValue(ContactInfoPropertyEditor.Street2) as string;
                this.AccountUser.FirstName = this.GetCommittedValue(BaseContactInfoPropertyEditor.FirstName) as string;
                this.AccountUser.LastName = this.GetCommittedValue(BaseContactInfoPropertyEditor.LastName) as string;
                this.AccountUser.Email = this.GetCommittedValue(BaseContactInfoPropertyEditor.Email) as string;
                this.AccountUser.PhoneNumber = this.GetCommittedValue(BaseContactInfoPropertyEditor.PhoneNumber) as string;
                this.AccountUser.Address.State = this.SelectedState;
                this.AccountUser.AccountSettings = null;
                this.AccountUser.ParentPassportIdentity = this.State.PassportPasswordParentStep.PassportIdentity;
                HRESULT hr = this.State.AccountManagement.SetAccount(null, this.AccountUser, out serviceError);
                if (hr.IsSuccess)
                {
                    SignIn.Instance.RefreshAccount();
                }
                else
                {
                    flag = false;
                    this.SetError(hr, serviceError);
                }
            }
            return flag;
        }

        protected override void OnActivate()
        {
            this.ServiceActivationRequestsDone = this.AccountUser != null;
            base.OnActivate();
        }

        protected override void OnStartActivationRequests(object state)
        {
            AccountUser accountUser = this.ObtainAccountUser();
            RegionServiceData regionServiceData = (RegionServiceData)state;
            if (accountUser != null && accountUser.Locale != null && regionServiceData.SelectedCountry == null)
            {
                string[] strArray = accountUser.Locale.Split('-');
                if (strArray.Length >= 2)
                    regionServiceData.SelectedCountry = strArray[1];
            }
            base.OnStartActivationRequests(state);
            this.EndActivationRequests(accountUser);
        }

        protected override void OnEndActivationRequests(object args)
        {
            if (args == null)
                this.NavigateToErrorHandler();
            else if (args is AccountUser)
                this.AccountUser = (AccountUser)args;
            else
                base.OnEndActivationRequests(args);
        }

        private AccountUser ObtainAccountUser()
        {
            AccountUser accountUser = null;
            ServiceError serviceError = null;
            HRESULT account = this.State.AccountManagement.GetAccount(null, out accountUser, out serviceError);
            if (account.IsError)
            {
                accountUser = null;
                this.SetError(account, serviceError);
            }
            return accountUser;
        }
    }
}
