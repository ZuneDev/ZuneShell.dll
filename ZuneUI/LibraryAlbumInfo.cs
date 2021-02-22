// Decompiled with JetBrains decompiler
// Type: ZuneUI.LibraryAlbumInfo
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;
using System.Threading;

namespace ZuneUI
{
    public class LibraryAlbumInfo : ModelItem
    {
        private int _trackId;
        private string _trackTitle;
        private string _albumTitle;
        private string _artistName;
        private string _albumArtUrl;
        private Image _thumbnailImage;
        private Guid _zuneMediaId;
        private ICommand _onAsyncUpdateAlbumArtUrlCompleted;

        public LibraryAlbumInfo(
          LibraryPlaybackTrack track,
          int thumbnailMaxWidth,
          int thumbnailMaxHeight)
          : this(track, thumbnailMaxWidth, thumbnailMaxHeight, null)
        {
        }

        public LibraryAlbumInfo(
          LibraryPlaybackTrack track,
          int thumbnailMaxWidth,
          int thumbnailMaxHeight,
          ICommand onAsyncUpdateAlbumArtUrlCompleted)
        {
            this._onAsyncUpdateAlbumArtUrlCompleted = onAsyncUpdateAlbumArtUrlCompleted == null ? new Command(this) : onAsyncUpdateAlbumArtUrlCompleted;
            if (track.MediaType == MediaType.Track)
            {
                this._trackId = track.MediaId;
                this._trackTitle = track.Title;
                int[] columnIndexes1 = new int[2] { 11, 78 };
                object[] fieldValues1 = new object[2]
                {
           -1,
           -1
                };
                ZuneLibrary.GetFieldValues(this._trackId, EListType.eTrackList, columnIndexes1.Length, columnIndexes1, fieldValues1, PlaylistManager.Instance.QueryContext);
                int albumId = (int)fieldValues1[0];
                int iMediaId = (int)fieldValues1[1];
                if (albumId >= 0)
                {
                    int[] columnIndexes2 = new int[2] { 382, 451 };
                    object[] fieldValues2 = new object[2];
                    ZuneLibrary.GetFieldValues(albumId, EListType.eAlbumList, columnIndexes2.Length, columnIndexes2, fieldValues2, PlaylistManager.Instance.QueryContext);
                    this._albumTitle = (string)fieldValues2[0];
                    this._zuneMediaId = GuidHelper.CreateFromString((string)fieldValues2[1]);
                    ThreadPool.QueueUserWorkItem(args =>
                   {
                       string str = LibraryDataProviderItemBase.GetArtUrl(albumId, "Album", false);
                       if (!string.IsNullOrEmpty(str))
                           str = "file://" + str;
                       Application.DeferredInvoke(new DeferredInvokeHandler(this.AsyncUpdateAlbumArtUrl), str);
                   }, null);
                }
                if (iMediaId < 0)
                    return;
                int[] columnIndexes3 = new int[1] { 138 };
                object[] fieldValues3 = new object[1];
                ZuneLibrary.GetFieldValues(iMediaId, EListType.eArtistList, columnIndexes3.Length, columnIndexes3, fieldValues3, PlaylistManager.Instance.QueryContext);
                this._artistName = (string)fieldValues3[0];
            }
            else if (track.MediaType == MediaType.PodcastEpisode)
            {
                this._trackId = track.MediaId;
                this._trackTitle = track.Title;
                int[] columnIndexes1 = new int[2] { 311, 24 };
                object[] fieldValues1 = new object[2]
                {
           -1,
          null
                };
                ZuneLibrary.GetFieldValues(this._trackId, EListType.ePodcastEpisodeList, columnIndexes1.Length, columnIndexes1, fieldValues1, PlaylistManager.Instance.QueryContext);
                int iMediaId = (int)fieldValues1[0];
                this._artistName = (string)fieldValues1[1];
                if (iMediaId < 0)
                    return;
                int[] columnIndexes2 = new int[2] { 344, 17 };
                object[] fieldValues2 = new object[2];
                ZuneLibrary.GetFieldValues(iMediaId, EListType.ePodcastList, columnIndexes2.Length, columnIndexes2, fieldValues2, PlaylistManager.Instance.QueryContext);
                this._albumTitle = (string)fieldValues2[0];
                this._albumArtUrl = (string)fieldValues2[1];
            }
            else
            {
                if (track.MediaType != MediaType.Video)
                    return;
                this._trackId = track.MediaId;
                this._trackTitle = track.Title;
                int[] columnIndexes = new int[3] { 380, 382, 312 };
                object[] fieldValues = new object[3];
                ZuneLibrary.GetFieldValues(this._trackId, EListType.eVideoList, columnIndexes.Length, columnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
                this._artistName = (string)fieldValues[0];
                this._albumTitle = (string)fieldValues[1];
                if (string.IsNullOrEmpty(this._albumTitle))
                    this._albumTitle = (string)fieldValues[2];
                if (Application.RenderingType != RenderingType.GDI)
                    return;
                ThreadPool.QueueUserWorkItem(args =>
               {
                   string artUrl = LibraryDataProviderItemBase.GetArtUrl(this._trackId, "Video", false);
                   if (string.IsNullOrEmpty(artUrl))
                       return;
                   Application.DeferredInvoke(new DeferredInvokeHandler(this.AsyncUpdateThumbnailUrl), "file://" + artUrl);
               }, null);
            }
        }

        private void AsyncUpdateAlbumArtUrl(object args)
        {
            this._albumArtUrl = (string)args;
            this.FirePropertyChanged("AlbumArtUrl");
            this.OnAsyncUpdateAlbumArtUrlCompleted.Invoke();
        }

        public ICommand OnAsyncUpdateAlbumArtUrlCompleted => this._onAsyncUpdateAlbumArtUrlCompleted;

        private void AsyncUpdateThumbnailUrl(object args)
        {
            string source = (string)args;
            Image.RemoveCache(source);
            this._thumbnailImage = new Image(source);
            this.FirePropertyChanged("ThumbnailImage");
        }

        public string TrackTitle => this._trackTitle;

        public string AlbumTitle => this._albumTitle;

        public string ArtistName => this._artistName;

        public string AlbumArtUrl => this._albumArtUrl;

        public Guid ZuneMediaId => this._zuneMediaId;

        public Image ThumbnailImage => this._thumbnailImage;
    }
}
