// Decompiled with JetBrains decompiler
// Type: ZuneUI.Management
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Subscription;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using UIXControls;

namespace ZuneUI
{
    public class Management : ModelItem
    {
        private static CategoryPage _currentCategoryPage;
        private Category _alertedDeviceCategory;
        private DeviceManagement _deviceManagement;
        private bool _deviceManagementLocked;
        private CommitListHashtable _commitList;
        private bool _hasPendingCommits;
        private bool _changeRequiresElevation;
        private BooleanChoice _sqmChoice;
        private ListDataSet _monitoredAudioFolders;
        private ListDataSet _monitoredPhotoFolders;
        private ListDataSet _monitoredPodcastFolders;
        private ListDataSet _monitoredVideoFolders;
        private BooleanChoice _mediaInfoChoice;
        private BooleanChoice _metadataChoice;
        private List<Management.MonitoredFolder> _removedMonitoredFoldersToRemoveFromCollection;
        private ProxySettingDelegate[] _actionsToCommitOnLibraryIntegrate;
        private Choice _podcastDefaultKeepEpisodesChoice;
        private Choice _podcastPlaybackChoice;
        private string[] _defaultFileTypeExtensions = new string[7]
        {
      ".mp3",
      ".m4a",
      ".mp4",
      ".m4b",
      ".m4v",
      ".mbr",
      ".zpl"
        };
        private Microsoft.Zune.Configuration.IFileAssociationHandler _fileAssocHandler;
        private IList<BooleanInputChoice> _allFileTypes;
        private IList<BooleanInputChoice> _audioFileTypes;
        private IList<BooleanInputChoice> _videoFileTypes;
        private IList<Microsoft.Zune.Configuration.FileAssociationInfo> _fileAssociationInfoList;
        private bool _canFileAssociationBeChanged;
        private ITunerInfoHandler _tunerHandler;
        private ArrayListDataSet _registeredComputersModelList;
        private ArrayListDataSet _registeredDevicesModelList;
        private ArrayListDataSet _registeredAppStoreDevicesModelList;
        private string _nextPCDeregistrationDate;
        private string _nextSubscriptionDeviceDeregistrationDate;
        private string _nextAppStoreDeviceDeregistrationDate;
        private IntRangedValue _slideShowSpeed;
        private Choice _burnDiscFormat;
        private BooleanChoice _autoEjectCDAfterBurn;
        private Choice _burnSpeed;
        private Choice _recordMode;
        private Choice _recordRate;
        private BooleanChoice _autoCopyCD;
        private BooleanChoice _autoEjectCD;
        private string _mediaFolder;
        private string _videoMediaFolder;
        private string _photoMediaFolder;
        private string _podcastMediaFolder;
        private Choice _wmaRate;
        private Choice _wmavRate;
        private Choice _mp3Rate;
        private string _sharingError;
        private string _sharingDisplayName;
        private BooleanChoice _sharingEnableMusic;
        private BooleanChoice _sharingEnableVideo;
        private BooleanChoice _sharingEnablePhoto;
        private Choice _sharingSelectDeviceChoice;
        private IList<Command> _sharingSelectDeviceOptions;
        private bool _sharingAllDevicesEnabled;
        private IList<BooleanInputChoice> _sharingDeviceList;
        private uint _sharingDeviceIndex;
        private HMESettings _HME;
        private bool _nssDeviceListChangeEventAdded;
        private string _backgroundImage;
        private WindowColor _backgroundColor;
        private BooleanChoice _showNowPlayingBackgroundOnIdle;
        private Choice _screenGraphicsSlider;
        private BooleanChoice _playSounds;
        private BooleanChoice _compactModeAlwaysOnTop;
        private BooleanChoice _ratingsChoice;
        private BooleanChoice _applyRatingsChoice;
        private Choice _startupPageChoice;
        private bool _autoLaunchZuneOnConnect;
        private object mylock = new object();

        public Management()
        {
            this._autoLaunchZuneOnConnect = ClientConfiguration.Devices.AutoLaunchZuneOnConnect;
            ClientConfiguration.Groveler.OnConfigurationChanged += new ConfigurationChangeEventHandler(this.OnGrovelerConfigurationChanged);
            this._actionsToCommitOnLibraryIntegrate = new ProxySettingDelegate[5]
            {
        new ProxySettingDelegate(this.OnMonitoredFoldersCommit),
        new ProxySettingDelegate(this.OnMediaFolderCommit),
        new ProxySettingDelegate(this.OnVideoMediaFolderCommit),
        new ProxySettingDelegate(this.OnPhotoMediaFolderCommit),
        new ProxySettingDelegate(this.OnPodcastMediaFolderCommit)
            };
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeDeviceManagement(true);
                ClientConfiguration.Groveler.OnConfigurationChanged -= new ConfigurationChangeEventHandler(this.OnGrovelerConfigurationChanged);
            }
            if (this._fileAssocHandler != null)
            {
                ((IDisposable)this._fileAssocHandler).Dispose();
                this._fileAssocHandler = (Microsoft.Zune.Configuration.IFileAssociationHandler)null;
            }
            base.OnDispose(disposing);
        }

        private Microsoft.Zune.Configuration.IFileAssociationHandler FileAssocHandler
        {
            get
            {
                if (this._fileAssocHandler == null)
                    this._fileAssocHandler = FileAssociationHandlerFactory.CreateFileAssociationHandler();
                return this._fileAssocHandler;
            }
        }

        public DeviceManagement DeviceManagement
        {
            get
            {
                if (this._deviceManagement == null && !this.DeviceManagementLocked)
                    this._deviceManagement = new DeviceManagement();
                return this._deviceManagement;
            }
        }

        public bool DeviceManagementLocked
        {
            get => this._deviceManagementLocked;
            private set
            {
                if (this._deviceManagementLocked == value)
                    return;
                this._deviceManagementLocked = value;
                this.FirePropertyChanged(nameof(DeviceManagementLocked));
            }
        }

        public void DisposeDeviceManagement(bool deviceManagementLocked)
        {
            this.DeviceManagementLocked = deviceManagementLocked;
            if (this._deviceManagement == null)
                return;
            UIDevice currentDeviceOverride = SyncControls.Instance.CurrentDeviceOverride;
            if (currentDeviceOverride.IsValid)
            {
                this.CommitList.RemoveByIntValue(currentDeviceOverride.IsGuest ? -1 : currentDeviceOverride.ID);
                this.CommitList.RemoveByStringValue("OnSyncPartnershipCommit");
            }
            else
                this.CommitList.RemoveByIntValue(-1);
            this._deviceManagement.Dispose();
            this._deviceManagement = (DeviceManagement)null;
            this.FirePropertyChanged("DeviceManagementChanged");
        }

        public bool DeviceManagementChanged => true;

        public CommitListHashtable CommitList
        {
            get
            {
                if (this._commitList == null)
                    this._commitList = new CommitListHashtable();
                return this._commitList;
            }
            set
            {
                if (this._commitList == value)
                    return;
                this._commitList = value;
                this.FirePropertyChanged(nameof(CommitList));
                if (value != null)
                    return;
                this.HasPendingCommits = false;
            }
        }

        public bool HasPendingCommits
        {
            get => this._hasPendingCommits;
            internal set
            {
                if (this._hasPendingCommits == value)
                    return;
                this._hasPendingCommits = value;
                this.FirePropertyChanged(nameof(HasPendingCommits));
            }
        }

        public bool ActiveDeviceHasPendingCommits => this.CommitList.ContainsIntValue(SyncControls.Instance.CurrentDevice.ID);

        public bool ChangeRequiresElevation
        {
            get => this._changeRequiresElevation;
            set
            {
                if (this._changeRequiresElevation == value)
                    return;
                this._changeRequiresElevation = value;
                this.FirePropertyChanged(nameof(ChangeRequiresElevation));
            }
        }

        public string BuildNumber => VersionInfo.BuildNumber;

        public Choice RecordMode
        {
            get
            {
                if (this._recordMode == null)
                {
                    this._wmaRate = new Choice((IModelItemOwner)this);
                    this._wmaRate.Options = (IList)new NamedIntOption[6]
                    {
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_WMA_48), 48000),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_WMA_64), 64000),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_WMA_96), 96000),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_WMA_128), 128000),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_WMA_160), 160000),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_WMA_192), 192000)
                    };
                    NamedIntOption.SelectOptionByValue(this._wmaRate, ClientConfiguration.Recorder.WMARecordRate);
                    this._wmaRate.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnWmaRateCommit)] = (object)null);
                    this._wmavRate = new Choice((IModelItemOwner)this);
                    this._wmavRate.Options = (IList)new NamedIntOption[5]
                    {
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_WMAV_25), 25),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_WMAV_50), 50),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_WMAV_75), 75),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_WMAV_90), 90),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_WMAV_98), 98)
                    };
                    NamedIntOption.SelectOptionByValue(this._wmavRate, ClientConfiguration.Recorder.WMAVBRRecordQuality);
                    this._wmavRate.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnWmavRateCommit)] = (object)null);
                    this._mp3Rate = new Choice((IModelItemOwner)this);
                    this._mp3Rate.Options = (IList)new NamedIntOption[4]
                    {
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_MP3_128), 128000),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_MP3_192), 192000),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_MP3_256), 256000),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_RIP_MP3_320), 320000)
                    };
                    NamedIntOption.SelectOptionByValue(this._mp3Rate, ClientConfiguration.Recorder.MP3RecordRate);
                    this._mp3Rate.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnMp3RateCommit)] = (object)null);
                    this._recordMode = new Choice((IModelItemOwner)this);
                    this._recordMode.Options = (IList)new RecordModeOption[4]
                    {
            new RecordModeOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_WMA_OPTION), 0, this._wmaRate),
            new RecordModeOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_WMA_VARIABLE_OPTION), 3, this._wmavRate),
            new RecordModeOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_WMA_LOSSLESS_OPTION), 1, (Choice) null),
            new RecordModeOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_MP3_OPTION), 2, this._mp3Rate)
                    };
                    NamedIntOption.SelectOptionByValue(this._recordMode, ClientConfiguration.Recorder.RecordMode);
                    this._recordMode.ChosenChanged += (EventHandler)((sender, args) =>
                   {
                       this.CommitList[(object)new ProxySettingDelegate(this.OnRecordModeCommit)] = (object)null;
                       this.RecordRate = ((RecordModeOption)this._recordMode.ChosenValue).BitRate;
                   });
                    this.RecordRate = ((RecordModeOption)this._recordMode.ChosenValue).BitRate;
                }
                return this._recordMode;
            }
        }

        private void OnWmaRateCommit(object data) => ClientConfiguration.Recorder.WMARecordRate = ((NamedIntOption)this._wmaRate.ChosenValue).Value;

        private void OnWmavRateCommit(object data) => ClientConfiguration.Recorder.WMAVBRRecordQuality = ((NamedIntOption)this._wmavRate.ChosenValue).Value;

        private void OnMp3RateCommit(object data) => ClientConfiguration.Recorder.MP3RecordRate = ((NamedIntOption)this._mp3Rate.ChosenValue).Value;

        private void OnRecordModeCommit(object data) => ClientConfiguration.Recorder.RecordMode = ((NamedIntOption)this._recordMode.ChosenValue).Value;

        public Choice RecordRate
        {
            get => this._recordRate;
            private set
            {
                if (this._recordRate == value)
                    return;
                this._recordRate = value;
                this.FirePropertyChanged(nameof(RecordRate));
            }
        }

        public Category AlertedDeviceCategory
        {
            get => this._alertedDeviceCategory;
            set
            {
                if (this._alertedDeviceCategory == value)
                    return;
                if (Management._currentCategoryPage != null && value != null && this._alertedDeviceCategory != null)
                {
                    Management._currentCategoryPage.CurrentCategory = this._alertedDeviceCategory;
                    this._alertedDeviceCategory = (Category)null;
                }
                else
                {
                    this._alertedDeviceCategory = value;
                    this.FirePropertyChanged(nameof(AlertedDeviceCategory));
                }
            }
        }

        public CategoryPage CurrentCategoryPage
        {
            get => Management._currentCategoryPage;
            set
            {
                if (Management._currentCategoryPage == value)
                    return;
                Management._currentCategoryPage = value;
                this.FirePropertyChanged(nameof(CurrentCategoryPage));
            }
        }

        public static void NavigateToSetupLandWizard(SetupLandPage page) => Management.NavigateAwayFromCategory((Command)new SetupLandWizardNavigationCommand(page));

        public static void NavigateToCategory(Category category)
        {
            if (ZuneShell.DefaultInstance.Management.CurrentCategoryPage == null)
                return;
            ZuneShell.DefaultInstance.Management.CurrentCategoryPage.CurrentCategory = category;
        }

        public static void NavigateAwayFromCategory(Command confirmed)
        {
            Management management = ZuneShell.DefaultInstance.Management;
            if (management.HasPendingCommits)
            {
                Command yesCommand = new Command((IModelItemOwner)null, ZuneUI.Shell.LoadString(StringId.IDS_DIALOG_YES), (EventHandler)null);
                yesCommand.Invoked += (EventHandler)((sender, args) =>
               {
                   management.CommitListSave();
                   Management.NavigateAwayFromCategory(confirmed);
               });
                Command noCommand = new Command((IModelItemOwner)null, ZuneUI.Shell.LoadString(StringId.IDS_DIALOG_NO), (EventHandler)null);
                noCommand.Invoked += (EventHandler)((sender, args) =>
               {
                   management.CommitList = (CommitListHashtable)null;
                   Management.NavigateAwayFromCategory(confirmed);
               });
                MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_SAVE_CHANGES_DIALOG_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_SAVE_CHANGES_ON_BACK_DIALOG_TEXT), yesCommand, noCommand, (BooleanChoice)null);
            }
            else
            {
                if (ZuneUI.Shell.SettingsFrame.IsCurrent && !ZuneUI.Shell.SettingsFrame.Wizard.FUE.IsCurrent && management.CurrentCategoryPage != null)
                    management.CurrentCategoryPage.CancelAndExit();
                Application.DeferredInvoke((DeferredInvokeHandler)delegate
               {
                   confirmed?.Invoke();
               }, (object)null);
            }
        }

        public void CommitListSave()
        {
            this.CheckForAutomatedRequirements();
            this.CommitList.Save();
        }

        public void CheckForAutomatedRequirements()
        {
            if (DeviceManagement.SetupDevice == null)
                return;
            this.DeviceManagement.CheckForAutomatedRequirements();
        }

        public bool CanFileAssociationBeChanged
        {
            get => this._canFileAssociationBeChanged;
            private set
            {
                if (this._canFileAssociationBeChanged == value)
                    return;
                this._canFileAssociationBeChanged = value;
                this.FirePropertyChanged(nameof(CanFileAssociationBeChanged));
            }
        }

        public SubscriptionState SubscribeToChannelFeed(
          bool isPersonalChannel,
          Guid channelId,
          string feedUrl,
          string title,
          ESubscriptionSource source)
        {
            return this.SubscribeToFeed(feedUrl, title, channelId, isPersonalChannel, source, EMediaTypes.eMediaTypePlaylist, ZuneUI.Shell.LoadString(StringId.IDS_PLAYLIST_SUBSCRIPTION_ERROR));
        }

        public SubscriptionState SubscribeToPodcastFeed(
          string feedUrl,
          string title,
          ESubscriptionSource source)
        {
            return this.SubscribeToPodcastFeed(feedUrl, title, Guid.Empty, source);
        }

        public SubscriptionState SubscribeToPodcastFeed(
          string feedUrl,
          string title,
          Guid serviceId,
          ESubscriptionSource source)
        {
            return this.SubscribeToFeed(feedUrl, title, serviceId, false, source, EMediaTypes.eMediaTypePodcastSeries, ZuneUI.Shell.LoadString(StringId.IDS_PODCAST_SUBSCRIPTION_ERROR));
        }

        private SubscriptionState SubscribeToFeed(
          string feedUrl,
          string title,
          Guid serviceId,
          bool isPersonalChannel,
          ESubscriptionSource source,
          EMediaTypes mediaType,
          string errorDialogHeader)
        {
            int subscriptionMediaId = 0;
            SubscriptionState subscriptionState = (SubscriptionState)null;
            HRESULT hresult = (HRESULT)SubscriptionManager.Instance.Subscribe(feedUrl, title, serviceId, isPersonalChannel, mediaType, source, out subscriptionMediaId);
            if (hresult.IsSuccess)
                subscriptionState = new SubscriptionState(true, true, subscriptionMediaId);
            else
                ErrorDialogInfo.Show(hresult.Int, errorDialogHeader);
            return subscriptionState;
        }

        public SubscriptionState GetSubscriptionState(
          string feedURL,
          EMediaTypes subscriptionType)
        {
            if (string.IsNullOrEmpty(feedURL))
                return (SubscriptionState)null;
            try
            {
                int subscriptionMediaId = -1;
                bool isSubscribed;
                bool byUrl = SubscriptionManager.Instance.FindByUrl(feedURL, subscriptionType, out subscriptionMediaId, out isSubscribed);
                return new SubscriptionState(isSubscribed, byUrl, subscriptionMediaId);
            }
            catch (ApplicationException ex)
            {
            }
            return (SubscriptionState)null;
        }

        public SubscriptionState GetSubscriptionState(
          Guid serviceId,
          EMediaTypes subscriptionType)
        {
            if (serviceId == Guid.Empty)
                return (SubscriptionState)null;
            try
            {
                int subscriptionMediaId = -1;
                bool isSubscribed;
                bool byServiceId = SubscriptionManager.Instance.FindByServiceId(serviceId, subscriptionType, out subscriptionMediaId, out isSubscribed);
                return new SubscriptionState(isSubscribed, byServiceId, subscriptionMediaId);
            }
            catch (ApplicationException ex)
            {
            }
            return (SubscriptionState)null;
        }

        private void OnGrovelerConfigurationChanged(object sender, ConfigurationChangeEventArgs e) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (!this.UsingWin7Libraries)
               return;
           if (e.PropertyName == "RipDirectory" || e.PropertyName == "MonitoredAudioFolders")
           {
               if (this._monitoredAudioFolders != null)
               {
                   this._monitoredAudioFolders = (ListDataSet)null;
                   this.FirePropertyChanged("MonitoredAudioFolders");
               }
               if (this._mediaFolder == null)
                   return;
               this._mediaFolder = (string)null;
               this.FirePropertyChanged("MediaFolder");
           }
           else if (e.PropertyName == "PhotoMediaFolder" || e.PropertyName == "MonitoredPhotoFolders")
           {
               if (this._monitoredPhotoFolders != null)
               {
                   this._monitoredPhotoFolders = (ListDataSet)null;
                   this.FirePropertyChanged("MonitoredPhotoFolders");
               }
               if (this._photoMediaFolder == null)
                   return;
               this._photoMediaFolder = (string)null;
               this.FirePropertyChanged("PhotoMediaFolder");
           }
           else if (e.PropertyName == "PodcastMediaFolder" || e.PropertyName == "MonitoredPodcastFolders")
           {
               if (this._monitoredPodcastFolders != null)
               {
                   this._monitoredPodcastFolders = (ListDataSet)null;
                   this.FirePropertyChanged("MonitoredPodcastFolders");
               }
               if (this._podcastMediaFolder == null)
                   return;
               this._podcastMediaFolder = (string)null;
               this.FirePropertyChanged("PodcastMediaFolder");
           }
           else
           {
               if (!(e.PropertyName == "VideoMediaFolder") && !(e.PropertyName == "MonitoredVideoFolders"))
                   return;
               if (this._monitoredVideoFolders != null)
               {
                   this._monitoredVideoFolders = (ListDataSet)null;
                   this.FirePropertyChanged("MonitoredVideoFolders");
               }
               if (this._videoMediaFolder == null)
                   return;
               this._videoMediaFolder = (string)null;
               this.FirePropertyChanged("VideoMediaFolder");
           }
       }, (object)null);

        public ListDataSet MonitoredAudioFolders
        {
            get
            {
                if (this._monitoredAudioFolders == null)
                {
                    if (this.UsingWin7Libraries)
                        this._monitoredAudioFolders = this.StringsToListDataSet((object)ClientConfiguration.Groveler.RipDirectory, (object)ClientConfiguration.Groveler.MonitoredAudioFolders);
                    else
                        this._monitoredAudioFolders = this.StringsToListDataSet((object)ClientConfiguration.Groveler.MonitoredAudioFolders);
                }
                return this._monitoredAudioFolders;
            }
        }

        public ListDataSet MonitoredPhotoFolders
        {
            get
            {
                if (this._monitoredPhotoFolders == null)
                {
                    if (this.UsingWin7Libraries)
                        this._monitoredPhotoFolders = this.StringsToListDataSet((object)ClientConfiguration.Groveler.PhotoMediaFolder, (object)ClientConfiguration.Groveler.MonitoredPhotoFolders);
                    else
                        this._monitoredPhotoFolders = this.StringsToListDataSet((object)ClientConfiguration.Groveler.MonitoredPhotoFolders);
                }
                return this._monitoredPhotoFolders;
            }
        }

        public ListDataSet MonitoredPodcastFolders
        {
            get
            {
                if (this._monitoredPodcastFolders == null)
                {
                    if (this.UsingWin7Libraries)
                        this._monitoredPodcastFolders = this.StringsToListDataSet((object)ClientConfiguration.Groveler.PodcastMediaFolder, (object)ClientConfiguration.Groveler.MonitoredPodcastFolders);
                    else
                        this._monitoredPodcastFolders = this.StringsToListDataSet((object)ClientConfiguration.Groveler.MonitoredPodcastFolders);
                }
                return this._monitoredPodcastFolders;
            }
        }

        public ListDataSet MonitoredVideoFolders
        {
            get
            {
                if (this._monitoredVideoFolders == null)
                {
                    if (this.UsingWin7Libraries)
                        this._monitoredVideoFolders = this.StringsToListDataSet((object)ClientConfiguration.Groveler.VideoMediaFolder, (object)ClientConfiguration.Groveler.MonitoredVideoFolders);
                    else
                        this._monitoredVideoFolders = this.StringsToListDataSet((object)ClientConfiguration.Groveler.MonitoredVideoFolders);
                }
                return this._monitoredVideoFolders;
            }
        }

        public bool Win7LibrariesAreAvailable => OSVersion.IsWin7();

        public bool UsingWin7Libraries => this.Win7LibrariesAreAvailable && ClientConfiguration.Groveler.LibrarySync != -1;

        public void UseWin7Libraries()
        {
            foreach (ProxySettingDelegate proxySettingDelegate in this._actionsToCommitOnLibraryIntegrate)
            {
                if (this.CommitList.ContainsKey((object)proxySettingDelegate))
                {
                    this.CommitList.Remove((object)proxySettingDelegate);
                    proxySettingDelegate((object)null);
                }
            }
            SQMLog.Log(SQMDataId.ZuneWin7LibraryOpt, 0);
            this.SetWin7LibrariesUsage(Win7LibrariesUsage.BeginIntegration);
        }

        public void DoNotUseWin7Libraries()
        {
            SQMLog.Log(SQMDataId.ZuneWin7LibraryOpt, 1);
            this.SetWin7LibrariesUsage(Win7LibrariesUsage.DoNotIntegrate);
        }

        private void SetWin7LibrariesUsage(Win7LibrariesUsage usage)
        {
            ClientConfiguration.Groveler.LibrarySync = (int)usage;
            this.FirePropertyChanged("UsingWin7Libraries");
        }

        private ListDataSet StringsToListDataSet(params object[] source)
        {
            ListDataSet listDataSet = (ListDataSet)new ArrayListDataSet((IModelItemOwner)this);
            if (source != null && source.Length > 0)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (object obj in source)
                {
                    if (obj != null)
                    {
                        if (!(obj is IEnumerable<string> strings))
                            strings = (IEnumerable<string>)new string[1]
                            {
                obj.ToString()
                            };
                        if (strings != null)
                        {
                            foreach (string str in strings)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    string lower = str.ToLower();
                                    if (!dictionary.ContainsKey(lower))
                                    {
                                        dictionary.Add(lower, (object)null);
                                        listDataSet.Add((object)str);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            listDataSet.Sort();
            return listDataSet;
        }

        private IList<string> ListDataSetToIList(ListDataSet listDataSet)
        {
            if (listDataSet == null)
                return (IList<string>)new List<string>();
            IList<string> stringList = (IList<string>)new List<string>(listDataSet.Count);
            for (int itemIndex = 0; itemIndex < listDataSet.Count; ++itemIndex)
                stringList.Add((string)listDataSet[itemIndex]);
            return stringList;
        }

        internal bool IsMonitored(ListDataSet monitoredFolders, string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            foreach (string monitoredFolder in monitoredFolders)
            {
                if (this.IsSubfolder(monitoredFolder, path))
                    return true;
            }
            return false;
        }

        private bool IsSubfolder(string root, string subfolder)
        {
            try
            {
                for (; subfolder != null; subfolder = Path.GetDirectoryName(subfolder))
                {
                    if (subfolder.Equals(root, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            catch (ArgumentException ex)
            {
            }
            catch (PathTooLongException ex)
            {
            }
            return false;
        }

        public void AddMonitoredFolder(ListDataSet monitoredFolders) => FolderBrowseDialog.Show(ZuneUI.Shell.LoadString(StringId.IDS_ADD_MONITORED_FOLDER_DIALOG_TITLE), (DeferredInvokeHandler)(args =>
       {
           if (args == null)
               return;
           string str = (string)args;
           if (ZuneApplication.ZuneLibrary.CanAddFromFolder(str))
               this.AddMonitoredFolder(monitoredFolders, str, false);
           else
               MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_INVALID_MONITORED_FOLDER_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_INVALID_MONITORED_FOLDER_MESSAGE), (EventHandler)null);
       }));

        public void AddMonitoredFolder(ListDataSet monitoredFolders, string path, bool commit)
        {
            bool flag = false;
            for (int itemIndex = 0; itemIndex < monitoredFolders.Count; ++itemIndex)
            {
                if (monitoredFolders[itemIndex].Equals((object)path))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
                monitoredFolders.Add((object)path);
            this.SaveMonitoredFolders(commit);
        }

        public void OpenMediaFile() => FileOpenDialog.Show(ZuneUI.Shell.LoadString(StringId.IDS_OPEN_FILE_DIALOG_TITLE), this.MediaFolder, (DeferredInvokeHandler)(args => { }));

        public bool RemoveChildMonitoredFolders(string path, bool commit)
        {
            bool flag = false | this.RemoveChildMonitoredFolders(this.MonitoredAudioFolders, EMediaTypes.eMediaTypeAudio, path) | this.RemoveChildMonitoredFolders(this.MonitoredPhotoFolders, EMediaTypes.eMediaTypeImage, path) | this.RemoveChildMonitoredFolders(this.MonitoredPodcastFolders, EMediaTypes.eMediaTypePodcastEpisode, path) | this.RemoveChildMonitoredFolders(this.MonitoredVideoFolders, EMediaTypes.eMediaTypeVideo, path);
            this.SaveMonitoredFolders(commit);
            return flag;
        }

        private bool RemoveChildMonitoredFolders(
          ListDataSet monitoredFolders,
          EMediaTypes type,
          string path)
        {
            EWin7LibraryKind libraryKind;
            switch (type)
            {
                case EMediaTypes.eMediaTypeAudio:
                    libraryKind = EWin7LibraryKind.eMusicLibrary;
                    break;
                case EMediaTypes.eMediaTypeVideo:
                    libraryKind = EWin7LibraryKind.eVideoLibrary;
                    break;
                case EMediaTypes.eMediaTypeImage:
                    libraryKind = EWin7LibraryKind.ePicturesLibrary;
                    break;
                case EMediaTypes.eMediaTypePodcastEpisode:
                    libraryKind = EWin7LibraryKind.ePodcastLibrary;
                    break;
                default:
                    return false;
            }
            List<int> intList = new List<int>();
            for (int itemIndex = 0; itemIndex < monitoredFolders.Count; ++itemIndex)
            {
                if (this.IsSubfolder(path, (string)monitoredFolders[itemIndex]))
                    intList.Add(itemIndex);
            }
            foreach (int num in intList)
            {
                if (this.UsingWin7Libraries)
                    Win7ShellManager.Instance.RemoveLocationFromLibrary(libraryKind, out bool _, (string)monitoredFolders[num]);
                else
                    this.RemoveMonitoredFolder(monitoredFolders, num, type);
            }
            return intList.Count > 0;
        }

        public void RemoveMonitoredFolder(ListDataSet monitoredFolders, string path, bool commit)
        {
            for (int index = 0; index < monitoredFolders.Count; ++index)
            {
                if (path.Equals((string)monitoredFolders[index], StringComparison.OrdinalIgnoreCase))
                {
                    this.RemoveMonitoredFolder(monitoredFolders, index, commit);
                    break;
                }
            }
        }

        private void RemoveMonitoredFolder(ListDataSet monitoredFolders, int index) => this.RemoveMonitoredFolder(monitoredFolders, index, false);

        public void RemoveMonitoredFolder(
          ListDataSet monitoredFolders,
          int index,
          EMediaTypes mediaType)
        {
            if (this._removedMonitoredFoldersToRemoveFromCollection == null)
                this._removedMonitoredFoldersToRemoveFromCollection = new List<Management.MonitoredFolder>();
            this._removedMonitoredFoldersToRemoveFromCollection.Add(new Management.MonitoredFolder((string)monitoredFolders[index], mediaType));
            this.RemoveMonitoredFolder(monitoredFolders, index, false);
        }

        private void RemoveMonitoredFolder(ListDataSet monitoredFolders, int index, bool commit)
        {
            monitoredFolders.RemoveAt(index);
            this.SaveMonitoredFolders(commit);
        }

        private void SaveMonitoredFolders() => this.SaveMonitoredFolders(false);

        public void SaveMonitoredFolders(bool commit)
        {
            if (commit)
                this.OnMonitoredFoldersCommit((object)null);
            else
                this.CommitList[(object)new ProxySettingDelegate(this.OnMonitoredFoldersCommit)] = (object)null;
        }

        public void OpenLibraryDialog(EMediaTypes mediaType)
        {
            IntPtr winHandle = Application.Window.Handle;
            Thread thread = new Thread((ParameterizedThreadStart)(args =>
           {
               EWin7LibraryKind libraryKind = EWin7LibraryKind.eMusicLibrary;
               switch (mediaType)
               {
                   case EMediaTypes.eMediaTypeAudio:
                       libraryKind = EWin7LibraryKind.eMusicLibrary;
                       break;
                   case EMediaTypes.eMediaTypeVideo:
                       libraryKind = EWin7LibraryKind.eVideoLibrary;
                       break;
                   case EMediaTypes.eMediaTypeImage:
                       libraryKind = EWin7LibraryKind.ePicturesLibrary;
                       break;
                   case EMediaTypes.eMediaTypePodcastEpisode:
                       libraryKind = EWin7LibraryKind.ePodcastLibrary;
                       break;
               }
               Win7ShellManager.Instance.ShowLibraryDialog(libraryKind, winHandle, (string)null, (string)null);
           }));
            thread.TrySetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void OnMonitoredFoldersCommit(object data)
        {
            SQMLog.LogToStream(SQMDataId.MonitoredAudioFolders, (uint)this.MonitoredAudioFolders.Count);
            SQMLog.LogToStream(SQMDataId.MonitoredPhotoFolders, (uint)this.MonitoredPhotoFolders.Count);
            SQMLog.LogToStream(SQMDataId.MonitoredPodcastFolders, (uint)this.MonitoredPodcastFolders.Count);
            SQMLog.LogToStream(SQMDataId.MonitoredVideoFolders, (uint)this.MonitoredVideoFolders.Count);
            if (!this.UsingWin7Libraries)
            {
                if (this._removedMonitoredFoldersToRemoveFromCollection != null)
                {
                    foreach (Management.MonitoredFolder foldersToRemoveFrom in this._removedMonitoredFoldersToRemoveFromCollection)
                        ZuneApplication.ZuneLibrary.DeleteRootFolder(foldersToRemoveFrom.Path, foldersToRemoveFrom.Schema);
                }
                ClientConfiguration.Groveler.MonitoredAudioFolders = this.ListDataSetToIList(this.MonitoredAudioFolders);
                ClientConfiguration.Groveler.MonitoredPhotoFolders = this.ListDataSetToIList(this.MonitoredPhotoFolders);
                ClientConfiguration.Groveler.MonitoredPodcastFolders = this.ListDataSetToIList(this.MonitoredPodcastFolders);
                ClientConfiguration.Groveler.MonitoredVideoFolders = this.ListDataSetToIList(this.MonitoredVideoFolders);
                this._removedMonitoredFoldersToRemoveFromCollection = (List<Management.MonitoredFolder>)null;
                this._monitoredAudioFolders = (ListDataSet)null;
                this._monitoredPhotoFolders = (ListDataSet)null;
                this._monitoredPodcastFolders = (ListDataSet)null;
                this._monitoredVideoFolders = (ListDataSet)null;
            }
            if (!this.HME.SharingEnabled)
                return;
            this.HME.SetSharedFoldersList(true);
        }

        public BooleanChoice AutoCopyCD
        {
            get
            {
                if (this._autoCopyCD == null)
                {
                    this._autoCopyCD = new BooleanChoice((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_AUTO_RIP));
                    this._autoCopyCD.Value = ClientConfiguration.Recorder.AutoCopyCD != 0;
                    this._autoCopyCD.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnAutoCopyCDCommit)] = (object)null);
                }
                return this._autoCopyCD;
            }
        }

        private void OnAutoCopyCDCommit(object data) => ClientConfiguration.Recorder.AutoCopyCD = this._autoCopyCD.Value ? 1 : 0;

        public BooleanChoice AutoEjectCD
        {
            get
            {
                if (this._autoEjectCD == null)
                {
                    this._autoEjectCD = new BooleanChoice((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_EJECT_AFTER_RIP));
                    this._autoEjectCD.Value = ClientConfiguration.Recorder.AutoEjectCD != 0;
                    this._autoEjectCD.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnAutoEjectCDCommit)] = (object)null);
                }
                return this._autoEjectCD;
            }
        }

        private void OnAutoEjectCDCommit(object data) => ClientConfiguration.Recorder.AutoEjectCD = this._autoEjectCD.Value ? 1 : 0;

        public bool MediaFolderHasSharedPathWithMonitoredFolder(
          string monitoredFolder,
          string mediaFolder)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(monitoredFolder) && !string.IsNullOrEmpty(mediaFolder))
            {
                string localizedFolderPath1 = LocalizationHelper.GetLocalizedFolderPath(monitoredFolder);
                string localizedFolderPath2 = LocalizationHelper.GetLocalizedFolderPath(mediaFolder);
                if ((int)localizedFolderPath1[localizedFolderPath1.Length - 1] != (int)Path.PathSeparator)
                    localizedFolderPath1 += (string)(object)Path.PathSeparator;
                if ((int)localizedFolderPath2[localizedFolderPath2.Length - 1] != (int)Path.PathSeparator)
                    localizedFolderPath2 += (string)(object)Path.PathSeparator;
                flag = localizedFolderPath1.ToLower().IndexOf(localizedFolderPath2.ToLower()) == 0;
            }
            return flag;
        }

        public string MediaFolder
        {
            get
            {
                if (this._mediaFolder == null)
                    this._mediaFolder = LocalizationHelper.GetLocalizedFolderPath(ClientConfiguration.Groveler.RipDirectory);
                return this._mediaFolder;
            }
            set
            {
                if (!(this._mediaFolder != value))
                    return;
                this.CommitList[(object)new ProxySettingDelegate(this.OnMediaFolderCommit)] = (object)null;
                this._mediaFolder = value;
                this.FirePropertyChanged(nameof(MediaFolder));
            }
        }

        private void OnMediaFolderCommit(object data)
        {
            if (this.UsingWin7Libraries)
                return;
            ClientConfiguration.Groveler.RipDirectory = this._mediaFolder;
            this.UpdateSharedFoldersList();
            this._mediaFolder = (string)null;
        }

        public string VideoMediaFolder
        {
            get
            {
                if (this._videoMediaFolder == null)
                {
                    this._videoMediaFolder = ClientConfiguration.Groveler.VideoMediaFolder;
                    string[] strArray;
                    string str;
                    string videoMediaFolder;
                    if (string.IsNullOrEmpty(this._videoMediaFolder) && ((HRESULT)ZuneApplication.ZuneLibrary.GetKnownFolders(out strArray, out strArray, out strArray, out strArray, out strArray, out str, out videoMediaFolder, out str, out str, out str)).IsSuccess)
                        this._videoMediaFolder = LocalizationHelper.GetLocalizedFolderPath(videoMediaFolder);
                }
                return this._videoMediaFolder;
            }
            set
            {
                if (!(this._videoMediaFolder != value))
                    return;
                this.CommitList[(object)new ProxySettingDelegate(this.OnVideoMediaFolderCommit)] = (object)null;
                this._videoMediaFolder = value;
                this.FirePropertyChanged(nameof(VideoMediaFolder));
            }
        }

        private void OnVideoMediaFolderCommit(object data)
        {
            if (this.UsingWin7Libraries)
                return;
            ClientConfiguration.Groveler.VideoMediaFolder = this._videoMediaFolder;
            this.UpdateSharedFoldersList();
            this._videoMediaFolder = (string)null;
        }

        public string PhotoMediaFolder
        {
            get
            {
                if (this._photoMediaFolder == null)
                {
                    this._photoMediaFolder = ClientConfiguration.Groveler.PhotoMediaFolder;
                    string[] strArray;
                    string str;
                    string photoMediaFolder;
                    if (string.IsNullOrEmpty(this._photoMediaFolder) && ((HRESULT)ZuneApplication.ZuneLibrary.GetKnownFolders(out strArray, out strArray, out strArray, out strArray, out strArray, out str, out str, out photoMediaFolder, out str, out str)).IsSuccess)
                        this._photoMediaFolder = LocalizationHelper.GetLocalizedFolderPath(photoMediaFolder);
                }
                return this._photoMediaFolder;
            }
            set
            {
                if (!(this._photoMediaFolder != value))
                    return;
                this.CommitList[(object)new ProxySettingDelegate(this.OnPhotoMediaFolderCommit)] = (object)null;
                this._photoMediaFolder = value;
                this.FirePropertyChanged(nameof(PhotoMediaFolder));
            }
        }

        private void OnPhotoMediaFolderCommit(object data)
        {
            if (this.UsingWin7Libraries)
                return;
            ClientConfiguration.Groveler.PhotoMediaFolder = this._photoMediaFolder;
            this.UpdateSharedFoldersList();
            this._photoMediaFolder = (string)null;
        }

        public string PodcastMediaFolder
        {
            get
            {
                if (this._podcastMediaFolder == null)
                {
                    this._podcastMediaFolder = ClientConfiguration.Groveler.PodcastMediaFolder;
                    string[] strArray;
                    string str;
                    string podcastMediaFolder;
                    if (string.IsNullOrEmpty(this._podcastMediaFolder) && ((HRESULT)ZuneApplication.ZuneLibrary.GetKnownFolders(out strArray, out strArray, out strArray, out strArray, out strArray, out str, out str, out str, out podcastMediaFolder, out str)).IsSuccess)
                        this._podcastMediaFolder = LocalizationHelper.GetLocalizedFolderPath(podcastMediaFolder);
                }
                return this._podcastMediaFolder;
            }
            set
            {
                if (!(this._podcastMediaFolder != value))
                    return;
                this.CommitList[(object)new ProxySettingDelegate(this.OnPodcastMediaFolderCommit)] = (object)null;
                this._podcastMediaFolder = value;
                this.FirePropertyChanged(nameof(PodcastMediaFolder));
            }
        }

        private void OnPodcastMediaFolderCommit(object data)
        {
            if (this.UsingWin7Libraries)
                return;
            ClientConfiguration.Groveler.PodcastMediaFolder = this._podcastMediaFolder;
            this.UpdateSharedFoldersList();
            this._podcastMediaFolder = (string)null;
        }

        private void UpdateSharedFoldersList()
        {
            if (!this.HME.SharingEnabled)
                return;
            this.HME.SetSharedFoldersList(true);
        }

        public void ChooseMediaFolder(MediaType mediaType) => FolderBrowseDialog.Show(ZuneUI.Shell.LoadString(StringId.IDS_CHANGE_MEDIA_FOLDER_DIALOG_TITLE), (DeferredInvokeHandler)(args =>
       {
           string folder = (string)args;
           if (folder == null)
               return;
           if (FolderBrowseDialog.CanWriteToFolder(folder) && ZuneApplication.ZuneLibrary.CanAddFromFolder(folder))
           {
               switch (mediaType)
               {
                   case MediaType.Track:
                       this.MediaFolder = folder;
                       break;
                   case MediaType.Video:
                       this.VideoMediaFolder = folder;
                       break;
                   case MediaType.Photo:
                       this.PhotoMediaFolder = folder;
                       break;
                   case MediaType.Podcast:
                       this.PodcastMediaFolder = folder;
                       break;
               }
           }
           else
               MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_INVALID_MEDIA_FOLDER_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_INVALID_MEDIA_FOLDER_MESSAGE), (EventHandler)null);
       }), true);

        public BooleanChoice AutoEjectCDAfterBurn
        {
            get
            {
                if (this._autoEjectCDAfterBurn == null)
                {
                    this._autoEjectCDAfterBurn = new BooleanChoice((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_BURN_AUTO_EJECT_CHECK));
                    this._autoEjectCDAfterBurn.Value = ClientConfiguration.CDBurn.AutoEject;
                    this._autoEjectCDAfterBurn.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnAutoEjectCDAfterBurnCommit)] = (object)null);
                }
                return this._autoEjectCDAfterBurn;
            }
        }

        private void OnAutoEjectCDAfterBurnCommit(object data) => ClientConfiguration.CDBurn.AutoEject = this._autoEjectCDAfterBurn.Value;

        public Choice BurnFormat
        {
            get
            {
                if (this._burnDiscFormat == null)
                {
                    this._burnDiscFormat = new Choice((IModelItemOwner)this);
                    this._burnDiscFormat.Options = (IList)new NamedIntOption[2]
                    {
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_BURN_AUDIO_OPTION), 0),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_BURN_DATA_OPTION), 1)
                    };
                    NamedIntOption.SelectOptionByValue(this._burnDiscFormat, ClientConfiguration.CDBurn.DiscFormat);
                    this._burnDiscFormat.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnBurnDiscFormatCommit)] = (object)null);
                }
                return this._burnDiscFormat;
            }
        }

        private void OnBurnDiscFormatCommit(object data)
        {
            ClientConfiguration.CDBurn.DiscFormat = ((NamedIntOption)this._burnDiscFormat.ChosenValue).Value;
            CDAccess.Instance.UpdateIsAudioBurn();
        }

        public Choice BurnSpeed
        {
            get
            {
                if (this._burnSpeed == null)
                {
                    this._burnSpeed = new Choice((IModelItemOwner)this);
                    this._burnSpeed.Options = (IList)new NamedIntOption[4]
                    {
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_BURN_FASTEST_OPTION), 0),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_BURN_FAST_OPTION), 1),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_BURN_MEDIUM_OPTION), 2),
            new NamedIntOption((IModelItemOwner) null, ZuneUI.Shell.LoadString(StringId.IDS_BURN_SLOW_OPTION), 3)
                    };
                    NamedIntOption.SelectOptionByValue(this._burnSpeed, ClientConfiguration.CDBurn.BurnSpeed);
                    this._burnSpeed.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnBurnSpeedCommit)] = (object)null);
                }
                return this._burnSpeed;
            }
        }

        private void OnBurnSpeedCommit(object data) => ClientConfiguration.CDBurn.BurnSpeed = ((NamedIntOption)this._burnSpeed.ChosenValue).Value;

        public BooleanChoice MediaInfoChoice
        {
            get
            {
                if (this._mediaInfoChoice == null)
                {
                    StringId stringId = StringId.IDS_UPDATE_METADATA_CHECK;
                    if (FeatureEnablement.IsFeatureEnabled(Features.eQuickMixZmp) || FeatureEnablement.IsFeatureEnabled(Features.eQuickMixLocal))
                        stringId = !FeatureEnablement.IsFeatureEnabled(Features.eMixview) ? StringId.IDS_UPDATE_METADATA_QUICKMIX_CHECK : StringId.IDS_UPDATE_METADATA_FEATURES_CHECK;
                    this._mediaInfoChoice = new BooleanChoice((IModelItemOwner)this, ZuneUI.Shell.LoadString(stringId));
                    this._mediaInfoChoice.Value = ClientConfiguration.MediaStore.ConnectToInternetForAlbumMetadata;
                    this._mediaInfoChoice.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnMediaInfoChoiceCommit)] = (object)null);
                }
                return this._mediaInfoChoice;
            }
        }

        private void OnMediaInfoChoiceCommit(object data) => ClientConfiguration.MediaStore.ConnectToInternetForAlbumMetadata = this._mediaInfoChoice.Value;

        public void ScanAndClearDeletedMedia() => ZuneLibrary.ScanAndClearDeletedMedia();

        public BooleanChoice SqmChoice
        {
            get
            {
                if (this._sqmChoice == null)
                {
                    this._sqmChoice = new BooleanChoice((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_USAGE_DATA_CHECK));
                    this._sqmChoice.Value = ClientConfiguration.SQM.UsageTracking;
                    this._sqmChoice.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnSqmChoiceCommit)] = (object)null);
                }
                return this._sqmChoice;
            }
        }

        private void OnSqmChoiceCommit(object data)
        {
            ClientConfiguration.SQM.UsageTracking = this._sqmChoice.Value;
            ClientConfiguration.FUE.AcceptedPrivacyStatement = this._sqmChoice.Value;
        }

        public Choice PodcastDefaultKeepEpisodesChoice
        {
            get
            {
                if (this._podcastDefaultKeepEpisodesChoice == null)
                {
                    Choice choice = new Choice((IModelItemOwner)this);
                    choice.Options = (IList)NamedIntOption.PodcastKeepOptions;
                    NamedIntOption.SelectOptionByValue(choice, ClientConfiguration.Series.PodcastDefaultKeepEpisodes);
                    this._podcastDefaultKeepEpisodesChoice = choice;
                    choice.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnPodcastDefaultKeepEpisodesChoiceCommit)] = (object)null);
                }
                return this._podcastDefaultKeepEpisodesChoice;
            }
        }

        private void OnPodcastDefaultKeepEpisodesChoiceCommit(object data) => ClientConfiguration.Series.PodcastDefaultKeepEpisodes = ((NamedIntOption)this._podcastDefaultKeepEpisodesChoice.ChosenValue).Value;

        public Choice PodcastPlaybackChoice
        {
            get
            {
                if (this._podcastPlaybackChoice == null)
                {
                    this._podcastPlaybackChoice = new Choice((IModelItemOwner)this);
                    this._podcastPlaybackChoice.Options = (IList)NamedIntOption.PodcastPlaybackOptions;
                    NamedIntOption.SelectOptionByValue(this._podcastPlaybackChoice, ClientConfiguration.Series.PodcastDefaultPlaybackOrder);
                    this._podcastPlaybackChoice.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnPodcastPlaybackChoiceCommit)] = (object)null);
                }
                return this._podcastPlaybackChoice;
            }
        }

        private void OnPodcastPlaybackChoiceCommit(object data) => ClientConfiguration.Series.PodcastDefaultPlaybackOrder = ((NamedIntOption)this._podcastPlaybackChoice.ChosenValue).Value;

        public BooleanChoice MetadataChoice
        {
            get
            {
                if (this._metadataChoice == null)
                {
                    Command[] commandArray = new Command[2];
                    Command command1 = new Command((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_MISSING_METADATA), (EventHandler)null);
                    commandArray[0] = command1;
                    Command command2 = new Command((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_OVERWRITE_METADATA), (EventHandler)null);
                    commandArray[1] = command2;
                    this._metadataChoice = new BooleanChoice((IModelItemOwner)this);
                    this._metadataChoice.Options = (IList)commandArray;
                    this._metadataChoice.Value = ClientConfiguration.MediaStore.OverwriteAllMetadata;
                    this._metadataChoice.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnMetadataChoiceCommit)] = (object)null);
                }
                return this._metadataChoice;
            }
        }

        private void OnMetadataChoiceCommit(object data) => ClientConfiguration.MediaStore.OverwriteAllMetadata = this._metadataChoice.Value;

        public IList AudioFileTypes
        {
            get
            {
                if (this._audioFileTypes == null)
                    this.PopulateFileTypes();
                return (IList)this._audioFileTypes;
            }
        }

        public IList VideoFileTypes
        {
            get
            {
                if (this._videoFileTypes == null)
                    this.PopulateFileTypes();
                return (IList)this._videoFileTypes;
            }
        }

        private void PopulateFileTypes()
        {
            if (this._audioFileTypes != null && this._videoFileTypes != null)
                return;
            this._allFileTypes = (IList<BooleanInputChoice>)new List<BooleanInputChoice>();
            this._audioFileTypes = (IList<BooleanInputChoice>)new List<BooleanInputChoice>();
            this._videoFileTypes = (IList<BooleanInputChoice>)new List<BooleanInputChoice>();
            HRESULT associationInfoList = (HRESULT)this.FileAssocHandler.GetFileAssociationInfoList(out this._fileAssociationInfoList);
            if (associationInfoList.IsSuccess)
            {
                this.CanFileAssociationBeChanged = this.FileAssocHandler.CanAssociationBeChanged();
                string format = ZuneUI.Shell.LoadString(StringId.IDS_FILE_TYPES_DESCRIPTION_FORMAT);
                for (int index = 0; index < this._fileAssociationInfoList.Count; ++index)
                {
                    string extension = this._fileAssociationInfoList[index].Extension;
                    BooleanInputChoice booleanInputChoice = new BooleanInputChoice((ModelItem)this, string.Format(format, (object)extension.Substring(1), (object)this._fileAssociationInfoList[index].Description), this.CanFileAssociationBeChanged);
                    if (ClientConfiguration.FUE.ShowFUE && this.CanFileAssociationBeChanged && Array.IndexOf<string>(this._defaultFileTypeExtensions, extension) >= 0)
                        this._fileAssociationInfoList[index].IsCurrentlyOwned = true;
                    booleanInputChoice.Value = this._fileAssociationInfoList[index].IsCurrentlyOwned;
                    booleanInputChoice.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnFileTypesCommit)] = (object)null);
                    switch (this._fileAssociationInfoList[index].MediaType)
                    {
                        case EMediaTypes.eMediaTypeAudio:
                            this._audioFileTypes.Add(booleanInputChoice);
                            break;
                        case EMediaTypes.eMediaTypeVideo:
                            this._videoFileTypes.Add(booleanInputChoice);
                            break;
                    }
                    this._allFileTypes.Add(booleanInputChoice);
                }
            }
            else
                ErrorDialogInfo.Show(associationInfoList.Int, ZuneUI.Shell.LoadString(StringId.IDS_FILE_TYPES_ERROR_DIALOG_TITLE));
        }

        private void OnFileTypesCommit(object data)
        {
            if (this.FileAssocHandler == null || !this.CanFileAssociationBeChanged)
                return;
            for (int index = 0; index < this._allFileTypes.Count; ++index)
                this._fileAssociationInfoList[index].IsCurrentlyOwned = this._allFileTypes[index].Value;
            HRESULT hresult = (HRESULT)this.FileAssocHandler.SetFileAssociationInfo(this._fileAssociationInfoList);
            if (hresult.IsError)
                ErrorDialogInfo.Show(hresult.Int, ZuneUI.Shell.LoadString(StringId.IDS_FILE_TYPES_ERROR_DIALOG_TITLE));
            if (!ZuneUI.Shell.SettingsFrame.Wizard.IsCurrent)
                return;
            Fue.Instance.SetFileTypeAssociationsAreSet();
        }

        public void SelectAllFileTypes()
        {
            if (!this.CanFileAssociationBeChanged)
                return;
            for (int index = 0; index < this._allFileTypes.Count; ++index)
                this._allFileTypes[index].Value = true;
        }

        public void SaveFileTypesAsDefault()
        {
            if (!this.FileAssocHandler.CanAssociationBeChanged())
                return;
            IList<Microsoft.Zune.Configuration.FileAssociationInfo> fileAssociationInfoList;
            HRESULT hresult = (HRESULT)this.FileAssocHandler.GetFileAssociationInfoList(out fileAssociationInfoList);
            if (hresult.IsSuccess)
            {
                for (int index = 0; index < fileAssociationInfoList.Count; ++index)
                {
                    if (Array.IndexOf<string>(this._defaultFileTypeExtensions, fileAssociationInfoList[index].Extension) >= 0)
                        fileAssociationInfoList[index].IsCurrentlyOwned = true;
                }
                hresult = (HRESULT)this.FileAssocHandler.SetFileAssociationInfo(fileAssociationInfoList);
                if (!hresult.IsError)
                    return;
                ErrorDialogInfo.Show(hresult.Int, ZuneUI.Shell.LoadString(StringId.IDS_FILE_TYPES_ERROR_DIALOG_TITLE));
            }
            else
                ErrorDialogInfo.Show(hresult.Int, ZuneUI.Shell.LoadString(StringId.IDS_FILE_TYPES_ERROR_DIALOG_TITLE));
        }

        public void ResetWarningMessages()
        {
            ClientConfiguration.MediaStore.LibraryDefaultDeleteChoice = 0;
            ClientConfiguration.MediaStore.PlaylistDefaultDeleteChoice = 0;
            ClientConfiguration.Pictures.DisplayAutouploadNotification = true;
            ClientConfiguration.Series.PodcastDefaultUnsubscribeChoice = 0;
            ClientConfiguration.Service.InhibitSubscriptionMachineCountExceededSignInWarning = false;
            ClientConfiguration.Service.InhibitSubscriptionBillingViolationSignInWarning = false;
            ClientConfiguration.Service.InhibitSubscriptionEndingWarning = false;
            ClientConfiguration.Service.InhibitSubscriptionFreePurchasePrompt = false;
            ClientConfiguration.Service.InhibitWinPhoneAppPurchaseConfirmation = false;
            ClientConfiguration.Service.InhibitReviewRefreshWarning = false;
            ClientConfiguration.Shell.ShowAppsForZuneHDOnlyHeader = true;
            ClientConfiguration.Shell.ShowAppsForWindowsPhoneOnlyHeader = true;
            ClientConfiguration.MediaStore.ConfirmAccountDevicePCDeletion = true;
            ClientConfiguration.MediaStore.ConfirmAccountDevicePortableDeletion = true;
            ClientConfiguration.MediaStore.ConfirmDeviceMediaDeletion = true;
            ClientConfiguration.MediaStore.ConfirmMultiAlbumEdit = true;
            ClientConfiguration.MediaStore.ConfirmMultiSongEdit = true;
            ClientConfiguration.MediaStore.ConfirmMultiVideoEdit = true;
            ClientConfiguration.MediaStore.ConfirmPasteAlbumArt = true;
            ClientConfiguration.MediaStore.AlertSyncAllFriendsBehavior = true;
            ClientConfiguration.Social.ConfirmAcceptFriend = true;
            ClientConfiguration.Social.ConfirmDeleteFriend = true;
            ClientConfiguration.QuickMix.OnlyEnableItemsWithQuickMix = false;
            ClientConfiguration.Devices.ShowExcludeFromSyncWarning = true;
            ClientConfiguration.Devices.ShowSyncInstructionsToast = true;
            foreach (UIDevice uiDevice in SingletonModelItem<UIDeviceList>.Instance)
                uiDevice.PromptForAccountLinkage = true;
        }

        public bool InhibitSubscriptionMachineCountExceededSignInWarning
        {
            get => ClientConfiguration.Service.InhibitSubscriptionMachineCountExceededSignInWarning;
            set => ClientConfiguration.Service.InhibitSubscriptionMachineCountExceededSignInWarning = value;
        }

        public bool InhibitSubscriptionBillingViolationSignInWarning
        {
            get => ClientConfiguration.Service.InhibitSubscriptionBillingViolationSignInWarning;
            set => ClientConfiguration.Service.InhibitSubscriptionBillingViolationSignInWarning = value;
        }

        public bool InhibitSubscriptionEndingWarning
        {
            get => ClientConfiguration.Service.InhibitSubscriptionEndingWarning;
            set => ClientConfiguration.Service.InhibitSubscriptionEndingWarning = value;
        }

        public bool InhibitSubscriptionFreePurchasePrompt
        {
            get => ClientConfiguration.Service.InhibitSubscriptionFreePurchasePrompt;
            set => ClientConfiguration.Service.InhibitSubscriptionFreePurchasePrompt = value;
        }

        public int LibraryDefaultDeleteChoice
        {
            get => ClientConfiguration.MediaStore.LibraryDefaultDeleteChoice;
            set => ClientConfiguration.MediaStore.LibraryDefaultDeleteChoice = value;
        }

        public int PlaylistDefaultDeleteChoice
        {
            get => ClientConfiguration.MediaStore.PlaylistDefaultDeleteChoice;
            set => ClientConfiguration.MediaStore.PlaylistDefaultDeleteChoice = value;
        }

        public bool ConfirmAcceptFriend
        {
            get => ClientConfiguration.Social.ConfirmAcceptFriend;
            set => ClientConfiguration.Social.ConfirmAcceptFriend = value;
        }

        public bool ConfirmDeleteFriend
        {
            get => ClientConfiguration.Social.ConfirmDeleteFriend;
            set => ClientConfiguration.Social.ConfirmDeleteFriend = value;
        }

        public bool ConfirmAccountDevicePCDeletion
        {
            get => ClientConfiguration.MediaStore.ConfirmAccountDevicePCDeletion;
            set => ClientConfiguration.MediaStore.ConfirmAccountDevicePCDeletion = value;
        }

        public bool ConfirmAccountDevicePortableDeletion
        {
            get => ClientConfiguration.MediaStore.ConfirmAccountDevicePortableDeletion;
            set => ClientConfiguration.MediaStore.ConfirmAccountDevicePortableDeletion = value;
        }

        public bool ConfirmDeviceMediaDeletion
        {
            get => ClientConfiguration.MediaStore.ConfirmDeviceMediaDeletion;
            set => ClientConfiguration.MediaStore.ConfirmDeviceMediaDeletion = value;
        }

        public bool ConfirmMultiAlbumEdit
        {
            get => ClientConfiguration.MediaStore.ConfirmMultiAlbumEdit;
            set => ClientConfiguration.MediaStore.ConfirmMultiAlbumEdit = value;
        }

        public bool ConfirmMultiSongEdit
        {
            get => ClientConfiguration.MediaStore.ConfirmMultiSongEdit;
            set => ClientConfiguration.MediaStore.ConfirmMultiSongEdit = value;
        }

        public bool ConfirmMultiVideoEdit
        {
            get => ClientConfiguration.MediaStore.ConfirmMultiVideoEdit;
            set => ClientConfiguration.MediaStore.ConfirmMultiVideoEdit = value;
        }

        public bool ConfirmPasteAlbumArt
        {
            get => ClientConfiguration.MediaStore.ConfirmPasteAlbumArt;
            set => ClientConfiguration.MediaStore.ConfirmPasteAlbumArt = value;
        }

        public ArrayListDataSet DeviceList
        {
            get
            {
                if (this._registeredDevicesModelList == null)
                    this.InitRegisteredTuners();
                return this._registeredDevicesModelList;
            }
        }

        public ArrayListDataSet AppStoreDeviceList
        {
            get
            {
                if (this._registeredAppStoreDevicesModelList == null)
                    this.InitRegisteredTuners();
                return this._registeredAppStoreDevicesModelList;
            }
        }

        public ArrayListDataSet ComputerList
        {
            get
            {
                if (this._registeredComputersModelList == null)
                    this.InitRegisteredTuners();
                return this._registeredComputersModelList;
            }
        }

        public string NextPCDeregistrationDate
        {
            get => this._nextPCDeregistrationDate;
            private set
            {
                if (!(this._nextPCDeregistrationDate != value))
                    return;
                this._nextPCDeregistrationDate = value;
                this.FirePropertyChanged(nameof(NextPCDeregistrationDate));
            }
        }

        public string NextSubscriptionDeviceDeregistrationDate
        {
            get => this._nextSubscriptionDeviceDeregistrationDate;
            private set
            {
                if (!(this._nextSubscriptionDeviceDeregistrationDate != value))
                    return;
                this._nextSubscriptionDeviceDeregistrationDate = value;
                this.FirePropertyChanged(nameof(NextSubscriptionDeviceDeregistrationDate));
            }
        }

        public string NextAppStoreDeviceDeregistrationDate
        {
            get => this._nextAppStoreDeviceDeregistrationDate;
            private set
            {
                if (!(this._nextAppStoreDeviceDeregistrationDate != value))
                    return;
                this._nextAppStoreDeviceDeregistrationDate = value;
                this.FirePropertyChanged(nameof(NextAppStoreDeviceDeregistrationDate));
            }
        }

        public bool CanShowDeviceList
        {
            get
            {
                if (this._tunerHandler == null)
                    this.InitRegisteredTuners();
                return this._tunerHandler.CanQueryTunerList();
            }
        }

        public void RemoveTuner(Microsoft.Zune.Configuration.TunerInfo tunerInfo)
        {
            if (this._tunerHandler == null)
                this.InitRegisteredTuners();
            this._tunerHandler.DeregisterTuner(tunerInfo);
        }

        public void RefreshTunerList()
        {
            if (!this._tunerHandler.CanQueryTunerList())
                return;
            this._tunerHandler.RefreshTunerList();
        }

        private void OnTunerInfoChanged(object oSenderUNUSED, EventArgs eargs)
        {
            if (eargs != null && eargs.GetType() == typeof(EventArgsHR) && (HRESULT)((EventArgsHR)eargs).HResult == HRESULT._ZEST_E_TOO_MANY_DEREGISTRATIONS_WITHIN_MONTH)
                Application.DeferredInvoke(new DeferredInvokeHandler(this.DisplayServiceErrorMessage), (object)eargs);
            else
                Application.DeferredInvoke(new DeferredInvokeHandler(this.UpdateRegisteredTunersList), DeferredInvokePriority.Normal);
        }

        private void DisplayServiceErrorMessage(object eargs) => ZuneUI.Shell.ShowErrorDialog(((EventArgsHR)eargs).HResult, StringId.IDS_REGDEVICES_CANT_REMOVE);

        private void UpdateRegisteredTunersList(object argsUNUSED)
        {
            if (this._registeredComputersModelList == null || this._registeredDevicesModelList == null || this._registeredAppStoreDevicesModelList == null)
                return;
            int count1 = this._registeredComputersModelList.Count;
            int count2 = this._registeredDevicesModelList.Count;
            int count3 = this._registeredAppStoreDevicesModelList.Count;
            this._registeredComputersModelList.Clear();
            this._registeredDevicesModelList.Clear();
            this._registeredAppStoreDevicesModelList.Clear();
            foreach (object pcs in (IEnumerable<Microsoft.Zune.Configuration.TunerInfo>)this._tunerHandler.GetPCsList())
                this._registeredComputersModelList.Add(pcs);
            foreach (object devices in (IEnumerable<Microsoft.Zune.Configuration.TunerInfo>)this._tunerHandler.GetDevicesList())
                this._registeredDevicesModelList.Add(devices);
            foreach (object appStoreDevices in (IEnumerable<Microsoft.Zune.Configuration.TunerInfo>)this._tunerHandler.GetAppStoreDevicesList())
                this._registeredAppStoreDevicesModelList.Add(appStoreDevices);
            DateTime deregistrationDate1 = this._tunerHandler.GetNextPCDeregistrationDate();
            this.NextPCDeregistrationDate = !(deregistrationDate1 != DateTime.MinValue) || !(deregistrationDate1 > DateTime.Now) ? (string)null : deregistrationDate1.AddDays(1.0).ToShortDateString();
            DateTime deregistrationDate2 = this._tunerHandler.GetNextSubscriptionDeviceDeregistrationDate();
            this.NextSubscriptionDeviceDeregistrationDate = !(deregistrationDate2 != DateTime.MinValue) || !(deregistrationDate2 > DateTime.Now) ? (string)null : deregistrationDate2.AddDays(1.0).ToShortDateString();
            DateTime deregistrationDate3 = this._tunerHandler.GetNextAppStoreDeviceDeregistrationDate();
            this.NextAppStoreDeviceDeregistrationDate = !(deregistrationDate3 != DateTime.MinValue) || !(deregistrationDate3 > DateTime.Now) ? (string)null : deregistrationDate3.AddDays(1.0).ToShortDateString();
            if (this._registeredComputersModelList.Count >= count1 && this._registeredDevicesModelList.Count >= count2 && this._registeredAppStoreDevicesModelList.Count >= count3)
                return;
            SignIn.Instance.RefreshAccount();
        }

        private void InitRegisteredTuners()
        {
            this._tunerHandler = TunerInfoHandlerFactory.CreateTunerInfoHandler();
            this._tunerHandler.OnChanged += new EventHandler(this.OnTunerInfoChanged);
            this._registeredComputersModelList = new ArrayListDataSet();
            this._registeredDevicesModelList = new ArrayListDataSet();
            this._registeredAppStoreDevicesModelList = new ArrayListDataSet();
            this._nextPCDeregistrationDate = (string)null;
            this._nextSubscriptionDeviceDeregistrationDate = (string)null;
            this._nextAppStoreDeviceDeregistrationDate = (string)null;
            if (!this._tunerHandler.CanQueryTunerList())
                return;
            this._tunerHandler.RefreshTunerList();
        }

        public IntRangedValue SlideShowSpeed
        {
            get
            {
                if (this._slideShowSpeed == null)
                {
                    this._slideShowSpeed = new IntRangedValue((IModelItemOwner)this);
                    this._slideShowSpeed.MinValue = 3000;
                    this._slideShowSpeed.MaxValue = 10000;
                    this._slideShowSpeed.Step = 1000;
                    this._slideShowSpeed.Value = ClientConfiguration.GeneralSettings.SlideShowSpeed;
                    this._slideShowSpeed.PropertyChanged += (PropertyChangedEventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnSlideShowSpeedCommit)] = (object)null);
                }
                return this._slideShowSpeed;
            }
        }

        private void OnSlideShowSpeedCommit(object data) => ClientConfiguration.GeneralSettings.SlideShowSpeed = this._slideShowSpeed.Value;

        public HMESettings HME
        {
            get
            {
                if (this._HME == null)
                {
                    lock (this.mylock)
                    {
                        if (this._HME == null)
                        {
                            HMESettings hmeSettings = new HMESettings();
                            int num = ((HRESULT)hmeSettings.Init()).IsError ? 1 : 0;
                            this._sharingAllDevicesEnabled = hmeSettings.GetAllDevicesEnabled();
                            this._HME = hmeSettings;
                        }
                    }
                }
                return this._HME;
            }
        }

        public bool UserCanModifySharing => Environment.OSVersion.Version.Major >= 6 || !this.SharingEnableRequiresElevation || this.SharingEnabled;

        public bool SharingEnableRequiresElevation => Environment.OSVersion.Version.Major < 6 ? this.HME.SharingEnableRequiresLoginAsAdmin : this.HME.SharingEnableRequiresElevation;

        public bool SharingEnabled => this.HME.SharingEnabled;

        private void SetSharingEnabledForAllMediaTypes(bool music, bool video, bool pictures)
        {
            this.HME.SetSharingEnabledForMediaType(EMediaTypes.eMediaTypeAudio, music);
            this.HME.SetSharingEnabledForMediaType(EMediaTypes.eMediaTypeVideo, video);
            this.HME.SetSharingEnabledForMediaType(EMediaTypes.eMediaTypeImage, pictures);
        }

        private void OnMediaSharingUpdate(object data)
        {
            if (this.HME == null)
                return;
            bool flag = this._sharingEnableMusic.Value || this._sharingEnableVideo.Value || this._sharingEnablePhoto.Value;
            HRESULT hresult1 = (HRESULT)0;
            if (flag)
            {
                if (((HRESULT)this.HME.EnableSharingForUser()).IsSuccess)
                    this.SetSharingEnabledForAllMediaTypes(this._sharingEnableMusic.Value, this._sharingEnableVideo.Value, this._sharingEnablePhoto.Value);
                else
                    this.SetSharingEnabledForAllMediaTypes(false, false, false);
            }
            else
            {
                HRESULT hresult2 = (HRESULT)this.HME.DisableSharingForMachine();
                if (hresult2.IsSuccess)
                    hresult2 = (HRESULT)this.HME.DisableSharingForUser();
                if (hresult2.IsSuccess)
                    this.SetSharingEnabledForAllMediaTypes(false, false, false);
            }
            this.HME.SetAllDevicesEnabled(this._sharingAllDevicesEnabled);
            if (!this._sharingAllDevicesEnabled)
                this.HME.EnableDevice(this._sharingDeviceIndex, true);
            this.HME.SetDisplayName(this._sharingDisplayName);
        }

        public string SharingDisplayName
        {
            get
            {
                if (this._sharingDisplayName == null)
                    this.HME.GetDisplayName(ref this._sharingDisplayName);
                return this._sharingDisplayName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.SharingError = ZuneUI.Shell.LoadString(StringId.IDS_SHARE_NAME_EMPTY);
                }
                else
                {
                    if (!(this._sharingDisplayName != value))
                        return;
                    this.SharingError = string.Empty;
                    this._sharingDisplayName = value;
                    this.CommitList[(object)new ProxySettingDelegate(this.OnMediaSharingUpdate)] = (object)null;
                }
            }
        }

        public string SharingError
        {
            get => this._sharingError;
            set
            {
                if (!(this._sharingError != value))
                    return;
                this._sharingError = value;
                this.FirePropertyChanged(nameof(SharingError));
            }
        }

        public BooleanChoice SharingEnableMusic
        {
            get
            {
                if (this._sharingEnableMusic == null)
                {
                    this._sharingEnableMusic = new BooleanChoice((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_SHARE_MUSIC_CHECK));
                    this._sharingEnableMusic.Value = this.HME.GetSharingEnabledForMediaType(EMediaTypes.eMediaTypeAudio);
                    this._sharingEnableMusic.ChosenChanged += (EventHandler)((sender, args) =>
                   {
                       this.CommitList[(object)new ProxySettingDelegate(this.OnMediaSharingUpdate)] = (object)null;
                       if (!this.SharingEnableRequiresElevation)
                           return;
                       this.ChangeRequiresElevation = true;
                   });
                }
                return this._sharingEnableMusic;
            }
        }

        public BooleanChoice SharingEnableVideo
        {
            get
            {
                if (this._sharingEnableVideo == null)
                {
                    this._sharingEnableVideo = new BooleanChoice((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_SHARE_VIDEOS_CHECK));
                    this._sharingEnableVideo.Value = this.HME.GetSharingEnabledForMediaType(EMediaTypes.eMediaTypeVideo);
                    this._sharingEnableVideo.ChosenChanged += (EventHandler)((sender, args) =>
                   {
                       this.CommitList[(object)new ProxySettingDelegate(this.OnMediaSharingUpdate)] = (object)null;
                       if (!this.SharingEnableRequiresElevation)
                           return;
                       this.ChangeRequiresElevation = true;
                   });
                }
                return this._sharingEnableVideo;
            }
        }

        public BooleanChoice SharingEnablePhoto
        {
            get
            {
                if (this._sharingEnablePhoto == null)
                {
                    this._sharingEnablePhoto = new BooleanChoice((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_SHARE_PICTURES_CHECK));
                    this._sharingEnablePhoto.Value = this.HME.GetSharingEnabledForMediaType(EMediaTypes.eMediaTypeImage);
                    this._sharingEnablePhoto.ChosenChanged += (EventHandler)((sender, args) =>
                   {
                       this.CommitList[(object)new ProxySettingDelegate(this.OnMediaSharingUpdate)] = (object)null;
                       if (!this.SharingEnableRequiresElevation)
                           return;
                       this.ChangeRequiresElevation = true;
                   });
                }
                return this._sharingEnablePhoto;
            }
        }

        public Choice SharingSelectDeviceChoice
        {
            get
            {
                if (this._sharingSelectDeviceChoice == null)
                {
                    this._sharingSelectDeviceOptions = (IList<Command>)new List<Command>();
                    Command command1 = new Command((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_GLOBAL_SHARING_OPTION), (EventHandler)null);
                    command1.Data.Add((object)"value", (object)true);
                    this._sharingSelectDeviceOptions.Add(command1);
                    Command command2 = new Command((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_SELECTIVE_SHARING_OPTION), (EventHandler)null);
                    command2.Data.Add((object)"value", (object)false);
                    command2.Available = this.SharingEnabled;
                    this._sharingSelectDeviceOptions.Add(command2);
                    this._sharingSelectDeviceChoice = new Choice((IModelItemOwner)this);
                    this._sharingSelectDeviceChoice.Options = (IList)this._sharingSelectDeviceOptions;
                    this._sharingSelectDeviceChoice.ChosenChanged += (EventHandler)((sender, args) => this.SharingAllDevicesEnabled = (bool)this._sharingSelectDeviceOptions[((Choice)sender).ChosenIndex].Data[(object)"value"]);
                }
                return this._sharingSelectDeviceChoice;
            }
        }

        public bool SharingAllDevicesEnabled
        {
            get => this._sharingAllDevicesEnabled;
            set
            {
                if (this._sharingAllDevicesEnabled == value)
                    return;
                this._sharingAllDevicesEnabled = value;
                this.CommitList[(object)new ProxySettingDelegate(this.OnMediaSharingUpdate)] = (object)null;
                this.FirePropertyChanged(nameof(SharingAllDevicesEnabled));
            }
        }

        public IList SharingDeviceList
        {
            get
            {
                if (this._sharingDeviceList == null)
                    this._sharingDeviceList = this.CreateSharingDeviceList();
                return (IList)this._sharingDeviceList;
            }
        }

        private IList<BooleanInputChoice> CreateSharingDeviceList()
        {
            IList<BooleanInputChoice> booleanInputChoiceList = (IList<BooleanInputChoice>)new List<BooleanInputChoice>();
            uint deviceCount = this.HME.GetDeviceCount();
            string strName = "";
            string strMAC = "";
            string strSerialNumber = "";
            for (uint dwIndex = 0; dwIndex < deviceCount; ++dwIndex)
            {
                this.HME.GetDeviceProps(dwIndex, ref strName, ref strMAC, ref strSerialNumber);
                BooleanInputChoice booleanInputChoice = new BooleanInputChoice((ModelItem)this, deviceCount > 1U ? string.Format(ZuneUI.Shell.LoadString(StringId.IDS_XBOX360_NAME_AND_SERIAL_NUMBER), (object)strName, (object)strSerialNumber) : strName, true);
                booleanInputChoice.Data[(object)"index"] = (object)dwIndex;
                booleanInputChoice.Value = this.HME.GetDeviceEnabled(dwIndex);
                booleanInputChoice.ChosenChanged += new EventHandler(this.HandleSharingDeviceListValueChanged);
                booleanInputChoiceList.Add(booleanInputChoice);
            }
            if (!this._nssDeviceListChangeEventAdded)
            {
                this.HME.NSSDeviceListChangeEvent += new NSSDeviceListChangeHandler(this.HandleNSSDeviceListChangeEvent);
                this._nssDeviceListChangeEventAdded = true;
            }
            return booleanInputChoiceList;
        }

        public void RemoveNSSDeviceListChangeEvent()
        {
            if (!this._nssDeviceListChangeEventAdded)
                return;
            this.HME.NSSDeviceListChangeEvent -= new NSSDeviceListChangeHandler(this.HandleNSSDeviceListChangeEvent);
            this._nssDeviceListChangeEventAdded = false;
            this._sharingDeviceList = (IList<BooleanInputChoice>)null;
        }

        private void HandleNSSDeviceListChangeEvent() => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this._sharingDeviceList == null)
               return;
           this._sharingDeviceList = this.CreateSharingDeviceList();
           this.FirePropertyChanged("SharingDeviceList");
       }, (object)null);

        private void HandleSharingDeviceListValueChanged(object sender, EventArgs args) => this._sharingDeviceIndex = (uint)((ModelItem)sender).Data[(object)"index"];

        public bool ReevaluateVideoSettings
        {
            get => ClientConfiguration.GeneralSettings.ReevaluateVideoSettings;
            set
            {
                ClientConfiguration.GeneralSettings.ReevaluateVideoSettings = value;
                this.FirePropertyChanged(nameof(ReevaluateVideoSettings));
            }
        }

        public RenderingType RequestedRenderingType
        {
            get
            {
                RenderingType renderingType = (RenderingType)ClientConfiguration.GeneralSettings.RenderingType;
                if (renderingType == RenderingType.Default)
                    renderingType = Application.RenderingType;
                return renderingType;
            }
            set
            {
                ClientConfiguration.GeneralSettings.RenderingType = (int)value;
                this.ReevaluateVideoSettings = Application.RenderingType != RenderingType.DX9 && value == RenderingType.DX9;
                this.FirePropertyChanged(nameof(RequestedRenderingType));
            }
        }

        private void ReevaluateVideoAcceleration(object sender, EventArgs args) => this.RequestedRenderingType = RenderingType.DX9;

        public RenderingQuality RequestedRenderingQuality
        {
            get => (RenderingQuality)ClientConfiguration.GeneralSettings.RenderingQuality;
            set
            {
                ClientConfiguration.GeneralSettings.RenderingQuality = (int)value;
                this.FirePropertyChanged(nameof(RequestedRenderingQuality));
            }
        }

        public bool AnimationsEnabled
        {
            get => ClientConfiguration.GeneralSettings.AnimationsEnabled;
            set
            {
                ClientConfiguration.GeneralSettings.AnimationsEnabled = value;
                this.FirePropertyChanged(nameof(AnimationsEnabled));
            }
        }

        public Choice ScreenGraphicsSlider
        {
            get
            {
                if (this._screenGraphicsSlider == null)
                {
                    Choice choice = new Choice((IModelItemOwner)this);
                    choice.Options = (IList)NamedIntOption.ScreenGraphicsOptions;
                    ScreenGraphics screenGraphics = Application.RenderingType != RenderingType.GDI ? (Application.RenderingQuality != RenderingQuality.MaxQuality ? (!Application.AnimationsEnabled ? ScreenGraphics.Advanced : ScreenGraphics.AdvancedWithAnimation) : ScreenGraphics.Premium) : ScreenGraphics.Basic;
                    NamedIntOption.SelectOptionByValue(choice, (int)screenGraphics);
                    this._screenGraphicsSlider = choice;
                    choice.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnScreenGraphicsSliderCommit)] = (object)null);
                }
                return this._screenGraphicsSlider;
            }
        }

        private void OnScreenGraphicsSliderCommit(object data)
        {
            ScreenGraphics screenGraphics = (ScreenGraphics)((NamedIntOption)this._screenGraphicsSlider.ChosenValue).Value;
            RenderingType requestedRenderingType = this.RequestedRenderingType;
            switch (screenGraphics)
            {
                case ScreenGraphics.Basic:
                    this.RequestedRenderingType = RenderingType.GDI;
                    this.RequestedRenderingQuality = RenderingQuality.MinQuality;
                    this.AnimationsEnabled = false;
                    break;
                case ScreenGraphics.Advanced:
                    this.RequestedRenderingType = RenderingType.DX9;
                    this.RequestedRenderingQuality = RenderingQuality.MinQuality;
                    this.AnimationsEnabled = false;
                    break;
                case ScreenGraphics.AdvancedWithAnimation:
                    this.RequestedRenderingType = RenderingType.DX9;
                    this.RequestedRenderingQuality = RenderingQuality.MinQuality;
                    this.AnimationsEnabled = true;
                    break;
                case ScreenGraphics.Premium:
                    this.RequestedRenderingType = RenderingType.DX9;
                    this.RequestedRenderingQuality = RenderingQuality.MaxQuality;
                    this.AnimationsEnabled = true;
                    break;
            }
            if (requestedRenderingType == RenderingType.GDI && this.RequestedRenderingType == RenderingType.DX9)
                MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_SCREEN_GRAPHICS_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_ACCELERATION_PROMPT_TEXT), (EventHandler)null);
            else
                MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_SCREEN_GRAPHICS_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_ACCELERATION_RESTART_TEXT), (EventHandler)null);
        }

        public string BackgroundImage
        {
            get
            {
                if (this._backgroundImage == null)
                    this._backgroundImage = ClientConfiguration.Shell.BackgroundImage;
                return this._backgroundImage;
            }
            set
            {
                if (!(this._backgroundImage != value))
                    return;
                this.CommitList[(object)new ProxySettingDelegate(this.OnBackgroundImageCommit)] = (object)null;
                this._backgroundImage = value;
                this.FirePropertyChanged(nameof(BackgroundImage));
            }
        }

        public WindowColor BackgroundColor
        {
            get => this._backgroundColor;
            set => this._backgroundColor = value;
        }

        private void OnBackgroundImageCommit(object data)
        {
            ClientConfiguration.Shell.BackgroundImage = this._backgroundImage;
            ClientConfiguration.Shell.BackgroundColor = ZuneUI.Shell.WindowColorToRGB(this._backgroundColor);
            ((ZuneUI.Shell)ZuneShell.DefaultInstance).BackgroundImage = this._backgroundImage;
            Application.Window.SetBackgroundColor(this._backgroundColor);
        }

        public BooleanChoice ShowNowPlayingBackgroundOnIdle
        {
            get
            {
                if (this._showNowPlayingBackgroundOnIdle == null)
                {
                    this._showNowPlayingBackgroundOnIdle = new BooleanChoice((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_SHOW_NOWPLAYING_ON_IDLE_DESCRIPTION));
                    this._showNowPlayingBackgroundOnIdle.Value = ClientConfiguration.Shell.ShowNowPlayingBackgroundOnIdleTimeout > 0;
                    this._showNowPlayingBackgroundOnIdle.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnShowNowPlayingBackgroundOnIdleCommit)] = (object)null);
                }
                return this._showNowPlayingBackgroundOnIdle;
            }
        }

        private void OnShowNowPlayingBackgroundOnIdleCommit(object data)
        {
            int num = this._showNowPlayingBackgroundOnIdle.Value ? 90 : 0;
            ClientConfiguration.Shell.ShowNowPlayingBackgroundOnIdleTimeout = num;
            ((ZuneUI.Shell)ZuneShell.DefaultInstance).ShowNowPlayingBackgroundOnIdleTimeout = num;
        }

        public BooleanChoice PlaySounds
        {
            get
            {
                if (this._playSounds == null)
                {
                    this._playSounds = new BooleanChoice((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_SOUNDS_DESCRIPTION));
                    this._playSounds.Value = ClientConfiguration.Shell.Sounds;
                    this._playSounds.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnPlaySoundsCommit)] = (object)null);
                }
                return this._playSounds;
            }
        }

        private void OnPlaySoundsCommit(object data)
        {
            ClientConfiguration.Shell.Sounds = this._playSounds.Value;
            ((ZuneUI.Shell)ZuneShell.DefaultInstance).PlaySounds = this._playSounds.Value;
        }

        public BooleanChoice CompactModeAlwaysOnTop
        {
            get
            {
                if (this._compactModeAlwaysOnTop == null)
                {
                    this._compactModeAlwaysOnTop = new BooleanChoice((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_COMPACT_MODE_ALWAYS_ON_TOP));
                    this._compactModeAlwaysOnTop.Value = ClientConfiguration.GeneralSettings.CompactModeAlwaysOnTop;
                    this._compactModeAlwaysOnTop.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnCompactModeAlwaysOnTopCommit)] = (object)null);
                }
                return this._compactModeAlwaysOnTop;
            }
        }

        private void OnCompactModeAlwaysOnTopCommit(object data)
        {
            ClientConfiguration.GeneralSettings.CompactModeAlwaysOnTop = this._compactModeAlwaysOnTop.Value;
            ((ZuneUI.Shell)ZuneShell.DefaultInstance).CompactModeAlwaysOnTop = this._compactModeAlwaysOnTop.Value;
            SQMLog.Log(SQMDataId.CompactModeOnTopSetting, 1);
        }

        public BooleanChoice RatingsChoice
        {
            get
            {
                if (this._ratingsChoice == null)
                {
                    Command[] commandArray = new Command[2]
                    {
            (Command) new RichLayoutCommand((IModelItemOwner) this, ZuneUI.Shell.LoadString(StringId.IDS_COMMON_RATINGS_ALL_USERS_OPTION), true),
            (Command) new RichLayoutCommand((IModelItemOwner) this, ZuneUI.Shell.LoadString(StringId.IDS_PERSONAL_RATINGS_EACH_USER_OPTION), false)
                    };
                    this._ratingsChoice = new BooleanChoice((IModelItemOwner)this);
                    this._ratingsChoice.Options = (IList)commandArray;
                    this._ratingsChoice.Value = !ClientConfiguration.MediaStore.SharedUserRatings;
                    this._ratingsChoice.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnRatingsCommit)] = (object)null);
                }
                return this._ratingsChoice;
            }
        }

        private void OnRatingsCommit(object data)
        {
            ClientConfiguration.MediaStore.SharedUserRatings = !this._ratingsChoice.Value;
            if (!this._applyRatingsChoice.Value || this._ratingsChoice.Value)
                return;
            ZuneLibrary.ExportUserRatings(SignIn.Instance.LastSignedInUserId, EMediaTypes.eMediaTypeAudio);
        }

        public BooleanChoice ApplyRatingsChoice
        {
            get
            {
                if (this._applyRatingsChoice == null)
                {
                    this._applyRatingsChoice = new BooleanChoice((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_APPLY_RATINGS_DIALOG_DESCRIPTION));
                    this._applyRatingsChoice.Value = false;
                    this._applyRatingsChoice.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnRatingsCommit)] = (object)null);
                }
                return this._applyRatingsChoice;
            }
        }

        public Choice StartupPageChoice
        {
            get
            {
                if (this._startupPageChoice == null)
                {
                    List<Command> commandList = new List<Command>();
                    if (FeatureEnablement.IsFeatureEnabled(Features.eQuickplay))
                        commandList.Add((Command)new NamedStringOption(ZuneUI.Shell.LoadString(StringId.IDS_VIEW_STARTUPPAGE_QUICKPLAY_CHOICE), ZuneUI.Shell.MainFrame.Quickplay.DefaultUIPath));
                    commandList.Add((Command)new NamedStringOption(ZuneUI.Shell.LoadString(StringId.IDS_VIEW_STARTUPPAGE_COLLECTION_CHOICE), ZuneUI.Shell.MainFrame.Collection.DefaultUIPath));
                    if (FeatureEnablement.IsFeatureEnabled(Features.eMarketplace))
                        commandList.Add((Command)new NamedStringOption(ZuneUI.Shell.LoadString(StringId.IDS_VIEW_STARTUPPAGE_MARKETPLACE_CHOICE), ZuneUI.Shell.MainFrame.Marketplace.DefaultUIPath));
                    if (FeatureEnablement.IsFeatureEnabled(Features.eSocial))
                        commandList.Add((Command)new NamedStringOption(ZuneUI.Shell.LoadString(StringId.IDS_VIEW_STARTUPPAGE_SOCIAL_CHOICE), ZuneUI.Shell.MainFrame.Social.DefaultUIPath));
                    this._startupPageChoice = new Choice((IModelItemOwner)this);
                    this._startupPageChoice.Options = (IList)commandList;
                    foreach (NamedStringOption namedStringOption in commandList)
                    {
                        if (namedStringOption.Value == ClientConfiguration.Shell.StartupPage)
                        {
                            this._startupPageChoice.ChosenValue = (object)namedStringOption;
                            break;
                        }
                    }
                    this._startupPageChoice.ChosenChanged += (EventHandler)((sender, args) => this.CommitList[(object)new ProxySettingDelegate(this.OnStartupPageCommit)] = (object)null);
                }
                return this._startupPageChoice;
            }
        }

        private void OnStartupPageCommit(object data)
        {
            ClientConfiguration.Shell.StartupPage = ((NamedStringOption)this._startupPageChoice.ChosenValue).Value;
            ClientConfiguration.Quickplay.CheckUseCount = false;
        }

        public bool AutoLaunchZuneOnConnect
        {
            get => this._autoLaunchZuneOnConnect;
            set
            {
                if (this._autoLaunchZuneOnConnect == value)
                    return;
                this._autoLaunchZuneOnConnect = value;
                this.CommitList[(object)new ProxySettingDelegate(this.OnAutoLaunchZuneOnConnectCommit)] = (object)null;
                this.FirePropertyChanged(nameof(AutoLaunchZuneOnConnect));
            }
        }

        private void OnAutoLaunchZuneOnConnectCommit(object data) => ClientConfiguration.Devices.AutoLaunchZuneOnConnect = this._autoLaunchZuneOnConnect;

        private struct MonitoredFolder
        {
            public readonly string Path;
            public readonly EMediaTypes Schema;

            public MonitoredFolder(string path, EMediaTypes schema)
            {
                this.Path = path;
                this.Schema = schema;
            }
        }
    }
}
