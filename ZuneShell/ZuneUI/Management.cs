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
using System.Linq;
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
        private List<MonitoredFolder> _removedMonitoredFoldersToRemoveFromCollection;
        private ProxySettingDelegate[] _actionsToCommitOnLibraryIntegrate;
        private Choice _podcastDefaultKeepEpisodesChoice;
        private Choice _podcastPlaybackChoice;
        private string[] _defaultFileTypeExtensions =
        [
            ".mp3",
            ".m4a",
            ".mp4",
            ".m4b",
            ".m4v",
            ".mbr",
            ".zpl"
        ];
        private IFileAssociationHandler _fileAssocHandler;
        private IList<BooleanInputChoice> _allFileTypes;
        private IList<BooleanInputChoice> _audioFileTypes;
        private IList<BooleanInputChoice> _videoFileTypes;
        private IList<FileAssociationInfo> _fileAssociationInfoList;
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
        private object mylock = new();

        public Management()
        {
            _autoLaunchZuneOnConnect = ClientConfiguration.Devices.AutoLaunchZuneOnConnect;
            ClientConfiguration.Groveler.OnConfigurationChanged += OnGrovelerConfigurationChanged;
            _actionsToCommitOnLibraryIntegrate =
            [
                OnMonitoredFoldersCommit,
                OnMediaFolderCommit,
                OnVideoMediaFolderCommit,
                OnPhotoMediaFolderCommit,
                OnPodcastMediaFolderCommit
            ];
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                DisposeDeviceManagement(true);
                ClientConfiguration.Groveler.OnConfigurationChanged -= OnGrovelerConfigurationChanged;
            }
            if (_fileAssocHandler != null)
            {
                // Not all file association handlers are disposable
                (_fileAssocHandler as IDisposable)?.Dispose();
                _fileAssocHandler = null;
            }
            base.OnDispose(disposing);
        }

        private IFileAssociationHandler FileAssocHandler =>
            _fileAssocHandler ??= FileAssociationHandlerFactory.CreateFileAssociationHandler();

        public DeviceManagement DeviceManagement
        {
            get
            {
                if (_deviceManagement == null && !DeviceManagementLocked)
                    _deviceManagement = new DeviceManagement();
                return _deviceManagement;
            }
        }

        public bool DeviceManagementLocked
        {
            get => _deviceManagementLocked;
            private set
            {
                if (_deviceManagementLocked == value)
                    return;
                _deviceManagementLocked = value;
                FirePropertyChanged(nameof(DeviceManagementLocked));
            }
        }

        public void DisposeDeviceManagement(bool deviceManagementLocked)
        {
            DeviceManagementLocked = deviceManagementLocked;
            if (_deviceManagement == null)
                return;
            var currentDeviceOverride = SyncControls.Instance.CurrentDeviceOverride;
            if (currentDeviceOverride.IsValid)
            {
                CommitList.RemoveByIntValue(currentDeviceOverride.IsGuest ? -1 : currentDeviceOverride.ID);
                CommitList.RemoveByStringValue("OnSyncPartnershipCommit");
            }
            else
                CommitList.RemoveByIntValue(-1);
            _deviceManagement.Dispose();
            _deviceManagement = null;
            FirePropertyChanged("DeviceManagementChanged");
        }

        public bool DeviceManagementChanged => true;

        public CommitListHashtable CommitList
        {
            get => _commitList ??= new CommitListHashtable();
            set
            {
                if (_commitList == value)
                    return;
                _commitList = value;
                FirePropertyChanged(nameof(CommitList));
                if (value != null)
                    return;
                HasPendingCommits = false;
            }
        }

        public bool HasPendingCommits
        {
            get => _hasPendingCommits;
            internal set
            {
                if (_hasPendingCommits == value)
                    return;
                _hasPendingCommits = value;
                FirePropertyChanged(nameof(HasPendingCommits));
            }
        }

        public bool ActiveDeviceHasPendingCommits => CommitList.ContainsIntValue(SyncControls.Instance.CurrentDevice.ID);

        public bool ChangeRequiresElevation
        {
            get => _changeRequiresElevation;
            set
            {
                if (_changeRequiresElevation == value)
                    return;
                _changeRequiresElevation = value;
                FirePropertyChanged(nameof(ChangeRequiresElevation));
            }
        }

        public string BuildNumber => VersionInfo.BuildNumber;

        public Choice RecordMode
        {
            get
            {
                if (_recordMode == null)
                {
                    _wmaRate = new Choice(this);
                    _wmaRate.Options = new NamedIntOption[]
                    {
                        new(null, Shell.LoadString(StringId.IDS_RIP_WMA_48), 48000),
                        new(null, Shell.LoadString(StringId.IDS_RIP_WMA_64), 64000),
                        new(null, Shell.LoadString(StringId.IDS_RIP_WMA_96), 96000),
                        new(null, Shell.LoadString(StringId.IDS_RIP_WMA_128), 128000),
                        new(null, Shell.LoadString(StringId.IDS_RIP_WMA_160), 160000),
                        new(null, Shell.LoadString(StringId.IDS_RIP_WMA_192), 192000)
                    };
                    NamedIntOption.SelectOptionByValue(_wmaRate, ClientConfiguration.Recorder.WMARecordRate);
                    _wmaRate.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnWmaRateCommit)] = null;
                    _wmavRate = new Choice(this);
                    _wmavRate.Options = new NamedIntOption[]
                    {
                        new(null, Shell.LoadString(StringId.IDS_RIP_WMAV_25), 25),
                        new(null, Shell.LoadString(StringId.IDS_RIP_WMAV_50), 50),
                        new(null, Shell.LoadString(StringId.IDS_RIP_WMAV_75), 75),
                        new(null, Shell.LoadString(StringId.IDS_RIP_WMAV_90), 90),
                        new(null, Shell.LoadString(StringId.IDS_RIP_WMAV_98), 98)
                    };
                    NamedIntOption.SelectOptionByValue(_wmavRate, ClientConfiguration.Recorder.WMAVBRRecordQuality);
                    _wmavRate.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnWmavRateCommit)] = null;
                    _mp3Rate = new Choice(this);
                    _mp3Rate.Options = new NamedIntOption[]
                    {
                        new(null, Shell.LoadString(StringId.IDS_RIP_MP3_128), 128000),
                        new(null, Shell.LoadString(StringId.IDS_RIP_MP3_192), 192000),
                        new(null, Shell.LoadString(StringId.IDS_RIP_MP3_256), 256000),
                        new(null, Shell.LoadString(StringId.IDS_RIP_MP3_320), 320000)
                    };
                    NamedIntOption.SelectOptionByValue(_mp3Rate, ClientConfiguration.Recorder.MP3RecordRate);
                    _mp3Rate.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnMp3RateCommit)] = null;
                    _recordMode = new Choice(this);
                    _recordMode.Options = new RecordModeOption[]
                    {
                        new(null, Shell.LoadString(StringId.IDS_WMA_OPTION), 0, _wmaRate),
                        new(null, Shell.LoadString(StringId.IDS_WMA_VARIABLE_OPTION), 3, _wmavRate),
                        new(null, Shell.LoadString(StringId.IDS_WMA_LOSSLESS_OPTION), 1,  null),
                        new(null, Shell.LoadString(StringId.IDS_MP3_OPTION), 2, _mp3Rate)
                    };
                    NamedIntOption.SelectOptionByValue(_recordMode, ClientConfiguration.Recorder.RecordMode);
                    _recordMode.ChosenChanged += (sender, args) =>
                    {
                        CommitList[new ProxySettingDelegate(OnRecordModeCommit)] = null;
                        RecordRate = ((RecordModeOption)_recordMode.ChosenValue).BitRate;
                    };
                    RecordRate = ((RecordModeOption)_recordMode.ChosenValue).BitRate;
                }
                return _recordMode;
            }
        }

        private void OnWmaRateCommit(object data) => ClientConfiguration.Recorder.WMARecordRate = ((NamedIntOption)_wmaRate.ChosenValue).Value;

        private void OnWmavRateCommit(object data) => ClientConfiguration.Recorder.WMAVBRRecordQuality = ((NamedIntOption)_wmavRate.ChosenValue).Value;

        private void OnMp3RateCommit(object data) => ClientConfiguration.Recorder.MP3RecordRate = ((NamedIntOption)_mp3Rate.ChosenValue).Value;

        private void OnRecordModeCommit(object data) => ClientConfiguration.Recorder.RecordMode = ((NamedIntOption)_recordMode.ChosenValue).Value;

        public Choice RecordRate
        {
            get => _recordRate;
            private set
            {
                if (_recordRate == value)
                    return;
                _recordRate = value;
                FirePropertyChanged(nameof(RecordRate));
            }
        }

        public Category AlertedDeviceCategory
        {
            get => _alertedDeviceCategory;
            set
            {
                if (_alertedDeviceCategory == value)
                    return;
                if (_currentCategoryPage != null && value != null && _alertedDeviceCategory != null)
                {
                    _currentCategoryPage.CurrentCategory = _alertedDeviceCategory;
                    _alertedDeviceCategory = null;
                }
                else
                {
                    _alertedDeviceCategory = value;
                    FirePropertyChanged(nameof(AlertedDeviceCategory));
                }
            }
        }

        public CategoryPage CurrentCategoryPage
        {
            get => _currentCategoryPage;
            set
            {
                if (_currentCategoryPage == value)
                    return;
                _currentCategoryPage = value;
                FirePropertyChanged(nameof(CurrentCategoryPage));
            }
        }

        public static void NavigateToSetupLandWizard(SetupLandPage page) => NavigateAwayFromCategory(new SetupLandWizardNavigationCommand(page));

        public static void NavigateToCategory(Category category)
        {
            if (ZuneShell.DefaultInstance.Management.CurrentCategoryPage == null)
                return;
            ZuneShell.DefaultInstance.Management.CurrentCategoryPage.CurrentCategory = category;
        }

        public static void NavigateAwayFromCategory(Command confirmed)
        {
            var management = ZuneShell.DefaultInstance.Management;
            if (management.HasPendingCommits)
            {
                var yesCommand = new Command(null, Shell.LoadString(StringId.IDS_DIALOG_YES), null);
                yesCommand.Invoked += (sender, args) =>
                {
                    management.CommitListSave();
                    NavigateAwayFromCategory(confirmed);
                };
                var noCommand = new Command(null, Shell.LoadString(StringId.IDS_DIALOG_NO), null);
                noCommand.Invoked += (sender, args) =>
                {
                    management.CommitList = null;
                    NavigateAwayFromCategory(confirmed);
                };
                MessageBox.Show(Shell.LoadString(StringId.IDS_SAVE_CHANGES_DIALOG_TITLE), Shell.LoadString(StringId.IDS_SAVE_CHANGES_ON_BACK_DIALOG_TEXT), yesCommand, noCommand, null);
            }
            else
            {
                if (Shell.SettingsFrame.IsCurrent && !Shell.SettingsFrame.Wizard.FUE.IsCurrent && management.CurrentCategoryPage != null)
                    management.CurrentCategoryPage.CancelAndExit();
                Application.DeferredInvoke(args => confirmed?.Invoke(), null);
            }
        }

        public void CommitListSave()
        {
            CheckForAutomatedRequirements();
            CommitList.Save();
        }

        public void CheckForAutomatedRequirements()
        {
            if (DeviceManagement.SetupDevice == null)
                return;
            DeviceManagement.CheckForAutomatedRequirements();
        }

        public bool CanFileAssociationBeChanged
        {
            get => _canFileAssociationBeChanged;
            private set
            {
                if (_canFileAssociationBeChanged == value)
                    return;
                _canFileAssociationBeChanged = value;
                FirePropertyChanged(nameof(CanFileAssociationBeChanged));
            }
        }

        public SubscriptionState SubscribeToChannelFeed(
          bool isPersonalChannel,
          Guid channelId,
          string feedUrl,
          string title,
          ESubscriptionSource source)
        {
            return SubscribeToFeed(feedUrl, title, channelId, isPersonalChannel, source, EMediaTypes.eMediaTypePlaylist, Shell.LoadString(StringId.IDS_PLAYLIST_SUBSCRIPTION_ERROR));
        }

        public SubscriptionState SubscribeToPodcastFeed(
          string feedUrl,
          string title,
          ESubscriptionSource source)
        {
            return SubscribeToPodcastFeed(feedUrl, title, Guid.Empty, source);
        }

        public SubscriptionState SubscribeToPodcastFeed(
          string feedUrl,
          string title,
          Guid serviceId,
          ESubscriptionSource source)
        {
            return SubscribeToFeed(feedUrl, title, serviceId, false, source, EMediaTypes.eMediaTypePodcastSeries, Shell.LoadString(StringId.IDS_PODCAST_SUBSCRIPTION_ERROR));
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
            SubscriptionState subscriptionState = null;
            HRESULT hresult = SubscriptionManager.Instance.Subscribe(feedUrl, title, serviceId, isPersonalChannel, mediaType, source, out var subscriptionMediaId);
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
                return null;
            try
            {
                var byUrl = SubscriptionManager.Instance.FindByUrl(feedURL, subscriptionType, out var subscriptionMediaId, out var isSubscribed);
                return new SubscriptionState(isSubscribed, byUrl, subscriptionMediaId);
            }
            catch (ApplicationException ex)
            {
            }
            return null;
        }

        public SubscriptionState GetSubscriptionState(
          Guid serviceId,
          EMediaTypes subscriptionType)
        {
            if (serviceId == Guid.Empty)
                return null;
            try
            {
                var byServiceId = SubscriptionManager.Instance.FindByServiceId(serviceId, subscriptionType, out var subscriptionMediaId, out var isSubscribed);
                return new SubscriptionState(isSubscribed, byServiceId, subscriptionMediaId);
            }
            catch (ApplicationException ex)
            {
            }
            return null;
        }

        private void OnGrovelerConfigurationChanged(object sender, ConfigurationChangeEventArgs e) => Application.DeferredInvoke(delegate
       {
           if (!UsingWin7Libraries)
               return;
           if (e.PropertyName == "RipDirectory" || e.PropertyName == "MonitoredAudioFolders")
           {
               if (_monitoredAudioFolders != null)
               {
                   _monitoredAudioFolders = null;
                   FirePropertyChanged("MonitoredAudioFolders");
               }
               if (_mediaFolder == null)
                   return;
               _mediaFolder = null;
               FirePropertyChanged("MediaFolder");
           }
           else if (e.PropertyName == "PhotoMediaFolder" || e.PropertyName == "MonitoredPhotoFolders")
           {
               if (_monitoredPhotoFolders != null)
               {
                   _monitoredPhotoFolders = null;
                   FirePropertyChanged("MonitoredPhotoFolders");
               }
               if (_photoMediaFolder == null)
                   return;
               _photoMediaFolder = null;
               FirePropertyChanged("PhotoMediaFolder");
           }
           else if (e.PropertyName == "PodcastMediaFolder" || e.PropertyName == "MonitoredPodcastFolders")
           {
               if (_monitoredPodcastFolders != null)
               {
                   _monitoredPodcastFolders = null;
                   FirePropertyChanged("MonitoredPodcastFolders");
               }
               if (_podcastMediaFolder == null)
                   return;
               _podcastMediaFolder = null;
               FirePropertyChanged("PodcastMediaFolder");
           }
           else
           {
               if (e.PropertyName != "VideoMediaFolder" && e.PropertyName != "MonitoredVideoFolders")
                   return;
               if (_monitoredVideoFolders != null)
               {
                   _monitoredVideoFolders = null;
                   FirePropertyChanged("MonitoredVideoFolders");
               }
               if (_videoMediaFolder == null)
                   return;
               _videoMediaFolder = null;
               FirePropertyChanged("VideoMediaFolder");
           }
       }, null);

        public ListDataSet MonitoredAudioFolders
        {
            get
            {
                if (_monitoredAudioFolders == null)
                {
                    _monitoredAudioFolders = UsingWin7Libraries
                        ? StringsToListDataSet(ClientConfiguration.Groveler.RipDirectory, ClientConfiguration.Groveler.MonitoredAudioFolders)
                        : StringsToListDataSet((object)ClientConfiguration.Groveler.MonitoredAudioFolders);
                }
                return _monitoredAudioFolders;
            }
        }

        public ListDataSet MonitoredPhotoFolders
        {
            get
            {
                if (_monitoredPhotoFolders == null)
                {
                    _monitoredPhotoFolders = UsingWin7Libraries
                        ? StringsToListDataSet(ClientConfiguration.Groveler.PhotoMediaFolder, ClientConfiguration.Groveler.MonitoredPhotoFolders)
                        : StringsToListDataSet((object)ClientConfiguration.Groveler.MonitoredPhotoFolders);
                }
                return _monitoredPhotoFolders;
            }
        }

        public ListDataSet MonitoredPodcastFolders
        {
            get
            {
                if (_monitoredPodcastFolders == null)
                {
                    _monitoredPodcastFolders = UsingWin7Libraries
                        ? StringsToListDataSet(ClientConfiguration.Groveler.PodcastMediaFolder, ClientConfiguration.Groveler.MonitoredPodcastFolders)
                        : StringsToListDataSet(ClientConfiguration.Groveler.MonitoredPodcastFolders);
                }
                return _monitoredPodcastFolders;
            }
        }

        public ListDataSet MonitoredVideoFolders
        {
            get
            {
                if (_monitoredVideoFolders == null)
                {
                    _monitoredVideoFolders = UsingWin7Libraries
                        ? StringsToListDataSet(ClientConfiguration.Groveler.VideoMediaFolder, ClientConfiguration.Groveler.MonitoredVideoFolders)
                        : StringsToListDataSet(ClientConfiguration.Groveler.MonitoredVideoFolders);
                }
                return _monitoredVideoFolders;
            }
        }

        public bool Win7LibrariesAreAvailable => OSVersion.IsWin7();

        public bool UsingWin7Libraries => Win7LibrariesAreAvailable && ClientConfiguration.Groveler.LibrarySync != -1;

        public void UseWin7Libraries()
        {
            foreach (var proxySettingDelegate in _actionsToCommitOnLibraryIntegrate)
            {
                if (!CommitList.ContainsKey(proxySettingDelegate))
                    continue;
                
                CommitList.Remove(proxySettingDelegate);
                proxySettingDelegate(null);
            }
            SQMLog.Log(SQMDataId.ZuneWin7LibraryOpt, 0);
            SetWin7LibrariesUsage(Win7LibrariesUsage.BeginIntegration);
        }

        public void DoNotUseWin7Libraries()
        {
            SQMLog.Log(SQMDataId.ZuneWin7LibraryOpt, 1);
            SetWin7LibrariesUsage(Win7LibrariesUsage.DoNotIntegrate);
        }

        private void SetWin7LibrariesUsage(Win7LibrariesUsage usage)
        {
            ClientConfiguration.Groveler.LibrarySync = (int)usage;
            FirePropertyChanged("UsingWin7Libraries");
        }

        private ListDataSet StringsToListDataSet(params object[] source)
        {
            ListDataSet listDataSet = new ArrayListDataSet(this);
            if (source != null && source.Length > 0)
            {
                var dictionary = new Dictionary<string, object>();
                foreach (var obj in source)
                {
                    if (obj == null)
                        continue;
                    
                    if (obj is not IEnumerable<string> strings)
                        strings = [obj.ToString()];

                    foreach (var str in strings)
                    {
                        if (string.IsNullOrEmpty(str))
                            continue;
                        var lower = str.ToLower();
                        if (dictionary.TryAdd(lower, null))
                            listDataSet.Add(str);
                    }
                }
            }
            listDataSet.Sort();
            return listDataSet;
        }

        private IList<string> ListDataSetToIList(ListDataSet listDataSet)
        {
            if (listDataSet == null)
                return new List<string>();
            
            var stringList = new List<string>(listDataSet.Count);
            stringList.AddRange(listDataSet.Cast<string>());

            return stringList;
        }

        internal bool IsMonitored(ListDataSet monitoredFolders, string path)
        {
            return monitoredFolders
                .Cast<string>()
                .Any(monitoredFolder => IsSubfolder(monitoredFolder, path));
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

        public void AddMonitoredFolder(ListDataSet monitoredFolders) => FolderBrowseDialog.Show(Shell.LoadString(StringId.IDS_ADD_MONITORED_FOLDER_DIALOG_TITLE), args =>
       {
           if (args == null)
               return;
           var str = (string)args;
           if (ZuneApplication.ZuneLibrary.CanAddFromFolder(str))
               AddMonitoredFolder(monitoredFolders, str, false);
           else
               MessageBox.Show(Shell.LoadString(StringId.IDS_INVALID_MONITORED_FOLDER_TITLE), Shell.LoadString(StringId.IDS_INVALID_MONITORED_FOLDER_MESSAGE), null);
       });

        public void AddMonitoredFolder(ListDataSet monitoredFolders, string path, bool commit)
        {
            var isAlreadyMonitored = monitoredFolders.Cast<string>().Contains(path);
            if (!isAlreadyMonitored)
                monitoredFolders.Add(path);
            
            SaveMonitoredFolders(commit);
        }

        public void OpenMediaFile() => FileOpenDialog.Show(Shell.LoadString(StringId.IDS_OPEN_FILE_DIALOG_TITLE), MediaFolder, args => { });

        public bool RemoveChildMonitoredFolders(string path, bool commit)
        {
            var flag = false | RemoveChildMonitoredFolders(MonitoredAudioFolders, EMediaTypes.eMediaTypeAudio, path) | RemoveChildMonitoredFolders(MonitoredPhotoFolders, EMediaTypes.eMediaTypeImage, path) | RemoveChildMonitoredFolders(MonitoredPodcastFolders, EMediaTypes.eMediaTypePodcastEpisode, path) | RemoveChildMonitoredFolders(MonitoredVideoFolders, EMediaTypes.eMediaTypeVideo, path);
            SaveMonitoredFolders(commit);
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
            var intList = new List<int>();
            for (var itemIndex = 0; itemIndex < monitoredFolders.Count; ++itemIndex)
            {
                if (IsSubfolder(path, (string)monitoredFolders[itemIndex]))
                    intList.Add(itemIndex);
            }
            foreach (var num in intList)
            {
                if (UsingWin7Libraries)
                    Win7ShellManager.Instance.RemoveLocationFromLibrary(libraryKind, out var _, (string)monitoredFolders[num]);
                else
                    RemoveMonitoredFolder(monitoredFolders, num, type);
            }
            return intList.Count > 0;
        }

        public void RemoveMonitoredFolder(ListDataSet monitoredFolders, string path, bool commit)
        {
            for (var index = 0; index < monitoredFolders.Count; ++index)
            {
                if (!path.Equals((string)monitoredFolders[index], StringComparison.OrdinalIgnoreCase))
                    continue;
                RemoveMonitoredFolder(monitoredFolders, index, commit);
                break;
            }
        }

        private void RemoveMonitoredFolder(ListDataSet monitoredFolders, int index) => RemoveMonitoredFolder(monitoredFolders, index, false);

        public void RemoveMonitoredFolder(
          ListDataSet monitoredFolders,
          int index,
          EMediaTypes mediaType)
        {
            _removedMonitoredFoldersToRemoveFromCollection ??= [];
            _removedMonitoredFoldersToRemoveFromCollection.Add(new MonitoredFolder((string)monitoredFolders[index], mediaType));
            RemoveMonitoredFolder(monitoredFolders, index, false);
        }

        private void RemoveMonitoredFolder(ListDataSet monitoredFolders, int index, bool commit)
        {
            monitoredFolders.RemoveAt(index);
            SaveMonitoredFolders(commit);
        }

        private void SaveMonitoredFolders() => SaveMonitoredFolders(false);

        public void SaveMonitoredFolders(bool commit)
        {
            if (commit)
                OnMonitoredFoldersCommit(null);
            else
                CommitList[new ProxySettingDelegate(OnMonitoredFoldersCommit)] = null;
        }

        public void OpenLibraryDialog(EMediaTypes mediaType)
        {
            var winHandle = Application.Window.Handle;
            var thread = new Thread(args =>
            {
                var libraryKind = EWin7LibraryKind.eMusicLibrary;
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
                Win7ShellManager.Instance.ShowLibraryDialog(libraryKind, winHandle, null, null);
            });
            thread.TrySetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void OnMonitoredFoldersCommit(object data)
        {
            SQMLog.LogToStream(SQMDataId.MonitoredAudioFolders, (uint)MonitoredAudioFolders.Count);
            SQMLog.LogToStream(SQMDataId.MonitoredPhotoFolders, (uint)MonitoredPhotoFolders.Count);
            SQMLog.LogToStream(SQMDataId.MonitoredPodcastFolders, (uint)MonitoredPodcastFolders.Count);
            SQMLog.LogToStream(SQMDataId.MonitoredVideoFolders, (uint)MonitoredVideoFolders.Count);
            if (!UsingWin7Libraries)
            {
                if (_removedMonitoredFoldersToRemoveFromCollection != null)
                    foreach (var foldersToRemoveFrom in _removedMonitoredFoldersToRemoveFromCollection)
                        ZuneApplication.ZuneLibrary.DeleteRootFolder(foldersToRemoveFrom.Path, foldersToRemoveFrom.Schema);
                
                ClientConfiguration.Groveler.MonitoredAudioFolders = ListDataSetToIList(MonitoredAudioFolders);
                ClientConfiguration.Groveler.MonitoredPhotoFolders = ListDataSetToIList(MonitoredPhotoFolders);
                ClientConfiguration.Groveler.MonitoredPodcastFolders = ListDataSetToIList(MonitoredPodcastFolders);
                ClientConfiguration.Groveler.MonitoredVideoFolders = ListDataSetToIList(MonitoredVideoFolders);
                _removedMonitoredFoldersToRemoveFromCollection = null;
                _monitoredAudioFolders = null;
                _monitoredPhotoFolders = null;
                _monitoredPodcastFolders = null;
                _monitoredVideoFolders = null;
            }
            if (!HME.SharingEnabled)
                return;
            HME.SetSharedFoldersList(true);
        }

        public BooleanChoice AutoCopyCD
        {
            get
            {
                if (_autoCopyCD == null)
                {
                    _autoCopyCD = new BooleanChoice(this, Shell.LoadString(StringId.IDS_AUTO_RIP));
                    _autoCopyCD.Value = ClientConfiguration.Recorder.AutoCopyCD != 0;
                    _autoCopyCD.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnAutoCopyCDCommit)] = null;
                }
                return _autoCopyCD;
            }
        }

        private void OnAutoCopyCDCommit(object data) => ClientConfiguration.Recorder.AutoCopyCD = _autoCopyCD.Value ? 1 : 0;

        public BooleanChoice AutoEjectCD
        {
            get
            {
                if (_autoEjectCD == null)
                {
                    _autoEjectCD = new BooleanChoice(this, Shell.LoadString(StringId.IDS_EJECT_AFTER_RIP));
                    _autoEjectCD.Value = ClientConfiguration.Recorder.AutoEjectCD != 0;
                    _autoEjectCD.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnAutoEjectCDCommit)] = null;
                }
                return _autoEjectCD;
            }
        }

        private void OnAutoEjectCDCommit(object data) => ClientConfiguration.Recorder.AutoEjectCD = _autoEjectCD.Value ? 1 : 0;

        public bool MediaFolderHasSharedPathWithMonitoredFolder(
          string monitoredFolder,
          string mediaFolder)
        {
            var flag = false;
            if (!string.IsNullOrEmpty(monitoredFolder) && !string.IsNullOrEmpty(mediaFolder))
            {
                var localizedFolderPath1 = LocalizationHelper.GetLocalizedFolderPath(monitoredFolder);
                var localizedFolderPath2 = LocalizationHelper.GetLocalizedFolderPath(mediaFolder);
                if (localizedFolderPath1[localizedFolderPath1.Length - 1] != Path.PathSeparator)
                    localizedFolderPath1 += (string)(object)Path.PathSeparator;
                if (localizedFolderPath2[localizedFolderPath2.Length - 1] != Path.PathSeparator)
                    localizedFolderPath2 += (string)(object)Path.PathSeparator;
                flag = localizedFolderPath1.ToLower().IndexOf(localizedFolderPath2.ToLower()) == 0;
            }
            return flag;
        }

        public string MediaFolder
        {
            get =>
                _mediaFolder ??= LocalizationHelper.GetLocalizedFolderPath(ClientConfiguration.Groveler.RipDirectory);
            set
            {
                if (_mediaFolder == value)
                    return;
                CommitList[new ProxySettingDelegate(OnMediaFolderCommit)] = null;
                _mediaFolder = value;
                FirePropertyChanged(nameof(MediaFolder));
            }
        }

        private void OnMediaFolderCommit(object data)
        {
            if (UsingWin7Libraries)
                return;
            ClientConfiguration.Groveler.RipDirectory = _mediaFolder;
            UpdateSharedFoldersList();
            _mediaFolder = null;
        }

        public string VideoMediaFolder
        {
            get
            {
                if (_videoMediaFolder == null)
                {
                    _videoMediaFolder = ClientConfiguration.Groveler.VideoMediaFolder;
                    if (string.IsNullOrEmpty(_videoMediaFolder) && ((HRESULT)ZuneApplication.ZuneLibrary.GetKnownFolders(out _, out _, out _, out _, out _, out _, out var videoMediaFolder, out _, out _, out _)).IsSuccess)
                        _videoMediaFolder = LocalizationHelper.GetLocalizedFolderPath(videoMediaFolder);
                }
                return _videoMediaFolder;
            }
            set
            {
                if (_videoMediaFolder == value)
                    return;
                CommitList[new ProxySettingDelegate(OnVideoMediaFolderCommit)] = null;
                _videoMediaFolder = value;
                FirePropertyChanged(nameof(VideoMediaFolder));
            }
        }

        private void OnVideoMediaFolderCommit(object data)
        {
            if (UsingWin7Libraries)
                return;
            ClientConfiguration.Groveler.VideoMediaFolder = _videoMediaFolder;
            UpdateSharedFoldersList();
            _videoMediaFolder = null;
        }

        public string PhotoMediaFolder
        {
            get
            {
                if (_photoMediaFolder == null)
                {
                    _photoMediaFolder = ClientConfiguration.Groveler.PhotoMediaFolder;
                    if (string.IsNullOrEmpty(_photoMediaFolder) && ((HRESULT)ZuneApplication.ZuneLibrary.GetKnownFolders(out _, out _, out _, out _, out _, out _, out _, out var photoMediaFolder, out _, out _)).IsSuccess)
                        _photoMediaFolder = LocalizationHelper.GetLocalizedFolderPath(photoMediaFolder);
                }
                return _photoMediaFolder;
            }
            set
            {
                if (_photoMediaFolder == value)
                    return;
                CommitList[new ProxySettingDelegate(OnPhotoMediaFolderCommit)] = null;
                _photoMediaFolder = value;
                FirePropertyChanged(nameof(PhotoMediaFolder));
            }
        }

        private void OnPhotoMediaFolderCommit(object data)
        {
            if (UsingWin7Libraries)
                return;
            ClientConfiguration.Groveler.PhotoMediaFolder = _photoMediaFolder;
            UpdateSharedFoldersList();
            _photoMediaFolder = null;
        }

        public string PodcastMediaFolder
        {
            get
            {
                if (_podcastMediaFolder == null)
                {
                    _podcastMediaFolder = ClientConfiguration.Groveler.PodcastMediaFolder;
                    if (string.IsNullOrEmpty(_podcastMediaFolder) && ((HRESULT)ZuneApplication.ZuneLibrary.GetKnownFolders(out _, out _, out _, out _, out _, out _, out _, out _, out var podcastMediaFolder, out _)).IsSuccess)
                        _podcastMediaFolder = LocalizationHelper.GetLocalizedFolderPath(podcastMediaFolder);
                }
                return _podcastMediaFolder;
            }
            set
            {
                if (_podcastMediaFolder == value)
                    return;
                CommitList[new ProxySettingDelegate(OnPodcastMediaFolderCommit)] = null;
                _podcastMediaFolder = value;
                FirePropertyChanged(nameof(PodcastMediaFolder));
            }
        }

        private void OnPodcastMediaFolderCommit(object data)
        {
            if (UsingWin7Libraries)
                return;
            ClientConfiguration.Groveler.PodcastMediaFolder = _podcastMediaFolder;
            UpdateSharedFoldersList();
            _podcastMediaFolder = null;
        }

        private void UpdateSharedFoldersList()
        {
            if (!HME.SharingEnabled)
                return;
            HME.SetSharedFoldersList(true);
        }

        public void ChooseMediaFolder(MediaType mediaType) => FolderBrowseDialog.Show(Shell.LoadString(StringId.IDS_CHANGE_MEDIA_FOLDER_DIALOG_TITLE), args =>
        {
            var folder = (string)args;
            if (folder == null)
                return;
            if (FolderBrowseDialog.CanWriteToFolder(folder) && ZuneApplication.ZuneLibrary.CanAddFromFolder(folder))
            {
                switch (mediaType)
                {
                    case MediaType.Track:
                        MediaFolder = folder;
                        break;
                    case MediaType.Video:
                        VideoMediaFolder = folder;
                        break;
                    case MediaType.Photo:
                        PhotoMediaFolder = folder;
                        break;
                    case MediaType.Podcast:
                        PodcastMediaFolder = folder;
                        break;
                }
            }
            else
                MessageBox.Show(Shell.LoadString(StringId.IDS_INVALID_MEDIA_FOLDER_TITLE),
                    Shell.LoadString(StringId.IDS_INVALID_MEDIA_FOLDER_MESSAGE), null);
        }, true);

        public BooleanChoice AutoEjectCDAfterBurn
        {
            get
            {
                if (_autoEjectCDAfterBurn == null)
                {
                    _autoEjectCDAfterBurn = new BooleanChoice(this, Shell.LoadString(StringId.IDS_BURN_AUTO_EJECT_CHECK));
                    _autoEjectCDAfterBurn.Value = ClientConfiguration.CDBurn.AutoEject;
                    _autoEjectCDAfterBurn.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnAutoEjectCDAfterBurnCommit)] = null;
                }
                return _autoEjectCDAfterBurn;
            }
        }

        private void OnAutoEjectCDAfterBurnCommit(object data) => ClientConfiguration.CDBurn.AutoEject = _autoEjectCDAfterBurn.Value;

        public Choice BurnFormat
        {
            get
            {
                if (_burnDiscFormat == null)
                {
                    _burnDiscFormat = new Choice(this);
                    _burnDiscFormat.Options = new NamedIntOption[]
                    {
                        new(null, Shell.LoadString(StringId.IDS_BURN_AUDIO_OPTION), 0),
                        new(null, Shell.LoadString(StringId.IDS_BURN_DATA_OPTION), 1)
                    };
                    NamedIntOption.SelectOptionByValue(_burnDiscFormat, ClientConfiguration.CDBurn.DiscFormat);
                    _burnDiscFormat.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnBurnDiscFormatCommit)] = null;
                }
                return _burnDiscFormat;
            }
        }

        private void OnBurnDiscFormatCommit(object data)
        {
            ClientConfiguration.CDBurn.DiscFormat = ((NamedIntOption)_burnDiscFormat.ChosenValue).Value;
            CDAccess.Instance.UpdateIsAudioBurn();
        }

        public Choice BurnSpeed
        {
            get
            {
                if (_burnSpeed == null)
                {
                    _burnSpeed = new Choice(this);
                    _burnSpeed.Options = new NamedIntOption[]
                    {
                        new(null, Shell.LoadString(StringId.IDS_BURN_FASTEST_OPTION), 0),
                        new(null, Shell.LoadString(StringId.IDS_BURN_FAST_OPTION), 1),
                        new(null, Shell.LoadString(StringId.IDS_BURN_MEDIUM_OPTION), 2),
                        new(null, Shell.LoadString(StringId.IDS_BURN_SLOW_OPTION), 3)
                    };
                    NamedIntOption.SelectOptionByValue(_burnSpeed, ClientConfiguration.CDBurn.BurnSpeed);
                    _burnSpeed.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnBurnSpeedCommit)] = null;
                }
                return _burnSpeed;
            }
        }

        private void OnBurnSpeedCommit(object data) => ClientConfiguration.CDBurn.BurnSpeed = ((NamedIntOption)_burnSpeed.ChosenValue).Value;

        public BooleanChoice MediaInfoChoice
        {
            get
            {
                if (_mediaInfoChoice == null)
                {
                    var stringId = StringId.IDS_UPDATE_METADATA_CHECK;
                    if (FeatureEnablement.IsFeatureEnabled(Features.eQuickMixZmp) || FeatureEnablement.IsFeatureEnabled(Features.eQuickMixLocal))
                        stringId = !FeatureEnablement.IsFeatureEnabled(Features.eMixview) ? StringId.IDS_UPDATE_METADATA_QUICKMIX_CHECK : StringId.IDS_UPDATE_METADATA_FEATURES_CHECK;
                    _mediaInfoChoice = new BooleanChoice(this, Shell.LoadString(stringId));
                    _mediaInfoChoice.Value = ClientConfiguration.MediaStore.ConnectToInternetForAlbumMetadata;
                    _mediaInfoChoice.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnMediaInfoChoiceCommit)] = null;
                }
                return _mediaInfoChoice;
            }
        }

        private void OnMediaInfoChoiceCommit(object data) => ClientConfiguration.MediaStore.ConnectToInternetForAlbumMetadata = _mediaInfoChoice.Value;

        public void ScanAndClearDeletedMedia() => ZuneLibrary.ScanAndClearDeletedMedia();

        public BooleanChoice SqmChoice
        {
            get
            {
                if (_sqmChoice == null)
                {
                    _sqmChoice = new BooleanChoice(this, Shell.LoadString(StringId.IDS_USAGE_DATA_CHECK));
                    _sqmChoice.Value = ClientConfiguration.SQM.UsageTracking;
                    _sqmChoice.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnSqmChoiceCommit)] = null;
                }
                return _sqmChoice;
            }
        }

        private void OnSqmChoiceCommit(object data)
        {
            ClientConfiguration.SQM.UsageTracking = _sqmChoice.Value;
            ClientConfiguration.FUE.AcceptedPrivacyStatement = _sqmChoice.Value;
        }

        public Choice PodcastDefaultKeepEpisodesChoice
        {
            get
            {
                if (_podcastDefaultKeepEpisodesChoice == null)
                {
                    var choice = new Choice(this);
                    choice.Options = NamedIntOption.PodcastKeepOptions;
                    NamedIntOption.SelectOptionByValue(choice, ClientConfiguration.Series.PodcastDefaultKeepEpisodes);
                    _podcastDefaultKeepEpisodesChoice = choice;
                    choice.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnPodcastDefaultKeepEpisodesChoiceCommit)] = null;
                }
                return _podcastDefaultKeepEpisodesChoice;
            }
        }

        private void OnPodcastDefaultKeepEpisodesChoiceCommit(object data) => ClientConfiguration.Series.PodcastDefaultKeepEpisodes = ((NamedIntOption)_podcastDefaultKeepEpisodesChoice.ChosenValue).Value;

        public Choice PodcastPlaybackChoice
        {
            get
            {
                if (_podcastPlaybackChoice == null)
                {
                    _podcastPlaybackChoice = new Choice(this);
                    _podcastPlaybackChoice.Options = NamedIntOption.PodcastPlaybackOptions;
                    NamedIntOption.SelectOptionByValue(_podcastPlaybackChoice, ClientConfiguration.Series.PodcastDefaultPlaybackOrder);
                    _podcastPlaybackChoice.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnPodcastPlaybackChoiceCommit)] = null;
                }
                return _podcastPlaybackChoice;
            }
        }

        private void OnPodcastPlaybackChoiceCommit(object data) => ClientConfiguration.Series.PodcastDefaultPlaybackOrder = ((NamedIntOption)_podcastPlaybackChoice.ChosenValue).Value;

        public BooleanChoice MetadataChoice
        {
            get
            {
                if (_metadataChoice == null)
                {
                    var commandArray = new Command[]
                    {
                        new(this, Shell.LoadString(StringId.IDS_MISSING_METADATA), null),
                        new(this, Shell.LoadString(StringId.IDS_OVERWRITE_METADATA), null)
                    };
                    _metadataChoice = new BooleanChoice(this);
                    _metadataChoice.Options = commandArray;
                    _metadataChoice.Value = ClientConfiguration.MediaStore.OverwriteAllMetadata;
                    _metadataChoice.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnMetadataChoiceCommit)] = null;
                }
                return _metadataChoice;
            }
        }

        private void OnMetadataChoiceCommit(object data) => ClientConfiguration.MediaStore.OverwriteAllMetadata = _metadataChoice.Value;

        public IList AudioFileTypes
        {
            get
            {
                if (_audioFileTypes == null)
                    PopulateFileTypes();
                return (IList)_audioFileTypes;
            }
        }

        public IList VideoFileTypes
        {
            get
            {
                if (_videoFileTypes == null)
                    PopulateFileTypes();
                return (IList)_videoFileTypes;
            }
        }

        private void PopulateFileTypes()
        {
            if (_audioFileTypes != null && _videoFileTypes != null)
                return;
            _allFileTypes = new List<BooleanInputChoice>();
            _audioFileTypes = new List<BooleanInputChoice>();
            _videoFileTypes = new List<BooleanInputChoice>();
            HRESULT associationInfoList = FileAssocHandler.GetFileAssociationInfoList(out _fileAssociationInfoList);
            if (associationInfoList.IsSuccess)
            {
                CanFileAssociationBeChanged = FileAssocHandler.CanAssociationBeChanged();
                var format = Shell.LoadString(StringId.IDS_FILE_TYPES_DESCRIPTION_FORMAT);
                foreach (var fileAssociation in _fileAssociationInfoList)
                {
                    var extension = fileAssociation.Extension;
                    var booleanInputChoice = new BooleanInputChoice(this, string.Format(format, extension.Substring(1), fileAssociation.Description), CanFileAssociationBeChanged);
                    if (ClientConfiguration.FUE.ShowFUE && CanFileAssociationBeChanged && Array.IndexOf(_defaultFileTypeExtensions, extension) >= 0)
                        fileAssociation.IsCurrentlyOwned = true;
                    booleanInputChoice.Value = fileAssociation.IsCurrentlyOwned;
                    booleanInputChoice.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnFileTypesCommit)] = null;
                    switch (fileAssociation.MediaType)
                    {
                        case EMediaTypes.eMediaTypeAudio:
                            _audioFileTypes.Add(booleanInputChoice);
                            break;
                        case EMediaTypes.eMediaTypeVideo:
                            _videoFileTypes.Add(booleanInputChoice);
                            break;
                    }
                    _allFileTypes.Add(booleanInputChoice);
                }
            }
            else
            {
                ErrorDialogInfo.Show(associationInfoList.Int, Shell.LoadString(StringId.IDS_FILE_TYPES_ERROR_DIALOG_TITLE));
            }
        }

        private void OnFileTypesCommit(object data)
        {
            if (FileAssocHandler == null || !CanFileAssociationBeChanged)
                return;
            for (var index = 0; index < _allFileTypes.Count; ++index)
                _fileAssociationInfoList[index].IsCurrentlyOwned = _allFileTypes[index].Value;
            HRESULT hresult = FileAssocHandler.SetFileAssociationInfo(_fileAssociationInfoList);
            if (hresult.IsError)
                ErrorDialogInfo.Show(hresult.Int, Shell.LoadString(StringId.IDS_FILE_TYPES_ERROR_DIALOG_TITLE));
            if (!Shell.SettingsFrame.Wizard.IsCurrent)
                return;
            Fue.Instance.SetFileTypeAssociationsAreSet();
        }

        public void SelectAllFileTypes()
        {
            if (!CanFileAssociationBeChanged)
                return;
            foreach (var fileTypeChoice in _allFileTypes)
                fileTypeChoice.Value = true;
        }

        public void SaveFileTypesAsDefault()
        {
            if (!FileAssocHandler.CanAssociationBeChanged())
                return;
            HRESULT hresult = FileAssocHandler.GetFileAssociationInfoList(out var fileAssociationInfoList);
            if (hresult.IsSuccess)
            {
                foreach (var fileAssociation in fileAssociationInfoList)
                    if (Array.IndexOf(_defaultFileTypeExtensions, fileAssociation.Extension) >= 0)
                        fileAssociation.IsCurrentlyOwned = true;
                
                hresult = FileAssocHandler.SetFileAssociationInfo(fileAssociationInfoList);
                if (!hresult.IsError)
                    return;
            }

            ErrorDialogInfo.Show(hresult.Int, Shell.LoadString(StringId.IDS_FILE_TYPES_ERROR_DIALOG_TITLE));
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
            foreach (var uiDevice in SingletonModelItem<UIDeviceList>.Instance)
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
                if (_registeredDevicesModelList == null)
                    InitRegisteredTuners();
                return _registeredDevicesModelList;
            }
        }

        public ArrayListDataSet AppStoreDeviceList
        {
            get
            {
                if (_registeredAppStoreDevicesModelList == null)
                    InitRegisteredTuners();
                return _registeredAppStoreDevicesModelList;
            }
        }

        public ArrayListDataSet ComputerList
        {
            get
            {
                if (_registeredComputersModelList == null)
                    InitRegisteredTuners();
                return _registeredComputersModelList;
            }
        }

        public string NextPCDeregistrationDate
        {
            get => _nextPCDeregistrationDate;
            private set
            {
                if (_nextPCDeregistrationDate == value)
                    return;
                _nextPCDeregistrationDate = value;
                FirePropertyChanged(nameof(NextPCDeregistrationDate));
            }
        }

        public string NextSubscriptionDeviceDeregistrationDate
        {
            get => _nextSubscriptionDeviceDeregistrationDate;
            private set
            {
                if (_nextSubscriptionDeviceDeregistrationDate == value)
                    return;
                _nextSubscriptionDeviceDeregistrationDate = value;
                FirePropertyChanged(nameof(NextSubscriptionDeviceDeregistrationDate));
            }
        }

        public string NextAppStoreDeviceDeregistrationDate
        {
            get => _nextAppStoreDeviceDeregistrationDate;
            private set
            {
                if (_nextAppStoreDeviceDeregistrationDate == value)
                    return;
                _nextAppStoreDeviceDeregistrationDate = value;
                FirePropertyChanged(nameof(NextAppStoreDeviceDeregistrationDate));
            }
        }

        public bool CanShowDeviceList
        {
            get
            {
                if (_tunerHandler == null)
                    InitRegisteredTuners();
                return _tunerHandler.CanQueryTunerList();
            }
        }

        public void RemoveTuner(TunerInfo tunerInfo)
        {
            if (_tunerHandler == null)
                InitRegisteredTuners();
            _tunerHandler.DeregisterTuner(tunerInfo);
        }

        public void RefreshTunerList()
        {
            if (!_tunerHandler.CanQueryTunerList())
                return;
            _tunerHandler.RefreshTunerList();
        }

        private void OnTunerInfoChanged(object oSenderUNUSED, EventArgs eargs)
        {
            if (eargs != null && eargs.GetType() == typeof(EventArgsHR) && ((EventArgsHR)eargs).HResult == HRESULT._ZEST_E_TOO_MANY_DEREGISTRATIONS_WITHIN_MONTH)
                Application.DeferredInvoke(DisplayServiceErrorMessage, eargs);
            else
                Application.DeferredInvoke(UpdateRegisteredTunersList, DeferredInvokePriority.Normal);
        }

        private void DisplayServiceErrorMessage(object eargs) => Shell.ShowErrorDialog(((EventArgsHR)eargs).HResult, StringId.IDS_REGDEVICES_CANT_REMOVE);

        private void UpdateRegisteredTunersList(object argsUNUSED)
        {
            if (_registeredComputersModelList == null || _registeredDevicesModelList == null || _registeredAppStoreDevicesModelList == null)
                return;
            var count1 = _registeredComputersModelList.Count;
            var count2 = _registeredDevicesModelList.Count;
            var count3 = _registeredAppStoreDevicesModelList.Count;
            _registeredComputersModelList.Clear();
            _registeredDevicesModelList.Clear();
            _registeredAppStoreDevicesModelList.Clear();
            foreach (var pcs in _tunerHandler.GetPCsList())
                _registeredComputersModelList.Add(pcs);
            foreach (var devices in _tunerHandler.GetDevicesList())
                _registeredDevicesModelList.Add(devices);
            foreach (var appStoreDevices in _tunerHandler.GetAppStoreDevicesList())
                _registeredAppStoreDevicesModelList.Add(appStoreDevices);
            var deregistrationDate1 = _tunerHandler.GetNextPCDeregistrationDate();
            NextPCDeregistrationDate = !(deregistrationDate1 != DateTime.MinValue) || !(deregistrationDate1 > DateTime.Now) ? null : deregistrationDate1.AddDays(1.0).ToShortDateString();
            var deregistrationDate2 = _tunerHandler.GetNextSubscriptionDeviceDeregistrationDate();
            NextSubscriptionDeviceDeregistrationDate = !(deregistrationDate2 != DateTime.MinValue) || !(deregistrationDate2 > DateTime.Now) ? null : deregistrationDate2.AddDays(1.0).ToShortDateString();
            var deregistrationDate3 = _tunerHandler.GetNextAppStoreDeviceDeregistrationDate();
            NextAppStoreDeviceDeregistrationDate = !(deregistrationDate3 != DateTime.MinValue) || !(deregistrationDate3 > DateTime.Now) ? null : deregistrationDate3.AddDays(1.0).ToShortDateString();
            if (_registeredComputersModelList.Count >= count1 && _registeredDevicesModelList.Count >= count2 && _registeredAppStoreDevicesModelList.Count >= count3)
                return;
            SignIn.Instance.RefreshAccount();
        }

        private void InitRegisteredTuners()
        {
            _tunerHandler = TunerInfoHandlerFactory.CreateTunerInfoHandler();
            _tunerHandler.OnChanged += OnTunerInfoChanged;
            _registeredComputersModelList = new ArrayListDataSet();
            _registeredDevicesModelList = new ArrayListDataSet();
            _registeredAppStoreDevicesModelList = new ArrayListDataSet();
            _nextPCDeregistrationDate = null;
            _nextSubscriptionDeviceDeregistrationDate = null;
            _nextAppStoreDeviceDeregistrationDate = null;
            if (!_tunerHandler.CanQueryTunerList())
                return;
            _tunerHandler.RefreshTunerList();
        }

        public IntRangedValue SlideShowSpeed
        {
            get
            {
                if (_slideShowSpeed == null)
                {
                    _slideShowSpeed = new IntRangedValue(this);
                    _slideShowSpeed.MinValue = 3000;
                    _slideShowSpeed.MaxValue = 10000;
                    _slideShowSpeed.Step = 1000;
                    _slideShowSpeed.Value = ClientConfiguration.GeneralSettings.SlideShowSpeed;
                    _slideShowSpeed.PropertyChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnSlideShowSpeedCommit)] = null;
                }
                return _slideShowSpeed;
            }
        }

        private void OnSlideShowSpeedCommit(object data) => ClientConfiguration.GeneralSettings.SlideShowSpeed = _slideShowSpeed.Value;

        public HMESettings HME
        {
            get
            {
                if (_HME == null)
                {
                    lock (mylock)
                    {
                        if (_HME == null)
                        {
                            var hmeSettings = new HMESettings();
                            var num = ((HRESULT)hmeSettings.Init()).IsError ? 1 : 0;
                            _sharingAllDevicesEnabled = hmeSettings.GetAllDevicesEnabled();
                            _HME = hmeSettings;
                        }
                    }
                }
                return _HME;
            }
        }

        public bool UserCanModifySharing => Environment.OSVersion.Version.Major >= 6 || !SharingEnableRequiresElevation || SharingEnabled;

        public bool SharingEnableRequiresElevation => Environment.OSVersion.Version.Major < 6 ? HME.SharingEnableRequiresLoginAsAdmin : HME.SharingEnableRequiresElevation;

        public bool SharingEnabled => HME.SharingEnabled;

        private void SetSharingEnabledForAllMediaTypes(bool music, bool video, bool pictures)
        {
            HME.SetSharingEnabledForMediaType(EMediaTypes.eMediaTypeAudio, music);
            HME.SetSharingEnabledForMediaType(EMediaTypes.eMediaTypeVideo, video);
            HME.SetSharingEnabledForMediaType(EMediaTypes.eMediaTypeImage, pictures);
        }

        private void OnMediaSharingUpdate(object data)
        {
            if (HME == null)
                return;
            
            HRESULT hr;
            if (_sharingEnableMusic.Value || _sharingEnableVideo.Value || _sharingEnablePhoto.Value)
            {
                hr = HME.EnableSharingForUser();
                if (hr.IsSuccess)
                    SetSharingEnabledForAllMediaTypes(_sharingEnableMusic.Value, _sharingEnableVideo.Value, _sharingEnablePhoto.Value);
                else
                    SetSharingEnabledForAllMediaTypes(false, false, false);
            }
            else
            {
                hr = HME.DisableSharingForMachine();
                if (hr.IsSuccess)
                    hr = HME.DisableSharingForUser();
                if (hr.IsSuccess)
                    SetSharingEnabledForAllMediaTypes(false, false, false);
            }
            HME.SetAllDevicesEnabled(_sharingAllDevicesEnabled);
            if (!_sharingAllDevicesEnabled)
                HME.EnableDevice(_sharingDeviceIndex, true);
            HME.SetDisplayName(_sharingDisplayName);
        }

        public string SharingDisplayName
        {
            get
            {
                if (_sharingDisplayName == null)
                    HME.GetDisplayName(ref _sharingDisplayName);
                return _sharingDisplayName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    SharingError = Shell.LoadString(StringId.IDS_SHARE_NAME_EMPTY);
                }
                else
                {
                    if (_sharingDisplayName == value)
                        return;
                    SharingError = string.Empty;
                    _sharingDisplayName = value;
                    CommitList[new ProxySettingDelegate(OnMediaSharingUpdate)] = null;
                }
            }
        }

        public string SharingError
        {
            get => _sharingError;
            set
            {
                if (_sharingError == value)
                    return;
                _sharingError = value;
                FirePropertyChanged(nameof(SharingError));
            }
        }

        public BooleanChoice SharingEnableMusic
        {
            get
            {
                if (_sharingEnableMusic == null)
                {
                    _sharingEnableMusic = new BooleanChoice(this, Shell.LoadString(StringId.IDS_SHARE_MUSIC_CHECK));
                    _sharingEnableMusic.Value = HME.GetSharingEnabledForMediaType(EMediaTypes.eMediaTypeAudio);
                    _sharingEnableMusic.ChosenChanged += (sender, args) =>
                    {
                        CommitList[new ProxySettingDelegate(OnMediaSharingUpdate)] = null;
                        if (!SharingEnableRequiresElevation)
                            return;
                        ChangeRequiresElevation = true;
                    };
                }
                return _sharingEnableMusic;
            }
        }

        public BooleanChoice SharingEnableVideo
        {
            get
            {
                if (_sharingEnableVideo == null)
                {
                    _sharingEnableVideo = new BooleanChoice(this, Shell.LoadString(StringId.IDS_SHARE_VIDEOS_CHECK));
                    _sharingEnableVideo.Value = HME.GetSharingEnabledForMediaType(EMediaTypes.eMediaTypeVideo);
                    _sharingEnableVideo.ChosenChanged += (sender, args) =>
                    {
                        CommitList[new ProxySettingDelegate(OnMediaSharingUpdate)] = null;
                        if (!SharingEnableRequiresElevation)
                            return;
                        ChangeRequiresElevation = true;
                    };
                }
                return _sharingEnableVideo;
            }
        }

        public BooleanChoice SharingEnablePhoto
        {
            get
            {
                if (_sharingEnablePhoto == null)
                {
                    _sharingEnablePhoto = new BooleanChoice(this, Shell.LoadString(StringId.IDS_SHARE_PICTURES_CHECK));
                    _sharingEnablePhoto.Value = HME.GetSharingEnabledForMediaType(EMediaTypes.eMediaTypeImage);
                    _sharingEnablePhoto.ChosenChanged += (sender, args) =>
                    {
                        CommitList[new ProxySettingDelegate(OnMediaSharingUpdate)] = null;
                        if (!SharingEnableRequiresElevation)
                            return;
                        ChangeRequiresElevation = true;
                    };
                }
                return _sharingEnablePhoto;
            }
        }

        public Choice SharingSelectDeviceChoice
        {
            get
            {
                if (_sharingSelectDeviceChoice == null)
                {
                    _sharingSelectDeviceOptions = new List<Command>();
                    var command1 = new Command(this, Shell.LoadString(StringId.IDS_GLOBAL_SHARING_OPTION), null);
                    command1.Data.Add("value", true);
                    _sharingSelectDeviceOptions.Add(command1);
                    var command2 = new Command(this, Shell.LoadString(StringId.IDS_SELECTIVE_SHARING_OPTION), null);
                    command2.Data.Add("value", false);
                    command2.Available = SharingEnabled;
                    _sharingSelectDeviceOptions.Add(command2);
                    _sharingSelectDeviceChoice = new Choice(this);
                    _sharingSelectDeviceChoice.Options = (IList)_sharingSelectDeviceOptions;
                    _sharingSelectDeviceChoice.ChosenChanged += (sender, args) => SharingAllDevicesEnabled = (bool)_sharingSelectDeviceOptions[((Choice)sender).ChosenIndex].Data["value"];
                }
                return _sharingSelectDeviceChoice;
            }
        }

        public bool SharingAllDevicesEnabled
        {
            get => _sharingAllDevicesEnabled;
            set
            {
                if (_sharingAllDevicesEnabled == value)
                    return;
                _sharingAllDevicesEnabled = value;
                CommitList[new ProxySettingDelegate(OnMediaSharingUpdate)] = null;
                FirePropertyChanged(nameof(SharingAllDevicesEnabled));
            }
        }

        public IList SharingDeviceList
        {
            get
            {
                _sharingDeviceList ??= CreateSharingDeviceList();
                return (IList)_sharingDeviceList;
            }
        }

        private IList<BooleanInputChoice> CreateSharingDeviceList()
        {
            IList<BooleanInputChoice> booleanInputChoiceList = new List<BooleanInputChoice>();
            var deviceCount = HME.GetDeviceCount();
            var strName = "";
            var strMAC = "";
            var strSerialNumber = "";
            for (uint dwIndex = 0; dwIndex < deviceCount; ++dwIndex)
            {
                HME.GetDeviceProps(dwIndex, ref strName, ref strMAC, ref strSerialNumber);
                var booleanInputChoice = new BooleanInputChoice(this, deviceCount > 1U ? string.Format(Shell.LoadString(StringId.IDS_XBOX360_NAME_AND_SERIAL_NUMBER), strName, strSerialNumber) : strName, true);
                booleanInputChoice.Data["index"] = dwIndex;
                booleanInputChoice.Value = HME.GetDeviceEnabled(dwIndex);
                booleanInputChoice.ChosenChanged += HandleSharingDeviceListValueChanged;
                booleanInputChoiceList.Add(booleanInputChoice);
            }
            if (!_nssDeviceListChangeEventAdded)
            {
                HME.NSSDeviceListChangeEvent += HandleNSSDeviceListChangeEvent;
                _nssDeviceListChangeEventAdded = true;
            }
            return booleanInputChoiceList;
        }

        public void RemoveNSSDeviceListChangeEvent()
        {
            if (!_nssDeviceListChangeEventAdded)
                return;
            HME.NSSDeviceListChangeEvent -= HandleNSSDeviceListChangeEvent;
            _nssDeviceListChangeEventAdded = false;
            _sharingDeviceList = null;
        }

        private void HandleNSSDeviceListChangeEvent() => Application.DeferredInvoke(delegate
        {
            if (_sharingDeviceList == null)
                return;
            _sharingDeviceList = CreateSharingDeviceList();
            FirePropertyChanged("SharingDeviceList");
        }, null);

        private void HandleSharingDeviceListValueChanged(object sender, EventArgs args) => _sharingDeviceIndex = (uint)((ModelItem)sender).Data["index"];

        public bool ReevaluateVideoSettings
        {
            get => ClientConfiguration.GeneralSettings.ReevaluateVideoSettings;
            set
            {
                ClientConfiguration.GeneralSettings.ReevaluateVideoSettings = value;
                FirePropertyChanged(nameof(ReevaluateVideoSettings));
            }
        }

        public RenderingType RequestedRenderingType
        {
            get
            {
                var renderingType = (RenderingType)ClientConfiguration.GeneralSettings.RenderingType;
                if (renderingType == RenderingType.Default)
                    renderingType = Application.RenderingType;
                return renderingType;
            }
            set
            {
                ClientConfiguration.GeneralSettings.RenderingType = (int)value;
                ReevaluateVideoSettings = Application.RenderingType != RenderingType.DX9 && value == RenderingType.DX9;
                FirePropertyChanged(nameof(RequestedRenderingType));
            }
        }

        private void ReevaluateVideoAcceleration(object sender, EventArgs args) => RequestedRenderingType = RenderingType.DX9;

        public RenderingQuality RequestedRenderingQuality
        {
            get => (RenderingQuality)ClientConfiguration.GeneralSettings.RenderingQuality;
            set
            {
                ClientConfiguration.GeneralSettings.RenderingQuality = (int)value;
                FirePropertyChanged(nameof(RequestedRenderingQuality));
            }
        }

        public bool AnimationsEnabled
        {
            get => ClientConfiguration.GeneralSettings.AnimationsEnabled;
            set
            {
                ClientConfiguration.GeneralSettings.AnimationsEnabled = value;
                FirePropertyChanged(nameof(AnimationsEnabled));
            }
        }

        public Choice ScreenGraphicsSlider
        {
            get
            {
                if (_screenGraphicsSlider == null)
                {
                    var choice = new Choice(this);
                    choice.Options = NamedIntOption.ScreenGraphicsOptions;
                    var screenGraphics = Application.RenderingType != RenderingType.GDI
                        ? Application.RenderingQuality != RenderingQuality.MaxQuality
                            ? !Application.AnimationsEnabled
                                ? ScreenGraphics.Advanced
                                : ScreenGraphics.AdvancedWithAnimation
                            : ScreenGraphics.Premium
                        : ScreenGraphics.Basic;
                    NamedIntOption.SelectOptionByValue(choice, (int)screenGraphics);
                    _screenGraphicsSlider = choice;
                    choice.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnScreenGraphicsSliderCommit)] = null;
                }
                return _screenGraphicsSlider;
            }
        }

        private void OnScreenGraphicsSliderCommit(object data)
        {
            var screenGraphics = (ScreenGraphics)((NamedIntOption)_screenGraphicsSlider.ChosenValue).Value;
            var requestedRenderingType = RequestedRenderingType;
            switch (screenGraphics)
            {
                case ScreenGraphics.Basic:
                    RequestedRenderingType = RenderingType.GDI;
                    RequestedRenderingQuality = RenderingQuality.MinQuality;
                    AnimationsEnabled = false;
                    break;
                case ScreenGraphics.Advanced:
                    RequestedRenderingType = RenderingType.DX9;
                    RequestedRenderingQuality = RenderingQuality.MinQuality;
                    AnimationsEnabled = false;
                    break;
                case ScreenGraphics.AdvancedWithAnimation:
                    RequestedRenderingType = RenderingType.DX9;
                    RequestedRenderingQuality = RenderingQuality.MinQuality;
                    AnimationsEnabled = true;
                    break;
                case ScreenGraphics.Premium:
                    RequestedRenderingType = RenderingType.DX9;
                    RequestedRenderingQuality = RenderingQuality.MaxQuality;
                    AnimationsEnabled = true;
                    break;
            }
            if (requestedRenderingType == RenderingType.GDI && RequestedRenderingType == RenderingType.DX9)
                MessageBox.Show(Shell.LoadString(StringId.IDS_SCREEN_GRAPHICS_TITLE), Shell.LoadString(StringId.IDS_ACCELERATION_PROMPT_TEXT), null);
            else
                MessageBox.Show(Shell.LoadString(StringId.IDS_SCREEN_GRAPHICS_TITLE), Shell.LoadString(StringId.IDS_ACCELERATION_RESTART_TEXT), null);
        }

        public string BackgroundImage
        {
            get => _backgroundImage ??= ClientConfiguration.Shell.BackgroundImage;
            set
            {
                if (_backgroundImage == value)
                    return;
                CommitList[new ProxySettingDelegate(OnBackgroundImageCommit)] = null;
                _backgroundImage = value;
                FirePropertyChanged(nameof(BackgroundImage));
            }
        }

        public WindowColor BackgroundColor
        {
            get => _backgroundColor;
            set => _backgroundColor = value;
        }

        private void OnBackgroundImageCommit(object data)
        {
            ClientConfiguration.Shell.BackgroundImage = _backgroundImage;
            ClientConfiguration.Shell.BackgroundColor = Shell.WindowColorToRGB(_backgroundColor);
            ((Shell)ZuneShell.DefaultInstance).BackgroundImage = _backgroundImage;
            Application.Window.SetBackgroundColor(_backgroundColor);
        }

        public BooleanChoice ShowNowPlayingBackgroundOnIdle
        {
            get
            {
                if (_showNowPlayingBackgroundOnIdle == null)
                {
                    _showNowPlayingBackgroundOnIdle = new BooleanChoice(this, Shell.LoadString(StringId.IDS_SHOW_NOWPLAYING_ON_IDLE_DESCRIPTION));
                    _showNowPlayingBackgroundOnIdle.Value = ClientConfiguration.Shell.ShowNowPlayingBackgroundOnIdleTimeout > 0;
                    _showNowPlayingBackgroundOnIdle.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnShowNowPlayingBackgroundOnIdleCommit)] = null;
                }
                return _showNowPlayingBackgroundOnIdle;
            }
        }

        private void OnShowNowPlayingBackgroundOnIdleCommit(object data)
        {
            var num = _showNowPlayingBackgroundOnIdle.Value ? 90 : 0;
            ClientConfiguration.Shell.ShowNowPlayingBackgroundOnIdleTimeout = num;
            ((Shell)ZuneShell.DefaultInstance).ShowNowPlayingBackgroundOnIdleTimeout = num;
        }

        public BooleanChoice PlaySounds
        {
            get
            {
                if (_playSounds == null)
                {
                    _playSounds = new BooleanChoice(this, Shell.LoadString(StringId.IDS_SOUNDS_DESCRIPTION));
                    _playSounds.Value = ClientConfiguration.Shell.Sounds;
                    _playSounds.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnPlaySoundsCommit)] = null;
                }
                return _playSounds;
            }
        }

        private void OnPlaySoundsCommit(object data)
        {
            ClientConfiguration.Shell.Sounds = _playSounds.Value;
            ((Shell)ZuneShell.DefaultInstance).PlaySounds = _playSounds.Value;
        }

        public BooleanChoice CompactModeAlwaysOnTop
        {
            get
            {
                if (_compactModeAlwaysOnTop == null)
                {
                    _compactModeAlwaysOnTop = new BooleanChoice(this, Shell.LoadString(StringId.IDS_COMPACT_MODE_ALWAYS_ON_TOP));
                    _compactModeAlwaysOnTop.Value = ClientConfiguration.GeneralSettings.CompactModeAlwaysOnTop;
                    _compactModeAlwaysOnTop.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnCompactModeAlwaysOnTopCommit)] = null;
                }
                return _compactModeAlwaysOnTop;
            }
        }

        private void OnCompactModeAlwaysOnTopCommit(object data)
        {
            ClientConfiguration.GeneralSettings.CompactModeAlwaysOnTop = _compactModeAlwaysOnTop.Value;
            ((Shell)ZuneShell.DefaultInstance).CompactModeAlwaysOnTop = _compactModeAlwaysOnTop.Value;
            SQMLog.Log(SQMDataId.CompactModeOnTopSetting, 1);
        }

        public BooleanChoice RatingsChoice
        {
            get
            {
                if (_ratingsChoice == null)
                {
                    var commandArray = new Command[2]
                    {
                        new RichLayoutCommand(this, Shell.LoadString(StringId.IDS_COMMON_RATINGS_ALL_USERS_OPTION), true),
                        new RichLayoutCommand( this, Shell.LoadString(StringId.IDS_PERSONAL_RATINGS_EACH_USER_OPTION), false)
                    };
                    _ratingsChoice = new BooleanChoice(this);
                    _ratingsChoice.Options = commandArray;
                    _ratingsChoice.Value = !ClientConfiguration.MediaStore.SharedUserRatings;
                    _ratingsChoice.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnRatingsCommit)] = null;
                }
                return _ratingsChoice;
            }
        }

        private void OnRatingsCommit(object data)
        {
            ClientConfiguration.MediaStore.SharedUserRatings = !_ratingsChoice.Value;
            if (!_applyRatingsChoice.Value || _ratingsChoice.Value)
                return;
            ZuneLibrary.ExportUserRatings(SignIn.Instance.LastSignedInUserId, EMediaTypes.eMediaTypeAudio);
        }

        public BooleanChoice ApplyRatingsChoice
        {
            get
            {
                if (_applyRatingsChoice == null)
                {
                    _applyRatingsChoice = new BooleanChoice(this, Shell.LoadString(StringId.IDS_APPLY_RATINGS_DIALOG_DESCRIPTION));
                    _applyRatingsChoice.Value = false;
                    _applyRatingsChoice.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnRatingsCommit)] = null;
                }
                return _applyRatingsChoice;
            }
        }

        public Choice StartupPageChoice
        {
            get
            {
                if (_startupPageChoice == null)
                {
                    var commandList = new List<Command>();
                    if (FeatureEnablement.IsFeatureEnabled(Features.eQuickplay))
                        commandList.Add(new NamedStringOption(Shell.LoadString(StringId.IDS_VIEW_STARTUPPAGE_QUICKPLAY_CHOICE), Shell.MainFrame.Quickplay.DefaultUIPath));
                    commandList.Add(new NamedStringOption(Shell.LoadString(StringId.IDS_VIEW_STARTUPPAGE_COLLECTION_CHOICE), Shell.MainFrame.Collection.DefaultUIPath));
                    if (FeatureEnablement.IsFeatureEnabled(Features.eMarketplace))
                        commandList.Add(new NamedStringOption(Shell.LoadString(StringId.IDS_VIEW_STARTUPPAGE_MARKETPLACE_CHOICE), Shell.MainFrame.Marketplace.DefaultUIPath));
                    if (FeatureEnablement.IsFeatureEnabled(Features.eSocial))
                        commandList.Add(new NamedStringOption(Shell.LoadString(StringId.IDS_VIEW_STARTUPPAGE_SOCIAL_CHOICE), Shell.MainFrame.Social.DefaultUIPath));
                    _startupPageChoice = new Choice(this);
                    _startupPageChoice.Options = commandList;
                    foreach (var command in commandList)
                    {
                        var namedStringOption = (NamedStringOption)command;
                        if (namedStringOption.Value != ClientConfiguration.Shell.StartupPage)
                            continue;
                        
                        _startupPageChoice.ChosenValue = namedStringOption;
                        break;
                    }
                    _startupPageChoice.ChosenChanged += (sender, args) => CommitList[new ProxySettingDelegate(OnStartupPageCommit)] = null;
                }
                return _startupPageChoice;
            }
        }

        private void OnStartupPageCommit(object data)
        {
            ClientConfiguration.Shell.StartupPage = ((NamedStringOption)_startupPageChoice.ChosenValue).Value;
            ClientConfiguration.Quickplay.CheckUseCount = false;
        }

        public bool AutoLaunchZuneOnConnect
        {
            get => _autoLaunchZuneOnConnect;
            set
            {
                if (_autoLaunchZuneOnConnect == value)
                    return;
                _autoLaunchZuneOnConnect = value;
                CommitList[new ProxySettingDelegate(OnAutoLaunchZuneOnConnectCommit)] = null;
                FirePropertyChanged(nameof(AutoLaunchZuneOnConnect));
            }
        }

        private void OnAutoLaunchZuneOnConnectCommit(object data) => ClientConfiguration.Devices.AutoLaunchZuneOnConnect = _autoLaunchZuneOnConnect;

        private struct MonitoredFolder
        {
            public readonly string Path;
            public readonly EMediaTypes Schema;

            public MonitoredFolder(string path, EMediaTypes schema)
            {
                Path = path;
                Schema = schema;
            }
        }
    }
}
