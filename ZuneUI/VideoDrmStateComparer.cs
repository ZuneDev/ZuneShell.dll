// Decompiled with JetBrains decompiler
// Type: ZuneUI.VideoDrmStateComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class VideoDrmStateComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            DataProviderObject dataProviderObject1 = x as DataProviderObject;
            DataProviderObject dataProviderObject2 = y as DataProviderObject;
            if (dataProviderObject1 == null || dataProviderObject2 == null)
                return 1;
            int num1 = (int)dataProviderObject1.GetProperty("DrmState");
            int num2 = (int)dataProviderObject2.GetProperty("DrmState");
            if (num1 == 20 || num1 == 23)
                num1 = 26;
            if (num2 == 20 || num2 == 23)
                num2 = 26;
            return num1.CompareTo(num2);
        }
    }
}
