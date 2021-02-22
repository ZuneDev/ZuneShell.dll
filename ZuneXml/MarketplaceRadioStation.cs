// Decompiled with JetBrains decompiler
// Type: ZuneXml.MarketplaceRadioStation
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneXml
{
    internal class MarketplaceRadioStation : MiniMedia
    {
        internal static XmlDataProviderObject ConstructMarketplaceRadioStationObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new MarketplaceRadioStation(owner, objectTypeCookie);
        }

        internal MarketplaceRadioStation(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal IList Genres => (IList)this.GetProperty(nameof(Genres));

        internal override Guid Id => (Guid)this.GetProperty(nameof(Id));

        internal override string Title => (string)this.GetProperty(nameof(Title));
    }
}
