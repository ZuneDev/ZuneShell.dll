using System;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class SocialUserGuidConfiguration : CConfigurationManagedBase
	{
		public DateTime ProfileCommentsLastRead
		{
			get
			{
				DateTime defaultValue = new DateTime(1999, 1, 1);
				return GetDateTimeProperty("ProfileCommentsLastRead", defaultValue);
			}
			set
			{
				SetDateTimeProperty("ProfileCommentsLastRead", value);
			}
		}

		public DateTime ProfilePlayCountUpdated
		{
			get
			{
				DateTime defaultValue = new DateTime(1999, 1, 1);
				return GetDateTimeProperty("ProfilePlayCountUpdated", defaultValue);
			}
			set
			{
				SetDateTimeProperty("ProfilePlayCountUpdated", value);
			}
		}

		public int ProfilePlayCount
		{
			get
			{
				return GetIntProperty("ProfilePlayCount", -1);
			}
			set
			{
				SetIntProperty("ProfilePlayCount", value);
			}
		}

		internal SocialUserGuidConfiguration(RegistryHive hive)
			: base(hive, null, "SocialUserGuid")
		{
		}

		public SocialUserGuidConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
