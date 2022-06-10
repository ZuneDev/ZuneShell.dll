// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Shell.ZuneApplication
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Win32;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Messaging;
using Microsoft.Zune.Service;
using Microsoft.Zune.Subscription;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using UIXControls;
using ZuneUI;
using ZuneXml;
using System.Linq;

#if OPENZUNE
using Microsoft.Zune.Playback;
using StrixMusic.Sdk.AdapterModels;
using StrixMusic.Sdk.CoreModels;
using StrixMusic.Sdk.MediaPlayback;
using StrixMusic.Sdk.PluginModels;
using StrixMusic.Sdk.Plugins.PlaybackHandler;
#endif

namespace Microsoft.Zune.Shell
{
    public class ZuneApplication
    {
        private const string ResourceDllName = "ZuneShellResources.dll";
        private const int ApplicationIconResourceId = 1;
        private static ZuneLibrary _zuneLibrary;
        private static bool _desktopLocked = false;
        private static bool _phase2InitComplete;
        private static List<Hashtable> _unprocessedAppArgs;
        private static ManualResetEvent _transientTableCleanupComplete = new ManualResetEvent(false);
        private static LaunchFromShellHelper _currentShellCommand;
        private static InteropNotifications _interopNotifications;
        private static IntPtr _hWndSplashScreen;
        private static bool _dbRebuilt;
        private static AppInitializationSequencer _appInitializationSequencer;
        private static InitializationFailsafe _initializationFailsafe;
        private static QuickMixProgress _quickMixProgress;

        public static double ZuneCurrentSettingsVersion => 2.0;

        public static void SetDesktopLockState(bool locked) => _desktopLocked = locked;

        public static bool IsDesktopLocked => _desktopLocked;

        public static string DefaultCommandLineParameterSwitch => "PlayMedia";

        public static ZuneLibrary ZuneLibrary => _zuneLibrary;

        public static Service.Service Service => Zune.Service.Service.Instance;
        public static Service.Service2 Service2 => Zune.Service.Service2.Instance;

        public static event EventHandler Closing;

        public static SetupInstallContext InstallContext
        {
            get
            {
                SetupInstallContext setupInstallContext = SetupInstallContext.Zune;
                try
                {
                    RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Zune\\Setup");
                    if (registryKey != null)
                        setupInstallContext = (SetupInstallContext)registryKey.GetValue("WindowsPhonePresent");
                }
                catch (Exception ex)
                {
                }
                return setupInstallContext;
            }
        }

        internal static IntPtr GetRenderWindow() => Application.Window.Handle;

        public static void PageLoadComplete() => _initializationFailsafe.Complete();

        private static void CorePhase3Ready(int hr, bool fSuc)
        {
            if (!fSuc)
            {
                ZuneUI.Shell.ShowErrorDialog(hr, StringId.IDS_ZUNELAUNCH_ERRORTITLE, StringId.IDS_ZUNELAUNCH_COMPONENT_ERROR);
                ConfirmCloseDialog.ShowDefault();
            }
            else
            {
                SingletonModelItem<WindowSnapSimulator>.Instance.Phase3Init();
                Service.Phase3Initialize();
                SignIn.Instance.Phase3Init();
                MetadataNotifications.Instance.Phase2Init();
                SingletonModelItem<UIDeviceList>.Instance.Phase2Init();
                CDAccess.Phase2Catchup();
                SingletonModelItem<TransportControls>.Instance.Phase2Init();
                SubscriptionEventsListener.Instance.StartListening();
                SoftwareUpdates.Instance.StartUp();
                _interopNotifications = new InteropNotifications();
                if (_interopNotifications != null)
                    _interopNotifications.ShowErrorDialog += new OnShowErrorDialogHandler(OnShowErrorDialog);
                Download.Instance.Phase3Init();
                SyncControls.Instance.Phase3Init();
                PodcastCredentials.Instance.Phase2Init();
                ProxyCredentials.Instance.Phase2Init();
                Win7ShellManager.Instance.SubprocWindow(Application.Window.Handle);
                PhotoManager.Instance.SetWindowHandle(Application.Window.Handle);
                if (OSVersion.IsWin7())
                {
                    SingletonModelItem<ThumbBarButtons>.Instance.Phase3Init();
                    SingletonModelItem<JumpListManager>.Instance.JumpListPinUpdateRequested.Invoke();
                }
                if (!QuickMix.QuickMix.Instance.IsReady)
                {
                    _quickMixProgress = new QuickMixProgress();
                    _quickMixProgress.PropertyChanged += new PropertyChangedEventHandler(OnQuickMixPropertyChanged);
                }
                Telemetry.Instance.StartUpload();
                FeaturesChanged.Instance.StartUp();
                CultureHelper.CheckValidRegionAndLanguage();

#if OPENZUNE
                string id = Guid.NewGuid().ToString();

                var fileService = new OwlCore.AbstractStorage.Win32FileSystemService(@"D:\OneDrive\Music");
                OwlCore.AbstractStorage.SystemIOFolderData settingsFolder =
                    new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Zune\OpenZune"));
                settingsFolder.EnsureExists().Wait();
                StrixMusic.Cores.LocalFiles.Services.LocalFilesCoreSettings settings = new(settingsFolder)
                {
                    InitWithEmptyMetadataRepos = true,
                    ScanWithTagLib = true,
                };

                var localCore = new StrixMusic.Cores.LocalFiles.LocalFilesCore(
                    id, settings, fileService, null);

                var prefs = new MergedCollectionConfig
                {
                    CoreRanking = new string[]
                    {
                        localCore.InstanceId
                    }
                };
                var mergedLayer = new MergedCore(new ICore[] { localCore }, prefs);

                // Perform any async initialization needed. Authenticating, connecting to database, etc.
                mergedLayer.InitAsync().ContinueWith(async task =>
                {
                    mergedLayer.Library.TracksCountChanged += LibraryTracksAdded;

                    // Add plugins
                    string mfAudioServiceId = Guid.NewGuid().ToString();
                    PlaybackHandlerService playbackHandler = new();
                    playbackHandler.RegisterAudioPlayer(MediaFoundationAudioService.Instance, mfAudioServiceId);

                    StrixDataRootPluginWrapper pluginLayer = new(mergedLayer,
                        new PlaybackHandlerPlugin(playbackHandler)
                    );
                    await pluginLayer.InitAsync();
                });
#endif

                ((ZuneUI.Shell)ZuneShell.DefaultInstance).ApplicationInitializationIsComplete = true;
            }
        }

#if OPENZUNE
        private static async void LibraryTracksAdded(object sender, int count)
        {
            if (sender is StrixMusic.Sdk.AppModels.ILibrary library && count > 50)
            {
                library.TracksCountChanged -= LibraryTracksAdded;
                var tracks = await library.GetTracksAsync(limit: 100, offset: 0).ToListAsync();
                await library.PlayTrackCollectionAsync(tracks[0]);
            }
        }
#endif

        private static void Phase2InitializationUIStage(object arg)
        {
            _initializationFailsafe.Initialize(delegate
            {
                _appInitializationSequencer.UIReady();
            });
            ProcessAppArgs();
            Download.Instance.Phase2Init();
            if (!ZuneShell.DefaultInstance.NavigationsPending && ZuneShell.DefaultInstance.CurrentPage is StartupPage)
                ZuneUI.Shell.NavigateToHomePage();
            if (_dbRebuilt)
            {
                string caption = ZuneLibrary.LoadStringFromResource(109U);
                string text = ZuneLibrary.LoadStringFromResource(110U);
                if (!string.IsNullOrEmpty(caption) && !string.IsNullOrEmpty(text))
                    Win32MessageBox.Show(text, caption, Win32MessageBoxType.MB_ICONHAND, null);
            }
            _phase2InitComplete = true;
        }

        private static void Phase2InitializationWorker(object arg)
        {
            if (_hWndSplashScreen != IntPtr.Zero)
                Win32Window.Close(_hWndSplashScreen);

            bool flag = _zuneLibrary.Phase2Initialization(out int hr);
            Application.DeferredInvoke(new DeferredInvokeHandler(Phase2InitializationUIStage), new object[2]
            {
                hr, flag
            });
            ZuneLibrary.CleanupTransientMedia();
            _transientTableCleanupComplete.Set();
            SQMLog.Log(SQMDataId.GdiMode, Application.RenderingType == RenderingType.GDI ? 1 : 0);
        }

        private static void Phase2Initialization(object arg) => ThreadPool.QueueUserWorkItem(new WaitCallback(Phase2InitializationWorker));

        public static void ProcessMessageFromCommandLine(string strArgs) => Application.DeferredInvoke(new DeferredInvokeHandler(ProcessMessageFromCommandLineDeferred), strArgs, DeferredInvokePriority.Low);

        private static void ProcessMessageFromCommandLineDeferred(object args)
        {
            string[] arArgs = SplitCommandLineArguments((string)args);
            if (arArgs != null)
            {
                if (_unprocessedAppArgs == null)
                    _unprocessedAppArgs = new List<Hashtable>();
                Hashtable hashtable = new Hashtable();
                foreach (CommandLineArgument commandLineArgument in CommandLineArgument.ParseArgs(arArgs, DefaultCommandLineParameterSwitch))
                    hashtable[commandLineArgument.Name] = commandLineArgument.Value;
                _unprocessedAppArgs.Add(hashtable);
            }
            if (!_phase2InitComplete)
                return;
            ProcessAppArgs();
        }

        private static void ProcessAppArgs()
        {
            if (_unprocessedAppArgs == null)
                return;
            if (ClientConfiguration.FUE.SettingsVersion < ZuneCurrentSettingsVersion || Fue.Instance.IsFirstLaunch)
            {
                Fue.FUECompleted += new EventHandler(ProcessAppArgsAfterFUE);
            }
            else
            {
                for (int index = 0; index < _unprocessedAppArgs.Count; ++index)
                    ProcessAppArgs(_unprocessedAppArgs[index]);
                _unprocessedAppArgs = null;
            }
        }

        private static void ProcessAppArgs(Hashtable args)
        {
            if (args["device"] is string str && !_phase2InitComplete)
            {
                char[] chArray = new char[1] { '"' };
                SyncControls.Instance.SetCurrentDeviceByCanonicalName(str.Trim(chArray));
            }
            if (args["link"] is string link && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
                ZuneUI.Shell.ProcessExternalLink(link);
            if (args["ripcd"] is string playCdPath && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
                CDAccess.HandleDiskFromAutoplay(playCdPath, CDAction.Rip);
            if (args["playcd"] is string ripCdPath && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
                CDAccess.HandleDiskFromAutoplay(ripCdPath, CDAction.Play);
            if (args["playmedia"] is string initialUrl && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
                RegisterNewFileEnumeration(new LaunchFromShellHelper("play", initialUrl));
            if (args["shellhlp_v2"] is string taskName && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
            {
                string marshalledDataObject = args["dataobject"] as string;
                string eventName = args["event"] as string;
                if (marshalledDataObject != null && eventName != null)
                    RegisterNewFileEnumeration(new LaunchFromShellHelper(taskName, marshalledDataObject, eventName));
            }
            if (args.Contains("refreshlicenses"))
                ZuneLibrary.MarkAllDRMFilesAsNeedingLicenseRefresh();
            if (args.Contains("update"))
                SoftwareUpdates.Instance.InstallUpdates();
            if (args.Contains("shuffleall") && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
                SingletonModelItem<TransportControls>.Instance.ShuffleAllRequested = true;
            if (args.Contains("resumenowplaying") && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
                SingletonModelItem<TransportControls>.Instance.ResumeLastNowPlayingHandler();
            if (args.Contains("refreshcontentandexit"))
            {
                if (!_phase2InitComplete)
                {
                    HRESULT sOk = HRESULT._S_OK;
                    HRESULT hr = ContentRefreshTask.Instance.StartContentRefresh(new AsyncCompleteHandler(OnContentRefreshTaskComplete));
                    if (hr.IsError)
                        OnContentRefreshTaskComplete(hr);
                }
                else
                    ZuneLibrary.MarkAllDRMFilesAsNeedingLicenseRefresh();
            }
            if (!(args["playpin"] is string pinString) || ZuneUI.Shell.IgnoreAppNavigationsArgs)
                return;
            JumpListManager.PlayPin(JumpListPin.Parse(pinString));
        }

        private static void OnContentRefreshTaskComplete(HRESULT hr) => Application.DeferredInvoke(new DeferredInvokeHandler(DeferredClose), DeferredInvokePriority.Normal);

        public static void DeferredClose(object arg) => Application.Window.Close();

        private static void ProcessAppArgsAfterFUE(object sender, EventArgs unused)
        {
            Fue.FUECompleted -= new EventHandler(ProcessAppArgsAfterFUE);
            ProcessAppArgs();
        }

        private static void RegisterNewFileEnumeration(LaunchFromShellHelper helper)
        {
            if (_currentShellCommand != null)
                _currentShellCommand.Cancel();
            _currentShellCommand = helper;
            _currentShellCommand.Go(new DeferredInvokeHandler(DataObjectEnumerationComplete));
        }

        private static void DataObjectEnumerationComplete(object args)
        {
            if (args != _currentShellCommand)
                return;
            string taskName = _currentShellCommand.TaskName;
            if (taskName == "play" || taskName == "playasplaylist")
            {
                List<FileEntry> files = _currentShellCommand.Files;
                MediaType mediaType = FilterFiles(files);
                if (files != null && files.Count > 0 && (mediaType == MediaType.Track || mediaType == MediaType.Video))
                {
                    PlaybackContext playbackContext = PlaybackContext.Music;
                    if (mediaType == MediaType.Video)
                        playbackContext = PlaybackContext.LibraryVideo;
                    SingletonModelItem<TransportControls>.Instance.PlayItems(files, playbackContext);
                }
            }
            _currentShellCommand = null;
        }

        private static MediaType FilterFiles(List<FileEntry> enumeratedFiles)
        {
            MediaType mediaType = MediaType.Undefined;
            if (enumeratedFiles != null && enumeratedFiles.Count > 0)
            {
                mediaType = (MediaType)enumeratedFiles[0].MediaType;
                bool flag = false;
                switch (mediaType)
                {
                    case MediaType.Track:
                        flag = true;
                        break;
                    case MediaType.Video:
                        if (enumeratedFiles.Count > 1)
                        {
                            enumeratedFiles.RemoveRange(1, enumeratedFiles.Count - 1);
                            break;
                        }
                        break;
                    default:
                        mediaType = MediaType.Undefined;
                        enumeratedFiles.Clear();
                        break;
                }
                if (flag)
                {
                    for (int index = enumeratedFiles.Count - 1; index >= 0; --index)
                    {
                        if (enumeratedFiles[index].MediaType != (EMediaTypes)mediaType)
                            enumeratedFiles.RemoveAt(index);
                    }
                }
            }
            return mediaType;
        }

        private static string[] SplitCommandLineArguments(string arguments)
        {
            if (string.IsNullOrEmpty(arguments))
                return null;
            string[] strArray = null;
            MatchCollection matchCollection = new Regex("(\\S*?(\\\")([^\\\"])+(\\\"))|[^\\s\"]+").Matches(arguments);
            if (matchCollection.Count > 0)
            {
                strArray = new string[matchCollection.Count];
                for (int i = 0; i < strArray.Length; ++i)
                    strArray[i] = matchCollection[i].Value;
            }
            return strArray;
        }

        [STAThread]
        public static int Launch(string strArgs, IntPtr hWndSplashScreen)
        {
            PerfTrace.PerfTrace.PERFTRACE_LAUNCHEVENT(PerfTrace.PerfTrace.LAUNCH_EVENT.IN_MANAGED_LAUNCH, 0U);
            Application.ErrorReport += new ErrorReportHandler(ErrorReportHandler);
            Hashtable hashtable = StandAlone.Startup(SplitCommandLineArguments(strArgs), DefaultCommandLineParameterSwitch);
            if (hashtable != null)
            {
                _unprocessedAppArgs = new List<Hashtable>();
                _unprocessedAppArgs.Add(hashtable);
            }
            DialogHelper.DialogCancel = ZuneUI.Shell.LoadString(StringId.IDS_DIALOG_CANCEL);
            DialogHelper.DialogYes = ZuneUI.Shell.LoadString(StringId.IDS_DIALOG_YES);
            DialogHelper.DialogNo = ZuneUI.Shell.LoadString(StringId.IDS_DIALOG_NO);
            DialogHelper.DialogOk = ZuneUI.Shell.LoadString(StringId.IDS_DIALOG_OK);
            XmlDataProviders.Register();
            LibraryDataProvider.Register();
            SubscriptionDataProvider.Register();
            StaticLibraryDataProvider.Register();
            AggregateDataProviderQuery.Register();
            ZuneUI.Shell.InitializeInstance();
            Application.Name = "Zune";
            Application.Window.Caption = "Zune";
            Application.Window.SetIcon("ZuneShellResources.dll", 1);
            Application.Window.AlwaysOnTop = true;
            if (!hashtable.Contains("noshadow"))
            {
                Image[] images = new Image[4];
                ImageInset imageInset1 = new ImageInset(26, 0, 30, 0);
                ImageInset imageInset2 = new ImageInset(0, 10, 0, 0);
                images[0] = new Image("res://ZuneShellResources.dll!activeshadowLeft.png", imageInset2);
                images[1] = new Image("res://ZuneShellResources.dll!activeshadowTop.png", imageInset1);
                images[2] = new Image("res://ZuneShellResources.dll!activeshadowRight.png", imageInset2);
                images[3] = new Image("res://ZuneShellResources.dll!activeshadowBottom.png", imageInset1);
                Application.Window.SetShadowEdgeImages(true, images);
                imageInset1 = new ImageInset(23, 0, 29, 0);
                imageInset2 = new ImageInset(0, 5, 0, 0);
                images[0] = new Image("res://ZuneShellResources.dll!inactiveshadowLeft.png", imageInset2);
                images[1] = new Image("res://ZuneShellResources.dll!inactiveshadowTop.png", imageInset1);
                images[2] = new Image("res://ZuneShellResources.dll!inactiveshadowRight.png", imageInset2);
                images[3] = new Image("res://ZuneShellResources.dll!inactiveshadowBottom.png", imageInset1);
                Application.Window.SetShadowEdgeImages(false, images);
            }
            Application.Window.CloseRequested += new WindowCloseRequestedHandler(CodeDialogManager.Instance.OnWindowCloseRequested);
            CodeDialogManager.Instance.WindowCloseNotBlocked += new EventHandler(OnWindowCloseNotBlocked);
            Application.Window.SessionConnected += new SessionConnectedHandler(OnSessionConnected);
            string source = "res://ZuneShellResources!Frame.uix#Frame";
            _hWndSplashScreen = hWndSplashScreen;
            _initializationFailsafe = new InitializationFailsafe();
            PerfTrace.PerfTrace.PERFTRACE_LAUNCHEVENT(PerfTrace.PerfTrace.LAUNCH_EVENT.REQUEST_UI_LOAD, 0U);
            Application.Window.RequestLoad(source);
            PerfTrace.PerfTrace.PERFTRACE_LAUNCHEVENT(PerfTrace.PerfTrace.LAUNCH_EVENT.REQUEST_UI_LOAD_COMPLETE, 0U);
            CallbackOnUIThread callbackOnUiThread = new CallbackOnUIThread();
            _appInitializationSequencer = new AppInitializationSequencer(new CorePhase2ReadyCallback(CorePhase3Ready));
            _zuneLibrary = new ZuneLibrary();
            HRESULT num = _zuneLibrary.Initialize(null, out _dbRebuilt);
            if (num.IsSuccess)
            {
                StandAlone.Run(new DeferredInvokeHandler(Phase2Initialization));
                Application.Window.CloseRequested -= new WindowCloseRequestedHandler(CodeDialogManager.Instance.OnWindowCloseRequested);
                CodeDialogManager.Instance.WindowCloseNotBlocked -= new EventHandler(OnWindowCloseNotBlocked);
                Application.Window.SessionConnected -= new SessionConnectedHandler(OnSessionConnected);
                if (Download.IsCreated)
                    Download.Instance.Dispose();
                ZuneShell.DefaultInstance?.Dispose();
                ViewTimeLogger.Instance.Shutdown();
                if (PodcastCredentials.HasInstance)
                    PodcastCredentials.Instance.Dispose();
                if (ProxyCredentials.HasInstance)
                    ProxyCredentials.Instance.Dispose();
                StandAlone.Shutdown();
                HttpWebRequest.Shutdown();
                WorkerQueue.ShutdownAll();
                if (ContentRefreshTask.HasInstance)
                    ContentRefreshTask.Instance.Dispose();
                if (Service != null)
                    Service.Dispose();
                if (ShellMessagingNotifier.HasInstance)
                    ShellMessagingNotifier.Instance.Dispose();
                if (MessagingService.HasInstance)
                    MessagingService.Instance.Dispose();
                if (FeaturesChangedApi.HasInstance)
                    FeaturesChangedApi.Instance.Dispose();
                CDAccess.Instance.Dispose();
                PlaylistManager.Instance.Dispose();
                if (_interopNotifications != null)
                {
                    _interopNotifications.ShowErrorDialog -= new OnShowErrorDialogHandler(OnShowErrorDialog);
                    _interopNotifications.Dispose();
                    _interopNotifications = null;
                }
            }
            _zuneLibrary.Dispose();
            return num.Int;
        }

        private static void ErrorReportHandler(Error[] errors)
        {
            int num = 0;
            string str = string.Empty;
            foreach (Error error in errors)
            {
                if (!error.Warning)
                {
                    str = str + error.ToString() + "\n";
                    ++num;
                }
            }
            if (num > 0)
                throw new ZuneShellException("Internal Zune Shell error", string.Format("Scripting errors encountered (Process ID) = {0}\n\n{1}", Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture), str));
        }

        internal static bool CanAddMedia(IList filenames, MediaType mediaType, CanAddMediaArgs args)
        {
            foreach (string filename in filenames)
            {
                if (args.Aborted)
                    return false;
                if (CanAddMedia(filename, mediaType, args))
                    return true;
            }
            return false;
        }

        private static bool CanAddMedia(string filename, MediaType mediaType, CanAddMediaArgs args)
        {
            try
            {
                return Directory.Exists(filename) ? ZuneLibrary.CanAddFromFolder(filename) && (CanAddMedia(Directory.GetFiles(filename), mediaType, args) || CanAddMedia(Directory.GetDirectories(filename), mediaType, args)) : ZuneLibrary.CanAddMedia(filename, (EMediaTypes)mediaType);
            }
            catch (UnauthorizedAccessException ex)
            {
                return false;
            }
            catch (IOException ex)
            {
                return false;
            }
        }

        internal static bool AddMedia(IList filenames, MediaType mediaType)
        {
            bool flag = false;
            foreach (string filename in filenames)
                flag |= AddMedia(filename, mediaType);
            return flag;
        }

        private static bool AddMedia(string filename, MediaType mediaType)
        {
            bool flag = false;
            try
            {
                if (Directory.Exists(filename))
                {
                    flag = AddMedia(Directory.GetFiles(filename), mediaType);
                    flag |= AddMedia(Directory.GetDirectories(filename), mediaType);
                }
                else if (ZuneLibrary.CanAddMedia(filename, (EMediaTypes)mediaType))
                    flag = ZuneLibrary.AddMedia(filename) != -1;
            }
            catch (UnauthorizedAccessException ex)
            {
            }
            catch (IOException ex)
            {
            }
            return flag;
        }

        internal static bool AddTransientMedia(
          string filename,
          MediaType mediaType,
          out int libraryID,
          out bool fFileAlreadyExists)
        {
            _transientTableCleanupComplete.WaitOne();
            return ZuneLibrary.AddTransientMedia(filename, (EMediaTypes)mediaType, out libraryID, out fFileAlreadyExists);
        }

        private static void OnWindowCloseNotBlocked(object sender, EventArgs args)
        {
            bool flag1 = SyncControls.Instance.CurrentDeviceOverride.UIFirmwareUpdater != null && SyncControls.Instance.CurrentDeviceOverride.UIFirmwareUpdater.UpdateInProgress;
            bool flag2 = SyncControls.Instance.CurrentDeviceOverride.UIFirmwareRestorer != null && SyncControls.Instance.CurrentDeviceOverride.UIFirmwareRestorer.RestoreInProgress;
            if (CDAccess.Instance.Notification != null || CDAccess.Instance.BurnNotification != null || Download.IsCreated && Download.Instance.Notification != null || (flag1 || flag2))
            {
                if (Application.RenderingType == RenderingType.GDI && SingletonModelItem<TransportControls>.Instance.PlayingVideo)
                    SingletonModelItem<TransportControls>.Instance.Stop.Invoke();
                string ui = "res://ZuneShellResources!ConfirmClose.uix#ConfirmCloseContentUI";
                if (flag1)
                    ui = "res://ZuneShellResources!ConfirmClose.uix#ConfirmFirmwareUpdateCloseContentUI";
                else if (flag2)
                    ui = "res://ZuneShellResources!ConfirmClose.uix#ConfirmFirmwareRestoreCloseContentUI";
                ConfirmCloseDialog.Show(ui, delegate
               {
                   ForceClose(sender, args);
               });
            }
            else
                ForceClose(sender, args);
        }

        private static void ForceClose(object sender, EventArgs args)
        {
            if (Closing != null)
                Closing(sender, args);
            Application.Window.ForceClose();
        }

        private static void OnSessionConnected(object sender, bool fIsConnected)
        {
            if (fIsConnected)
                return;
            SingletonModelItem<TransportControls>.Instance.CloseCurrentSession();
        }

        public static void OnShowErrorDialog(int hr, uint uiStringId) => Application.DeferredInvoke(new DeferredInvokeHandler(DeferredShowErrorDialog), new object[2]
        {
            hr,
            uiStringId
        });

        public static void DeferredShowErrorDialog(object arg)
        {
            object[] objArray = (object[])arg;
            ZuneUI.Shell.ShowErrorDialog(Convert.ToInt32(objArray[0]), ZuneUI.Shell.LoadString((StringId)Convert.ToUInt32(objArray[1])));
        }

        private static void OnQuickMixPropertyChanged(object Sender, PropertyChangedEventArgs e)
        {
            if (!(e.PropertyName == "Progress") || _quickMixProgress.Progress < 100.0)
                return;
            NotificationArea.Instance.Add(new QuickMixNotification(ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_NOTIFICATION_BOOTSTRAP_READY_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_NOTIFICATION_BOOTSTRAP_READY_TEXT), NotificationState.Completed, true, 10000));
            _quickMixProgress.PropertyChanged -= new PropertyChangedEventHandler(OnQuickMixPropertyChanged);
            _quickMixProgress = null;
        }
    }
}
