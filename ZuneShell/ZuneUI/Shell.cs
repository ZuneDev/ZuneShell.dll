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
        private static FeatureEnumUIPathMapping[] s_FeatureEnumUIPathMap = new FeatureEnumUIPathMapping[32]
        {
      new FeatureEnumUIPathMapping("Quickplay", Features.eQuickplay),
      new FeatureEnumUIPathMapping("Social", Features.eSocial),
      new FeatureEnumUIPathMapping("Marketplace\\Fresh", Features.ePicks),
      new FeatureEnumUIPathMapping("Marketplace\\Default", Features.eMarketplace),
      new FeatureEnumUIPathMapping("Marketplace\\MusicVideos", Features.eMusicVideos),
      new FeatureEnumUIPathMapping("Marketplace\\Podcasts", Features.ePodcasts),
      new FeatureEnumUIPathMapping("Marketplace\\Channels", Features.eChannels),
      new FeatureEnumUIPathMapping("Marketplace\\Apps", Features.eGames),
      new FeatureEnumUIPathMapping("Marketplace\\Apps", Features.eApps),
      new FeatureEnumUIPathMapping("Marketplace\\Videos\\Series", Features.eTV),
      new FeatureEnumUIPathMapping("Marketplace\\Videos\\TrailersHome", Features.eMovieTrailers),
      new FeatureEnumUIPathMapping("Marketplace", Features.eMarketplace),
      new FeatureEnumUIPathMapping("Collection\\Podcasts", Features.ePodcasts),
      new FeatureEnumUIPathMapping("Collection\\Channels", Features.eChannels),
      new FeatureEnumUIPathMapping("", Features.eSubscription),
      new FeatureEnumUIPathMapping("", Features.eSubscriptionConfirmation),
      new FeatureEnumUIPathMapping("", Features.eSubscriptionFreeTracks),
      new FeatureEnumUIPathMapping("", Features.eSubscriptionTrial),
      new FeatureEnumUIPathMapping("", Features.eSubscriptionMusicDownload),
      new FeatureEnumUIPathMapping("", Features.eSubscriptionMusicVideoStreaming),
      new FeatureEnumUIPathMapping("", Features.eSignInAvailable),
      new FeatureEnumUIPathMapping("", Features.eFirstLaunchIntroVideo),
      new FeatureEnumUIPathMapping("", Features.eMBRRental),
      new FeatureEnumUIPathMapping("", Features.eMBRPurchase),
      new FeatureEnumUIPathMapping("", Features.eMBRPreview),
      new FeatureEnumUIPathMapping("", Features.eOptIn),
      new FeatureEnumUIPathMapping("", Features.eQuickMixLocal),
      new FeatureEnumUIPathMapping("", Features.eQuickMixZmp),
      new FeatureEnumUIPathMapping("", Features.eMusic),
      new FeatureEnumUIPathMapping("", Features.eMixview),
      new FeatureEnumUIPathMapping("", Features.eSocialMarketplace),
      new FeatureEnumUIPathMapping("", Features.eVideoAllHub)
        };
        private static ExternalLinkMapping[] s_externalLinkMap = new ExternalLinkMapping[19]
        {
      new ExternalLinkMapping("trackID", "Marketplace\\Music\\Artist", "TrackId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.eMusic),
      new ExternalLinkMapping("albumID", "Marketplace\\Music\\Artist", "AlbumId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.eMusic),
      new ExternalLinkMapping("videoID", "Marketplace\\Music\\Artist", "VideoId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.eMusic),
      new ExternalLinkMapping("artistID", "Marketplace\\Music\\Artist", "ArtistId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.eMusic),
      new ExternalLinkMapping("playlistID", "Marketplace\\Music\\Playlist", "PlaylistId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.eMusic),
      new ExternalLinkMapping("hubID", "Marketplace\\Music\\FlexHub", "HubId",  null,  null, Features.eMusic),
      new ExternalLinkMapping("podcastID", "Marketplace\\Podcasts\\Series", "PodcastId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.ePodcasts),
      new ExternalLinkMapping("tvSeriesID", "Marketplace\\Videos\\Series", "SeriesId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.eTV),
      new ExternalLinkMapping("tvEpisodeID", "Marketplace\\Videos\\Series", "EpisodeId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.eTV),
      new ExternalLinkMapping("tvSpecialID", "Marketplace\\Videos\\Short", "ShortId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.eVideos),
      new ExternalLinkMapping("movieID", "Marketplace\\Videos\\Movie", "MovieId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.eVideos),
      new ExternalLinkMapping("channelID", "Marketplace\\Channels\\Channel", "ChannelId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.eChannels),
      new ExternalLinkMapping("appID", "Marketplace\\Apps\\Details\\ZuneHD", "AppId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.eGames),
      new ExternalLinkMapping("phoneAppID", "Marketplace\\Apps\\Details\\WindowsPhone", "AppId", new ConvertArgumentsDelegate(CanonicalizeGuid), new SetClientContextDelegate(SetGuidClientContext), Features.eApps),
      new ExternalLinkMapping("cartItemID", "Marketplace\\Cart", "MessageId",  null,  null, Features.eMusic),
      new ExternalLinkMapping("messageID", "Social\\Inbox", "MessageId",  null,  null, Features.eSocial),
      new ExternalLinkMapping("profile", "Social\\Profile", "ZuneTag", new ConvertArgumentsDelegate(IsValidZuneTag),  null, Features.eSocial),
      new ExternalLinkMapping("myProfile", "Social\\Profile",  null,  null,  null, Features.eSocial),
      new ExternalLinkMapping("purchasePass", "Settings\\Account\\PurchaseSubscription",  null,  null,  null, Features.eSubscription)
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
            this._mainFrame = new MainFrame(this);
            this._searchButton = new Command(this, new EventHandler(this.OnSearchButtonClicked));
            this._playSounds = ClientConfiguration.Shell.Sounds;
            this._compactModeAlwaysOnTop = ClientConfiguration.GeneralSettings.CompactModeAlwaysOnTop;
            this._showNowPlayingBackgroundOnIdleTimeout = ClientConfiguration.Shell.ShowNowPlayingBackgroundOnIdleTimeout;
            this._showWhatsNew = ClientConfiguration.Shell.ShowWhatsNew;
            this.ReadNormalWindowPositionAndSize();
            this.ReadCompactModeWindowPosition();
        }

        public Frame CurrentFrame => this.CurrentExperience?.Frame;

        public Experience CurrentExperience => this._currentNode == null ? null : this._currentNode.Experience;

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
                    int num1 = experiences.Options.IndexOf(currentExperience2);
                    if (num1 != -1)
                    {
                        this.PivotMismatch = false;
                        experiences.ChosenIndex = num1;
                        Choice nodes = currentExperience2.Nodes;
                        int num2 = nodes.Options.IndexOf(_currentNode);
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
                    this._settingsFrame = new SettingsFrame(this);
                return this._settingsFrame;
            }
        }

        public MainFrame MainFrameImpl => this._mainFrame;

        public static SettingsFrame SettingsFrame => ((Shell)DefaultInstance)?.SettingsFrameImpl;

        public static MainFrame MainFrame => ((Shell)DefaultInstance)?.MainFrameImpl;

        public static bool PreRelease => false;

        public static string SessionStartupPath
        {
            get => _sessionStartupPath;
            private set => _sessionStartupPath = value;
        }

        public static void InitializeInstance()
        {
            Shell shell = new Shell();
        }

        public static void NavigateToHomePage()
        {
            try
            {
                string path = ClientConfiguration.Shell.StartupPage;
                if (!IsUIPathEnabled(path))
                    path = MainFrame.Collection.DefaultUIPath;
                SessionStartupPath = path;
                DefaultInstance.Execute(SessionStartupPath, null);
            }
            catch (ArgumentException ex)
            {
                SessionStartupPath = MainFrame.Collection.DefaultUIPath;
                DefaultInstance.Execute(SessionStartupPath, null);
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
                    (!FeatureEnablement.IsFeatureEnabled(Features.eQuickplay) ? MainFrame.Collection : (Command)MainFrame.Quickplay).Invoke(InvokePolicy.Synchronous);
                    ClientConfiguration.Devices.CurrentDeviceID = 0;
                    if (Application.RenderingType == RenderingType.GDI)
                        this.NavigateToPage(new GDILandPage());
                    else
                        this.NavigateToPage(new FirstLaunchLandPage());
                }
                else if (Application.RenderingType != RenderingType.GDI && ClientConfiguration.GeneralSettings.ReevaluateVideoSettings)
                    Application.DeferredInvoke(delegate
                   {
                       this.GdiToD3DRenderPrompt();
                   }, new TimeSpan(0, 0, 3));
                this._haveDoneInitialNavigation = true;
            }
            base.OnPropertyChanged(property);
        }

        private void GdiToD3DRenderPrompt() => Win32MessageBox.Show(LoadString(StringId.IDS_RENDER_PROMPT_AFTER_D3D_SWITCH), LoadString(StringId.IDS_RENDER_PROMPT_CAPTION), Win32MessageBoxType.MB_YESNO | Win32MessageBoxType.MB_ICONQUESTION, args =>
       {
           int num = (int)args;
           ClientConfiguration.GeneralSettings.ReevaluateVideoSettings = false;
           if (num != 7)
               return;
           ClientConfiguration.GeneralSettings.RenderingType = 0;
           Win32MessageBox.Show(LoadString(StringId.IDS_RENDER_PROMPT_RESTART), LoadString(StringId.IDS_RENDER_PROMPT_CAPTION), Win32MessageBoxType.MB_ICONASTERISK, delegate
       {
           Application.Window.Close();
       });
       });

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
            if (!(this.CurrentPage.UI != SearchTemplate))
                return;
            ZunePage page = new ZunePage();
            page.UI = SearchTemplate;
            page.TakeFocusOnNavigate = false;
            Node pivotPreference = this.CurrentPage.PivotPreference;
            SearchResultContextType resultContextType = SearchResultContextType.Undefined;
            if (pivotPreference == MainFrame.Collection.Music || pivotPreference == MainFrame.Marketplace.Music)
                resultContextType = SearchResultContextType.Music;
            else if (pivotPreference == MainFrame.Collection.Videos || pivotPreference == MainFrame.Marketplace.Videos)
                resultContextType = SearchResultContextType.Video;
            else if (pivotPreference == MainFrame.Collection.Podcasts || pivotPreference == MainFrame.Marketplace.Podcasts)
                resultContextType = SearchResultContextType.Podcast;
            else if (pivotPreference == MainFrame.Collection.Channels || pivotPreference == MainFrame.Marketplace.Channels)
                resultContextType = SearchResultContextType.Channel;
            else if (this.CurrentFrame.Experiences.ChosenValue == MainFrame.Social)
                resultContextType = SearchResultContextType.Social;
            else if (pivotPreference == MainFrame.Marketplace.Apps)
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
            return (rgb >> 24 & byte.MaxValue) == 0 ? new WindowColor(243, 239, 241) : new WindowColor(rgb >> 16 & byte.MaxValue, rgb >> 8 & byte.MaxValue, rgb & byte.MaxValue);
        }

        public static int WindowColorToRGB(WindowColor color) => -16777216 | color.R << 16 | color.G << 8 | color.B;

        public static void ShowErrorDialog(int hr, string title) => ErrorDialogInfo.Show(hr, title);

        public static void ShowErrorDialog(int hr, eErrorCondition condition, string title) => ErrorDialogInfo.Show(hr, condition, title);

        public static void ShowErrorDialog(int hr, StringId stringId) => ShowErrorDialog(hr, LoadString(stringId));

        public static void ShowErrorDialog(int hr, eErrorCondition condition, StringId stringId) => ShowErrorDialog(hr, condition, LoadString(stringId));

        public static void ShowErrorDialog(int hr, StringId titleId, StringId descriptionId) => ErrorDialogInfo.Show(hr, LoadString(titleId), LoadString(descriptionId));

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
                    str = LoadString((StringId)System.Enum.Parse(typeof(StringId), stringIdString, true));
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
            return time.Hours != 0 ? string.Format("{4}{1}{0}{2:00}{0}{3:00}", CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator, time.Hours, time.Minutes, time.Seconds, str) : string.Format("{3}{1:0}{0}{2:00}", CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator, time.Minutes, time.Seconds, str);
        }

        public static string TimeSpanToString(TimeSpan time) => TimeSpanToString(time, false);

        public static void DeleteMedia(IList mediaList, bool deleteFileOnDisk)
        {
            ArrayList tempMediaList = new ArrayList(mediaList);
            bool tempDelFileOnDisk = deleteFileOnDisk;
            ThreadPool.QueueUserWorkItem(o =>
           {
               LibraryDataProvider.ActOnItems(tempMediaList, BulkItemAction.DeleteFromLibrary, new DeleteFromLibraryEventArgs()
               {
                   DeleteFromDisk = tempDelFileOnDisk
               });
               foreach (IDatabaseMedia databaseMedia in tempMediaList)
               {
                   if (databaseMedia is LibraryDataProviderListItem providerListItem && providerListItem.TypeName == "MediaFolder")
                   {
                       Management management = DefaultInstance.Management;
                       management.RemoveMonitoredFolder(management.MonitoredPhotoFolders, (string)providerListItem.GetProperty("FolderPath"), true);
                   }
               }
           }, null);
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
                        ErrorDialogInfo.Show(-1072884971, LoadString(StringId.IDS_PODCAST_SUBSCRIPTION_ERROR));
                }
                else
                {
                    if (string.Compare(strA, "navigate", true) != 0)
                        return;
                    ProtocolHandlerNavigate(match.Groups["param1"].Value, match.Groups["value1"]?.Value);
                }
            }
        }

        public static void ProtocolHandlerNavigate(string typeId, string value)
        {
            bool flag = false;
            foreach (ExternalLinkMapping externalLink in s_externalLinkMap)
            {
                if (string.Equals(externalLink.ExternalLinkName, typeId, StringComparison.OrdinalIgnoreCase))
                {
                    if (externalLink.FeatureRequired == Features.eLastFeature || FeatureEnablement.IsFeatureEnabled(externalLink.FeatureRequired))
                    {
                        string convertedValue = value;
                        if (externalLink.ConversionFunction != null && !externalLink.ConversionFunction(value, out convertedValue))
                            return;
                        Hashtable hashtable = null;
                        if (externalLink.ParamName != null)
                        {
                            hashtable = new Hashtable();
                            hashtable[externalLink.ParamName] = convertedValue;
                            hashtable["IsDeepLink"] = true;
                        }
                        DefaultInstance.Execute(externalLink.NavigatePath, hashtable);
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
                                message = LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_VIDEO);
                                break;
                            case Features.ePodcasts:
                                message = LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_PODCAST);
                                break;
                            case Features.eChannels:
                                message = LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_CHANNEL);
                                break;
                            case Features.eGames:
                                message = LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_APPS);
                                break;
                            case Features.eApps:
                                message = LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_APPS);
                                break;
                            case Features.eTV:
                                message = LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_TV);
                                break;
                            case Features.eSubscription:
                                message = LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_SUBSCRIPTION);
                                break;
                            case Features.eMusic:
                                message = LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_MUSIC);
                                break;
                            default:
                                message = LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_GENERAL);
                                break;
                        }
                        MessageBox.Show(LoadString(StringId.IDS_EXTERNAL_LINK_BLOCKED_HEADER), message, null);
                    }
                    flag = true;
                    break;
                }
            }
            int num = flag ? 1 : 0;
        }

        private static bool CanonicalizeGuid(string value, out string canonicalValue)
        {
            canonicalValue = null;
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
            canonicalValue = null;
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
                string format = LoadString(StringId.IDS_MEDIASTORE_TRACK_REFORMATTING);
                if (string.IsNullOrEmpty(fileInfo.Name))
                    ;
                return string.Format(format, fileInfo.Name, path);
            }
            catch (Exception ex)
            {
                return path;
            }
        }

        public static void OpenFolderAndSelectItems(string filePath) => ShellInterop.OpenFolderAndSelectItem(filePath);

        public static string SettingsRegistryPath => "HKEY_CURRENT_USER\\Software\\Microsoft\\Zune\\Shell";

        public static void SaveInt(string keyName, int value) => Registry.SetValue(SettingsRegistryPath, keyName, value);

        public static int GetInt(string keyName, int min, int max, int defaultValue) => string.IsNullOrEmpty(keyName) || (!(Registry.GetValue(SettingsRegistryPath, keyName, defaultValue) is int num) || num < min || num > max) ? defaultValue : num;

        private static void SaveList(string keyName, IList values, ToStringer toString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (object obj in values)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder.Append(';');
                stringBuilder.Append(toString(obj));
            }
            Registry.SetValue(SettingsRegistryPath, keyName, stringBuilder.ToString());
        }

        public static void SaveIntList(string keyName, IList values) => SaveList(keyName, values, value => ((int)value).ToString(NumberFormatInfo.InvariantInfo));

        public static void SaveFloatList(string keyName, IList values) => SaveList(keyName, values, value => ((float)value).ToString(NumberFormatInfo.InvariantInfo));

        private static IList GetList(string keyName, int expectedCount, TryParser tryParse)
        {
            string str = Registry.GetValue(SettingsRegistryPath, keyName, null) as string;
            if (string.IsNullOrEmpty(str))
                return null;
            string[] strArray = str.Split(';');
            if (strArray.Length != expectedCount)
                return null;
            ArrayList arrayList = new ArrayList(expectedCount);
            for (int index = 0; index < expectedCount; ++index)
            {
                object obj;
                if (!tryParse(strArray[index], out obj))
                    return null;
                arrayList.Add(obj);
            }
            return arrayList;
        }

        public static IList GetIntList(string keyName, int expectedCount) => GetList(keyName, expectedCount, (string s, out object value) =>
       {
           int result;
           bool flag = int.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result);
           value = result;
           return flag;
       });

        public static IList GetPositiveIntList(string keyName, int expectedCount)
        {
            IList list = GetIntList(keyName, expectedCount);
            if (list != null)
            {
                foreach (int num in list)
                {
                    if (num <= 0)
                    {
                        list = null;
                        break;
                    }
                }
            }
            return list;
        }

        public static IList GetReorderedIntList(string keyName, int expectedCount)
        {
            IList list = GetIntList(keyName, expectedCount);
            if (list != null)
            {
                BitArray bitArray = new BitArray(expectedCount);
                foreach (int index in list)
                {
                    if (index < 0 || index >= expectedCount || bitArray[index])
                    {
                        list = null;
                        break;
                    }
                    bitArray[index] = true;
                }
            }
            return list;
        }

        public static IList GetFloatList(string keyName, int expectedCount) => GetList(keyName, expectedCount, (string s, out object value) =>
       {
           float result;
           bool flag = float.TryParse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out result);
           value = result;
           return flag;
       });

        public static IList GetPositionList(string keyName, int expectedCount)
        {
            IList list = GetFloatList(keyName, expectedCount);
            if (list != null)
            {
                float num1 = 0.0f;
                foreach (float num2 in list)
                {
                    if (num2 < (double)num1 || num2 > 1.0)
                    {
                        list = null;
                        break;
                    }
                    num1 = num2;
                }
            }
            return list;
        }

        private void ReadNormalWindowPositionAndSize()
        {
            IList intList = GetIntList(regNormalWindowPosition, 4);
            if (intList == null || intList.Count != 4)
                return;
            this._normalWindowPosition = new Point((int)intList[0], (int)intList[1]);
            this._normalWindowSize = new Size((int)intList[2], (int)intList[3]);
        }

        private void SaveNormalWindowPositionAndSize()
        {
            if (this._normalWindowPosition == null || this._normalWindowSize == null)
                return;
            SaveIntList(regNormalWindowPosition, new int[4]
            {
        this._normalWindowPosition.X,
        this._normalWindowPosition.Y,
        this._normalWindowSize.Width,
        this._normalWindowSize.Height
            });
        }

        private void ReadCompactModeWindowPosition()
        {
            IList intList = GetIntList(regCompactModeWindowPosition, 2);
            if (intList == null || intList.Count != 2)
                return;
            this._compactModeWindowPosition = new Point((int)intList[0], (int)intList[1]);
        }

        private void SaveCompactModeWindowPosition()
        {
            if (this._compactModeWindowPosition == null)
                return;
            SaveIntList(regCompactModeWindowPosition, new int[2]
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
                foreach (FeatureEnumUIPathMapping featureEnumUiPath in s_FeatureEnumUIPathMap)
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
            public ConvertArgumentsDelegate ConversionFunction;
            public SetClientContextDelegate ClientContextFunction;
            public Features FeatureRequired;

            public ExternalLinkMapping(
              string externalLinkName,
              string navigatePath,
              string paramName,
              ConvertArgumentsDelegate conversionFunction,
              SetClientContextDelegate clientContextFunction,
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
