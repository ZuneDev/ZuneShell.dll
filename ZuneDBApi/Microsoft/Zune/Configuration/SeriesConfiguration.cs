using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class SeriesConfiguration : CConfigurationManagedBase
	{
		public int PodcastLastSelectedSeriesIndex
		{
			get
			{
				return GetIntProperty("PodcastLastSelectedSeriesIndex", 0);
			}
			set
			{
				SetIntProperty("PodcastLastSelectedSeriesIndex", value);
			}
		}

		public int PodcastDefaultUnsubscribeChoice
		{
			get
			{
				return GetIntProperty("PodcastDefaultUnsubscribeChoice", 0);
			}
			set
			{
				SetIntProperty("PodcastDefaultUnsubscribeChoice", value);
			}
		}

		public int PodcastDefaultPlaybackOrder
		{
			get
			{
				return GetIntProperty("PodcastDefaultPlaybackOrder", 0);
			}
			set
			{
				SetIntProperty("PodcastDefaultPlaybackOrder", value);
			}
		}

		public int PodcastDefaultKeepEpisodes
		{
			get
			{
				return GetIntProperty("PodcastDefaultKeepEpisodes", 3);
			}
			set
			{
				SetIntProperty("PodcastDefaultKeepEpisodes", value);
			}
		}

		internal SeriesConfiguration(RegistryHive hive)
			: base(hive, null, "Series")
		{
		}

		public SeriesConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
