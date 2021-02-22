// Decompiled with JetBrains decompiler
// Type: ZuneXml.PodcastCatalogServiceQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Text;

namespace ZuneXml
{
    internal class PodcastCatalogServiceQueryHelper : CatalogServiceQueryHelper
    {
        internal static ZuneServiceQueryHelper ConstructPodcastCatalogQueryHelper(
          ZuneServiceQuery query)
        {
            return new PodcastCatalogServiceQueryHelper(query);
        }

        internal PodcastCatalogServiceQueryHelper(ZuneServiceQuery query)
          : base(query)
        {
        }

        protected override bool RequireId => false;

        protected override bool RequireResource => false;

        protected override bool RequireRepresentation => false;

        protected override void AppendStuffAfterRepresentation(
          StringBuilder requestUri,
          ref bool fFirst)
        {
            base.AppendStuffAfterRepresentation(requestUri, ref fFirst);
            string property1 = (string)this.Query.GetProperty("PodcastType");
            if (!string.IsNullOrEmpty(property1))
                AppendParam(requestUri, "type", property1, ref fFirst);
            string property2 = (string)this.Query.GetProperty("PodcastUrl");
            if (string.IsNullOrEmpty(property2))
                return;
            AppendParam(requestUri, "url", property2, ref fFirst);
        }

        internal override string GetQueryPostBody()
        {
            string property = (string)this.Query.GetProperty("PostUrl");
            return !string.IsNullOrEmpty(property) ? "URL=" + property : null;
        }
    }
}
