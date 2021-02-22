// Decompiled with JetBrains decompiler
// Type: ZuneUI.ReleaseYearComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneUI
{
    public class ReleaseYearComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            DataProviderObject dataProviderObject1 = x as DataProviderObject;
            DataProviderObject dataProviderObject2 = y as DataProviderObject;
            return dataProviderObject1 != null && dataProviderObject2 != null ? ((DateTime)dataProviderObject1.GetProperty("ReleaseDate")).Year.CompareTo(((DateTime)dataProviderObject2.GetProperty("ReleaseDate")).Year) : 1;
        }
    }
}
