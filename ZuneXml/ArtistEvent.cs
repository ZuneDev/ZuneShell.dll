// Decompiled with JetBrains decompiler
// Type: ZuneXml.ArtistEvent
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal class ArtistEvent : XmlDataProviderObject
    {
        internal static XmlDataProviderObject ConstructArtistEventObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new ArtistEvent(owner, objectTypeCookie);
        }

        internal ArtistEvent(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal string Type => (string)this.GetProperty(nameof(Type));

        internal string WebLinkUrl => (string)this.GetProperty(nameof(WebLinkUrl));

        internal DateTime Date => (DateTime)this.GetProperty(nameof(Date));

        internal string Venue => (string)this.GetProperty(nameof(Venue));

        internal string City => (string)this.GetProperty(nameof(City));

        internal string State => (string)this.GetProperty(nameof(State));

        internal string Country => (string)this.GetProperty(nameof(Country));
    }
}
