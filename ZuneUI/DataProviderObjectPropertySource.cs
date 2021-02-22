// Decompiled with JetBrains decompiler
// Type: ZuneUI.DataProviderObjectPropertySource
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class DataProviderObjectPropertySource : PropertySource
    {
        private static PropertySource _instance;

        protected DataProviderObjectPropertySource()
        {
        }

        public static PropertySource Instance
        {
            get
            {
                if (DataProviderObjectPropertySource._instance == null)
                    DataProviderObjectPropertySource._instance = (PropertySource)new DataProviderObjectPropertySource();
                return DataProviderObjectPropertySource._instance;
            }
        }

        public override object Get(object media, PropertyDescriptor property) => !(media is DataProviderObject dataProviderObject) ? (object)null : dataProviderObject.GetProperty(property.DescriptorName);

        public override void Set(object media, PropertyDescriptor property, object value)
        {
            if (!(media is DataProviderObject dataProviderObject))
                return;
            dataProviderObject.SetProperty(property.DescriptorName, value);
        }
    }
}
