using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class HMESQMConfiguration : CConfigurationManagedBase
	{
		public int BrowseCount
		{
			get
			{
				return GetIntProperty("BrowseCount", 0);
			}
			set
			{
				SetIntProperty("BrowseCount", value);
			}
		}

		public int SearchCount
		{
			get
			{
				return GetIntProperty("SearchCount", 0);
			}
			set
			{
				SetIntProperty("SearchCount", value);
			}
		}

		public int NumberOfUsersSharing
		{
			get
			{
				return GetIntProperty("NumberOfUsersSharing", 0);
			}
			set
			{
				SetIntProperty("NumberOfUsersSharing", value);
			}
		}

		public bool SharingEnabled
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("SharingEnabled", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("SharingEnabled", value);
			}
		}

		internal HMESQMConfiguration(RegistryHive hive)
			: base(hive, null, "HMESQM")
		{
		}

		public HMESQMConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
