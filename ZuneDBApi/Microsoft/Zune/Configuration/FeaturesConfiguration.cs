using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class FeaturesConfiguration : CConfigurationManagedBase
	{
		public string VideoAllHub
		{
			get
			{
				return GetStringProperty("VideoAllHub", "CA,US");
			}
			set
			{
				SetStringProperty("VideoAllHub", value);
			}
		}

		public string Videos
		{
			get
			{
				return GetStringProperty("Videos", "AT,AU,BE,CA,CH,DE,DK,ES,FI,FR,GB,IE,IT,JP,MX,NL,NO,NZ,SE,US");
			}
			set
			{
				SetStringProperty("Videos", value);
			}
		}

		public string TV
		{
			get
			{
				return GetStringProperty("TV", "CA,US");
			}
			set
			{
				SetStringProperty("TV", value);
			}
		}

		public string SubscriptionMusicVideoStreaming
		{
			get
			{
				return GetStringProperty("SubscriptionMusicVideoStreaming", "US");
			}
			set
			{
				SetStringProperty("SubscriptionMusicVideoStreaming", value);
			}
		}

		public string SubscriptionMusicDownload
		{
			get
			{
				return GetStringProperty("SubscriptionMusicDownload", "US");
			}
			set
			{
				SetStringProperty("SubscriptionMusicDownload", value);
			}
		}

		public string SubscriptionTrial
		{
			get
			{
				return GetStringProperty("SubscriptionTrial", "ES,FR,GB,IT,US");
			}
			set
			{
				SetStringProperty("SubscriptionTrial", value);
			}
		}

		public string SubscriptionFreeTracks
		{
			get
			{
				return GetStringProperty("SubscriptionFreeTracks", "US");
			}
			set
			{
				SetStringProperty("SubscriptionFreeTracks", value);
			}
		}

		public string SubscriptionConfirmation
		{
			get
			{
				return GetStringProperty("SubscriptionConfirmation", "US");
			}
			set
			{
				SetStringProperty("SubscriptionConfirmation", value);
			}
		}

		public string Subscription
		{
			get
			{
				return GetStringProperty("Subscription", "ES,FR,GB,IT,US");
			}
			set
			{
				SetStringProperty("Subscription", value);
			}
		}

		public string SocialMarketplace
		{
			get
			{
				return GetStringProperty("SocialMarketplace", "CA,DE,ES,FR,GB,IT,US");
			}
			set
			{
				SetStringProperty("SocialMarketplace", value);
			}
		}

		public string Social
		{
			get
			{
				return GetStringProperty("Social", "CA,DE,ES,FR,GB,IT,US");
			}
			set
			{
				SetStringProperty("Social", value);
			}
		}

		public string SignInAvailable
		{
			get
			{
				return GetStringProperty("SignInAvailable", "AT,AU,BE,BR,CA,CH,CL,CO,CZ,DE,DK,ES,FI,FR,GB,GR,HK,HU,IE,IT,JP,MX,NL,NO,NZ,PL,PT,RU,SE,SG,TW,US,ZA");
			}
			set
			{
				SetStringProperty("SignInAvailable", value);
			}
		}

		public string Radio
		{
			get
			{
				return GetStringProperty("Radio", "all");
			}
			set
			{
				SetStringProperty("Radio", value);
			}
		}

		public string Quickplay
		{
			get
			{
				return GetStringProperty("Quickplay", "all");
			}
			set
			{
				SetStringProperty("Quickplay", value);
			}
		}

		public string QuickMixZmp
		{
			get
			{
				return GetStringProperty("QuickMixZmp", "ES,FR,GB,IT,US");
			}
			set
			{
				SetStringProperty("QuickMixZmp", value);
			}
		}

		public string QuickMixLocal
		{
			get
			{
				return GetStringProperty("QuickMixLocal", "ES,FR,GB,IT,US");
			}
			set
			{
				SetStringProperty("QuickMixLocal", value);
			}
		}

		public string Podcasts
		{
			get
			{
				return GetStringProperty("Podcasts", "US");
			}
			set
			{
				SetStringProperty("Podcasts", value);
			}
		}

		public string Picks
		{
			get
			{
				return GetStringProperty("Picks", "US");
			}
			set
			{
				SetStringProperty("Picks", value);
			}
		}

		public string OptIn
		{
			get
			{
				return GetStringProperty("OptIn", "CA,US");
			}
			set
			{
				SetStringProperty("OptIn", value);
			}
		}

		public string MusicVideos
		{
			get
			{
				return GetStringProperty("MusicVideos", "DE,ES,FR,GB,IT,US");
			}
			set
			{
				SetStringProperty("MusicVideos", value);
			}
		}

		public string Music
		{
			get
			{
				return GetStringProperty("Music", "DE,ES,FR,GB,IT,US");
			}
			set
			{
				SetStringProperty("Music", value);
			}
		}

		public string MovieTrailers
		{
			get
			{
				return GetStringProperty("MovieTrailers", "US");
			}
			set
			{
				SetStringProperty("MovieTrailers", value);
			}
		}

		public string Mixview
		{
			get
			{
				return GetStringProperty("Mixview", "US");
			}
			set
			{
				SetStringProperty("Mixview", value);
			}
		}

		public string Marketplace
		{
			get
			{
				return GetStringProperty("Marketplace", "AT,AU,BE,BR,CA,CH,CL,CO,CZ,DE,DK,ES,FI,FR,GB,GR,HK,HU,IE,IT,JP,MX,NL,NO,NZ,PL,PT,RU,SE,SG,TW,US,ZA");
			}
			set
			{
				SetStringProperty("Marketplace", value);
			}
		}

		public string MBRRental
		{
			get
			{
				return GetStringProperty("MBRRental", "all");
			}
			set
			{
				SetStringProperty("MBRRental", value);
			}
		}

		public string MBRPurchase
		{
			get
			{
				return GetStringProperty("MBRPurchase", "all");
			}
			set
			{
				SetStringProperty("MBRPurchase", value);
			}
		}

		public string MBRPreview
		{
			get
			{
				return GetStringProperty("MBRPreview", "all");
			}
			set
			{
				SetStringProperty("MBRPreview", value);
			}
		}

		public string Games
		{
			get
			{
				return GetStringProperty("Games", "US");
			}
			set
			{
				SetStringProperty("Games", value);
			}
		}

		public string FirstLaunchIntroVideo
		{
			get
			{
				return GetStringProperty("FirstLaunchIntroVideo", "all");
			}
			set
			{
				SetStringProperty("FirstLaunchIntroVideo", value);
			}
		}

		public string Device
		{
			get
			{
				return GetStringProperty("Device", "all");
			}
			set
			{
				SetStringProperty("Device", value);
			}
		}

		public string Channels
		{
			get
			{
				return GetStringProperty("Channels", "ES,FR,GB,IT,US");
			}
			set
			{
				SetStringProperty("Channels", value);
			}
		}

		public string Apps
		{
			get
			{
				return GetStringProperty("Apps", "AT,AU,BE,BR,CA,CH,CL,CO,CZ,DE,DK,ES,FI,FR,GB,GR,HK,HU,IE,IT,JP,MX,NL,NO,NZ,PL,PT,RU,SE,SG,TW,US,ZA");
			}
			set
			{
				SetStringProperty("Apps", value);
			}
		}

		internal FeaturesConfiguration(RegistryHive hive)
			: base(hive, null, "Features")
		{
		}

		public FeaturesConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
