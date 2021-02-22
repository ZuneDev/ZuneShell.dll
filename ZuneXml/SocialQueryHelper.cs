// Decompiled with JetBrains decompiler
// Type: ZuneXml.SocialQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Text;
using ZuneUI;

namespace ZuneXml
{
    internal class SocialQueryHelper : ZuneServiceQueryHelper
    {
        internal static ZuneServiceQueryHelper ConstructSocialQueryHelper(
          ZuneServiceQuery query)
        {
            return new SocialQueryHelper(query);
        }

        internal SocialQueryHelper(ZuneServiceQuery query)
          : base(query)
        {
        }

        internal override object GetComputedProperty(string propertyName) => propertyName == "URI" ? this.GetResourceUri() : base.GetComputedProperty(propertyName);

        internal override string GetResourceUri()
        {
            string zuneTagOrGuid = null;
            Guid? property1 = (Guid?)this.Query.GetProperty("UserGuid");
            if (property1.HasValue && property1.Value != Guid.Empty)
                zuneTagOrGuid = property1.Value.ToString().ToUpper();
            if (string.IsNullOrEmpty(zuneTagOrGuid))
                zuneTagOrGuid = this.Query.GetProperty("ZuneTag") as string;
            StringBuilder args = new StringBuilder(ComposerHelper.CreateOperationUri(this.Query.GetProperty("Operation") as string, zuneTagOrGuid));
            bool first = true;
            int? property2 = (int?)this.Query.GetProperty("StartIndex");
            if (property2.HasValue)
            {
                UrlHelper.AppendParam(first, args, "startIndex", property2.ToString());
                first = false;
            }
            int? property3 = (int?)this.Query.GetProperty("ChunkSize");
            if (property3.HasValue)
                UrlHelper.AppendParam(first, args, "chunkSize", property3.ToString());
            return args.ToString();
        }
    }
}
