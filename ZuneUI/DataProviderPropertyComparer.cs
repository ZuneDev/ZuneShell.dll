// Decompiled with JetBrains decompiler
// Type: ZuneUI.DataProviderPropertyComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneUI
{
    public class DataProviderPropertyComparer : IComparer
    {
        private string _propertyName;

        public DataProviderPropertyComparer()
        {
        }

        public DataProviderPropertyComparer(string propertyName) => this._propertyName = propertyName;

        public string PropertyName
        {
            get => this._propertyName;
            set => this._propertyName = value;
        }

        public int Compare(object x, object y)
        {
            DataProviderObject dataProviderObject1 = x as DataProviderObject;
            DataProviderObject dataProviderObject2 = y as DataProviderObject;
            if (dataProviderObject1 != null && dataProviderObject2 != null)
            {
                object property1 = dataProviderObject1.GetProperty(this.PropertyName);
                object property2 = dataProviderObject2.GetProperty(this.PropertyName);
                if (property1 is IComparable comparable)
                    return comparable.CompareTo(property2);
                if (Equals(property1, property2))
                    return 0;
            }
            return 1;
        }
    }
}
