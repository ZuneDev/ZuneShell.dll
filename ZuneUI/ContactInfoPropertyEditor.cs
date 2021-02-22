// Decompiled with JetBrains decompiler
// Type: ZuneUI.ContactInfoPropertyEditor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class ContactInfoPropertyEditor : BaseContactInfoPropertyEditor
    {
        private static PropertyDescriptor[] s_dataProviderProperties;
        public static CountryFieldValidationPropertyDescriptor s_Street1 = new CountryFieldValidationPropertyDescriptor(nameof(Street1), CountryFieldValidatorType.Street1);
        public static CountryFieldValidationPropertyDescriptor s_Street2 = new CountryFieldValidationPropertyDescriptor(nameof(Street2), CountryFieldValidatorType.Street2);
        public static CountryFieldValidationPropertyDescriptor s_City = new CountryFieldValidationPropertyDescriptor(nameof(City), CountryFieldValidatorType.City);
        public static CountryFieldValidationPropertyDescriptor s_District = new CountryFieldValidationPropertyDescriptor(nameof(District), CountryFieldValidatorType.District);
        public static StateDescriptor s_State = new StateDescriptor(nameof(State));
        public static CountryPropertyDescriptor s_Country = new CountryPropertyDescriptor(nameof(Country), string.Empty, string.Empty, true);
        public static PostalCodeDescriptor s_PostalCode = new PostalCodeDescriptor(nameof(PostalCode));
        public static LanguagePropertyDescriptor s_Language = new LanguagePropertyDescriptor(nameof(Language), string.Empty, string.Empty, true);

        public override PropertyDescriptor[] PropertyDescriptors
        {
            get
            {
                if (ContactInfoPropertyEditor.s_dataProviderProperties == null)
                    ContactInfoPropertyEditor.s_dataProviderProperties = new PropertyDescriptor[13]
                    {
            (PropertyDescriptor) BaseContactInfoPropertyEditor.s_FirstName,
            (PropertyDescriptor) BaseContactInfoPropertyEditor.s_LastName,
            (PropertyDescriptor) ContactInfoPropertyEditor.s_Street1,
            (PropertyDescriptor) ContactInfoPropertyEditor.s_Street2,
            (PropertyDescriptor) ContactInfoPropertyEditor.s_City,
            (PropertyDescriptor) ContactInfoPropertyEditor.s_District,
            (PropertyDescriptor) ContactInfoPropertyEditor.s_State,
            (PropertyDescriptor) ContactInfoPropertyEditor.s_Country,
            (PropertyDescriptor) ContactInfoPropertyEditor.s_PostalCode,
            (PropertyDescriptor) BaseContactInfoPropertyEditor.s_PhoneNumber,
            (PropertyDescriptor) BaseContactInfoPropertyEditor.s_PhoneExtension,
            (PropertyDescriptor) BaseContactInfoPropertyEditor.s_Email,
            (PropertyDescriptor) ContactInfoPropertyEditor.s_Language
                    };
                return ContactInfoPropertyEditor.s_dataProviderProperties;
            }
        }

        public static PropertyDescriptor Street1 => (PropertyDescriptor)ContactInfoPropertyEditor.s_Street1;

        public static PropertyDescriptor Street2 => (PropertyDescriptor)ContactInfoPropertyEditor.s_Street2;

        public static PropertyDescriptor City => (PropertyDescriptor)ContactInfoPropertyEditor.s_City;

        public static PropertyDescriptor District => (PropertyDescriptor)ContactInfoPropertyEditor.s_District;

        public static PropertyDescriptor State => (PropertyDescriptor)ContactInfoPropertyEditor.s_State;

        public static PropertyDescriptor Country => (PropertyDescriptor)ContactInfoPropertyEditor.s_Country;

        public static PropertyDescriptor PostalCode => (PropertyDescriptor)ContactInfoPropertyEditor.s_PostalCode;

        public static PropertyDescriptor Language => (PropertyDescriptor)ContactInfoPropertyEditor.s_Language;
    }
}
