using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class HMEConfiguration : CConfigurationManagedBase
{
	public int LastUsedUID
	{
		get
		{
			return GetIntProperty("LastUsedUID", 0);
		}
		set
		{
			SetIntProperty("LastUsedUID", value);
		}
	}

	public int CurrentSharingUID
	{
		get
		{
			return GetIntProperty("CurrentSharingUID", 0);
		}
		set
		{
			SetIntProperty("CurrentSharingUID", value);
		}
	}

	public int StallServiceStartup
	{
		get
		{
			return GetIntProperty("StallServiceStartup", 0);
		}
		set
		{
			SetIntProperty("StallServiceStartup", value);
		}
	}

	public bool ResetDatabase
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ResetDatabase", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ResetDatabase", value);
		}
	}

	public bool TraceResults
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("TraceResults", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("TraceResults", value);
		}
	}

	public bool UsageTracking
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("UsageTracking", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("UsageTracking", value);
		}
	}

	public bool SetupCompleted
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("SetupCompleted", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("SetupCompleted", value);
		}
	}

	internal HMEConfiguration(RegistryHive hive)
		: base(hive, null, "HME")
	{
	}

	public HMEConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
