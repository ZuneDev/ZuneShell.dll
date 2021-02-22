// Decompiled with JetBrains decompiler
// Type: ZuneXml.AppTitleComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneXml
{
    public class AppTitleComparer : PropertyComparer<string>
    {
        public AppTitleComparer()
          : base(new Converter<object, string>(AppTitleComparer.GetSortTitle), false)
        {
        }

        private static string GetSortTitle(object o) => !(o is App app) ? (string)null : app.SortTitle;
    }
}
