// Decompiled with JetBrains decompiler
// Type: ZuneUI.VideoSeasonComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class VideoSeasonComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            DataProviderObject dataProviderObject1 = x as DataProviderObject;
            DataProviderObject dataProviderObject2 = y as DataProviderObject;
            if (dataProviderObject1 == null || dataProviderObject2 == null)
                return 1;
            int property1 = (int)dataProviderObject1.GetProperty("CategoryId");
            int property2 = (int)dataProviderObject2.GetProperty("CategoryId");
            int num1 = property1.CompareTo(property2);
            if (num1 != 0)
                return num1;
            if (property1 != 5)
                return 0;
            int num2 = string.Compare((string)dataProviderObject1.GetProperty("SeriesTitle"), (string)dataProviderObject2.GetProperty("SeriesTitle"));
            return num2 != 0 ? num2 : ((int)dataProviderObject1.GetProperty("SeasonNumber")).CompareTo((int)dataProviderObject2.GetProperty("SeasonNumber"));
        }
    }
}
