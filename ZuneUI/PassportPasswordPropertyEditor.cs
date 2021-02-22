// Decompiled with JetBrains decompiler
// Type: ZuneUI.PassportPasswordPropertyEditor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class PassportPasswordPropertyEditor : WizardPropertyEditor
    {
        private static PropertyDescriptor[] s_dataProviderProperties;
        public static PropertyDescriptor s_Email = new PropertyDescriptor(nameof(Email), string.Empty, string.Empty, true);
        public static PropertyDescriptor s_Password = new PropertyDescriptor(nameof(Password), string.Empty, string.Empty, true);

        public override PropertyDescriptor[] PropertyDescriptors
        {
            get
            {
                if (PassportPasswordPropertyEditor.s_dataProviderProperties == null)
                    PassportPasswordPropertyEditor.s_dataProviderProperties = new PropertyDescriptor[2]
                    {
            PassportPasswordPropertyEditor.s_Email,
            PassportPasswordPropertyEditor.s_Password
                    };
                return PassportPasswordPropertyEditor.s_dataProviderProperties;
            }
        }

        public static PropertyDescriptor Password => PassportPasswordPropertyEditor.s_Password;

        public static PropertyDescriptor Email => PassportPasswordPropertyEditor.s_Email;
    }
}
