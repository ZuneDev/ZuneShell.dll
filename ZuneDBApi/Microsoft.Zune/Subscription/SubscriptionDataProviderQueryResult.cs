namespace Microsoft.Zune.Subscription;

public class SubscriptionDataProviderQueryResult
{
    private SubscriptionSeriesInfo m_seriesInfo;
    private VirtualSubscriptionEpisodeList m_episodeList;

    internal SubscriptionDataProviderQueryResult(SubscriptionSeriesInfo seriesInfo, VirtualSubscriptionEpisodeList episodeList)
    {
        m_seriesInfo = seriesInfo;
        m_episodeList = episodeList;
    }

    public void OnDispose()
    {
    }

    public object GetProperty(string propertyName)
    {
        return propertyName switch
        {
            "SeriesInfo" => m_seriesInfo,
            "EpisodeList" => m_episodeList,
            _ => null,
        };
    }

    public void SetProperty(string propertyName, object value)
    {
    }
}
