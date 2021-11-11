using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class FeaturesOverrideConfiguration : CConfigurationManagedBase
	{
		public int VideoAllHub
		{
			get
			{
				return GetIntProperty("VideoAllHub", 2);
			}
			set
			{
				SetIntProperty("VideoAllHub", value);
			}
		}

		public int SubscriptionConfirmation
		{
			get
			{
				return GetIntProperty("SubscriptionConfirmation", 2);
			}
			set
			{
				SetIntProperty("SubscriptionConfirmation", value);
			}
		}

		public int SocialMarketplace
		{
			get
			{
				return GetIntProperty("SocialMarketplace", 2);
			}
			set
			{
				SetIntProperty("SocialMarketplace", value);
			}
		}

		public int Mixview
		{
			get
			{
				return GetIntProperty("Mixview", 2);
			}
			set
			{
				SetIntProperty("Mixview", value);
			}
		}

		public int Music
		{
			get
			{
				return GetIntProperty("Music", 2);
			}
			set
			{
				SetIntProperty("Music", value);
			}
		}

		public int QuickMixZmp
		{
			get
			{
				return GetIntProperty("QuickMixZmp", 2);
			}
			set
			{
				SetIntProperty("QuickMixZmp", value);
			}
		}

		public int QuickMixLocal
		{
			get
			{
				return GetIntProperty("QuickMixLocal", 2);
			}
			set
			{
				SetIntProperty("QuickMixLocal", value);
			}
		}

		public int MBRPreview
		{
			get
			{
				return GetIntProperty("MBRPreview", 1);
			}
			set
			{
				SetIntProperty("MBRPreview", value);
			}
		}

		public int MBRPurchase
		{
			get
			{
				return GetIntProperty("MBRPurchase", 1);
			}
			set
			{
				SetIntProperty("MBRPurchase", value);
			}
		}

		public int MBRRental
		{
			get
			{
				return GetIntProperty("MBRRental", 1);
			}
			set
			{
				SetIntProperty("MBRRental", value);
			}
		}

		public int FirstLaunchIntroVideo
		{
			get
			{
				return GetIntProperty("FirstLaunchIntroVideo", 2);
			}
			set
			{
				SetIntProperty("FirstLaunchIntroVideo", value);
			}
		}

		public int OptIn
		{
			get
			{
				return GetIntProperty("OptIn", 2);
			}
			set
			{
				SetIntProperty("OptIn", value);
			}
		}

		public int SignInAvailable
		{
			get
			{
				return GetIntProperty("SignInAvailable", 2);
			}
			set
			{
				SetIntProperty("SignInAvailable", value);
			}
		}

		public int SubscriptionMusicVideoStreaming
		{
			get
			{
				return GetIntProperty("SubscriptionMusicVideoStreaming", 2);
			}
			set
			{
				SetIntProperty("SubscriptionMusicVideoStreaming", value);
			}
		}

		public int SubscriptionMusicDownload
		{
			get
			{
				return GetIntProperty("SubscriptionMusicDownload", 2);
			}
			set
			{
				SetIntProperty("SubscriptionMusicDownload", value);
			}
		}

		public int SubscriptionFreeTracks
		{
			get
			{
				return GetIntProperty("SubscriptionFreeTracks", 2);
			}
			set
			{
				SetIntProperty("SubscriptionFreeTracks", value);
			}
		}

		public int SubscriptionTrial
		{
			get
			{
				return GetIntProperty("SubscriptionTrial", 2);
			}
			set
			{
				SetIntProperty("SubscriptionTrial", value);
			}
		}

		public int Subscription
		{
			get
			{
				return GetIntProperty("Subscription", 2);
			}
			set
			{
				SetIntProperty("Subscription", value);
			}
		}

		public int MovieTrailers
		{
			get
			{
				return GetIntProperty("MovieTrailers", 2);
			}
			set
			{
				SetIntProperty("MovieTrailers", value);
			}
		}

		public int TV
		{
			get
			{
				return GetIntProperty("TV", 2);
			}
			set
			{
				SetIntProperty("TV", value);
			}
		}

		public int Apps
		{
			get
			{
				return GetIntProperty("Apps", 2);
			}
			set
			{
				SetIntProperty("Apps", value);
			}
		}

		public int Games
		{
			get
			{
				return GetIntProperty("Games", 2);
			}
			set
			{
				SetIntProperty("Games", value);
			}
		}

		public int Radio
		{
			get
			{
				return GetIntProperty("Radio", 0);
			}
			set
			{
				SetIntProperty("Radio", value);
			}
		}

		public int Channels
		{
			get
			{
				return GetIntProperty("Channels", 2);
			}
			set
			{
				SetIntProperty("Channels", value);
			}
		}

		public int Podcasts
		{
			get
			{
				return GetIntProperty("Podcasts", 2);
			}
			set
			{
				SetIntProperty("Podcasts", value);
			}
		}

		public int MusicVideos
		{
			get
			{
				return GetIntProperty("MusicVideos", 2);
			}
			set
			{
				SetIntProperty("MusicVideos", value);
			}
		}

		public int Social
		{
			get
			{
				return GetIntProperty("Social", 2);
			}
			set
			{
				SetIntProperty("Social", value);
			}
		}

		public int Videos
		{
			get
			{
				return GetIntProperty("Videos", 2);
			}
			set
			{
				SetIntProperty("Videos", value);
			}
		}

		public int Picks
		{
			get
			{
				return GetIntProperty("Picks", 2);
			}
			set
			{
				SetIntProperty("Picks", value);
			}
		}

		public int Marketplace
		{
			get
			{
				return GetIntProperty("Marketplace", 2);
			}
			set
			{
				SetIntProperty("Marketplace", value);
			}
		}

		public int Device
		{
			get
			{
				return GetIntProperty("Device", 2);
			}
			set
			{
				SetIntProperty("Device", value);
			}
		}

		public int Quickplay
		{
			get
			{
				return GetIntProperty("Quickplay", 2);
			}
			set
			{
				SetIntProperty("Quickplay", value);
			}
		}

		internal FeaturesOverrideConfiguration(RegistryHive hive)
			: base(hive, null, "FeaturesOverride")
		{
		}

		public FeaturesOverrideConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
