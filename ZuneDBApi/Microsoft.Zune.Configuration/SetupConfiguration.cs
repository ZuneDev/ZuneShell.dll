using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class SetupConfiguration : CConfigurationManagedBase
{
	public bool ReinitializeFirmwareTable
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ReinitializeFirmwareTable", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ReinitializeFirmwareTable", value);
		}
	}

	public int InstallPreReqTime
	{
		get
		{
			return GetIntProperty("InstallPreReqTime", 0);
		}
		set
		{
			SetIntProperty("InstallPreReqTime", value);
		}
	}

	public int InstallSearchTime
	{
		get
		{
			return GetIntProperty("InstallSearchTime", 0);
		}
		set
		{
			SetIntProperty("InstallSearchTime", value);
		}
	}

	public int InstallUserTime
	{
		get
		{
			return GetIntProperty("InstallUserTime", 0);
		}
		set
		{
			SetIntProperty("InstallUserTime", value);
		}
	}

	public int InstallZuneTime
	{
		get
		{
			return GetIntProperty("InstallZuneTime", 0);
		}
		set
		{
			SetIntProperty("InstallZuneTime", value);
		}
	}

	public int TotalInstallTime
	{
		get
		{
			return GetIntProperty("TotalInstallTime", 0);
		}
		set
		{
			SetIntProperty("TotalInstallTime", value);
		}
	}

	public string WTProfile
	{
		get
		{
			return GetStringProperty("WTProfile", "Installer.zune.setup");
		}
		set
		{
			SetStringProperty("WTProfile", value);
		}
	}

	public bool CodecInfoSent
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("CodecInfoSent", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("CodecInfoSent", value);
		}
	}

	public int ServicePack
	{
		get
		{
			return GetIntProperty("ServicePack", 2);
		}
		set
		{
			SetIntProperty("ServicePack", value);
		}
	}

	public int OSVersion
	{
		get
		{
			return GetIntProperty("OSVersion", 501);
		}
		set
		{
			SetIntProperty("OSVersion", value);
		}
	}

	public string OldVersion
	{
		get
		{
			return GetStringProperty("OldVersion", "");
		}
		set
		{
			SetStringProperty("OldVersion", value);
		}
	}

	public string InstallationSource
	{
		get
		{
			return GetStringProperty("InstallationSource", "");
		}
		set
		{
			SetStringProperty("InstallationSource", value);
		}
	}

	public int UpdateCheckFrequency
	{
		get
		{
			return GetIntProperty("UpdateCheckFrequency", 3);
		}
		set
		{
			SetIntProperty("UpdateCheckFrequency", value);
		}
	}

	internal SetupConfiguration(RegistryHive hive)
		: base(hive, null, "Setup")
	{
	}

	public SetupConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
