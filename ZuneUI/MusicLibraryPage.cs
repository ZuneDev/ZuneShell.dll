// Decompiled with JetBrains decompiler
// Type: ZuneUI.MusicLibraryPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.PerfTrace;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace ZuneUI
{
    public class MusicLibraryPage : LibraryPage
    {
        private ArtistsPanel _artistsPanel;
        private GenresPanel _genresPanel;
        private AlbumsPanel _albumsPanel;
        private TracksPanel _tracksPanel;
        private PlaylistsPanel _playlistsPanel;
        private PlaylistContentsPanel _playlistContentsPanel;
        private Guid _selectedArtistZuneMediaId;
        private string _selectedArtistTitle;
        private IList _selectedArtistIds;
        private IList _selectedGenreIds;
        private IList _selectedAlbumIds;
        private IList _selectedTrackIds;
        private object _selectedPlaylist;
        private string _trackListSort;
        private string _trackListSortAllTracksArtistView;
        private string _trackListSortAllTracksAlbumView;
        private string _trackListSortAllTracksGenreView;
        private string _trackListSortSelectedArtists;
        private string _trackListSortSelectedGenres;
        private string _trackListSortSelectedAlbums;
        private bool _allArtistsSelected;
        private bool _allGenresSelected;
        private bool _allAlbumsSelected;
        private int _artistsCount;
        private int _genresCount;
        private int _albumsCount;
        private Command _albumsChanged;
        private Command _albumEdited;
        private int _albumArtistCount;
        private MusicLibraryView _view;
        private Command _leftItemClicked;
        private Command _albumClicked;
        private Command _createPlaylistCommand;
        private Command _createAutoPlaylistCommand;
        private bool _updatePreferredView;
        private static Command _preferredContentType;
        private static bool _hasContentTypesPersonal;
        private static bool _hasContentTypesProtected;
        private static bool _hasContentTypesZunePass;
        private static Command _showContentTypesAll;
        private static Command _showContentTypesPersonal;
        private static Command _showContentTypesProtected;
        private static Command _showContentTypesZunePass;

        public MusicLibraryPage(bool showDevice)
          : this(showDevice, MusicLibraryView.Invalid)
        {
        }

        public MusicLibraryPage(bool showDevice, MusicLibraryView desiredView)
          : base(showDevice, MediaType.Track)
        {
            this.UI = MusicLibraryPage.LibraryTemplate;
            this.UIPath = "Collection\\Music";
            this._updatePreferredView = true;
            if (showDevice)
            {
                this.PivotPreference = ZuneUI.Shell.MainFrame.Device.Music;
                Deviceland.InitDevicePage((ZunePage)this);
            }
            else
                this.PivotPreference = ZuneUI.Shell.MainFrame.Collection.Music;
            this.IsRootPage = true;
            this.Views = (Choice)new NotifyChoice((IModelItemOwner)this);
            if (!this.ShowDeviceContents)
                this.Views.Options = (IList)new MusicLibraryPage.ViewCommand[5]
                {
          new MusicLibraryPage.ViewCommand((IModelItemOwner) this.Views, ZuneUI.Shell.LoadString(StringId.IDS_COLLECTION_BROWSE), new EventHandler(this.ShowArtistPivot), MusicLibraryView.Artist),
          new MusicLibraryPage.ViewCommand((IModelItemOwner) this.Views, ZuneUI.Shell.LoadString(StringId.IDS_COLLECTION_GENRE), new EventHandler(this.ShowGenrePivot), MusicLibraryView.Genre),
          new MusicLibraryPage.ViewCommand((IModelItemOwner) this.Views, ZuneUI.Shell.LoadString(StringId.IDS_COLLECTION_ALBUM), new EventHandler(this.ShowAlbumPivot), MusicLibraryView.Album),
          new MusicLibraryPage.ViewCommand((IModelItemOwner) this.Views, ZuneUI.Shell.LoadString(StringId.IDS_COLLECTION_LIST), new EventHandler(this.ShowSongPivot), MusicLibraryView.Song),
          new MusicLibraryPage.ViewCommand((IModelItemOwner) this.Views, ZuneUI.Shell.LoadString(StringId.IDS_PLAYLISTS_PIVOT), new EventHandler(this.ShowPlaylistPivot), MusicLibraryView.Playlist)
                };
            else
                this.Views.Options = (IList)new MusicLibraryPage.ViewCommand[2]
                {
          new MusicLibraryPage.ViewCommand((IModelItemOwner) this.Views, ZuneUI.Shell.LoadString(StringId.IDS_COLLECTION_BROWSE), new EventHandler(this.ShowArtistPivot), MusicLibraryView.Artist),
          new MusicLibraryPage.ViewCommand((IModelItemOwner) this.Views, ZuneUI.Shell.LoadString(StringId.IDS_PLAYLISTS_PIVOT), new EventHandler(this.ShowPlaylistPivot), MusicLibraryView.Playlist)
                };
            int num = -1;
            if (desiredView != MusicLibraryView.Invalid)
            {
                for (int index = 0; index < this.Views.Options.Count; ++index)
                {
                    if (desiredView == ((MusicLibraryPage.ViewCommand)this.Views.Options[index]).View)
                    {
                        num = index;
                        break;
                    }
                }
            }
            if (num == -1)
                num = !this.ShowDeviceContents ? ClientConfiguration.Shell.MusicCollectionView : ClientConfiguration.Shell.MusicDeviceView;
            this.Views.ChosenChanged += new EventHandler(this.ViewChanged);
            if (num >= 0 && num < this.Views.Options.Count)
                this.Views.ChosenIndex = num;
            this.ContentTypes = (Choice)new NotifyChoice((IModelItemOwner)this);
            this.ContentTypes.Options = (IList)new ArrayListDataSet((IModelItemOwner)this.ContentTypes);
            this.ShowContentTypes.Value = ClientConfiguration.Shell.ShowContentTypes;
            this.ShowContentTypes.ChosenChanged += new EventHandler(this.ShowContentTypesChanged);
            this.TransportControlStyle = TransportControlStyle.Music;
            this.PlaybackContext = PlaybackContext.Music;
            this._createPlaylistCommand = new Command((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_PLAYLIST_DIALOG_CREATEPLAYLIST), (EventHandler)null);
            this._createAutoPlaylistCommand = new Command((IModelItemOwner)this, ZuneUI.Shell.LoadString(StringId.IDS_CREATEAUTOPLAYLIST_BUTTON), (EventHandler)null);
            this._albumsChanged = new Command((IModelItemOwner)this);
            this._albumEdited = new Command((IModelItemOwner)this);
            this._leftItemClicked = new Command((IModelItemOwner)this);
            this._albumClicked = new Command((IModelItemOwner)this);
        }

        public MusicLibraryView View
        {
            get => this._view;
            private set
            {
                if (this._view == value)
                    return;
                this._view = value;
                this.FirePropertyChanged(nameof(View));
            }
        }

        public IList SelectedArtistIds
        {
            get => this._selectedArtistIds;
            set
            {
                if (this._selectedArtistIds == value)
                    return;
                this._selectedArtistIds = value;
                this.FirePropertyChanged(nameof(SelectedArtistIds));
                this.FirePropertyChanged("SelectedArtistsCount");
            }
        }

        public int SelectedArtistsCount
        {
            get
            {
                if (this.AllArtistsSelected)
                    return this.ArtistsCount;
                return this.SelectedArtistIds == null ? 0 : this.SelectedArtistIds.Count;
            }
        }

        public IList SelectedGenreIds
        {
            get => this._selectedGenreIds;
            set
            {
                if (this._selectedGenreIds == value)
                    return;
                this._selectedGenreIds = value;
                this.FirePropertyChanged(nameof(SelectedGenreIds));
                this.FirePropertyChanged("SelectedGenresCount");
            }
        }

        public int SelectedGenresCount
        {
            get
            {
                if (this.AllGenresSelected)
                    return this.GenresCount;
                return this.SelectedGenreIds == null ? 0 : this.SelectedGenreIds.Count;
            }
        }

        public Guid SelectedArtistZuneMediaId
        {
            get => this._selectedArtistZuneMediaId;
            set
            {
                if (!(this._selectedArtistZuneMediaId != value))
                    return;
                this._selectedArtistZuneMediaId = value;
                this.FirePropertyChanged(nameof(SelectedArtistZuneMediaId));
            }
        }

        public string SelectedArtistTitle
        {
            get => this._selectedArtistTitle;
            set
            {
                if (!(this._selectedArtistTitle != value))
                    return;
                this._selectedArtistTitle = value;
                this.FirePropertyChanged(nameof(SelectedArtistTitle));
            }
        }

        public Command LeftItemClicked => this._leftItemClicked;

        public Command AlbumsChanged => this._albumsChanged;

        public Command AlbumEdited => this._albumEdited;

        public bool AllArtistsSelected
        {
            get => this._allArtistsSelected;
            set
            {
                if (this._allArtistsSelected == value)
                    return;
                this._allArtistsSelected = value;
                this.FirePropertyChanged(nameof(AllArtistsSelected));
                this.FirePropertyChanged("SelectedArtistsCount");
            }
        }

        public bool AllGenresSelected
        {
            get => this._allGenresSelected;
            set
            {
                if (this._allGenresSelected == value)
                    return;
                this._allGenresSelected = value;
                this.FirePropertyChanged(nameof(AllGenresSelected));
                this.FirePropertyChanged("SelectedGenresCount");
            }
        }

        public bool AllAlbumsSelected
        {
            get => this._allAlbumsSelected;
            set
            {
                if (this._allAlbumsSelected == value)
                    return;
                this._allAlbumsSelected = value;
                this.FirePropertyChanged(nameof(AllAlbumsSelected));
                this.FirePropertyChanged("SelectedAlbumsCount");
            }
        }

        public int ArtistsCount
        {
            get => this._artistsCount;
            set
            {
                if (this._artistsCount == value)
                    return;
                this._artistsCount = value;
                this.FirePropertyChanged(nameof(ArtistsCount));
                if (!this.AllArtistsSelected)
                    return;
                this.FirePropertyChanged("SelectedArtistsCount");
            }
        }

        public int GenresCount
        {
            get => this._genresCount;
            set
            {
                if (this._genresCount == value)
                    return;
                this._genresCount = value;
                this.FirePropertyChanged(nameof(GenresCount));
                if (!this.AllGenresSelected)
                    return;
                this.FirePropertyChanged("SelectedGenresCount");
            }
        }

        public int AlbumsCount
        {
            get => this._albumsCount;
            set
            {
                if (this._albumsCount == value)
                    return;
                this._albumsCount = value;
                this.FirePropertyChanged(nameof(AlbumsCount));
                if (!this.AllAlbumsSelected)
                    return;
                this.FirePropertyChanged("SelectedAlbumsCount");
            }
        }

        public int AlbumArtistCount
        {
            get => this._albumArtistCount;
            set
            {
                if (this._albumArtistCount == value)
                    return;
                this._albumArtistCount = value;
                this.FirePropertyChanged(nameof(AlbumArtistCount));
            }
        }

        public IList SelectedAlbumIds
        {
            get => this._selectedAlbumIds;
            set
            {
                if (this._selectedAlbumIds == value)
                    return;
                this._selectedAlbumIds = value;
                this.FirePropertyChanged(nameof(SelectedAlbumIds));
                this.FirePropertyChanged("SelectedAlbumsCount");
            }
        }

        public int SelectedAlbumsCount
        {
            get
            {
                if (this.AllAlbumsSelected)
                    return this.AlbumsCount;
                return this.SelectedAlbumIds == null ? 0 : this.SelectedAlbumIds.Count;
            }
        }

        public string TrackListSort
        {
            get => this._trackListSort;
            set
            {
                if (!(this._trackListSort != value))
                    return;
                this._trackListSort = value;
                this.FirePropertyChanged(nameof(TrackListSort));
            }
        }

        public string TrackListSortAllTracksArtistView
        {
            get => this._trackListSortAllTracksArtistView;
            set
            {
                if (!(this._trackListSortAllTracksArtistView != value))
                    return;
                this._trackListSortAllTracksArtistView = value;
                this.FirePropertyChanged(nameof(TrackListSortAllTracksArtistView));
            }
        }

        public string TrackListSortAllTracksAlbumView
        {
            get => this._trackListSortAllTracksAlbumView;
            set
            {
                if (!(this._trackListSortAllTracksAlbumView != value))
                    return;
                this._trackListSortAllTracksAlbumView = value;
                this.FirePropertyChanged(nameof(TrackListSortAllTracksAlbumView));
            }
        }

        public string TrackListSortAllTracksGenreView
        {
            get => this._trackListSortAllTracksGenreView;
            set
            {
                if (!(this._trackListSortAllTracksGenreView != value))
                    return;
                this._trackListSortAllTracksGenreView = value;
                this.FirePropertyChanged(nameof(TrackListSortAllTracksGenreView));
            }
        }

        public string TrackListSortSelectedArtists
        {
            get => this._trackListSortSelectedArtists;
            set
            {
                if (!(this._trackListSortSelectedArtists != value))
                    return;
                this._trackListSortSelectedArtists = value;
                this.FirePropertyChanged(nameof(TrackListSortSelectedArtists));
            }
        }

        public string TrackListSortSelectedGenres
        {
            get => this._trackListSortSelectedGenres;
            set
            {
                if (!(this._trackListSortSelectedGenres != value))
                    return;
                this._trackListSortSelectedGenres = value;
                this.FirePropertyChanged(nameof(TrackListSortSelectedGenres));
            }
        }

        public string TrackListSortSelectedAlbums
        {
            get => this._trackListSortSelectedAlbums;
            set
            {
                if (!(this._trackListSortSelectedAlbums != value))
                    return;
                this._trackListSortSelectedAlbums = value;
                this.FirePropertyChanged(nameof(TrackListSortSelectedAlbums));
            }
        }

        public string GetSort(bool singleAlbum, MediaType mediaType)
        {
            if (this.View != MusicLibraryView.Artist && this.View != MusicLibraryView.Genre && this.View != MusicLibraryView.Album)
                return this.TrackListSort;
            bool flag = (this.SelectedAlbumIds == null || this.SelectedAlbumIds.Count == 0 || this.AllAlbumsSelected) && (this.SelectedGenreIds == null || this.SelectedGenreIds.Count == 0 || this.AllGenresSelected) && (this.SelectedArtistIds == null || this.SelectedArtistIds.Count == 0 || this.AllArtistsSelected);
            if (mediaType == MediaType.Album && !flag)
                return this.TrackListSortSelectedAlbums;
            if (mediaType == MediaType.Artist && !flag)
                return this.TrackListSortSelectedArtists;
            if (mediaType == MediaType.Genre && !flag)
                return this.TrackListSortSelectedGenres;
            if (this.View == MusicLibraryView.Artist)
                return this.TrackListSortAllTracksArtistView;
            return this.View == MusicLibraryView.Album ? this.TrackListSortAllTracksAlbumView : this.TrackListSortAllTracksGenreView;
        }

        public Command AlbumClicked => this._albumClicked;

        public IList SelectedTrackIds
        {
            get => this._selectedTrackIds;
            set
            {
                if (this._selectedTrackIds == value)
                    return;
                this._selectedTrackIds = value;
                this.FirePropertyChanged(nameof(SelectedTrackIds));
            }
        }

        public Command CreatePlaylistCommand => this._createPlaylistCommand;

        public Command CreateAutoPlaylistCommand => this._createAutoPlaylistCommand;

        public object SelectedPlaylist
        {
            get => this._selectedPlaylist;
            set
            {
                if (this._selectedPlaylist == value)
                    return;
                this._selectedPlaylist = value;
                this.FirePropertyChanged(nameof(SelectedPlaylist));
            }
        }

        private void ViewChanged(object sender, EventArgs e)
        {
            if (this._updatePreferredView)
            {
                if (this.ShowDeviceContents)
                    ClientConfiguration.Shell.MusicDeviceView = this.Views.ChosenIndex;
                else
                    ClientConfiguration.Shell.MusicCollectionView = this.Views.ChosenIndex;
            }
            this._selectedArtistIds = (IList)null;
            this._selectedGenreIds = (IList)null;
            this._selectedAlbumIds = (IList)null;
            this._selectedPlaylist = (object)null;
            this.ShowPlaylistIcon = !this.ShowDeviceContents && ((MusicLibraryPage.ViewCommand)this.Views.ChosenValue).View != MusicLibraryView.Playlist;
        }

        public ArtistsPanel ArtistsPanel
        {
            get
            {
                if (this._artistsPanel == null)
                    this._artistsPanel = new ArtistsPanel(this);
                return this._artistsPanel;
            }
        }

        public GenresPanel GenresPanel
        {
            get
            {
                if (this._genresPanel == null)
                    this._genresPanel = new GenresPanel(this);
                return this._genresPanel;
            }
        }

        public AlbumsPanel AlbumsPanel
        {
            get
            {
                if (this._albumsPanel == null)
                    this._albumsPanel = new AlbumsPanel(this);
                return this._albumsPanel;
            }
        }

        public TracksPanel TracksPanel
        {
            get
            {
                if (this._tracksPanel == null)
                    this._tracksPanel = new TracksPanel(this);
                return this._tracksPanel;
            }
        }

        public PlaylistsPanel PlaylistsPanel
        {
            get
            {
                if (this._playlistsPanel == null)
                {
                    this._playlistsPanel = new PlaylistsPanel(this);
                    if (!this.ShowDeviceContents)
                        this._playlistsPanel.SelectedLibraryIds = (IList)new int[1]
                        {
              PlaylistManager.Instance.DefaultPlaylistId
                        };
                }
                return this._playlistsPanel;
            }
        }

        public PlaylistContentsPanel PlaylistContentsPanel
        {
            get
            {
                if (this._playlistContentsPanel == null)
                    this._playlistContentsPanel = new PlaylistContentsPanel((LibraryPage)this);
                return this._playlistContentsPanel;
            }
        }

        private void ShowContentTypesChanged(object sender, EventArgs e)
        {
            if (!this.ShowContentTypes.Value)
                this.DrmStateMask = ZuneUI.DrmStateMask.All();
            ClientConfiguration.Shell.ShowContentTypes = this.ShowContentTypes.Value;
        }

        private void ContentTypesChanged(object sender, EventArgs e) => MusicLibraryPage._preferredContentType = (Command)this.ContentTypes.ChosenValue;

        public bool HasContentTypesPersonal
        {
            get => MusicLibraryPage._hasContentTypesPersonal;
            set
            {
                if (MusicLibraryPage._hasContentTypesPersonal == value)
                    return;
                MusicLibraryPage._hasContentTypesPersonal = value;
                this.FirePropertyChanged(nameof(HasContentTypesPersonal));
                this.UpdateContentTypesPivots();
            }
        }

        public bool HasContentTypesProtected
        {
            get => MusicLibraryPage._hasContentTypesProtected;
            set
            {
                if (MusicLibraryPage._hasContentTypesProtected == value)
                    return;
                MusicLibraryPage._hasContentTypesProtected = value;
                this.FirePropertyChanged(nameof(HasContentTypesProtected));
                this.UpdateContentTypesPivots();
            }
        }

        public bool HasContentTypesZunePass
        {
            get => MusicLibraryPage._hasContentTypesZunePass;
            set
            {
                if (MusicLibraryPage._hasContentTypesZunePass == value)
                    return;
                MusicLibraryPage._hasContentTypesZunePass = value;
                this.FirePropertyChanged(nameof(HasContentTypesZunePass));
                this.UpdateContentTypesPivots();
            }
        }

        private void UpdateContentTypesPivots()
        {
            if (this.ShowDeviceContents)
                return;
            int num = 0;
            if (MusicLibraryPage._hasContentTypesPersonal)
                ++num;
            if (MusicLibraryPage._hasContentTypesProtected)
                ++num;
            if (MusicLibraryPage._hasContentTypesZunePass)
                ++num;
            this.ContentTypes.Options.Clear();
            if (num > 1)
            {
                if (MusicLibraryPage._showContentTypesAll == null)
                    MusicLibraryPage._showContentTypesAll = new Command((IModelItemOwner)null, ZuneUI.Shell.LoadString(StringId.IDS_COLLECTION_CONTENT_TYPES_ALL), new EventHandler(MusicLibraryPage.ShowContentTypesAll));
                this.ContentTypes.Options.Add((object)MusicLibraryPage._showContentTypesAll);
                if (MusicLibraryPage._hasContentTypesPersonal)
                {
                    if (MusicLibraryPage._showContentTypesPersonal == null)
                        MusicLibraryPage._showContentTypesPersonal = new Command((IModelItemOwner)null, ZuneUI.Shell.LoadString(StringId.IDS_COLLECTION_CONTENT_TYPES_PERSONAL), new EventHandler(MusicLibraryPage.ShowContentTypesPersonal));
                    this.ContentTypes.Options.Add((object)MusicLibraryPage._showContentTypesPersonal);
                }
                if (MusicLibraryPage._hasContentTypesProtected)
                {
                    if (MusicLibraryPage._showContentTypesProtected == null)
                        MusicLibraryPage._showContentTypesProtected = new Command((IModelItemOwner)null, ZuneUI.Shell.LoadString(StringId.IDS_COLLECTION_CONTENT_TYPES_PROTECTED), new EventHandler(MusicLibraryPage.ShowContentTypesProtected));
                    this.ContentTypes.Options.Add((object)MusicLibraryPage._showContentTypesProtected);
                }
                if (MusicLibraryPage._hasContentTypesZunePass)
                {
                    if (MusicLibraryPage._showContentTypesZunePass == null)
                        MusicLibraryPage._showContentTypesZunePass = new Command((IModelItemOwner)null, ZuneUI.Shell.LoadString(StringId.IDS_COLLECTION_CONTENT_TYPES_ZUNEPASS), new EventHandler(MusicLibraryPage.ShowContentTypesZunePass));
                    this.ContentTypes.Options.Add((object)MusicLibraryPage._showContentTypesZunePass);
                }
                if (!this.ContentTypes.Options.Contains((object)MusicLibraryPage._preferredContentType))
                    MusicLibraryPage._preferredContentType = MusicLibraryPage._showContentTypesAll;
                this.ContentTypes.ChosenValue = (object)MusicLibraryPage._preferredContentType;
            }
            this.ShowContentTypes.Value = num > 1;
        }

        private static void ShowContentTypesAll(object sender, EventArgs args)
        {
            if (!(ZuneShell.DefaultInstance.CurrentPage is LibraryPage currentPage))
                return;
            currentPage.DrmStateMask = ZuneUI.DrmStateMask.All();
            SQMLog.Log(SQMDataId.LibraryContentTypeAllClicks, 1);
        }

        private static void ShowContentTypesPersonal(object sender, EventArgs args)
        {
            if (!(ZuneShell.DefaultInstance.CurrentPage is LibraryPage currentPage))
                return;
            currentPage.DrmStateMask = ZuneUI.DrmStateMask.Personal();
            SQMLog.Log(SQMDataId.LibraryContentTypePersonalClicks, 1);
        }

        private static void ShowContentTypesProtected(object sender, EventArgs args)
        {
            if (!(ZuneShell.DefaultInstance.CurrentPage is LibraryPage currentPage))
                return;
            currentPage.DrmStateMask = ZuneUI.DrmStateMask.Protected();
            SQMLog.Log(SQMDataId.LibraryContentTypeProtectedClicks, 1);
        }

        private static void ShowContentTypesZunePass(object sender, EventArgs args)
        {
            if (!(ZuneShell.DefaultInstance.CurrentPage is LibraryPage currentPage))
                return;
            currentPage.DrmStateMask = ZuneUI.DrmStateMask.ZunePass();
            SQMLog.Log(SQMDataId.LibraryContentTypeZunePassClicks, 1);
        }

        private void ShowArtistPivot(object sender, EventArgs args)
        {
            this.View = MusicLibraryView.Artist;
            SQMLog.Log(SQMDataId.ArtistViewClicks, 1);
            ViewTimeLogger.Instance.ViewChanged(SQMDataId.ArtistViewTime);
        }

        private void ShowGenrePivot(object sender, EventArgs args)
        {
            this.View = MusicLibraryView.Genre;
            SQMLog.Log(SQMDataId.GenreViewClicks, 1);
            ViewTimeLogger.Instance.ViewChanged(SQMDataId.GenreViewTime);
        }

        private void ShowAlbumPivot(object sender, EventArgs args)
        {
            this.View = MusicLibraryView.Album;
            SQMLog.Log(SQMDataId.AlbumViewClicks, 1);
            ViewTimeLogger.Instance.ViewChanged(SQMDataId.AlbumViewTime);
        }

        private void ShowSongPivot(object sender, EventArgs args)
        {
            this.View = MusicLibraryView.Song;
            SQMLog.Log(SQMDataId.SongViewClicks, 1);
            ViewTimeLogger.Instance.ViewChanged(SQMDataId.SongViewTime);
        }

        private void ShowPlaylistPivot(object sender, EventArgs args)
        {
            this.View = MusicLibraryView.Playlist;
            SQMLog.Log(SQMDataId.PlaylistViewClicks, 1);
            ViewTimeLogger.Instance.ViewChanged(SQMDataId.PlaylistViewTime);
        }

        protected override void OnNavigatedToWorker()
        {
            if (!this.ShowDeviceContents)
                this.ContentTypes.ChosenChanged += new EventHandler(this.ContentTypesChanged);
            if (this.NavigationArguments != null)
            {
                MusicLibraryPage._preferredContentType = MusicLibraryPage._showContentTypesAll;
                this.DrmStateMask = ZuneUI.DrmStateMask.All();
                if (this.NavigationArguments.Contains((object)"ViewOverrideId"))
                {
                    MusicLibraryView navigationArgument = (MusicLibraryView)this.NavigationArguments[(object)"ViewOverrideId"];
                    for (int index = 0; index < this.Views.Options.Count; ++index)
                    {
                        if (((MusicLibraryPage.ViewCommand)this.Views.Options[index]).View == navigationArgument)
                        {
                            bool updatePreferredView = this._updatePreferredView;
                            this._updatePreferredView = false;
                            this.Views.ChosenIndex = index;
                            this._updatePreferredView = updatePreferredView;
                            break;
                        }
                    }
                }
                this._selectedArtistIds = (IList)null;
                if (this.NavigationArguments.Contains((object)"ArtistLibraryId"))
                    this._selectedArtistIds = (IList)new int[1]
                    {
            (int) this.NavigationArguments[(object) "ArtistLibraryId"]
                    };
                this._selectedGenreIds = (IList)null;
                if (this.NavigationArguments.Contains((object)"GenreLibraryId"))
                    this._selectedGenreIds = (IList)new int[1]
                    {
            (int) this.NavigationArguments[(object) "GenreLibraryId"]
                    };
                this._selectedAlbumIds = (IList)null;
                if (this.NavigationArguments.Contains((object)"AlbumLibraryId"))
                    this._selectedAlbumIds = (IList)new int[1]
                    {
            (int) this.NavigationArguments[(object) "AlbumLibraryId"]
                    };
                this._selectedTrackIds = (IList)null;
                if (this.NavigationArguments.Contains((object)"TrackLibraryId"))
                    this._selectedTrackIds = (IList)new int[1]
                    {
            (int) this.NavigationArguments[(object) "TrackLibraryId"]
                    };
                if (this.NavigationArguments.Contains((object)"PlaylistLibraryId"))
                {
                    this.PlaylistsPanel.SelectedLibraryIds = (IList)new int[1]
                    {
            (int) this.NavigationArguments[(object) "PlaylistLibraryId"]
                    };
                    this.PlaylistContentsPanel.SelectedLibraryIds = this._selectedTrackIds;
                }
                this.NavigationArguments = (IDictionary)null;
            }
            this.UpdateContentTypesPivots();
            base.OnNavigatedToWorker();
        }

        protected override void OnNavigatedAwayWorker(IPage destination)
        {
            ViewTimeLogger.Instance.ViewChanged(SQMDataId.Invalid);
            base.OnNavigatedAwayWorker(destination);
            this.SelectedPlaylist = (object)null;
            if (!this.ShowDeviceContents)
                this.ContentTypes.ChosenChanged -= new EventHandler(this.ContentTypesChanged);
            PlaylistManager.Instance.ValidateDefaultPlaylist();
        }

        public override IPageState SaveAndRelease()
        {
            if (this._artistsPanel != null)
                this._artistsPanel.Release();
            if (this._genresPanel != null)
                this._genresPanel.Release();
            if (this._albumsPanel != null)
                this._albumsPanel.Release();
            if (this._tracksPanel != null)
                this._tracksPanel.Release();
            if (this._playlistsPanel != null)
                this._playlistsPanel.Release();
            if (this._playlistContentsPanel != null)
                this._playlistContentsPanel.Release();
            return base.SaveAndRelease();
        }

        public static void FindInCollection(int artistId, int albumId, int trackId) => MusicLibraryPage.FindInCollection(artistId, albumId, trackId, true);

        public static void FindInCollection(int artistId, int albumId, int trackId, bool selectTrack)
        {
            if (trackId >= 0 && albumId < 0)
                albumId = PlaylistManager.GetFieldValue<int>(trackId, EListType.eTrackList, 11, -1);
            if (albumId >= 0 && artistId < 0)
                artistId = PlaylistManager.GetFieldValue<int>(albumId, EListType.eAlbumList, 78, -1);
            Hashtable hashtable = new Hashtable();
            hashtable.Add((object)"AlbumLibraryId", (object)albumId);
            hashtable.Add((object)"ArtistLibraryId", (object)artistId);
            if (selectTrack)
                hashtable.Add((object)"TrackLibraryId", (object)trackId);
            hashtable.Add((object)"ViewOverrideId", (object)MusicLibraryView.Artist);
            ZuneShell.DefaultInstance.Execute("Collection\\Music\\Default", (IDictionary)hashtable);
        }

        public static void FindPlaylistInCollection(int playlistId) => MusicLibraryPage.FindPlaylistInCollection(playlistId, -1, false);

        public static void FindPlaylistInCollection(int playlistId, int trackId, bool selectTrack)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add((object)"PlaylistLibraryId", (object)playlistId);
            hashtable.Add((object)"ViewOverrideId", (object)MusicLibraryView.Playlist);
            if (selectTrack)
                hashtable.Add((object)"TrackLibraryId", (object)trackId);
            ZuneShell.DefaultInstance.Execute("Collection\\Music\\Default", (IDictionary)hashtable);
        }

        public static Guid GetArtistZuneMediaId(int dbMediaId) => PlaylistManager.GetFieldValue<Guid>(dbMediaId, EListType.eArtistList, 451, Guid.Empty);

        public static void NavigateToPlaylistLand()
        {
            if (!(ZuneShell.DefaultInstance.CurrentPage is MusicLibraryPage currentPage))
            {
                ZuneShell.DefaultInstance.NavigateToPage((ZunePage)new MusicLibraryPage(false, MusicLibraryView.Playlist));
            }
            else
            {
                Choice views = currentPage.Views;
                for (int index = 0; index < views.Options.Count; ++index)
                {
                    if (((MusicLibraryPage.ViewCommand)views.Options[index]).View == MusicLibraryView.Playlist)
                    {
                        views.ChosenIndex = index;
                        break;
                    }
                }
            }
        }

        private static string LibraryTemplate => "res://ZuneShellResources!MusicLibrary.uix#MusicLibrary";

        private class ViewCommand : Command
        {
            private MusicLibraryView _view;

            public ViewCommand(
              IModelItemOwner owner,
              string description,
              EventHandler invokeHandler,
              MusicLibraryView view)
              : base(owner, description, invokeHandler)
            {
                this._view = view;
            }

            public MusicLibraryView View => this._view;

            protected override void OnInvoked()
            {
                Microsoft.Zune.PerfTrace.PerfTrace.TraceUICollectionEvent(UICollectionEvent.MusicLibraryViewCommandInvoked, this.Description);
                base.OnInvoked();
            }
        }
    }
}
