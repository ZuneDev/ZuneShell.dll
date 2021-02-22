// Decompiled with JetBrains decompiler
// Type: ZuneUI.CreatePassportStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.ErrorMapperApi;
using Microsoft.Zune.Service;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class CreatePassportStep : AccountManagementStep
    {
        private IList _suggestedPassportIds;
        private WinLiveInformation _winLiveInformation;
        private string _lastLocale;
        private bool _createdPassport;
        private Dictionary<int, PropertyDescriptor> _errorMappings;
        private static IList s_secretQuestions;
        private string _secretAnswerString;

        public CreatePassportStep(Wizard owner, AccountManagementWizardState state, bool parentAccount)
          : base(owner, state, parentAccount)
        {
            if (parentAccount)
                this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_START_PARENT_HEADER);
            else
                this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_PASSPORT_STEP);
            this._secretAnswerString = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_SECRET_ANSWER_HELP);
            this.NextTextOverride = Shell.LoadString(StringId.IDS_I_ACCEPT_BUTTON);
            this.Initialize((WizardPropertyEditor)new CreatePassportPropertyEditor());
        }

        public override string UI => "res://ZuneShellResources!CreatePassport.uix#CreatePassportStep";

        public override bool IsEnabled
        {
            get
            {
                bool flag = base.IsEnabled && !this.CreatedPassport;
                if (flag)
                    flag = this.EmailSelectionStep.IsEnabled && (!this.EmailSelectionStep.HasEmail || !this.EmailSelectionStep.IsEmailPassportId) && (this._owner.CurrentPage != this || this.WinLiveInformation != null);
                return flag;
            }
        }

        public override bool IsValid
        {
            get
            {
                MetadataEditProperty property1 = this.WizardPropertyEditor.GetProperty(CreatePassportPropertyEditor.PassportId);
                MetadataEditProperty property2 = this.WizardPropertyEditor.GetProperty(CreatePassportPropertyEditor.PassportDomain);
                string email = this.Email;
                HRESULT hresult = HRESULT._S_OK;
                if (this.ParentAccount && (email.Equals(this.State.EmailSelectionStep.Email, StringComparison.InvariantCultureIgnoreCase) || email.Equals(this.State.CreatePassportStep.Email, StringComparison.InvariantCultureIgnoreCase)))
                    hresult = HRESULT._ZUNE_E_SIGNUP_INVALID_PARENT_EMAIL;
                if (hresult.IsError || property1.ExternalError == HRESULT._ZUNE_E_SIGNUP_INVALID_PARENT_EMAIL)
                    property1.ExternalError = hresult;
                if (hresult.IsError || property2.ExternalError == HRESULT._ZUNE_E_SIGNUP_INVALID_PARENT_EMAIL)
                    property2.ExternalError = hresult;
                return base.IsValid;
            }
        }

        public WinLiveInformation WinLiveInformation
        {
            get => this._winLiveInformation;
            private set
            {
                if (this._winLiveInformation == value)
                    return;
                this._winLiveInformation = value;
                this.FirePropertyChanged(nameof(WinLiveInformation));
                this.FirePropertyChanged("IsEnabled");
                if (!this.CreateEmail)
                    return;
                this.SetDefaultWindowsLiveDomain();
            }
        }

        public WinLiveInformation WinLiveHip => !this.ParentAccount ? this.State.HipPassportStep.WinLiveHip : this.State.HipPassportParentStep.WinLiveHip;

        public bool CreateEmail => this.EmailSelectionStep.IsEnabled && !this.EmailSelectionStep.HasEmail;

        public IList SuggestedPassportIds
        {
            get => this._suggestedPassportIds;
            private set
            {
                if (this._suggestedPassportIds == value)
                    return;
                this._suggestedPassportIds = value;
                this.FirePropertyChanged(nameof(SuggestedPassportIds));
            }
        }

        public string CommittedEmail
        {
            get
            {
                string committedValue1 = this.GetCommittedValue(CreatePassportPropertyEditor.PassportId) as string;
                string committedValue2 = this.GetCommittedValue(CreatePassportPropertyEditor.PassportDomain) as string;
                string str = (string)null;
                if (committedValue1 != null && committedValue2 != null)
                    str = string.Format("{0}@{1}", (object)committedValue1, (object)committedValue2);
                return str;
            }
        }

        public string Email
        {
            get
            {
                string uncommittedValue1 = this.GetUncommittedValue(CreatePassportPropertyEditor.PassportId) as string;
                string uncommittedValue2 = this.GetUncommittedValue(CreatePassportPropertyEditor.PassportDomain) as string;
                string str = (string)null;
                if (uncommittedValue1 != null && uncommittedValue2 != null)
                    str = string.Format("{0}@{1}", (object)uncommittedValue1, (object)uncommittedValue2);
                return str;
            }
            set
            {
                if (!(this.Email != value))
                    return;
                string str1 = string.Empty;
                string str2 = string.Empty;
                int length = 0;
                if (value != null)
                    length = value.IndexOf('@');
                if (length > 0 && length + 2 <= value.Length)
                {
                    str1 = value.Substring(0, length);
                    str2 = value.Substring(length + 1);
                }
                this.SetCommittedValue(CreatePassportPropertyEditor.PassportId, (object)str1);
                this.SetCommittedValue(CreatePassportPropertyEditor.PassportDomain, (object)str2);
                this.FirePropertyChanged(nameof(Email));
            }
        }

        internal bool CreatedPassport
        {
            get => this._createdPassport;
            private set => this._createdPassport = value;
        }

        internal override Dictionary<int, PropertyDescriptor> ErrorPropertyMappings
        {
            get
            {
                if (this._errorMappings == null)
                {
                    this._errorMappings = new Dictionary<int, PropertyDescriptor>(18);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_SIGNIN_NAME_TOO_SHORT.Int, CreatePassportPropertyEditor.PassportId);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_SIGNIN_NAME_TOO_LONG.Int, CreatePassportPropertyEditor.PassportId);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_SIGNIN_NAME_INVALID.Int, CreatePassportPropertyEditor.PassportId);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_EMAIL_INVALID.Int, CreatePassportPropertyEditor.PassportId);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_NAME_INVALID.Int, CreatePassportPropertyEditor.PassportId);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_MEMBER_EXISTS.Int, CreatePassportPropertyEditor.PassportId);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_DOMAIN_IS_MANAGED.Int, CreatePassportPropertyEditor.PassportDomain);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_PASSWORD_TOO_LONG.Int, CreatePassportPropertyEditor.Password1);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_PASSWORD_TOO_SHORT.Int, CreatePassportPropertyEditor.Password1);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_PASSWORD_INVALID.Int, CreatePassportPropertyEditor.Password1);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_SECRET_QUESTION_TOO_SHORT.Int, CreatePassportPropertyEditor.SecretQuestion);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_SECRET_QUESTION_TOO_LONG.Int, CreatePassportPropertyEditor.SecretQuestion);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_SECRET_QUESTION_CONTAINS_ANSWER.Int, CreatePassportPropertyEditor.SecretQuestion);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_SECRET_QUESTION_CONTAINS_PASSWORD.Int, CreatePassportPropertyEditor.SecretQuestion);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_SECRET_ANSWER_TOO_SHORT.Int, (PropertyDescriptor)CreatePassportPropertyEditor.SecretAnswer);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_SECRET_ANSWER_TOO_LONG.Int, (PropertyDescriptor)CreatePassportPropertyEditor.SecretAnswer);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_SECRET_ANSWER_CONTAINS_MEMBER_NAME.Int, (PropertyDescriptor)CreatePassportPropertyEditor.SecretAnswer);
                    this._errorMappings.Add(HRESULT._NS_E_WINLIVE_SECRET_ANSWER_CONTAINS_PASSWORD.Int, (PropertyDescriptor)CreatePassportPropertyEditor.SecretAnswer);
                }
                return this._errorMappings;
            }
        }

        public static IList SecretQuestions
        {
            get
            {
                if (CreatePassportStep.s_secretQuestions == null)
                {
                    string str = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_SECRET_QUESTIONS);
                    if (str != null)
                        CreatePassportStep.s_secretQuestions = (IList)str.Split(';');
                }
                return CreatePassportStep.s_secretQuestions;
            }
        }

        private EmailSelectionStep EmailSelectionStep => !this.ParentAccount ? this.State.EmailSelectionStep : this.State.EmailSelectionParentStep;

        protected override void OnActivate()
        {
            this.ServiceDeactivationRequestsDone = false;
            if (this.EmailSelectionStep.HasEmail && !this.CreateEmail)
                this.Email = this.EmailSelectionStep.GetCommittedValue(EmailSelectionPropertyEditor.Email) as string;
            string selectedLocale = this.State.BasicAccountInfoStep.SelectedLocale;
            if (this._lastLocale != selectedLocale)
                this.ServiceActivationRequestsDone = false;
            if (!this.ServiceActivationRequestsDone)
            {
                this._lastLocale = selectedLocale;
                this.StartActivationRequests((object)selectedLocale);
            }
            else if (this.CreateEmail)
                this.SetDefaultWindowsLiveDomain();
            this.SecretAnswerString = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_SECRET_ANSWER_HELP);
            CreatePassportPropertyEditor.SecretAnswer.MinLength = 5;
        }

        public string SecretAnswerString
        {
            get => this._secretAnswerString;
            private set
            {
                if (!(this._secretAnswerString != value))
                    return;
                this._secretAnswerString = value;
                this.FirePropertyChanged(nameof(SecretAnswerString));
            }
        }

        internal override void Deactivate()
        {
            base.Deactivate();
            this.SuggestedPassportIds = (IList)null;
        }

        internal override bool OnMovingNext()
        {
            string email = this.Email;
            string committedEmail = this.CommittedEmail;
            if (email == committedEmail)
                this.ServiceDeactivationRequestsDone = true;
            if (this.ServiceDeactivationRequestsDone)
            {
                if (this.SuggestedPassportIds == null || this.SuggestedPassportIds.Count == 0 || this.SuggestedPassportIds.Contains((object)email))
                {
                    this.SuggestedPassportIds = (IList)null;
                    return base.OnMovingNext();
                }
                this.ServiceDeactivationRequestsDone = false;
                return false;
            }
            this.StartDeactivationRequests((object)email);
            return false;
        }

        internal override ErrorMapperResult GetMappedErrorDescriptionAndUrl(HRESULT hr) => Microsoft.Zune.ErrorMapperApi.ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hr.Int, eErrorCondition.eEC_WinLive);

        protected override bool OnCommitChanges()
        {
            bool flag = true;
            if (this.IsEnabled && !this.CreatedPassport)
            {
                string email = this.Email;
                string committedValue1 = this.GetCommittedValue(CreatePassportPropertyEditor.Password1) as string;
                string committedValue2 = this.GetCommittedValue(CreatePassportPropertyEditor.SecretQuestion) as string;
                string committedValue3 = this.GetCommittedValue((PropertyDescriptor)CreatePassportPropertyEditor.SecretAnswer) as string;
                string selectedCountry = this.State.BasicAccountInfoStep.SelectedCountry;
                int languagePreference = 0;
                string selectedLocale = this.State.BasicAccountInfoStep.SelectedLocale;
                if (!string.IsNullOrEmpty(selectedLocale))
                    languagePreference = CultureHelper.GetLCIDFromCultureString(selectedLocale, true);
                int termsOfServiceVersion = 0;
                if (this.WinLiveInformation != null)
                    termsOfServiceVersion = this.WinLiveInformation.TermsOfServiceVersion;
                string hipChallenge = string.Empty;
                if (this.WinLiveHip != null)
                    hipChallenge = this.WinLiveHip.HipChallenge;
                string committedValue4;
                DateTime? committedValue5;
                if (this.ParentAccount)
                {
                    committedValue4 = this.State.HipPassportParentStep.GetCommittedValue(HipPropertyEditor.HipCharacters) as string;
                    committedValue5 = (DateTime?)this.State.ContactInfoParentStep.GetCommittedValue(ContactInfoParentPropertyEditor.Birthday);
                }
                else
                {
                    committedValue4 = this.State.HipPassportStep.GetCommittedValue(HipPropertyEditor.HipCharacters) as string;
                    committedValue5 = (DateTime?)this.State.BasicAccountInfoStep.GetCommittedValue(BasicAccountInfoPropertyEditor.Birthday);
                }
                ServiceError serviceError = (ServiceError)null;
                HRESULT account = this.State.WinLiveSignup.CreateAccount(email, committedValue1, committedValue2, committedValue3, selectedCountry, hipChallenge, committedValue4, committedValue5.Value, termsOfServiceVersion, languagePreference, out serviceError);
                flag = account.IsSuccess;
                if (flag)
                    this.CreatedPassport = true;
                else
                    this.SetError(account, serviceError);
            }
            return flag;
        }

        protected override void OnStartActivationRequests(object state) => this.EndActivationRequests((object)this.ObtainWinLiveInformation(state as string));

        protected override void OnEndActivationRequests(object args)
        {
            if (args == null)
                this.NavigateToErrorHandler();
            else
                this.WinLiveInformation = args as WinLiveInformation;
        }

        protected override void OnStartDeactivationRequests(object state) => this.EndDeactivationRequests((object)this.ObtainUniquePassportIds(state as string));

        protected override void OnEndDeactivationRequests(object args) => this.SuggestedPassportIds = args as IList;

        private void SetDefaultWindowsLiveDomain()
        {
            if (this.GetUncommittedValue(CreatePassportPropertyEditor.PassportDomain) is string uncommittedValue && (this._winLiveInformation.Domains == null || this._winLiveInformation.Domains.Count <= 0 || this._winLiveInformation.Domains.Contains((object)uncommittedValue)))
                return;
            this.SetUncommittedValue(CreatePassportPropertyEditor.PassportDomain, (object)(this._winLiveInformation.Domains[0] as string));
        }

        private WinLiveInformation ObtainWinLiveInformation(string locale)
        {
            WinLiveInformation information1 = (WinLiveInformation)null;
            ServiceError serviceError = (ServiceError)null;
            HRESULT information2 = this.State.WinLiveSignup.GetInformation(locale, Microsoft.Zune.Service.EHipType.Image, out information1, out serviceError);
            if (information2.IsError)
                this.SetError(information2, serviceError);
            return information1;
        }

        private IList ObtainUniquePassportIds(string email)
        {
            IList list = (IList)null;
            ServiceError serviceError = (ServiceError)null;
            WinLiveAvailableInformation information;
            HRESULT hr = this.State.WinLiveSignup.CheckAvailableSigninName(email, true, (string)null, (string)null, out information, out serviceError);
            if (hr.IsError)
                this.SetError(hr, serviceError);
            else if (!information.Available)
                list = information.SuggestedNames;
            return list;
        }
    }
}
