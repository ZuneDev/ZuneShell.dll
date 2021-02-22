// Decompiled with JetBrains decompiler
// Type: ZuneXml.WinPhoneAppGenre
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneXml
{
    internal class WinPhoneAppGenre : AppGenre
    {
        internal static XmlDataProviderObject ConstructWinPhoneAppGenreObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new WinPhoneAppGenre(owner, objectTypeCookie);
        }

        internal WinPhoneAppGenre(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }
    }
}
