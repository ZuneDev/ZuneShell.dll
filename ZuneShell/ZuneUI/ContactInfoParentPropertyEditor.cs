// Decompiled with JetBrains decompiler
// Type: ZuneUI.ContactInfoParentPropertyEditor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class ContactInfoParentPropertyEditor : BaseContactInfoPropertyEditor
    {
        private static PropertyDescriptor[] s_dataProviderProperties;
        public static BirthdayPropertyDescriptor s_Birthday = new BirthdayPropertyDescriptor(nameof(Birthday), string.Empty, string.Empty, true);
        public static CountryPropertyDescriptor s_Country = new CountryPropertyDescriptor(nameof(Country), string.Empty, string.Empty, true);

        public override PropertyDescriptor[] PropertyDescriptors
        {
            get
            {
                if (s_dataProviderProperties == null)
                    s_dataProviderProperties = new PropertyDescriptor[7]
                    {
             s_FirstName,
             s_LastName,
             s_PhoneNumber,
             s_PhoneExtension,
             s_Email,
             s_Birthday,
             s_Country
                    };
                return s_dataProviderProperties;
            }
        }

        public static PropertyDescriptor Birthday => s_Birthday;

        public static PropertyDescriptor Country => s_Country;
    }
}
