// Decompiled with JetBrains decompiler
// Type: ZuneUI.SyncControls
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UIXControls;

namespace ZuneUI
{
    public class SyncControls : ModelItem
    {
        private UIDeviceList _deviceList;
        private UIDevice _currentDevice;
        private UIDevice _deferredArrivalDevice;
        private string _canonicalNameToMakeActive;
        private bool _changeIntoSetupDevice;
        private Command _deviceSignInFailureEvent;
        private Queue<UIDevice> _failedSignInQueue;
        private DownloadTaskList _downloadTasks;
        private bool _haveSeenDownloadsOngoing;
        private MessageBox _currentUnlinkedDeviceDialog;
        private bool _showSyncInstructionsToast;
        private bool _displayWirelessSyncBanner;
        private static string _sizeInKB = Shell.LoadString(StringId.IDS_GAS_GAUGE_SIZE_IN_KB);
        private static string _sizeInMB = Shell.LoadString(StringId.IDS_GAS_GAUGE_SIZE_IN_MB);
        private static string _sizeInGB = Shell.LoadString(StringId.IDS_GAS_GAUGE_SIZE_IN_GB);
        private static long _devicelandKilobyte = 1024;
        private static long _devicelandMegabyte = DevicelandKilobyte * DevicelandKilobyte;
        private static long _devicelandGigabyte = DevicelandMegabyte * DevicelandKilobyte;
        private static SyncControls s_singletonInstance;
        private static string _unlinkedDeviceNagDialogTitle = Shell.LoadString(StringId.IDS_DIALOG_TITLE_LINK_TO_SYNC_FRIENDS);
        private static string _unlinkedDeviceNagDialogMessageBase = Shell.LoadString(StringId.IDS_DIALOG_TEXT_LINK_TO_SYNC_FRIENDS);

        private SyncControls(IModelItemOwner owner)
          : base(owner)
        {
            this._deviceList = SingletonModelItem<UIDeviceList>.Instance;
            this._deviceList.DeviceAddedEvent += new DeviceListEventHandler(this.OnDeviceAdded);
            this._deviceList.DeviceRemovedEvent += new DeviceListEventHandler(this.OnDeviceRemoved);
            this._deviceList.DeviceConnectedEvent += new DeviceListEventHandler(this.OnDeviceConnected);
            this._deviceList.DeviceDisconnectedEvent += new DeviceListEventHandler(this.OnDeviceDisconnected);
            this._currentDevice = UIDeviceList.NullDevice;
            SignIn.Instance.PropertyChanged += new PropertyChangedEventHandler(this.SignInPropertyChanged);
            AccountCreationWizard.CreationCompleted += new EventHandler(this.AccountCreationFinished);
            DeviceManagement.DeviceConnectionHandled += new DeviceConnectionHandledEventHandler(this.OnDeviceConnectionHandled);
            this.UpdateWirelessSyncBannerDisplay();
        }

        protected override void OnDispose(bool fDisposing)
        {
            if (fDisposing)
            {
                if (this._deviceList != null)
                {
                    this._deviceList.DeviceAddedEvent -= new DeviceListEventHandler(this.OnDeviceAdded);
                    this._deviceList.DeviceRemovedEvent -= new DeviceListEventHandler(this.OnDeviceRemoved);
                    this._deviceList.DeviceConnectedEvent -= new DeviceListEventHandler(this.OnDeviceConnected);
                    this._deviceList.DeviceDisconnectedEvent -= new DeviceListEventHandler(this.OnDeviceDisconnected);
                    this._deviceList = null;
                }
                if (this._downloadTasks != null)
                {
                    this._downloadTasks.ActiveDownloads.PropertyChanged -= new PropertyChangedEventHandler(this.OnDownloadPropertyChanged);
                    this._downloadTasks = null;
                }
                SignIn.Instance.PropertyChanged -= new PropertyChangedEventHandler(this.SignInPropertyChanged);
                AccountCreationWizard.CreationCompleted -= new EventHandler(this.AccountCreationFinished);
                DeviceManagement.DeviceConnectionHandled -= new DeviceConnectionHandledEventHandler(this.OnDeviceConnectionHandled);
            }
            base.OnDispose(fDisposing);
        }

        public bool ShowPhoneWelcomeMessage
        {
            get => ClientConfiguration.FUE.ShowPhoneFUEDeviceLandOptions;
            set
            {
                if (ClientConfiguration.FUE.ShowPhoneFUEDeviceLandOptions == value)
                    return;
                ClientConfiguration.FUE.ShowPhoneFUEDeviceLandOptions = value;
                this.FirePropertyChanged(nameof(ShowPhoneWelcomeMessage));
            }
        }

        public bool ShowSyncInstructionsToast
        {
            get => this._showSyncInstructionsToast;
            set
            {
                if (this._showSyncInstructionsToast == value)
                    return;
                if (this.CurrentDevice.Class == DeviceClass.Classic || this.CurrentDevice.Class == DeviceClass.ZuneHD)
                {
                    bool flag = value && ClientConfiguration.Devices.ShowSyncInstructionsToast;
                    value = flag;
                    ClientConfiguration.Devices.ShowSyncInstructionsToast = flag;
                }
                this._showSyncInstructionsToast = value;
                this.FirePropertyChanged(nameof(ShowSyncInstructionsToast));
            }
        }

        public bool DisplayWirelessSyncBanner
        {
            get => this._displayWirelessSyncBanner && !this.CurrentDevice.IsWirelessSyncEnabled;
            set
            {
                if (this._displayWirelessSyncBanner == value)
                    return;
                this._displayWirelessSyncBanner = value;
                this.FirePropertyChanged(nameof(DisplayWirelessSyncBanner));
            }
        }

        public UIDevice CurrentDevice
        {
            get => this._currentDevice ?? UIDeviceList.NullDevice;
            private set
            {
                if (this._currentDevice == value)
                    return;
                this._currentDevice = value ?? UIDeviceList.NullDevice;
                PhoneBrandingStringMap.Instance.BrandingEnabled = this._currentDevice != null && this._currentDevice.SupportsBrandingType(DeviceBranding.WindowsPhone);
                KinBrandingStringMap.Instance.BrandingEnabled = this._currentDevice != null && this._currentDevice.SupportsBrandingType(DeviceBranding.Kin);
                this.FirePropertyChanged(nameof(CurrentDevice));
            }
        }

        public UIDevice CurrentDeviceOverride => DeviceManagement.SetupDevice ?? this.CurrentDevice;

        public bool ChangeIntoSetupDevice
        {
            get => this._changeIntoSetupDevice;
            set
            {
                if (this._changeIntoSetupDevice == value)
                    return;
                this._changeIntoSetupDevice = value;
                this.FirePropertyChanged(nameof(ChangeIntoSetupDevice));
            }
        }

        public Command DeviceSignInFailureEvent
        {
            get
            {
                if (this._deviceSignInFailureEvent == null)
                    this._deviceSignInFailureEvent = new Command(this);
                return this._deviceSignInFailureEvent;
            }
        }

        public Queue<UIDevice> FailedSignInDevices
        {
            get
            {
                if (this._failedSignInQueue == null)
                    this._failedSignInQueue = new Queue<UIDevice>();
                return this._failedSignInQueue;
            }
        }

        public UIDevice CurrentFailedSignInDevice
        {
            get
            {
                try
                {
                    return this.FailedSignInDevices.Peek();
                }
                catch (InvalidOperationException ex)
                {
                    return UIDeviceList.NullDevice;
                }
            }
        }

        public void SetCurrentDevice(UIDevice device)
        {
            if (device == null)
                device = UIDeviceList.NullDevice;
            if (this._canonicalNameToMakeActive != null && device.CanonicalName != null && device.CanonicalName.StartsWith(this._canonicalNameToMakeActive))
            {
                this._canonicalNameToMakeActive = null;
                if ((!(ZuneShell.DefaultInstance.CurrentPage is QuickplayPage) || !ClientConfiguration.FUE.ShowArtistChooser) && (ZuneShell.DefaultInstance.Management.CurrentCategoryPage == null || !ZuneShell.DefaultInstance.Management.CurrentCategoryPage.IsWizard))
                    ZuneShell.DefaultInstance.NavigateToPage(new Deviceland());
            }
            if (device == this.CurrentDevice)
                return;
            Management management = null;
            CategoryPage categoryPage = null;
            if (ZuneShell.DefaultInstance != null)
                management = ZuneShell.DefaultInstance.Management;
            if (management != null)
                categoryPage = management.CurrentCategoryPage;
            categoryPage?.RestartSyncIfNecessary();
            this.CurrentDevice = device;
            ClientConfiguration.Devices.CurrentDeviceID = this.CurrentDevice.ID;
            categoryPage?.PauseSyncIfNecessary();
            management?.DisposeDeviceManagement(false);
        }

        public void Phase3Init()
        {
            this._downloadTasks = DownloadTaskList.Instance;
            this._downloadTasks.ActiveDownloads.PropertyChanged += new PropertyChangedEventHandler(this.OnDownloadPropertyChanged);
            this._haveSeenDownloadsOngoing = this._downloadTasks.ActiveDownloads.Count > 0;
        }

        public void SetCurrentDeviceByCanonicalName(string canonicalName)
        {
            this._canonicalNameToMakeActive = canonicalName.ToLower();
            if (this._deviceList == null)
                return;
            foreach (UIDevice device in this._deviceList)
            {
                if (device.IsConnectedToClient && device.CanonicalName.StartsWith(this._canonicalNameToMakeActive))
                {
                    this.SetCurrentDevice(device);
                    break;
                }
            }
        }

        public void SetCurrentDeviceIfNecessary(UIDevice device, bool comingOutOfFirstConnect)
        {
            if (!device.IsConnectedToClientPhysically)
                return;
            this.SetCurrentDevice(device);
            if (comingOutOfFirstConnect)
                return;
            this.ShowUnlinkedDeviceNagDialogIfNecessary();
        }

        public UIDevice FindNewActiveDevice() => this.FindNewActiveDevice(null);

        public UIDevice FindNewActiveDevice(UIDevice excludedDevice)
        {
            foreach (UIDevice device in this._deviceList)
            {
                if (device != excludedDevice)
                    return device;
            }
            return null;
        }

        public void DeleteCurrentDevice()
        {
            if (!this.CurrentDevice.IsValid)
                return;
            MessageBox.Show(Shell.LoadString(StringId.IDS_DELETE_DIALOG_TITLE), Shell.LoadString(StringId.IDS_DELETE_DIALOG_TEXT), new EventHandler(this.ConfirmedDeleteDevice));
        }

        private void ConfirmedDeleteDevice(object sender, EventArgs e)
        {
            if (this.CurrentDevice.IsGuest || !this.CurrentDevice.SupportsWirelessSetupMethod1 && !this.CurrentDevice.SupportsWirelessSetupMethod2)
                this.DeleteCurrentDeviceWorker();
            else
                WirelessSync.Instance.ClearWirelessOnDeviceForForget();
        }

        public void DeleteCurrentDeviceWorker()
        {
            if (this.CurrentDevice == null)
                return;
            string message = this.CurrentDevice.SupportsBrandingType(DeviceBranding.WindowsPhone) ? Shell.LoadString(StringId.IDS_PHONE_FORGET_DEVICE_DIALOG_CONTENT) : Shell.LoadString(StringId.IDS_FORGET_DEVICE_DIALOG_CONTENT);
            ZuneShell.DefaultInstance.Management.CommitList.RemoveByIntValue(this.CurrentDevice.ID);
            HRESULT hresult = this._deviceList.DeleteDevice(this.CurrentDevice);
            if (hresult.IsError)
                Shell.ShowErrorDialog(hresult.Int, StringId.IDS_DELETE_DEVICE_FAILED);
            else
                MessageBox.Show(Shell.LoadString(StringId.IDS_FORGET_DEVICE_DIALOG_TITLE), message, null);
        }

        public void PromptForAccountLinkage() => this.ShowUnlinkedDeviceNagDialog(false);

        public void HideWirelessSyncBanner()
        {
            ClientConfiguration.Devices.ConnectionsUntilWirelessSyncBannerDisplay = -1;
            this.UpdateWirelessSyncBannerDisplay();
        }

        public void AddDeviceToFailedSignInQueue(UIDevice device)
        {
            device.IsLockedAgainstSyncing = true;
            this.FailedSignInDevices.Enqueue(device);
            if (this.FailedSignInDevices.Count != 1)
                return;
            this.ShowFailedSignInMessageBox();
        }

        public void HandleFailedSignInDevice(string username, string password)
        {
            this.CurrentFailedSignInDevice.SendMarketplaceCredentials(username, password);
            this.IgnoreFailedSignInDevice();
        }

        public void IgnoreFailedSignInDevice()
        {
            try
            {
                UIDevice uiDevice = this.FailedSignInDevices.Dequeue();
                uiDevice.IsLockedAgainstSyncing = false;
                uiDevice.BeginSync();
            }
            catch (InvalidOperationException ex)
            {
            }
            if (this.FailedSignInDevices.Count <= 0)
                return;
            this.ShowFailedSignInMessageBox();
        }

        public void HandleFailedSignInDeviceGuidMismatch()
        {
            string message = string.IsNullOrEmpty(this.CurrentFailedSignInDevice.ZuneTag) ? string.Format(Shell.LoadString(StringId.IDS_DEVICE_CREDS_GUID_MISMATCH_TEXT_UNKNONW_ZUNETAG), CurrentFailedSignInDevice.Name, SignIn.Instance.ZuneTag) : string.Format(Shell.LoadString(StringId.IDS_DEVICE_CREDS_GUID_MISMATCH_TEXT), CurrentFailedSignInDevice.Name, CurrentFailedSignInDevice.ZuneTag, SignIn.Instance.ZuneTag);
            MessageBox.Show(Shell.LoadString(StringId.IDS_DEVICE_CREDS_GUID_MISMATCH_TITLE), message, new Command(this, Shell.LoadString(StringId.IDS_ENTER_CREDENTIALS), delegate
          {
              this.DeviceSignInFailureEvent.Invoke();
          }), null, delegate
    {
        this.IgnoreFailedSignInDevice();
    }, true);
        }

        private void ShowFailedSignInMessageBox()
        {
            string message = string.IsNullOrEmpty(this.CurrentFailedSignInDevice.ZuneTag) ? string.Format(Shell.LoadString(StringId.IDS_DEVICE_SIGN_IN_FAILURE_TEXT_UNKNONW_ZUNETAG), CurrentFailedSignInDevice.Name) : string.Format(Shell.LoadString(StringId.IDS_DEVICE_SIGN_IN_FAILURE_TEXT), CurrentFailedSignInDevice.Name, CurrentFailedSignInDevice.ZuneTag);
            MessageBox.Show(Shell.LoadString(StringId.IDS_DEVICE_SIGN_IN_FAILURE_TITLE), message, new Command(this, Shell.LoadString(StringId.IDS_ENTER_CREDENTIALS), delegate
          {
              this.DeviceSignInFailureEvent.Invoke();
          }), null, delegate
    {
        this.IgnoreFailedSignInDevice();
    }, true);
        }

        private void OnFUECompleted(object sender, EventArgs args)
        {
            Fue.FUECompleted -= new EventHandler(this.OnFUECompleted);
            if (this._deferredArrivalDevice != null)
                this.OnDeviceConnected(null, new DeviceListEventArgs(this._deferredArrivalDevice));
            this._deferredArrivalDevice = null;
        }

        private void OnDeviceConnected(object sender, DeviceListEventArgs args)
        {
            if (args.Device.SupportsBrandingType(DeviceBranding.WindowsPhone) && ZuneApplication.IsDesktopLocked && !args.Device.IsConnectedToClientWirelessly)
                SingletonModelItem<UIDeviceList>.Instance.HideDevice(args.Device);
            else if (Fue.Instance.IsFirstLaunch)
            {
                if (ZuneApplication.InstallContext != SetupInstallContext.Zune && args.Device.SupportsBrandingType(DeviceBranding.WindowsPhone) && !args.Device.RequiresAutoRestore)
                    return;
                this._deferredArrivalDevice = args.Device;
                Fue.FUECompleted += new EventHandler(this.OnFUECompleted);
            }
            else
            {
                DeviceManagement.SetupQueue[args.Device.ID] = args.Device;
                if (ClientConfiguration.Devices.ConnectionsUntilWirelessSyncBannerDisplay > 0)
                    --ClientConfiguration.Devices.ConnectionsUntilWirelessSyncBannerDisplay;
                this.UpdateWirelessSyncBannerDisplay();
            }
        }

        private void OnDeviceDisconnected(object sender, DeviceListEventArgs args)
        {
            if (this._deferredArrivalDevice == args.Device)
            {
                this._deferredArrivalDevice = null;
                Fue.FUECompleted -= new EventHandler(this.OnFUECompleted);
            }
            else
            {
                if (args.Device == this.CurrentDevice)
                {
                    ZuneShell.DefaultInstance.Management.DisposeDeviceManagement(false);
                    this.HideUnlinkedDeviceNagDialog();
                    foreach (UIDevice device in this._deviceList)
                    {
                        if (device != args.Device && device.IsConnectedToClientPhysically)
                        {
                            this.SetCurrentDevice(device);
                            break;
                        }
                    }
                }
                if (DeviceManagement.SetupDevice != args.Device)
                    return;
                ZuneShell.DefaultInstance.Management.AlertedDeviceCategory = null;
            }
        }

        private void OnDeviceAdded(object sender, DeviceListEventArgs args)
        {
            if (args.Device.ID != ClientConfiguration.Devices.CurrentDeviceID && ClientConfiguration.Devices.CurrentDeviceID != 0 && (this.CurrentDevice.IsConnectedToClient || !args.Device.IsConnectedToClient))
                return;
            this.SetCurrentDevice(args.Device);
        }

        private void OnDeviceRemoved(object sender, DeviceListEventArgs args)
        {
            if (args.Device != this.CurrentDevice)
                return;
            this.SetCurrentDevice(this.FindNewActiveDevice(this.CurrentDevice));
        }

        private void OnDeviceConnectionHandled(object sender, DeviceConnectionHandledEventArgs args)
        {
            if (!args.IsFirstConnect)
                return;
            ClientConfiguration.Devices.ConnectionsUntilWirelessSyncBannerDisplay = 1;
            this.UpdateWirelessSyncBannerDisplay();
        }

        private void UpdateWirelessSyncBannerDisplay() => this.DisplayWirelessSyncBanner = ClientConfiguration.Devices.ConnectionsUntilWirelessSyncBannerDisplay == 0;

        private void OnDownloadPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "Count"))
                return;
            if (this._downloadTasks.ActiveDownloads.Count > 0)
            {
                this._haveSeenDownloadsOngoing = true;
            }
            else
            {
                if (!this._haveSeenDownloadsOngoing)
                    return;
                foreach (UIDevice device in this._deviceList)
                {
                    if (device.IsConnectedToClient)
                        device.BeginSync(true, true);
                }
                this._haveSeenDownloadsOngoing = false;
            }
        }

        private void SignInPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (!(args.PropertyName == "SignedIn") || !SignIn.Instance.SignedIn)
                return;
            this.ShowUnlinkedDeviceNagDialogIfNecessary();
        }

        private void AccountCreationFinished(object sender, EventArgs args) => this.ShowUnlinkedDeviceNagDialogIfNecessary();

        private void ShowUnlinkedDeviceNagDialogIfNecessary()
        {
            if (!this.CurrentDevice.PromptForAccountLinkage || this.CurrentDevice.IsGuest || (this.CurrentDevice.UserId != 0 || !this.CurrentDevice.IsConnectedToClientPhysically) || (!this.CurrentDevice.SupportsZuneTagLinking || !SignIn.Instance.SignedIn || (AccountCreationWizard.AccountCreationInProgress || ZuneShell.DefaultInstance.CurrentPage is DialogPage)))
                return;
            this.ShowUnlinkedDeviceNagDialog(true);
        }

        private void ShowUnlinkedDeviceNagDialog(bool allowUserToOptOut)
        {
            if (!SettingsExperience.ShouldShowDeviceMarketplaceCategory)
                return;
            this.HideUnlinkedDeviceNagDialog();
            SQMLog.Log(SQMDataId.UnlinkedDeviceNagDialogShow, 1);
            UIDevice device = this.CurrentDevice;
            BooleanChoice optOutChoice = null;
            Command okCommand = new Command(this, DialogHelper.DialogYes, delegate
          {
              SQMLog.Log(SQMDataId.UnlinkedDeviceNagDialogYes, 1);
              Shell.SettingsFrame.Settings.Device.Invoke(SettingCategories.DeviceMarketplace);
          });
            EventHandler cancelCommand = delegate
           {
               SQMLog.Log(SQMDataId.UnlinkedDeviceNagDialogNo, 1);
               if (optOutChoice == null || optOutChoice.Value)
                   return;
               SQMLog.Log(SQMDataId.UnlinkedDeviceNagDialogNoShow, 1);
           };
            if (allowUserToOptOut)
            {
                optOutChoice = new BooleanChoice();
                optOutChoice.Value = !device.PromptForAccountLinkage;
                optOutChoice.ChosenChanged += delegate
               {
                   device.PromptForAccountLinkage = !optOutChoice.Value;
               };
                optOutChoice.Description = Shell.LoadString(StringId.IDS_DONT_SHOW_THIS_MESSAGE_AGAIN);
            }
            this._currentUnlinkedDeviceDialog = MessageBox.Show(_unlinkedDeviceNagDialogTitle, string.Format(_unlinkedDeviceNagDialogMessageBase, device.Name), DialogHelper.DialogNo, true, okCommand, null, null, cancelCommand, optOutChoice);
        }

        private void HideUnlinkedDeviceNagDialog()
        {
            if (this._currentUnlinkedDeviceDialog == null || this._currentUnlinkedDeviceDialog.WouldLikeToBeHidden)
                return;
            this._currentUnlinkedDeviceDialog.Hide();
        }

        public static int ConvertSyncOperationToInt(ESyncOperation operation) => (int)operation;

        public static int ConvertSyncStateToInt(ESyncState state) => (int)state;

        public static int ConvertSyncStatusToInt(TrackSyncStatus status) => (int)status;

        public static int GetGasGaugeSegmentWidth(
          int maxWidth,
          long size,
          long totalSize,
          int margins)
        {
            totalSize = Math.Max(totalSize, 1L);
            size = Math.Max(size, 0L);
            return Math.Max((int)Math.Min(maxWidth * size / totalSize, maxWidth - margins), 0);
        }

        public static long DevicelandKilobyte => _devicelandKilobyte;

        public static long DevicelandMegabyte => _devicelandMegabyte;

        public static long DevicelandGigabyte => _devicelandGigabyte;

        public static string FormatLongAsSize(long sizeInBytes)
        {
            if (sizeInBytes < DevicelandMegabyte && sizeInBytes != 0L)
                return string.Format(_sizeInKB, (float)(sizeInBytes / (double)DevicelandKilobyte));
            return sizeInBytes < DevicelandGigabyte ? string.Format(_sizeInMB, (float)(sizeInBytes / (double)DevicelandMegabyte)) : string.Format(_sizeInGB, (float)(sizeInBytes / (double)DevicelandGigabyte));
        }

        public void BrowseAndReplaceMedia(IList items, int mediaType) => FileOpenDialog.Show(Shell.LoadString(StringId.IDS_OPEN_FILE_DIALOG_TITLE), ZuneShell.DefaultInstance.Management.MediaFolder, args =>
       {
           string path = (string)args;
           if (string.IsNullOrEmpty(path))
               return;
           Shell.DeleteMedia(items, false);
           Application.DeferredInvoke(delegate
       {
           try
           {
               if (!ZuneApplication.ZuneLibrary.CanAddMedia(path, (EMediaTypes)mediaType))
                   return;
               ZuneApplication.ZuneLibrary.AddMedia(path);
           }
           catch (UnauthorizedAccessException ex)
           {
           }
           catch (IOException ex)
           {
           }
       }, null);
       });

        public static SyncControls Instance
        {
            get
            {
                if (s_singletonInstance == null)
                    s_singletonInstance = new SyncControls(ZuneShell.DefaultInstance);
                return s_singletonInstance;
            }
        }
    }
}
