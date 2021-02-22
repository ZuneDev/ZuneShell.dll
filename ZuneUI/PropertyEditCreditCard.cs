// Decompiled with JetBrains decompiler
// Type: ZuneUI.PropertyEditCreditCard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System.Collections;

namespace ZuneUI
{
    public class PropertyEditCreditCard : MetadataEditMedia
    {
        private CreditCard _creditCard;
        private static PropertyDescriptor[] s_dataProviderProperties;
        public static CountryFieldValidationPropertyDescriptor s_Street1 = new CountryFieldValidationPropertyDescriptor(nameof(Street1), CountryFieldValidatorType.Street1);
        public static CountryFieldValidationPropertyDescriptor s_Street2 = new CountryFieldValidationPropertyDescriptor(nameof(Street2), CountryFieldValidatorType.Street2);
        public static CountryFieldValidationPropertyDescriptor s_City = new CountryFieldValidationPropertyDescriptor(nameof(City), CountryFieldValidatorType.City);
        public static CountryFieldValidationPropertyDescriptor s_District = new CountryFieldValidationPropertyDescriptor(nameof(District), CountryFieldValidatorType.District);
        public static StateDescriptor s_State = new StateDescriptor(nameof(State));
        public static PostalCodeDescriptor s_PostalCode = new PostalCodeDescriptor(nameof(PostalCode));
        public static PhoneNumberDescriptors s_PhoneNumber = new PhoneNumberDescriptors(nameof(PhoneNumber), CountryFieldValidatorType.PhoneNumber);
        public static CreditCardNumberDescriptor s_PhoneExtension = new CreditCardNumberDescriptor(nameof(PhoneExtension), string.Empty, string.Empty, false, false);
        public static CardTypePropertyDescriptor s_CardType = new CardTypePropertyDescriptor(nameof(CardType), string.Empty, Shell.LoadString(StringId.IDS_BILLING_EDIT_CC_CCTYPE_EMPTY), true);
        public static CountryFieldValidationPropertyDescriptor s_AccountHolderName = new CountryFieldValidationPropertyDescriptor(nameof(AccountHolderName), CountryFieldValidatorType.AccountHolderName);
        public static CreditCardNumberDescriptor s_AccountNumber = new CreditCardNumberDescriptor(nameof(AccountNumber), string.Empty, string.Empty, true, true);
        public static CreditCardNumberDescriptor s_CcvNumber = new CreditCardNumberDescriptor(nameof(CcvNumber), string.Empty, string.Empty, true, false);
        public static CreditCardExpirationDateDescriptor s_ExpirationDate = new CreditCardExpirationDateDescriptor(nameof(ExpirationDate), string.Empty, string.Empty, true);

        public PropertyEditCreditCard(CreditCard creditCard)
        {
            this._source = CreditCardPropertySource.Instance;
            this.Initialize(creditCard);
        }

        protected void Initialize(CreditCard creditCard)
        {
            this._creditCard = creditCard;
            if (PropertyEditCreditCard.s_dataProviderProperties == null)
                PropertyEditCreditCard.s_dataProviderProperties = new PropertyDescriptor[13]
                {
          (PropertyDescriptor) PropertyEditCreditCard.s_Street1,
          (PropertyDescriptor) PropertyEditCreditCard.s_Street2,
          (PropertyDescriptor) PropertyEditCreditCard.s_City,
          (PropertyDescriptor) PropertyEditCreditCard.s_District,
          (PropertyDescriptor) PropertyEditCreditCard.s_State,
          (PropertyDescriptor) PropertyEditCreditCard.s_PostalCode,
          (PropertyDescriptor) PropertyEditCreditCard.s_PhoneNumber,
          (PropertyDescriptor) PropertyEditCreditCard.s_PhoneExtension,
          (PropertyDescriptor) PropertyEditCreditCard.s_CardType,
          (PropertyDescriptor) PropertyEditCreditCard.s_AccountHolderName,
          (PropertyDescriptor) PropertyEditCreditCard.s_AccountNumber,
          (PropertyDescriptor) PropertyEditCreditCard.s_CcvNumber,
          (PropertyDescriptor) PropertyEditCreditCard.s_ExpirationDate
                };
            this.Initialize((IList)new object[1]
            {
        (object) creditCard
            }, PropertyEditCreditCard.s_dataProviderProperties);
        }

        public CreditCard CreditCard => this._creditCard;

        public static PropertyDescriptor Street1 => (PropertyDescriptor)PropertyEditCreditCard.s_Street1;

        public static PropertyDescriptor Street2 => (PropertyDescriptor)PropertyEditCreditCard.s_Street2;

        public static PropertyDescriptor City => (PropertyDescriptor)PropertyEditCreditCard.s_City;

        public static PropertyDescriptor District => (PropertyDescriptor)PropertyEditCreditCard.s_District;

        public static PropertyDescriptor State => (PropertyDescriptor)PropertyEditCreditCard.s_State;

        public static PropertyDescriptor PostalCode => (PropertyDescriptor)PropertyEditCreditCard.s_PostalCode;

        public static PropertyDescriptor PhoneNumber => (PropertyDescriptor)PropertyEditCreditCard.s_PhoneNumber;

        public static PropertyDescriptor PhoneExtension => (PropertyDescriptor)PropertyEditCreditCard.s_PhoneExtension;

        public static PropertyDescriptor CardType => (PropertyDescriptor)PropertyEditCreditCard.s_CardType;

        public static PropertyDescriptor AccountHolderName => (PropertyDescriptor)PropertyEditCreditCard.s_AccountHolderName;

        public static PropertyDescriptor AccountNumber => (PropertyDescriptor)PropertyEditCreditCard.s_AccountNumber;

        public static PropertyDescriptor CcvNumber => (PropertyDescriptor)PropertyEditCreditCard.s_CcvNumber;

        public static PropertyDescriptor ExpirationDate => (PropertyDescriptor)PropertyEditCreditCard.s_ExpirationDate;
    }
}
