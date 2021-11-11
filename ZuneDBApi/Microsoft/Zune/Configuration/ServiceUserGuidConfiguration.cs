using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class ServiceUserGuidConfiguration : CConfigurationManagedBase
	{
		public long SubscriptionId
		{
			get
			{
				return GetInt64Property("SubscriptionId", 0L);
			}
			set
			{
				SetInt64Property("SubscriptionId", value);
			}
		}

		public bool ActiveSubscription
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ActiveSubscription", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ActiveSubscription", value);
			}
		}

		public int Seed
		{
			get
			{
				return GetIntProperty("Seed", 0);
			}
			set
			{
				SetIntProperty("Seed", value);
			}
		}

		public DateTime RecommendationsRefreshDate
		{
			get
			{
				DateTime defaultValue = new DateTime(1999, 1, 1);
				return GetDateTimeProperty("RecommendationsRefreshDate", defaultValue);
			}
			set
			{
				SetDateTimeProperty("RecommendationsRefreshDate", value);
			}
		}

		public DateTime RecommendationsDate
		{
			get
			{
				DateTime defaultValue = new DateTime(1999, 1, 1);
				return GetDateTimeProperty("RecommendationsDate", defaultValue);
			}
			set
			{
				SetDateTimeProperty("RecommendationsDate", value);
			}
		}

		internal ServiceUserGuidConfiguration(RegistryHive hive)
			: base(hive, null, "ServiceUserGuid")
		{
		}

		public ServiceUserGuidConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
