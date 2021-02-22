// Decompiled with JetBrains decompiler
// Type: ZuneUI.QuickMixSessionManager
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.QuickMix;
using System;

namespace ZuneUI
{
    public class QuickMixSessionManager : ModelItem
    {
        private QuickMixSession _quickMixSession;
        private bool _isRefreshing;
        private int _playlistId;

        public QuickMixSessionManager(int playlistId)
        {
            this._playlistId = playlistId;
            QuickMix.Instance.CreateSession(EQuickMixMode.eQuickMixModePlaylist, new int[1]
            {
        playlistId
            }, EMediaTypes.eMediaTypePlaylist, out this._quickMixSession);
        }

        public int PlaylistId => this._playlistId;

        internal QuickMixSession QuickMixSession => this._quickMixSession;

        public bool IsRefreshing
        {
            get => this._isRefreshing;
            set
            {
                if (this._isRefreshing == value)
                    return;
                this._isRefreshing = value;
                this.FirePropertyChanged(nameof(IsRefreshing));
            }
        }

        protected override void OnDispose(bool disposing)
        {
            if (this._quickMixSession != null)
            {
                this._quickMixSession.Dispose();
                this._quickMixSession = null;
            }
            base.OnDispose(disposing);
        }

        public void Refresh(bool deepRefresh)
        {
            if (this._quickMixSession == null || this.IsRefreshing || !this._quickMixSession.Refresh(TimeSpan.FromMilliseconds(5000.0), deepRefresh, null, new BatchEndHandler(this.RefreshedHandler)).IsSuccess)
                return;
            this.IsRefreshing = true;
        }

        private void RefreshedHandler(HRESULT hrAsync) => Application.DeferredInvoke(delegate
       {
           this.IsRefreshing = false;
       }, null);
    }
}
