// Decompiled with JetBrains decompiler
// Type: ZuneUI.VideoLibraryPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using System;
using System.Collections;

namespace ZuneUI
{
    public class VideoLibraryPage : LibraryPage
    {
        private VideosPanel _videosPanel;
        private bool _hasSelectedVideo;
        private object _selectedVideo;
        private IList _selectedVideoIds;
        private DataProviderQueryStatus _queryStatus;

        public VideoLibraryPage()
          : this(false, VideoLibraryView.Invalid)
        {
        }

        public VideoLibraryPage(bool showDevice, VideoLibraryView desiredView)
          : base(showDevice, MediaType.Video)
        {
            this.UI = LibraryTemplate;
            this.UIPath = "Collection\\Videos\\Default";
            if (showDevice)
            {
                this.PivotPreference = Shell.MainFrame.Device.Videos;
                Deviceland.InitDevicePage(this);
            }
            else
                this.PivotPreference = Shell.MainFrame.Collection.Videos;
            this.IsRootPage = true;
            if (!this.ShowDeviceContents)
            {
                this.Views = new NotifyChoice(this);
                this.Views.Options = (new ViewCommand[6]
                {
          new ViewCommand( Views, Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_ALL_PIVOT), VideoLibraryView.All),
          new ViewCommand( Views, Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_TV_PIVOT), VideoLibraryView.TV),
          new ViewCommand( Views, Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_MUSIC_PIVOT), VideoLibraryView.Music),
          new ViewCommand( Views, Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_MOVIES_PIVOT), VideoLibraryView.Movies),
          new ViewCommand( Views, Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_OTHER_PIVOT), VideoLibraryView.Other),
          new ViewCommand( Views, Shell.LoadString(StringId.IDS_COLLECTION_VIDEO_PERSONAL_PIVOT), VideoLibraryView.Personal)
                });
                int num = -1;
                if (desiredView != VideoLibraryView.Invalid)
                {
                    for (int index = 0; index < this.Views.Options.Count; ++index)
                    {
                        if (desiredView == ((ViewCommand)this.Views.Options[index]).View)
                        {
                            num = index;
                            break;
                        }
                    }
                }
                if (num == -1)
                    num = ClientConfiguration.Shell.VideoCollectionView;
                this.Views.ChosenChanged += new EventHandler(this.ViewChanged);
                if (num >= 0 && num < this.Views.Options.Count)
                    this.Views.ChosenIndex = num;
            }
            this._videosPanel = new VideosPanel(this);
            this.ShowPlaylistIcon = false;
            this.TransportControlStyle = TransportControlStyle.Video;
            this.PlaybackContext = PlaybackContext.LibraryVideo;
        }

        private void ViewChanged(object sender, EventArgs e)
        {
            ClientConfiguration.Shell.VideoCollectionView = this.Views.ChosenIndex;
            this.FirePropertyChanged("View");
            this.SelectedVideo = null;
            this.SelectedVideoIds = null;
        }

        public VideoLibraryView View
        {
            get => this.ShowDeviceContents ? VideoLibraryView.All : ((ViewCommand)this.Views.ChosenValue).View;
            private set
            {
                if (this.ShowDeviceContents)
                    return;
                foreach (ViewCommand option in Views.Options)
                {
                    if (option.View == value)
                    {
                        this.Views.ChosenValue = option;
                        break;
                    }
                }
            }
        }

        public bool HasSelectedVideo
        {
            get => this._hasSelectedVideo;
            set
            {
                if (this._hasSelectedVideo == value)
                    return;
                this._hasSelectedVideo = value;
                this.FirePropertyChanged(nameof(HasSelectedVideo));
            }
        }

        public object SelectedVideo
        {
            get => this._selectedVideo;
            set
            {
                if (this._selectedVideo == value)
                    return;
                this._selectedVideo = value;
                this.FirePropertyChanged(nameof(SelectedVideo));
            }
        }

        public IList SelectedVideoIds
        {
            get => this._selectedVideoIds;
            set
            {
                if (this._selectedVideoIds == value)
                    return;
                this._selectedVideoIds = value;
                this.FirePropertyChanged(nameof(SelectedVideoIds));
                this.FirePropertyChanged("SelectedVideosCount");
            }
        }

        public DataProviderQueryStatus QueryStatus
        {
            get => this._queryStatus;
            set
            {
                if (this._queryStatus == value)
                    return;
                this._queryStatus = value;
                this.FirePropertyChanged(nameof(QueryStatus));
            }
        }

        public int SelectedVideosCount => this._selectedVideoIds == null ? 0 : this._selectedVideoIds.Count;

        protected override void OnNavigatedToWorker()
        {
            if (this.NavigationArguments != null)
            {
                if (this.NavigationArguments.Contains("ViewOverrideId"))
                    this.View = (VideoLibraryView)this.NavigationArguments["ViewOverrideId"];
                this._selectedVideoIds = null;
                if (this.NavigationArguments.Contains("VideoLibraryId"))
                    this._selectedVideoIds = (new int[1]
                    {
            (int) this.NavigationArguments[ "VideoLibraryId"]
                    });
                this.NavigationArguments = null;
            }
            base.OnNavigatedToWorker();
        }

        public VideosPanel VideosPanel => this._videosPanel;

        protected override void OnNavigatedAwayWorker(IPage destination)
        {
            base.OnNavigatedAwayWorker(destination);
            this.SelectedVideo = null;
        }

        public override IPageState SaveAndRelease()
        {
            this._videosPanel.Release();
            return base.SaveAndRelease();
        }

        public static void FindInCollection(int videoId) => ZuneShell.DefaultInstance.Execute("Collection\\Videos\\Default", new Hashtable()
    {
      {
         "VideoLibraryId",
         videoId
      },
      {
         "ViewOverrideId",
         VideoLibraryView.All
      }
    });

        private static string LibraryTemplate => "res://ZuneShellResources!VideoLibrary.uix#VideoLibrary";

        private class ViewCommand : Command
        {
            private VideoLibraryView _view;

            public ViewCommand(IModelItemOwner owner, string description, VideoLibraryView view)
              : base(owner, description, null)
              => this._view = view;

            public VideoLibraryView View => this._view;
        }
    }
}
