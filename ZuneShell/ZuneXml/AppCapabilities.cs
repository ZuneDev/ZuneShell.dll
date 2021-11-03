// Decompiled with JetBrains decompiler
// Type: ZuneXml.AppCapabilities
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneXml
{
    internal class AppCapabilities : XmlDataProviderObject
    {
        internal static XmlDataProviderObject ConstructAppCapabilitiesObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return new AppCapabilities(owner, objectTypeCookie);
        }

        internal AppCapabilities(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal IList Items => (IList)this.GetProperty(nameof(Items));
    }
}
