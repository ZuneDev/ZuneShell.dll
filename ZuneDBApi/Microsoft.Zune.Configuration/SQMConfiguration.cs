using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class SQMConfiguration : CConfigurationManagedBase
{
	public bool UsePredictableSQMFile
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("UsePredictableSQMFile", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("UsePredictableSQMFile", value);
		}
	}

	public bool WriteSQMLogFile
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("WriteSQMLogFile", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("WriteSQMLogFile", value);
		}
	}

	public int SQMLaunchIndex
	{
		get
		{
			return GetIntProperty("SQMLaunchIndex", 0);
		}
		set
		{
			SetIntProperty("SQMLaunchIndex", value);
		}
	}

	public bool SQMTest
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("SQMTest", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("SQMTest", value);
		}
	}

	public bool ForceUsageTracking
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ForceUsageTracking", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ForceUsageTracking", value);
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

	internal SQMConfiguration(RegistryHive hive)
		: base(hive, null, "SQM")
	{
	}

	public SQMConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
