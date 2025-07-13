using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class PlaybackConfiguration : CConfigurationManagedBase
{
	public int FSDKCloseReaderThresholdMs
	{
		get
		{
			return GetIntProperty("FSDKCloseReaderThresholdMs", 10000);
		}
		set
		{
			SetIntProperty("FSDKCloseReaderThresholdMs", value);
		}
	}

	public bool ShowNowPlayingClipAnimations
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowNowPlayingClipAnimations", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowNowPlayingClipAnimations", value);
		}
	}

	public bool NotifyIncludePodcasts
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("NotifyIncludePodcasts", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("NotifyIncludePodcasts", value);
		}
	}

	public bool NotifyIncludeVideos
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("NotifyIncludeVideos", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("NotifyIncludeVideos", value);
		}
	}

	public bool NotifyIMClient
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("NotifyIMClient", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("NotifyIMClient", value);
		}
	}

	public bool CDPlayJitterCorrect
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("CDPlayJitterCorrect", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("CDPlayJitterCorrect", value);
		}
	}

	public bool ShowNowPlayingList
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowNowPlayingList", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowNowPlayingList", value);
		}
	}

	public bool ShowTotalTime
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowTotalTime", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowTotalTime", value);
		}
	}

	public bool ModeLoop
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ModeLoop", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ModeLoop", value);
		}
	}

	public bool ModeShuffle
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ModeShuffle", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ModeShuffle", value);
		}
	}

	public bool Mute
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("Mute", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("Mute", value);
		}
	}

	public int Volume
	{
		get
		{
			return GetIntProperty("Volume", 50);
		}
		set
		{
			SetIntProperty("Volume", value);
		}
	}

	public int DXVAMaximumFPS
	{
		get
		{
			return GetIntProperty("DXVAMaximumFPS", 31);
		}
		set
		{
			SetIntProperty("DXVAMaximumFPS", value);
		}
	}

	public int ForceDeinterlaceMode
	{
		get
		{
			return GetIntProperty("ForceDeinterlaceMode", 0);
		}
		set
		{
			SetIntProperty("ForceDeinterlaceMode", value);
		}
	}

	public int ApproximateSeekBitrate
	{
		get
		{
			return GetIntProperty("ApproximateSeekBitrate", 0);
		}
		set
		{
			SetIntProperty("ApproximateSeekBitrate", value);
		}
	}

	public int ApproximateSeekDuration
	{
		get
		{
			return GetIntProperty("ApproximateSeekDuration", 1800);
		}
		set
		{
			SetIntProperty("ApproximateSeekDuration", value);
		}
	}

	public bool AllowApproximateSeekingOnShinyDiscOnly
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("AllowApproximateSeekingOnShinyDiscOnly", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("AllowApproximateSeekingOnShinyDiscOnly", value);
		}
	}

	public bool ForceApproximateSeeking
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ForceApproximateSeeking", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ForceApproximateSeeking", value);
		}
	}

	public bool AllowApproximateSeeking
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("AllowApproximateSeeking", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("AllowApproximateSeeking", value);
		}
	}

	public int DynamicRangeControl
	{
		get
		{
			return GetIntProperty("DynamicRangeControl", 0);
		}
		set
		{
			SetIntProperty("DynamicRangeControl", value);
		}
	}

	public bool DisableMediaQueuing
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("DisableMediaQueuing", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("DisableMediaQueuing", value);
		}
	}

	public bool DumpErrorInfo
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("DumpErrorInfo", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("DumpErrorInfo", value);
		}
	}

	public int MP4DropFramesIfLateByInMilliseconds
	{
		get
		{
			return GetIntProperty("MP4DropFramesIfLateByInMilliseconds", 750);
		}
		set
		{
			SetIntProperty("MP4DropFramesIfLateByInMilliseconds", value);
		}
	}

	internal PlaybackConfiguration(RegistryHive hive)
		: base(hive, null, "Playback")
	{
	}

	public PlaybackConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
