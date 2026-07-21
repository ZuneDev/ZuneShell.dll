using System;
using Microsoft.Iris;

namespace Microsoft.Zune.Subscription;

// Original populates this list by async native RSS-feed retrieval into native
// IMSMediaSchemaPropertySet* items. Not reverse engineered — see
// logs/Microsoft.Zune/Subscription/SubscriptionManager.md. The list is always empty
// since there's no native feed engine to populate it.
public class VirtualSubscriptionEpisodeList : VirtualList
{
    internal string FeedUrl { get; }

    internal VirtualSubscriptionEpisodeList(string feedUrl)
    {
        FeedUrl = feedUrl;
    }

    internal void AsyncRetrieveEpisodeList(SubscriptionSeriesInfo seriesInfo)
    {
    }

    internal void Sort(string sortProperty)
    {
    }

    protected override object OnRequestItem(int index)
    {
        throw new IndexOutOfRangeException(nameof(index));
    }
}
