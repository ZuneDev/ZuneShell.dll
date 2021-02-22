// Decompiled with JetBrains decompiler
// Type: ZuneUI.AccountManagementWizardState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using Microsoft.Zune.Util;
using System;

namespace ZuneUI
{
    public class AccountManagementWizardState
    {
        private AccountManagementWizard _wizard;
        private BasicAccountInfoStep _basicAccountInfoStep;
        private EditContactInfoStep _contactInfoStep;
        private ContactInfoParentStep _contactInfoParentStep;
        private CreatePassportStep _createPassportStep;
        private CreatePassportStep _createPassportParentStep;
        private EmailSelectionStep _emailSelectionStep;
        private EmailSelectionStep _emailSelectionParentStep;
        private PassportPasswordStep _passportPasswordStep;
        private PassportPasswordStep _passportPasswordParentStep;
        private PaymentInstrumentStep _paymentInstrumentStep;
        private ParentPaymentIntrumentStep _paymentInstrumentParentStep;
        private HipPassportStep _hipPassportStep;
        private HipPassportStep _hipPassportParentStep;
        private PrivacyInfoStep _privacyInfoStep;
        private PrivacyInfoStep _privacyInfoParentStep;
        private TermsOfServiceStep _termsOfServiceStep;
        private ZuneTagStep _zuneTagStep;
        private SelectBillingOfferStep _selectBillingOfferStep;
        private SelectPaymentInstrumentStep _selectPaymentInstrumentStep;
        private ConfirmationStep _confirmationStep;
        private ListAndAddPaymentInstrumentStep _listAndAddPaymentInstrumentStep;
        private RedeemCodeStep _redeemCodeStep;
        private WinLiveSignup _winLiveSignup;
        private AccountManagement _accountManagment;

        public AccountManagementWizardState(AccountManagementWizard wizard) => this._wizard = wizard;

        public BasicAccountInfoStep BasicAccountInfoStep
        {
            get
            {
                if (this._basicAccountInfoStep == null)
                    this._basicAccountInfoStep = new BasicAccountInfoStep((Wizard)this._wizard, this, false);
                return this._basicAccountInfoStep;
            }
        }

        public EditContactInfoStep ContactInfoStep
        {
            get
            {
                if (this._contactInfoStep == null)
                    this._contactInfoStep = new EditContactInfoStep((Wizard)this._wizard, this);
                return this._contactInfoStep;
            }
        }

        public ContactInfoParentStep ContactInfoParentStep
        {
            get
            {
                if (this._contactInfoParentStep == null)
                    this._contactInfoParentStep = new ContactInfoParentStep((Wizard)this._wizard, this);
                return this._contactInfoParentStep;
            }
        }

        public CreatePassportStep CreatePassportStep
        {
            get
            {
                if (this._createPassportStep == null)
                    this._createPassportStep = new CreatePassportStep((Wizard)this._wizard, this, false);
                return this._createPassportStep;
            }
        }

        public CreatePassportStep CreatePassportParentStep
        {
            get
            {
                if (this._createPassportParentStep == null)
                    this._createPassportParentStep = new CreatePassportStep((Wizard)this._wizard, this, true);
                return this._createPassportParentStep;
            }
        }

        public EmailSelectionStep EmailSelectionStep
        {
            get
            {
                if (this._emailSelectionStep == null)
                    this._emailSelectionStep = new EmailSelectionStep((Wizard)this._wizard, this, false);
                return this._emailSelectionStep;
            }
        }

        public EmailSelectionStep EmailSelectionParentStep
        {
            get
            {
                if (this._emailSelectionParentStep == null)
                    this._emailSelectionParentStep = new EmailSelectionStep((Wizard)this._wizard, this, true);
                return this._emailSelectionParentStep;
            }
        }

        public HipPassportStep HipPassportStep
        {
            get
            {
                if (this._hipPassportStep == null)
                    this._hipPassportStep = new HipPassportStep((Wizard)this._wizard, this, false);
                return this._hipPassportStep;
            }
        }

        public HipPassportStep HipPassportParentStep
        {
            get
            {
                if (this._hipPassportParentStep == null)
                    this._hipPassportParentStep = new HipPassportStep((Wizard)this._wizard, this, true);
                return this._hipPassportParentStep;
            }
        }

        public PassportPasswordStep PassportPasswordStep
        {
            get
            {
                if (this._passportPasswordStep == null)
                    this._passportPasswordStep = new PassportPasswordStep((Wizard)this._wizard, this, false);
                return this._passportPasswordStep;
            }
        }

        public PassportPasswordStep PassportPasswordParentStep
        {
            get
            {
                if (this._passportPasswordParentStep == null)
                    this._passportPasswordParentStep = new PassportPasswordStep((Wizard)this._wizard, this, true);
                return this._passportPasswordParentStep;
            }
        }

        public PaymentInstrumentStep PaymentInstrumentStep
        {
            get
            {
                if (this._paymentInstrumentStep == null)
                    this._paymentInstrumentStep = new PaymentInstrumentStep((Wizard)this._wizard, this, false);
                return this._paymentInstrumentStep;
            }
        }

        public ParentPaymentIntrumentStep PaymentInstrumentParentStep
        {
            get
            {
                if (this._paymentInstrumentParentStep == null)
                    this._paymentInstrumentParentStep = new ParentPaymentIntrumentStep((Wizard)this._wizard, this);
                return this._paymentInstrumentParentStep;
            }
        }

        public PrivacyInfoStep PrivacyInfoStep
        {
            get
            {
                if (this._privacyInfoStep == null)
                    this._privacyInfoStep = new PrivacyInfoStep((Wizard)this._wizard, this, false, PrivacyInfoSettings.None);
                return this._privacyInfoStep;
            }
        }

        public PrivacyInfoStep PrivacyInfoParentStep
        {
            get
            {
                if (this._privacyInfoParentStep == null)
                    this._privacyInfoParentStep = new PrivacyInfoStep((Wizard)this._wizard, this, true, PrivacyInfoSettings.None);
                return this._privacyInfoParentStep;
            }
        }

        public TermsOfServiceStep TermsOfServiceStep
        {
            get
            {
                if (this._termsOfServiceStep == null)
                    this._termsOfServiceStep = new TermsOfServiceStep((Wizard)this._wizard, this, false);
                return this._termsOfServiceStep;
            }
        }

        public ZuneTagStep ZuneTagStep
        {
            get
            {
                if (this._zuneTagStep == null)
                    this._zuneTagStep = new ZuneTagStep((Wizard)this._wizard, this, false);
                return this._zuneTagStep;
            }
        }

        public SelectBillingOfferStep SelectBillingOfferStep
        {
            get
            {
                if (this._selectBillingOfferStep == null)
                    this._selectBillingOfferStep = new SelectBillingOfferStep((Wizard)this._wizard, this);
                return this._selectBillingOfferStep;
            }
        }

        public SelectPaymentInstrumentStep SelectPaymentInstrumentStep
        {
            get
            {
                if (this._selectPaymentInstrumentStep == null)
                    this._selectPaymentInstrumentStep = new SelectPaymentInstrumentStep((Wizard)this._wizard, this, false);
                return this._selectPaymentInstrumentStep;
            }
        }

        public ConfirmationStep ConfirmationStep
        {
            get
            {
                if (this._confirmationStep == null)
                    this._confirmationStep = new ConfirmationStep((Wizard)this._wizard, this, false);
                return this._confirmationStep;
            }
        }

        public ListAndAddPaymentInstrumentStep ListAndAddPaymentInstrumentStep
        {
            get
            {
                if (this._listAndAddPaymentInstrumentStep == null)
                    this._listAndAddPaymentInstrumentStep = new ListAndAddPaymentInstrumentStep((Wizard)this._wizard, this, false);
                return this._listAndAddPaymentInstrumentStep;
            }
        }

        public RedeemCodeStep RedeemCodeStep
        {
            get
            {
                if (this._redeemCodeStep == null)
                    this._redeemCodeStep = new RedeemCodeStep((Wizard)this._wizard, this);
                return this._redeemCodeStep;
            }
        }

        public WinLiveSignup WinLiveSignup
        {
            get
            {
                if (this._winLiveSignup == null)
                    this._winLiveSignup = new WinLiveSignup();
                return this._winLiveSignup;
            }
        }

        public AccountManagement AccountManagement
        {
            get
            {
                if (this._accountManagment == null)
                    this._accountManagment = new AccountManagement();
                return this._accountManagment;
            }
        }

        internal void SetPrivacySettings(AccountUserType type)
        {
            if (type == AccountUserType.Adult)
            {
                this.PrivacyInfoStep.ShowSettings = FeatureEnablement.IsFeatureEnabled(Features.eSocial) ? PrivacyInfoSettings.CreateNewAccountWithSocial : PrivacyInfoSettings.CreateNewAccount;
                this.PrivacyInfoParentStep.ShowSettings = PrivacyInfoSettings.None;
            }
            else
            {
                this.PrivacyInfoStep.ShowSettings = PrivacyInfoSettings.AllowMicrosoftCommunications;
                if (type == AccountUserType.ChildWithSocial)
                    this.PrivacyInfoParentStep.ShowSettings = FeatureEnablement.IsFeatureEnabled(Features.eSocial) ? PrivacyInfoSettings.CreateChildAccountWithSocial : PrivacyInfoSettings.CreateChildAccount;
                else
                    this.PrivacyInfoParentStep.ShowSettings = PrivacyInfoSettings.CreateChildAccount;
            }
        }

        internal bool AcceptTermsOfService() => this.UpgradeZuneAccount(true, this.TermsOfServiceStep.PassportIdentity);

        internal bool UpgradeZuneAccount(bool includeAccountSettings)
        {
            PassportIdentity passportIdentity = this.GetPassportIdentity();
            return this.UpgradeZuneAccount(includeAccountSettings, passportIdentity);
        }

        public void SaveFamilySettings()
        {
            if (!SignIn.Instance.SignedIn || !this.PrivacyInfoParentStep.IsEnabled || this.PrivacyInfoParentStep.FamilySettings == null)
                return;
            this.PrivacyInfoParentStep.FamilySettings.UserId = SignIn.Instance.LastSignedInUserId;
            this.PrivacyInfoParentStep.FamilySettings.CommitSettings();
            SignIn.Instance.FamilySettings.ReloadSettings();
        }

        internal bool UpgradeZuneAccount(bool includeAccountSettings, PassportIdentity passportIdentity)
        {
            PassportIdentity passportIdentity1 = this.GetParentPassportIdentity();
            AccountSettings accountSettings = (AccountSettings)null;
            if (includeAccountSettings)
            {
                if (this.PrivacyInfoParentStep.IsEnabled)
                {
                    accountSettings = this.PrivacyInfoParentStep.CommittedSettings;
                    accountSettings.AllowPartnerEmails = this.PrivacyInfoStep.CommittedSettings.AllowPartnerEmails;
                    accountSettings.AllowZuneEmails = this.PrivacyInfoStep.CommittedSettings.AllowZuneEmails;
                }
                else
                    accountSettings = this.PrivacyInfoStep.CommittedSettings;
            }
            ServiceError serviceError = (ServiceError)null;
            HRESULT hr = this.AccountManagement.UpgradeAccount(passportIdentity, accountSettings, passportIdentity1, out serviceError);
            bool isSuccess = hr.IsSuccess;
            if (!isSuccess)
                this._wizard.SetError(hr, (object)new AccountManagementErrorState(false, serviceError));
            return isSuccess;
        }

        internal string GetEmailAddress()
        {
            string empty = string.Empty;
            return this.CreatePassportStep.IsEnabled || this.CreatePassportStep.CreatedPassport ? this.CreatePassportStep.Email : this.PassportPasswordStep.CommittedEmail;
        }

        internal bool CreateZuneAccount()
        {
            PassportIdentity passportIdentity1 = this.GetPassportIdentity();
            string committedValue1 = this.ZuneTagStep.GetCommittedValue(ZuneTagPropertyEditor.ZuneTag) as string;
            string emailAddress = this.GetEmailAddress();
            string selectedLocale = this.BasicAccountInfoStep.SelectedLocale;
            DateTime? committedValue2 = (DateTime?)this.BasicAccountInfoStep.GetCommittedValue(BasicAccountInfoPropertyEditor.Birthday);
            PassportIdentity passportIdentity2 = this.GetParentPassportIdentity();
            CreditCard parentCreditCard = (CreditCard)null;
            AccountSettings committedSettings;
            if (this.BasicAccountInfoStep.IsParentAccountNeeded || this.PassportPasswordStep.IsParentAccountNeeded)
            {
                committedSettings = this.PrivacyInfoParentStep.CommittedSettings;
                committedSettings.AllowPartnerEmails = this.PrivacyInfoStep.CommittedSettings != null && this.PrivacyInfoStep.CommittedSettings.AllowPartnerEmails;
                committedSettings.AllowZuneEmails = this.PrivacyInfoStep.CommittedSettings != null && this.PrivacyInfoStep.CommittedSettings.AllowZuneEmails;
                parentCreditCard = this.PaymentInstrumentParentStep.CommittedCreditCard;
            }
            else
                committedSettings = this.PrivacyInfoStep.CommittedSettings;
            Address address = new Address();
            address.PostalCode = this.BasicAccountInfoStep.GetCommittedValue(BasicAccountInfoPropertyEditor.PostalCode) as string;
            ServiceError serviceError = (ServiceError)null;
            HRESULT account = this.AccountManagement.CreateAccount(passportIdentity1, committedValue1, selectedLocale, committedValue2.Value, string.Empty, string.Empty, emailAddress, address, committedSettings, passportIdentity2, parentCreditCard, out serviceError);
            bool isSuccess = account.IsSuccess;
            if (!isSuccess)
                this._wizard.SetError(account, (object)new AccountManagementErrorState(false, serviceError));
            return isSuccess;
        }

        internal bool PurchaseBillingOffer(
          BillingOffer billingOffer,
          PaymentInstrument paymentInstrument)
        {
            HRESULT hr = (HRESULT)Microsoft.Zune.Service.Service.Instance.PurchaseBillingOffer(billingOffer, paymentInstrument);
            if (hr.IsError)
                this._wizard.SetError(hr, (object)null);
            else if (billingOffer.OfferType == Microsoft.Zune.Service.EBillingOfferType.Subscription || billingOffer.OfferType == Microsoft.Zune.Service.EBillingOfferType.Trial || billingOffer.OfferType == Microsoft.Zune.Service.EBillingOfferType.Renewal)
                SignIn.Instance.RefreshAccount();
            return hr.IsSuccess;
        }

        internal bool RedeemCode() => this.PurchaseBillingOffer(this.RedeemCodeStep.MatchingBillingOffer, (PaymentInstrument)this.RedeemCodeStep.TokenDetails);

        private PassportIdentity GetPassportIdentity()
        {
            PassportIdentity passportIdentity = (PassportIdentity)null;
            if (this.PassportPasswordStep.IsEnabled)
                passportIdentity = this.PassportPasswordStep.PassportIdentity;
            else
                AccountManagementHelper.GetPassportIdentity(this.CreatePassportStep.Email, this.CreatePassportStep.GetCommittedValue(CreatePassportPropertyEditor.Password1) as string, out passportIdentity);
            return passportIdentity;
        }

        private PassportIdentity GetParentPassportIdentity()
        {
            PassportIdentity passportIdentity = (PassportIdentity)null;
            if (this.PassportPasswordParentStep.IsEnabled)
                passportIdentity = this.PassportPasswordParentStep.PassportIdentity;
            else if (this.CreatePassportParentStep.CreatedPassport)
                AccountManagementHelper.GetPassportIdentity(this.CreatePassportParentStep.Email, this.CreatePassportParentStep.GetCommittedValue(CreatePassportPropertyEditor.Password1) as string, out passportIdentity);
            return passportIdentity;
        }

        public void SignInNewUser()
        {
            string emailAddress = this.GetEmailAddress();
            string committedValue = this.PassportPasswordStep.GetCommittedValue(PassportPasswordPropertyEditor.Password) as string;
            if (string.IsNullOrEmpty(committedValue))
                committedValue = this.CreatePassportStep.GetCommittedValue(CreatePassportPropertyEditor.Password1) as string;
            SignIn.Instance.SignOut();
            SignIn.Instance.SignInUser(emailAddress, committedValue, true, false, false);
        }

        public bool IsPurchaseConfirmationNeeded => FeatureEnablement.IsFeatureEnabled(Features.eSubscriptionConfirmation);
    }
}
