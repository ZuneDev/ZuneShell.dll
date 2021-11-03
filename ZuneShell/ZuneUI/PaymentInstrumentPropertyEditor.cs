// Decompiled with JetBrains decompiler
// Type: ZuneUI.PaymentInstrumentPropertyEditor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class PaymentInstrumentPropertyEditor : WizardPropertyEditor
    {
        private static PropertyDescriptor[] s_dataProviderProperties;
        public static CountryFieldValidationPropertyDescriptor s_Street1 = new CountryFieldValidationPropertyDescriptor(nameof(Street1), CountryFieldValidatorType.Street1);
        public static CountryFieldValidationPropertyDescriptor s_Street2 = new CountryFieldValidationPropertyDescriptor(nameof(Street2), CountryFieldValidatorType.Street2);
        public static CountryFieldValidationPropertyDescriptor s_City = new CountryFieldValidationPropertyDescriptor(nameof(City), CountryFieldValidatorType.City);
        public static CountryFieldValidationPropertyDescriptor s_District = new CountryFieldValidationPropertyDescriptor(nameof(District), CountryFieldValidatorType.District);
        public static StateDescriptor s_State = new StateDescriptor(nameof(State));
        public static PostalCodeDescriptor s_PostalCode = new PostalCodeDescriptor(nameof(PostalCode));
        public static PhoneNumberDescriptors s_PhoneNumber = new PhoneNumberDescriptors(nameof(PhoneNumber), CountryFieldValidatorType.PhoneNumber);
        public static CountryFieldValidationPropertyDescriptor s_PhoneExtension = new CountryFieldValidationPropertyDescriptor(nameof(PhoneExtension), CountryFieldValidatorType.PhoneExtension);
        public static CardTypePropertyDescriptor s_CardType = new CardTypePropertyDescriptor(nameof(CardType), string.Empty, Shell.LoadString(StringId.IDS_BILLING_EDIT_CC_CCTYPE_EMPTY), true);
        public static CountryFieldValidationPropertyDescriptor s_AccountHolderName = new CountryFieldValidationPropertyDescriptor(nameof(AccountHolderName), CountryFieldValidatorType.AccountHolderName);
        public static CreditCardNumberDescriptor s_AccountNumber = new CreditCardNumberDescriptor(nameof(AccountNumber), string.Empty, string.Empty, true, true);
        public static CreditCardNumberDescriptor s_CcvNumber = new CreditCardNumberDescriptor(nameof(CcvNumber), string.Empty, string.Empty, true, false);
        public static CreditCardExpirationDateDescriptor s_ExpirationDate = new CreditCardExpirationDateDescriptor(nameof(ExpirationDate), string.Empty, string.Empty, true);
        public static CountryPropertyDescriptor s_Country = new CountryPropertyDescriptor(nameof(Country), string.Empty, string.Empty, true);
        public static LanguagePropertyDescriptor s_Language = new LanguagePropertyDescriptor(nameof(Language), string.Empty, string.Empty, false);
        public static EmailPropertyDescriptor s_Email = new EmailPropertyDescriptor(nameof(Email), string.Empty, string.Empty, false);

        public override PropertyDescriptor[] PropertyDescriptors
        {
            get
            {
                if (s_dataProviderProperties == null)
                    s_dataProviderProperties = new PropertyDescriptor[16]
                    {
             s_Street1,
             s_Street2,
             s_City,
             s_District,
             s_State,
             s_PostalCode,
             s_PhoneNumber,
             s_PhoneExtension,
             s_CardType,
             s_AccountHolderName,
             s_AccountNumber,
             s_CcvNumber,
             s_ExpirationDate,
             s_Country,
             s_Language,
             s_Email
                    };
                return s_dataProviderProperties;
            }
        }

        public static PropertyDescriptor Street1 => s_Street1;

        public static PropertyDescriptor Street2 => s_Street2;

        public static PropertyDescriptor City => s_City;

        public static PropertyDescriptor District => s_District;

        public static PropertyDescriptor State => s_State;

        public static PropertyDescriptor PostalCode => s_PostalCode;

        public static PropertyDescriptor PhoneNumber => s_PhoneNumber;

        public static PropertyDescriptor PhoneExtension => s_PhoneExtension;

        public static PropertyDescriptor CardType => s_CardType;

        public static PropertyDescriptor AccountHolderName => s_AccountHolderName;

        public static PropertyDescriptor AccountNumber => s_AccountNumber;

        public static PropertyDescriptor CcvNumber => s_CcvNumber;

        public static PropertyDescriptor ExpirationDate => s_ExpirationDate;

        public static PropertyDescriptor Country => s_Country;

        public static PropertyDescriptor Language => s_Language;

        public static PropertyDescriptor Email => s_Email;
    }
}
