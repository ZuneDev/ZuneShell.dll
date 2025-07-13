using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class QuickplayConfiguration : CConfigurationManagedBase
{
	public bool QuickplayInForeground
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("QuickplayInForeground", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("QuickplayInForeground", value);
		}
	}

	public bool ShowQuickMixDeletionIcon
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowQuickMixDeletionIcon", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowQuickMixDeletionIcon", value);
		}
	}

	public bool ShowFUE
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowFUE", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowFUE", value);
		}
	}

	public bool CheckUseCount
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("CheckUseCount", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("CheckUseCount", value);
		}
	}

	public int MaxUnusedCount
	{
		get
		{
			return GetIntProperty("MaxUnusedCount", 3);
		}
		set
		{
			SetIntProperty("MaxUnusedCount", value);
		}
	}

	public int UnusedCount
	{
		get
		{
			return GetIntProperty("UnusedCount", 0);
		}
		set
		{
			SetIntProperty("UnusedCount", value);
		}
	}

	public string FavoredExperience
	{
		get
		{
			return GetStringProperty("FavoredExperience", "");
		}
		set
		{
			SetStringProperty("FavoredExperience", value);
		}
	}

	internal QuickplayConfiguration(RegistryHive hive)
		: base(hive, null, "Quickplay")
	{
	}

	public QuickplayConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
