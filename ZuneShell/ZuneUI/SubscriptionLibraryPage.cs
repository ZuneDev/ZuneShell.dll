// Decompiled with JetBrains decompiler
// Type: ZuneUI.SubscriptionLibraryPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Subscription;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace ZuneUI
{
    public abstract class SubscriptionLibraryPage : LibraryPage
    {
        private SubscriptionSeriesPanel m_seriesPanel;
        private SubscriptionEpisodePanel m_episodePanel;
        private LibraryPanel m_detailsPanel;
        private SubscriptionManager m_subscriptionManager;
        private int m_selectedSeriesId;
        private int m_selectedEpisodeId;
        private Command m_seriesClicked = new Command();
        private Command m_episodeClicked = new Command();
        protected MenuItemCommand m_markAllAsPlayed;
        protected MenuItemCommand m_markAllAsUnplayed;

        public SubscriptionSeriesPanel SeriesPanel
        {
            get => this.m_seriesPanel;
            set
            {
                if (value == this.m_seriesPanel)
                    return;
                this.m_seriesPanel = value;
                this.FirePropertyChanged(nameof(SeriesPanel));
            }
        }

        public SubscriptionEpisodePanel EpisodePanel
        {
            get => this.m_episodePanel;
            set
            {
                if (value == this.m_episodePanel)
                    return;
                this.m_episodePanel = value;
                this.FirePropertyChanged(nameof(EpisodePanel));
            }
        }

        public LibraryPanel DetailsPanel
        {
            get => this.m_detailsPanel;
            set
            {
                if (value == this.m_detailsPanel)
                    return;
                this.m_detailsPanel = value;
                this.FirePropertyChanged(nameof(DetailsPanel));
            }
        }

        public int SeriesState
        {
            get
            {
                int num = 1;
                if (this.m_seriesPanel.SelectedItem is DataProviderObject selectedItem)
                    num = (int)selectedItem.GetProperty(nameof(SeriesState));
                return num;
            }
            set => this.FirePropertyChanged(nameof(SeriesState));
        }

        public SubscriptionLibraryPage(bool showDevice, MediaType mediaType)
          : base(showDevice, mediaType)
        {
            this.UI = this.LandUI;
            this.m_selectedSeriesId = -1;
            this.m_selectedEpisodeId = -1;
            this.m_subscriptionManager = SubscriptionManager.Instance;
        }

        public MenuItemCommand MarkAllAsPlayed => this.m_markAllAsPlayed;

        public MenuItemCommand MarkAllAsUnplayed => this.m_markAllAsUnplayed;

        protected override void OnNavigatedToWorker()
        {
            if (this.NavigationArguments != null)
            {
                int num = -1;
                int mediaId = -1;
                if (this.NavigationArguments.Contains("SeriesLibraryId"))
                    num = (int)this.NavigationArguments["SeriesLibraryId"];
                if (this.NavigationArguments.Contains("EpisodeLibraryId"))
                    mediaId = (int)this.NavigationArguments["EpisodeLibraryId"];
                if (num <= 0 && mediaId > 0)
                    num = PlaylistManager.GetFieldValue(mediaId, this.SeriesListType, 311, -1);
                this.SelectedSeriesId = num;
                this.SelectedEpisodeId = mediaId;
                this.NavigationArguments = null;
            }
            base.OnNavigatedToWorker();
        }

        public override IPageState SaveAndRelease()
        {
            this.m_seriesPanel.Release();
            this.m_episodePanel.Release();
            this.m_seriesPanel.SelectedItem = null;
            this.m_episodePanel.SelectedItem = null;
            return base.SaveAndRelease();
        }

        public int LastSelectedSeriesIndex
        {
            get => ClientConfiguration.Series.PodcastLastSelectedSeriesIndex;
            set => ClientConfiguration.Series.PodcastLastSelectedSeriesIndex = value;
        }

        protected abstract string LandUI { get; }

        protected abstract EMediaTypes SeriesMediaType { get; }

        protected abstract EListType SeriesListType { get; }

        protected abstract StringId SubscriptionErrorStringId { get; }

        public void OpenOfficialWebSite(string link)
        {
            if (string.IsNullOrEmpty(link))
                return;
            try
            {
                Uri result;
                Uri.TryCreate(link, UriKind.Absolute, out result);
                if (!(result != null) || !(result.Scheme == Uri.UriSchemeHttp) && !(result.Scheme == Uri.UriSchemeHttps))
                    return;
                Process.Start(link);
            }
            catch (Win32Exception ex)
            {
            }
        }

        public void Unsubscribe(int seriesId, bool deleteContent)
        {
            HRESULT hresult = this.m_subscriptionManager.Unsubscribe(seriesId, this.SeriesMediaType, deleteContent);
            if (hresult.IsSuccess)
                this.SeriesState = 1;
            else
                ErrorDialogInfo.Show(hresult.Int, Shell.LoadString(this.SubscriptionErrorStringId));
        }

        public void Resubscribe(int seriesId)
        {
            HRESULT hresult = this.m_subscriptionManager.Subscribe(seriesId, this.SeriesMediaType);
            if (hresult.IsSuccess)
                this.SeriesState = 0;
            else
                ErrorDialogInfo.Show(hresult.Int, Shell.LoadString(this.SubscriptionErrorStringId));
        }

        public SeriesSettings GetSeriesSettings(int seriesId) => new SeriesSettings(this.m_subscriptionManager, seriesId);

        public bool IsSubscribed(DataProviderObject series)
        {
            bool isSubscribed = false;
            if (series != null)
            {
                string property = (string)series.GetProperty("FeedUrl");
                if (property != null)
                    this.m_subscriptionManager.FindByUrl(property, this.SeriesMediaType, out int _, out isSubscribed);
            }
            return isSubscribed;
        }

        public bool IsSubscribed() => this.IsSubscribed(this.m_seriesPanel.SelectedItem as DataProviderObject);

        public static Guid GetZuneMediaId(int seriesId, EListType listType)
        {
            Guid guid = Guid.Empty;
            if (seriesId > 0)
                guid = PlaylistManager.GetFieldValue(seriesId, listType, 451, Guid.Empty);
            return guid;
        }

        public int SelectedSeriesId
        {
            get => this.m_selectedSeriesId;
            set
            {
                if (this.m_selectedSeriesId == value)
                    return;
                this.SelectedEpisodeId = -1;
                if (this.EpisodePanel != null)
                {
                    this.EpisodePanel.SelectedItem = null;
                    this.EpisodePanel.SelectedLibraryIds.Clear();
                }
                this.m_selectedSeriesId = value;
                this.FirePropertyChanged(nameof(SelectedSeriesId));
            }
        }

        public Command SeriesClicked => this.m_seriesClicked;

        public int SelectedEpisodeId
        {
            get => this.m_selectedEpisodeId;
            set
            {
                if (this.m_selectedEpisodeId == value)
                    return;
                this.m_selectedEpisodeId = value;
                this.FirePropertyChanged(nameof(SelectedEpisodeId));
            }
        }

        public Command EpisodeClicked => this.m_episodeClicked;
    }
}
