// Decompiled with JetBrains decompiler
// Type: ZuneUI.BaseContactInfoPropertyEditor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public abstract class BaseContactInfoPropertyEditor : WizardPropertyEditor
    {
        public static CountryFieldValidationPropertyDescriptor s_FirstName = new CountryFieldValidationPropertyDescriptor(nameof(FirstName), CountryFieldValidatorType.FirstName);
        public static CountryFieldValidationPropertyDescriptor s_LastName = new CountryFieldValidationPropertyDescriptor(nameof(LastName), CountryFieldValidatorType.LastName);
        public static PhoneNumberDescriptors s_PhoneNumber = new PhoneNumberDescriptors(nameof(PhoneNumber), CountryFieldValidatorType.PhoneNumber);
        public static CountryFieldValidationPropertyDescriptor s_PhoneExtension = new CountryFieldValidationPropertyDescriptor(nameof(PhoneExtension), CountryFieldValidatorType.PhoneExtension);
        public static EmailPropertyDescriptor s_Email = new EmailPropertyDescriptor(nameof(Email), string.Empty, string.Empty, true);

        public static PropertyDescriptor FirstName => (PropertyDescriptor)BaseContactInfoPropertyEditor.s_FirstName;

        public static PropertyDescriptor LastName => (PropertyDescriptor)BaseContactInfoPropertyEditor.s_LastName;

        public static PropertyDescriptor PhoneNumber => (PropertyDescriptor)BaseContactInfoPropertyEditor.s_PhoneNumber;

        public static PropertyDescriptor PhoneExtension => (PropertyDescriptor)BaseContactInfoPropertyEditor.s_PhoneExtension;

        public static PropertyDescriptor Email => (PropertyDescriptor)BaseContactInfoPropertyEditor.s_Email;
    }
}
