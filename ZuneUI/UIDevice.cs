// Decompiled with JetBrains decompiler
// Type: ZuneUI.UIDevice
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Threading;
using UIXControls;
using ZuneXml;

namespace ZuneUI
{
    public class UIDevice : ModelItem
    {
        private IDeviceIconSet _iconSet;
        private static readonly UIDevice.INewRuleMessageType[] _newRuleMessageLookupTable = new UIDevice.INewRuleMessageType[11]
        {
      (UIDevice.INewRuleMessageType) new UIDevice.NewRuleMessageType(MediaType.Track, StringId.IDS_ADDED_1_TRACK, StringId.IDS_ADDED_N_TRACKS),
      (UIDevice.INewRuleMessageType) new UIDevice.NewRuleMessageType(MediaType.Album, StringId.IDS_ADDED_1_ALBUM, StringId.IDS_ADDED_N_ALBUMS),
      (UIDevice.INewRuleMessageType) new UIDevice.NewRuleMessageType(MediaType.Artist, StringId.IDS_ADDED_1_ARTIST, StringId.IDS_ADDED_N_ARTISTS),
      (UIDevice.INewRuleMessageType) new UIDevice.NewRuleMessageType(MediaType.Genre, StringId.IDS_ADDED_1_GENRE, StringId.IDS_ADDED_N_GENRES),
      (UIDevice.INewRuleMessageType) new UIDevice.NewRuleMessageType(MediaType.Playlist, StringId.IDS_ADDED_1_PLAYLIST, StringId.IDS_ADDED_N_PLAYLISTS),
      (UIDevice.INewRuleMessageType) new UIDevice.NewRuleMessageType(MediaType.Video, StringId.IDS_ADDED_1_VIDEO, StringId.IDS_ADDED_N_VIDEOS),
      (UIDevice.INewRuleMessageType) new UIDevice.NewRuleMessageType(MediaType.Photo, StringId.IDS_ADDED_1_PHOTO, StringId.IDS_ADDED_N_PHOTOS),
      (UIDevice.INewRuleMessageType) new UIDevice.NewRuleMessageType(MediaType.MediaFolder, StringId.IDS_ADDED_1_FOLDER, StringId.IDS_ADDED_N_FOLDERS),
      (UIDevice.INewRuleMessageType) new UIDevice.NewRuleMessageType(MediaType.PodcastEpisode, StringId.IDS_ADDED_1_PODCAST_EPISODE, StringId.IDS_ADDED_N_PODCAST_EPISODES),
      (UIDevice.INewRuleMessageType) new UIDevice.NewRuleMessageType(MediaType.Podcast, StringId.IDS_ADDED_1_PODCAST, StringId.IDS_ADDED_N_PODCASTS),
      (UIDevice.INewRuleMessageType) new UIDevice.NewRuleMessageType(MediaType.UserCard, StringId.IDS_ADDED_1_FRIEND, StringId.IDS_ADDED_N_FRIENDS)
        };
        private static readonly UIDevice.INewRuleMessageType _channelNewRuleMessage = (UIDevice.INewRuleMessageType)new UIDevice.NewRuleMessageType(MediaType.Playlist, StringId.IDS_ADDED_1_CHANNEL, StringId.IDS_ADDED_N_CHANNELS);
        private static readonly UIDevice.INewRuleMessageType _genericNewRuleMessage = (UIDevice.INewRuleMessageType)new UIDevice.NewRuleMessageType(MediaType.Undefined, StringId.IDS_ADDED_1_ITEM, StringId.IDS_ADDED_N_ITEMS);
        private static bool _isOosDialogVisible;
        private static string _syncStatusTitleDivider = ZuneUI.Shell.LoadString(StringId.IDS_SYNCITEM_TITLE_SEPARATOR);
        private static string _syncStatusDatumDivider = ZuneUI.Shell.LoadString(StringId.IDS_SYNCITEM_METADATUM_SEPARATOR);
        private UIFirmwareUpdater _firmwareUpdater;
        private UIFirmwareRestorer _firmwareRestorer;
        private Device _device;
        private UIGasGauge _actualGasGauge;
        private UIGasGauge _predictedGasGauge;
        private SyncNotification _syncProgress;
        private Command _syncBegun;
        private Command _syncProgressed;
        private Command _syncCompleted;
        private bool _currentlySyncing;
        private bool _currentlyFormatting;
        private bool _isWirelessSyncEnabled;
        private EEndpointStatus _lastDeviceState;
        private bool _isReadyForSync;
        private bool _isLocked;
        private bool _userStoppedLastSync;
        private Microsoft.Iris.Timer _formatSanityTimer;
        private string _lastConnectTimestring;
        private string _lastSyncStartTimestring;
        private HRESULT _lastLoginFailure;
        private HRESULT _lastSyncFailure;
        private MessageBox _pinUnlockMessageBox;

        public IDeviceIconSet IconSet
        {
            get
            {
                if (this._iconSet == null)
                    this._iconSet = !this.IsValid || this._device.DeviceAssetSet == null ? DeviceIconSetFactory.DefaultIconSet : DeviceIconSetFactory.BuildDeviceIconSet(this._device.DeviceAssetSet, new DeviceIconSetFactory.DeviceIconSetConstructionCompletedCallback(this.SetDeviceIconSetCallback));
                return this._iconSet;
            }
            private set
            {
                if (this._iconSet == value)
                    return;
                this._iconSet = value;
                this.FirePropertyChanged(nameof(IconSet));
            }
        }

        private void ReloadIconSet() => this.IconSet = (IDeviceIconSet)null;

        private void SetDeviceIconSetCallback(IDeviceIconSet iconSet) => this.IconSet = iconSet;

        public event FallibleEventHandler FormatCompletedEvent;

        public event FallibleEventHandler EnumeratedEvent;

        public event FallibleEventHandler WiFiRemovalCompletedEvent;

        public event FallibleEventHandler WiFiProfilesSentEvent;

        public event FallibleEventHandler WiFiProfilesReceivedEvent;

        public event FallibleEventHandler WiFiTestCompletedEvent;

        public event FallibleEventHandler WiFiAssociationCompletedEvent;

        public event FallibleEventHandler ComputerWiFiProfilesLoadedEvent;

        public event FallibleEventHandler WiFiScanCompletedEvent;

        internal UIDevice(IModelItemOwner owner, Device device)
          : base(owner)
        {
            this._device = device;
            this._lastLoginFailure = HRESULT._S_OK;
            this._lastSyncFailure = HRESULT._S_OK;
            if (this._device != null)
            {
                this._device.FriendlyNameChangedEvent += new FriendlyNameChangedHandler(this.OnNameChange);
                this._device.DeviceStatusChangedEvent += new DeviceStatusChangedHandler(this.OnDeviceStatusChanged);
                this._device.FormatCompleteEvent += new FormatCompleteHandler(this.OnFormatPhase1Completed);
                this._device.SyncBegan += new SyncBeganHandler(this.OnSyncBegun);
                this._device.SyncProgressed += new SyncProgressedHandler(this.OnSyncProgressed);
                this._device.SyncCompleted += new SyncCompletedHandler(this.OnSyncCompleted);
                this._device.UnassociateWlanDeviceCompleteEvent += new UnassociateWlanDeviceCompleteHandler(this.OnUnassociateWlanCompleted);
                this._device.SetDeviceWlanProfilesCompleteEvent += new SetDeviceWlanProfilesCompleteHandler(this.OnSetDeviceWlanProfilesCompleted);
                this._device.GetDeviceWlanProfilesCompleteEvent += new GetDeviceWlanProfilesCompleteHandler(this.OnGetDeviceWlanProfilesCompleted);
                this._device.TestDeviceWlanCompleteEvent += new TestDeviceWlanCompleteHandler(this.OnTestWlanCompleted);
                this._device.AssociateWlanDeviceCompleteEvent += new AssociateWlanDeviceCompleteHandler(this.OnAssociateWlanCompleted);
                this._device.GetWlanProfilesCompleteEvent += new GetWlanProfilesCompleteHandler(this.OnGetWlanProfilesCompleted);
                this._device.GetDeviceWlanNetworksCompleteEvent += new GetDeviceWlanNetworksCompleteHandler(this.OnGetDeviceWlanVisibleNetworksCompleted);
                this._actualGasGauge = new UIGasGauge((IModelItemOwner)this, this._device.ActualGasGauge);
                this._predictedGasGauge = new UIGasGauge((IModelItemOwner)this, this._device.PredictedGasGauge);
                if (this._device.PredictedGasGauge != null)
                    this._device.PredictedGasGauge.DeviceOverflowEvent += new DeviceOverflowHandler(this.OnDeviceOverfill);
                EEndpointStatus currentStatus = this._device.DeviceStatus;
                if (currentStatus != EEndpointStatus.eEndpointStatusUndefined)
                    Application.DeferredInvoke((DeferredInvokeHandler)delegate
                   {
                       this.HandleDeviceState(currentStatus, HRESULT._S_OK);
                   }, (object)null);
            }
            else
            {
                this._actualGasGauge = new UIGasGauge((IModelItemOwner)this, (GasGauge)null);
                this._predictedGasGauge = new UIGasGauge((IModelItemOwner)this, (GasGauge)null);
            }
            this._syncBegun = new Command((IModelItemOwner)this);
            this._syncProgressed = new Command((IModelItemOwner)this);
            this._syncCompleted = new Command((IModelItemOwner)this);
        }

        protected override void OnDispose(bool disposing)
        {
            if (this._device != null)
            {
                this._device.FriendlyNameChangedEvent -= new FriendlyNameChangedHandler(this.OnNameChange);
                this._device.DeviceStatusChangedEvent -= new DeviceStatusChangedHandler(this.OnDeviceStatusChanged);
                this._device.FormatCompleteEvent -= new FormatCompleteHandler(this.OnFormatPhase1Completed);
                if (this._device.PredictedGasGauge != null)
                    this._device.PredictedGasGauge.DeviceOverflowEvent -= new DeviceOverflowHandler(this.OnDeviceOverfill);
                this._device.SyncBegan -= new SyncBeganHandler(this.OnSyncBegun);
                this._device.SyncProgressed -= new SyncProgressedHandler(this.OnSyncProgressed);
                this._device.SyncCompleted -= new SyncCompletedHandler(this.OnSyncCompleted);
                this._device.UnassociateWlanDeviceCompleteEvent -= new UnassociateWlanDeviceCompleteHandler(this.OnUnassociateWlanCompleted);
                this._device.SetDeviceWlanProfilesCompleteEvent -= new SetDeviceWlanProfilesCompleteHandler(this.OnSetDeviceWlanProfilesCompleted);
                this._device.GetDeviceWlanProfilesCompleteEvent -= new GetDeviceWlanProfilesCompleteHandler(this.OnGetDeviceWlanProfilesCompleted);
                this._device.TestDeviceWlanCompleteEvent -= new TestDeviceWlanCompleteHandler(this.OnTestWlanCompleted);
                this._device.AssociateWlanDeviceCompleteEvent -= new AssociateWlanDeviceCompleteHandler(this.OnAssociateWlanCompleted);
                this._device.GetWlanProfilesCompleteEvent -= new GetWlanProfilesCompleteHandler(this.OnGetWlanProfilesCompleted);
                this._device.GetDeviceWlanNetworksCompleteEvent -= new GetDeviceWlanNetworksCompleteHandler(this.OnGetDeviceWlanVisibleNetworksCompleted);
            }
            base.OnDispose(disposing);
        }

        public bool IsValid => this._device != null && !this.IsDisposed && this._lastDeviceState > EEndpointStatus.eEndpointStatusUndefined;

        public int ID => !this.IsValid ? 0 : this._device.DeviceID;

        public string MyPhoneDeviceID
        {
            get
            {
                string str = string.Empty;
                if (this.IsValid && this.SupportsMyPhoneLinks && this._device.MyPhoneDeviceID != null)
                    str = this._device.MyPhoneDeviceID;
                return str;
            }
        }

        public string EndpointId => !this.IsValid ? string.Empty : this._device.EndpointId;

        public string CanonicalName => !this.IsConnectedToClient ? string.Empty : this._device.CanonicalName;

        public DeviceClass Class => !this.IsValid ? DeviceClass.Invalid : (DeviceClass)this._device.ClassID;

        public int AdvertisedCapacity => !this.IsValid ? 0 : (int)this._device.StatedCapacity;

        public bool AllowChainedUpdates { get; set; }

        public bool NavigateToDeviceSummaryAfterUpdate { get; set; }

        public int SequentialUpdatesInstalled { get; set; }

        public bool SkipFutureBackupRequests { get; set; }

        public bool SupportsTVOutput
        {
            get
            {
                bool fIsTvOutSupported = false;
                if (this.IsValid)
                    this._device.GetIsTvOutSupported(ref fIsTvOutSupported);
                return fIsTvOutSupported;
            }
        }

        public bool SupportsClassicGames
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityInstallFirmwareGames, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsLiveId
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityLiveId, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsOOBECompleted
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityOOBECompleted, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsRestorePoint
        {
            get
            {
                bool fHasCapability = false;
                return (!this.IsValid || ((HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityRestorePoint, ref fHasCapability)).IsSuccess) && fHasCapability;
            }
        }

        public bool RequiresAutoRestore => this.SupportsRestorePoint && !this._device.InStandardMode;

        public bool InStandardMode => this.IsValid && this._device.InStandardMode;

        public bool SupportsSyncApplications
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilitySyncApps, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsPaidApplications
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityPaidApps, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsStoreApplications => this.SupportsSyncApplications || this.SupportsPaidApplications;

        public bool SupportsHD
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityHdVideo, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsWiFi
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityWireless, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsWirelessSetupMethod1
        {
            get
            {
                bool fHasCapability = true;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityWirelessSetupMethod1, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsWirelessSetupMethod2
        {
            get
            {
                bool fHasCapability = true;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityWirelessSetupMethod2, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsFirmwareUpdate
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityFirmwareUpdate, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsMyPhoneLinks
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityMyPhoneLinks, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsRental
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityRental, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsUserCards
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityCloudSync, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsChannels
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityCloudSync, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsFormat
        {
            get
            {
                bool fHasCapability = true;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityFormat, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsZuneTagLinking
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityZuneTagLinking, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool SupportsUsageData
        {
            get
            {
                bool fHasCapability = false;
                HRESULT hresult = HRESULT._S_OK;
                if (this.IsValid)
                    hresult = (HRESULT)this._device.GetCapability(EEndpointCapability.eEndpointCapabilityUsageData, ref fHasCapability);
                return hresult.IsSuccess && fHasCapability;
            }
        }

        public bool HasOffloadedContent => this.IsValid && !string.IsNullOrEmpty(this.OffloadedContentMessage) && !string.IsNullOrEmpty(this.OffloadedContentUrl);

        public string OffloadedContentMessage
        {
            get
            {
                string str = (string)null;
                if (this.IsValid)
                    str = this._device.PicturesVideosViewText;
                return str ?? string.Empty;
            }
        }

        public string OffloadedContentUrl
        {
            get
            {
                string str = (string)null;
                if (this.IsValid)
                    str = this._device.PicturesVideosViewUrl;
                return str ?? string.Empty;
            }
        }

        public bool HasRestorePoint => this.SupportsRestorePoint && this.UIFirmwareRestorer != null && this.UIFirmwareRestorer.RestorePoint != null;

        public string RestorePointDate => !this.HasRestorePoint ? string.Empty : StringFormatHelper.Format(this.UIFirmwareRestorer.RestorePoint.CreationDate.ToLocalTime(), StringFormatHelper.FriendlyMonthYearPattern);

        public string Manufacturer
        {
            get
            {
                string strManufacturer = (string)null;
                if (this.IsValid)
                    this._device.GetManufacturer(ref strManufacturer);
                return strManufacturer ?? string.Empty;
            }
        }

        public string ModelName
        {
            get
            {
                string strModelName = (string)null;
                if (this.IsValid)
                    this._device.GetModelName(ref strModelName);
                return strModelName ?? string.Empty;
            }
        }

        public bool IsConnectedToPC => this.IsValid && this._lastDeviceState >= EEndpointStatus.eEndpointStatusConnected;

        public bool IsConnectedToClient => this.IsValid && this._lastDeviceState >= EEndpointStatus.eEndpointStatusAvailable;

        public bool IsConnectedToClientWirelessly => this.IsValid && this.IsConnectedToClient && this._device.IsConnectedWirelessly;

        public bool IsConnectedToClientPhysically => this.IsConnectedToClient && !this.IsConnectedToClientWirelessly;

        public bool IsConnectedToSideloader => this.IsValid && this._lastDeviceState == EEndpointStatus.eEndpointStatusInUse;

        public string OwnerApplicationName => !this.IsConnectedToSideloader ? string.Empty : this._device.OwnerApplicationName;

        public bool IsReadyForSync
        {
            get => this.IsConnectedToClient && this._isReadyForSync;
            private set
            {
                if (!this.IsValid || this._isReadyForSync == value)
                    return;
                this._isReadyForSync = value;
                this.FirePropertyChanged(nameof(IsReadyForSync));
            }
        }

        public bool IsFormatting
        {
            get => this._currentlyFormatting;
            private set
            {
                if (!this.IsValid || this._currentlyFormatting == value)
                    return;
                this._currentlyFormatting = value;
                this.FirePropertyChanged(nameof(IsFormatting));
            }
        }

        public bool IsLockedAgainstSyncing
        {
            get => this._isLocked;
            set
            {
                if (!this.IsValid || this._isLocked == value)
                    return;
                this._isLocked = value;
                if (!this._isLocked)
                    return;
                this.EndSync();
            }
        }

        public bool UserStoppedLastSync
        {
            get => this._userStoppedLastSync;
            private set
            {
                if (this._userStoppedLastSync == value)
                    return;
                this._userStoppedLastSync = value;
                this.FirePropertyChanged(nameof(UserStoppedLastSync));
            }
        }

        public HRESULT LastFailedLoginError
        {
            get => this._lastLoginFailure;
            set
            {
                if (!(this._lastLoginFailure != value))
                    return;
                this._lastLoginFailure = value;
                this.FirePropertyChanged(nameof(LastFailedLoginError));
                this.FirePropertyChanged("HasFailedLogin");
            }
        }

        public bool HasFailedLogin => this.LastFailedLoginError.IsError;

        public HRESULT LastSyncError
        {
            get => this._lastSyncFailure;
            private set
            {
                if (!(this._lastSyncFailure != value))
                    return;
                this._lastSyncFailure = value;
                this.FirePropertyChanged(nameof(LastSyncError));
                this.FirePropertyChanged("HasFailedSync");
            }
        }

        public bool HasFailedSync => this.LastSyncError.IsError;

        public string PivotDescription => ZuneUI.Shell.LoadString(StringId.IDS_DEVICE_PIVOT);

        public bool IsWirelessSyncEnabled
        {
            get => this.IsValid && this._isWirelessSyncEnabled;
            private set
            {
                if (this._isWirelessSyncEnabled == value)
                    return;
                this._isWirelessSyncEnabled = value;
                this.FirePropertyChanged(nameof(IsWirelessSyncEnabled));
            }
        }

        public DeviceRelationship Relationship
        {
            get
            {
                DeviceRelationship deviceRelationship = DeviceRelationship.None;
                if (this.IsValid)
                {
                    ESyncRelationship relationship = ESyncRelationship.srNone;
                    if (((HRESULT)this._device.GetSyncRelationship(ref relationship)).IsSuccess)
                        deviceRelationship = (DeviceRelationship)relationship;
                }
                return deviceRelationship;
            }
            set
            {
                if (!this.IsValid || this.Relationship == value)
                    return;
                this._device.SetSyncRelationship((ESyncRelationship)value);
                if (value == DeviceRelationship.Permanent)
                    this.SetGeoId();
                this._device.SetTimeZoneBias(Convert.ToInt32(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalMinutes));
                int num = this.HasFailedLogin ? 1 : 0;
                this.FirePropertyChanged(nameof(Relationship));
            }
        }

        public bool IsGuest
        {
            get
            {
                if (!this.IsValid)
                    return false;
                return this.Relationship == DeviceRelationship.Guest || this.Relationship == DeviceRelationship.None;
            }
        }

        public string GetLocalizedDevicePath(string devicePath)
        {
            if (!this.IsValid)
                return devicePath;
            string strPath = devicePath;
            return !((HRESULT)this._device.GetLocalizedDevicePath(ref strPath)).IsSuccess ? devicePath : strPath;
        }

        public string Name
        {
            get
            {
                if (!this.IsValid)
                    return ZuneUI.Shell.LoadString(StringId.IDS_NO_DEVICE);
                string strName = (string)null;
                return !((HRESULT)this._device.GetFriendlyName(ref strName)).IsSuccess ? ZuneUI.Shell.LoadString(StringId.IDS_SYNC_DEFAULT_DEVICE_WORD) : strName;
            }
            set
            {
                if (!this.IsConnectedToClient)
                    return;
                this._device.SetFriendlyName(value);
            }
        }

        public string NameXMLEscaped => SecurityElement.Escape(this.Name);

        public bool IsPermanentGuest
        {
            get
            {
                bool bPromptGuest = true;
                if (this.IsValid)
                    this._device.GetPromptGuest(ref bPromptGuest);
                return !bPromptGuest;
            }
            set
            {
                if (!this.IsValid || this.IsPermanentGuest == value)
                    return;
                this._device.SetPromptGuest(!value);
                this.FirePropertyChanged(nameof(IsPermanentGuest));
            }
        }

        public Guid UserGuid
        {
            get
            {
                Guid empty = Guid.Empty;
                if (this.IsValid)
                    this._device.GetUserGuid(ref empty);
                return empty;
            }
        }

        public int UserId
        {
            get
            {
                int userId = 0;
                if (this.IsValid)
                    this._device.GetUserId(ref userId);
                return userId;
            }
        }

        public string ZuneTag
        {
            get
            {
                string empty = string.Empty;
                if (this.IsConnectedToClient)
                    this._device.GetZuneTag(ref empty);
                return empty;
            }
        }

        public string LiveId
        {
            get
            {
                string empty = string.Empty;
                if (this.SupportsLiveId)
                    this._device.GetLiveId(ref empty);
                return empty;
            }
        }

        public bool OOBECompleted
        {
            get
            {
                bool oobeCompleted = false;
                if (this.SupportsOOBECompleted && this.IsConnectedToClient)
                {
                    if (!this._device.InStandardMode)
                        return true;
                    this._device.GetOOBECompleted(ref oobeCompleted);
                }
                return oobeCompleted;
            }
        }

        public bool ExcludeDislikedContent
        {
            get
            {
                bool fDontSyncHatedContent = false;
                if (this.IsValid)
                    this._device.Rules.GetDontSyncHatedContent(ref fDontSyncHatedContent);
                return fDontSyncHatedContent;
            }
            set
            {
                if (!this.IsValid || this.ExcludeDislikedContent == value)
                    return;
                this._device.Rules.SetDontSyncHatedContent(value);
                this.FirePropertyChanged(nameof(ExcludeDislikedContent));
            }
        }

        private bool ReverseSyncPicsFromPhone
        {
            get
            {
                bool bReverseSync = false;
                if (this.IsValid)
                    this._device.GetPhotoVideoReverseSync(ref bReverseSync);
                return bReverseSync;
            }
            set
            {
                if (!this.IsValid || this.ReverseSyncPicsFromPhone == value)
                    return;
                this._device.SetPhotoVideoReverseSync(value);
                this.FirePropertyChanged(nameof(ReverseSyncPicsFromPhone));
            }
        }

        public bool DeletePicsFromPhoneAfterReverseSync
        {
            get
            {
                bool bDeleteAfterSync = false;
                if (this.IsValid)
                    this._device.GetDeletePhotoVideoAfterReverseSync(ref bDeleteAfterSync);
                return bDeleteAfterSync;
            }
            set
            {
                if (!this.IsValid || this.DeletePicsFromPhoneAfterReverseSync == value)
                    return;
                this._device.SetDeletePhotoVideoAfterReverseSync(value);
                this.FirePropertyChanged(nameof(DeletePicsFromPhoneAfterReverseSync));
            }
        }

        public ETranscodePhotoSetting ImageTranscodeQuality
        {
            get
            {
                ETranscodePhotoSetting ePhotoSetting = ETranscodePhotoSetting.tsPhotoSettingInvalid;
                if (this.IsValid)
                    this._device.GetPhotoTranscodeSetting(ref ePhotoSetting);
                return ePhotoSetting;
            }
            set
            {
                if (!this.IsValid || this.ImageTranscodeQuality == value)
                    return;
                this._device.SetPhotoTranscodeSetting(value);
                this.FirePropertyChanged(nameof(ImageTranscodeQuality));
            }
        }

        public string CameraRollDestinationPath
        {
            get
            {
                string empty = string.Empty;
                if (this.IsValid)
                    this._device.GetCameraRollDestinationFolder(ref empty);
                return empty;
            }
            set
            {
                if (!this.IsValid)
                    return;
                this._device.SetCameraRollDestinationFolder(value);
                this.FirePropertyChanged(nameof(CameraRollDestinationPath));
            }
        }

        public string SavedFolderDestinationPath
        {
            get
            {
                string empty = string.Empty;
                if (this.IsValid)
                    this._device.GetSavedDestinationFolder(ref empty);
                return empty;
            }
            set
            {
                if (!this.IsValid)
                    return;
                this._device.SetSavedDestinationFolder(value);
                this.FirePropertyChanged(nameof(SavedFolderDestinationPath));
            }
        }

        public int AudioTranscodeLimit
        {
            get
            {
                int audioThresholdBitRate = 0;
                int audioTargetBitRate = 0;
                if (this.IsValid)
                    this._device.GetAudioTranscodeParams(ref audioThresholdBitRate, ref audioTargetBitRate);
                return audioThresholdBitRate;
            }
            set
            {
                if (!this.IsValid)
                    return;
                this._device.SetAudioTranscodeParams(value, this.AudioTranscodeTarget);
                this.FirePropertyChanged(nameof(AudioTranscodeLimit));
            }
        }

        public int AudioTranscodeTarget
        {
            get
            {
                int audioThresholdBitRate = 0;
                int audioTargetBitRate = 0;
                if (this.IsValid)
                    this._device.GetAudioTranscodeParams(ref audioThresholdBitRate, ref audioTargetBitRate);
                return audioTargetBitRate;
            }
            set
            {
                if (!this.IsValid)
                    return;
                this._device.SetAudioTranscodeParams(this.AudioTranscodeLimit, value);
                this.FirePropertyChanged(nameof(AudioTranscodeTarget));
            }
        }

        public bool OptimizeVideoForTV
        {
            get
            {
                ETranscodeOptimization transcodeOptimization = ETranscodeOptimization.toOptimizeForSize;
                if (this.IsValid)
                    this._device.GetVideoTranscodeOptimization(ref transcodeOptimization);
                return transcodeOptimization == ETranscodeOptimization.toOptimizeForQuality;
            }
            set
            {
                if (!this.IsValid || this.OptimizeVideoForTV == value)
                    return;
                if (value)
                    this._device.SetVideoTranscodeOptimization(ETranscodeOptimization.toOptimizeForQuality);
                else
                    this._device.SetVideoTranscodeOptimization(ETranscodeOptimization.toOptimizeForSize);
                this.FirePropertyChanged(nameof(OptimizeVideoForTV));
            }
        }

        public bool PurchaseEnabled
        {
            get
            {
                bool purchaseEnabled = false;
                if (this.IsConnectedToClient)
                    this._device.GetPurchaseEnabled(ref purchaseEnabled);
                return purchaseEnabled;
            }
            set
            {
                if (!this.IsConnectedToClient || this.PurchaseEnabled == value)
                    return;
                this._device.SetPurchaseEnabled(value);
                this.FirePropertyChanged(nameof(PurchaseEnabled));
            }
        }

        public int PercentReserved
        {
            get
            {
                uint ulPercentage = 0;
                if (this.IsValid)
                    this._device.GetPercentSpaceReserved(ref ulPercentage);
                return (int)ulPercentage;
            }
            set
            {
                if (!this.IsValid)
                    return;
                this._device.SetPercentSpaceReserved((uint)value);
                this.FirePropertyChanged(nameof(PercentReserved));
            }
        }

        public bool PromptForAccountLinkage
        {
            get
            {
                bool bPromptLink = false;
                if (this.IsValid)
                    this._device.GetPromptLink(ref bPromptLink);
                return bPromptLink;
            }
            set
            {
                if (!this.IsValid)
                    return;
                this._device.SetPromptLink(value);
                this.FirePropertyChanged(nameof(PromptForAccountLinkage));
            }
        }

        public bool RequiresClientUpdate => this.IsConnectedToClient && this._device.ClientUpdateRequired;

        public bool RequiresFirmwareUpdate => this.IsConnectedToClient && this._device.FirmwareUpdateRequired;

        public UIFirmwareUpdater UIFirmwareUpdater
        {
            get
            {
                if (this._firmwareUpdater == null && this.IsValid && (this._device.FirmwareUpdater != null && this.IsConnectedToClientPhysically))
                    this.UIFirmwareUpdater = new UIFirmwareUpdater(this, this._device.FirmwareUpdater);
                return this._firmwareUpdater;
            }
            private set
            {
                if (this._firmwareUpdater == value)
                    return;
                this._firmwareUpdater = value;
                this.FirePropertyChanged(nameof(UIFirmwareUpdater));
            }
        }

        public UIFirmwareRestorer UIFirmwareRestorer
        {
            get
            {
                if (this._firmwareRestorer == null && this.IsValid && (this._device.FirmwareUpdater != null && this.IsConnectedToClientPhysically))
                    this.UIFirmwareRestorer = new UIFirmwareRestorer(this._device.FirmwareUpdater.Restorer, this.RequiresAutoRestore);
                return this._firmwareRestorer;
            }
            private set
            {
                this._firmwareRestorer = value;
                this.FirePropertyChanged(nameof(UIFirmwareRestorer));
            }
        }

        public string FirmwareVersion
        {
            get
            {
                string empty = string.Empty;
                if (this.IsConnectedToClient)
                    this._device.GetFirmwareVersion(ref empty);
                return empty;
            }
        }

        public bool IsSyncing
        {
            get => this._currentlySyncing;
            private set
            {
                if (!this.IsValid || this._currentlySyncing == value)
                    return;
                this._currentlySyncing = value;
                this.FirePropertyChanged(nameof(IsSyncing));
            }
        }

        public SyncNotification SyncProgress
        {
            get => this._syncProgress;
            private set
            {
                if (!this.IsValid || this._syncProgress == value)
                    return;
                this._syncProgress = value;
                this.FirePropertyChanged(nameof(SyncProgress));
            }
        }

        public Command SyncBegun => this._syncBegun;

        public Command SyncProgressed => this._syncProgressed;

        public Command SyncCompleted => this._syncCompleted;

        private Microsoft.Iris.Timer FormatSanityTimer
        {
            get
            {
                if (this._formatSanityTimer == null)
                {
                    this._formatSanityTimer = new Microsoft.Iris.Timer((IModelItemOwner)this);
                    this._formatSanityTimer.Interval = 60000;
                    this._formatSanityTimer.AutoRepeat = false;
                    this._formatSanityTimer.Tick += new EventHandler(this.FormatSanityFailed);
                }
                return this._formatSanityTimer;
            }
        }

        public DateTime LastConnectTime
        {
            get
            {
                DateTime dateTime = DateTime.MinValue;
                if (this.IsValid)
                    dateTime = this._device.LastConnectTime;
                return dateTime;
            }
        }

        public string LastSyncTime
        {
            get
            {
                string str = string.Empty;
                if (this.IsValid)
                {
                    DateTime lastSyncTime = this._device.LastSyncTime;
                    if (lastSyncTime.Year >= 2006)
                        str = !(lastSyncTime.Date == DateTime.Now.Date) ? lastSyncTime.ToString("d") : lastSyncTime.ToString("t");
                }
                return str;
            }
        }

        public string LastConnectTimestring
        {
            get => this._lastConnectTimestring ?? string.Empty;
            private set
            {
                if (!(this._lastConnectTimestring != value))
                    return;
                this._lastConnectTimestring = value;
                this.FirePropertyChanged(nameof(LastConnectTimestring));
            }
        }

        public string LastSyncStartTimestring
        {
            get => this._lastSyncStartTimestring ?? this.LastConnectTimestring;
            private set
            {
                if (!(this._lastSyncStartTimestring != value))
                    return;
                this._lastSyncStartTimestring = value;
                this.FirePropertyChanged(nameof(LastSyncStartTimestring));
            }
        }

        public UIGasGauge ActualGasGauge
        {
            get => this._actualGasGauge;
            private set
            {
                if (this._actualGasGauge == value)
                    return;
                this._actualGasGauge = value;
                this.FirePropertyChanged(nameof(ActualGasGauge));
            }
        }

        public UIGasGauge PredictedGasGauge
        {
            get => this._predictedGasGauge;
            private set
            {
                if (this._predictedGasGauge == value)
                    return;
                this._predictedGasGauge = value;
                this.FirePropertyChanged(nameof(PredictedGasGauge));
            }
        }

        public bool EnableWatson
        {
            get
            {
                uint dwWatsonSetting = 0;
                if (this.IsConnectedToClient)
                    this._device.GetWatsonSetting(ref dwWatsonSetting);
                return dwWatsonSetting > 0U;
            }
            set
            {
                if (!this.IsConnectedToClient || this.EnableWatson == value)
                    return;
                this._device.SetWatsonSetting(value ? 1U : 0U);
                this.FirePropertyChanged(nameof(EnableWatson));
            }
        }

        public SyncMode GetSyncMode(SyncCategory schema) => this.GetSyncMode(schema, false);

        internal SyncMode GetSyncMode(SyncCategory schema, bool fEstablishingPartnership)
        {
            ESyncMode mode = ESyncMode.eSyncModeInvalid;
            if (this.IsValid && this.SupportsSyncCategory(schema))
                this._device.Rules.GetCategorySyncMode((ESyncCategory)schema, ref mode, fEstablishingPartnership);
            return (SyncMode)mode;
        }

        public void SetSyncMode(SyncCategory schema, SyncMode mode)
        {
            if (!this.IsValid)
                return;
            this._device.Rules.SetCategorySyncMode((ESyncCategory)schema, (ESyncMode)mode);
        }

        public bool IsManualFor(MediaType type) => this.IsManualFor(UIDeviceList.MapMediaTypeToSyncCategory(type));

        public bool IsManualFor(SyncCategory type) => this.IsManualFor(type, false);

        internal bool IsManualFor(SyncCategory type, bool fEstablishingPartnership) => this.GetSyncMode(type, fEstablishingPartnership) == SyncMode.Manual;

        public bool IsSyncAllFor(MediaType schema) => this.IsSyncAllFor(UIDeviceList.MapMediaTypeToSyncCategory(schema));

        public bool IsSyncAllFor(SyncCategory schema) => this.IsSyncAllFor(schema, false);

        internal bool IsSyncAllFor(SyncCategory schema, bool fEstablishingPartnership) => this.GetSyncMode(schema, fEstablishingPartnership) == SyncMode.SyncAll;

        public void AddSyncRule(IList items)
        {
            throw new NotImplementedException();
            /*// ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            UIDevice.c__DisplayClass6 cDisplayClass6_1 = new UIDevice.c__DisplayClass6();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass6_1.items = items;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass6_1.<>4__this = this;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (cDisplayClass6_1.items == null || cDisplayClass6_1.items.Count <= 0 || !this.IsValid)
              return;
            TypeDiscoveringSyncEventArgs args = new TypeDiscoveringSyncEventArgs();
            args.Device = this._device;
            long startingSize = this.ActualGasGauge.UsedSpace;
            // ISSUE: reference to a compiler-generated field
            this.MaterializeMarketplaceMedia(cDisplayClass6_1.items);
            // ISSUE: reference to a compiler-generated field
            MediaIdAndType[] threadSafeItems = this.GenerateThreadSafeDatabaseItems(cDisplayClass6_1.items);
            ThreadPool.QueueUserWorkItem((WaitCallback) delegate
            {
              // ISSUE: variable of a compiler-generated type
              UIDevice.c__DisplayClass6 cDisplayClass6 = cDisplayClass6_1;
              bool actionSucceeded = LibraryDataProvider.ActOnItems((IList) threadSafeItems, BulkItemAction.AddSyncRules, (EventArgs) args);
              Application.DeferredInvoke((DeferredInvokeHandler) delegate
              {
                if (actionSucceeded)
                {
                  string message = (string) null;
                  MediaType mediaType = args.ContainedTypes == null || args.ContainedTypes.Count != 1 ? MediaType.Undefined : (MediaType) args.ContainedTypes[0];
                  // ISSUE: reference to a compiler-generated field
                  if (mediaType == MediaType.Playlist && cDisplayClass6.items[0] is IDatabaseMedia databaseMedia)
                  {
                    int mediaId;
                    databaseMedia.GetMediaIdAndType(out mediaId, out EMediaTypes _);
                    if (PlaylistManager.IsChannel(mediaId))
                    {
                      // ISSUE: reference to a compiler-generated field
                      message = UIDevice._channelNewRuleMessage.GetMessageForCount(cDisplayClass6.items.Count);
                    }
                  }
                  if (message == null)
                  {
                    foreach (UIDevice.INewRuleMessageType newRuleMessageType in UIDevice._newRuleMessageLookupTable)
                    {
                      if (newRuleMessageType.Type == mediaType)
                      {
                        // ISSUE: reference to a compiler-generated field
                        message = newRuleMessageType.GetMessageForCount(cDisplayClass6.items.Count);
                        break;
                      }
                    }
                  }
                  if (message == null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    message = UIDevice._genericNewRuleMessage.GetMessageForCount(cDisplayClass6.items.Count);
                  }
                  // ISSUE: reference to a compiler-generated field
                  NotificationArea.Instance.Override((Notification) new SyncNewRuleAddedNotification(message, startingSize, cDisplayClass6.<>4__this.PredictedGasGauge));
                }
                // ISSUE: reference to a compiler-generated field
                cDisplayClass6.<>4__this.BeginSync(true, false);
              }, (object) null);
            }, (object) null);*/
        }

        public void AddPlaylistSyncRule(int dbPlaylistId) => this.AddSyncRule((IList)new MediaIdAndType[1]
        {
      new MediaIdAndType(dbPlaylistId, MediaType.Playlist)
        });

        public void RemoveSyncRule(IList items)
        {
            if (items == null || items.Count <= 0 || !this.IsValid)
                return;
            SyncEventArgs args = new SyncEventArgs();
            args.Device = this._device;
            MediaIdAndType[] threadSafeItems = this.GenerateThreadSafeDatabaseItems(items);
            ThreadPool.QueueUserWorkItem((WaitCallback)delegate
           {
               LibraryDataProvider.ActOnItems((IList)threadSafeItems, BulkItemAction.RemoveSyncRules, (EventArgs)args);
               Application.DeferredInvoke((DeferredInvokeHandler)delegate
         {
                 this.BeginSync(true, false);
             }, (object)null);
           }, (object)null);
        }

        public void Exclude(IList items)
        {
            if (items == null || items.Count <= 0 || !this.IsValid)
                return;
            SyncEventArgs args = new SyncEventArgs();
            args.Device = this._device;
            MediaIdAndType[] threadSafeItems = this.GenerateThreadSafeDatabaseItems(items);
            bool shouldBeginSyncAfter = false;
            foreach (object obj in (IEnumerable)items)
            {
                if (obj is LibraryDataProviderItemBase providerItemBase)
                {
                    object property = providerItemBase.GetProperty("SyncState");
                    if (property != null)
                    {
                        switch ((ESyncState)property)
                        {
                            case ESyncState.eSyncStateCurrentlySyncing:
                            case ESyncState.eSyncStateOnDevice:
                                break;
                            default:
                                continue;
                        }
                    }
                }
                shouldBeginSyncAfter = true;
                break;
            }
            ThreadPool.QueueUserWorkItem((WaitCallback)delegate
           {
               LibraryDataProvider.ActOnItems((IList)threadSafeItems, BulkItemAction.ExcludeFromSync, (EventArgs)args);
               if (!shouldBeginSyncAfter)
                   return;
               Application.DeferredInvoke((DeferredInvokeHandler)delegate
         {
                 this.BeginSync(true, false);
             }, (object)null);
           }, (object)null);
        }

        public void Unexclude(IList items)
        {
            if (items == null || items.Count <= 0 || !this.IsValid)
                return;
            SyncEventArgs args = new SyncEventArgs();
            args.Device = this._device;
            MediaIdAndType[] threadSafeItems = this.GenerateThreadSafeDatabaseItems(items);
            ThreadPool.QueueUserWorkItem((WaitCallback)delegate
           {
               LibraryDataProvider.ActOnItems((IList)threadSafeItems, BulkItemAction.UnexcludeFromSync, (EventArgs)args);
               Application.DeferredInvoke((DeferredInvokeHandler)delegate
         {
                 this.BeginSync(true, false);
             }, (object)null);
           }, (object)null);
        }

        public void DeleteAndExclude(IList items)
        {
            if (items == null || items.Count <= 0 || !this.IsReadyForSync)
                return;
            DeferrableSyncEventArgs args = new DeferrableSyncEventArgs();
            args.Device = this._device;
            MediaIdAndType[] threadSafeItems = this.GenerateThreadSafeDatabaseItems(items);
            ThreadPool.QueueUserWorkItem((WaitCallback)delegate
           {
               LibraryDataProvider.ActOnItems((IList)threadSafeItems, BulkItemAction.DeleteFromDevice, (EventArgs)args);
           }, (object)null);
        }

        public SyncGroupList GenerateSyncGroupList(
          IModelItemOwner owner,
          bool expandSyncAllEntries)
        {
            SyncRulesView syncRulesView = (SyncRulesView)null;
            if (this.IsValid)
            {
                HRESULT snapshot = (HRESULT)this._device.Rules.GenerateSnapshot(expandSyncAllEntries, ref syncRulesView);
            }
            return new SyncGroupList(owner, this, syncRulesView, expandSyncAllEntries);
        }

        public PodcastSyncLimit GetPodcastSyncLimit(int podcastID)
        {
            EDeviceSyncRuleType ruleType = EDeviceSyncRuleType.eDeviceSyncRuleTypeInvalid;
            if (this.IsValid)
                this._device.Rules.GetSyncRuleForMedia(EMediaTypes.eMediaTypePodcastSeries, podcastID, ref ruleType);
            return (PodcastSyncLimit)ruleType;
        }

        public void SetPodcastSyncLimit(int podcastID, PodcastSyncLimit limit)
        {
            if (!this.IsValid)
                return;
            this._device.Rules.Add(new int[1] { podcastID }, EMediaTypes.eMediaTypePodcastSeries, (EDeviceSyncRuleType)limit);
        }

        public int GetPodcastSyncLimitWithValue(int podcastID)
        {
            int iValue = -1;
            if (this.IsValid && this._device != null && this._device.Rules != null)
                this._device.Rules.GetSyncRuleValueForMedia(podcastID, ref iValue);
            return iValue;
        }

        public void SetPodcastSyncLimitWithValue(int podcastID, int value)
        {
            if (!this.IsValid)
                return;
            int[] rgIds = new int[1] { podcastID };
            if (this._device == null || this._device.Rules == null)
                return;
            this._device.Rules.AddDeviceSyncRuleWithValue(rgIds, value);
        }

        public void BeginSync() => this.BeginSync(false, false);

        public void BeginSync(bool userInitiated, bool syncOnNextNotify)
        {
            if (!this.IsReadyForSync || this.IsLockedAgainstSyncing || !userInitiated && this.UserStoppedLastSync)
                return;
            if (syncOnNextNotify)
                this._device.StartSyncNextNotify();
            else
                this._device.StartSync();
        }

        public void EndSync() => this.EndSync(false);

        public void EndSync(bool userInitiated)
        {
            if (this.SyncProgress != null)
                this.SyncProgress.SyncCanceled = true;
            if (this.IsConnectedToClient)
                this._device.StopSync();
            if (!userInitiated)
                return;
            if (this.IsGuest)
                this._device.ClearRules();
            else
                this._device.ClearManualModeRules();
            this.UserStoppedLastSync = true;
        }

        public bool ReverseSync(IList items)
        {
            if (items != null && items.Count > 0 && (this.IsConnectedToClient && this.IsReadyForSync))
            {
                DeferrableSyncEventArgs deferrableSyncEventArgs = new DeferrableSyncEventArgs();
                deferrableSyncEventArgs.Device = this._device;
                LibraryDataProvider.ActOnItems(items, BulkItemAction.ReverseSync, (EventArgs)deferrableSyncEventArgs);
                if (deferrableSyncEventArgs.Status == ESyncOperationStatus.osDeferred)
                    return false;
            }
            return true;
        }

        public HRESULT Enumerate() => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.StartEnumeration();

        public HRESULT Format()
        {
            if (!this.IsConnectedToClient)
                return HRESULT._E_UNEXPECTED;
            this.IsFormatting = true;
            return (HRESULT)this._device.Format();
        }

        public HRESULT DeleteAllGuestContent()
        {
            if (!this.IsConnectedToClient)
                return HRESULT._E_UNEXPECTED;
            ESyncOperationStatus operationStatus = ESyncOperationStatus.osInvalid;
            return (HRESULT)this._device.DeleteAllGuestContent(ref operationStatus);
        }

        public HRESULT ForceAppUpdate() => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.ForceAppUpdate();

        public HRESULT ClearAccountAssociation()
        {
            HRESULT hresult = HRESULT._E_UNEXPECTED;
            if (this.IsConnectedToClient)
            {
                hresult = (HRESULT)this._device.ClearUserGuidandZuneTag();
                int num = this.HasFailedLogin ? 1 : 0;
                this.FirePropertyChanged("UserId");
                this.FirePropertyChanged("ZuneTag");
                this.PurchaseEnabled = false;
            }
            return hresult;
        }

        public HRESULT AssociateWithAccount(Guid guid, string tag)
        {
            HRESULT hresult = HRESULT._E_UNEXPECTED;
            if (this.IsConnectedToClient)
            {
                hresult = (HRESULT)this._device.SetUserGuidandZuneTag(guid, tag);
                if (hresult.IsSuccess)
                    hresult = this.SetGeoId();
                int num = this.HasFailedLogin ? 1 : 0;
                this.FirePropertyChanged("UserId");
                this.FirePropertyChanged("ZuneTag");
            }
            return hresult;
        }

        public void SendMarketplaceCredentials(string username, string password)
        {
            if (!this.IsConnectedToClient)
                return;
            SecureString secureUsername = ZuneUI.Shell.MakeSecureString(username, true);
            SecureString securePassword = ZuneUI.Shell.MakeSecureString(password, true);
            ThreadPool.QueueUserWorkItem((WaitCallback)delegate
           {
               this._device.SetMarketplaceCredentials(secureUsername, securePassword);
           });
        }

        public HRESULT SetGeoId()
        {
            HRESULT hresult = HRESULT._E_UNEXPECTED;
            if (this.IsValid)
            {
                uint num = CultureHelper.GeoId();
                if (!CultureHelper.IsValidGeoId(num))
                    num = 0U;
                hresult = (HRESULT)this._device.SetGeoId(num);
            }
            return hresult;
        }

        public HRESULT RemoveWiFiAssociation() => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.UnassociateWlanDevice();

        public HRESULT SetWiFiProfileList(WlanProfileList list) => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.SetWlanProfileList(list);

        public HRESULT GetWiFiProfileList(ref WlanProfileList list) => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.GetWlanProfileList(ref list);

        public HRESULT SendWiFiProfiles() => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.SetDeviceWlanProfiles();

        public HRESULT ReceiveWiFiProfiles() => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.GetDeviceWlanProfiles();

        public HRESULT TestWiFi() => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.TestDeviceWlan();

        public HRESULT CancelWiFiTest() => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.CancelTestDeviceWlan();

        public HRESULT AssociateWiFi() => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.AssociateWlanDevice();

        public HRESULT LoadComputerWiFiProfiles() => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.GetWlanProfiles();

        public HRESULT ScanForWiFiNetworks() => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.GetDeviceWlanNetworks();

        public HRESULT GetDisconnectedWiFiUUID(ref string uuid) => !this.IsValid ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.GetDisconnectedWlanDeviceUuid(ref uuid);

        public HRESULT UnassociateWiFiUUID(string uuid) => !this.IsValid ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.UnassociateWlanDeviceUuid(uuid);

        public bool WirelessEnableRequiresElevation()
        {
            bool bEnabled = true;
            if (this.IsValid)
                this._device.IsWlanFirewallEnabled(ref bEnabled);
            return Environment.OSVersion.Version.Major >= 6 && !bEnabled;
        }

        public HRESULT GetWiFiAuthorizationCipherList(ref WlanAuthCipherPairList list) => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.GetWlanDeviceAuthCipherPairList(ref list);

        public HRESULT GetWifiConnectedSSID(ref string connectedSSID) => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.GetDeviceWlanConnectedSSID(ref connectedSSID);

        public HRESULT IsWlanDeviceDisabled(ref bool disabled) => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.IsWlanDeviceDisabled(ref disabled);

        public HRESULT GetWifiMediaSyncSSID(ref string mediaSyncSSID) => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.GetDeviceWlanMediaSyncSSID(ref mediaSyncSSID);

        public HRESULT SetWifiMediaSyncSSID(string mediaSyncSSID) => !this.IsConnectedToClient ? HRESULT._E_UNEXPECTED : (HRESULT)this._device.SetDeviceWlanMediaSyncSSID(mediaSyncSSID);

        public override bool Equals(object obj) => obj is UIDevice uiDevice ? this._device == uiDevice._device : base.Equals(obj);

        public override int GetHashCode() => this._device == null ? 0 : this._device.GetHashCode();

        public bool SupportsRentalOfVideo(bool isHD)
        {
            if (!this.SupportsRental)
                return false;
            return !isHD || this.SupportsHD;
        }

        public bool SupportsBrandingType(DeviceBranding brand)
        {
            bool flag = false;
            switch (this.Class)
            {
                case DeviceClass.Invalid:
                case DeviceClass.Reserved:
                    return flag;
                case DeviceClass.WindowsPhone:
                    flag = brand == DeviceBranding.WindowsPhone;
                    goto case DeviceClass.Invalid;
                case DeviceClass.Kin:
                    flag = brand == DeviceBranding.Kin;
                    goto case DeviceClass.Invalid;
                default:
                    flag = brand == DeviceBranding.Zune;
                    goto case DeviceClass.Invalid;
            }
        }

        private void OnNameChange(Device device, string newName) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed)
               return;
           this.FirePropertyChanged("Name");
           this.FirePropertyChanged("NameXMLEscaped");
       }, (object)null);

        private void OnDeviceOverfill(GasGauge gasGauge) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed)
               return;
           this.EndSync();
           if (this.IsGuest)
               this._device.ClearRules();
           ZuneShell shell = ZuneShell.DefaultInstance;
           if (shell.CurrentPage is PlaybackPage)
               shell.NavigateBack();
           if (shell.CurrentPage is DialogPage || UIDevice._isOosDialogVisible)
               return;
           UIDevice._isOosDialogVisible = true;
           if (this.IsGuest)
           {
               MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_DEVICE_OUT_OF_SPACE_TITLE), string.Format(ZuneUI.Shell.LoadString(StringId.IDS_GUEST_OUT_OF_SPACE_MESSAGE), (object)this.Name), (EventHandler)null, (EventHandler)null, (EventHandler)null, (EventHandler)delegate
       {
               UIDevice._isOosDialogVisible = false;
           });
           }
           else
           {
               Command okCommand = new Command((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_HANDLE_DEVICE_OUT_OF_SPACE_BUTTON), (EventHandler)delegate
          {
               shell.NavigateToPage((ZunePage)new DeviceOverfillLand(this));
               UIDevice._isOosDialogVisible = false;
           });
               MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_DEVICE_OUT_OF_SPACE_TITLE), string.Format(ZuneUI.Shell.LoadString(StringId.IDS_DEVICE_OUT_OF_SPACE_MESSAGE), (object)this.Name), okCommand, (string)null, (EventHandler)delegate
         {
               UIDevice._isOosDialogVisible = false;
           }, false);
           }
       }, (object)null);

        private void OnDeviceStatusChanged(Device device, int rawHR, EEndpointStatus endpointStatus) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed)
               return;
           this.HandleDeviceState(endpointStatus, (HRESULT)rawHR);
       }, (object)null);

        private void OnFormatPhase1Completed(Device device, int rawHR) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed)
               return;
           HRESULT hr = (HRESULT)rawHR;
           if (hr.IsError)
               this.OnFormatCompleted(hr);
           else
               this.FormatSanityTimer.Start();
       }, (object)null);

        private void OnSyncBegun(Device device) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed)
               return;
           this.SyncProgress = new SyncNotification(this);
           this.IsSyncing = true;
           this.SyncBegun.Invoke();
           this.SyncProgressed.Invoke();
           if (this.IsLockedAgainstSyncing)
               this.EndSync();
           else
               this.UserStoppedLastSync = false;
           this.LastSyncStartTimestring = this.GenerateSyncTimestring();
           this.FirePropertyChanged("LastSyncTime");
       }, (object)null);

        private void OnSyncProgressed(
          Device device,
          uint percent,
          uint percentItem,
          uint percentTitle,
          string group,
          string title,
          ESyncEngineState engineState)
        {
            Application.DeferredInvoke((DeferredInvokeHandler)delegate
           {
               if (this.IsDisposed)
                   return;
               if (this.SyncProgress != null)
                   this.SyncProgress.UpdateProgress((int)percent, (int)percentItem, (int)percentTitle, group, title, engineState);
               this.SyncProgressed.Invoke();
           }, (object)null);
        }

        private void OnSyncCompleted(Device device, ESyncEventReason reason) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed)
               return;
           if (this.SyncProgress != null)
           {
               this.SyncProgress.Complete(reason);
               this.SyncProgress = (SyncNotification)null;
               this.LastSyncError = reason != ESyncEventReason.eSyncEventSucceeded ? HRESULT._E_FAIL : HRESULT._S_OK;
           }
           this.IsSyncing = false;
           this.SyncProgressed.Invoke();
           this.SyncCompleted.Invoke();
           this.FirePropertyChanged("LastSyncTime");
       }, (object)null);

        private void FormatSanityFailed(object sender, EventArgs e)
        {
            if (!this.IsFormatting)
                return;
            this.OnFormatCompleted(HRESULT._E_FAIL);
            SingletonModelItem<UIDeviceList>.Instance.HideDevice(this);
        }

        private void OnUnassociateWlanCompleted(Device device, int hr) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed)
               return;
           this.UpdateWirelessSyncEnabled();
           if (this.WiFiRemovalCompletedEvent == null)
               return;
           this.WiFiRemovalCompletedEvent((object)this, new FallibleEventArgs((HRESULT)hr));
       }, (object)null);

        private void OnSetDeviceWlanProfilesCompleted(Device device, int hr) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed)
               return;
           this.UpdateWirelessSyncEnabled();
           if (this.WiFiProfilesSentEvent == null)
               return;
           this.WiFiProfilesSentEvent((object)this, new FallibleEventArgs((HRESULT)hr));
       }, (object)null);

        private void OnGetDeviceWlanProfilesCompleted(Device device, int hr) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed || this.WiFiProfilesReceivedEvent == null)
               return;
           this.WiFiProfilesReceivedEvent((object)this, new FallibleEventArgs((HRESULT)hr));
       }, (object)null);

        private void OnTestWlanCompleted(Device device, WlanTestResultCode result, int hr) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           this.UpdateWirelessSyncEnabled();
           if (this.IsDisposed || this.WiFiTestCompletedEvent == null)
               return;
           this.WiFiTestCompletedEvent((object)this, new FallibleEventArgs((HRESULT)hr));
       }, (object)null);

        private void OnAssociateWlanCompleted(Device device, int hr) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           this.UpdateWirelessSyncEnabled();
           if (this.IsDisposed || this.WiFiAssociationCompletedEvent == null)
               return;
           this.WiFiAssociationCompletedEvent((object)this, new FallibleEventArgs((HRESULT)hr));
       }, (object)null);

        private void OnGetWlanProfilesCompleted(Device device, int hr) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed || this.ComputerWiFiProfilesLoadedEvent == null)
               return;
           this.ComputerWiFiProfilesLoadedEvent((object)this, new FallibleEventArgs((HRESULT)hr));
       }, (object)null);

        private void OnGetDeviceWlanVisibleNetworksCompleted(Device device, int hr) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this.IsDisposed || this.WiFiScanCompletedEvent == null)
               return;
           this.WiFiScanCompletedEvent((object)this, new FallibleEventArgs((HRESULT)hr));
       }, (object)null);

        private void ReleaseFirmwareObjects()
        {
            this.UIFirmwareUpdater = (UIFirmwareUpdater)null;
            this.UIFirmwareRestorer = (UIFirmwareRestorer)null;
        }

        private void MaterializeMarketplaceMedia(IList items)
        {
            List<int> intList = new List<int>(items.Count);
            for (int index = 0; index < items.Count; ++index)
            {
                if (items[index] is DataProviderObject dataProviderObject)
                {
                    switch (dataProviderObject)
                    {
                        case Track _:
                            Track track = (Track)dataProviderObject;
                            int dbMediaId1 = -1;
                            if (!ZuneApplication.Service.InCompleteCollection(track.Id, Microsoft.Zune.Service.EContentType.MusicTrack, out dbMediaId1, out bool _) && (track.IsDownloading || track.CanDownload || track.CanPurchase))
                                dbMediaId1 = ZuneApplication.ZuneLibrary.AddTrack(track.Id, track.AlbumId, track.TrackNumber, track.Title, track.Duration, track.AlbumTitle, track.Artist, track.PrimaryGenre.Title);
                            if (dbMediaId1 >= 0)
                            {
                                track.LibraryId = dbMediaId1;
                                continue;
                            }
                            continue;
                        case Album _:
                            Album album = (Album)dataProviderObject;
                            int num = ZuneApplication.ZuneLibrary.AddAlbum(album.Id, album.Title, album.Artist);
                            if (num >= 0)
                            {
                                album.LibraryId = num;
                                continue;
                            }
                            continue;
                        case PodcastSeries _:
                            PodcastSeries podcastSeries = (PodcastSeries)dataProviderObject;
                            string sourceUrl = podcastSeries.SourceUrl;
                            if (!string.IsNullOrEmpty(sourceUrl))
                            {
                                SubscriptionState subscriptionState = ZuneShell.DefaultInstance.Management.GetSubscriptionState(sourceUrl, EMediaTypes.eMediaTypePodcastSeries);
                                if (subscriptionState != null && !subscriptionState.IsSubscribed)
                                {
                                    ZuneShell.DefaultInstance.Management.SubscribeToPodcastFeed(sourceUrl, podcastSeries.Title, podcastSeries.Id, ESubscriptionSource.eSubscriptionSourceZMP);
                                    continue;
                                }
                                continue;
                            }
                            continue;
                        case MusicVideo _:
                            MusicVideo musicVideo = (MusicVideo)dataProviderObject;
                            int dbMediaId2 = -1;
                            if (!ZuneApplication.Service.InCompleteCollection(musicVideo.Id, Microsoft.Zune.Service.EContentType.Video, out dbMediaId2, out bool _) && (musicVideo.IsDownloading || musicVideo.CanPurchase))
                                dbMediaId2 = ZuneApplication.ZuneLibrary.AddVideo(musicVideo.Id, musicVideo.Title, musicVideo.Duration);
                            if (dbMediaId2 >= 0)
                            {
                                musicVideo.LibraryId = dbMediaId2;
                                continue;
                            }
                            continue;
                        default:
                            continue;
                    }
                }
            }
        }

        private void HandleDeviceState(EEndpointStatus state, HRESULT hr)
        {
            bool isValid = this.IsValid;
            bool isConnectedToPc = this.IsConnectedToPC;
            bool connectedToClient = this.IsConnectedToClient;
            bool connectedToSideloader = this.IsConnectedToSideloader;
            bool isReadyForSync = this.IsReadyForSync;
            this._lastDeviceState = state;
            if (isValid != this.IsValid)
            {
                this.ReverseSyncPicsFromPhone = true;
                this.FirePropertyChanged("IsValid");
            }
            if (isConnectedToPc != this.IsConnectedToPC)
            {
                this.FirePropertyChanged("IsConnectedToPC");
                if (!FeatureEnablement.IsFeatureEnabled(Features.eDevice))
                {
                    ClientConfiguration.FeaturesOverride.Device = 1;
                    FeatureEnablement.ForceFeatureOn(Features.eDevice);
                }
                Application.DeferredInvoke((DeferredInvokeHandler)delegate
               {
                   ZuneUI.Shell.MainFrame.Device.UpdateShowDevice();
                   ZuneUI.Shell.SettingsFrame.Settings.ShowDevice(true);
               }, (object)null);
            }
            if (connectedToClient != this.IsConnectedToClient)
            {
                if (this.IsConnectedToClient)
                {
                    this.ReloadIconSet();
                    if (this.IsFormatting)
                        this.OnFormatCompleted(HRESULT._S_OK);
                }
                if (!this.ShouldSuppressConnectionNotifications())
                {
                    this.OnConnectivityChanged();
                    this.FirePropertyChanged("IsConnectedToClient");
                    this.FirePropertyChanged("IsConnectedToClientWirelessly");
                    this.FirePropertyChanged("IsConnectedToClientPhysically");
                    if (isReadyForSync != this.IsReadyForSync)
                        this.FirePropertyChanged("IsReadyForSync");
                }
            }
            if (connectedToSideloader != this.IsConnectedToSideloader)
            {
                this.FirePropertyChanged("IsConnectedToSideloader");
                this.FirePropertyChanged("OwnerApplicationName");
            }
            if (!this.IsConnectedToPC)
                this.IsReadyForSync = false;
            else if (state >= EEndpointStatus.eEndpointStatusReadyForSync)
                this.IsReadyForSync = true;
            switch (state - 6)
            {
                case EEndpointStatus.eEndpointStatusUndefined:
                    this.HidePinUnlockDialog(false);
                    MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_PHONE_TLS_ERROR_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_PHONE_TLS_ERROR_TEXT), (EventHandler)null);
                    break;
                case EEndpointStatus.eEndpointStatusNotPresent:
                    this.ShowPinUnlockDialog();
                    break;
                case EEndpointStatus.eEndpointStatusHidden:
                    this.HidePinUnlockDialog(true);
                    break;
                case EEndpointStatus.eEndpointStatusAuthenticationRequired:
                    if (!hr.IsSuccess)
                    {
                        MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_DEVICE_ENUMERATION_FAILED_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_DEVICE_ENUMERATION_FAILED_BODY), (EventHandler)null);
                        SingletonModelItem<UIDeviceList>.Instance.HideDevice(this);
                    }
                    if (this.EnumeratedEvent == null)
                        break;
                    this.EnumeratedEvent((object)this, new FallibleEventArgs(hr));
                    break;
                case EEndpointStatus.eEndpointStatusAuthenticationCompleted:
                    if (this.IsFormatting)
                    {
                        this.OnFormatCompleted(HRESULT._S_OK);
                        break;
                    }
                    this.BeginSync();
                    if (!this.SupportsPaidApplications)
                        break;
                    this.ForceAppUpdate();
                    break;
                case EEndpointStatus.eEndpointStatusDetectingProxy:
                    MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_PHONE_NO_IP_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_PHONE_NO_IP_TEXT), (EventHandler)null);
                    break;
            }
        }

        private void OnConnectivityChanged()
        {
            if (this.IsConnectedToClient)
            {
                HRESULT hresult = HRESULT._S_OK;
                if (this.UserGuid != Guid.Empty)
                {
                    int hrLogin = 0;
                    if (((HRESULT)this._device.GetAndResetLastLoginError(ref hrLogin)).IsSuccess)
                        hresult = (HRESULT)hrLogin;
                }
                this.LastFailedLoginError = hresult;
            }
            else
            {
                if (this.SyncProgress != null)
                {
                    this.SyncProgress.SyncCanceled = true;
                    this.SyncProgress.Complete(ESyncEventReason.eSyncEventSucceeded);
                    this.SyncProgress = (SyncNotification)null;
                }
                this.ReleaseFirmwareObjects();
            }
            if (this.ActualGasGauge != null)
                this.ActualGasGauge.Dispose();
            if (this.PredictedGasGauge != null)
                this.PredictedGasGauge.Dispose();
            this.ActualGasGauge = new UIGasGauge((IModelItemOwner)this, this._device.ActualGasGauge);
            this.PredictedGasGauge = new UIGasGauge((IModelItemOwner)this, this._device.PredictedGasGauge);
            if (this._device.PredictedGasGauge != null)
            {
                this._device.PredictedGasGauge.DeviceOverflowEvent -= new DeviceOverflowHandler(this.OnDeviceOverfill);
                this._device.PredictedGasGauge.DeviceOverflowEvent += new DeviceOverflowHandler(this.OnDeviceOverfill);
            }
            this.UpdateWirelessSyncEnabled();
            this.FirePropertyChanged("CanonicalName");
            this.FirePropertyChanged("ZuneTag");
            this.FirePropertyChanged("RequiresClientUpdate");
            this.FirePropertyChanged("RequiresFirmwareUpdate");
            this.FirePropertyChanged("FirmwareVersion");
            this.UserStoppedLastSync = false;
            this.LastConnectTimestring = this.GenerateSyncTimestring();
        }

        private void OnFormatCompleted(HRESULT hr)
        {
            this.FormatSanityTimer.Stop();
            this.IsFormatting = false;
            if (this.FormatCompletedEvent == null)
                return;
            this.FormatCompletedEvent((object)this, new FallibleEventArgs(hr));
        }

        private bool ShouldSuppressConnectionNotifications()
        {
            if (this.IsFormatting || this.UIFirmwareUpdater != null && this.UIFirmwareUpdater.UpdateInProgress)
                return true;
            return this.UIFirmwareRestorer != null && this.UIFirmwareRestorer.RestoreInProgress;
        }

        private void UpdateWirelessSyncEnabled()
        {
            bool flag = false;
            if (this.IsConnectedToClient)
            {
                string mediaSyncSSID = (string)null;
                bool disabled = false;
                HRESULT hresult = this.IsWlanDeviceDisabled(ref disabled);
                if (hresult.IsSuccess)
                    hresult = this.GetWifiMediaSyncSSID(ref mediaSyncSSID);
                if (hresult.IsSuccess)
                    flag = !disabled && !string.IsNullOrEmpty(mediaSyncSSID);
            }
            this.IsWirelessSyncEnabled = flag;
        }

        private void ShowPinUnlockDialog()
        {
            if (this._pinUnlockMessageBox == null)
            {
                this._pinUnlockMessageBox = MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_PHONE_PIN_UNLOCK_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_PHONE_PIN_UNLOCK_TEXT), (Command)null, ZuneUI.Shell.LoadString(StringId.IDS_CANCEL_BUTTON), new EventHandler(this.PinUnlockCancelled), false);
            }
            else
            {
                if (Application.RenderingType == RenderingType.GDI && SingletonModelItem<TransportControls>.Instance.PlayingVideo)
                    SingletonModelItem<TransportControls>.Instance.Stop.Invoke();
                this._pinUnlockMessageBox.Show();
            }
        }

        private void HidePinUnlockDialog(bool unlockSucceeded)
        {
            if (this._pinUnlockMessageBox == null)
                return;
            if (unlockSucceeded)
                this._pinUnlockMessageBox.Hide();
            else
                this._pinUnlockMessageBox.Cancel.Invoke();
            this._pinUnlockMessageBox = (MessageBox)null;
        }

        private void PinUnlockCancelled(object sender, EventArgs args) => SingletonModelItem<UIDeviceList>.Instance.HideDevice(this);

        private string GenerateSyncTimestring() => DateTime.UtcNow.ToString("yyyy-MM-dd HH\\:mm\\:ss.fff", (IFormatProvider)CultureInfo.CreateSpecificCulture("en-US"));

        private bool SupportsSyncCategory(SyncCategory category)
        {
            bool flag;
            switch (category)
            {
                case SyncCategory.Friend:
                    flag = this.SupportsUserCards;
                    break;
                case SyncCategory.Channel:
                    flag = this.SupportsChannels;
                    break;
                case SyncCategory.Application:
                    flag = this.SupportsSyncApplications;
                    break;
                default:
                    flag = true;
                    break;
            }
            return flag;
        }

        private MediaIdAndType[] GenerateThreadSafeDatabaseItems(IList source)
        {
            MediaIdAndType[] mediaIdAndTypeArray = new MediaIdAndType[source.Count];
            for (int index = 0; index < source.Count; ++index)
            {
                if (source[index] is IDatabaseMedia databaseMedia)
                {
                    int mediaId;
                    EMediaTypes mediaType;
                    databaseMedia.GetMediaIdAndType(out mediaId, out mediaType);
                    mediaIdAndTypeArray[index] = new MediaIdAndType(mediaId, mediaType);
                }
            }
            return mediaIdAndTypeArray;
        }

        public static void WarnUserAboutFriendSyncSize()
        {
            if (!ClientConfiguration.MediaStore.AlertSyncAllFriendsBehavior)
                return;
            BooleanChoice neverAlertSyncAllFriendsBehavior = new BooleanChoice((IModelItemOwner)ZuneShell.DefaultInstance, ZuneUI.Shell.LoadString(StringId.IDS_DONT_SHOW_THIS_MESSAGE_AGAIN));
            neverAlertSyncAllFriendsBehavior.Value = false;
            neverAlertSyncAllFriendsBehavior.ChosenChanged += (EventHandler)delegate
           {
               ClientConfiguration.MediaStore.AlertSyncAllFriendsBehavior = !neverAlertSyncAllFriendsBehavior.Value;
           };
            MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_SYNC_FRIENDS_NOTICE_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_SYNC_FRIENDS_NOTICE), (Command)null, neverAlertSyncAllFriendsBehavior);
        }

        public static string FormatSyncStatus(object title, IList metadata)
        {
            string str1 = (title ?? (object)string.Empty).ToString();
            bool flag = false;
            foreach (object obj in (IEnumerable)metadata)
            {
                string str2 = (obj ?? (object)string.Empty).ToString();
                if (!string.IsNullOrEmpty(str2))
                {
                    if (flag)
                    {
                        str1 += UIDevice._syncStatusDatumDivider;
                    }
                    else
                    {
                        str1 += UIDevice._syncStatusTitleDivider;
                        flag = true;
                    }
                    str1 += str2;
                }
            }
            return str1;
        }

        public static string FormatDeviceClass(DeviceClass deviceClass)
        {
            if (deviceClass == DeviceClass.ZuneHD)
                return ZuneUI.Shell.LoadString(StringId.IDS_AppsZuneHDDeviceName);
            return deviceClass == DeviceClass.WindowsPhone ? ZuneUI.Shell.LoadString(StringId.IDS_AppsWindowsPhoneDeviceName) : ZuneUI.Shell.LoadString(StringId.IDS_GENERIC_ERROR);
        }

        public static DeviceClass ToDeviceClass(int deviceClass) => (DeviceClass)deviceClass;

        public static DeviceClass ToDeviceClass(object deviceClass)
        {
            DeviceClass? nullable = (DeviceClass?)deviceClass;
            return !nullable.HasValue ? DeviceClass.Invalid : nullable.Value;
        }

        public static int ToInt32(DeviceClass deviceClass) => (int)deviceClass;

        protected interface INewRuleMessageType
        {
            MediaType Type { get; }

            string SingularMessage { get; }

            string PluralMessage { get; }

            string GetMessageForCount(int count);
        }

        protected class NewRuleMessageType : UIDevice.INewRuleMessageType
        {
            private MediaType _type;
            private string _singular;
            private string _plural;

            public NewRuleMessageType(MediaType type, StringId singular, StringId plural)
            {
                this._type = type;
                this._singular = ZuneUI.Shell.LoadString(singular);
                this._plural = ZuneUI.Shell.LoadString(plural);
            }

            public MediaType Type => this._type;

            public string SingularMessage => this._singular;

            public string PluralMessage => this._plural;

            public string GetMessageForCount(int count) => count == 1 ? this.SingularMessage : string.Format(this.PluralMessage, (object)count);
        }
    }
}
