// Decompiled with JetBrains decompiler
// Type: ZuneXml.ZuneServiceQuery
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections.Generic;

namespace ZuneXml
{
    internal class ZuneServiceQuery : XmlDataProviderQuery
    {
        private static IDictionary<string, ZuneServiceQuery.ConstructQueryHelper> _queryTypeToHelper;
        private ZuneServiceQueryHelper _helper;

        private static void RegisterZuneServiceQueryConstructors(
          IDictionary<string, ZuneServiceQuery.ConstructQueryHelper> queryTypeToHelper)
        {
            queryTypeToHelper.Add("Marketplace", new ZuneServiceQuery.ConstructQueryHelper(CatalogServiceQueryHelper.ConstructMusicCatalogQueryHelper));
            queryTypeToHelper.Add("MarketplaceSearch", new ZuneServiceQuery.ConstructQueryHelper(CatalogSearchQueryHelper.ConstructSearchQueryHelper));
            queryTypeToHelper.Add("Messaging", new ZuneServiceQuery.ConstructQueryHelper(MessagingQueryHelper.ConstructMessagingQueryHelper));
            queryTypeToHelper.Add("PodcastMarketplace", new ZuneServiceQuery.ConstructQueryHelper(PodcastCatalogServiceQueryHelper.ConstructPodcastCatalogQueryHelper));
            queryTypeToHelper.Add("PodcastMarketplaceSearch", new ZuneServiceQuery.ConstructQueryHelper(CatalogSearchQueryHelper.ConstructSearchQueryHelper));
            queryTypeToHelper.Add("PrefixSearch", new ZuneServiceQuery.ConstructQueryHelper(CatalogPrefixSearchQueryHelper.ConstructPrefixSearchQueryHelper));
            queryTypeToHelper.Add("Recommendations", new ZuneServiceQuery.ConstructQueryHelper(RecommendationsQueryHelper.ConstructRecommendationsQueryHelper));
            queryTypeToHelper.Add("Social", new ZuneServiceQuery.ConstructQueryHelper(SocialQueryHelper.ConstructSocialQueryHelper));
            queryTypeToHelper.Add("TopListeners", new ZuneServiceQuery.ConstructQueryHelper(TopListenersQueryHelper.ConstructTopListenersQueryHelper));
            queryTypeToHelper.Add("UriResource", new ZuneServiceQuery.ConstructQueryHelper(ZuneServiceQueryHelper.ConstructZuneServiceQueryHelper));
            queryTypeToHelper.Add("VideoMarketplace", new ZuneServiceQuery.ConstructQueryHelper(VideoCatalogServiceQueryHelper.ConstructVideoCatalogQuery));
            queryTypeToHelper.Add("VideoMarketplaceSearch", new ZuneServiceQuery.ConstructQueryHelper(CatalogSearchQueryHelper.ConstructSearchQueryHelper));
            queryTypeToHelper.Add("AppDetails", new ZuneServiceQuery.ConstructQueryHelper(AppDetailsQueryHelper.ConstructAppDetailsQueryHelper));
            queryTypeToHelper.Add("AppGenres", new ZuneServiceQuery.ConstructQueryHelper(AppGenresQueryHelper.ConstructAppGenresQueryHelper));
            queryTypeToHelper.Add("Reviews", new ZuneServiceQuery.ConstructQueryHelper(ReviewsQueryHelper.ConstructReviewsQueryHelper));
            queryTypeToHelper.Add("SubscriptionHistory", new ZuneServiceQuery.ConstructQueryHelper(SubscriptionHistoryQueryHelper.ConstructSubscriptionHistoryQueryHelper));
            queryTypeToHelper.Add("PurchaseHistory", new ZuneServiceQuery.ConstructQueryHelper(PurchaseHistoryQueryHelper.ConstructPurchaseHistoryQueryHelper));
        }

        internal static DataProviderQuery ConstructZuneServiceQuery(
          object queryTypeCookie)
        {
            return (DataProviderQuery)new ZuneServiceQuery(queryTypeCookie);
        }

        internal ZuneServiceQuery(object queryTypeCookie)
          : base(queryTypeCookie)
          => this._acceptGZipEncoding = true;

        internal ZuneServiceQueryHelper Helper
        {
            get
            {
                if (this._helper == null)
                {
                    if (ZuneServiceQuery._queryTypeToHelper == null)
                    {
                        ZuneServiceQuery._queryTypeToHelper = (IDictionary<string, ZuneServiceQuery.ConstructQueryHelper>)new Dictionary<string, ZuneServiceQuery.ConstructQueryHelper>(17);
                        ZuneServiceQuery.RegisterZuneServiceQueryConstructors(ZuneServiceQuery._queryTypeToHelper);
                    }
                    string property = base.GetProperty("QueryType") as string;
                    this._helper = ZuneServiceQuery._queryTypeToHelper[property](this);
                }
                return this._helper;
            }
        }

        public override object GetProperty(string propertyName) => this.Helper.GetComputedProperty(propertyName) ?? base.GetProperty(propertyName);

        protected override string GetResourceUri() => this.Helper.GetResourceUri();

        protected override string GetPostBody() => this.Helper.GetQueryPostBody();

        protected override void BeginExecute()
        {
            if (this.Helper.HandleQueryBeginExecute())
                return;
            base.BeginExecute();
        }

        internal override bool FilterDataProviderObject(XmlDataProviderObject dataObject) => this.Helper.OnQueryFilterDataProviderObject(dataObject);

        protected override void OnPropertyChanged(string propertyName)
        {
            if (!this.Initialized)
                return;
            base.OnPropertyChanged(propertyName);
            this.Helper.OnQueryPropertyChanged(propertyName);
        }

        private delegate ZuneServiceQueryHelper ConstructQueryHelper(
          ZuneServiceQuery query);
    }
}
