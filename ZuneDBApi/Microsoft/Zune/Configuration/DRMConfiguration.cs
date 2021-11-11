using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class DRMConfiguration : CConfigurationManagedBase
	{
		public DateTime LastFullRefresh
		{
			get
			{
				DateTime defaultValue = new DateTime(1999, 1, 1);
				return GetDateTimeProperty("LastFullRefresh", defaultValue);
			}
			set
			{
				SetDateTimeProperty("LastFullRefresh", value);
			}
		}

		public int TimeBetweenRefreshSessionsMins
		{
			get
			{
				return GetIntProperty("TimeBetweenRefreshSessionsMins", 10080);
			}
			set
			{
				SetIntProperty("TimeBetweenRefreshSessionsMins", value);
			}
		}

		public int TimeBetweenRefreshms
		{
			get
			{
				return GetIntProperty("TimeBetweenRefreshms", 25);
			}
			set
			{
				SetIntProperty("TimeBetweenRefreshms", value);
			}
		}

		public int SyncLicenseMinCount
		{
			get
			{
				return GetIntProperty("SyncLicenseMinCount", 1);
			}
			set
			{
				SetIntProperty("SyncLicenseMinCount", value);
			}
		}

		public int SyncLicenseMinHours
		{
			get
			{
				return GetIntProperty("SyncLicenseMinHours", 384);
			}
			set
			{
				SetIntProperty("SyncLicenseMinHours", value);
			}
		}

		public bool SilentDRMConfiguration
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("SilentDRMConfiguration", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("SilentDRMConfiguration", value);
			}
		}

		internal DRMConfiguration(RegistryHive hive)
			: base(hive, null, "DRM")
		{
		}

		public DRMConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
