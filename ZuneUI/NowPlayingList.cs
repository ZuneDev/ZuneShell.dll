// Decompiled with JetBrains decompiler
// Type: ZuneUI.NowPlayingList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.Playlist;
using Microsoft.Zune.QuickMix;
using Microsoft.Zune.Shell;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using UIXControls;
using ZuneXml;

namespace ZuneUI
{
    [Serializable]
    internal class NowPlayingList : IDisposable
    {
        private const int s_quickMixCreationTimeoutMilliseconds = 15000;
        private const int s_nearingCompletionThreshold = 3;
        [NonSerialized]
        private ArrayListDataSet _tracks;
        private List<PlaybackTrack> _savedTracks;
        private int[] _shuffleOrder;
        private bool _shuffling;
        private bool _repeating;
        private int _indexShuffleStart;
        private PlaybackContext _playbackContext;
        private int _currentRandomSeed;
        private PlayNavigationOptions _playNavigationOptions;
        private bool _dontPlayMarketplaceTracks;
        private bool _playWhenReady;
        private EQuickMixType _quickMixType = EQuickMixType.eQuickMixTypeInvalid;
        private string _quickMixTitle;
        [NonSerialized]
        private int _transientTracksPlaylistId;
        [NonSerialized]
        private static int _transientPlaylistCount;
        [NonSerialized]
        private PlaybackTrack _trackCurrent;
        [NonSerialized]
        private PlaybackTrack _trackNext;
        private int _indexCurrent;
        [NonSerialized]
        private QuickMixSession _quickMixSession;
        [NonSerialized]
        private QuickMixNotification _quickMixCreatingNotification;
        [NonSerialized]
        private StringId _quickMixNoResultsStringId;
        private static Random s_random = new Random();

        public NowPlayingList(
          IList items,
          int startIndex,
          PlaybackContext context,
          PlayNavigationOptions playNavigationOptions,
          bool shuffling,
          ContainerPlayMarker containerPlayMarker,
          bool dontPlayMarketplaceTracks)
        {
            this._transientTracksPlaylistId = int.MinValue;
            this._playbackContext = context;
            this._playNavigationOptions = playNavigationOptions;
            this._dontPlayMarketplaceTracks = dontPlayMarketplaceTracks;
            if (this._playbackContext == PlaybackContext.QuickMix)
                this.AddQuickMixItemsWorker(items);
            else
                this.AddItemsWorker(items, startIndex, shuffling, containerPlayMarker);
        }

        public void Dispose()
        {
            if (this._quickMixSession != null)
            {
                this._quickMixSession.Dispose();
                this._quickMixSession = (QuickMixSession)null;
            }
            this.CleanupTransientTracks();
        }

        private void CleanupTransientTracks()
        {
            if (this._transientTracksPlaylistId == int.MinValue)
                return;
            ZuneApplication.ZuneLibrary.DeleteMedia(new int[1]
            {
        this._transientTracksPlaylistId
            }, EMediaTypes.eMediaTypePlaylist, false);
            this._transientTracksPlaylistId = int.MinValue;
        }

        private void AddQuickMixItemsWorker(IList items)
        {
            Microsoft.Zune.QuickMix.QuickMix instance = Microsoft.Zune.QuickMix.QuickMix.Instance;
            HRESULT hresult = HRESULT._S_OK;
            EMediaTypes eMediaType = EMediaTypes.eMediaTypeInvalid;
            this._quickMixNoResultsStringId = StringId.IDS_QUICKMIX_ITEM_CREATION_UNAVAILABLE_TEXT;
            if (this._quickMixSession != null)
            {
                this._quickMixSession.Dispose();
                this._quickMixSession = (QuickMixSession)null;
            }
            NotificationArea.Instance.RemoveAll(NotificationTask.QuickMix, NotificationState.OneShot);
            if (items[0] is Artist artist)
            {
                this._quickMixNoResultsStringId = StringId.IDS_QUICKMIX_ARTIST_CREATION_UNAVAILABLE_TEXT;
                hresult = instance.CreateSession(EQuickMixMode.eQuickMixModeNowPlaying, artist.Id, EMediaTypes.eMediaTypePersonArtist, artist.Title, out this._quickMixSession);
                if (hresult.IsSuccess)
                {
                    string text = string.Format(ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_NOTIFICATION_CREATION_BUSY_ONE_PARAM_TEXT), (object)artist.Title);
                    this._quickMixCreatingNotification = new QuickMixNotification(ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_NOTIFICATION_CREATION_BUSY_TITLE), text, NotificationState.OneShot, false, 15000);
                    SQMLog.Log(SQMDataId.QuickMixRadioPlays, 1);
                }
            }
            if (hresult.IsSuccess && items[0] is LibraryDataProviderItemBase providerItemBase)
            {
                if (providerItemBase.TypeName == "Artist")
                {
                    eMediaType = EMediaTypes.eMediaTypePersonArtist;
                    this._quickMixNoResultsStringId = StringId.IDS_QUICKMIX_ARTIST_CREATION_UNAVAILABLE_TEXT;
                    string property = (string)providerItemBase.GetProperty("Title");
                    string text = string.Format(ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_NOTIFICATION_CREATION_BUSY_ONE_PARAM_TEXT), (object)property);
                    this._quickMixCreatingNotification = new QuickMixNotification(ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_NOTIFICATION_CREATION_BUSY_TITLE), text, NotificationState.OneShot, false, 15000);
                }
                else if (providerItemBase.TypeName == "Album")
                {
                    eMediaType = EMediaTypes.eMediaTypeAudioAlbum;
                    this._quickMixNoResultsStringId = StringId.IDS_QUICKMIX_ALBUM_CREATION_UNAVAILABLE_TEXT;
                    string property1 = (string)providerItemBase.GetProperty("ArtistName");
                    string property2 = (string)providerItemBase.GetProperty("Title");
                    string text = string.Format(ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_NOTIFICATION_CREATION_BUSY_TWO_PARAM_TEXT), (object)property1, (object)property2);
                    this._quickMixCreatingNotification = new QuickMixNotification(ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_NOTIFICATION_CREATION_BUSY_TITLE), text, NotificationState.OneShot, false, 15000);
                }
                else if (providerItemBase.TypeName == "Track")
                {
                    eMediaType = EMediaTypes.eMediaTypeAudio;
                    this._quickMixNoResultsStringId = StringId.IDS_QUICKMIX_SONG_CREATION_UNAVAILABLE_TEXT;
                    string property1 = (string)providerItemBase.GetProperty("ArtistName");
                    string property2 = (string)providerItemBase.GetProperty("Title");
                    string text = string.Format(ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_NOTIFICATION_CREATION_BUSY_TWO_PARAM_TEXT), (object)property1, (object)property2);
                    this._quickMixCreatingNotification = new QuickMixNotification(ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_NOTIFICATION_CREATION_BUSY_TITLE), text, NotificationState.OneShot, false, 15000);
                }
                int[] seedMediaIds = new int[1]
                {
          (int) providerItemBase.GetProperty("LibraryId")
                };
                hresult = instance.CreateSession(EQuickMixMode.eQuickMixModeNowPlaying, seedMediaIds, eMediaType, out this._quickMixSession);
                SQMLog.Log(SQMDataId.QuickMixLocalPlays, 1);
            }
            if (hresult.IsSuccess)
            {
                if (this._quickMixSession != null)
                {
                    hresult = this._quickMixSession.GetSimilarMedia((uint)ClientConfiguration.QuickMix.DefaultPlaylistLength, TimeSpan.FromMilliseconds(15000.0), new SimilarMediaBatchHandler(this.SimilarBatchHandler), new Microsoft.Zune.QuickMix.BatchEndHandler(this.BatchEndHandler));
                    this._quickMixType = this._quickMixSession.GetQuickMixType();
                    this._quickMixSession.GetPlaylistTitle(out this._quickMixTitle);
                }
                else
                    hresult = HRESULT._E_UNEXPECTED;
            }
            if (hresult.IsSuccess)
            {
                NotificationArea.Instance.Add((Notification)this._quickMixCreatingNotification);
            }
            else
            {
                if (this._quickMixSession != null)
                {
                    this._quickMixSession.Dispose();
                    this._quickMixSession = (QuickMixSession)null;
                }
                if ((HRESULT)hresult.Int == HRESULT._ZUNE_E_QUICKMIX_MEDIA_NOT_FOUND)
                    MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_CREATION_UNAVAILABLE_NO_RESULTS_TITLE), ZuneUI.Shell.LoadString(this._quickMixNoResultsStringId), (EventHandler)null);
                else
                    ErrorDialogInfo.Show(hresult.Int, ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_CREATION_UNAVAILABLE_NO_RESULTS_TITLE));
            }
        }

        private void SimilarBatchHandler(IList itemList) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this._quickMixSession == null)
               return;
           this.AddItems(itemList, new ContainerPlayMarker()
           {
               LibraryId = -1,
               MediaType = MediaType.Playlist,
               PlaylistType = PlaylistType.QuickMix,
               PlaylistSubType = (int)this._quickMixSession.GetQuickMixType()
           });
           if (!this.PlayWhenReady)
               return;
           this.PlayWhenReady = false;
           SingletonModelItem<TransportControls>.Instance.PlayPendingList();
       }, (object)null);

        private void BatchEndHandler(HRESULT hrAsync) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           NotificationArea.Instance.Remove((Notification)this._quickMixCreatingNotification);
           this._quickMixCreatingNotification = (QuickMixNotification)null;
           if (this._quickMixSession == null || !hrAsync.IsError)
               return;
           if ((HRESULT)hrAsync.Int == HRESULT._ZUNE_E_QUICKMIX_MEDIA_NOT_FOUND)
               MessageBox.Show(ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_CREATION_UNAVAILABLE_NO_RESULTS_TITLE), ZuneUI.Shell.LoadString(this._quickMixNoResultsStringId), (EventHandler)null);
           else
               ErrorDialogInfo.Show(hrAsync.Int, ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_CREATION_UNAVAILABLE_NO_RESULTS_TITLE));
       }, (object)null);

        public ArrayListDataSet TrackList => this._tracks;

        public int Count => this._tracks == null ? 0 : this._tracks.Count;

        public PlaybackContext PlaybackContext => this._playbackContext;

        public PlayNavigationOptions PlayNavigationOptions
        {
            get => this._playNavigationOptions;
            set => this._playNavigationOptions = value;
        }

        public string QuickMixTitle
        {
            get
            {
                string playlistTitle = this._quickMixTitle;
                if (this._quickMixSession != null)
                    this._quickMixSession.GetPlaylistTitle(out playlistTitle);
                return playlistTitle;
            }
        }

        public bool DontPlayMarketplaceTracks
        {
            get => this._dontPlayMarketplaceTracks;
            set
            {
                if (value == this._dontPlayMarketplaceTracks)
                    return;
                this._dontPlayMarketplaceTracks = value;
                this.UpdateTracks();
            }
        }

        public bool PlayWhenReady
        {
            get => this._playWhenReady;
            set
            {
                if (value == this._playWhenReady)
                    return;
                this._playWhenReady = value;
            }
        }

        public QuickMixSession QuickMixSession => this._quickMixSession;

        public EQuickMixType QuickMixType => this._quickMixType;

        public PlaybackTrack CurrentTrack => this._trackCurrent;

        public PlaybackTrack NextTrack => this._trackNext;

        public int ListIndexOfCurrentTrack
        {
            get
            {
                int indexCurrent = this._indexCurrent;
                if (indexCurrent != -1 && this._shuffling)
                    indexCurrent = this._shuffleOrder[indexCurrent];
                return indexCurrent;
            }
        }

        public void SetShuffling(bool shuffling)
        {
            if (this._quickMixSession != null)
                return;
            if (this._tracks == null || this._tracks.Count == 0)
            {
                this._shuffling = shuffling;
            }
            else
            {
                if (shuffling == this._shuffling)
                    return;
                int index = !this._shuffling ? this._indexCurrent : this._shuffleOrder[this._indexCurrent];
                if (shuffling)
                    this.RebuildShuffleOrder();
                else
                    this._shuffleOrder = (int[])null;
                this._indexCurrent = !shuffling ? index : this.FindShuffleIndexFromListIndex(index);
                this._shuffling = shuffling;
                this.UpdateTracks();
            }
        }

        public void SetRepeating(bool repeating)
        {
            if (this._repeating == repeating)
                return;
            this._repeating = repeating;
            this.UpdateTracks();
        }

        public void SyncCurrentTrackTo(PlaybackTrack track)
        {
            if (track == this._trackCurrent)
                return;
            int num;
            if (track == this._trackNext)
            {
                num = this.ComputeNextIndex(NowPlayingList.MovingMode.Advancing);
            }
            else
            {
                num = this._tracks.IndexOf((object)track);
                if (num != -1 && this._shuffling)
                    num = this.FindShuffleIndexFromListIndex(num);
            }
            if (num == -1)
                return;
            this.MoveToWorker(num, NowPlayingList.MovingMode.Advancing);
        }

        public void MoveToTrackIndex(int indexNew)
        {
            if (this._shuffling)
                indexNew = this.FindShuffleIndexFromListIndex(indexNew);
            this.MoveToWorker(indexNew, NowPlayingList.MovingMode.Jumping);
        }

        public void Advance() => this.MoveToWorker(this.ComputeNextIndex(NowPlayingList.MovingMode.Advancing), NowPlayingList.MovingMode.Advancing);

        public bool CanAdvance => this._trackNext != null;

        public void Retreat()
        {
            if (this._shuffling && this._repeating && this._indexCurrent == this._indexShuffleStart)
                this.RebuildShuffleOrder(new int?(this._currentRandomSeed - 1));
            this.MoveToWorker(this.ComputeNextIndex(NowPlayingList.MovingMode.Retreating), NowPlayingList.MovingMode.Retreating);
        }

        public bool CanRetreat => this._repeating || (!this._shuffling ? this._indexCurrent > 0 : this._indexCurrent != this._indexShuffleStart);

        public int AddItems(IList items) => this.AddItems(items, (ContainerPlayMarker)null);

        public int AddItems(IList items, ContainerPlayMarker playMarker) => this.AddItemsWorker(items, -1, false, playMarker);

        public void ResetForReplay()
        {
            this._indexCurrent = !this._shuffling ? 0 : NowPlayingList.s_random.Next(this._tracks.Count);
            if (this._shuffling)
                this.RebuildShuffleOrder();
            this.UpdateTracks();
        }

        private void MoveToWorker(int indexNew, NowPlayingList.MovingMode mode)
        {
            this._indexCurrent = indexNew;
            if (this._shuffling)
            {
                if (mode == NowPlayingList.MovingMode.Advancing && indexNew == this._indexShuffleStart)
                    this.RebuildShuffleOrder(new int?(this._currentRandomSeed + 1));
                else if (mode == NowPlayingList.MovingMode.Jumping)
                    this.RebuildShuffleOrder();
            }
            this.UpdateTracks();
            if (!this.CanGetMoreQuickMixTracks() || !this.ListNearingCompletion())
                return;
            HRESULT similarMedia = this._quickMixSession.GetSimilarMedia((uint)ClientConfiguration.QuickMix.DefaultPlaylistLength, TimeSpan.FromMilliseconds(15000.0), new SimilarMediaBatchHandler(this.SimilarBatchHandler), new Microsoft.Zune.QuickMix.BatchEndHandler(this.BatchEndHandler));
            if (!similarMedia.IsError || (HRESULT)similarMedia.Int == HRESULT._ZUNE_E_QUICKMIX_SESSION_IN_USE)
                return;
            ErrorDialogInfo.Show(similarMedia.Int, ZuneUI.Shell.LoadString(StringId.IDS_QUICKMIX_CREATION_UNAVAILABLE_NO_RESULTS_TITLE));
        }

        private bool CanGetMoreQuickMixTracks()
        {
            bool flag = false;
            if (this._quickMixSession != null)
                flag = this._quickMixSession.GetQuickMixType() != EQuickMixType.eQuickMixTypeRadio || SignIn.Instance.SignedIn;
            return flag;
        }

        private int ComputeNextIndex(NowPlayingList.MovingMode mode) => this.ComputeNextIndex(mode, this._indexCurrent, true);

        private int ComputeNextIndex(NowPlayingList.MovingMode mode, int fromIndex, bool allowLooping)
        {
            if (mode != NowPlayingList.MovingMode.Advancing && mode != NowPlayingList.MovingMode.Retreating)
                return fromIndex;
            int index1;
            if (!this._dontPlayMarketplaceTracks)
            {
                index1 = this.ComputeNextIndexUnfiltered(mode, fromIndex, allowLooping);
            }
            else
            {
                index1 = fromIndex;
                bool flag = false;
                for (int index2 = 0; index2 < this._tracks.Count; ++index2)
                {
                    index1 = this.ComputeNextIndexUnfiltered(mode, index1, allowLooping);
                    if (index1 != -1)
                    {
                        PlaybackTrack playbackTrack = !this._shuffling ? this._tracks[index1] as PlaybackTrack : this._tracks[this._shuffleOrder[index1]] as PlaybackTrack;
                        if (playbackTrack != null && playbackTrack.IsInVisibleCollection)
                        {
                            flag = true;
                            break;
                        }
                    }
                    else
                        break;
                }
                if (!flag)
                    index1 = -1;
            }
            return index1;
        }

        private int ComputeNextIndexUnfiltered(
          NowPlayingList.MovingMode mode,
          int indexCurrent,
          bool allowLooping)
        {
            int num = mode != NowPlayingList.MovingMode.Advancing ? indexCurrent - 1 : indexCurrent + 1;
            if (this._shuffling)
            {
                if (num == this._tracks.Count)
                    num = 0;
                else if (num == -1)
                    num = this._tracks.Count - 1;
                if ((mode == NowPlayingList.MovingMode.Advancing && num == this._indexShuffleStart || mode == NowPlayingList.MovingMode.Retreating && num == this._indexShuffleStart - 1) && (!this._repeating || !allowLooping))
                    num = -1;
            }
            else if (num == this._tracks.Count)
                num = !this._repeating || !allowLooping ? -1 : 0;
            else if (num == -1)
                num = !this._repeating || !allowLooping ? -1 : this._tracks.Count - 1;
            return num;
        }

        private bool ListNearingCompletion()
        {
            if (this._tracks.Count == 0)
                return false;
            int fromIndex = this._indexCurrent;
            int count = this._tracks.Count;
            bool flag = false;
            for (int index = 0; index < 3; ++index)
            {
                fromIndex = this.ComputeNextIndex(NowPlayingList.MovingMode.Advancing, fromIndex, false);
                if (fromIndex == -1)
                {
                    flag = true;
                    break;
                }
            }
            int num = flag ? 1 : 0;
            return flag;
        }

        internal void UpdateTracks()
        {
            object obj1 = (object)null;
            object obj2 = (object)null;
            if (this._tracks != null && this._indexCurrent != -1)
            {
                obj1 = this._tracks[this._shuffling ? this._shuffleOrder[this._indexCurrent] : this._indexCurrent];
                int nextIndex = this.ComputeNextIndex(NowPlayingList.MovingMode.Advancing);
                if (nextIndex != -1)
                {
                    if (this._shuffling)
                        nextIndex = this._shuffleOrder[nextIndex];
                    obj2 = this._tracks[nextIndex];
                }
            }
            this._trackCurrent = (PlaybackTrack)obj1;
            this._trackNext = (PlaybackTrack)obj2;
        }

        private int FindShuffleIndexFromListIndex(int index)
        {
            int num = -1;
            for (int index1 = 0; index1 < this._tracks.Count; ++index1)
            {
                if (this._shuffleOrder[index1] == index)
                {
                    num = index1;
                    break;
                }
            }
            return num;
        }

        private void RebuildShuffleOrder(int? seed)
        {
            int index1 = !this._shuffling ? this._indexCurrent : this._shuffleOrder[this._indexCurrent];
            if (this._tracks == null)
            {
                this._shuffleOrder = (int[])null;
            }
            else
            {
                if (this._shuffleOrder == null || this._shuffleOrder.Length != this._tracks.Count)
                    this._shuffleOrder = new int[this._tracks.Count];
                for (int index2 = 0; index2 < this._tracks.Count; ++index2)
                    this._shuffleOrder[index2] = index2;
                this._currentRandomSeed = !seed.HasValue ? (int)DateTime.Now.Ticks : seed.Value;
                Random random = new Random(this._currentRandomSeed);
                for (int index2 = this._tracks.Count - 1; index2 > 0; --index2)
                {
                    int index3 = random.Next(index2 + 1);
                    int num = this._shuffleOrder[index3];
                    this._shuffleOrder[index3] = this._shuffleOrder[index2];
                    this._shuffleOrder[index2] = num;
                }
            }
            this._indexShuffleStart = this.FindShuffleIndexFromListIndex(index1);
            if (!this._shuffling)
                return;
            this._indexCurrent = this._indexShuffleStart;
        }

        private void RebuildShuffleOrder() => this.RebuildShuffleOrder(new int?());

        private int AddItemsWorker(
          IList items,
          int startIndex,
          bool shuffling,
          ContainerPlayMarker containerPlayMarker)
        {
            ArrayListDataSet arrayListDataSet = new ArrayListDataSet();
            if (containerPlayMarker != null && containerPlayMarker.MediaType == MediaType.Playlist && containerPlayMarker.PlaylistType == PlaylistType.QuickMix)
            {
                this._playbackContext = PlaybackContext.QuickMix;
                this._quickMixType = (EQuickMixType)containerPlayMarker.PlaylistSubType;
                this._quickMixTitle = PlaylistManager.GetPlaylistName(containerPlayMarker.LibraryId);
                SQMLog.Log(SQMDataId.QuickMixPlaylistPlays, 1);
            }
            else if (items != null && items.Count == 1 && items[0] is LibraryDataProviderItemBase)
            {
                int mediaId = -1;
                EMediaTypes mediaType = EMediaTypes.eMediaTypeInvalid;
                (items[0] as LibraryDataProviderItemBase).GetMediaIdAndType(out mediaId, out mediaType);
                if (mediaType == EMediaTypes.eMediaTypePlaylist && PlaylistManager.GetPlaylistType(mediaId) == 7)
                {
                    this._playbackContext = PlaybackContext.QuickMix;
                    this._quickMixType = (EQuickMixType)PlaylistManager.GetSubType(mediaId);
                    this._quickMixTitle = PlaylistManager.GetPlaylistName(mediaId);
                    SQMLog.Log(SQMDataId.QuickMixPlaylistPlays, 1);
                }
            }
            PlaylistManager.AddItemsToTrackList(items, (IList)arrayListDataSet, ref startIndex, true, false, false, containerPlayMarker);
            if (arrayListDataSet.Count != 0)
            {
                bool flag1 = false;
                if (this._tracks == null)
                {
                    this._tracks = new ArrayListDataSet();
                    if (startIndex == -1)
                        startIndex = !shuffling ? 0 : NowPlayingList.s_random.Next(arrayListDataSet.Count);
                    flag1 = true;
                }
                bool flag2 = false;
                bool flag3 = false;
                if (this._tracks.Count > 0)
                {
                    if (((PlaybackTrack)this._tracks[0]).IsVideo)
                        flag2 = true;
                    else
                        flag3 = true;
                }
                for (int itemIndex = 0; itemIndex < arrayListDataSet.Count; ++itemIndex)
                {
                    PlaybackTrack playbackTrack = (PlaybackTrack)arrayListDataSet[itemIndex];
                    bool flag4 = true;
                    if (playbackTrack.IsVideo)
                    {
                        if (flag2)
                            flag4 = false;
                        else if (flag3)
                            flag4 = false;
                        else
                            flag2 = true;
                    }
                    else if (flag2)
                        flag4 = false;
                    else
                        flag3 = true;
                    if (flag4)
                        this._tracks.Add(arrayListDataSet[itemIndex]);
                    if (flag1 && itemIndex == startIndex)
                        this._indexCurrent = this._tracks.Count <= 0 ? 0 : this._tracks.Count - 1;
                }
                if (this._shuffling)
                    this.RebuildShuffleOrder();
                if (this._quickMixType != EQuickMixType.eQuickMixTypeInvalid)
                    this.AddReferencesToRemoteTracks((IList)arrayListDataSet);
            }
            this.UpdateTracks();
            return arrayListDataSet.Count;
        }

        private void AddReferencesToRemoteTracks(IList tracks)
        {
            if (tracks == null)
                return;
            if (this._transientTracksPlaylistId == int.MinValue)
            {
                PlaylistResult playlist = PlaylistManager.Instance.CreatePlaylist("Now Playing Transient Tracks Playlist [" + (object)NowPlayingList._transientPlaylistCount + "]", true);
                ++NowPlayingList._transientPlaylistCount;
                this._transientTracksPlaylistId = playlist.PlaylistId;
            }
            List<LibraryPlaybackTrack> libraryPlaybackTrackList = new List<LibraryPlaybackTrack>(tracks.Count);
            foreach (object track in (IEnumerable)tracks)
            {
                if (track is LibraryPlaybackTrack libraryPlaybackTrack && !libraryPlaybackTrack.IsInVisibleCollection)
                    libraryPlaybackTrackList.Add(libraryPlaybackTrack);
            }
            int playlist1 = (int)PlaylistManager.Instance.AddToPlaylist(this._transientTracksPlaylistId, (IList)libraryPlaybackTrackList, false, false);
        }

        public void Reorder(IList indices, int targetIndex)
        {
            PlaybackTrack currentTrack = this.CurrentTrack;
            this._tracks.Reorder(indices, targetIndex);
            if (this._shuffling)
                this.RebuildShuffleOrder();
            this._indexCurrent = this._tracks.IndexOf((object)currentTrack);
            if (this._shuffling)
                this._indexCurrent = this.FindShuffleIndexFromListIndex(this._indexCurrent);
            this.UpdateTracks();
        }

        public bool Remove(IList indices)
        {
            bool shuffling = this._shuffling;
            this.SetShuffling(false);
            int num = this._indexCurrent;
            bool flag = false;
            for (int index1 = indices.Count - 1; index1 >= 0; --index1)
            {
                int index2 = (int)indices[index1];
                this._tracks.RemoveAt(index2);
                if (num == index2)
                    flag = true;
                else if (num > index2)
                    --num;
            }
            if (num >= this._tracks.Count)
                num = this._tracks.Count - 1;
            this._indexCurrent = num;
            this.SetShuffling(shuffling);
            this.UpdateTracks();
            return flag;
        }

        public IList GetNextTracks(int count)
        {
            List<PlaybackTrack> playbackTrackList = (List<PlaybackTrack>)null;
            if (this._tracks != null && this._tracks.Count > 0)
            {
                playbackTrackList = new List<PlaybackTrack>(count);
                for (int index = 0; index < count; ++index)
                {
                    int itemIndex = this._indexCurrent + index;
                    if (this._repeating || this._shuffling)
                    {
                        while (itemIndex >= this._tracks.Count)
                            itemIndex -= this._tracks.Count;
                    }
                    if (itemIndex >= 0 && (this._shuffling ? (itemIndex != this._indexShuffleStart ? 0 : (index != 0 ? 1 : 0)) : (itemIndex == this._tracks.Count ? 1 : 0)) == 0)
                    {
                        if (this._shuffling)
                            itemIndex = this._shuffleOrder[itemIndex];
                        playbackTrackList.Add((PlaybackTrack)this._tracks[itemIndex]);
                    }
                    else
                        break;
                }
            }
            return (IList)playbackTrackList;
        }

        [OnSerializing]
        internal void PrepareForSerialization(StreamingContext context)
        {
            if (this._tracks == null)
                return;
            int num1 = Math.Min(this._indexCurrent + 380, this._tracks.Count);
            int num2 = Math.Max(this._indexCurrent - (400 - (num1 - this._indexCurrent)), 0);
            this._savedTracks = new List<PlaybackTrack>(num1 - num2);
            for (int index = num2; index < num1; ++index)
                this._savedTracks.Add((PlaybackTrack)this._tracks[!this._shuffling ? index : this._shuffleOrder[index]]);
            int num3 = this._indexCurrent - num2;
            if (this._shuffling)
            {
                this._indexCurrent = 0;
                this._shuffleOrder[0] = num3;
            }
            else
                this._indexCurrent = num3;
        }

        [OnDeserialized]
        internal void HandleDeserialization(StreamingContext context)
        {
            this._transientTracksPlaylistId = int.MinValue;
            if (this._savedTracks != null)
            {
                this._tracks = new ArrayListDataSet();
                foreach (object savedTrack in this._savedTracks)
                    this._tracks.Add(savedTrack);
                this._savedTracks = (List<PlaybackTrack>)null;
                if (this._shuffling)
                    this.RebuildShuffleOrder();
                this.UpdateTracks();
            }
            Application.DeferredInvoke((DeferredInvokeHandler)delegate
           {
               SingletonModelItem<TransportControls>.Instance.Shuffling.Value = this._shuffling;
           }, (object)null);
        }

        [Conditional("DEBUG_NOW_PLAYING_LIST")]
        private static void _DEBUG_Trace(string message, params object[] args)
        {
        }

        private enum MovingMode
        {
            Advancing,
            Retreating,
            Jumping,
        }
    }
}
