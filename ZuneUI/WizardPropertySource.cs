// Decompiled with JetBrains decompiler
// Type: ZuneUI.WizardPropertySource
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class WizardPropertySource : PropertySource
    {
        private static PropertySource _instance;

        private WizardPropertySource()
        {
        }

        public static PropertySource Instance
        {
            get
            {
                if (WizardPropertySource._instance == null)
                    WizardPropertySource._instance = (PropertySource)new WizardPropertySource();
                return WizardPropertySource._instance;
            }
        }

        public override object Get(object model, PropertyDescriptor property) => !(model is WizardPropertyEditorPage propertyEditorPage) ? (object)null : propertyEditorPage.GetCommittedValue(property);

        public override void Set(object model, PropertyDescriptor property, object value)
        {
            if (!(model is WizardPropertyEditorPage propertyEditorPage))
                return;
            propertyEditorPage.SetCommittedValue(property, value);
        }
    }
}
