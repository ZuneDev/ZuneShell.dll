using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Zune.Configuration;

public class DevicesConfiguration : CConfigurationManagedBase
{
	public bool HasPhoneBeenConnected
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("HasPhoneBeenConnected", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("HasPhoneBeenConnected", value);
		}
	}

	public bool AlwaysUpdatePlaylists
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("AlwaysUpdatePlaylists", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("AlwaysUpdatePlaylists", value);
		}
	}

	public int ConnectionsUntilWirelessSyncBannerDisplay
	{
		get
		{
			return GetIntProperty("ConnectionsUntilWirelessSyncBannerDisplay", -1);
		}
		set
		{
			SetIntProperty("ConnectionsUntilWirelessSyncBannerDisplay", value);
		}
	}

	public bool ShowExcludeFromSyncWarning
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowExcludeFromSyncWarning", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowExcludeFromSyncWarning", value);
		}
	}

	public bool ShowSyncInstructionsToast
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ShowSyncInstructionsToast", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ShowSyncInstructionsToast", value);
		}
	}

	public int FormatAsyncTimeoutMs
	{
		get
		{
			return GetIntProperty("FormatAsyncTimeoutMs", 600000);
		}
		set
		{
			SetIntProperty("FormatAsyncTimeoutMs", value);
		}
	}

	public int FormatAsyncPollingFrequencyMs
	{
		get
		{
			return GetIntProperty("FormatAsyncPollingFrequencyMs", 1000);
		}
		set
		{
			SetIntProperty("FormatAsyncPollingFrequencyMs", value);
		}
	}

	public string DeviceAssetCabPathOem
	{
		get
		{
			return GetStringProperty("DeviceAssetCabPathOem", "");
		}
		set
		{
			SetStringProperty("DeviceAssetCabPathOem", value);
		}
	}

	public string DeviceAssetCabPathMicrosoft
	{
		get
		{
			return GetStringProperty("DeviceAssetCabPathMicrosoft", "");
		}
		set
		{
			SetStringProperty("DeviceAssetCabPathMicrosoft", value);
		}
	}

	public int CloudSyncIdleTimeout
	{
		get
		{
			return GetIntProperty("CloudSyncIdleTimeout", 90000);
		}
		set
		{
			SetIntProperty("CloudSyncIdleTimeout", value);
		}
	}

	public bool CloudSyncEnabled
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("CloudSyncEnabled", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("CloudSyncEnabled", value);
		}
	}

	public bool RebootDeviceAfterFormat
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("RebootDeviceAfterFormat", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("RebootDeviceAfterFormat", value);
		}
	}

	public bool EnableSummaryRestore
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("EnableSummaryRestore", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("EnableSummaryRestore", value);
		}
	}

	public int SyncRelationshipOverrideValue
	{
		get
		{
			return GetIntProperty("SyncRelationshipOverrideValue", 0);
		}
		set
		{
			SetIntProperty("SyncRelationshipOverrideValue", value);
		}
	}

	public bool OverrideSyncRelationship
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("OverrideSyncRelationship", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("OverrideSyncRelationship", value);
		}
	}

	public bool AbortTasksWhenCanceled
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("AbortTasksWhenCanceled", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("AbortTasksWhenCanceled", value);
		}
	}

	public bool DisplayTitlesForMetadataUpdates
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("DisplayTitlesForMetadataUpdates", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("DisplayTitlesForMetadataUpdates", value);
		}
	}

	public int WlanTestTimeout
	{
		get
		{
			return GetIntProperty("WlanTestTimeout", 60000);
		}
		set
		{
			SetIntProperty("WlanTestTimeout", value);
		}
	}

	public bool PreserveStaleDeviceContentOnConnect
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("PreserveStaleDeviceContentOnConnect", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("PreserveStaleDeviceContentOnConnect", value);
		}
	}

	public int CurrentDeviceID
	{
		get
		{
			return GetIntProperty("CurrentDeviceID", 0);
		}
		set
		{
			SetIntProperty("CurrentDeviceID", value);
		}
	}

	public bool TraceIntSet
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("TraceIntSet", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("TraceIntSet", value);
		}
	}

	public bool SyncInProgress
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("SyncInProgress", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("SyncInProgress", value);
		}
	}

	public int EstimatedArtistSizeInKB
	{
		get
		{
			return GetIntProperty("EstimatedArtistSizeInKB", 500);
		}
		set
		{
			SetIntProperty("EstimatedArtistSizeInKB", value);
		}
	}

	public int SmallDeviceSizeInGigs
	{
		get
		{
			return GetIntProperty("SmallDeviceSizeInGigs", 17);
		}
		set
		{
			SetIntProperty("SmallDeviceSizeInGigs", value);
		}
	}

	public bool DefaultWpOptimizationForWvga
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("DefaultWpOptimizationForWvga", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("DefaultWpOptimizationForWvga", value);
		}
	}

	public bool DefaultHdOptimizationForQuality
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("DefaultHdOptimizationForQuality", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("DefaultHdOptimizationForQuality", value);
		}
	}

	public bool NotifyRemoteSessionsOnArrival
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("NotifyRemoteSessionsOnArrival", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("NotifyRemoteSessionsOnArrival", value);
		}
	}

	public bool BreakOnSyncError
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("BreakOnSyncError", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("BreakOnSyncError", value);
		}
	}

	public int SyncProgressInterval
	{
		get
		{
			return GetIntProperty("SyncProgressInterval", 250);
		}
		set
		{
			SetIntProperty("SyncProgressInterval", value);
		}
	}

	public int LiveSyncHeartbeatPeriod
	{
		get
		{
			return GetIntProperty("LiveSyncHeartbeatPeriod", 15000);
		}
		set
		{
			SetIntProperty("LiveSyncHeartbeatPeriod", value);
		}
	}

	public int LiveSyncThrottlingPeriod
	{
		get
		{
			return GetIntProperty("LiveSyncThrottlingPeriod", 60000);
		}
		set
		{
			SetIntProperty("LiveSyncThrottlingPeriod", value);
		}
	}

	public long FakeAvailableSpace
	{
		get
		{
			return GetInt64Property("FakeAvailableSpace", 0L);
		}
		set
		{
			SetInt64Property("FakeAvailableSpace", value);
		}
	}

	public int FakeLastFirmwareUpdateError
	{
		get
		{
			return GetIntProperty("FakeLastFirmwareUpdateError", 0);
		}
		set
		{
			SetIntProperty("FakeLastFirmwareUpdateError", value);
		}
	}

	public string FakeFirmwareVersion
	{
		get
		{
			return GetStringProperty("FakeFirmwareVersion", "");
		}
		set
		{
			SetStringProperty("FakeFirmwareVersion", value);
		}
	}

	public int FakeProtocolCompatibilityVersion
	{
		get
		{
			return GetIntProperty("FakeProtocolCompatibilityVersion", 0);
		}
		set
		{
			SetIntProperty("FakeProtocolCompatibilityVersion", value);
		}
	}

	public bool ForceMetering
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ForceMetering", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ForceMetering", value);
		}
	}

	public string DeviceLastAvailable
	{
		get
		{
			return GetStringProperty("DeviceLastAvailable", "");
		}
		set
		{
			SetStringProperty("DeviceLastAvailable", value);
		}
	}

	public int DeviceAvailableInterval
	{
		get
		{
			return GetIntProperty("DeviceAvailableInterval", 168);
		}
		set
		{
			SetIntProperty("DeviceAvailableInterval", value);
		}
	}

	public string ZMDBSaveToFile
	{
		get
		{
			return GetStringProperty("ZMDBSaveToFile", "");
		}
		set
		{
			SetStringProperty("ZMDBSaveToFile", value);
		}
	}

	public string ZMDBLoadFromFile
	{
		get
		{
			return GetStringProperty("ZMDBLoadFromFile", "");
		}
		set
		{
			SetStringProperty("ZMDBLoadFromFile", value);
		}
	}

	public bool ReingestZMDB
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("ReingestZMDB", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("ReingestZMDB", value);
		}
	}

	public string DeviceCapsSaveToFile
	{
		get
		{
			return GetStringProperty("DeviceCapsSaveToFile", "");
		}
		set
		{
			SetStringProperty("DeviceCapsSaveToFile", value);
		}
	}

	public int DeviceCapsInitializeTimeout
	{
		get
		{
			return GetIntProperty("DeviceCapsInitializeTimeout", 45000);
		}
		set
		{
			SetIntProperty("DeviceCapsInitializeTimeout", value);
		}
	}

	public int DeviceGetAvailableSpaceTimeout
	{
		get
		{
			return GetIntProperty("DeviceGetAvailableSpaceTimeout", 3000);
		}
		set
		{
			SetIntProperty("DeviceGetAvailableSpaceTimeout", value);
		}
	}

	public int SetDeviceRelationshipIDTimeout
	{
		get
		{
			return GetIntProperty("SetDeviceRelationshipIDTimeout", 20000);
		}
		set
		{
			SetIntProperty("SetDeviceRelationshipIDTimeout", value);
		}
	}

	public bool IgnoreSyncOnConnect
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("IgnoreSyncOnConnect", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("IgnoreSyncOnConnect", value);
		}
	}

	public bool InvestigateDevicePresence
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("InvestigateDevicePresence", defaultValue: false);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("InvestigateDevicePresence", value);
		}
	}

	public bool CheckCRNOnIncrement
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("CheckCRNOnIncrement", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("CheckCRNOnIncrement", value);
		}
	}

	public string AutoLaunchZuneParams
	{
		get
		{
			return GetStringProperty("AutoLaunchZuneParams", "");
		}
		set
		{
			SetStringProperty("AutoLaunchZuneParams", value);
		}
	}

	public bool AutoLaunchZuneOnConnect
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return GetBoolProperty("AutoLaunchZuneOnConnect", defaultValue: true);
		}
		[param: MarshalAs(UnmanagedType.U1)]
		set
		{
			SetBoolProperty("AutoLaunchZuneOnConnect", value);
		}
	}

	internal DevicesConfiguration(RegistryHive hive)
		: base(hive, null, "Devices")
	{
	}

	public DevicesConfiguration(RegistryHive hive, string basePath, string instance)
		: base(hive, basePath, instance)
	{
	}
}
