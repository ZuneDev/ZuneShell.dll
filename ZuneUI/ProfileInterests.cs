// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfileInterests
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;
using ZuneXml;

namespace ZuneUI
{
    public class ProfileInterests : NotifyPropertyChangedImpl
    {
        private const int c_maxIterests = 4;
        private IList _recentPlays;
        private IList _favorites;
        private IList _topArtists;
        private ArrayList _topAlbums;

        public ProfileInterests() => this._topAlbums = new ArrayList(4);

        public IList RecentPlays
        {
            get => this._recentPlays;
            set
            {
                if (this._recentPlays == value)
                    return;
                if (this._recentPlays == null)
                {
                    this._recentPlays = value;
                    this.AddNewTopAlbums(ProfileCategories.RecentlyPlayed, value);
                }
                else
                {
                    this._recentPlays = value;
                    this.RecreateTopAlbums();
                }
                this.FirePropertyChanged(nameof(RecentPlays));
            }
        }

        public IList Favorites
        {
            get => this._favorites;
            set
            {
                if (this._favorites == value)
                    return;
                if (this._favorites == null)
                {
                    this._favorites = value;
                    this.AddNewTopAlbums(ProfileCategories.Favorites, value);
                }
                else
                {
                    this._favorites = value;
                    this.RecreateTopAlbums();
                }
                this.FirePropertyChanged(nameof(Favorites));
            }
        }

        public IList TopArtists
        {
            get => this._topArtists;
            set
            {
                if (this._topArtists == value)
                    return;
                if (this._topArtists == null)
                {
                    this._topArtists = value;
                    this.AddNewTopAlbums(ProfileCategories.TopArtists, value);
                }
                else
                {
                    this._topArtists = value;
                    this.RecreateTopAlbums();
                }
                this.FirePropertyChanged(nameof(TopArtists));
            }
        }

        public bool TopAlbumsFull => this._topAlbums.Count >= 4;

        public IList TopAlbums => (IList)this._topAlbums;

        private void AddNewTopAlbums(Category category, IList newInterests)
        {
            if (newInterests == null)
                return;
            foreach (object newInterest in (IEnumerable)newInterests)
            {
                if (this.TopAlbumsFull)
                {
                    this.FirePropertyChanged("TopAlbumsFull");
                    break;
                }
                Track track = newInterest as Track;
                if (this.CanAddTrack(track))
                {
                    this._topAlbums.Add((object)new ProfileTrack(category, (DataProviderObject)track));
                    this.FirePropertyChanged("TopAlbums");
                }
            }
        }

        private bool CanAddTrack(Track track)
        {
            if (track == null || string.IsNullOrEmpty(track.AlbumTitle))
                return false;
            Guid albumId = track.AlbumId;
            if (Guid.Empty == albumId)
                return false;
            foreach (ProfileTrack topAlbum in this._topAlbums)
            {
                if (((Track)topAlbum.Track).AlbumId == albumId)
                    return false;
            }
            return true;
        }

        private void RecreateTopAlbums()
        {
            this._topAlbums.Clear();
            this.AddNewTopAlbums(ProfileCategories.RecentlyPlayed, this._recentPlays);
            this.AddNewTopAlbums(ProfileCategories.Favorites, this._favorites);
            this.AddNewTopAlbums(ProfileCategories.TopArtists, this._topArtists);
        }
    }
}
