// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.PerfTrace.UICollectionEvent
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace Microsoft.Zune.PerfTrace
{
    public enum UICollectionEvent
    {
        LibraryQueryIssued = 1,
        LibraryQueryResultsAvailable = 2,
        LibraryQueryResultsVisible = 3,
        MusicLibraryViewCommandInvoked = 4,
        DeviceIconItemDropped = 5,
        SearchQueryIssued = 6,
        SearchQueryResultsAvailable = 7,
        SearchQueryResultsVisible = 8,
        MarketplaceQueriesIssued = 9,
        MarketplaceQueriesResultsAvailable = 10, // 0x0000000A
        MarketplaceQueriesResultsVisible = 11, // 0x0000000B
        PlayRequestIssued = 12, // 0x0000000C
        PlayRequestComplete = 13, // 0x0000000D
        BulkEventsPushBegin = 14, // 0x0000000E
        BulkEventsPushEnd = 15, // 0x0000000F
        RecommendationQueriesIssued = 16, // 0x00000010
        RecommendationQueriesResultsAvailable = 17, // 0x00000011
        RecommendationQueriesResultsVisible = 18, // 0x00000012
        SocialQueriesIssued = 19, // 0x00000013
        SocialQueriesResultsAvailable = 20, // 0x00000014
        SocialQueriesResultsVisible = 21, // 0x00000015
        MixQueriesIssued = 22, // 0x00000016
        MixQueriesResultsAvailable = 23, // 0x00000017
        MixQueriesResultsVisible = 24, // 0x00000018
        QuickplayQueriesIssued = 25, // 0x00000019
        QuickplayQueriesResultsAvailable = 26, // 0x0000001A
        QuickplayQueriesResultsVisible = 27, // 0x0000001B
        DataProviderQueryBegin = 28, // 0x0000001C
        DataProviderQueryComplete = 29, // 0x0000001D
        QuickMixBegin = 30, // 0x0000001E
        QuickMixUpdate = 31, // 0x0000001F
        QuickMixComplete = 32, // 0x00000020
        MarketplaceSearchResultArtistClicked = 33, // 0x00000021
        SearchHintCollectionQueryArtistBegin = 34, // 0x00000022
        SearchHintCollectionQueryArtistComplete = 35, // 0x00000023
        SearchHintMarketplaceQueryPrefixBegin = 36, // 0x00000024
        SearchHintMarketplaceQueryPrefixComplete = 37, // 0x00000025
        SyncStatusVisible = 38, // 0x00000026
    }
}
