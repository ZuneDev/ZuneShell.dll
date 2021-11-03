// Decompiled with JetBrains decompiler
// Type: ZuneUI.VideoPlaybackTrack
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;
using ZuneXml;

namespace ZuneUI
{
    [Serializable]
    public class VideoPlaybackTrack : PlaybackTrack
    {
        private Guid _zuneMediaId;
        private Guid _zuneMediaInstanceId;
        private string _title;
        private string _artist;
        private string _uri;
        private bool _isDownloading;
        private bool _fallbackToPreview;
        private bool _forcePreview;
        private bool _ignoreCollection;
        private bool _isStreaming;
        private VideoDefinitionEnum _videoDefinition;

        public VideoPlaybackTrack(
          Guid zuneMediaId,
          string title,
          string artist,
          string uri,
          bool isDownloading,
          bool isStreaming,
          bool ignoreCollection,
          bool fallbackToPreview,
          bool forcePreview,
          VideoDefinitionEnum videoDefinition)
        {
            this._title = title;
            this._zuneMediaId = zuneMediaId;
            this._artist = artist;
            this._uri = uri;
            this._isDownloading = isDownloading;
            this._isStreaming = isStreaming;
            this._ignoreCollection = ignoreCollection;
            this._fallbackToPreview = fallbackToPreview;
            this._forcePreview = forcePreview;
            this._videoDefinition = videoDefinition;
        }

        public override Guid ZuneMediaId => this._zuneMediaId;

        public override HRESULT GetURI(out string uri)
        {
            HRESULT hresult = HRESULT._S_OK;
            string uriOut = null;
            if (!ZuneApplication.Service.InCompleteCollection(this._zuneMediaId, EContentType.Video) && !string.IsNullOrEmpty(this._uri))
            {
                uriOut = this._uri;
            }
            else
            {
                EContentUriFlags eContentUriFlags = EContentUriFlags.None;
                if (this.IgnoreCollection)
                    eContentUriFlags |= EContentUriFlags.IgnoreCollection;
                if (this.FallbackToPreview)
                    eContentUriFlags |= EContentUriFlags.FallbackToPreview;
                if (this.ForcePreview)
                    eContentUriFlags |= EContentUriFlags.ForcePreview;
                hresult = ZuneApplication.Service.GetContentUri(this._zuneMediaId, EContentType.Video, eContentUriFlags, out uriOut, out this._zuneMediaInstanceId);
            }
            if (!string.IsNullOrEmpty(uriOut) && uriOut.Contains(".ism/manifest"))
                this._isStreaming = true;
            uri = uriOut;
            return hresult;
        }

        public Guid Id => this._zuneMediaId;

        public override Guid ZuneMediaInstanceId => this._zuneMediaInstanceId;

        public override string Title => this._title;

        public string Artist => this._artist;

        public override TimeSpan Duration => TimeSpan.Zero;

        public override bool IsVideo => true;

        public override MediaType MediaType => MediaType.Video;

        public bool IsDownloading => this._isDownloading;

        public bool FallbackToPreview => this._fallbackToPreview;

        public bool ForcePreview => this._forcePreview;

        public bool IgnoreCollection => this._ignoreCollection;

        public override bool IsStreaming => this._isStreaming;

        public override bool IsHD => this._videoDefinition == VideoDefinitionEnum.HD;
    }
}
