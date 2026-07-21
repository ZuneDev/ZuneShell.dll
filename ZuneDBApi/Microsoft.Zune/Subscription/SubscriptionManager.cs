using System;
using System.Collections;

namespace Microsoft.Zune.Subscription;

// Original wraps a native ISubscriptionManager* singleton. Not reverse engineered — see
// logs/Microsoft.Zune/Subscription/SubscriptionManager.md.
public class SubscriptionManager : IDisposable
{
    private static SubscriptionManager sm_instance;

    public static SubscriptionManager Instance => sm_instance ??= new SubscriptionManager();

    public event SubscriptionEventHandler OnForegroundSubscriptionChanged;

    private SubscriptionManager()
    {
    }

    public int Subscribe(int subscriptionMediaId, EMediaTypes eSubscriptionMediaType) => unchecked((int)0x80004005);

    public int Subscribe(string feedUrl, string subscriptionTitle, Guid serviceId, bool isPersonalChannel, EMediaTypes eSubscriptionMediaType, ESubscriptionSource subscriptionSource, out int subscriptionMediaId)
    {
        subscriptionMediaId = 0;
        return unchecked((int)0x80004005);
    }

    public int Unsubscribe(int subscriptionMediaId, EMediaTypes eSubscriptionMediaType, bool deleteContent) => unchecked((int)0x80004005);

    public int Refresh(int subscriptionMediaId, EMediaTypes eSubscriptionMediaType, bool refreshCache) => unchecked((int)0x80004005);

    public int SetSeriesUrl(int subscriptionMediaId, string feedUrl) => unchecked((int)0x80004005);

    public bool FindByUrl(string feedUrl, EMediaTypes eSubscriptionMediaType, out int subscriptionMediaId, out bool isSubscribed)
    {
        subscriptionMediaId = 0;
        isSubscribed = false;
        return false;
    }

    public bool FindByServiceId(Guid serviceId, EMediaTypes eSubscriptionMediaType, out int subscriptionMediaId, out bool isSubscribed)
    {
        subscriptionMediaId = 0;
        isSubscribed = false;
        return false;
    }

    public int SetCredentialHandler(EMediaTypes eSubscriptionMediaType, SubscriptionCredentialHandler credentialHandler) => unchecked((int)0x80004005);

    public int SetManagementSettings(int subscriptionMediaId, uint keepEpisodes, ESeriesPlaybackOrder playbackOrder) => unchecked((int)0x80004005);

    public int GetManagementSettings(int subscriptionMediaId, out uint keepEpisodes, out ESeriesPlaybackOrder playbackOrder)
    {
        keepEpisodes = 0;
        playbackOrder = ESeriesPlaybackOrder.eSeriesPlaybackOrderNewestFirst;
        return unchecked((int)0x80004005);
    }

    public int DownloadEpisode(int subscriptionMediaId, int subscriptionItemMediaId) => unchecked((int)0x80004005);

    public int DeleteEpisode(int subscriptionMediaId, int subscriptionItemMediaId) => unchecked((int)0x80004005);

    public int SaveEpisodeToCollection(int subscriptionMediaId, int subscriptionItemMediaId) => unchecked((int)0x80004005);

    public int SetEpisodeSeriesUrl(IList subscriptionItemMediaIdList, string feedUrl) => unchecked((int)0x80004005);

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
