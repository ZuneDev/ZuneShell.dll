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
using System.Collections.Generic;

namespace ZuneUI
{
    public class Fue : ModelItem
    {
        private const int RenderPromptIntervalInitValue = 120000;
        private const int RenderPromptIntervalPostValue = 120000;
        private bool _proxyDefaultPathsComplete;
        private bool _autoFUE;
        private string errorMessage;
        private static Fue singletonInstance;
        private bool fileTypeAssociationsAreSet;
        private int _renderPromptInterval = 120000;
        private Timer _renderPromptTimer;

        public static Fue Instance
        {
            get
            {
                if (Fue.singletonInstance == null)
                    Fue.singletonInstance = new Fue();
                return Fue.singletonInstance;
            }
        }

        public bool IsFirstLaunch
        {
            get
            {
                bool flag = ClientConfiguration.FUE.ShowFirstLaunchVideo && FeatureEnablement.IsFeatureEnabled(Features.eFirstLaunchIntroVideo);
                return ClientConfiguration.FUE.ShowFUE || flag;
            }
        }

        public string ErrorMessage
        {
            get => this.errorMessage;
            set
            {
                this.errorMessage = value;
                ZuneUI.Shell.ShowErrorDialog(0, this.errorMessage);
            }
        }

        public static event EventHandler FUECompleted;

        private Fue()
        {
        }

        public void StartJobs()
        {
            this.InitializeDefaultPaths();
            this.UpdateNSS();
        }

        public bool AutoFUE
        {
            get => this._autoFUE;
            set => this._autoFUE = value;
        }

        public void MigrateLegacyConfiguration() => ClientConfiguration.FUE.SettingsVersion = ZuneApplication.ZuneCurrentSettingsVersion;

        public void SetFileTypeAssociationsAreSet() => this.fileTypeAssociationsAreSet = true;

        public void InitializeQuickplayConfig()
        {
            if (!FeatureEnablement.IsFeatureEnabled(Features.eQuickplay))
                return;
            ClientConfiguration.Shell.StartupPage = ZuneUI.Shell.MainFrame.Quickplay.DefaultUIPath;
            ClientConfiguration.Quickplay.ShowFUE = true;
            ClientConfiguration.Quickplay.CheckUseCount = true;
            ClientConfiguration.Quickplay.UnusedCount = 0;
            ClientConfiguration.Quickplay.FavoredExperience = "";
        }

        public void CompleteFUE()
        {
            ClientConfiguration.FUE.ShowFUE = false;
            ClientConfiguration.FUE.ShowArtistChooser = FeatureEnablement.IsFeatureEnabled(Features.eQuickMixLocal) && (FeatureEnablement.IsFeatureEnabled(Features.eQuickplay) || FeatureEnablement.IsFeatureEnabled(Features.ePicks));
            this.InitializeQuickplayConfig();
            if (!this.fileTypeAssociationsAreSet)
                ZuneShell.DefaultInstance.Management.SaveFileTypesAsDefault();
            ZuneShell.DefaultInstance.NavigateBack();
            SQMLog.LogToStream(SQMDataId.LanguageLocale, ZuneUI.Shell.LoadString(StringId.IDS_ZUNECLIENT_LOCALE));
            SQMLog.Log(SQMDataId.DXModeEnabled, Application.RenderingType == RenderingType.DX9 ? 1 : 0);
            ClientConfiguration.SQM.SQMLaunchIndex = 0;
            this.StartJobs();
            if (Fue.FUECompleted == null)
                return;
            Fue.FUECompleted((object)Fue.Instance, EventArgs.Empty);
        }

        public void CompleteMigration()
        {
            this.InitializeQuickplayConfig();
            ZuneShell.DefaultInstance.NavigateBack();
            if (Fue.FUECompleted == null)
                return;
            Fue.FUECompleted((object)Fue.Instance, EventArgs.Empty);
        }

        public void ProxyDefaultPaths()
        {
            if (this._proxyDefaultPathsComplete)
                return;
            Management management = ZuneShell.DefaultInstance.Management;
            string[] music;
            string[] videos;
            string[] pictures;
            string[] podcasts;
            string ripFolder;
            string videoMediaFolder;
            string photoMediaFolder;
            string podcastMediaFolder;
            HRESULT knownFolders = (HRESULT)ZuneApplication.ZuneLibrary.GetKnownFolders(out music, out videos, out pictures, out podcasts, out string[] _, out ripFolder, out videoMediaFolder, out photoMediaFolder, out podcastMediaFolder, out string _);
            if (ClientConfiguration.Groveler.MonitoredAudioFolders == null)
            {
                for (int index = 0; index < music.Length; ++index)
                    management.MonitoredAudioFolders.Add((object)music[index]);
            }
            if (ClientConfiguration.Groveler.MonitoredPhotoFolders == null)
            {
                for (int index = 0; index < pictures.Length; ++index)
                    management.MonitoredPhotoFolders.Add((object)pictures[index]);
            }
            if (ClientConfiguration.Groveler.MonitoredVideoFolders == null)
            {
                for (int index = 0; index < videos.Length; ++index)
                    management.MonitoredVideoFolders.Add((object)videos[index]);
            }
            if (ClientConfiguration.Groveler.MonitoredPodcastFolders == null)
            {
                for (int index = 0; index < podcasts.Length; ++index)
                    management.MonitoredPodcastFolders.Add((object)podcasts[index]);
            }
            management.MediaFolder = string.IsNullOrEmpty(ClientConfiguration.Groveler.RipDirectory) ? ripFolder : ClientConfiguration.Groveler.RipDirectory;
            management.VideoMediaFolder = string.IsNullOrEmpty(ClientConfiguration.Groveler.VideoMediaFolder) ? videoMediaFolder : ClientConfiguration.Groveler.VideoMediaFolder;
            management.PhotoMediaFolder = string.IsNullOrEmpty(ClientConfiguration.Groveler.PhotoMediaFolder) ? photoMediaFolder : ClientConfiguration.Groveler.PhotoMediaFolder;
            management.PodcastMediaFolder = string.IsNullOrEmpty(ClientConfiguration.Groveler.PodcastMediaFolder) ? podcastMediaFolder : ClientConfiguration.Groveler.PodcastMediaFolder;
            management.SaveMonitoredFolders(false);
            this._proxyDefaultPathsComplete = true;
        }

        public void InitializeDefaultPaths()
        {
            string[] music;
            string[] videos;
            string[] pictures;
            string[] podcasts;
            string ripFolder;
            string videoMediaFolder;
            string photoMediaFolder;
            string podcastMediaFolder;
            HRESULT knownFolders = (HRESULT)ZuneApplication.ZuneLibrary.GetKnownFolders(out music, out videos, out pictures, out podcasts, out string[] _, out ripFolder, out videoMediaFolder, out photoMediaFolder, out podcastMediaFolder, out string _);
            if (ClientConfiguration.Groveler.MonitoredAudioFolders == null)
                ClientConfiguration.Groveler.MonitoredAudioFolders = (IList<string>)music;
            if (ClientConfiguration.Groveler.MonitoredPhotoFolders == null)
                ClientConfiguration.Groveler.MonitoredPhotoFolders = (IList<string>)pictures;
            if (ClientConfiguration.Groveler.MonitoredVideoFolders == null)
                ClientConfiguration.Groveler.MonitoredVideoFolders = (IList<string>)videos;
            if (ClientConfiguration.Groveler.MonitoredPodcastFolders == null)
                ClientConfiguration.Groveler.MonitoredPodcastFolders = (IList<string>)podcasts;
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
            HMESettings hmeSettings = new HMESettings();
            if (((HRESULT)hmeSettings.Init()).IsError || !hmeSettings.VelaSharingEnabled)
                return;
            hmeSettings.EnableSharingForUser();
            hmeSettings.SetSharingEnabledForMediaType(EMediaTypes.eMediaTypeAudio, true);
            hmeSettings.SetSharingEnabledForMediaType(EMediaTypes.eMediaTypeImage, false);
            hmeSettings.SetSharingEnabledForMediaType(EMediaTypes.eMediaTypeVideo, false);
        }

        public int RenderPromptInterval
        {
            get => this._renderPromptInterval;
            set
            {
                if (this._renderPromptInterval == value)
                    return;
                this._renderPromptInterval = value;
                this.FirePropertyChanged(nameof(RenderPromptInterval));
            }
        }

        public Timer RenderPromptTimer
        {
            get
            {
                if (this._renderPromptTimer == null)
                {
                    this._renderPromptTimer = new Timer((IModelItemOwner)this);
                    this._renderPromptTimer.Interval = this.RenderPromptInterval;
                    this._renderPromptTimer.AutoRepeat = false;
                    this._renderPromptTimer.Tick += new EventHandler(this.RenderPromptTimeout);
                }
                return this._renderPromptTimer;
            }
        }

        private void RenderPromptTimeout(object sender, EventArgs e)
        {
            if (Application.RenderingType == RenderingType.GDI)
                return;
            Win32MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_RENDER_PROMPT), ZuneUI.Shell.LoadString(StringId.IDS_RENDER_PROMPT_CAPTION), Win32MessageBoxType.MB_YESNO | Win32MessageBoxType.MB_ICONQUESTION, (DeferredInvokeHandler)(args =>
           {
               switch ((int)args)
               {
                   case 6:
                       this.RenderPromptInterval = 120000;
                       break;
                   case 7:
                       ClientConfiguration.GeneralSettings.RenderingType = 0;
                       Win32MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_RENDER_PROMPT_RESTART), ZuneUI.Shell.LoadString(StringId.IDS_RENDER_PROMPT_CAPTION), Win32MessageBoxType.MB_ICONASTERISK, (DeferredInvokeHandler)(args2 => Application.Window.Close()));
                       break;
               }
           }));
        }
    }
}
