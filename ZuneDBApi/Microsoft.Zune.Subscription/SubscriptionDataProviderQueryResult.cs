using System;
using Microsoft.Iris;

namespace Microsoft.Zune.Subscription;

public class SubscriptionDataProviderQueryResult : DataProviderObject
{
    private SubscriptionSeriesInfo m_seriesInfo;

    private VirtualSubscriptionEpisodeList m_episodeList;

    public SubscriptionDataProviderQueryResult(DataProviderQuery owner, object typeCookie, string feedUrl, string serviceId, string sort)
        : base(owner, typeCookie)
    {
        m_episodeList = new VirtualSubscriptionEpisodeList(owner, base.Mappings["Items"].UnderlyingCollectionTypeCookie, feedUrl, sort);
        m_seriesInfo = new SubscriptionSeriesInfo(owner, base.Mappings["PodcastSeriesInfo"].PropertyTypeCookie, serviceId);
        if (!string.IsNullOrEmpty(feedUrl))
        {
            m_episodeList.AsyncRetrieveEpisodeList(m_seriesInfo);
        }
    }

    public void OnDispose()
    {
        ((IDisposable)m_episodeList)?.Dispose();
        m_episodeList = null;
        SubscriptionSeriesInfo seriesInfo = m_seriesInfo;
        if (seriesInfo != null)
        {
            seriesInfo.OnDispose();
            m_seriesInfo = null;
        }
    }

    public override object GetProperty(string propertyName)
    {
        if ("Items" == propertyName)
        {
            return m_episodeList;
        }
        if ("PodcastSeriesInfo" == propertyName)
        {
            return m_seriesInfo;
        }
        return null;
    }

    public override void SetProperty(string propertyName, object value)
    {
        throw new NotSupportedException();
    }

    internal unsafe static object ConvertVariantToType(string typeName, CComPropVariant* varValue)
    {
        switch (typeName)
        {
            case "Int32":
                return *(int*)&varValue->val1;

            case "Boolean":
                var b = (*(short*)&varValue->val1 == -1) ? 1 : 0;
                return b != 0;

            case "String":
                return new string((char*)varValue->val1);

            case "TimeSpan":
                return new TimeSpan(0, 0, 0, 0, *(int*)&varValue->val1);

            case "DateTime":
                return DateTime.FromOADate(*(double*)&varValue->val1);
        }

        return null;
    }
}
