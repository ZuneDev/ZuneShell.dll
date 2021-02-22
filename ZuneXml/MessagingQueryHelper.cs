// Decompiled with JetBrains decompiler
// Type: ZuneXml.MessagingQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using ZuneUI;

namespace ZuneXml
{
    internal class MessagingQueryHelper : ZuneServiceQueryHelper
    {
        private EServiceEndpointId _endPoint;

        internal static ZuneServiceQueryHelper ConstructMessagingQueryHelper(
          ZuneServiceQuery query)
        {
            return new MessagingQueryHelper(query);
        }

        internal MessagingQueryHelper(ZuneServiceQuery query)
          : base(query)
        {
            this._endPoint = EServiceEndpointId.SEID_Messaging;
            query.CachePolicy = HttpRequestCachePolicy.BypassCache;
            query.PassportTicketType = EPassportPolicyId.MBI_SSL;
        }

        internal override string GetResourceUri()
        {
            string property1 = (string)this.Query.GetProperty("ZuneTag");
            if (string.IsNullOrEmpty(property1))
                return null;
            string property2 = (string)this.Query.GetProperty("RequestType");
            return string.IsNullOrEmpty(property2) ? null : UrlHelper.MakeUrl(string.Format("{0}/messaging/{1}/inbox/{2}", Service.GetEndPointUri(this._endPoint), property1.ToLower(), property2));
        }
    }
}
