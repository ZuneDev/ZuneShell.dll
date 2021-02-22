// Decompiled with JetBrains decompiler
// Type: ZuneXml.SubRepresentationCatalogServiceQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Text;

namespace ZuneXml
{
    internal class SubRepresentationCatalogServiceQueryHelper : CatalogServiceQueryHelper
    {
        internal SubRepresentationCatalogServiceQueryHelper(ZuneServiceQuery query)
          : base(query)
        {
        }

        protected override void AppendStuffAfterRepresentation(
          StringBuilder requestUri,
          ref bool fFirst)
        {
            string property1 = (string)this.Query.GetProperty("SubId");
            string property2 = (string)this.Query.GetProperty("SubRepresentation");
            if (!string.IsNullOrEmpty(property1))
            {
                requestUri.Append("/");
                requestUri.Append(property1);
            }
            if (!string.IsNullOrEmpty(property2))
            {
                requestUri.Append("/");
                requestUri.Append(property2);
            }
            base.AppendStuffAfterRepresentation(requestUri, ref fFirst);
        }
    }
}
