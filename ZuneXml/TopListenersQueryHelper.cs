// Decompiled with JetBrains decompiler
// Type: ZuneXml.TopListenersQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneXml
{
    internal class TopListenersQueryHelper : ZuneServiceQueryHelper
    {
        internal static ZuneServiceQueryHelper ConstructTopListenersQueryHelper(
          ZuneServiceQuery query)
        {
            return (ZuneServiceQueryHelper)new TopListenersQueryHelper(query);
        }

        internal TopListenersQueryHelper(ZuneServiceQuery query)
          : base(query)
          => query.PassportTicketType = EPassportPolicyId.MBI;

        internal override string GetResourceUri()
        {
            string property = (string)this.Query.GetProperty("ArtistId");
            return string.IsNullOrEmpty(property) ? (string)null : string.Format("{0}/music/artist/{1}/toplisteners", (object)Microsoft.Zune.Service.Service.GetEndPointUri(EServiceEndpointId.SEID_SocialApi), (object)property.ToLower());
        }
    }
}
