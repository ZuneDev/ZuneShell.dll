using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class GeneralSettingsConfiguration : CConfigurationManagedBase
{
	public bool ShowDebugAssertDialogs
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowDebugAssertDialogs", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowDebugAssertDialogs", value);
		}
	}

	public bool DebugDRM
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("DebugDRM", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("DebugDRM", value);
		}
	}

	public int AccessMediaHangTimeoutSec
	{
		get
		{
			return GetIntProperty("AccessMediaHangTimeoutSec", 5);
		}
		set
		{
			SetIntProperty("AccessMediaHangTimeoutSec", value);
		}
	}

	public int BandwidthTestTimeoutSec
	{
		get
		{
			return GetIntProperty("BandwidthTestTimeoutSec", 20);
		}
		set
		{
			SetIntProperty("BandwidthTestTimeoutSec", value);
		}
	}

	public int SBEGraphPauseDelayMs
	{
		get
		{
			return GetIntProperty("SBEGraphPauseDelayMs", 100);
		}
		set
		{
			SetIntProperty("SBEGraphPauseDelayMs", value);
		}
	}

	public int VirusScanTimeOutSec
	{
		get
		{
			return GetIntProperty("VirusScanTimeOutSec", 30);
		}
		set
		{
			SetIntProperty("VirusScanTimeOutSec", value);
		}
	}

	public bool TaskbarInstalledWin7
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("TaskbarInstalledWin7", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("TaskbarInstalledWin7", value);
		}
	}

	public bool TaskbarInstalledVista
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("TaskbarInstalledVista", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("TaskbarInstalledVista", value);
		}
	}

	public bool TaskbarInstalledXP
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("TaskbarInstalledXP", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("TaskbarInstalledXP", value);
		}
	}

	public bool CompactModeAlwaysOnTop
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("CompactModeAlwaysOnTop", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("CompactModeAlwaysOnTop", value);
		}
	}

	public bool ShowTaskbarPlayerPrompt
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowTaskbarPlayerPrompt", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowTaskbarPlayerPrompt", value);
		}
	}

	public bool ProxyAuthenticationEnabled
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ProxyAuthenticationEnabled", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ProxyAuthenticationEnabled", value);
		}
	}

	public long MaxChunkDownloadSizeBytes
	{
		get
		{
			return GetInt64Property("MaxChunkDownloadSizeBytes", 20971520L);
		}
		set
		{
			SetInt64Property("MaxChunkDownloadSizeBytes", value);
		}
	}

	public bool ForceChunkDownloadForWMV
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ForceChunkDownloadForWMV", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ForceChunkDownloadForWMV", value);
		}
	}

	public bool ForceChunkDownload
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ForceChunkDownload", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ForceChunkDownload", value);
		}
	}

	public int MaxParallelDownloadFileSizeKB
	{
		get
		{
			return GetIntProperty("MaxParallelDownloadFileSizeKB", 30720);
		}
		set
		{
			SetIntProperty("MaxParallelDownloadFileSizeKB", value);
		}
	}

	public int MaxBackgroundSimultaneousDownloads
	{
		get
		{
			return GetIntProperty("MaxBackgroundSimultaneousDownloads", 5);
		}
		set
		{
			SetIntProperty("MaxBackgroundSimultaneousDownloads", value);
		}
	}

	public int MaxSimultaneousDownloads
	{
		get
		{
			return GetIntProperty("MaxSimultaneousDownloads", 2);
		}
		set
		{
			SetIntProperty("MaxSimultaneousDownloads", value);
		}
	}

	public int UNCTimeout
	{
		get
		{
			return GetIntProperty("UNCTimeout", 4000);
		}
		set
		{
			SetIntProperty("UNCTimeout", value);
		}
	}

	public int UNCRetryTimeout
	{
		get
		{
			return GetIntProperty("UNCRetryTimeout", 4000);
		}
		set
		{
			SetIntProperty("UNCRetryTimeout", value);
		}
	}

	public int SlideShowSpeed
	{
		get
		{
			return GetIntProperty("SlideShowSpeed", 5000);
		}
		set
		{
			SetIntProperty("SlideShowSpeed", value);
		}
	}

	public bool ReevaluateVideoSettings
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ReevaluateVideoSettings", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ReevaluateVideoSettings", value);
		}
	}

	public bool AnimationsEnabled
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("AnimationsEnabled", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("AnimationsEnabled", value);
		}
	}

	public int RenderingQuality
	{
		get
		{
			return GetIntProperty("RenderingQuality", 1);
		}
		set
		{
			SetIntProperty("RenderingQuality", value);
		}
	}

	public int RenderingType
	{
		get
		{
			return GetIntProperty("RenderingType", 2);
		}
		set
		{
			SetIntProperty("RenderingType", value);
		}
	}

	public bool UseGDI
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("UseGDI", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("UseGDI", value);
		}
	}

	public bool AVIEnabled
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("AVIEnabled", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("AVIEnabled", value);
		}
	}

	internal GeneralSettingsConfiguration(RegistryHive hive)
		: base(hive, null, "GeneralSettings")
	{
	}

	public GeneralSettingsConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
