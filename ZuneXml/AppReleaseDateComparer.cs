// Decompiled with JetBrains decompiler
// Type: ZuneXml.AppReleaseDateComparer
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneXml
{
    public class AppReleaseDateComparer : PropertyComparer<DateTime>
    {
        public AppReleaseDateComparer()
          : base(new Converter<object, DateTime>(AppReleaseDateComparer.GetReleaseDate), true)
        {
        }

        private static DateTime GetReleaseDate(object o) => !(o is App app) ? DateTime.MinValue : app.ReleaseDate;
    }
}
