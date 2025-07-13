using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class SocialConfiguration : CConfigurationManagedBase
{
	public bool ConfirmDeleteFriend
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ConfirmDeleteFriend", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ConfirmDeleteFriend", value);
		}
	}

	public bool ConfirmAcceptFriend
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ConfirmAcceptFriend", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ConfirmAcceptFriend", value);
		}
	}

	internal SocialConfiguration(RegistryHive hive)
		: base(hive, null, "Social")
	{
	}

	public SocialConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
