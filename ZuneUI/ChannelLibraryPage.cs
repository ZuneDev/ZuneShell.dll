// Decompiled with JetBrains decompiler
// Type: ZuneUI.ChannelLibraryPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Playlist;
using System;
using System.Collections;

namespace ZuneUI
{
    public class ChannelLibraryPage : SubscriptionLibraryPage
    {
        private static string _saveAsPlaylistMessageSuccess = Shell.LoadString(StringId.IDS_CHANNEL_SAVE_PLAYLIST_SUCCESS);
        private static string _saveAsPlaylistMessageFailure = Shell.LoadString(StringId.IDS_CHANNEL_SAVE_PLAYLIST_FAILURE);

        public ChannelLibraryPage()
          : this(false)
        {
        }

        public ChannelLibraryPage(bool showDevice)
          : base(showDevice, MediaType.Playlist)
        {
            if (showDevice)
            {
                this.PivotPreference = Shell.MainFrame.Device.Channels;
                Deviceland.InitDevicePage(this);
                this.ShowComputerIcon = ComputerIconState.Show;
            }
            else
            {
                this.DetailsPanel = new ChannelDetailsPanel(this);
                this.PivotPreference = Shell.MainFrame.Collection.Channels;
            }
            this.IsRootPage = true;
            this.UIPath = "Collection\\Channels";
            this.SeriesPanel = new ChannelSeriesPanel(this);
            this.EpisodePanel = new ChannelEpisodePanel(this);
            this.TransportControlStyle = TransportControlStyle.Music;
            this.PlaybackContext = PlaybackContext.Music;
        }

        protected override string LandUI => "res://ZuneShellResources!ChannelLibrary.uix#ChannelLibrary";

        protected override EMediaTypes SeriesMediaType => EMediaTypes.eMediaTypePlaylist;

        protected override EListType SeriesListType => EListType.ePlaylistList;

        protected override StringId SubscriptionErrorStringId => StringId.IDS_PLAYLIST_SUBSCRIPTION_ERROR;

        public static Guid GetZuneMediaId(int seriesId) => GetZuneMediaId(seriesId, EListType.ePlaylistList);

        public static void FindInCollection(int seriesId) => FindInCollection(seriesId, -1);

        public static void FindInCollection(int seriesId, int libraryId)
        {
            if (seriesId <= 0 && libraryId <= 0)
                return;
            Hashtable hashtable = new Hashtable();
            if (seriesId > 0)
                hashtable.Add("SeriesLibraryId", seriesId);
            if (libraryId > 0)
                hashtable.Add("EpisodeLibraryId", libraryId);
            ZuneShell.DefaultInstance.Execute("Collection\\Channels", hashtable);
        }

        public static void SaveAsPlaylist(int playlistId) => PlaylistManager.Instance.SavePlaylistAsStatic(playlistId, new PlaylistAsyncOperationCompleted(SaveAsPlaylistCompleted));

        public static void SaveAsPlaylistCompleted(HRESULT hr) => Application.DeferredInvoke(delegate
       {
           NotificationArea.Instance.Add(new MessageNotification(!hr.IsSuccess ? _saveAsPlaylistMessageFailure : _saveAsPlaylistMessageSuccess, NotificationTask.Library, NotificationState.Completed));
       }, null);
    }
}
