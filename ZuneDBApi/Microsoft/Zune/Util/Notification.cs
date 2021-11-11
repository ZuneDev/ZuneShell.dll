using System;
using Microsoft.Zune.Configuration;

namespace Microsoft.Zune.Util
{
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
				_003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x6d314898_002ENotifyInstantMessenger(AlbumName, ArtistName, TrackTitle, TrackNumber, ZuneMediaId);
				break;
			case EMediaTypes.eMediaTypeInvalid:
				break;
			}
		}

		public static void ResetNowPlaying()
		{
			if (ClientConfiguration.Playback.NotifyIMClient)
			{
				_003CModule_003E.Microsoft_002EZune_002EUtil_002E_003FA0x6d314898_002EResetInstantMessenger();
			}
		}
	}
}
