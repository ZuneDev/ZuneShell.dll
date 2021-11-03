// Decompiled with JetBrains decompiler
// Type: ZuneXml.HistoryQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using System.Text;

namespace ZuneXml
{
    internal class HistoryQueryHelper : ZuneServiceQueryHelper
    {
        protected string _api;

        internal HistoryQueryHelper(ZuneServiceQuery query)
          : base(query)
        {
            query.PassportTicketType = EPassportPolicyId.MBI_SSL;
            query.CachePolicy = HttpRequestCachePolicy.BypassCache;
        }

        internal override string GetResourceUri()
        {
            StringBuilder requestUri = new StringBuilder(128);
            requestUri.Append(Service.GetEndPointUri(EServiceEndpointId.SEID_CommerceV2));
            requestUri.Append(this._api);
            bool fFirst = true;
            AppendParam(requestUri, "tunerType", "zunePCClient", ref fFirst);
            if (this.Query.GetProperty("MediaType") is string property)
                AppendParam(requestUri, "mediaTypeOrCategory", property, ref fFirst);
            AppendParam(requestUri, "startIndex", "1", ref fFirst);
            object property1 = this.Query.GetProperty("ChunkSize");
            if (property1 is int)
                AppendParam(requestUri, "chunkSize", property1.ToString(), ref fFirst);
            return requestUri.ToString();
        }

        internal override string GetQueryPostBody() => "";
    }
}
