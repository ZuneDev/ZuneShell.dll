// Decompiled with JetBrains decompiler
// Type: ZuneUI.CreatePassportPropertyEditor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class CreatePassportPropertyEditor : WizardPropertyEditor
    {
        private static PropertyDescriptor[] s_dataProviderProperties;
        public static PropertyDescriptor s_PassportId = new PropertyDescriptor("Name", string.Empty, string.Empty, true);
        public static PropertyDescriptor s_PassportDomain = new EmailDomainPropertyDescriptor("Domain", string.Empty, string.Empty);
        public static PropertyDescriptor s_Password1 = new NonBlankPropertyDescriptor("Password", string.Empty, string.Empty, 6, true);
        public static PropertyDescriptor s_Password2 = new NonBlankPropertyDescriptor(nameof(Password2), string.Empty, string.Empty, 6, true);
        public static PropertyDescriptor s_SecretQuestion = new PropertyDescriptor(nameof(SecretQuestion), string.Empty, string.Empty, true);
        public static NonBlankPropertyDescriptor s_SecretAnswer = new NonBlankPropertyDescriptor(nameof(SecretAnswer), string.Empty, string.Empty, 5, true);

        public override PropertyDescriptor[] PropertyDescriptors
        {
            get
            {
                if (s_dataProviderProperties == null)
                    s_dataProviderProperties = new PropertyDescriptor[6]
                    {
            s_PassportId,
            s_PassportDomain,
            s_Password1,
            s_Password2,
            s_SecretQuestion,
             s_SecretAnswer
                    };
                return s_dataProviderProperties;
            }
        }

        public override bool IsValid()
        {
            HRESULT hresult = HRESULT._S_OK;
            MetadataEditProperty property1 = this.GetProperty(s_Password1);
            MetadataEditProperty property2 = this.GetProperty(s_Password2);
            if (property1.Value != property2.Value)
                hresult = HRESULT._ZUNE_E_SIGNUP_PASSWORDS_DONT_MATCH;
            if (hresult.IsError || property1.ExternalError == HRESULT._ZUNE_E_SIGNUP_INVALID_PARENT_EMAIL)
                property1.ExternalError = hresult;
            if (hresult.IsError || property2.ExternalError == HRESULT._ZUNE_E_SIGNUP_INVALID_PARENT_EMAIL)
                property2.ExternalError = hresult;
            return base.IsValid();
        }

        public static PropertyDescriptor PassportId => s_PassportId;

        public static PropertyDescriptor PassportDomain => s_PassportDomain;

        public static PropertyDescriptor Password1 => s_Password1;

        public static PropertyDescriptor Password2 => s_Password2;

        public static PropertyDescriptor SecretQuestion => s_SecretQuestion;

        public static NonBlankPropertyDescriptor SecretAnswer => s_SecretAnswer;
    }
}
