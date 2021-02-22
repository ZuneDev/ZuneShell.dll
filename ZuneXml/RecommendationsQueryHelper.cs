// Decompiled with JetBrains decompiler
// Type: ZuneXml.RecommendationsQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneXml
{
    internal class RecommendationsQueryHelper : CatalogServiceQueryHelper
    {
        internal static ZuneServiceQueryHelper ConstructRecommendationsQueryHelper(
          ZuneServiceQuery query)
        {
            return (ZuneServiceQueryHelper)new RecommendationsQueryHelper(query);
        }

        internal RecommendationsQueryHelper(ZuneServiceQuery query)
          : base(query)
        {
            this._endPoint = EServiceEndpointId.SEID_Recommendations;
            query.CachePolicy = HttpRequestCachePolicy.BypassCache;
        }
    }
}
