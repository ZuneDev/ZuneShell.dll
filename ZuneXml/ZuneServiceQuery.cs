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
        private static IDictionary<string, ConstructQueryHelper> _queryTypeToHelper;
        private ZuneServiceQueryHelper _helper;

        private static void RegisterZuneServiceQueryConstructors(
          IDictionary<string, ConstructQueryHelper> queryTypeToHelper)
        {
            queryTypeToHelper.Add("Marketplace", new ConstructQueryHelper(CatalogServiceQueryHelper.ConstructMusicCatalogQueryHelper));
            queryTypeToHelper.Add("MarketplaceSearch", new ConstructQueryHelper(CatalogSearchQueryHelper.ConstructSearchQueryHelper));
            queryTypeToHelper.Add("Messaging", new ConstructQueryHelper(MessagingQueryHelper.ConstructMessagingQueryHelper));
            queryTypeToHelper.Add("PodcastMarketplace", new ConstructQueryHelper(PodcastCatalogServiceQueryHelper.ConstructPodcastCatalogQueryHelper));
            queryTypeToHelper.Add("PodcastMarketplaceSearch", new ConstructQueryHelper(CatalogSearchQueryHelper.ConstructSearchQueryHelper));
            queryTypeToHelper.Add("PrefixSearch", new ConstructQueryHelper(CatalogPrefixSearchQueryHelper.ConstructPrefixSearchQueryHelper));
            queryTypeToHelper.Add("Recommendations", new ConstructQueryHelper(RecommendationsQueryHelper.ConstructRecommendationsQueryHelper));
            queryTypeToHelper.Add("Social", new ConstructQueryHelper(SocialQueryHelper.ConstructSocialQueryHelper));
            queryTypeToHelper.Add("TopListeners", new ConstructQueryHelper(TopListenersQueryHelper.ConstructTopListenersQueryHelper));
            queryTypeToHelper.Add("UriResource", new ConstructQueryHelper(ZuneServiceQueryHelper.ConstructZuneServiceQueryHelper));
            queryTypeToHelper.Add("VideoMarketplace", new ConstructQueryHelper(VideoCatalogServiceQueryHelper.ConstructVideoCatalogQuery));
            queryTypeToHelper.Add("VideoMarketplaceSearch", new ConstructQueryHelper(CatalogSearchQueryHelper.ConstructSearchQueryHelper));
            queryTypeToHelper.Add("AppDetails", new ConstructQueryHelper(AppDetailsQueryHelper.ConstructAppDetailsQueryHelper));
            queryTypeToHelper.Add("AppGenres", new ConstructQueryHelper(AppGenresQueryHelper.ConstructAppGenresQueryHelper));
            queryTypeToHelper.Add("Reviews", new ConstructQueryHelper(ReviewsQueryHelper.ConstructReviewsQueryHelper));
            queryTypeToHelper.Add("SubscriptionHistory", new ConstructQueryHelper(SubscriptionHistoryQueryHelper.ConstructSubscriptionHistoryQueryHelper));
            queryTypeToHelper.Add("PurchaseHistory", new ConstructQueryHelper(PurchaseHistoryQueryHelper.ConstructPurchaseHistoryQueryHelper));
        }

        internal static DataProviderQuery ConstructZuneServiceQuery(
          object queryTypeCookie)
        {
            return new ZuneServiceQuery(queryTypeCookie);
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
                    if (_queryTypeToHelper == null)
                    {
                        _queryTypeToHelper = new Dictionary<string, ConstructQueryHelper>(17);
                        RegisterZuneServiceQueryConstructors(_queryTypeToHelper);
                    }
                    string property = base.GetProperty("QueryType") as string;
                    this._helper = _queryTypeToHelper[property](this);
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
