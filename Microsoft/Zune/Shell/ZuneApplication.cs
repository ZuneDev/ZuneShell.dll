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

        public static void SetDesktopLockState(bool locked) => ZuneApplication._desktopLocked = locked;

        public static bool IsDesktopLocked => ZuneApplication._desktopLocked;

        public static string DefaultCommandLineParameterSwitch => "PlayMedia";

        public static ZuneLibrary ZuneLibrary => ZuneApplication._zuneLibrary;

        public static Microsoft.Zune.Service.Service Service => Microsoft.Zune.Service.Service.Instance;

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

        public static void PageLoadComplete() => ZuneApplication._initializationFailsafe.Complete();

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
                ZuneApplication.Service.Phase3Initialize();
                SignIn.Instance.Phase3Init();
                MetadataNotifications.Instance.Phase2Init();
                SingletonModelItem<UIDeviceList>.Instance.Phase2Init();
                CDAccess.Phase2Catchup();
                SingletonModelItem<TransportControls>.Instance.Phase2Init();
                SubscriptionEventsListener.Instance.StartListening();
                SoftwareUpdates.Instance.StartUp();
                ZuneApplication._interopNotifications = new InteropNotifications();
                if (ZuneApplication._interopNotifications != null)
                    ZuneApplication._interopNotifications.ShowErrorDialog += new OnShowErrorDialogHandler(ZuneApplication.OnShowErrorDialog);
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
                if (!Microsoft.Zune.QuickMix.QuickMix.Instance.IsReady)
                {
                    ZuneApplication._quickMixProgress = new QuickMixProgress();
                    ZuneApplication._quickMixProgress.PropertyChanged += new PropertyChangedEventHandler(ZuneApplication.OnQuickMixPropertyChanged);
                }
                Telemetry.Instance.StartUpload();
                FeaturesChanged.Instance.StartUp();
                CultureHelper.CheckValidRegionAndLanguage();
                ((ZuneUI.Shell)ZuneShell.DefaultInstance).ApplicationInitializationIsComplete = true;
            }
        }

        private static void Phase2InitializationUIStage(object arg)
        {
            ZuneApplication._initializationFailsafe.Initialize((DeferredInvokeHandler)delegate
           {
               ZuneApplication._appInitializationSequencer.UIReady();
           });
            ZuneApplication.ProcessAppArgs();
            Download.Instance.Phase2Init();
            if (!ZuneShell.DefaultInstance.NavigationsPending && ZuneShell.DefaultInstance.CurrentPage is StartupPage)
                ZuneUI.Shell.NavigateToHomePage();
            if (ZuneApplication._dbRebuilt)
            {
                string caption = ZuneLibrary.LoadStringFromResource(109U);
                string text = ZuneLibrary.LoadStringFromResource(110U);
                if (!string.IsNullOrEmpty(caption) && !string.IsNullOrEmpty(text))
                    Win32MessageBox.Show(text, caption, Win32MessageBoxType.MB_ICONHAND, (DeferredInvokeHandler)null);
            }
            ZuneApplication._phase2InitComplete = true;
        }

        private static void Phase2InitializationWorker(object arg)
        {
            Win32Window.Close(ZuneApplication._hWndSplashScreen);
            int hr;
            bool flag = ZuneApplication._zuneLibrary.Phase2Initialization(out hr);
            Application.DeferredInvoke(new DeferredInvokeHandler(ZuneApplication.Phase2InitializationUIStage), (object)new object[2]
            {
        (object) hr,
        (object) flag
            });
            ZuneApplication.ZuneLibrary.CleanupTransientMedia();
            ZuneApplication._transientTableCleanupComplete.Set();
            SQMLog.Log(SQMDataId.GdiMode, Application.RenderingType == RenderingType.GDI ? 1 : 0);
        }

        private static void Phase2Initialization(object arg) => ThreadPool.QueueUserWorkItem(new WaitCallback(ZuneApplication.Phase2InitializationWorker));

        public static void ProcessMessageFromCommandLine(string strArgs) => Application.DeferredInvoke(new DeferredInvokeHandler(ZuneApplication.ProcessMessageFromCommandLineDeferred), (object)strArgs, DeferredInvokePriority.Low);

        private static void ProcessMessageFromCommandLineDeferred(object args)
        {
            string[] arArgs = ZuneApplication.SplitCommandLineArguments((string)args);
            if (arArgs != null)
            {
                if (ZuneApplication._unprocessedAppArgs == null)
                    ZuneApplication._unprocessedAppArgs = new List<Hashtable>();
                Hashtable hashtable = new Hashtable();
                foreach (CommandLineArgument commandLineArgument in CommandLineArgument.ParseArgs(arArgs, ZuneApplication.DefaultCommandLineParameterSwitch))
                    hashtable[(object)commandLineArgument.Name] = (object)commandLineArgument.Value;
                ZuneApplication._unprocessedAppArgs.Add(hashtable);
            }
            if (!ZuneApplication._phase2InitComplete)
                return;
            ZuneApplication.ProcessAppArgs();
        }

        private static void ProcessAppArgs()
        {
            if (ZuneApplication._unprocessedAppArgs == null)
                return;
            if (ClientConfiguration.FUE.SettingsVersion < ZuneApplication.ZuneCurrentSettingsVersion || Fue.Instance.IsFirstLaunch)
            {
                Fue.FUECompleted += new EventHandler(ZuneApplication.ProcessAppArgsAfterFUE);
            }
            else
            {
                for (int index = 0; index < ZuneApplication._unprocessedAppArgs.Count; ++index)
                    ZuneApplication.ProcessAppArgs(ZuneApplication._unprocessedAppArgs[index]);
                ZuneApplication._unprocessedAppArgs = (List<Hashtable>)null;
            }
        }

        private static void ProcessAppArgs(Hashtable args)
        {
            if (args[(object)"device"] is string str && !ZuneApplication._phase2InitComplete)
            {
                char[] chArray = new char[1] { '"' };
                SyncControls.Instance.SetCurrentDeviceByCanonicalName(str.Trim(chArray));
            }
            if (args[(object)"link"] is string link && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
                ZuneUI.Shell.ProcessExternalLink(link);
            if (args[(object)"ripcd"] is string path && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
                CDAccess.HandleDiskFromAutoplay(path, CDAction.Rip);
            if (args[(object)"playcd"] is string path && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
                CDAccess.HandleDiskFromAutoplay(path, CDAction.Play);
            if (args[(object)"playmedia"] is string initialUrl && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
                ZuneApplication.RegisterNewFileEnumeration(new LaunchFromShellHelper("play", initialUrl));
            if (args[(object)"shellhlp_v2"] is string taskName && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
            {
                string marshalledDataObject = args[(object)"dataobject"] as string;
                string eventName = args[(object)"event"] as string;
                if (marshalledDataObject != null && eventName != null)
                    ZuneApplication.RegisterNewFileEnumeration(new LaunchFromShellHelper(taskName, marshalledDataObject, eventName));
            }
            if (args.Contains((object)"refreshlicenses"))
                ZuneApplication.ZuneLibrary.MarkAllDRMFilesAsNeedingLicenseRefresh();
            if (args.Contains((object)"update"))
                SoftwareUpdates.Instance.InstallUpdates();
            if (args.Contains((object)"shuffleall") && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
                SingletonModelItem<TransportControls>.Instance.ShuffleAllRequested = true;
            if (args.Contains((object)"resumenowplaying") && !ZuneUI.Shell.IgnoreAppNavigationsArgs)
                SingletonModelItem<TransportControls>.Instance.ResumeLastNowPlayingHandler();
            if (args.Contains((object)"refreshcontentandexit"))
            {
                if (!ZuneApplication._phase2InitComplete)
                {
                    HRESULT sOk = HRESULT._S_OK;
                    HRESULT hr = ContentRefreshTask.Instance.StartContentRefresh(new AsyncCompleteHandler(ZuneApplication.OnContentRefreshTaskComplete));
                    if (hr.IsError)
                        ZuneApplication.OnContentRefreshTaskComplete(hr);
                }
                else
                    ZuneApplication.ZuneLibrary.MarkAllDRMFilesAsNeedingLicenseRefresh();
            }
            if (!(args[(object)"playpin"] is string pinString) || ZuneUI.Shell.IgnoreAppNavigationsArgs)
                return;
            JumpListManager.PlayPin(JumpListPin.Parse(pinString));
        }

        private static void OnContentRefreshTaskComplete(HRESULT hr) => Application.DeferredInvoke(new DeferredInvokeHandler(ZuneApplication.DeferredClose), DeferredInvokePriority.Normal);

        public static void DeferredClose(object arg) => Application.Window.Close();

        private static void ProcessAppArgsAfterFUE(object sender, EventArgs unused)
        {
            Fue.FUECompleted -= new EventHandler(ZuneApplication.ProcessAppArgsAfterFUE);
            ZuneApplication.ProcessAppArgs();
        }

        private static void RegisterNewFileEnumeration(LaunchFromShellHelper helper)
        {
            if (ZuneApplication._currentShellCommand != null)
                ZuneApplication._currentShellCommand.Cancel();
            ZuneApplication._currentShellCommand = helper;
            ZuneApplication._currentShellCommand.Go(new DeferredInvokeHandler(ZuneApplication.DataObjectEnumerationComplete));
        }

        private static void DataObjectEnumerationComplete(object args)
        {
            if (args != ZuneApplication._currentShellCommand)
                return;
            string taskName = ZuneApplication._currentShellCommand.TaskName;
            if (taskName == "play" || taskName == "playasplaylist")
            {
                List<FileEntry> files = ZuneApplication._currentShellCommand.Files;
                MediaType mediaType = ZuneApplication.FilterFiles(files);
                if (files != null && files.Count > 0 && (mediaType == MediaType.Track || mediaType == MediaType.Video))
                {
                    PlaybackContext playbackContext = PlaybackContext.Music;
                    if (mediaType == MediaType.Video)
                        playbackContext = PlaybackContext.LibraryVideo;
                    SingletonModelItem<TransportControls>.Instance.PlayItems((IList)files, playbackContext);
                }
            }
            ZuneApplication._currentShellCommand = (LaunchFromShellHelper)null;
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
                return (string[])null;
            string[] strArray = (string[])null;
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
            Microsoft.Zune.PerfTrace.PerfTrace.PERFTRACE_LAUNCHEVENT(Microsoft.Zune.PerfTrace.PerfTrace.LAUNCH_EVENT.IN_MANAGED_LAUNCH, 0U);
            Application.ErrorReport += new Microsoft.Iris.ErrorReportHandler(ZuneApplication.ErrorReportHandler);
            Hashtable hashtable = StandAlone.Startup(ZuneApplication.SplitCommandLineArguments(strArgs), ZuneApplication.DefaultCommandLineParameterSwitch);
            if (hashtable != null)
            {
                ZuneApplication._unprocessedAppArgs = new List<Hashtable>();
                ZuneApplication._unprocessedAppArgs.Add(hashtable);
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
            if (!hashtable.Contains((object)"noshadow"))
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
            CodeDialogManager.Instance.WindowCloseNotBlocked += new EventHandler(ZuneApplication.OnWindowCloseNotBlocked);
            Application.Window.SessionConnected += new SessionConnectedHandler(ZuneApplication.OnSessionConnected);
            string source = "res://ZuneShellResources!Frame.uix#Frame";
            ZuneApplication._hWndSplashScreen = hWndSplashScreen;
            ZuneApplication._initializationFailsafe = new InitializationFailsafe();
            Microsoft.Zune.PerfTrace.PerfTrace.PERFTRACE_LAUNCHEVENT(Microsoft.Zune.PerfTrace.PerfTrace.LAUNCH_EVENT.REQUEST_UI_LOAD, 0U);
            Application.Window.RequestLoad(source);
            Microsoft.Zune.PerfTrace.PerfTrace.PERFTRACE_LAUNCHEVENT(Microsoft.Zune.PerfTrace.PerfTrace.LAUNCH_EVENT.REQUEST_UI_LOAD_COMPLETE, 0U);
            CallbackOnUIThread callbackOnUiThread = new CallbackOnUIThread();
            ZuneApplication._appInitializationSequencer = new AppInitializationSequencer(new CorePhase2ReadyCallback(ZuneApplication.CorePhase3Ready));
            ZuneApplication._zuneLibrary = new ZuneLibrary();
            int num = ZuneApplication._zuneLibrary.Initialize((string)null, out ZuneApplication._dbRebuilt);
            if (num == 0)
            {
                StandAlone.Run(new DeferredInvokeHandler(ZuneApplication.Phase2Initialization));
                Application.Window.CloseRequested -= new WindowCloseRequestedHandler(CodeDialogManager.Instance.OnWindowCloseRequested);
                CodeDialogManager.Instance.WindowCloseNotBlocked -= new EventHandler(ZuneApplication.OnWindowCloseNotBlocked);
                Application.Window.SessionConnected -= new SessionConnectedHandler(ZuneApplication.OnSessionConnected);
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
                if (ZuneApplication.Service != null)
                    ZuneApplication.Service.Dispose();
                if (ShellMessagingNotifier.HasInstance)
                    ShellMessagingNotifier.Instance.Dispose();
                if (MessagingService.HasInstance)
                    MessagingService.Instance.Dispose();
                if (FeaturesChangedApi.HasInstance)
                    FeaturesChangedApi.Instance.Dispose();
                CDAccess.Instance.Dispose();
                PlaylistManager.Instance.Dispose();
                if (ZuneApplication._interopNotifications != null)
                {
                    ZuneApplication._interopNotifications.ShowErrorDialog -= new OnShowErrorDialogHandler(ZuneApplication.OnShowErrorDialog);
                    ZuneApplication._interopNotifications.Dispose();
                    ZuneApplication._interopNotifications = (InteropNotifications)null;
                }
            }
            ZuneApplication._zuneLibrary.Dispose();
            return num;
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
                throw new ZuneShellException("Internal Zune Shell error", string.Format("Scripting errors encountered (Process ID) = {0}\n\n{1}", (object)Process.GetCurrentProcess().Id.ToString((IFormatProvider)CultureInfo.InvariantCulture), (object)str));
        }

        internal static bool CanAddMedia(IList filenames, MediaType mediaType, CanAddMediaArgs args)
        {
            foreach (string filename in (IEnumerable)filenames)
            {
                if (args.Aborted)
                    return false;
                if (ZuneApplication.CanAddMedia(filename, mediaType, args))
                    return true;
            }
            return false;
        }

        private static bool CanAddMedia(string filename, MediaType mediaType, CanAddMediaArgs args)
        {
            try
            {
                return Directory.Exists(filename) ? ZuneApplication.ZuneLibrary.CanAddFromFolder(filename) && (ZuneApplication.CanAddMedia((IList)Directory.GetFiles(filename), mediaType, args) || ZuneApplication.CanAddMedia((IList)Directory.GetDirectories(filename), mediaType, args)) : ZuneApplication.ZuneLibrary.CanAddMedia(filename, (EMediaTypes)mediaType);
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
            foreach (string filename in (IEnumerable)filenames)
                flag |= ZuneApplication.AddMedia(filename, mediaType);
            return flag;
        }

        private static bool AddMedia(string filename, MediaType mediaType)
        {
            bool flag = false;
            try
            {
                if (Directory.Exists(filename))
                {
                    flag = ZuneApplication.AddMedia((IList)Directory.GetFiles(filename), mediaType);
                    flag |= ZuneApplication.AddMedia((IList)Directory.GetDirectories(filename), mediaType);
                }
                else if (ZuneApplication.ZuneLibrary.CanAddMedia(filename, (EMediaTypes)mediaType))
                    flag = ZuneApplication.ZuneLibrary.AddMedia(filename) != -1;
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
            ZuneApplication._transientTableCleanupComplete.WaitOne();
            return ZuneApplication.ZuneLibrary.AddTransientMedia(filename, (EMediaTypes)mediaType, out libraryID, out fFileAlreadyExists);
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
                ConfirmCloseDialog.Show(ui, (EventHandler)delegate
               {
                   ZuneApplication.ForceClose(sender, args);
               });
            }
            else
                ZuneApplication.ForceClose(sender, args);
        }

        private static void ForceClose(object sender, EventArgs args)
        {
            if (ZuneApplication.Closing != null)
                ZuneApplication.Closing(sender, args);
            Application.Window.ForceClose();
        }

        private static void OnSessionConnected(object sender, bool fIsConnected)
        {
            if (fIsConnected)
                return;
            SingletonModelItem<TransportControls>.Instance.CloseCurrentSession();
        }

        public static void OnShowErrorDialog(int hr, uint uiStringId) => Application.DeferredInvoke(new DeferredInvokeHandler(ZuneApplication.DeferredShowErrorDialog), (object)new object[2]
        {
      (object) hr,
      (object) uiStringId
        });

        public static void DeferredShowErrorDialog(object arg)
        {
            object[] objArray = (object[])arg;
            ZuneUI.Shell.ShowErrorDialog(Convert.ToInt32(objArray[0]), ZuneUI.Shell.LoadString((StringId)Convert.ToUInt32(objArray[1])));
        }

        private static void OnQuickMixPropertyChanged(object Sender, PropertyChangedEventArgs e)
        {
            if (!(e.PropertyName == "Progress") || (double)ZuneApplication._quickMixProgress.Progress < 100.0)
                return;
            NotificationArea.Instance.Add((ZuneUI.Notification)new QuickMixNotification(ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_NOTIFICATION_BOOTSTRAP_READY_TITLE), ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_NOTIFICATION_BOOTSTRAP_READY_TEXT), NotificationState.Completed, true, 10000));
            ZuneApplication._quickMixProgress.PropertyChanged -= new PropertyChangedEventHandler(ZuneApplication.OnQuickMixPropertyChanged);
            ZuneApplication._quickMixProgress = (QuickMixProgress)null;
        }
    }
}
