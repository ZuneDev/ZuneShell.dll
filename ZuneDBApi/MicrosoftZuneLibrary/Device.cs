using System;
using System.Collections;
using System.Security;

namespace MicrosoftZuneLibrary;

// Original wraps a native IEndpointHost*/ISyncEngine*/IWlanProvider* triple plus a
// DeviceMediator callback sink — the core of device sync, wireless provisioning, and
// firmware update. None of that native layer has been reverse engineered — see
// logs/MicrosoftZuneLibrary/Device.md. Every member below has its exact original
// signature (recovered via ILSpy's get_type_members) but is a "pure no-op stub" per
// CLAUDE.md's stage 2 convention.
public class Device : IDisposable
{
    private SyncRules m_syncRules = new();
    private FirmwareUpdater m_firmwareUpdater = new();
    private GasGauge m_predictedGasGauge = new();
    private GasGauge m_actualGasGauge = new();
    private DeviceAssetSet m_DeviceAssetSet = new();

    public event SyncBeganHandler SyncBegan;
    public event SyncProgressedHandler SyncProgressed;
    public event SyncCompletedHandler SyncCompleted;
    public event FriendlyNameChangedHandler FriendlyNameChangedEvent;
    public event DeviceStatusChangedHandler DeviceStatusChangedEvent;
    public event FormatCompleteHandler FormatCompleteEvent;
    public event GetWlanProfilesCompleteHandler GetWlanProfilesCompleteEvent;
    public event GetDeviceWlanNetworksCompleteHandler GetDeviceWlanNetworksCompleteEvent;
    public event GetDeviceWlanProfilesCompleteHandler GetDeviceWlanProfilesCompleteEvent;
    public event SetDeviceWlanProfilesCompleteHandler SetDeviceWlanProfilesCompleteEvent;
    public event AssociateWlanDeviceCompleteHandler AssociateWlanDeviceCompleteEvent;
    public event UnassociateWlanDeviceCompleteHandler UnassociateWlanDeviceCompleteEvent;
    public event TestDeviceWlanCompleteHandler TestDeviceWlanCompleteEvent;

    internal Device()
    {
    }

    public string MyPhoneDeviceID => null;

    public bool InStandardMode => false;

    public DeviceAssetSet DeviceAssetSet => m_DeviceAssetSet;

    public int LastFirmwareUpdateError => 0;

    public FirmwareUpdater FirmwareUpdater => m_firmwareUpdater;

    public string PicturesVideosViewUrl => null;

    public string PicturesVideosViewText => null;

    public bool SyncSetupRequired => false;

    public bool ClientUpdateRequired => false;

    public bool FirmwareUpdateRequired => false;

    public GasGauge ActualGasGauge => m_actualGasGauge;

    public GasGauge PredictedGasGauge => m_predictedGasGauge;

    public SyncRules Rules => m_syncRules;

    public DateTime LastConnectTime => DateTime.MinValue;

    public DateTime LastSyncTime => DateTime.MinValue;

    public bool IsFormatting => false;

    public bool IsConnectedWirelessly => false;

    public bool IsConnected => false;

    public bool IsAvailable => false;

    public EEndpointStatus DeviceStatus => EEndpointStatus.eEndpointStatusUndefined;

    public string OwnerApplicationName => null;

    public bool IsReady => false;

    public bool IsSyncSuspended => false;

    public bool IsSyncRunning => false;

    public ulong DeviceCapacity => 0;

    public string CanonicalName => null;

    public string EndpointId => null;

    public uint StatedCapacity => 0;

    public uint BackgroundID => 0;

    public uint TattooID => 0;

    public ulong ColorID => 0;

    public uint PrimaryColorID => 0;

    public uint FamilyID => 0;

    public int ClassID => 0;

    public int DeviceID => 0;

    public int StartSync() => unchecked((int)0x80004005);

    public int StartSyncNextNotify() => unchecked((int)0x80004005);

    public int StopSync() => unchecked((int)0x80004005);

    public int StartEnumeration() => unchecked((int)0x80004005);

    public int Format() => unchecked((int)0x80004005);

    public int GetFriendlyName(ref string strName)
    {
        strName = null;
        return unchecked((int)0x80004005);
    }

    public int SetFriendlyName(string strName) => unchecked((int)0x80004005);

    public int GetManufacturer(ref string strManufacturer)
    {
        strManufacturer = null;
        return unchecked((int)0x80004005);
    }

    public int GetModelName(ref string strModelName)
    {
        strModelName = null;
        return unchecked((int)0x80004005);
    }

    public int GetUserGuid(ref Guid guidUserGuid)
    {
        guidUserGuid = Guid.Empty;
        return unchecked((int)0x80004005);
    }

    public int GetUserId(ref int userId)
    {
        userId = 0;
        return unchecked((int)0x80004005);
    }

    public int GetZuneTag(ref string strZuneTag)
    {
        strZuneTag = null;
        return unchecked((int)0x80004005);
    }

    public int GetLiveId(ref string strLiveId)
    {
        strLiveId = null;
        return unchecked((int)0x80004005);
    }

    public int GetIsTvOutSupported(ref bool fIsTvOutSupported)
    {
        fIsTvOutSupported = false;
        return unchecked((int)0x80004005);
    }

    public int GetIsRestorePointSupported(ref bool fIsRestorePointSupported)
    {
        fIsRestorePointSupported = false;
        return unchecked((int)0x80004005);
    }

    public int GetCapability(EEndpointCapability capabilityId, ref bool fHasCapability)
    {
        fHasCapability = false;
        return unchecked((int)0x80004005);
    }

    public int GetGeoId(ref uint dwGeoId)
    {
        dwGeoId = 0;
        return unchecked((int)0x80004005);
    }

    public int SetGeoId(uint dwGeoId) => unchecked((int)0x80004005);

    public int GetTimeZoneBias(ref int lTimeZoneBias)
    {
        lTimeZoneBias = 0;
        return unchecked((int)0x80004005);
    }

    public int SetTimeZoneBias(int lTimeZoneBias) => unchecked((int)0x80004005);

    public int GetWatsonSetting(ref uint dwWatsonSetting)
    {
        dwWatsonSetting = 0;
        return unchecked((int)0x80004005);
    }

    public int SetWatsonSetting(uint dwWatsonSetting) => unchecked((int)0x80004005);

    public int SetUserGuidandZuneTag(Guid guidUserGuid, string strZuneTag) => unchecked((int)0x80004005);

    public int ClearUserGuidandZuneTag() => unchecked((int)0x80004005);

    public int SetMarketplaceCredentials(SecureString strUsername, SecureString strPassword) => unchecked((int)0x80004005);

    public int GetOOBECompleted(ref bool oobeCompleted)
    {
        oobeCompleted = false;
        return unchecked((int)0x80004005);
    }

    public int GetPurchaseEnabled(ref bool purchaseEnabled)
    {
        purchaseEnabled = false;
        return unchecked((int)0x80004005);
    }

    public int SetPurchaseEnabled(bool purchaseEnabled) => unchecked((int)0x80004005);

    public int GetAndResetLastLoginError(ref int hrLogin)
    {
        hrLogin = 0;
        return unchecked((int)0x80004005);
    }

    public int GetAndResetLastDownloadError(ref int hrDownload)
    {
        hrDownload = 0;
        return unchecked((int)0x80004005);
    }

    public int LoadWlanProvider() => unchecked((int)0x80004005);

    public int SetWlanProfileList(WlanProfileList profileList) => unchecked((int)0x80004005);

    public int GetWlanProfileList(ref WlanProfileList profileList)
    {
        profileList = null;
        return unchecked((int)0x80004005);
    }

    public int GetWlanProfiles() => unchecked((int)0x80004005);

    public int IsWlanFirewallEnabled(ref bool bEnabled)
    {
        bEnabled = false;
        return unchecked((int)0x80004005);
    }

    public void GetWlanProfilesComplete(int hr)
    {
        GetWlanProfilesCompleteEvent?.Invoke(this, hr);
    }

    public int GetDeviceWlanNetworks() => unchecked((int)0x80004005);

    public void GetDeviceWlanNetworksComplete(int hr)
    {
        GetDeviceWlanNetworksCompleteEvent?.Invoke(this, hr);
    }

    public int GetDeviceWlanProfiles() => unchecked((int)0x80004005);

    public void GetDeviceWlanProfilesComplete(int hr)
    {
        GetDeviceWlanProfilesCompleteEvent?.Invoke(this, hr);
    }

    public int SetDeviceWlanProfiles() => unchecked((int)0x80004005);

    public void SetDeviceWlanProfilesComplete(int hr)
    {
        SetDeviceWlanProfilesCompleteEvent?.Invoke(this, hr);
    }

    public int GetWlanDeviceAuthCipherPairList(ref WlanAuthCipherPairList authCipherPairList)
    {
        authCipherPairList = null;
        return unchecked((int)0x80004005);
    }

    public int GetDisconnectedWlanDeviceUuid(ref string strDeviceUuid)
    {
        strDeviceUuid = null;
        return unchecked((int)0x80004005);
    }

    public int GetAssociatedWlanDeviceUuidList(ref IList deviceUuidList)
    {
        deviceUuidList = new ArrayList();
        return unchecked((int)0x80004005);
    }

    public int AssociateWlanDevice() => unchecked((int)0x80004005);

    public void AssociateWlanDeviceComplete(int hr)
    {
        AssociateWlanDeviceCompleteEvent?.Invoke(this, hr);
    }

    public int UnassociateWlanDevice() => unchecked((int)0x80004005);

    public void UnassociateWlanDeviceComplete(int hr)
    {
        UnassociateWlanDeviceCompleteEvent?.Invoke(this, hr);
    }

    public int UnassociateWlanDeviceUuid(string strDeviceUuid) => unchecked((int)0x80004005);

    public int IsWlanDeviceDisabled(ref bool bDisabled)
    {
        bDisabled = false;
        return unchecked((int)0x80004005);
    }

    public int IsWlanDeviceUuidDisabled(string strDeviceUuid, ref bool bDisabled)
    {
        bDisabled = false;
        return unchecked((int)0x80004005);
    }

    public int TestDeviceWlan() => unchecked((int)0x80004005);

    public int CancelTestDeviceWlan() => unchecked((int)0x80004005);

    public void TestDeviceWlanComplete(int hr)
    {
        TestDeviceWlanCompleteEvent?.Invoke(this, WlanTestResultCode.wlanTestResultFailInternal, hr);
    }

    public int GetDeviceWlanConnectedSSID(ref string strSSID)
    {
        strSSID = null;
        return unchecked((int)0x80004005);
    }

    public int GetDeviceWlanMediaSyncSSID(ref string strSSID)
    {
        strSSID = null;
        return unchecked((int)0x80004005);
    }

    public int SetDeviceWlanMediaSyncSSID(string strSSID) => unchecked((int)0x80004005);

    public int GetSyncRelationship(ref ESyncRelationship relationship)
    {
        relationship = ESyncRelationship.srNone;
        return unchecked((int)0x80004005);
    }

    public int SetSyncRelationship(ESyncRelationship relationship) => unchecked((int)0x80004005);

    public int GetPromptGuest(ref bool bPromptGuest)
    {
        bPromptGuest = false;
        return unchecked((int)0x80004005);
    }

    public int SetPromptGuest(bool bPromptGuest) => unchecked((int)0x80004005);

    public int GetPromptLink(ref bool bPromptLink)
    {
        bPromptLink = false;
        return unchecked((int)0x80004005);
    }

    public int SetPromptLink(bool bPromptLink) => unchecked((int)0x80004005);

    public int GetFirmwareVersion(ref string strFirmwareVersion)
    {
        strFirmwareVersion = null;
        return unchecked((int)0x80004005);
    }

    public int GetSpaceFree(ref ulong ui64SpaceFree)
    {
        ui64SpaceFree = 0;
        return unchecked((int)0x80004005);
    }

    public int GetSyncOnConnect(ref bool bSyncOnConnect)
    {
        bSyncOnConnect = false;
        return unchecked((int)0x80004005);
    }

    public int SetSyncOnConnect(bool bSyncOnConnect) => unchecked((int)0x80004005);

    public int GetPercentSpaceReserved(ref uint ulPercentage)
    {
        ulPercentage = 0;
        return unchecked((int)0x80004005);
    }

    public int SetPercentSpaceReserved(uint ulPercentage) => unchecked((int)0x80004005);

    public int DeleteMedia(int[] rgIds, EMediaTypes mediaType, ref ESyncOperationStatus operationStatus)
    {
        operationStatus = ESyncOperationStatus.osInvalid;
        return unchecked((int)0x80004005);
    }

    public int ReverseSync(int[] rgIds, EMediaTypes mediaType, ref ESyncOperationStatus operationStatus)
    {
        operationStatus = ESyncOperationStatus.osInvalid;
        return unchecked((int)0x80004005);
    }

    public int GetDeviceVideoDeleteSet(ref int[] rgIds)
    {
        rgIds = Array.Empty<int>();
        return unchecked((int)0x80004005);
    }

    public int GetVideoTranscodeOptimization(ref ETranscodeOptimization transcodeOptimization)
    {
        transcodeOptimization = ETranscodeOptimization.toOptimizeInvalid;
        return unchecked((int)0x80004005);
    }

    public int SetVideoTranscodeOptimization(ETranscodeOptimization transcodeOptimization) => unchecked((int)0x80004005);

    public int GetPhotoVideoReverseSync(ref bool bReverseSync)
    {
        bReverseSync = false;
        return unchecked((int)0x80004005);
    }

    public int SetPhotoVideoReverseSync(bool bReverseSync) => unchecked((int)0x80004005);

    public int GetDeletePhotoVideoAfterReverseSync(ref bool bDeleteAfterSync)
    {
        bDeleteAfterSync = false;
        return unchecked((int)0x80004005);
    }

    public int SetDeletePhotoVideoAfterReverseSync(bool bDeleteAfterSync) => unchecked((int)0x80004005);

    public int GetCameraRollDestinationFolder(ref string strDestinationFolder)
    {
        strDestinationFolder = null;
        return unchecked((int)0x80004005);
    }

    public int SetCameraRollDestinationFolder(string strDestinationFolder) => unchecked((int)0x80004005);

    public int GetSavedDestinationFolder(ref string strDestinationFolder)
    {
        strDestinationFolder = null;
        return unchecked((int)0x80004005);
    }

    public int SetSavedDestinationFolder(string strDestinationFolder) => unchecked((int)0x80004005);

    public int GetPhotoTranscodeSetting(ref ETranscodePhotoSetting ePhotoSetting)
    {
        ePhotoSetting = ETranscodePhotoSetting.tsPhotoSettingInvalid;
        return unchecked((int)0x80004005);
    }

    public int SetPhotoTranscodeSetting(ETranscodePhotoSetting ePhotoSetting) => unchecked((int)0x80004005);

    public int GetAudioTranscodeParams(ref int audioThresholdBitRate, ref int audioTargetBitRate)
    {
        audioThresholdBitRate = 0;
        audioTargetBitRate = 0;
        return unchecked((int)0x80004005);
    }

    public int SetAudioTranscodeParams(int audioThresholdBitRate, int audioTargetBitRate) => unchecked((int)0x80004005);

    public int GetLocalizedDevicePath(ref string strPath)
    {
        strPath = null;
        return unchecked((int)0x80004005);
    }

    public int ClearCache() => unchecked((int)0x80004005);

    public int ClearRules() => unchecked((int)0x80004005);

    public int ClearManualModeRules() => unchecked((int)0x80004005);

    public int DeleteAllGuestContent(ref ESyncOperationStatus operationStatus)
    {
        operationStatus = ESyncOperationStatus.osInvalid;
        return unchecked((int)0x80004005);
    }

    public int ForceAppUpdate() => unchecked((int)0x80004005);

    public int FileExistsForTranscode(int iMediaId, EMediaTypes eMediaType, ref string strTranscodedFileName)
    {
        strTranscodedFileName = null;
        return unchecked((int)0x80004005);
    }

    public int SyncBeginCallback()
    {
        SyncBegan?.Invoke(this);
        return 0;
    }

    public int SyncProgressCallback(uint uiPercentComplete, uint uiPercentItemComplete, uint uiPercentTranscodeComplete, string bstrGroup, string bstrTitle, ESyncEngineState engineState)
    {
        SyncProgressed?.Invoke(this, uiPercentComplete, uiPercentItemComplete, uiPercentTranscodeComplete, bstrGroup, bstrTitle, engineState);
        return 0;
    }

    public int SyncCompleteCallback(int hrResult)
    {
        SyncCompleted?.Invoke(this, hrResult >= 0 ? ESyncEventReason.eSyncEventSucceeded : ESyncEventReason.eSyncEventFailed);
        return 0;
    }

    public void FriendlyNameChanged(string wszFriendlyName)
    {
        FriendlyNameChangedEvent?.Invoke(this, wszFriendlyName);
    }

    public void EndpointStatusChanged(int hrEnumeration, EEndpointStatus eDeviceStatus)
    {
        DeviceStatusChangedEvent?.Invoke(this, hrEnumeration, eDeviceStatus);
    }

    public void FormatComplete(int hrResult)
    {
        FormatCompleteEvent?.Invoke(this, hrResult);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
