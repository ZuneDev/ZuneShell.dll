using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class ShellConfiguration : CConfigurationManagedBase
{
	public DateTime LastUpdateCheck
	{
		get
		{
			DateTime defaultValue = new DateTime(1999, 1, 1);
			return GetDateTimeProperty("LastUpdateCheck", defaultValue);
		}
		set
		{
			SetDateTimeProperty("LastUpdateCheck", value);
		}
	}

	public string LastMarketplaceCulture
	{
		get
		{
			return GetStringProperty("LastMarketplaceCulture", "");
		}
		set
		{
			SetStringProperty("LastMarketplaceCulture", value);
		}
	}

	public string LastClientCulture
	{
		get
		{
			return GetStringProperty("LastClientCulture", "");
		}
		set
		{
			SetStringProperty("LastClientCulture", value);
		}
	}

	public bool ShowAppsCollectionNotification
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowAppsCollectionNotification", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowAppsCollectionNotification", value);
		}
	}

	public bool ShowApplicationPivot
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowApplicationPivot", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowApplicationPivot", value);
		}
	}

	public int CompactModePivot
	{
		get
		{
			return GetIntProperty("CompactModePivot", 0);
		}
		set
		{
			SetIntProperty("CompactModePivot", value);
		}
	}

	public bool ShowSongInfoInWindowTitle
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowSongInfoInWindowTitle", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowSongInfoInWindowTitle", value);
		}
	}

	public bool ShowAppsForWindowsPhoneOnlyHeader
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowAppsForWindowsPhoneOnlyHeader", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowAppsForWindowsPhoneOnlyHeader", value);
		}
	}

	public bool ShowAppsForZuneHDOnlyHeader
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowAppsForZuneHDOnlyHeader", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowAppsForZuneHDOnlyHeader", value);
		}
	}

	public bool ShowContentTypes
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowContentTypes", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowContentTypes", value);
		}
	}

	public string StartupPage
	{
		get
		{
			return GetStringProperty("StartupPage", "Collection\\Default");
		}
		set
		{
			SetStringProperty("StartupPage", value);
		}
	}

	public bool MixFixedRandom
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("MixFixedRandom", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("MixFixedRandom", value);
		}
	}

	public int MixIdleTimeoutMS
	{
		get
		{
			return GetIntProperty("MixIdleTimeoutMS", 30000);
		}
		set
		{
			SetIntProperty("MixIdleTimeoutMS", value);
		}
	}

	public bool ShowWhatsNew
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowWhatsNew", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowWhatsNew", value);
		}
	}

	public int ShowNowPlayingBackgroundOnIdleTimeout
	{
		get
		{
			return GetIntProperty("ShowNowPlayingBackgroundOnIdleTimeout", 90);
		}
		set
		{
			SetIntProperty("ShowNowPlayingBackgroundOnIdleTimeout", value);
		}
	}

	public bool ShowPlayInfoInTaskbar
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowPlayInfoInTaskbar", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowPlayInfoInTaskbar", value);
		}
	}

	public long InitializationSequence
	{
		get
		{
			return GetInt64Property("InitializationSequence", 0L);
		}
		set
		{
			SetInt64Property("InitializationSequence", value);
		}
	}

	public bool Sounds
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("Sounds", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("Sounds", value);
		}
	}

	public int BackgroundColor
	{
		get
		{
			return GetIntProperty("BackgroundColor", 0);
		}
		set
		{
			SetIntProperty("BackgroundColor", value);
		}
	}

	public string BackgroundImage
	{
		get
		{
			return GetStringProperty("BackgroundImage", "");
		}
		set
		{
			SetStringProperty("BackgroundImage", value);
		}
	}

	public int VideoCollectionView
	{
		get
		{
			return GetIntProperty("VideoCollectionView", 0);
		}
		set
		{
			SetIntProperty("VideoCollectionView", value);
		}
	}

	public int MusicDeviceView
	{
		get
		{
			return GetIntProperty("MusicDeviceView", 0);
		}
		set
		{
			SetIntProperty("MusicDeviceView", value);
		}
	}

	public int MusicCollectionView
	{
		get
		{
			return GetIntProperty("MusicCollectionView", 0);
		}
		set
		{
			SetIntProperty("MusicCollectionView", value);
		}
	}

	internal ShellConfiguration(RegistryHive hive)
		: base(hive, null, "Shell")
	{
	}

	public ShellConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
