// Decompiled with JetBrains decompiler
// Type: ZuneUI.Shell
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Win32;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UIXControls;

namespace ZuneUI
{
    public class Shell : ZuneShell
    {
        private const int c_minimumWindowWidth = 734;
        private const int c_minimumWindowHeight = 500;
        private static ZuneUI.Shell.FeatureEnumUIPathMapping[] s_FeatureEnumUIPathMap = new ZuneUI.Shell.FeatureEnumUIPathMapping[32]
        {
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Quickplay", Features.eQuickplay),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Social", Features.eSocial),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Marketplace\\Fresh", Features.ePicks),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Marketplace\\Default", Features.eMarketplace),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Marketplace\\MusicVideos", Features.eMusicVideos),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Marketplace\\Podcasts", Features.ePodcasts),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Marketplace\\Channels", Features.eChannels),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Marketplace\\Apps", Features.eGames),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Marketplace\\Apps", Features.eApps),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Marketplace\\Videos\\Series", Features.eTV),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Marketplace\\Videos\\TrailersHome", Features.eMovieTrailers),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Marketplace", Features.eMarketplace),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Collection\\Podcasts", Features.ePodcasts),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("Collection\\Channels", Features.eChannels),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eSubscription),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eSubscriptionConfirmation),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eSubscriptionFreeTracks),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eSubscriptionTrial),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eSubscriptionMusicDownload),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eSubscriptionMusicVideoStreaming),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eSignInAvailable),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eFirstLaunchIntroVideo),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eMBRRental),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eMBRPurchase),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eMBRPreview),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eOptIn),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eQuickMixLocal),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eQuickMixZmp),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eMusic),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eMixview),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eSocialMarketplace),
      new ZuneUI.Shell.FeatureEnumUIPathMapping("", Features.eVideoAllHub)
        };
        private static ZuneUI.Shell.ExternalLinkMapping[] s_externalLinkMap = new ZuneUI.Shell.ExternalLinkMapping[19]
        {
      new ZuneUI.Shell.ExternalLinkMapping("trackID", "Marketplace\\Music\\Artist", "TrackId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.eMusic),
      new ZuneUI.Shell.ExternalLinkMapping("albumID", "Marketplace\\Music\\Artist", "AlbumId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.eMusic),
      new ZuneUI.Shell.ExternalLinkMapping("videoID", "Marketplace\\Music\\Artist", "VideoId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.eMusic),
      new ZuneUI.Shell.ExternalLinkMapping("artistID", "Marketplace\\Music\\Artist", "ArtistId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.eMusic),
      new ZuneUI.Shell.ExternalLinkMapping("playlistID", "Marketplace\\Music\\Playlist", "PlaylistId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.eMusic),
      new ZuneUI.Shell.ExternalLinkMapping("hubID", "Marketplace\\Music\\FlexHub", "HubId", (ZuneUI.Shell.ConvertArgumentsDelegate) null, (ZuneUI.Shell.SetClientContextDelegate) null, Features.eMusic),
      new ZuneUI.Shell.ExternalLinkMapping("podcastID", "Marketplace\\Podcasts\\Series", "PodcastId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.ePodcasts),
      new ZuneUI.Shell.ExternalLinkMapping("tvSeriesID", "Marketplace\\Videos\\Series", "SeriesId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.eTV),
      new ZuneUI.Shell.ExternalLinkMapping("tvEpisodeID", "Marketplace\\Videos\\Series", "EpisodeId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.eTV),
      new ZuneUI.Shell.ExternalLinkMapping("tvSpecialID", "Marketplace\\Videos\\Short", "ShortId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.eVideos),
      new ZuneUI.Shell.ExternalLinkMapping("movieID", "Marketplace\\Videos\\Movie", "MovieId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.eVideos),
      new ZuneUI.Shell.ExternalLinkMapping("channelID", "Marketplace\\Channels\\Channel", "ChannelId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.eChannels),
      new ZuneUI.Shell.ExternalLinkMapping("appID", "Marketplace\\Apps\\Details\\ZuneHD", "AppId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.eGames),
      new ZuneUI.Shell.ExternalLinkMapping("phoneAppID", "Marketplace\\Apps\\Details\\WindowsPhone", "AppId", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.CanonicalizeGuid), new ZuneUI.Shell.SetClientContextDelegate(ZuneUI.Shell.SetGuidClientContext), Features.eApps),
      new ZuneUI.Shell.ExternalLinkMapping("cartItemID", "Marketplace\\Cart", "MessageId", (ZuneUI.Shell.ConvertArgumentsDelegate) null, (ZuneUI.Shell.SetClientContextDelegate) null, Features.eMusic),
      new ZuneUI.Shell.ExternalLinkMapping("messageID", "Social\\Inbox", "MessageId", (ZuneUI.Shell.ConvertArgumentsDelegate) null, (ZuneUI.Shell.SetClientContextDelegate) null, Features.eSocial),
      new ZuneUI.Shell.ExternalLinkMapping("profile", "Social\\Profile", "ZuneTag", new ZuneUI.Shell.ConvertArgumentsDelegate(ZuneUI.Shell.IsValidZuneTag), (ZuneUI.Shell.SetClientContextDelegate) null, Features.eSocial),
      new ZuneUI.Shell.ExternalLinkMapping("myProfile", "Social\\Profile", (string) null, (ZuneUI.Shell.ConvertArgumentsDelegate) null, (ZuneUI.Shell.SetClientContextDelegate) null, Features.eSocial),
      new ZuneUI.Shell.ExternalLinkMapping("purchasePass", "Settings\\Account\\PurchaseSubscription", (string) null, (ZuneUI.Shell.ConvertArgumentsDelegate) null, (ZuneUI.Shell.SetClientContextDelegate) null, Features.eSubscription)
        };
        private Node _currentNode;
        private bool _haveDoneInitialNavigation;
        private Command _searchButton;
        private MainFrame _mainFrame;
        private SettingsFrame _settingsFrame;
        private bool _pivotMismatch;
        private string _backgroundImage;
        private bool _playSounds;
        private bool _compactModeAlwaysOnTop;
        private bool _showWhatsNew;
        private Point _compactModeWindowPosition;
        private Point _normalWindowPosition;
        private Size _normalWindowSize;
        private int _showNowPlayingBackgroundOnIdleTimeout;
        private bool _applicationInitializationIsComplete;
        private static string _sessionStartupPath;

        public Shell()
        {
            this._mainFrame = new MainFrame((IModelItemOwner)this);
            this._searchButton = new Command((IModelItemOwner)this, new EventHandler(this.OnSearchButtonClicked));
            this._playSounds = ClientConfiguration.Shell.Sounds;
            this._compactModeAlwaysOnTop = ClientConfiguration.GeneralSettings.CompactModeAlwaysOnTop;
            this._showNowPlayingBackgroundOnIdleTimeout = ClientConfiguration.Shell.ShowNowPlayingBackgroundOnIdleTimeout;
            this._showWhatsNew = ClientConfiguration.Shell.ShowWhatsNew;
            this.ReadNormalWindowPositionAndSize();
            this.ReadCompactModeWindowPosition();
        }

        public Frame CurrentFrame => this.CurrentExperience?.Frame;

        public Experience CurrentExperience => this._currentNode == null ? (Experience)null : this._currentNode.Experience;

        public bool PivotMismatch
        {
            get => this._pivotMismatch;
            set
            {
                if (this._pivotMismatch == value)
                    return;
                this._pivotMismatch = value;
                this.FirePropertyChanged(nameof(PivotMismatch));
            }
        }

        public Node CurrentNode
        {
            get => this._currentNode;
            set
            {
                if (this._currentNode == value)
                    return;
                Frame currentFrame1 = this.CurrentFrame;
                Experience currentExperience1 = this.CurrentExperience;
                Node currentNode = this._currentNode;
                this._currentNode = value;
                if (currentNode != null)
                    currentNode.IsCurrent = false;
                if (this._currentNode != null)
                {
                    this._currentNode.IsCurrent = true;
                    Experience currentExperience2 = this.CurrentExperience;
                    Choice experiences = this.CurrentFrame.Experiences;
                    int num1 = experiences.Options.IndexOf((object)currentExperience2);
                    if (num1 != -1)
                    {
                        this.PivotMismatch = false;
                        experiences.ChosenIndex = num1;
                        Choice nodes = currentExperience2.Nodes;
                        int num2 = nodes.Options.IndexOf((object)this._currentNode);
                        if (num2 != -1)
                            nodes.ChosenIndex = num2;
                    }
                    else
                        this.PivotMismatch = true;
                }
                Frame currentFrame2 = this.CurrentFrame;
                if (currentFrame1 != currentFrame2)
                {
                    if (currentFrame1 != null)
                        currentFrame1.IsCurrent = false;
                    if (currentFrame2 != null)
                        currentFrame2.IsCurrent = true;
                    this.FirePropertyChanged("CurrentFrame");
                }
                Experience currentExperience3 = this.CurrentExperience;
                if (currentExperience1 != currentExperience3)
                {
                    if (currentExperience1 != null)
                        currentExperience1.IsCurrent = false;
                    if (currentExperience3 != null)
                        currentExperience3.IsCurrent = true;
                    this.FirePropertyChanged("CurrentExperience");
                }
                this.FirePropertyChanged(nameof(CurrentNode));
            }
        }

        public SettingsFrame SettingsFrameImpl
        {
            get
            {
                if (this._settingsFrame == null)
                    this._settingsFrame = new SettingsFrame((IModelItemOwner)this);
                return this._settingsFrame;
            }
        }

        public MainFrame MainFrameImpl => this._mainFrame;

        public static SettingsFrame SettingsFrame => ((ZuneUI.Shell)ZuneShell.DefaultInstance)?.SettingsFrameImpl;

        public static MainFrame MainFrame => ((ZuneUI.Shell)ZuneShell.DefaultInstance)?.MainFrameImpl;

        public static bool PreRelease => false;

        public static string SessionStartupPath
        {
            get => ZuneUI.Shell._sessionStartupPath;
            private set => ZuneUI.Shell._sessionStartupPath = value;
        }

        public static void InitializeInstance()
        {
            ZuneUI.Shell shell = new ZuneUI.Shell();
        }

        public static void NavigateToHomePage()
        {
            try
            {
                string path = ClientConfiguration.Shell.StartupPage;
                if (!ZuneUI.Shell.IsUIPathEnabled(path))
                    path = ZuneUI.Shell.MainFrame.Collection.DefaultUIPath;
                ZuneUI.Shell.SessionStartupPath = path;
                ZuneShell.DefaultInstance.Execute(ZuneUI.Shell.SessionStartupPath, (IDictionary)null);
            }
            catch (ArgumentException ex)
            {
                ZuneUI.Shell.SessionStartupPath = ZuneUI.Shell.MainFrame.Collection.DefaultUIPath;
                ZuneShell.DefaultInstance.Execute(ZuneUI.Shell.SessionStartupPath, (IDictionary)null);
            }
        }

        public static unsafe SecureString MakeSecureString(string value, bool readOnly)
        {
            SecureString secureString;
            if (string.IsNullOrEmpty(value))
            {
                secureString = new SecureString();
            }
            else
            {
                fixed (char* chPtr = value.ToCharArray())
                    secureString = new SecureString(chPtr, value.Length);
            }
            if (readOnly)
                secureString.MakeReadOnly();
            return secureString;
        }

        protected override void OnPropertyChanged(string property)
        {
            if (property == "CurrentPage")
                this.UpdatePivots();
            else if (property == "CommandHandler" && !this._haveDoneInitialNavigation)
            {
                if (ClientConfiguration.FUE.SettingsVersion < ZuneApplication.ZuneCurrentSettingsVersion || Fue.Instance.IsFirstLaunch)
                {
                    (!FeatureEnablement.IsFeatureEnabled(Features.eQuickplay) ? (Command)ZuneUI.Shell.MainFrame.Collection : (Command)ZuneUI.Shell.MainFrame.Quickplay).Invoke(InvokePolicy.Synchronous);
                    ClientConfiguration.Devices.CurrentDeviceID = 0;
                    if (Application.RenderingType == RenderingType.GDI)
                        this.NavigateToPage((ZunePage)new GDILandPage());
                    else
                        this.NavigateToPage((ZunePage)new FirstLaunchLandPage());
                }
                else if (Application.RenderingType != RenderingType.GDI && ClientConfiguration.GeneralSettings.ReevaluateVideoSettings)
                    Application.DeferredInvoke((DeferredInvokeHandler)delegate
                   {
                       this.GdiToD3DRenderPrompt();
                   }, new TimeSpan(0, 0, 3));
                this._haveDoneInitialNavigation = true;
            }
            base.OnPropertyChanged(property);
        }

        private void GdiToD3DRenderPrompt() => Win32MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_RENDER_PROMPT_AFTER_D3D_SWITCH), ZuneUI.Shell.LoadString(StringId.IDS_RENDER_PROMPT_CAPTION), Win32MessageBoxType.MB_YESNO | Win32MessageBoxType.MB_ICONQUESTION, (DeferredInvokeHandler)(args =>
       {
           int num = (int)args;
           ClientConfiguration.GeneralSettings.ReevaluateVideoSettings = false;
           if (num != 7)
               return;
           ClientConfiguration.GeneralSettings.RenderingType = 0;
           Win32MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_RENDER_PROMPT_RESTART), ZuneUI.Shell.LoadString(StringId.IDS_RENDER_PROMPT_CAPTION), Win32MessageBoxType.MB_ICONASTERISK, (DeferredInvokeHandler)delegate
       {
             Application.Window.Close();
         });
       }));

        private void UpdatePivots()
        {
            if (this._mainFrame == null)
                return;
            ZunePage currentPage = this.CurrentPage;
            int num = TraceSwitches.ShellSwitch.TraceVerbose ? 1 : 0;
            if (currentPage.PivotPreference != null)
                this.CurrentNode = currentPage.PivotPreference;
            else
                currentPage.PivotPreference = this.CurrentNode;
        }

        private void OnSearchButtonClicked(object sender, EventArgs e)
        {
            if (!(this.CurrentPage.UI != ZuneUI.Shell.SearchTemplate))
                return;
            ZunePage page = new ZunePage();
            page.UI = ZuneUI.Shell.SearchTemplate;
            page.TakeFocusOnNavigate = false;
            Node pivotPreference = this.CurrentPage.PivotPreference;
            SearchResultContextType resultContextType = SearchResultContextType.Undefined;
            if (pivotPreference == ZuneUI.Shell.MainFrame.Collection.Music || pivotPreference == ZuneUI.Shell.MainFrame.Marketplace.Music)
                resultContextType = SearchResultContextType.Music;
            else if (pivotPreference == ZuneUI.Shell.MainFrame.Collection.Videos || pivotPreference == ZuneUI.Shell.MainFrame.Marketplace.Videos)
                resultContextType = SearchResultContextType.Video;
            else if (pivotPreference == ZuneUI.Shell.MainFrame.Collection.Podcasts || pivotPreference == ZuneUI.Shell.MainFrame.Marketplace.Podcasts)
                resultContextType = SearchResultContextType.Podcast;
            else if (pivotPreference == ZuneUI.Shell.MainFrame.Collection.Channels || pivotPreference == ZuneUI.Shell.MainFrame.Marketplace.Channels)
                resultContextType = SearchResultContextType.Channel;
            else if (this.CurrentFrame.Experiences.ChosenValue == ZuneUI.Shell.MainFrame.Social)
                resultContextType = SearchResultContextType.Social;
            else if (pivotPreference == ZuneUI.Shell.MainFrame.Marketplace.Apps)
                resultContextType = SearchResultContextType.App;
            Search.Instance.UsersContextType = resultContextType;
            this.NavigateToPage(page);
        }

        public Command SearchButton => this._searchButton;

        public string BackgroundImage
        {
            get
            {
                if (this._backgroundImage == null)
                    this._backgroundImage = ClientConfiguration.Shell.BackgroundImage;
                return this._backgroundImage;
            }
            internal set
            {
                if (!(this._backgroundImage != value))
                    return;
                this._backgroundImage = value;
                this.FirePropertyChanged(nameof(BackgroundImage));
            }
        }

        public bool AmbientAnimations => Application.RenderingQuality == RenderingQuality.MaxQuality;

        public bool PlaySounds
        {
            get => this._playSounds;
            internal set
            {
                if (this._playSounds == value)
                    return;
                this._playSounds = value;
                this.FirePropertyChanged(nameof(PlaySounds));
            }
        }

        public bool CompactModeAlwaysOnTop
        {
            get => this._compactModeAlwaysOnTop;
            internal set
            {
                if (this._compactModeAlwaysOnTop == value)
                    return;
                this._compactModeAlwaysOnTop = value;
                this.FirePropertyChanged(nameof(CompactModeAlwaysOnTop));
            }
        }

        public int ShowNowPlayingBackgroundOnIdleTimeout
        {
            get => this._showNowPlayingBackgroundOnIdleTimeout;
            set
            {
                if (this._showNowPlayingBackgroundOnIdleTimeout == value)
                    return;
                this._showNowPlayingBackgroundOnIdleTimeout = value;
                this.FirePropertyChanged(nameof(ShowNowPlayingBackgroundOnIdleTimeout));
            }
        }

        public bool ShowWhatsNew
        {
            get => this._showWhatsNew && FeatureEnablement.IsFeatureEnabled(Features.eSubscriptionFreeTracks);
            set
            {
                if (this._showWhatsNew == value)
                    return;
                this._showWhatsNew = value;
                this.FirePropertyChanged(nameof(ShowWhatsNew));
            }
        }

        public Point CompactModeWindowPosition
        {
            get => this._compactModeWindowPosition;
            set
            {
                if (this._compactModeWindowPosition == value)
                    return;
                this._compactModeWindowPosition = value;
                this.FirePropertyChanged(nameof(CompactModeWindowPosition));
                this.SaveCompactModeWindowPosition();
            }
        }

        public Point NormalWindowPosition
        {
            get => this._normalWindowPosition;
            set
            {
                if (this._normalWindowPosition == value)
                    return;
                this._normalWindowPosition = value;
                this.FirePropertyChanged(nameof(NormalWindowPosition));
                this.SaveNormalWindowPositionAndSize();
            }
        }

        public Size NormalWindowSize
        {
            get => this._normalWindowSize;
            set
            {
                if (this._normalWindowSize == value)
                    return;
                this._normalWindowSize = value;
                this.FirePropertyChanged(nameof(NormalWindowSize));
                this.SaveNormalWindowPositionAndSize();
            }
        }

        public bool ApplicationInitializationIsComplete
        {
            get => this._applicationInitializationIsComplete;
            set
            {
                if (!value || this._applicationInitializationIsComplete)
                    return;
                this._applicationInitializationIsComplete = value;
                this.FirePropertyChanged(nameof(ApplicationInitializationIsComplete));
            }
        }

        public static WindowColor WindowColorFromRGB(int rgb)
        {
            if (ClientConfiguration.Shell.StartupPage.ToLower().Contains("quickplay"))
                return new WindowColor(17, 9, 15);
            return (rgb >> 24 & (int)byte.MaxValue) == 0 ? new WindowColor(243, 239, 241) : new WindowColor(rgb >> 16 & (int)byte.MaxValue, rgb >> 8 & (int)byte.MaxValue, rgb & (int)byte.MaxValue);
        }

        public static int WindowColorToRGB(WindowColor color) => -16777216 | (int)color.R << 16 | (int)color.G << 8 | (int)color.B;

        public static void ShowErrorDialog(int hr, string title) => ErrorDialogInfo.Show(hr, title);

        public static void ShowErrorDialog(int hr, eErrorCondition condition, string title) => ErrorDialogInfo.Show(hr, condition, title);

        public static void ShowErrorDialog(int hr, StringId stringId) => ZuneUI.Shell.ShowErrorDialog(hr, ZuneUI.Shell.LoadString(stringId));

        public static void ShowErrorDialog(int hr, eErrorCondition condition, StringId stringId) => ZuneUI.Shell.ShowErrorDialog(hr, condition, ZuneUI.Shell.LoadString(stringId));

        public static void ShowErrorDialog(int hr, StringId titleId, StringId descriptionId) => ErrorDialogInfo.Show(hr, ZuneUI.Shell.LoadString(titleId), ZuneUI.Shell.LoadString(descriptionId));

        public static string LoadString(StringId stringId)
        {
            if (PhoneBrandingStringMap.Instance.BrandingEnabled)
                stringId = PhoneBrandingStringMap.Instance.TryGetMappedStringId(stringId);
            else if (KinBrandingStringMap.Instance.BrandingEnabled)
                stringId = KinBrandingStringMap.Instance.TryGetMappedStringId(stringId);
            return ZuneLibrary.LoadStringFromResource((uint)stringId);
        }

        public static string LoadString(string stringIdString)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(stringIdString))
            {
                try
                {
                    str = ZuneUI.Shell.LoadString((StringId)System.Enum.Parse(typeof(StringId), stringIdString, true));
                }
                catch (ArgumentException ex)
                {
                }
            }
            return str;
        }

        public static string TimeSpanToString(TimeSpan time, bool prefixWithNegative)
        {
            string str = !prefixWithNegative ? "" : CultureInfo.CurrentCulture.NumberFormat.NegativeSign;
            return time.Hours != 0 ? string.Format("{4}{1}{0}{2:00}{0}{3:00}", (object)CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator, (object)time.Hours, (object)time.Minutes, (object)time.Seconds, (object)str) : string.Format("{3}{1:0}{0}{2:00}", (object)CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator, (object)time.Minutes, (object)time.Seconds, (object)str);
        }

        public static string TimeSpanToString(TimeSpan time) => ZuneUI.Shell.TimeSpanToString(time, false);

        public static void DeleteMedia(IList mediaList, bool deleteFileOnDisk)
        {
            ArrayList tempMediaList = new ArrayList((ICollection)mediaList);
            bool tempDelFileOnDisk = deleteFileOnDisk;
            ThreadPool.QueueUserWorkItem((WaitCallback)(o =>
           {
               LibraryDataProvider.ActOnItems((IList)tempMediaList, BulkItemAction.DeleteFromLibrary, (EventArgs)new DeleteFromLibraryEventArgs()
               {
                   DeleteFromDisk = tempDelFileOnDisk
               });
               foreach (IDatabaseMedia databaseMedia in tempMediaList)
               {
                   if (databaseMedia is LibraryDataProviderListItem providerListItem && providerListItem.TypeName == "MediaFolder")
                   {
                       Management management = ZuneShell.DefaultInstance.Management;
                       management.RemoveMonitoredFolder(management.MonitoredPhotoFolders, (string)providerListItem.GetProperty("FolderPath"), true);
                   }
               }
           }), (object)null);
        }

        public static void ProcessExternalLink(string link)
        {
            if (string.IsNullOrEmpty(link))
                return;
            if (string.Compare(link, "zune://refreshAccount/", true) == 0)
            {
                SignIn.Instance.RefreshAccount();
            }
            else
            {
                Match match = new Regex("^zune:(\\/\\/)?(?<action>\\w+)(\\/)?\\?(?<param1>[^=]+)=?(?<value1>.*)$").Match(link);
                if (!match.Success)
                    return;
                string strA = match.Groups["action"].Value;
                if (string.Compare(strA, "subscribe", true) == 0)
                {
                    string feedTitle = match.Groups["param1"].Value;
                    string stringToEscape = match.Groups["value1"].Value;
                    if (stringToEscape != null && stringToEscape.Length < 32766)
                    {
                        string feedUrl = Uri.EscapeUriString(stringToEscape);
                        SubscribeConfirmDialogHelper.Show(feedTitle, feedUrl);
                    }
                    else
                        ErrorDialogInfo.Show(-1072884971, ZuneUI.Shell.LoadString(StringId.IDS_PODCAST_SUBSCRIPTION_ERROR));
                }
                else
                {
                    if (string.Compare(strA, "navigate", true) != 0)
                        return;
                    ZuneUI.Shell.ProtocolHandlerNavigate(match.Groups["param1"].Value, match.Groups["value1"]?.Value);
                }
            }
        }

        public static void ProtocolHandlerNavigate(string typeId, string value)
        {
            bool flag = false;
            foreach (ZuneUI.Shell.ExternalLinkMapping externalLink in ZuneUI.Shell.s_externalLinkMap)
            {
                if (string.Equals(externalLink.ExternalLinkName, typeId, StringComparison.OrdinalIgnoreCase))
                {
                    if (externalLink.FeatureRequired == Features.eLastFeature || FeatureEnablement.IsFeatureEnabled(externalLink.FeatureRequired))
                    {
                        string convertedValue = value;
                        if (externalLink.ConversionFunction != null && !externalLink.ConversionFunction(value, out convertedValue))
                            return;
                        Hashtable hashtable = (Hashtable)null;
                        if (externalLink.ParamName != null)
                        {
                            hashtable = new Hashtable();
                            hashtable[(object)externalLink.ParamName] = (object)convertedValue;
                            hashtable[(object)"IsDeepLink"] = (object)true;
                        }
                        ZuneShell.DefaultInstance.Execute(externalLink.NavigatePath, (IDictionary)hashtable);
                        if (externalLink.ClientContextFunction != null)
                            externalLink.ClientContextFunction(convertedValue);
                    }
                    else
                    {
                        string empty = string.Empty;
                        string message;
                        switch (externalLink.FeatureRequired)
                        {
                            case Features.eVideos:
                                message = ZuneUI.Shell.LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_VIDEO);
                                break;
                            case Features.ePodcasts:
                                message = ZuneUI.Shell.LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_PODCAST);
                                break;
                            case Features.eChannels:
                                message = ZuneUI.Shell.LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_CHANNEL);
                                break;
                            case Features.eGames:
                                message = ZuneUI.Shell.LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_APPS);
                                break;
                            case Features.eApps:
                                message = ZuneUI.Shell.LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_APPS);
                                break;
                            case Features.eTV:
                                message = ZuneUI.Shell.LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_TV);
                                break;
                            case Features.eSubscription:
                                message = ZuneUI.Shell.LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_SUBSCRIPTION);
                                break;
                            case Features.eMusic:
                                message = ZuneUI.Shell.LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_MUSIC);
                                break;
                            default:
                                message = ZuneUI.Shell.LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_GENERAL);
                                break;
                        }
                        MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_HEADER), message, (EventHandler)null);
                    }
                    flag = true;
                    break;
                }
            }
            int num = flag ? 1 : 0;
        }

        private static bool CanonicalizeGuid(string value, out string canonicalValue)
        {
            canonicalValue = (string)null;
            bool flag = false;
            try
            {
                canonicalValue = new Guid(value).ToString();
                flag = true;
            }
            catch (Exception ex)
            {
            }
            return flag;
        }

        private static void SetGuidClientContext(string value)
        {
            Guid empty = Guid.Empty;
            Guid clientContextEventValue;
            try
            {
                clientContextEventValue = new Guid(value);
            }
            catch (FormatException ex)
            {
                clientContextEventValue = Guid.Empty;
            }
            Download.Instance.ReportClientContextEvent(Microsoft.Zune.Service.EDownloadContextEvent.DeepLink, clientContextEventValue);
        }

        private static bool IsValidZuneTag(string value, out string canonicalValue)
        {
            canonicalValue = (string)null;
            bool flag = true;
            foreach (char c in value)
            {
                if (!char.IsLetterOrDigit(c) && c != ' ')
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
                canonicalValue = value;
            return flag;
        }

        public static string ReformatFolderPathName(string path)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(path);
                string format = ZuneUI.Shell.LoadString(StringId.IDS_MEDIASTORE_TRACK_REFORMATTING);
                if (string.IsNullOrEmpty(fileInfo.Name))
                    ;
                return string.Format(format, (object)fileInfo.Name, (object)path);
            }
            catch (Exception ex)
            {
                return path;
            }
        }

        public static void OpenFolderAndSelectItems(string filePath) => ShellInterop.OpenFolderAndSelectItem(filePath);

        public static string SettingsRegistryPath => "HKEY_CURRENT_USER\\Software\\Microsoft\\Zune\\Shell";

        public static void SaveInt(string keyName, int value) => Registry.SetValue(ZuneUI.Shell.SettingsRegistryPath, keyName, (object)value);

        public static int GetInt(string keyName, int min, int max, int defaultValue) => string.IsNullOrEmpty(keyName) || (!(Registry.GetValue(ZuneUI.Shell.SettingsRegistryPath, keyName, (object)defaultValue) is int num) || num < min || num > max) ? defaultValue : num;

        private static void SaveList(string keyName, IList values, ZuneUI.Shell.ToStringer toString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (object obj in (IEnumerable)values)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder.Append(';');
                stringBuilder.Append(toString(obj));
            }
            Registry.SetValue(ZuneUI.Shell.SettingsRegistryPath, keyName, (object)stringBuilder.ToString());
        }

        public static void SaveIntList(string keyName, IList values) => ZuneUI.Shell.SaveList(keyName, values, (ZuneUI.Shell.ToStringer)(value => ((int)value).ToString((IFormatProvider)NumberFormatInfo.InvariantInfo)));

        public static void SaveFloatList(string keyName, IList values) => ZuneUI.Shell.SaveList(keyName, values, (ZuneUI.Shell.ToStringer)(value => ((float)value).ToString((IFormatProvider)NumberFormatInfo.InvariantInfo)));

        private static IList GetList(string keyName, int expectedCount, ZuneUI.Shell.TryParser tryParse)
        {
            string str = Registry.GetValue(ZuneUI.Shell.SettingsRegistryPath, keyName, (object)null) as string;
            if (string.IsNullOrEmpty(str))
                return (IList)null;
            string[] strArray = str.Split(';');
            if (strArray.Length != expectedCount)
                return (IList)null;
            ArrayList arrayList = new ArrayList(expectedCount);
            for (int index = 0; index < expectedCount; ++index)
            {
                object obj;
                if (!tryParse(strArray[index], out obj))
                    return (IList)null;
                arrayList.Add(obj);
            }
            return (IList)arrayList;
        }

        public static IList GetIntList(string keyName, int expectedCount) => ZuneUI.Shell.GetList(keyName, expectedCount, (ZuneUI.Shell.TryParser)((string s, out object value) =>
       {
           int result;
           bool flag = int.TryParse(s, NumberStyles.Integer, (IFormatProvider)NumberFormatInfo.InvariantInfo, out result);
           value = (object)result;
           return flag;
       }));

        public static IList GetPositiveIntList(string keyName, int expectedCount)
        {
            IList list = ZuneUI.Shell.GetIntList(keyName, expectedCount);
            if (list != null)
            {
                foreach (int num in (IEnumerable)list)
                {
                    if (num <= 0)
                    {
                        list = (IList)null;
                        break;
                    }
                }
            }
            return list;
        }

        public static IList GetReorderedIntList(string keyName, int expectedCount)
        {
            IList list = ZuneUI.Shell.GetIntList(keyName, expectedCount);
            if (list != null)
            {
                BitArray bitArray = new BitArray(expectedCount);
                foreach (int index in (IEnumerable)list)
                {
                    if (index < 0 || index >= expectedCount || bitArray[index])
                    {
                        list = (IList)null;
                        break;
                    }
                    bitArray[index] = true;
                }
            }
            return list;
        }

        public static IList GetFloatList(string keyName, int expectedCount) => ZuneUI.Shell.GetList(keyName, expectedCount, (ZuneUI.Shell.TryParser)((string s, out object value) =>
       {
           float result;
           bool flag = float.TryParse(s, NumberStyles.Float, (IFormatProvider)NumberFormatInfo.InvariantInfo, out result);
           value = (object)result;
           return flag;
       }));

        public static IList GetPositionList(string keyName, int expectedCount)
        {
            IList list = ZuneUI.Shell.GetFloatList(keyName, expectedCount);
            if (list != null)
            {
                float num1 = 0.0f;
                foreach (float num2 in (IEnumerable)list)
                {
                    if ((double)num2 < (double)num1 || (double)num2 > 1.0)
                    {
                        list = (IList)null;
                        break;
                    }
                    num1 = num2;
                }
            }
            return list;
        }

        private void ReadNormalWindowPositionAndSize()
        {
            IList intList = ZuneUI.Shell.GetIntList(ZuneUI.Shell.regNormalWindowPosition, 4);
            if (intList == null || intList.Count != 4)
                return;
            this._normalWindowPosition = new Point((int)intList[0], (int)intList[1]);
            this._normalWindowSize = new Size((int)intList[2], (int)intList[3]);
        }

        private void SaveNormalWindowPositionAndSize()
        {
            if (this._normalWindowPosition == null || this._normalWindowSize == null)
                return;
            ZuneUI.Shell.SaveIntList(ZuneUI.Shell.regNormalWindowPosition, (IList)new int[4]
            {
        this._normalWindowPosition.X,
        this._normalWindowPosition.Y,
        this._normalWindowSize.Width,
        this._normalWindowSize.Height
            });
        }

        private void ReadCompactModeWindowPosition()
        {
            IList intList = ZuneUI.Shell.GetIntList(ZuneUI.Shell.regCompactModeWindowPosition, 2);
            if (intList == null || intList.Count != 2)
                return;
            this._compactModeWindowPosition = new Point((int)intList[0], (int)intList[1]);
        }

        private void SaveCompactModeWindowPosition()
        {
            if (this._compactModeWindowPosition == null)
                return;
            ZuneUI.Shell.SaveIntList(ZuneUI.Shell.regCompactModeWindowPosition, (IList)new int[2]
            {
        this._compactModeWindowPosition.X,
        this._compactModeWindowPosition.Y
            });
        }

        public static bool IsUIPathEnabled(string path)
        {
            bool flag = true;
            int num = 0;
            if (!string.IsNullOrEmpty(path))
            {
                foreach (ZuneUI.Shell.FeatureEnumUIPathMapping featureEnumUiPath in ZuneUI.Shell.s_FeatureEnumUIPathMap)
                {
                    if (path.StartsWith(featureEnumUiPath.Path, StringComparison.InvariantCultureIgnoreCase) && (featureEnumUiPath.Path.Length == path.Length || featureEnumUiPath.Path.Length < path.Length && path[featureEnumUiPath.Path.Length] == '\\') && featureEnumUiPath.Path.Length >= num)
                    {
                        num = featureEnumUiPath.Path.Length;
                        flag = FeatureEnablement.IsFeatureEnabled(featureEnumUiPath.Feature);
                        if (flag)
                            break;
                    }
                }
            }
            return flag;
        }

        private static string LibraryTemplate => "res://ZuneShellResources!Library.uix#Library";

        private static string SearchTemplate => "res://ZuneShellResources!Search.uix#Search";

        private static string regNormalWindowPosition => "NormalWindowPosition";

        private static string regCompactModeWindowPosition => "CompactModeWindowPosition";

        public static int MinimumWindowWidth => 734;

        public static int MinimumWindowHeight => 500;

        public static bool IgnoreAppNavigationsArgs { get; set; }

        private delegate string ToStringer(object value);

        private delegate bool TryParser(string s, out object value);

        private class FeatureEnumUIPathMapping
        {
            public readonly string Path;
            public readonly Features Feature;

            public FeatureEnumUIPathMapping(string path, Features feature)
            {
                this.Path = path;
                this.Feature = feature;
            }
        }

        private delegate bool ConvertArgumentsDelegate(string value, out string convertedValue);

        private delegate void SetClientContextDelegate(string value);

        private class ExternalLinkMapping
        {
            public string ExternalLinkName;
            public string NavigatePath;
            public string ParamName;
            public ZuneUI.Shell.ConvertArgumentsDelegate ConversionFunction;
            public ZuneUI.Shell.SetClientContextDelegate ClientContextFunction;
            public Features FeatureRequired;

            public ExternalLinkMapping(
              string externalLinkName,
              string navigatePath,
              string paramName,
              ZuneUI.Shell.ConvertArgumentsDelegate conversionFunction,
              ZuneUI.Shell.SetClientContextDelegate clientContextFunction,
              Features featureRequired)
            {
                this.ExternalLinkName = externalLinkName;
                this.NavigatePath = navigatePath;
                this.ParamName = paramName;
                this.ConversionFunction = conversionFunction;
                this.ClientContextFunction = clientContextFunction;
                this.FeatureRequired = featureRequired;
            }
        }
    }
}
