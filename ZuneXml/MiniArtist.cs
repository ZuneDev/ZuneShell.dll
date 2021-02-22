// Decompiled with JetBrains decompiler
// Type: ZuneXml.MiniArtist
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal class MiniArtist : MiniMedia
    {
        internal static XmlDataProviderObject ConstructMiniArtistObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new MiniArtist(owner, objectTypeCookie);
        }

        internal MiniArtist(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal override Guid Id => (Guid)this.GetProperty(nameof(Id));

        internal override string Title => (string)this.GetProperty(nameof(Title));
    }
}
