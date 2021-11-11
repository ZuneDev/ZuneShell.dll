using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class SubscriptionConfiguration : CConfigurationManagedBase
	{
		public bool DisableAutomaticManagement
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("DisableAutomaticManagement", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("DisableAutomaticManagement", value);
			}
		}

		public int RefreshInterval
		{
			get
			{
				return GetIntProperty("RefreshInterval", 180);
			}
			set
			{
				SetIntProperty("RefreshInterval", value);
			}
		}

		public int MaximumValueLength
		{
			get
			{
				return GetIntProperty("MaximumValueLength", 65536);
			}
			set
			{
				SetIntProperty("MaximumValueLength", value);
			}
		}

		public int MaximumDocumentNestingLevels
		{
			get
			{
				return GetIntProperty("MaximumDocumentNestingLevels", 15);
			}
			set
			{
				SetIntProperty("MaximumDocumentNestingLevels", value);
			}
		}

		public int MaximumFeedItems
		{
			get
			{
				return GetIntProperty("MaximumFeedItems", 512);
			}
			set
			{
				SetIntProperty("MaximumFeedItems", value);
			}
		}

		public int MaximumFeedSizeKBytes
		{
			get
			{
				return GetIntProperty("MaximumFeedSizeKBytes", 20480);
			}
			set
			{
				SetIntProperty("MaximumFeedSizeKBytes", value);
			}
		}

		internal SubscriptionConfiguration(RegistryHive hive)
			: base(hive, null, "Subscription")
		{
		}

		public SubscriptionConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
