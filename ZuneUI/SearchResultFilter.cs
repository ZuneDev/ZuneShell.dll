// Decompiled with JetBrains decompiler
// Type: ZuneUI.SearchResultFilter
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class SearchResultFilter
    {
        private static SearchResultFilterCommand _all;
        private static SearchResultFilterCommand _artists;
        private static SearchResultFilterCommand _albums;
        private static SearchResultFilterCommand _tracks;
        private static SearchResultFilterCommand _musicVideos;
        private static SearchResultFilterCommand _tvShows;
        private static SearchResultFilterCommand _movies;
        private static SearchResultFilterCommand _otherVideo;
        private static SearchResultFilterCommand _podcasts;
        private static SearchResultFilterCommand _playlists;
        private static SearchResultFilterCommand _channels;
        private static SearchResultFilterCommand _profile;
        private static SearchResultFilterCommand _windowsPhoneApps;

        public static SearchResultFilterCommand All
        {
            get
            {
                if (SearchResultFilter._all == null)
                {
                    SearchResultFilter._all = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_ALL), SearchResultFilterType.All);
                    SearchResultFilter._all.HasResults = true;
                }
                return SearchResultFilter._all;
            }
        }

        public static SearchResultFilterCommand Artists
        {
            get
            {
                if (SearchResultFilter._artists == null)
                    SearchResultFilter._artists = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_ARTISTS), SearchResultFilterType.Artist);
                return SearchResultFilter._artists;
            }
        }

        public static SearchResultFilterCommand Albums
        {
            get
            {
                if (SearchResultFilter._albums == null)
                    SearchResultFilter._albums = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_ALBUMS), SearchResultFilterType.Album);
                return SearchResultFilter._albums;
            }
        }

        public static SearchResultFilterCommand Tracks
        {
            get
            {
                if (SearchResultFilter._tracks == null)
                    SearchResultFilter._tracks = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_SONGS), SearchResultFilterType.Track);
                return SearchResultFilter._tracks;
            }
        }

        public static SearchResultFilterCommand MusicVideos
        {
            get
            {
                if (SearchResultFilter._musicVideos == null)
                    SearchResultFilter._musicVideos = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_MUSIC_VIDEOS), SearchResultFilterType.MusicVideo);
                return SearchResultFilter._musicVideos;
            }
        }

        public static SearchResultFilterCommand TVShows
        {
            get
            {
                if (SearchResultFilter._tvShows == null)
                    SearchResultFilter._tvShows = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_TV_SHOWS), SearchResultFilterType.TV);
                return SearchResultFilter._tvShows;
            }
        }

        public static SearchResultFilterCommand Movies
        {
            get
            {
                if (SearchResultFilter._movies == null)
                    SearchResultFilter._movies = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_MOVIES), SearchResultFilterType.Movie);
                return SearchResultFilter._movies;
            }
        }

        public static SearchResultFilterCommand OtherVideo
        {
            get
            {
                if (SearchResultFilter._otherVideo == null)
                    SearchResultFilter._otherVideo = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_OTHER_VIDEO), SearchResultFilterType.OtherVideo);
                return SearchResultFilter._otherVideo;
            }
        }

        public static SearchResultFilterCommand Podcasts
        {
            get
            {
                if (SearchResultFilter._podcasts == null)
                    SearchResultFilter._podcasts = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_PODCASTS), SearchResultFilterType.Podcast);
                return SearchResultFilter._podcasts;
            }
        }

        public static SearchResultFilterCommand Playlists
        {
            get
            {
                if (SearchResultFilter._playlists == null)
                    SearchResultFilter._playlists = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_PLAYLISTS), SearchResultFilterType.Playlist);
                return SearchResultFilter._playlists;
            }
        }

        public static SearchResultFilterCommand Channels
        {
            get
            {
                if (SearchResultFilter._channels == null)
                    SearchResultFilter._channels = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_CHANNELS), SearchResultFilterType.Channel);
                return SearchResultFilter._channels;
            }
        }

        public static SearchResultFilterCommand Profile
        {
            get
            {
                if (SearchResultFilter._profile == null)
                    SearchResultFilter._profile = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_SOCIAL_USERS), SearchResultFilterType.Profile);
                return SearchResultFilter._profile;
            }
        }

        public static SearchResultFilterCommand WindowsPhoneApps
        {
            get
            {
                if (SearchResultFilter._windowsPhoneApps == null)
                    SearchResultFilter._windowsPhoneApps = SearchResultFilter.CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_WINDOWS_PHONE_APPS), SearchResultFilterType.WindowsPhoneApp);
                return SearchResultFilter._windowsPhoneApps;
            }
        }

        private static SearchResultFilterCommand CreateSearchResultFilterCommand(
          string description,
          SearchResultFilterType type)
        {
            return new SearchResultFilterCommand((ModelItem)Search.Instance, description, type);
        }
    }
}
