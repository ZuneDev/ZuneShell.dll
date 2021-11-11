using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration
{
	public class FUEConfiguration : CConfigurationManagedBase
	{
		public bool ShowPhoneFUEDeviceLandOptions
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ShowPhoneFUEDeviceLandOptions", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ShowPhoneFUEDeviceLandOptions", value);
			}
		}

		public bool ShowArtistChooser
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ShowArtistChooser", defaultValue: false);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ShowArtistChooser", value);
			}
		}

		public string FirstLaunchVideo
		{
			get
			{
				return GetStringProperty("FirstLaunchVideo", "");
			}
			set
			{
				SetStringProperty("FirstLaunchVideo", value);
			}
		}

		public double SettingsVersion
		{
			get
			{
				return GetDoubleProperty("SettingsVersion", 0.0);
			}
			set
			{
				SetDoubleProperty("SettingsVersion", value);
			}
		}

		public bool ShowFirstLaunchVideo
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("ShowFirstLaunchVideo", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("ShowFirstLaunchVideo", value);
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

		public bool AcceptedPrivacyStatement
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return GetBoolProperty("AcceptedPrivacyStatement", defaultValue: true);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				SetBoolProperty("AcceptedPrivacyStatement", value);
			}
		}

		internal FUEConfiguration(RegistryHive hive)
			: base(hive, null, "FUE")
		{
		}

		public FUEConfiguration(RegistryHive hive, string basePath, string instance)
			: base(hive, basePath, instance)
		{
		}
	}
}
