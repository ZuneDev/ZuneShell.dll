// Decompiled with JetBrains decompiler
// Type: ZuneXml.Thumbnail
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal abstract class Thumbnail : XmlDataProviderObject
    {
        protected Thumbnail(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal abstract string Id { get; }

        internal abstract string Title { get; }

        internal abstract string SortTitle { get; }

        internal abstract Guid ImageId { get; }
    }
}
