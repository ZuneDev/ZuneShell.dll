// Decompiled with JetBrains decompiler
// Type: ZuneUI.PaymentInstrumentStepBase
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System;
using System.Collections.Generic;

namespace ZuneUI
{
    public class PaymentInstrumentStepBase : RegionInfoStep
    {
        private CreditCard _committedCreditCard;
        private Dictionary<int, PropertyDescriptor> _errorMappings;

        public PaymentInstrumentStepBase(
          Wizard owner,
          AccountManagementWizardState state,
          bool parentAccount)
          : base(owner, state, parentAccount)
        {
            this.LoadLanguages = false;
            this.LoadStates = true;
            if (parentAccount)
                this.Description = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_PARENT_AGE_HEADER);
            else
                this.Description = Shell.LoadString(StringId.IDS_BILLING_EDIT_CC_ADD_HEADER);
            this.Initialize(new PaymentInstrumentPropertyEditor());
        }

        public override string UI => "res://ZuneShellResources!AccountInfo.uix#PaymentInstrumentStep";

        protected override PropertyDescriptor CountryDescriptor => PaymentInstrumentPropertyEditor.Country;

        protected override PropertyDescriptor LanguageDescriptor => PaymentInstrumentPropertyEditor.Language;

        protected override PropertyDescriptor StateDescriptor => PaymentInstrumentPropertyEditor.State;

        public CreditCard CommittedCreditCard
        {
            get => this._committedCreditCard;
            protected set
            {
                if (this._committedCreditCard == value)
                    return;
                this._committedCreditCard = value;
                this.FirePropertyChanged(nameof(CommittedCreditCard));
            }
        }

        internal override Dictionary<int, PropertyDescriptor> ErrorPropertyMappings
        {
            get
            {
                if (this._errorMappings == null)
                {
                    this._errorMappings = new Dictionary<int, PropertyDescriptor>(11);
                    this._errorMappings.Add(HRESULT._ZEST_E_CREDITCARD_ADDRESS_CITY_INVALID.Int, PaymentInstrumentPropertyEditor.City);
                    this._errorMappings.Add(HRESULT._ZEST_E_CREDITCARD_ADDRESS_POSTALCODE_INVALID.Int, PaymentInstrumentPropertyEditor.PostalCode);
                    this._errorMappings.Add(HRESULT._ZEST_E_CREDITCARD_ADDRESS_STATE_INVALID.Int, PaymentInstrumentPropertyEditor.State);
                    this._errorMappings.Add(HRESULT._ZEST_E_CREDITCARD_ADDRESS_STREET1_INVALID.Int, PaymentInstrumentPropertyEditor.Street1);
                    this._errorMappings.Add(HRESULT._ZEST_E_CREDITCARD_PARENTPHONE_INVALID.Int, PaymentInstrumentPropertyEditor.PhoneNumber);
                    this._errorMappings.Add(HRESULT._ZEST_E_INVALID_ARG_PARENT_PHONE_INVALID.Int, PaymentInstrumentPropertyEditor.PhoneNumber);
                    this._errorMappings.Add(HRESULT._ZEST_E_CREDITCARD_ADD_FAILED.Int, null);
                    this._errorMappings.Add(HRESULT._ZEST_E_CREDITCARD_VALIDATE_FAILED.Int, null);
                    this._errorMappings.Add(HRESULT._ZEST_E_CREDITCARD_INVALID.Int, null);
                    this._errorMappings.Add(HRESULT._ZEST_E_CREDITCARD_ADDRESS_INVALID.Int, null);
                    this._errorMappings.Add(HRESULT._ZEST_E_LIVEACCOUNT_PAYMENT_INSTRUMENT_INVALID.Int, null);
                    this._errorMappings.Add(HRESULT._ZEST_E_LIVEACCOUNT_ADDRESS_INVALID.Int, null);
                }
                return this._errorMappings;
            }
        }

        protected override void OnCountryChanged()
        {
            if (this.WizardPropertyEditor == null)
                return;
            this.WizardPropertyEditor.SetPropertyState(PaymentInstrumentPropertyEditor.AccountHolderName, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(PaymentInstrumentPropertyEditor.Street1, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(PaymentInstrumentPropertyEditor.Street2, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(PaymentInstrumentPropertyEditor.City, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(PaymentInstrumentPropertyEditor.State, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(PaymentInstrumentPropertyEditor.PostalCode, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(PaymentInstrumentPropertyEditor.PhoneNumber, SelectedCountry);
            this.WizardPropertyEditor.SetPropertyState(PaymentInstrumentPropertyEditor.PhoneExtension, SelectedCountry);
        }

        protected CreditCard CreateCreditCard()
        {
            CreditCard creditCard = new CreditCard();
            creditCard.ParentCreditCard = this.ParentAccount;
            creditCard.AccountNumber = this.GetUncommittedValue(PaymentInstrumentPropertyEditor.AccountNumber) as string;
            creditCard.Address.City = this.GetUncommittedValue(PaymentInstrumentPropertyEditor.City) as string;
            creditCard.Address.PostalCode = this.GetUncommittedValue(PaymentInstrumentPropertyEditor.PostalCode) as string;
            creditCard.Address.State = this.GetUncommittedValue(PaymentInstrumentPropertyEditor.State) as string;
            creditCard.Address.Street1 = this.GetUncommittedValue(PaymentInstrumentPropertyEditor.Street1) as string;
            creditCard.Address.Street2 = this.GetUncommittedValue(PaymentInstrumentPropertyEditor.Street2) as string;
            creditCard.CCVNumber = this.GetUncommittedValue(PaymentInstrumentPropertyEditor.CcvNumber) as string;
            CreditCardType? uncommittedValue1 = (CreditCardType?)this.GetUncommittedValue(PaymentInstrumentPropertyEditor.CardType);
            if (uncommittedValue1.HasValue && uncommittedValue1.HasValue)
                creditCard.CreditCardType = uncommittedValue1.Value;
            creditCard.Email = this.GetUncommittedValue(PaymentInstrumentPropertyEditor.Email) as string;
            DateTime? uncommittedValue2 = (DateTime?)this.GetUncommittedValue(PaymentInstrumentPropertyEditor.ExpirationDate);
            if (uncommittedValue2.HasValue && uncommittedValue2.HasValue)
                creditCard.ExpirationDate = uncommittedValue2.Value;
            creditCard.AccountHolderName = this.GetUncommittedValue(PaymentInstrumentPropertyEditor.AccountHolderName) as string;
            creditCard.Locale = this.SelectedLocale;
            creditCard.PhoneNumber = this.GetUncommittedValue(PaymentInstrumentPropertyEditor.PhoneNumber) as string;
            creditCard.PhoneExtension = this.GetUncommittedValue(PaymentInstrumentPropertyEditor.PhoneExtension) as string;
            return creditCard;
        }
    }
}
