using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class PicksConfiguration : CConfigurationManagedBase
	{
		public string UsersWhoHaveUploadedArtists
		{
			get
			{
				return GetStringProperty("UsersWhoHaveUploadedArtists", "");
			}
			set
			{
				SetStringProperty("UsersWhoHaveUploadedArtists", value);
			}
		}

		public bool ShowPlayAndRate
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ShowPlayAndRate", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ShowPlayAndRate", value);
			}
		}

		public int CacheRefreshMinutes
		{
			get
			{
				return GetIntProperty("CacheRefreshMinutes", 60);
			}
			set
			{
				SetIntProperty("CacheRefreshMinutes", value);
			}
		}

		internal PicksConfiguration(RegistryHive hive)
			: base(hive, null, "Picks")
		{
		}

		public PicksConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
