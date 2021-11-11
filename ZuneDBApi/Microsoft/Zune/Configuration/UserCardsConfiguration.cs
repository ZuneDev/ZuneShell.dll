using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class UserCardsConfiguration : CConfigurationManagedBase
	{
		public bool CleanupCompleted
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("CleanupCompleted", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("CleanupCompleted", value);
			}
		}

		public long EstimatedUserCardSizeInMB
		{
			get
			{
				return GetInt64Property("EstimatedUserCardSizeInMB", 90L);
			}
			set
			{
				SetInt64Property("EstimatedUserCardSizeInMB", value);
			}
		}

		public string FriendsFeedUrl
		{
			get
			{
				return GetStringProperty("FriendsFeedUrl", "");
			}
			set
			{
				SetStringProperty("FriendsFeedUrl", value);
			}
		}

		internal UserCardsConfiguration(RegistryHive hive)
			: base(hive, null, "UserCards")
		{
		}

		public UserCardsConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
