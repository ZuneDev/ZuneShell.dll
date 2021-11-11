using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class FirmwareUpdateConfiguration : CConfigurationManagedBase
	{
		public bool IgnorePhoneSyncErrors
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("IgnorePhoneSyncErrors", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("IgnorePhoneSyncErrors", value);
			}
		}

		public IList<string> PendingFirmwareUpdates
		{
			get
			{
				return GetStringListProperty("PendingFirmwareUpdates");
			}
			set
			{
				SetStringListProperty("PendingFirmwareUpdates", value);
			}
		}

		public int CancelFirmwareTimeout
		{
			get
			{
				return GetIntProperty("CancelFirmwareTimeout", 600000);
			}
			set
			{
				SetIntProperty("CancelFirmwareTimeout", value);
			}
		}

		public int CommitDelay
		{
			get
			{
				return GetIntProperty("CommitDelay", 1000);
			}
			set
			{
				SetIntProperty("CommitDelay", value);
			}
		}

		public int FirmwareUpdateRequestFrequency
		{
			get
			{
				return GetIntProperty("FirmwareUpdateRequestFrequency", 259200);
			}
			set
			{
				SetIntProperty("FirmwareUpdateRequestFrequency", value);
			}
		}

		public int FirmwareServerRequestTimeout
		{
			get
			{
				return GetIntProperty("FirmwareServerRequestTimeout", 45000);
			}
			set
			{
				SetIntProperty("FirmwareServerRequestTimeout", value);
			}
		}

		public int FirmwareUpdateConnectionThreshold
		{
			get
			{
				return GetIntProperty("FirmwareUpdateConnectionThreshold", 2592000);
			}
			set
			{
				SetIntProperty("FirmwareUpdateConnectionThreshold", value);
			}
		}

		public bool UseLocalFirmwareUpdate
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("UseLocalFirmwareUpdate", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("UseLocalFirmwareUpdate", value);
			}
		}

		public string FirmwareManagerVersion
		{
			get
			{
				return GetStringProperty("FirmwareManagerVersion", "");
			}
			set
			{
				SetStringProperty("FirmwareManagerVersion", value);
			}
		}

		internal FirmwareUpdateConfiguration(RegistryHive hive)
			: base(hive, null, "FirmwareUpdate")
		{
		}

		public FirmwareUpdateConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
