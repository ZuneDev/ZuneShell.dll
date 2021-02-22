// Decompiled with JetBrains decompiler
// Type: ZuneUI.QuickMixPlaylistFactory
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Configuration;
using Microsoft.Zune.PerfTrace;
using Microsoft.Zune.Playlist;
using Microsoft.Zune.QuickMix;
using Microsoft.Zune.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class QuickMixPlaylistFactory : PlaylistFactory
    {
        private const int _minimumSimilarArtistsRequired = 10;
        private QuickMixSession _quickMixSession;
        private bool _shouldDisposeSession;
        private HRESULT _hrCreation = HRESULT._E_PENDING;
        private EQuickMixMode _mode;
        private List<string> _similarArtistStrings;

        private QuickMixPlaylistFactory(
          int mediaId,
          MediaType mediaType,
          EQuickMixMode mode,
          out HRESULT hr)
          : base(true)
        {
            TimeSpan maxBatchTimeout = TimeSpan.FromMilliseconds(5000.0);
            int[] seedMediaIds = new int[1] { mediaId };
            this._mode = mode;
            Microsoft.Zune.QuickMix.QuickMix instance = Microsoft.Zune.QuickMix.QuickMix.Instance;
            hr = instance.CreateSession(this._mode, seedMediaIds, (EMediaTypes)mediaType, out this._quickMixSession);
            if (!hr.IsSuccess)
                return;
            this._shouldDisposeSession = true;
            if (this._mode != EQuickMixMode.eQuickMixModeSimilarArtists)
                Microsoft.Zune.PerfTrace.PerfTrace.TraceUICollectionEvent(UICollectionEvent.QuickMixBegin, "");
            hr = this._quickMixSession.GetSimilarMedia((uint)ClientConfiguration.QuickMix.DefaultPlaylistLength, maxBatchTimeout, new SimilarMediaBatchHandler(this.SimilarBatchHandler), new Microsoft.Zune.QuickMix.BatchEndHandler(this.BatchEndHandler));
        }

        private QuickMixPlaylistFactory(
          Guid serviceMediaId,
          MediaType mediaType,
          EQuickMixMode mode,
          out HRESULT hr)
          : base(true)
        {
            TimeSpan maxBatchTimeout = TimeSpan.FromMilliseconds(10000.0);
            this._mode = mode;
            Microsoft.Zune.QuickMix.QuickMix instance = Microsoft.Zune.QuickMix.QuickMix.Instance;
            hr = instance.CreateSession(this._mode, serviceMediaId, (EMediaTypes)mediaType, (string)null, out this._quickMixSession);
            if (!hr.IsSuccess)
                return;
            this._shouldDisposeSession = true;
            hr = this._quickMixSession.GetSimilarMedia((uint)ClientConfiguration.QuickMix.DefaultPlaylistLength, maxBatchTimeout, new SimilarMediaBatchHandler(this.SimilarBatchHandler), new Microsoft.Zune.QuickMix.BatchEndHandler(this.BatchEndHandler));
        }

        private QuickMixPlaylistFactory(QuickMixSession quickMixSession)
          : base(true)
        {
            this._quickMixSession = quickMixSession;
            this._shouldDisposeSession = false;
            this._hrCreation = HRESULT._S_OK;
            this.Ready = true;
        }

        public override void Dispose()
        {
            if (!this._shouldDisposeSession || this._quickMixSession == null)
                return;
            this._quickMixSession.Dispose();
            this._quickMixSession = (QuickMixSession)null;
        }

        public static QuickMixPlaylistFactory CreateInstance(
          int mediaId,
          MediaType mediaType)
        {
            return QuickMixPlaylistFactory.CreateInstance(mediaId, EQuickMixMode.eQuickMixModePlaylist, mediaType);
        }

        public static QuickMixPlaylistFactory CreateInstance(
          int mediaId,
          EQuickMixMode mode,
          MediaType mediaType)
        {
            HRESULT hr;
            QuickMixPlaylistFactory mixPlaylistFactory = new QuickMixPlaylistFactory(mediaId, mediaType, mode, out hr);
            return hr.IsSuccess ? mixPlaylistFactory : (QuickMixPlaylistFactory)null;
        }

        public static QuickMixPlaylistFactory CreateInstance(
          Guid serviceMediaId,
          EQuickMixMode mode,
          MediaType mediaType)
        {
            HRESULT hr;
            QuickMixPlaylistFactory mixPlaylistFactory = new QuickMixPlaylistFactory(serviceMediaId, mediaType, mode, out hr);
            return hr.IsSuccess ? mixPlaylistFactory : (QuickMixPlaylistFactory)null;
        }

        public static QuickMixPlaylistFactory CreateInstance(
          QuickMixSession quickMixSession)
        {
            return new QuickMixPlaylistFactory(quickMixSession);
        }

        public override string GetUniqueTitle()
        {
            string playlistTitle = "";
            this._quickMixSession.GetPlaylistTitle(out playlistTitle);
            return playlistTitle;
        }

        public override PlaylistResult CreatePlaylist(
          string title,
          CreatePlaylistOption option)
        {
            SQMLog.Log(SQMDataId.QuickMixPlaylistCreates, 1);
            int playlistId = -1;
            HRESULT hr = HRESULT._E_PENDING;
            if (this.Ready)
            {
                hr = this._hrCreation;
                if (hr.IsSuccess)
                    hr = this._quickMixSession.SaveAsPlaylist(title, option, out playlistId);
            }
            return new PlaylistResult(playlistId, hr);
        }

        private void BatchEndHandler(HRESULT hrAsync) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           this._hrCreation = hrAsync;
           this.Ready = true;
           if (this._mode == EQuickMixMode.eQuickMixModeSimilarArtists)
               return;
           Microsoft.Zune.PerfTrace.PerfTrace.TraceUICollectionEvent(UICollectionEvent.QuickMixComplete, "");
       }, (object)null);

        private void SimilarBatchHandler(IList itemList) => Application.DeferredInvoke((DeferredInvokeHandler)delegate
       {
           if (this._mode != EQuickMixMode.eQuickMixModeSimilarArtists)
               return;
           if (this._similarArtistStrings == null)
               this._similarArtistStrings = new List<string>();
           foreach (QuickMixItem quickMixItem in (IEnumerable)itemList)
               this._similarArtistStrings.Add(quickMixItem.ArtistName);
           if (this._similarArtistStrings.Count <= 10)
               return;
           this.Ready = true;
       }, (object)null);

        public IList SimilarArtists => (IList)this._similarArtistStrings;
    }
}
