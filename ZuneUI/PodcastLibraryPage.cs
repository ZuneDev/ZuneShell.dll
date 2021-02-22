// Decompiled with JetBrains decompiler
// Type: ZuneUI.PodcastLibraryPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;

namespace ZuneUI
{
    public class PodcastLibraryPage : SubscriptionLibraryPage
    {
        public PodcastLibraryPage()
          : this(false)
        {
        }

        protected override string LandUI => "res://ZuneShellResources!PodcastLibrary.uix#PodcastLibrary";

        protected override EMediaTypes SeriesMediaType => EMediaTypes.eMediaTypePodcastSeries;

        protected override EListType SeriesListType => EListType.ePodcastEpisodeList;

        protected override StringId SubscriptionErrorStringId => StringId.IDS_PODCAST_SUBSCRIPTION_ERROR;

        public static Guid GetZuneMediaId(int seriesId) => SubscriptionLibraryPage.GetZuneMediaId(seriesId, EListType.ePodcastList);

        public PodcastLibraryPage(bool showDevice)
          : base(showDevice, MediaType.Podcast)
        {
            this.UIPath = "Collection\\Podcasts";
            if (showDevice)
            {
                this.PivotPreference = Shell.MainFrame.Device.Podcasts;
                Deviceland.InitDevicePage((ZunePage)this);
            }
            else
                this.PivotPreference = Shell.MainFrame.Collection.Podcasts;
            this.IsRootPage = true;
            this.SeriesPanel = new SubscriptionSeriesPanel((SubscriptionLibraryPage)this);
            this.EpisodePanel = new SubscriptionEpisodePanel((SubscriptionLibraryPage)this);
            if (!showDevice)
                this.DetailsPanel = (LibraryPanel)new SubscriptionDetailsPanel((SubscriptionLibraryPage)this);
            this.m_markAllAsPlayed = new MenuItemCommand(Shell.LoadString(StringId.IDS_PODCAST_MARK_AS_PLAYED_MENUITEM));
            this.m_markAllAsUnplayed = new MenuItemCommand(Shell.LoadString(StringId.IDS_PODCAST_MARK_AS_UNPLAYED_MENUITEM));
            this.TransportControlStyle = TransportControlStyle.Music;
            this.PlaybackContext = PlaybackContext.LibraryPodcast;
            this.ShowPlaylistIcon = false;
        }

        public static void FindInCollection(int seriesId) => PodcastLibraryPage.FindInCollection(seriesId, -1);

        public static void FindInCollection(int seriesId, int libraryId)
        {
            if (seriesId <= 0 && libraryId <= 0)
                return;
            Hashtable hashtable = new Hashtable();
            if (seriesId > 0)
                hashtable.Add((object)"SeriesLibraryId", (object)seriesId);
            if (libraryId > 0)
                hashtable.Add((object)"EpisodeLibraryId", (object)libraryId);
            ZuneShell.DefaultInstance.Execute("Collection\\Podcasts", (IDictionary)hashtable);
        }
    }
}
