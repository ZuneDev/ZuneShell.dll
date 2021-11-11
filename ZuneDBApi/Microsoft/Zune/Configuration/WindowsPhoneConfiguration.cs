using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class WindowsPhoneConfiguration : CConfigurationManagedBase
	{
		public bool DTPTAllowed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("DTPTAllowed", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("DTPTAllowed", value);
			}
		}

		public bool IPAllowed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("IPAllowed", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("IPAllowed", value);
			}
		}

		internal WindowsPhoneConfiguration(RegistryHive hive)
			: base(hive, null, "WindowsPhone")
		{
		}

		public WindowsPhoneConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
