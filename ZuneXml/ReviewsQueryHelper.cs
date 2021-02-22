// Decompiled with JetBrains decompiler
// Type: ZuneXml.ReviewsQueryHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneXml
{
    internal class ReviewsQueryHelper : SubRepresentationCatalogServiceQueryHelper
    {
        internal static ZuneServiceQueryHelper ConstructReviewsQueryHelper(
          ZuneServiceQuery query)
        {
            return new ReviewsQueryHelper(query);
        }

        internal ReviewsQueryHelper(ZuneServiceQuery query)
          : base(query)
          => this._endPoint = EServiceEndpointId.SEID_Stats;
    }
}
