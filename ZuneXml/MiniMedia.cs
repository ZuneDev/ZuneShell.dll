// Decompiled with JetBrains decompiler
// Type: ZuneXml.MiniMedia
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal abstract class MiniMedia : XmlDataProviderObject
    {
        protected MiniMedia(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal abstract Guid Id { get; }

        internal abstract string Title { get; }
    }
}
