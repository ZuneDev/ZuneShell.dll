using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class ClientConfiguration
	{
		private static CDBurnConfiguration m_CDBurn = new CDBurnConfiguration(RegistryHive.CurrentUser);

		private static DevicesConfiguration m_Devices = new DevicesConfiguration(RegistryHive.CurrentUser);

		private static DRMConfiguration m_DRM = new DRMConfiguration(RegistryHive.CurrentUser);

		private static FeaturesConfiguration m_Features = new FeaturesConfiguration(RegistryHive.CurrentUser);

		private static FeaturesOverrideConfiguration m_FeaturesOverride = new FeaturesOverrideConfiguration(RegistryHive.CurrentUser);

		private static FirmwareUpdateConfiguration m_FirmwareUpdate = new FirmwareUpdateConfiguration(RegistryHive.CurrentUser);

		private static FUEConfiguration m_FUE = new FUEConfiguration(RegistryHive.CurrentUser);

		private static GeneralSettingsConfiguration m_GeneralSettings = new GeneralSettingsConfiguration(RegistryHive.CurrentUser);

		private static GrovelerConfiguration m_Groveler = new GrovelerConfiguration(RegistryHive.CurrentUser);

		private static MBRConfiguration m_MBR = new MBRConfiguration(RegistryHive.CurrentUser);

		private static MediaStoreConfiguration m_MediaStore = new MediaStoreConfiguration(RegistryHive.CurrentUser);

		private static MessagingConfiguration m_Messaging = new MessagingConfiguration(RegistryHive.CurrentUser);

		private static NSSConfiguration m_NSS = new NSSConfiguration(RegistryHive.CurrentUser);

		private static PicksConfiguration m_Picks = new PicksConfiguration(RegistryHive.CurrentUser);

		private static PicturesConfiguration m_Pictures = new PicturesConfiguration(RegistryHive.CurrentUser);

		private static PlaybackConfiguration m_Playback = new PlaybackConfiguration(RegistryHive.CurrentUser);

		private static QuickMixConfiguration m_QuickMix = new QuickMixConfiguration(RegistryHive.CurrentUser);

		private static QuickplayConfiguration m_Quickplay = new QuickplayConfiguration(RegistryHive.CurrentUser);

		private static RecorderConfiguration m_Recorder = new RecorderConfiguration(RegistryHive.CurrentUser);

		private static SeriesConfiguration m_Series = new SeriesConfiguration(RegistryHive.CurrentUser);

		private static ChannelsConfiguration m_Channels = new ChannelsConfiguration(RegistryHive.CurrentUser);

		private static ServiceConfiguration m_Service = new ServiceConfiguration(RegistryHive.CurrentUser);

		private static ShellConfiguration m_Shell = new ShellConfiguration(RegistryHive.CurrentUser);

		private static ShipAssertsConfiguration m_ShipAsserts = new ShipAssertsConfiguration(RegistryHive.CurrentUser);

		private static SocialConfiguration m_Social = new SocialConfiguration(RegistryHive.CurrentUser);

		private static SQMConfiguration m_SQM = new SQMConfiguration(RegistryHive.CurrentUser);

		private static SubscriptionConfiguration m_Subscription = new SubscriptionConfiguration(RegistryHive.CurrentUser);

		private static TranscodeConfiguration m_Transcode = new TranscodeConfiguration(RegistryHive.CurrentUser);

		private static UnitTestConfiguration m_UnitTest = new UnitTestConfiguration(RegistryHive.CurrentUser);

		private static UsageDataConfiguration m_UsageData = new UsageDataConfiguration(RegistryHive.CurrentUser);

		private static UserCardsConfiguration m_UserCards = new UserCardsConfiguration(RegistryHive.CurrentUser);

		public static UserCardsConfiguration UserCards => m_UserCards;

		public static UsageDataConfiguration UsageData => m_UsageData;

		public static UnitTestConfiguration UnitTest => m_UnitTest;

		public static TranscodeConfiguration Transcode => m_Transcode;

		public static SubscriptionConfiguration Subscription => m_Subscription;

		public static SQMConfiguration SQM => m_SQM;

		public static SocialConfiguration Social => m_Social;

		public static ShipAssertsConfiguration ShipAsserts => m_ShipAsserts;

		public static ShellConfiguration Shell => m_Shell;

		public static ServiceConfiguration Service => m_Service;

		public static ChannelsConfiguration Channels => m_Channels;

		public static SeriesConfiguration Series => m_Series;

		public static RecorderConfiguration Recorder => m_Recorder;

		public static QuickplayConfiguration Quickplay => m_Quickplay;

		public static QuickMixConfiguration QuickMix => m_QuickMix;

		public static PlaybackConfiguration Playback => m_Playback;

		public static PicturesConfiguration Pictures => m_Pictures;

		public static PicksConfiguration Picks => m_Picks;

		public static NSSConfiguration NSS => m_NSS;

		public static MessagingConfiguration Messaging => m_Messaging;

		public static MediaStoreConfiguration MediaStore => m_MediaStore;

		public static MBRConfiguration MBR => m_MBR;

		public static GrovelerConfiguration Groveler => m_Groveler;

		public static GeneralSettingsConfiguration GeneralSettings => m_GeneralSettings;

		public static FUEConfiguration FUE => m_FUE;

		public static FirmwareUpdateConfiguration FirmwareUpdate => m_FirmwareUpdate;

		public static FeaturesOverrideConfiguration FeaturesOverride => m_FeaturesOverride;

		public static FeaturesConfiguration Features => m_Features;

		public static DRMConfiguration DRM => m_DRM;

		public static DevicesConfiguration Devices => m_Devices;

		public static CDBurnConfiguration CDBurn => m_CDBurn;
	}
}
