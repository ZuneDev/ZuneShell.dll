namespace MicrosoftZuneLibrary;

// Grouped together since each is a one-line delegate declaration recovered from
// Device/DeviceList/GasGauge/HMESettings's event fields — see
// logs/MicrosoftZuneLibrary/Device.md.
public delegate void DeviceAddedHandler(Device device);
public delegate void SyncBeganHandler(Device device);
public delegate void SyncProgressedHandler(Device device, uint percentComplete, uint percentItemComplete, uint percentTranscodeComplete, string group, string title, ESyncEngineState engineState);
public delegate void SyncCompletedHandler(Device device, ESyncEventReason reason);
public delegate void FriendlyNameChangedHandler(Device device, string strFriendlyName);
public delegate void DeviceStatusChangedHandler(Device device, int hrEnumeration, EEndpointStatus eDeviceStatus);
public delegate void FormatCompleteHandler(Device device, int hrResult);
public delegate void GetWlanProfilesCompleteHandler(Device device, int hr);
public delegate void GetDeviceWlanNetworksCompleteHandler(Device device, int hr);
public delegate void GetDeviceWlanProfilesCompleteHandler(Device device, int hr);
public delegate void SetDeviceWlanProfilesCompleteHandler(Device device, int hr);
public delegate void AssociateWlanDeviceCompleteHandler(Device device, int hr);
public delegate void UnassociateWlanDeviceCompleteHandler(Device device, int hr);
public delegate void TestDeviceWlanCompleteHandler(Device device, WlanTestResultCode result, int hr);
public delegate void CategorySpaceUsedUpdatedHandler(GasGauge gasGauge, ESyncCategory syncCategory, long llNewSchemaSpace, long llNewFreeSpace);
public delegate void ReservedSpaceUpdatedHandler(GasGauge gasGauge, long llNewReservedSpace, long llNewFreeSpace);
public delegate void DeviceOverflowHandler(GasGauge gasGauge);
public delegate void NSSDeviceListChangeHandler();
public delegate void CorePhase2ReadyCallback(int hr, bool success);
public delegate void OnShowErrorDialogHandler(int hr, uint uiStringId);
