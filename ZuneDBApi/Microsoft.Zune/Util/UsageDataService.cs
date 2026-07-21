using System;

namespace Microsoft.Zune.Util;

// Original reports playback/download telemetry to a native usage-data service. Not
// reverse engineered — see logs/Microsoft.Zune/Util/DownloadManager.md.
public class UsageDataService
{
    public static bool GetPostUsageDataFlagForSignedInUser() => false;

    public static void SetPostUsageDataFlagForSignedInUser(bool fCanPostUsageData)
    {
    }

    public static void ReportTrackSubscriptionPlayback(Guid guidTrackId, string strReferrer)
    {
    }

    public static void ReportTrackPreviewPlayback(Guid guidTrackId, string strReferrer)
    {
    }

    public static void ReportTrackSubscriptionSkipPlay(Guid guidTrackId, string strReferrer)
    {
    }

    public static void ReportTrackPreviewSkipPlay(Guid guidTrackId, string strReferrer)
    {
    }

    public static void ReportTrackAddToCollection(Guid guidMediaId, string strReferrer)
    {
    }

    public static void ReportPlaylistDownload(bool purchase, Guid guidMediaId)
    {
    }
}
