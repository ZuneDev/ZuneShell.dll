using System;
using Microsoft.Zune.Configuration;

namespace Microsoft.Zune.Util;

public class Notification
{
    public static void BroadcastNowPlaying(EMediaTypes MediaType, string AlbumName, string ArtistName, string TrackTitle, int TrackNumber, Guid ZuneMediaId)
    {
        if (!ClientConfiguration.Playback.NotifyIMClient)
        {
            return;
        }
        switch (MediaType)
        {
            case EMediaTypes.eMediaTypePodcastEpisode:
                if (ClientConfiguration.Playback.NotifyIncludePodcasts)
                {
                    goto default;
                }
                break;
            case EMediaTypes.eMediaTypeVideo:
                if (!ClientConfiguration.Playback.NotifyIncludeVideos)
                {
                    break;
                }
                goto default;
            default:
                // Stub: Original called native code
                break;
            case EMediaTypes.eMediaTypeInvalid:
                break;
        }
    }

    public static void ResetNowPlaying()
    {
        if (ClientConfiguration.Playback.NotifyIMClient)
        {
            // Stub: Original called native code
        }
    }
}
