// Decompiled with JetBrains decompiler
// Type: ZuneUI.Fue
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System;

namespace ZuneUI
{
    public class Fue : ModelItem
    {
        private const int RenderPromptIntervalInitValue = 120000;
        private const int RenderPromptIntervalPostValue = 120000;
        private bool _proxyDefaultPathsComplete;
        private string errorMessage;
        private static Fue singletonInstance;
        private bool fileTypeAssociationsAreSet;
        private int _renderPromptInterval = RenderPromptIntervalInitValue;
        private Timer _renderPromptTimer;

        public static Fue Instance
        {
            get
            {
                if (singletonInstance == null)
                    singletonInstance = new Fue();
                return singletonInstance;
            }
        }

        public bool IsFirstLaunch =>
            ClientConfiguration.FUE.ShowFUE
            || ClientConfiguration.FUE.ShowFirstLaunchVideo
            && FeatureEnablement.IsFeatureEnabled(Features.eFirstLaunchIntroVideo);

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                Shell.ShowErrorDialog(0, errorMessage);
            }
        }

        public static event EventHandler FUECompleted;

        private Fue()
        {
        }

        public void StartJobs()
        {
            InitializeDefaultPaths();
            UpdateNSS();
        }

        public bool AutoFUE { get; set; }

        public void MigrateLegacyConfiguration() => ClientConfiguration.FUE.SettingsVersion = ZuneApplication.ZuneCurrentSettingsVersion;

        public void SetFileTypeAssociationsAreSet() => fileTypeAssociationsAreSet = true;

        public void InitializeQuickplayConfig()
        {
            if (!FeatureEnablement.IsFeatureEnabled(Features.eQuickplay))
                return;
            ClientConfiguration.Shell.StartupPage = Shell.MainFrame.Quickplay.DefaultUIPath;
            ClientConfiguration.Quickplay.ShowFUE = true;
            ClientConfiguration.Quickplay.CheckUseCount = true;
            ClientConfiguration.Quickplay.UnusedCount = 0;
            ClientConfiguration.Quickplay.FavoredExperience = "";
        }

        public void CompleteFUE()
        {
            ClientConfiguration.FUE.ShowFUE = false;
            ClientConfiguration.FUE.ShowArtistChooser = FeatureEnablement.IsFeatureEnabled(Features.eQuickMixLocal) && (FeatureEnablement.IsFeatureEnabled(Features.eQuickplay) || FeatureEnablement.IsFeatureEnabled(Features.ePicks));
            InitializeQuickplayConfig();
            if (!fileTypeAssociationsAreSet)
                ZuneShell.DefaultInstance.Management.SaveFileTypesAsDefault();
            ZuneShell.DefaultInstance.NavigateBack();
            SQMLog.LogToStream(SQMDataId.LanguageLocale, Shell.LoadString(StringId.IDS_ZUNECLIENT_LOCALE));
            SQMLog.Log(SQMDataId.DXModeEnabled, Application.RenderingType == RenderingType.DX9 ? 1 : 0);
            ClientConfiguration.SQM.SQMLaunchIndex = 0;
            StartJobs();
            FUECompleted?.Invoke(Instance, EventArgs.Empty);
        }

        public void CompleteMigration()
        {
            InitializeQuickplayConfig();
            ZuneShell.DefaultInstance.NavigateBack();
            FUECompleted?.Invoke(Instance, EventArgs.Empty);
        }

        public void ProxyDefaultPaths()
        {
            if (_proxyDefaultPathsComplete)
                return;
            var management = ZuneShell.DefaultInstance.Management;
            HRESULT knownFolders = ZuneApplication.ZuneLibrary.GetKnownFolders(out var music, out var videos, out var pictures, out var podcasts, out string[] _, out var ripFolder, out var videoMediaFolder, out var photoMediaFolder, out var podcastMediaFolder, out string _);
            if (ClientConfiguration.Groveler.MonitoredAudioFolders == null)
            {
                for (int index = 0; index < music.Length; ++index)
                    management.MonitoredAudioFolders.Add(music[index]);
            }
            if (ClientConfiguration.Groveler.MonitoredPhotoFolders == null)
            {
                for (int index = 0; index < pictures.Length; ++index)
                    management.MonitoredPhotoFolders.Add(pictures[index]);
            }
            if (ClientConfiguration.Groveler.MonitoredVideoFolders == null)
            {
                for (int index = 0; index < videos.Length; ++index)
                    management.MonitoredVideoFolders.Add(videos[index]);
            }
            if (ClientConfiguration.Groveler.MonitoredPodcastFolders == null)
            {
                for (int index = 0; index < podcasts.Length; ++index)
                    management.MonitoredPodcastFolders.Add(podcasts[index]);
            }
            management.MediaFolder = string.IsNullOrEmpty(ClientConfiguration.Groveler.RipDirectory) ? ripFolder : ClientConfiguration.Groveler.RipDirectory;
            management.VideoMediaFolder = string.IsNullOrEmpty(ClientConfiguration.Groveler.VideoMediaFolder) ? videoMediaFolder : ClientConfiguration.Groveler.VideoMediaFolder;
            management.PhotoMediaFolder = string.IsNullOrEmpty(ClientConfiguration.Groveler.PhotoMediaFolder) ? photoMediaFolder : ClientConfiguration.Groveler.PhotoMediaFolder;
            management.PodcastMediaFolder = string.IsNullOrEmpty(ClientConfiguration.Groveler.PodcastMediaFolder) ? podcastMediaFolder : ClientConfiguration.Groveler.PodcastMediaFolder;
            management.SaveMonitoredFolders(false);
            _proxyDefaultPathsComplete = true;
        }

        public void InitializeDefaultPaths()
        {
            HRESULT knownFolders = ZuneApplication.ZuneLibrary.GetKnownFolders(out var music, out var videos, out var pictures, out var podcasts, out string[] _, out var ripFolder, out var videoMediaFolder, out var photoMediaFolder, out var podcastMediaFolder, out string _);
            if (ClientConfiguration.Groveler.MonitoredAudioFolders == null)
                ClientConfiguration.Groveler.MonitoredAudioFolders = music;
            if (ClientConfiguration.Groveler.MonitoredPhotoFolders == null)
                ClientConfiguration.Groveler.MonitoredPhotoFolders = pictures;
            if (ClientConfiguration.Groveler.MonitoredVideoFolders == null)
                ClientConfiguration.Groveler.MonitoredVideoFolders = videos;
            if (ClientConfiguration.Groveler.MonitoredPodcastFolders == null)
                ClientConfiguration.Groveler.MonitoredPodcastFolders = podcasts;
            if (string.IsNullOrEmpty(ClientConfiguration.Groveler.RipDirectory) && !string.IsNullOrEmpty(ripFolder))
                ClientConfiguration.Groveler.RipDirectory = ripFolder;
            if (string.IsNullOrEmpty(ClientConfiguration.Groveler.VideoMediaFolder) && !string.IsNullOrEmpty(videoMediaFolder))
                ClientConfiguration.Groveler.VideoMediaFolder = videoMediaFolder;
            if (string.IsNullOrEmpty(ClientConfiguration.Groveler.PhotoMediaFolder) && !string.IsNullOrEmpty(photoMediaFolder))
                ClientConfiguration.Groveler.PhotoMediaFolder = photoMediaFolder;
            if (!string.IsNullOrEmpty(ClientConfiguration.Groveler.PodcastMediaFolder) || string.IsNullOrEmpty(podcastMediaFolder))
                return;
            ClientConfiguration.Groveler.PodcastMediaFolder = podcastMediaFolder;
        }

        public void UpdateNSS()
        {
            var hmeSettings = new HMESettings();
            if (((HRESULT)hmeSettings.Init()).IsError || !hmeSettings.VelaSharingEnabled)
                return;
            hmeSettings.EnableSharingForUser();
            hmeSettings.SetSharingEnabledForMediaType(EMediaTypes.eMediaTypeAudio, true);
            hmeSettings.SetSharingEnabledForMediaType(EMediaTypes.eMediaTypeImage, false);
            hmeSettings.SetSharingEnabledForMediaType(EMediaTypes.eMediaTypeVideo, false);
        }

        public int RenderPromptInterval
        {
            get => _renderPromptInterval;
            set
            {
                if (_renderPromptInterval == value)
                    return;
                _renderPromptInterval = value;
                FirePropertyChanged(nameof(RenderPromptInterval));
            }
        }

        public Timer RenderPromptTimer
        {
            get
            {
                if (_renderPromptTimer == null)
                {
                    _renderPromptTimer = new Timer(this);
                    _renderPromptTimer.Interval = RenderPromptInterval;
                    _renderPromptTimer.AutoRepeat = false;
                    _renderPromptTimer.Tick += RenderPromptTimeout;
                }
                return _renderPromptTimer;
            }
        }

        private void RenderPromptTimeout(object sender, EventArgs e)
        {
            if (Application.RenderingType == RenderingType.GDI)
                return;

            Win32MessageBox.Show(
                Shell.LoadString(StringId.IDS_RENDER_PROMPT),
                Shell.LoadString(StringId.IDS_RENDER_PROMPT_CAPTION),
                Win32MessageBoxType.MB_YESNO | Win32MessageBoxType.MB_ICONQUESTION,
                Callback);
            return;
            
            void Callback(object args)
            {
                switch ((int)args)
                {
                    case 6:
                        RenderPromptInterval = RenderPromptIntervalPostValue;
                        break;
                    case 7:
                        ClientConfiguration.GeneralSettings.RenderingType = 0;
                        Win32MessageBox.Show(
                            Shell.LoadString(StringId.IDS_RENDER_PROMPT_RESTART),
                            Shell.LoadString(StringId.IDS_RENDER_PROMPT_CAPTION),
                            Win32MessageBoxType.MB_ICONASTERISK, _ => Application.Window.Close());
                        break;
                }
            }
        }
    }
}
