// Decompiled with JetBrains decompiler
// Type: ZuneUI.BasicAccountInfoPropertyEditor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class BasicAccountInfoPropertyEditor : WizardPropertyEditor
    {
        private static PropertyDescriptor[] s_dataProviderProperties;
        public static CountryPropertyDescriptor s_Country = new CountryPropertyDescriptor("HomeAddress.Country", string.Empty, string.Empty, true);
        public static LanguagePropertyDescriptor s_Language = new LanguagePropertyDescriptor("Personal.LanguagePreference", string.Empty, string.Empty, true);
        public static BirthdayPropertyDescriptor s_Birthday = new BirthdayPropertyDescriptor("Personal.Birthday", string.Empty, string.Empty, true);
        public static PostalCodeDescriptor s_PostalCode = new PostalCodeDescriptor(nameof(PostalCode));

        public override PropertyDescriptor[] PropertyDescriptors
        {
            get
            {
                if (s_dataProviderProperties == null)
                    s_dataProviderProperties = new PropertyDescriptor[4]
                    {
             s_Country,
             s_Language,
             s_Birthday,
             s_PostalCode
                    };
                return s_dataProviderProperties;
            }
        }

        public static PropertyDescriptor Country => s_Country;

        public static PropertyDescriptor Language => s_Language;

        public static PropertyDescriptor Birthday => s_Birthday;

        public static PropertyDescriptor PostalCode => s_PostalCode;
    }
}
