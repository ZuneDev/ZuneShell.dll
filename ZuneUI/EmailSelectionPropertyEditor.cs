// Decompiled with JetBrains decompiler
// Type: ZuneUI.EmailSelectionPropertyEditor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class EmailSelectionPropertyEditor : WizardPropertyEditor
    {
        private static PropertyDescriptor[] s_dataProviderProperties;
        public static PropertyDescriptor s_Email = (PropertyDescriptor)new EmailPropertyDescriptor(nameof(Email), string.Empty, string.Empty, true);

        public override PropertyDescriptor[] PropertyDescriptors
        {
            get
            {
                if (EmailSelectionPropertyEditor.s_dataProviderProperties == null)
                    EmailSelectionPropertyEditor.s_dataProviderProperties = new PropertyDescriptor[1]
                    {
            EmailSelectionPropertyEditor.s_Email
                    };
                return EmailSelectionPropertyEditor.s_dataProviderProperties;
            }
        }

        public static PropertyDescriptor Email => EmailSelectionPropertyEditor.s_Email;
    }
}
