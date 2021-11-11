using System;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class ShipAssertsConfiguration : CConfigurationManagedBase
	{
		public int ConfigSerialNumber
		{
			get
			{
				return GetIntProperty("ConfigSerialNumber", 0);
			}
			set
			{
				SetIntProperty("ConfigSerialNumber", value);
			}
		}

		public int DefaultExpirationDays
		{
			get
			{
				return GetIntProperty("DefaultExpirationDays", 14);
			}
			set
			{
				SetIntProperty("DefaultExpirationDays", value);
			}
		}

		public int CaptureDumpTimeOutSec
		{
			get
			{
				return GetIntProperty("CaptureDumpTimeOutSec", 120);
			}
			set
			{
				SetIntProperty("CaptureDumpTimeOutSec", value);
			}
		}

		public int UpdateFrequency
		{
			get
			{
				return GetIntProperty("UpdateFrequency", 1440);
			}
			set
			{
				SetIntProperty("UpdateFrequency", value);
			}
		}

		public DateTime LastConfigDownload
		{
			get
			{
				DateTime defaultValue = new DateTime(1999, 1, 1);
				return GetDateTimeProperty("LastConfigDownload", defaultValue);
			}
			set
			{
				SetDateTimeProperty("LastConfigDownload", value);
			}
		}

		internal ShipAssertsConfiguration(RegistryHive hive)
			: base(hive, null, "ShipAsserts")
		{
		}

		public ShipAssertsConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
