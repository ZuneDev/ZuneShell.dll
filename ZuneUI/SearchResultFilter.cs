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
                if (_all == null)
                {
                    _all = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_ALL), SearchResultFilterType.All);
                    _all.HasResults = true;
                }
                return _all;
            }
        }

        public static SearchResultFilterCommand Artists
        {
            get
            {
                if (_artists == null)
                    _artists = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_ARTISTS), SearchResultFilterType.Artist);
                return _artists;
            }
        }

        public static SearchResultFilterCommand Albums
        {
            get
            {
                if (_albums == null)
                    _albums = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_ALBUMS), SearchResultFilterType.Album);
                return _albums;
            }
        }

        public static SearchResultFilterCommand Tracks
        {
            get
            {
                if (_tracks == null)
                    _tracks = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_SONGS), SearchResultFilterType.Track);
                return _tracks;
            }
        }

        public static SearchResultFilterCommand MusicVideos
        {
            get
            {
                if (_musicVideos == null)
                    _musicVideos = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_MUSIC_VIDEOS), SearchResultFilterType.MusicVideo);
                return _musicVideos;
            }
        }

        public static SearchResultFilterCommand TVShows
        {
            get
            {
                if (_tvShows == null)
                    _tvShows = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_TV_SHOWS), SearchResultFilterType.TV);
                return _tvShows;
            }
        }

        public static SearchResultFilterCommand Movies
        {
            get
            {
                if (_movies == null)
                    _movies = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_MOVIES), SearchResultFilterType.Movie);
                return _movies;
            }
        }

        public static SearchResultFilterCommand OtherVideo
        {
            get
            {
                if (_otherVideo == null)
                    _otherVideo = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_OTHER_VIDEO), SearchResultFilterType.OtherVideo);
                return _otherVideo;
            }
        }

        public static SearchResultFilterCommand Podcasts
        {
            get
            {
                if (_podcasts == null)
                    _podcasts = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_PODCASTS), SearchResultFilterType.Podcast);
                return _podcasts;
            }
        }

        public static SearchResultFilterCommand Playlists
        {
            get
            {
                if (_playlists == null)
                    _playlists = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_PLAYLISTS), SearchResultFilterType.Playlist);
                return _playlists;
            }
        }

        public static SearchResultFilterCommand Channels
        {
            get
            {
                if (_channels == null)
                    _channels = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_CHANNELS), SearchResultFilterType.Channel);
                return _channels;
            }
        }

        public static SearchResultFilterCommand Profile
        {
            get
            {
                if (_profile == null)
                    _profile = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_SOCIAL_USERS), SearchResultFilterType.Profile);
                return _profile;
            }
        }

        public static SearchResultFilterCommand WindowsPhoneApps
        {
            get
            {
                if (_windowsPhoneApps == null)
                    _windowsPhoneApps = CreateSearchResultFilterCommand(Shell.LoadString(StringId.IDS_SEARCH_FILTER_WINDOWS_PHONE_APPS), SearchResultFilterType.WindowsPhoneApp);
                return _windowsPhoneApps;
            }
        }

        private static SearchResultFilterCommand CreateSearchResultFilterCommand(
          string description,
          SearchResultFilterType type)
        {
            return new SearchResultFilterCommand(Search.Instance, description, type);
        }
    }
}
