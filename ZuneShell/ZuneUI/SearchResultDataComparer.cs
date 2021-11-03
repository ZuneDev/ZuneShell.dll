// Decompiled with JetBrains decompiler
// Type: ZuneUI.SearchResultDataComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class SearchResultDataComparer : IComparer
    {
        private string GetStringOrDataProviderTitle(object value) => value is DataProviderObject ? (string)((DataProviderObject)value).GetProperty("Title") : value as string;

        public int Compare(object x, object y)
        {
            string dataProviderTitle1 = this.GetStringOrDataProviderTitle(x);
            string dataProviderTitle2 = this.GetStringOrDataProviderTitle(y);
            return dataProviderTitle1 != null && dataProviderTitle2 != null ? dataProviderTitle1.CompareTo(dataProviderTitle2) : 1;
        }
    }
}
