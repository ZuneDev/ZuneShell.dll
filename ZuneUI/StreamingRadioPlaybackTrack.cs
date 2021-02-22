// Decompiled with JetBrains decompiler
// Type: ZuneUI.StreamingRadioPlaybackTrack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;

namespace ZuneUI
{
    [Serializable]
    public class StreamingRadioPlaybackTrack : PlaybackTrack
    {
        private string _originalUri;
        private string _title;
        private bool _isVideo;
        private MediaType _mediaType;
        [NonSerialized]
        private RadioPlaylist _playlist;

        public StreamingRadioPlaybackTrack(string uri, string title, MediaType mediaType)
        {
            this._originalUri = uri;
            this._title = title;
            this._isVideo = mediaType == MediaType.Video;
            this._mediaType = mediaType;
        }

        public override Guid ZuneMediaId => Guid.Empty;

        public override HRESULT GetURI(out string uri)
        {
            string str = (string)null;
            if (this._playlist == null && this._originalUri != null)
                this._playlist = RadioStationManager.Instance.GetRadioPlaylist(this._originalUri);
            if (this._playlist != null)
                str = this._playlist.GetNextUri();
            uri = str;
            return HRESULT._S_OK;
        }

        public override string Title => this._title;

        public override TimeSpan Duration => TimeSpan.Zero;

        public override bool IsVideo => this._isVideo;

        public override bool IsStreaming => true;

        public override MediaType MediaType => this._mediaType;
    }
}
