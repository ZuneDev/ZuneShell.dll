// Decompiled with JetBrains decompiler
// Type: ZuneUI.QuickplayPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Subscription;
using Microsoft.Zune.Util;
using System;
using UIXControls;

namespace ZuneUI
{
    public class QuickplayPage : ZunePage
    {
        private bool _startupPage;
        private bool _userInteracted;
        private static bool _showFUE;
        private static bool _createdOnce;
        private bool _hasNavigatedAway;

        public QuickplayPage()
        {
            this.PivotPreference = Shell.MainFrame.Quickplay.Default;
            this.IsRootPage = true;
            this.UI = "res://ZuneShellResources!Quickplay.uix#Quickplay";
            this.UIPath = "Quickplay\\Default";
            this.BackgroundUI = LandBackgroundUI;
            this.ShowSearch = false;
            if (_createdOnce)
                return;
            _createdOnce = true;
            _showFUE = ClientConfiguration.Quickplay.ShowFUE;
            if (!(Shell.SessionStartupPath == Shell.MainFrame.Quickplay.DefaultUIPath))
                return;
            SQMLog.Log(SQMDataId.QuickPlayAsDefaultStarts, 1);
            this._startupPage = true;
        }

        protected override void OnNavigatedAwayWorker(IPage destination)
        {
            if (!this._hasNavigatedAway && this._startupPage)
            {
                if (!this.UserInteracted && !(destination is NowPlayingLand))
                    SQMLog.Log(SQMDataId.QuickPlayAsDefaultSkips, 1);
                if (ClientConfiguration.Quickplay.CheckUseCount)
                {
                    if (this.UserInteracted)
                    {
                        ClientConfiguration.Quickplay.UnusedCount = 0;
                    }
                    else
                    {
                        ZunePage zunePage = destination as ZunePage;
                        string str1 = "";
                        if (zunePage != null && zunePage.PivotPreference != null)
                        {
                            string str2 = "";
                            if (zunePage.PivotPreference.Experience is MarketplaceExperience || zunePage.PivotPreference.Experience is SocialExperience || zunePage.PivotPreference.Experience is CollectionExperience && !(zunePage.PivotPreference.Experience is DeviceExperience))
                                str2 = zunePage.UIPath;
                            if (!string.IsNullOrEmpty(str2))
                            {
                                str1 = str2;
                                int startIndex = str2.IndexOf('\\');
                                if (startIndex != -1)
                                    str1 = str2.Remove(startIndex);
                            }
                        }
                        if (!string.IsNullOrEmpty(str1))
                        {
                            if (ClientConfiguration.Quickplay.FavoredExperience == str1)
                            {
                                ++ClientConfiguration.Quickplay.UnusedCount;
                            }
                            else
                            {
                                ClientConfiguration.Quickplay.FavoredExperience = str1;
                                ClientConfiguration.Quickplay.UnusedCount = 1;
                            }
                            if (ClientConfiguration.Quickplay.UnusedCount >= ClientConfiguration.Quickplay.MaxUnusedCount)
                            {
                                ClientConfiguration.Quickplay.CheckUseCount = false;
                                this.Prompt(zunePage.PivotPreference.Experience);
                            }
                        }
                        else
                            ClientConfiguration.Quickplay.UnusedCount = 0;
                    }
                }
            }
            this._hasNavigatedAway = true;
            base.OnNavigatedAwayWorker(destination);
        }

        public void Prompt(Experience destinationExperience)
        {
            Command yesCommand = new Command(null, Shell.LoadString(StringId.IDS_DIALOG_YES), null);
            yesCommand.Invoked += (sender, args) => ClientConfiguration.Shell.StartupPage = destinationExperience.DefaultUIPath;
            string message = null;
            if (destinationExperience is CollectionExperience)
                message = Shell.LoadString(StringId.IDS_QP_CHANGESTARTUPPAGE_COLLECTION);
            else if (destinationExperience is MarketplaceExperience)
                message = Shell.LoadString(StringId.IDS_QP_CHANGESTARTUPPAGE_MARKETPLACE);
            else if (destinationExperience is SocialExperience)
                message = Shell.LoadString(StringId.IDS_QP_CHANGESTARTUPPAGE_SOCIAL);
            if (string.IsNullOrEmpty(message))
                return;
            MessageBox.ShowYesNo(Shell.LoadString(StringId.IDS_QP_CHANGESTARTUPPAGE_TITLE), message, yesCommand);
        }

        public static void FUEComplete()
        {
            if (!ShowFUE)
                return;
            ClientConfiguration.Quickplay.ShowFUE = false;
        }

        public static DateTime InvalidDate => DateTime.MinValue;

        public static bool PlayPodcastsNewestFirst(int seriesId)
        {
            uint keepEpisodes = (uint)ClientConfiguration.Series.PodcastDefaultKeepEpisodes;
            ESeriesPlaybackOrder playbackOrder = (ESeriesPlaybackOrder)ClientConfiguration.Series.PodcastDefaultPlaybackOrder;
            SubscriptionManager.Instance.GetManagementSettings(seriesId, out keepEpisodes, out playbackOrder);
            return playbackOrder == ESeriesPlaybackOrder.eSeriesPlaybackOrderNewestFirst;
        }

        public static bool ShowFUE => _showFUE;

        public bool UserInteracted
        {
            get => this._userInteracted;
            set => this._userInteracted = value;
        }

        private static string LandBackgroundUI => "res://ZuneShellResources!Quickplay.uix#QuickplayBackground";

        public static int PlaylistTypeMask => 163;
    }
}
