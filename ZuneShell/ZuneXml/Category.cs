// Decompiled with JetBrains decompiler
// Type: ZuneXml.Category
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneXml
{
    internal abstract class Category : XmlDataProviderObject
    {
        protected Category(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal abstract string Id { get; }

        internal abstract string Title { get; }
    }
}
