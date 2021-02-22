// Decompiled with JetBrains decompiler
// Type: ZuneXml.PropertyComparer`1
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;

namespace ZuneXml
{
    public class PropertyComparer<TPropertyType> : IComparer where TPropertyType : IComparable<TPropertyType>
    {
        private Converter<object, TPropertyType> _propertyGetter;
        private bool _sortDescending;

        public PropertyComparer(Converter<object, TPropertyType> propertyGetter, bool sortDescending)
        {
            this._propertyGetter = propertyGetter != null ? propertyGetter : throw new ArgumentNullException(nameof(propertyGetter));
            this._sortDescending = sortDescending;
        }

        public int Compare(object x, object y)
        {
            int comparisonResult = 0;
            if (!PropertyComparer<TPropertyType>.IsNullComparison(x, y, out comparisonResult))
            {
                TPropertyType propertyType = this._propertyGetter(x);
                TPropertyType other = this._propertyGetter(y);
                if (!PropertyComparer<TPropertyType>.IsNullComparison((object)propertyType, (object)other, out comparisonResult))
                    comparisonResult = propertyType.CompareTo(other);
            }
            if (this._sortDescending)
                comparisonResult = -comparisonResult;
            return comparisonResult;
        }

        private static bool IsNullComparison(object x, object y, out int comparisonResult)
        {
            comparisonResult = 0;
            bool flag = false;
            if (x == null)
            {
                flag = true;
                if (y != null)
                    comparisonResult = -1;
            }
            if (y == null)
            {
                flag = true;
                comparisonResult = 1;
            }
            return flag;
        }
    }
}
