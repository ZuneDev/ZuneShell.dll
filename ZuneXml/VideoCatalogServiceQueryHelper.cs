// Decompiled with JetBrains decompiler
// Type: ZuneXml.VideoCatalogServiceQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneXml
{
    internal class VideoCatalogServiceQueryHelper : SubRepresentationCatalogServiceQueryHelper
    {
        internal static ZuneServiceQueryHelper ConstructVideoCatalogQuery(
          ZuneServiceQuery query)
        {
            return (ZuneServiceQueryHelper)new VideoCatalogServiceQueryHelper(query);
        }

        internal VideoCatalogServiceQueryHelper(ZuneServiceQuery query)
          : base(query)
        {
        }

        protected override bool RequireId => false;

        protected override bool RequireResource => false;

        protected override bool RequireRepresentation => false;
    }
}
