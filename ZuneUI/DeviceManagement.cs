// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceManagement
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UIXControls;

namespace ZuneUI
{
    public class DeviceManagement : ModelItem
    {
        private static UIDevice _setupDevice;
        private static bool _setupQueueHandlingLocked = false;
        private static MessageBox _deviceOOBEIncompleteDialog;
        private int[] _defaultBitRates = new int[5]
        {
      96,
      128,
      192,
      256,
      320
        };
        private string errorMessage;
        private bool validatingCreds;
        private string _friendlyNameOnDevice;
        private int _deviceNameMaxLength = 32;
        private Choice _enableMarketplaceChoice;
        private BooleanChoice _enableMarketplacePurchase;
        private MarketplaceCredentialsForDevice _marketplaceCredentials;
        private float _reservedSpaceOnDevice = float.NaN;
        private Choice _musicSyncChoice;
        private Choice _videoSyncChoice;
        private Choice _photoSyncChoice;
        private Choice _podcastSyncChoice;
        private Choice _friendSyncChoice;
        private Choice _channelSyncChoice;
        private Choice _applicationSyncChoice;
        private BooleanChoice _dontSyncDislikedContent;
        private Command _formatNotifier;
        private int _transcodeSizeLimit = -1;
        private bool _transcodeInBG;
        private bool _transcodeInBGIsSet;
        private string _transcodedFilesCachePath;
        private IList<Command> _bitRateList;
        private Choice _audioConversionChoice;
        private IList<Command> _audioConversionOptions;
        private string _audioThresholdBitRate;
        private string _audioTargetBitRate;
        private Choice _videoConversionChoice;
        private IList<Command> _videoConversionOptions;
        private string _cameraRollDestinationPath;
        private string _savedFolderDestinationPath;
        private Choice _deleteAfterReverseSyncChoice;
        private IList<Command> _deleteAfterReverseSyncOptions;
        private Choice _imageQualitySyncChoice;
        private IList<Command> _imageQualitySyncOptions;
        private SyncGroupList _syncGroups;
        private BooleanChoice _privacyChoice;
        public static bool NavigatingToWizard = false;
        internal static DeviceSetupHashtable SetupQueue = new DeviceSetupHashtable();

        private UIDevice ActiveDevice => SyncControls.Instance.CurrentDeviceOverride;

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            SignIn.Instance.TempPasswordStorage = (string)null;
            WirelessSync.Instance = (WirelessSync)null;
            SignIn.Instance.SignInStatusUpdatedEvent -= new EventHandler(this.OnSignInStatusUpdatedEvent);
        }

        internal static event DeviceConnectionHandledEventHandler DeviceConnectionHandled;

        public string ErrorMessage
        {
            get => this.errorMessage;
            set
            {
                if (!(this.errorMessage != value))
                    return;
                this.errorMessage = value;
                this.FirePropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool ValidatingCredentials
        {
            get => this.validatingCreds;
            set
            {
                if (this.validatingCreds == value)
                    return;
                this.validatingCreds = value;
                this.FirePropertyChanged(nameof(ValidatingCredentials));
            }
        }

        public static void NavigateToWizardMode(CategoryPageNode node, Category category)
        {
            Management management = ZuneShell.DefaultInstance.Management;
            if (management.HasPendingCommits)
            {
                Command yesCommand = new Command((IModelItemOwner)null, Shell.LoadString(StringId.IDS_DIALOG_YES), (EventHandler)null);
                yesCommand.Invoked += (EventHandler)((sender, args) =>
               {
                   management.CommitListSave();
                   DeviceManagement.NavigateToWizardMode(node, category);
               });
                Command noCommand = new Command((IModelItemOwner)null, Shell.LoadString(StringId.IDS_DIALOG_NO), (EventHandler)null);
                noCommand.Invoked += (EventHandler)((sender, args) =>
               {
                   management.CommitList = (CommitListHashtable)null;
                   DeviceManagement.NavigateToWizardMode(node, category);
               });
                MessageBox.Show(Shell.LoadString(StringId.IDS_SAVE_CHANGES_DIALOG_TITLE), Shell.LoadString(StringId.IDS_SAVE_CHANGES_ON_BACK_DIALOG_TEXT), yesCommand, noCommand, (BooleanChoice)null);
            }
            else
            {
                DeviceManagement.NavigatingToWizard = true;
                if (Shell.SettingsFrame.IsCurrent && !Shell.SettingsFrame.Wizard.FUE.IsCurrent)
                    management.CurrentCategoryPage.CancelAndExit();
                Application.DeferredInvoke((DeferredInvokeHandler)delegate
               {
                   if (category != null)
                       node.Invoke(category);
                   else
                       node.Invoke();
               }, (object)null);
            }
        }

        public static UIDevice SetupDevice
        {
            get => DeviceManagement._setupDevice;
            set
            {
                if (DeviceManagement._setupDevice == value)
                    return;
                if (value == null)
                {
                    DeviceManagement._setupQueueHandlingLocked = false;
                    DeviceManagement.SetupQueue.Remove((object)DeviceManagement._setupDevice.ID);
                }
                DeviceManagement._setupDevice = value;
                if (DeviceManagement._setupDevice == null || Shell.SettingsFrame.IsCurrent)
                    return;
                PhoneBrandingStringMap.Instance.BrandingEnabled = DeviceManagement._setupDevice.SupportsBrandingType(DeviceBranding.WindowsPhone);
                KinBrandingStringMap.Instance.BrandingEnabled = DeviceManagement._setupDevice.SupportsBrandingType(DeviceBranding.Kin);
            }
        }

        internal static void HandleSetupQueue()
        {
            if (DeviceManagement._setupQueueHandlingLocked)
                return;
            DeviceManagement._setupQueueHandlingLocked = true;
            DeviceManagement.SetupDevice = (UIDevice)null;
            List<int> intList = new List<int>();
            foreach (DictionaryEntry setup in (Hashtable)DeviceManagement.SetupQueue)
            {
                UIDevice uiDevice = (UIDevice)setup.Value;
                if (!uiDevice.IsConnectedToClient)
                {
                    intList.Add(uiDevice.ID);
                }
                else
                {
                    DeviceManagement.SetupDevice = uiDevice;
                    break;
                }
            }
            foreach (int num in intList)
                DeviceManagement.SetupQueue.Remove((object)num);
            if (DeviceManagement.SetupDevice == null)
                DeviceManagement._setupQueueHandlingLocked = false;
            else
                DeviceManagement.HandleCurrentSetupDevice();
        }

        public static void HandleCurrentSetupDevice()
        {
            if (DeviceManagement.SetupDevice == null)
                return;
            if (DeviceManagement.SetupDevice.SupportsOOBECompleted)
            {
                if (!DeviceManagement.SetupDevice.OOBECompleted)
                {
                    SingletonModelItem<UIDeviceList>.Instance.HideDevice(DeviceManagement.SetupDevice);
                    DeviceManagement.ShowDeviceOOBEIncompleteDialog();
                    DeviceManagement.SetupDevice = (UIDevice)null;
                    return;
                }
                DeviceManagement.HideDeviceOOBEIncompleteDialog();
            }
            if (DeviceManagement.SetupDevice.RequiresClientUpdate)
            {
                SingletonModelItem<UIDeviceList>.Instance.HideDevice(DeviceManagement.SetupDevice);
                MessageBox.Show(Shell.LoadString(StringId.IDS_CLIENT_UPDATE_HEADER), Shell.LoadString(StringId.IDS_CLIENT_UPDATE_DESC), (EventHandler)((sender, args) => ClientUpdate.Instance.InvokeClientUpdate()));
                DeviceManagement.SetupDevice = (UIDevice)null;
            }
            else if (!UIDeviceList.IsSuitableForConnection(DeviceManagement.SetupDevice))
            {
                if (DeviceManagement.SetupDevice.RequiresAutoRestore)
                    ZuneShell.DefaultInstance.NavigateToPage((ZunePage)new AutoRestoreLandPage());
                else
                    ZuneShell.DefaultInstance.NavigateToPage((ZunePage)new FirstConnectLandPage());
            }
            else
            {
                UIFirmwareUpdater uiFirmwareUpdater = DeviceManagement.SetupDevice.UIFirmwareUpdater;
                if (uiFirmwareUpdater != null)
                {
                    bool launchWizardIfUpdatesFound = DeviceManagement.SetupDevice.SupportsBrandingType(DeviceBranding.WindowsPhone);
                    uiFirmwareUpdater.StartCheckForUpdates(DeviceManagement.SetupDevice.RequiresFirmwareUpdate, launchWizardIfUpdatesFound);
                }
                DeviceManagement.EndDeviceHandling(false, true);
            }
        }

        public static void HideSetupDevice()
        {
            SingletonModelItem<UIDeviceList>.Instance.HideDevice(DeviceManagement.SetupDevice);
            DeviceManagement.SetupDevice = (UIDevice)null;
        }

        public void SetupComplete(bool navigateToLandingPage)
        {
            if (DeviceManagement.SetupDevice == null)
                return;
            if (this.ActiveDevice.IsValid)
            {
                this.ActiveDevice.PromptForAccountLinkage = true;
                SyncControls.Instance.ChangeIntoSetupDevice = false;
                DeviceManagement.EndDeviceHandling(true, navigateToLandingPage);
                ZuneShell.DefaultInstance.Management.DisposeDeviceManagement(false);
            }
            else
                this.ErrorMessage = Shell.LoadString(StringId.IDS_SYNC_SETUP_COMPLETION_ERROR);
        }

        private static void EndDeviceHandling(bool comingOutOfFirstConnect, bool navigateToLandingPage)
        {
            if (DeviceManagement.SetupDevice == null)
                return;
            DeviceManagement.SetupDevice.Enumerate();
            SyncControls instance = SyncControls.Instance;
            if (!Shell.SettingsFrame.IsCurrent || comingOutOfFirstConnect)
                instance.SetCurrentDeviceIfNecessary(DeviceManagement.SetupDevice, comingOutOfFirstConnect);
            if (!DeviceManagement.SetupDevice.IsGuest && DeviceManagement.SetupDevice.HasFailedLogin)
                instance.AddDeviceToFailedSignInQueue(DeviceManagement.SetupDevice);
            if (DeviceManagement.SetupDevice.SupportsBrandingType(DeviceBranding.WindowsPhone) || DeviceManagement.SetupDevice.SupportsBrandingType(DeviceBranding.Kin))
            {
                instance.ShowPhoneWelcomeMessage = true;
                ClientConfiguration.Devices.HasPhoneBeenConnected = true;
            }
            if (navigateToLandingPage && comingOutOfFirstConnect && !(ZuneShell.DefaultInstance.CurrentPage is Deviceland))
                Shell.MainFrame.Device.Invoke();
            instance.ShowSyncInstructionsToast = true;
            if (DeviceManagement.DeviceConnectionHandled != null)
                DeviceManagement.DeviceConnectionHandled((object)null, new DeviceConnectionHandledEventArgs(DeviceManagement.SetupDevice, comingOutOfFirstConnect));
            DeviceManagement.SetupDevice = (UIDevice)null;
        }

        internal static void ShowDeviceOOBEIncompleteDialog()
        {
            if (DeviceManagement._deviceOOBEIncompleteDialog != null)
                return;
            DeviceManagement._deviceOOBEIncompleteDialog = MessageBox.Show(Shell.LoadString(StringId.IDS_PHONE_OOBE_INCOMPLETE_TITLE), Shell.LoadString(StringId.IDS_PHONE_OOBE_INCOMPLETE_DESCRIPTION), (EventHandler)null, (EventHandler)null, (EventHandler)null, (EventHandler)((sender, args) => DeviceManagement.HideDeviceOOBEIncompleteDialog()), (BooleanChoice)null);
        }

        internal static void HideDeviceOOBEIncompleteDialog()
        {
            if (DeviceManagement._deviceOOBEIncompleteDialog == null)
                return;
            DeviceManagement._deviceOOBEIncompleteDialog.Hide();
            DeviceManagement._deviceOOBEIncompleteDialog = (MessageBox)null;
        }

        public Choice MusicSyncChoice
        {
            get
            {
                if (this._musicSyncChoice == null)
                    this._musicSyncChoice = this.GenerateSyncModeChoice(SyncCategory.Music);
                return this._musicSyncChoice;
            }
            private set
            {
                if (this._musicSyncChoice == value)
                    return;
                this._musicSyncChoice = value;
                this.FirePropertyChanged(nameof(MusicSyncChoice));
            }
        }

        public Choice VideoSyncChoice
        {
            get
            {
                if (this._videoSyncChoice == null)
                    this._videoSyncChoice = this.GenerateSyncModeChoice(SyncCategory.Video);
                return this._videoSyncChoice;
            }
            private set
            {
                if (this._videoSyncChoice == value)
                    return;
                this._videoSyncChoice = value;
                this.FirePropertyChanged(nameof(VideoSyncChoice));
            }
        }

        public Choice PhotoSyncChoice
        {
            get
            {
                if (this._photoSyncChoice == null)
                    this._photoSyncChoice = this.GenerateSyncModeChoice(SyncCategory.Photo);
                return this._photoSyncChoice;
            }
            private set
            {
                if (this._photoSyncChoice == value)
                    return;
                this._photoSyncChoice = value;
                this.FirePropertyChanged(nameof(PhotoSyncChoice));
            }
        }

        public Choice PodcastSyncChoice
        {
            get
            {
                if (this._podcastSyncChoice == null)
                    this._podcastSyncChoice = this.GenerateSyncModeChoice(SyncCategory.Podcast);
                return this._podcastSyncChoice;
            }
            private set
            {
                if (this._podcastSyncChoice == value)
                    return;
                this._podcastSyncChoice = value;
                this.FirePropertyChanged(nameof(PodcastSyncChoice));
            }
        }

        public Choice FriendSyncChoice
        {
            get
            {
                if (this._friendSyncChoice == null)
                    this._friendSyncChoice = this.GenerateSyncModeChoice(SyncCategory.Friend);
                return this._friendSyncChoice;
            }
            private set
            {
                if (this._friendSyncChoice == value)
                    return;
                this._friendSyncChoice = value;
                this.FirePropertyChanged(nameof(FriendSyncChoice));
            }
        }

        public Choice ChannelSyncChoice
        {
            get
            {
                if (this._channelSyncChoice == null)
                    this._channelSyncChoice = this.GenerateSyncModeChoice(SyncCategory.Channel);
                return this._channelSyncChoice;
            }
            private set
            {
                if (this._channelSyncChoice == value)
                    return;
                this._channelSyncChoice = value;
                this.FirePropertyChanged(nameof(ChannelSyncChoice));
            }
        }

        public Choice ApplicationSyncChoice
        {
            get
            {
                if (this._applicationSyncChoice == null)
                    this._applicationSyncChoice = this.GenerateSyncModeChoice(SyncCategory.Application);
                return this._applicationSyncChoice;
            }
            private set
            {
                if (this._applicationSyncChoice == value)
                    return;
                this._applicationSyncChoice = value;
                this.FirePropertyChanged(nameof(ApplicationSyncChoice));
            }
        }

        private Choice GenerateSyncModeChoice(SyncCategory syncType)
        {
            Choice choice = new Choice((IModelItemOwner)this);
            List<SyncModeOptionPair> syncModeOptionPairList = new List<SyncModeOptionPair>();
            string name1 = "";
            string name2 = "";
            string name3 = (string)null;
            EventHandler eventHandler = (EventHandler)null;
            switch (syncType)
            {
                case SyncCategory.Music:
                    name1 = Shell.LoadString(StringId.IDS_SYNC_ALL_OPTION_MUSIC);
                    name2 = Shell.LoadString(StringId.IDS_LET_ME_CHOOSE_OPTION_MUSIC);
                    name3 = Shell.LoadString(StringId.IDS_MANUAL_OPTION_MUSIC);
                    eventHandler = new EventHandler(this.HandleMusicSyncOptionChanged);
                    break;
                case SyncCategory.Video:
                    name1 = Shell.LoadString(StringId.IDS_SYNC_ALL_OPTION_VIDEOS);
                    name2 = Shell.LoadString(StringId.IDS_LET_ME_CHOOSE_OPTION_VIDEOS);
                    name3 = Shell.LoadString(StringId.IDS_MANUAL_OPTION_VIDEOS);
                    eventHandler = new EventHandler(this.HandleVideoSyncOptionChanged);
                    break;
                case SyncCategory.Photo:
                    name1 = Shell.LoadString(StringId.IDS_SYNC_ALL_OPTION_PICTURES);
                    name2 = Shell.LoadString(StringId.IDS_LET_ME_CHOOSE_OPTION_PICTURES);
                    name3 = Shell.LoadString(StringId.IDS_MANUAL_OPTION_PICTURES);
                    eventHandler = new EventHandler(this.HandlePhotoSyncOptionChanged);
                    break;
                case SyncCategory.Podcast:
                    name1 = Shell.LoadString(StringId.IDS_SYNC_ALL_OPTION_PODCASTS);
                    name2 = Shell.LoadString(StringId.IDS_LET_ME_CHOOSE_OPTION_PODCASTS);
                    eventHandler = new EventHandler(this.HandlePodcastSyncOptionChanged);
                    break;
                case SyncCategory.Friend:
                    name1 = Shell.LoadString(StringId.IDS_SYNC_ALL_OPTION_FRIENDS);
                    name2 = Shell.LoadString(StringId.IDS_LET_ME_CHOOSE_OPTION_FRIENDS);
                    eventHandler = new EventHandler(this.HandleFriendSyncOptionChanged);
                    break;
                case SyncCategory.Channel:
                    name1 = Shell.LoadString(StringId.IDS_SYNC_ALL_OPTION_CHANNELS);
                    name2 = Shell.LoadString(StringId.IDS_LET_ME_CHOOSE_OPTION_CHANNELS);
                    eventHandler = new EventHandler(this.HandleChannelSyncOptionChanged);
                    break;
                case SyncCategory.Application:
                    name1 = Shell.LoadString(StringId.IDS_SYNC_ALL_OPTION_APPLICATIONS);
                    name2 = Shell.LoadString(StringId.IDS_LET_ME_CHOOSE_OPTION_APPLICATIONS);
                    name3 = Shell.LoadString(StringId.IDS_MANUAL_OPTION_APPLICATIONS);
                    eventHandler = new EventHandler(this.HandleApplicationSyncOptionChanged);
                    break;
            }
            SyncModeOptionPair syncModeOptionPair1 = new SyncModeOptionPair(name1, SyncMode.SyncAll);
            syncModeOptionPairList.Add(syncModeOptionPair1);
            SyncModeOptionPair syncModeOptionPair2 = new SyncModeOptionPair(name2, SyncMode.LetMeChoose);
            syncModeOptionPairList.Add(syncModeOptionPair2);
            if (!string.IsNullOrEmpty(name3))
            {
                SyncModeOptionPair syncModeOptionPair3 = new SyncModeOptionPair(name3, SyncMode.Manual);
                syncModeOptionPairList.Add(syncModeOptionPair3);
            }
            bool flag1 = this.ActiveDevice.IsSyncAllFor(syncType, true);
            bool flag2 = !string.IsNullOrEmpty(name3) && this.ActiveDevice.IsManualFor(syncType, true);
            choice.Options = (IList)syncModeOptionPairList;
            choice.ChosenIndex = flag2 ? 2 : (flag1 ? 0 : 1);
            if (eventHandler != null)
                choice.ChosenChanged += eventHandler;
            return choice;
        }

        private void HandleMusicSyncOptionChanged(object sender, EventArgs args) => ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnMusicSyncChoiceCommit)] = (object)this.CommitDeviceID;

        private void OnMusicSyncChoiceCommit(object data) => this.OnCategorySyncChoiceCommit(SyncCategory.Music, ((SyncModeOptionPair)this._musicSyncChoice.ChosenValue).Mode);

        private void HandleVideoSyncOptionChanged(object sender, EventArgs args) => ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnVideoSyncChoiceCommit)] = (object)this.CommitDeviceID;

        private void OnVideoSyncChoiceCommit(object data) => this.OnCategorySyncChoiceCommit(SyncCategory.Video, ((SyncModeOptionPair)this._videoSyncChoice.ChosenValue).Mode);

        private void HandlePhotoSyncOptionChanged(object sender, EventArgs args) => ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnPhotoSyncChoiceCommit)] = (object)this.CommitDeviceID;

        private void OnPhotoSyncChoiceCommit(object data) => this.OnCategorySyncChoiceCommit(SyncCategory.Photo, ((SyncModeOptionPair)this._photoSyncChoice.ChosenValue).Mode);

        private void HandlePodcastSyncOptionChanged(object sender, EventArgs args) => ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnPodcastSyncChoiceCommit)] = (object)this.CommitDeviceID;

        private void OnPodcastSyncChoiceCommit(object data) => this.OnCategorySyncChoiceCommit(SyncCategory.Podcast, ((SyncModeOptionPair)this._podcastSyncChoice.ChosenValue).Mode);

        private void HandleFriendSyncOptionChanged(object sender, EventArgs args)
        {
            if (((SyncModeOptionPair)this._friendSyncChoice.ChosenValue).Mode == SyncMode.SyncAll)
                UIDevice.WarnUserAboutFriendSyncSize();
            ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnFriendSyncChoiceCommit)] = (object)this.CommitDeviceID;
        }

        private void OnFriendSyncChoiceCommit(object data)
        {
            if (!this.DeviceHasTag)
                return;
            this.OnCategorySyncChoiceCommit(SyncCategory.Friend, ((SyncModeOptionPair)this._friendSyncChoice.ChosenValue).Mode);
        }

        private void HandleChannelSyncOptionChanged(object sender, EventArgs args) => ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnChannelSyncChoiceCommit)] = (object)this.CommitDeviceID;

        private void OnChannelSyncChoiceCommit(object data)
        {
            if (!this.DeviceHasTag)
                return;
            this.OnCategorySyncChoiceCommit(SyncCategory.Channel, ((SyncModeOptionPair)this._channelSyncChoice.ChosenValue).Mode);
        }

        private void HandleApplicationSyncOptionChanged(object sender, EventArgs args) => ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnApplicationSyncChoiceCommit)] = (object)this.CommitDeviceID;

        private void OnApplicationSyncChoiceCommit(object data) => this.OnCategorySyncChoiceCommit(SyncCategory.Application, ((SyncModeOptionPair)this._applicationSyncChoice.ChosenValue).Mode);

        private void OnCategorySyncChoiceCommit(SyncCategory cat, SyncMode mode)
        {
            if (!this.ActiveDevice.IsValid)
                return;
            SyncMode syncMode = this.ActiveDevice.GetSyncMode(cat);
            this.ActiveDevice.SetSyncMode(cat, mode);
            this.HandleSyncOptionChanges(syncMode, mode);
        }

        private void HandleSyncOptionChanges(SyncMode oldMode, SyncMode mode)
        {
            if (mode == oldMode)
                return;
            CategoryPage currentCategoryPage = ZuneShell.DefaultInstance.Management.CurrentCategoryPage;
        }

        public void FormatCurrentDevice() => MessageBox.Show(Shell.LoadString(StringId.IDS_FORMAT_DIALOG_TITLE), Shell.LoadString(StringId.IDS_FORMAT_DIALOG_TEXT), new EventHandler(this.ConfirmedFormatDevice), (EventHandler)null);

        private void ConfirmedFormatDevice(object sender, EventArgs e)
        {
            this.FormatBegun.Invoke();
            SyncControls.Instance.CurrentDevice.Format();
        }

        public Command FormatBegun
        {
            get
            {
                if (this._formatNotifier == null)
                    this._formatNotifier = new Command((IModelItemOwner)this);
                return this._formatNotifier;
            }
        }

        public BooleanChoice DontSyncDislikedContentChoice
        {
            get
            {
                if (this._dontSyncDislikedContent == null)
                {
                    this._dontSyncDislikedContent = new BooleanChoice((IModelItemOwner)this, Shell.LoadString(StringId.IDS_DONT_SYNC_DISLIKED_CONTENT));
                    this._dontSyncDislikedContent.Value = this.ActiveDevice.ExcludeDislikedContent;
                    this._dontSyncDislikedContent.ChosenChanged += (EventHandler)((sender, args) => ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnDontSyncDislikedContentChoiceCommit)] = (object)this.CommitDeviceID);
                }
                return this._dontSyncDislikedContent;
            }
        }

        private void OnDontSyncDislikedContentChoiceCommit(object data) => this.ActiveDevice.ExcludeDislikedContent = this.DontSyncDislikedContentChoice.Value;

        public Choice DeleteAfterReverseSyncChoice
        {
            get
            {
                if (this._deleteAfterReverseSyncChoice == null)
                {
                    this._deleteAfterReverseSyncOptions = (IList<Command>)new List<Command>();
                    this._deleteAfterReverseSyncOptions.Add(new Command((IModelItemOwner)this, Shell.LoadString(StringId.IDS_PHOTOS_SETTINGS_LEAVE_AFTER_REVERSE_SYNC), (EventHandler)null));
                    this._deleteAfterReverseSyncOptions.Add(new Command((IModelItemOwner)this, Shell.LoadString(StringId.IDS_PHOTOS_SETTINGS_DELETE_AFTER_REVERSE_SYNC), (EventHandler)null));
                    this._deleteAfterReverseSyncChoice = new Choice((IModelItemOwner)this);
                    this._deleteAfterReverseSyncChoice.Options = (IList)this._deleteAfterReverseSyncOptions;
                    this._deleteAfterReverseSyncChoice.DefaultIndex = this.ActiveDevice.DeletePicsFromPhoneAfterReverseSync ? 1 : 0;
                    this._deleteAfterReverseSyncChoice.DefaultValue();
                    this._deleteAfterReverseSyncChoice.ChosenChanged += (EventHandler)((sender, args) => ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnDeleteAfterReverseSyncCommit)] = (object)this.CommitDeviceID);
                }
                return this._deleteAfterReverseSyncChoice;
            }
        }

        private void OnDeleteAfterReverseSyncCommit(object data) => this.ActiveDevice.DeletePicsFromPhoneAfterReverseSync = this.DeleteAfterReverseSyncChoice.ChosenIndex == 1;

        public string FriendlyNameOnDevice
        {
            get
            {
                if (this._friendlyNameOnDevice == null)
                    this._friendlyNameOnDevice = this.ActiveDevice.Name;
                return this._friendlyNameOnDevice;
            }
            set
            {
                string errorMessage;
                if (this.IsDeviceNameValid(value, out errorMessage))
                {
                    if (this._friendlyNameOnDevice != value)
                    {
                        this._friendlyNameOnDevice = value;
                        ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnFriendlyNameOnDeviceCommit)] = (object)this.CommitDeviceID;
                        this.FirePropertyChanged(nameof(FriendlyNameOnDevice));
                    }
                    this.ErrorMessage = (string)null;
                }
                else
                    this.ErrorMessage = errorMessage;
            }
        }

        private void OnFriendlyNameOnDeviceCommit(object data) => this.ActiveDevice.Name = this._friendlyNameOnDevice.Trim();

        private bool IsDeviceNameValid(string name, out string errorMessage)
        {
            if (string.IsNullOrEmpty(name))
            {
                errorMessage = Shell.LoadString(StringId.IDS_DEVICE_NAME_EMPTY);
                return false;
            }
            if (name.Length > this._deviceNameMaxLength)
            {
                errorMessage = Shell.LoadString(StringId.IDS_DEVICE_NAME_TOO_LONG);
                return false;
            }
            if (this.DeviceNameHasInvalidCharacters(name))
            {
                errorMessage = Shell.LoadString(StringId.IDS_DEVICE_NAME_INVALID_CHARS);
                return false;
            }
            errorMessage = (string)null;
            return true;
        }

        private bool DeviceNameHasInvalidCharacters(string name)
        {
            if (this.ActiveDevice.SupportsBrandingType(DeviceBranding.WindowsPhone))
            {
                if (name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                    return true;
            }
            try
            {
                new UnicodeEncoding(false, false, true).GetBytes(name);
                return false;
            }
            catch (EncoderFallbackException ex)
            {
                return true;
            }
            catch (ArgumentException ex)
            {
                return true;
            }
        }

        public void CheckForAutomatedRequirements()
        {
        }

        public MarketplaceCredentialsForDevice MarketplaceCredentials => this._marketplaceCredentials;

        public Choice EnableMarketplaceChoice
        {
            get
            {
                if (this._enableMarketplaceChoice == null)
                {
                    this._enableMarketplaceChoice = new Choice((IModelItemOwner)this);
                    this._enableMarketplaceChoice.Options = (IList)new Command[2]
                    {
            new Command((IModelItemOwner) this, Shell.LoadString(StringId.IDS_SKIP_FOR_NOW_OPTION), (EventHandler) null),
            new Command((IModelItemOwner) this, Shell.LoadString(StringId.IDS_ENABLE_ZUNE_MARKETPLACE_OPTION), (EventHandler) null)
                    };
                    this._marketplaceCredentials = new MarketplaceCredentialsForDevice((string)null, (string)null, this.ActiveDevice.PurchaseEnabled, !Shell.SettingsFrame.Wizard.IsCurrent && !string.IsNullOrEmpty(this.ActiveDevice.ZuneTag), this.ActiveDevice.ZuneTag, this._enableMarketplaceChoice);
                    if (Shell.SettingsFrame.Wizard.IsCurrent)
                    {
                        this.MarketplaceCredentials.Email = SignIn.GetPassportIdFromUserId(SignIn.Instance.LastSignedInUserId);
                        this._enableMarketplaceChoice.DefaultIndex = string.IsNullOrEmpty(this.MarketplaceCredentials.Email) ? 0 : 1;
                        ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnMarketplaceCredentialsCommit)] = (object)this.CommitDeviceID;
                    }
                    else
                        this._enableMarketplaceChoice.DefaultIndex = string.IsNullOrEmpty(this.ActiveDevice.ZuneTag) ? 0 : 1;
                    this._enableMarketplaceChoice.DefaultValue();
                    this._enableMarketplaceChoice.ChosenChanged += (EventHandler)((sender, args) =>
                   {
                       ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnMarketplaceCredentialsCommit)] = (object)this.CommitDeviceID;
                       this.FirePropertyChanged(nameof(EnableMarketplaceChoice));
                   });
                }
                return this._enableMarketplaceChoice;
            }
        }

        private void OnMarketplaceCredentialsCommit(object data)
        {
            if (this.EnableMarketplaceChoice.ChosenIndex == 0)
            {
                this.ActiveDevice.ClearAccountAssociation();
            }
            else
            {
                this.ActiveDevice.SetGeoId();
                this.ActiveDevice.PurchaseEnabled = this._marketplaceCredentials.PurchaseEnabled;
                if (this.ActiveDevice.AssociateWithAccount(this._marketplaceCredentials.UserGuid, this._marketplaceCredentials.ZuneTag).IsSuccess)
                    this.ActiveDevice.SendMarketplaceCredentials(this._marketplaceCredentials.Email, this._marketplaceCredentials.Password);
                else
                    this.ActiveDevice.ClearAccountAssociation();
            }
        }

        public bool IsDevicePurchaseEnabled
        {
            get
            {
                if (this.MarketplaceCredentials == null)
                    return this.ActiveDevice.PurchaseEnabled;
                return this.MarketplaceCredentials.IsAssociated && this.MarketplaceCredentials.PurchaseEnabled;
            }
        }

        public bool IsAssociated => this.MarketplaceCredentials != null ? this.MarketplaceCredentials.IsAssociated : !string.IsNullOrEmpty(this.ActiveDevice.ZuneTag);

        public bool DeviceHasTag
        {
            get
            {
                bool flag = false;
                if (this.IsPartnered)
                {
                    flag = SyncControls.Instance.CurrentDeviceOverride.UserId > 0;
                    if (!flag)
                        flag = this.IsAssociated;
                }
                return flag;
            }
        }

        public BooleanChoice EnableMarketplacePurchase
        {
            get
            {
                if (this._enableMarketplacePurchase == null)
                {
                    this._enableMarketplacePurchase = new BooleanChoice((IModelItemOwner)this, Shell.LoadString(StringId.IDS_ENABLE_DEVICE_PURCHASE));
                    this._enableMarketplacePurchase.Value = this.MarketplaceCredentials.PurchaseEnabled;
                    this._enableMarketplacePurchase.ChosenChanged += (EventHandler)((sender, args) => this.MarketplaceCredentials.PurchaseEnabled = this._enableMarketplacePurchase.Value);
                }
                return this._enableMarketplacePurchase;
            }
        }

        public void ValidatePassportAccount(string email, string password)
        {
            if (email == string.Empty || password == string.Empty)
            {
                this.ErrorMessage = Shell.LoadString(StringId.IDS_VERIFY_CREDS_ERROR);
                this.MarketplaceCredentials.PurchaseEnabled = false;
                this.MarketplaceCredentials.ZuneTag = string.Empty;
                this.FirePropertyChanged("MarketplaceCredentials");
            }
            else
            {
                this.ErrorMessage = string.Empty;
                this.ValidatingCredentials = true;
                this.MarketplaceCredentials.Email = email;
                this.MarketplaceCredentials.Password = password;
                SignIn.Instance.SignOut();
                SignIn.Instance.SignInStatusUpdatedEvent += new EventHandler(this.OnSignInStatusUpdatedEvent);
                SignIn.Instance.SignInUser(email, password);
            }
        }

        private void OnSignInStatusUpdatedEvent(object sender, EventArgs e)
        {
            if (SignIn.Instance.SigningIn || !SignIn.Instance.SignInError.IsError && !SignIn.Instance.SignedIn)
                return;
            SignIn.Instance.SignInStatusUpdatedEvent -= new EventHandler(this.OnSignInStatusUpdatedEvent);
            this.MarketplaceCredentials.hr = SignIn.Instance.SignInError.hr;
            if (SignIn.Instance.SignInError.IsError)
            {
                this.ErrorMessage = Shell.LoadString(StringId.IDS_VERIFY_CREDS_ERROR);
                this.MarketplaceCredentials.PurchaseEnabled = false;
                this.MarketplaceCredentials.ZuneTag = string.Empty;
            }
            else
            {
                this.ErrorMessage = string.Empty;
                this.MarketplaceCredentials.ZuneTag = SignIn.Instance.ZuneTag;
                this.MarketplaceCredentials.UserGuid = SignIn.Instance.UserGuid;
            }
            if (!Shell.SettingsFrame.Wizard.IsCurrent)
                this.OnMarketplaceCredentialsCommit((object)null);
            this.ValidatingCredentials = false;
            this.FirePropertyChanged("MarketplaceCredentials");
        }

        public bool CredentialValidationRequested
        {
            get => true;
            set => this.FirePropertyChanged(nameof(CredentialValidationRequested));
        }

        public DeviceRelationship DevicePartnership
        {
            get
            {
                Management management = ZuneShell.DefaultInstance.Management;
                if (management.CommitList.ContainsValue((object)"OnSyncPartnershipCommit"))
                    return DeviceRelationship.Permanent;
                DeviceRelationship deviceRelationship = this.ActiveDevice.Relationship;
                if (this.ActiveDevice.IsValid && deviceRelationship == DeviceRelationship.None)
                {
                    management.CommitList[(object)new ProxySettingDelegate(this.OnSyncPartnershipCommit)] = (object)"OnSyncPartnershipCommit";
                    deviceRelationship = DeviceRelationship.Permanent;
                }
                return deviceRelationship;
            }
            set
            {
                Management management = ZuneShell.DefaultInstance.Management;
                if (value == DeviceRelationship.Permanent)
                {
                    management.CommitList[(object)new ProxySettingDelegate(this.OnSyncPartnershipCommit)] = (object)"OnSyncPartnershipCommit";
                }
                else
                {
                    management.CommitList[(object)new ProxySettingDelegate(this.OnSyncPartnershipCommit)] = (object)null;
                    this.ActiveDevice.Relationship = value;
                }
                this.FirePropertyChanged(nameof(DevicePartnership));
                this.FirePropertyChanged("IsPartnered");
            }
        }

        public bool IsPartnered => this.DevicePartnership == DeviceRelationship.Permanent;

        public bool IsCurrentDeviceNull => this.ActiveDevice == UIDeviceList.NullDevice;

        private void OnSyncPartnershipCommit(object data)
        {
            if (!this.ActiveDevice.IsValid || data == null || this.ActiveDevice.Relationship == DeviceRelationship.Permanent)
                return;
            this.ActiveDevice.Relationship = DeviceRelationship.Permanent;
            if (!this.ActiveDevice.SupportsWirelessSetupMethod1 && !this.ActiveDevice.SupportsWirelessSetupMethod2)
                return;
            WirelessSync.Instance.ClearWirelessOnDevice();
        }

        public string TranscodeSizeLimit
        {
            get
            {
                string transcodedFilesCachePath = this.TranscodedFilesCachePath;
                if (this._transcodeSizeLimit == -1)
                    this._transcodeSizeLimit = SingletonModelItem<UIDeviceList>.Instance.TranscodedFilesCacheSize;
                return this._transcodeSizeLimit.ToString();
            }
            set
            {
                int result = 1;
                if (string.IsNullOrEmpty(value) || int.TryParse(value, out result))
                {
                    if (result < 1)
                        result = 1;
                    else if (result > 999999)
                        result = 999999;
                    if (this._transcodeSizeLimit != result)
                    {
                        this._transcodeSizeLimit = result;
                        ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnTranscodeSizeLimitCommit)] = (object)this.CommitDeviceID;
                    }
                }
                this.FirePropertyChanged(nameof(TranscodeSizeLimit));
            }
        }

        private void OnTranscodeSizeLimitCommit(object data) => SingletonModelItem<UIDeviceList>.Instance.TranscodedFilesCacheSize = this._transcodeSizeLimit;

        public string TranscodedFilesCachePath
        {
            get
            {
                if (this._transcodedFilesCachePath == null)
                    this._transcodedFilesCachePath = SingletonModelItem<UIDeviceList>.Instance.TranscodedFilesCachePath;
                return this._transcodedFilesCachePath;
            }
            private set
            {
                if (!(this._transcodedFilesCachePath != value))
                    return;
                this._transcodedFilesCachePath = value;
                this.FirePropertyChanged(nameof(TranscodedFilesCachePath));
            }
        }

        public string CameraRollDestinationPath
        {
            get
            {
                if (this._cameraRollDestinationPath == null)
                    this._cameraRollDestinationPath = this.ActiveDevice.CameraRollDestinationPath;
                return this._cameraRollDestinationPath;
            }
            private set
            {
                if (!(this._cameraRollDestinationPath != value))
                    return;
                this._cameraRollDestinationPath = value;
                ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnCameraRollDestinationPathCommit)] = (object)this.CommitDeviceID;
                this.FirePropertyChanged(nameof(CameraRollDestinationPath));
            }
        }

        public void OnCameraRollDestinationPathCommit(object data) => this.ActiveDevice.CameraRollDestinationPath = this._cameraRollDestinationPath;

        public string SavedFolderDestinationPath
        {
            get
            {
                if (this._savedFolderDestinationPath == null)
                    this._savedFolderDestinationPath = this.ActiveDevice.SavedFolderDestinationPath;
                return this._savedFolderDestinationPath;
            }
            private set
            {
                if (!(this._savedFolderDestinationPath != value))
                    return;
                this._savedFolderDestinationPath = value;
                ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnSavedFolderDestinationPathCommit)] = (object)this.CommitDeviceID;
                this.FirePropertyChanged(nameof(SavedFolderDestinationPath));
            }
        }

        public void OnSavedFolderDestinationPathCommit(object data) => this.ActiveDevice.SavedFolderDestinationPath = this._savedFolderDestinationPath;

        public bool TranscodeInBG
        {
            get
            {
                if (!this._transcodeInBGIsSet)
                {
                    this._transcodeInBG = ClientConfiguration.Transcode.BackgroundTranscode;
                    this._transcodeInBGIsSet = true;
                }
                return this._transcodeInBG;
            }
            set
            {
                if (this._transcodeInBG == value)
                    return;
                this._transcodeInBG = value;
                ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnTranscodeInBGCommit)] = (object)null;
                this.FirePropertyChanged(nameof(TranscodeInBG));
                this._transcodeInBGIsSet = true;
            }
        }

        private void OnTranscodeInBGCommit(object data) => ClientConfiguration.Transcode.BackgroundTranscode = this._transcodeInBG;

        public void ChangeTranscodeFolder() => FolderBrowseDialog.Show("", (DeferredInvokeHandler)(args =>
       {
           if (args == null)
               return;
           ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnChangeTranscodeFolderCommit)] = (object)null;
           this.TranscodedFilesCachePath = (string)args;
       }), true);

        private void OnChangeTranscodeFolderCommit(object data) => SingletonModelItem<UIDeviceList>.Instance.TranscodedFilesCachePath = this._transcodedFilesCachePath;

        public void ClearTranscodeFolder()
        {
            if (!this.ActiveDevice.IsValid)
                return;
            HRESULT hresult = SingletonModelItem<UIDeviceList>.Instance.ClearTranscodeCache();
            if (!hresult.IsError)
                return;
            Shell.ShowErrorDialog(hresult.Int, StringId.IDS_CACHE_CLEAR_FAILED);
        }

        public void ChangeCameraRollDestinationPath()
        {
            if (!this.ActiveDevice.IsValid)
                return;
            FolderBrowseDialog.Show("", (DeferredInvokeHandler)(args =>
           {
               if (args == null)
                   return;
               ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnChangeCameraRollDestinationPathCommit)] = (object)this.CommitDeviceID;
               this.CameraRollDestinationPath = (string)args;
           }), true);
        }

        private void OnChangeCameraRollDestinationPathCommit(object data) => this.ActiveDevice.CameraRollDestinationPath = this._transcodedFilesCachePath;

        public Choice ImageQualitySyncChoice
        {
            get
            {
                if (this._imageQualitySyncChoice == null)
                {
                    this._imageQualitySyncOptions = (IList<Command>)new List<Command>();
                    this._imageQualitySyncOptions.Add(new Command((IModelItemOwner)this, Shell.LoadString(StringId.IDS_PHOTOS_SETTINGS_QUALITY_DEFAULT), (EventHandler)null));
                    this._imageQualitySyncOptions.Add(new Command((IModelItemOwner)this, Shell.LoadString(StringId.IDS_PHOTOS_SETTINGS_QUALITY_ORIGINAL), (EventHandler)null));
                    this._imageQualitySyncOptions.Add(new Command((IModelItemOwner)this, Shell.LoadString(StringId.IDS_PHOTOS_SETTINGS_QUALITY_VGA), (EventHandler)null));
                    this._imageQualitySyncChoice = new Choice((IModelItemOwner)this);
                    this._imageQualitySyncChoice.Options = (IList)this._imageQualitySyncOptions;
                    this._imageQualitySyncChoice.DefaultIndex = this.ActiveDevice.ImageTranscodeQuality > ETranscodePhotoSetting.tsPhotoSettingDevicePreferred ? (int)this.ActiveDevice.ImageTranscodeQuality : 0;
                    this._imageQualitySyncChoice.DefaultValue();
                    this._imageQualitySyncChoice.ChosenChanged += (EventHandler)((sender, args) => ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnImageQualitySyncChoiceCommit)] = (object)this.CommitDeviceID);
                }
                return this._imageQualitySyncChoice;
            }
            private set
            {
                if (this._imageQualitySyncChoice == value)
                    return;
                this._imageQualitySyncChoice = value;
                this.FirePropertyChanged(nameof(ImageQualitySyncChoice));
            }
        }

        private void OnImageQualitySyncChoiceCommit(object data) => this.ActiveDevice.ImageTranscodeQuality = (ETranscodePhotoSetting)this._imageQualitySyncChoice.ChosenIndex;

        public Choice AudioConversionChoice
        {
            get
            {
                if (this._audioConversionChoice == null)
                {
                    this._audioConversionOptions = (IList<Command>)new List<Command>();
                    this._audioConversionOptions.Add(new Command((IModelItemOwner)this, Shell.LoadString(StringId.IDS_TRANSCODE_AUDIO_NOSUPPORT_OPTION), (EventHandler)null));
                    this._audioConversionOptions.Add(new Command((IModelItemOwner)this, Shell.LoadString(StringId.IDS_TRANSCODE_AUDIO_EXCEEDS_OPTION), (EventHandler)null));
                    this._audioConversionChoice = new Choice((IModelItemOwner)this);
                    this._audioConversionChoice.Options = (IList)this._audioConversionOptions;
                    this._audioConversionChoice.DefaultIndex = this.ActiveDevice.AudioTranscodeLimit > 0 ? 1 : 0;
                    this._audioConversionChoice.DefaultValue();
                    this._audioConversionChoice.ChosenChanged += (EventHandler)((sender, args) => ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnAudioConversionChoiceCommit)] = (object)this.CommitDeviceID);
                }
                return this._audioConversionChoice;
            }
            private set
            {
                if (this._audioConversionChoice == value)
                    return;
                this._audioConversionChoice = value;
                this.FirePropertyChanged(nameof(AudioConversionChoice));
            }
        }

        public string AudioThresholdBitRate
        {
            get
            {
                if (this._audioThresholdBitRate == null)
                    this._audioThresholdBitRate = this.ActiveDevice.AudioTranscodeLimit.ToString();
                return this._audioThresholdBitRate;
            }
            set
            {
                if (!(this._audioThresholdBitRate != value))
                    return;
                this._audioThresholdBitRate = value;
                ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnAudioConversionChoiceCommit)] = (object)this.CommitDeviceID;
                this.FirePropertyChanged(nameof(AudioThresholdBitRate));
            }
        }

        public string AudioTargetBitRate
        {
            get
            {
                if (this._audioTargetBitRate == null)
                    this._audioTargetBitRate = this.ActiveDevice.AudioTranscodeTarget.ToString();
                return this._audioTargetBitRate;
            }
            set
            {
                if (!(this._audioTargetBitRate != value))
                    return;
                this._audioTargetBitRate = value;
                ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnAudioConversionChoiceCommit)] = (object)this.CommitDeviceID;
                this.FirePropertyChanged(nameof(AudioTargetBitRate));
            }
        }

        private void OnAudioConversionChoiceCommit(object data)
        {
            int result = 0;
            if (this.AudioConversionChoice.ChosenIndex == 1)
            {
                int.TryParse(this._audioThresholdBitRate, out result);
                this.ActiveDevice.AudioTranscodeLimit = result;
            }
            else
                this.ActiveDevice.AudioTranscodeLimit = -1;
            int.TryParse(this._audioTargetBitRate, out result);
            this.ActiveDevice.AudioTranscodeTarget = result;
        }

        public Choice VideoConversionChoice
        {
            get
            {
                if (this._videoConversionChoice == null)
                {
                    this._videoConversionOptions = (IList<Command>)new List<Command>();
                    this._videoConversionOptions.Add(new Command((IModelItemOwner)this, Shell.LoadString(StringId.IDS_TRANSCODE_VIDEO_PLAYBACK_OPTION), (EventHandler)null));
                    this._videoConversionOptions.Add(new Command((IModelItemOwner)this, Shell.LoadString(StringId.IDS_TRANSCODE_VIDEO_TV_OUT_OPTION), (EventHandler)null));
                    this._videoConversionChoice = new Choice((IModelItemOwner)this);
                    this._videoConversionChoice.Options = (IList)this._videoConversionOptions;
                    this._videoConversionChoice.DefaultIndex = this.ActiveDevice.OptimizeVideoForTV ? 1 : 0;
                    this._videoConversionChoice.DefaultValue();
                    this._videoConversionChoice.ChosenChanged += (EventHandler)((sender, args) => ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnVideoConversionChoiceCommit)] = (object)this.CommitDeviceID);
                }
                return this._videoConversionChoice;
            }
            private set
            {
                if (this._videoConversionChoice == value)
                    return;
                this._videoConversionChoice = value;
                this.FirePropertyChanged(nameof(VideoConversionChoice));
            }
        }

        private void OnVideoConversionChoiceCommit(object data) => this.ActiveDevice.OptimizeVideoForTV = this._videoConversionChoice.ChosenIndex == 1;

        public IList<Command> BitRateList
        {
            get
            {
                if (this._bitRateList == null)
                {
                    this._bitRateList = (IList<Command>)new List<Command>(this._defaultBitRates.Length);
                    for (int index = 0; index < this._defaultBitRates.Length; ++index)
                        this._bitRateList.Add(new Command((IModelItemOwner)null, this._defaultBitRates[index].ToString(), (EventHandler)null));
                }
                return this._bitRateList;
            }
        }

        public float ReservedSpaceOnDevice
        {
            get
            {
                if (float.IsNaN(this._reservedSpaceOnDevice))
                    this._reservedSpaceOnDevice = (float)this.ActiveDevice.PercentReserved;
                return this._reservedSpaceOnDevice;
            }
            set
            {
                if ((double)this._reservedSpaceOnDevice == (double)value)
                    return;
                ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnReservedSpaceOnDeviceCommit)] = (object)this.CommitDeviceID;
                this._reservedSpaceOnDevice = value;
            }
        }

        private void OnReservedSpaceOnDeviceCommit(object data) => this.ActiveDevice.PercentReserved = (int)this._reservedSpaceOnDevice;

        public SyncGroupList SyncGroupList
        {
            get
            {
                if (this._syncGroups == null)
                    this._syncGroups = this.ActiveDevice.GenerateSyncGroupList((IModelItemOwner)this, false);
                return this._syncGroups;
            }
        }

        public BooleanChoice PrivacyChoice
        {
            get
            {
                if (this._privacyChoice == null)
                {
                    this._privacyChoice = new BooleanChoice((IModelItemOwner)this, Shell.LoadString(StringId.IDS_DEVICE_USAGE_DATA_CHECK));
                    this._privacyChoice.Value = this.ActiveDevice.EnableWatson;
                    this._privacyChoice.ChosenChanged += (EventHandler)((sender, args) => ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnPrivacyChoiceCommit)] = (object)this.CommitDeviceID);
                }
                return this._privacyChoice;
            }
        }

        private void OnPrivacyChoiceCommit(object data) => this.ActiveDevice.EnableWatson = this.PrivacyChoice.Value;

        public void AutomatePrivacy(bool enable)
        {
            this.ActiveDevice.EnableWatson = enable;
            ZuneShell.DefaultInstance.Management.CommitList[(object)new ProxySettingDelegate(this.OnPrivacyChoiceCommit)] = (object)this.CommitDeviceID;
        }

        private int CommitDeviceID => !this.ActiveDevice.IsGuest ? this.ActiveDevice.ID : -1;
    }
}
