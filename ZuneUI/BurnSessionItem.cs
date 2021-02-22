// Decompiled with JetBrains decompiler
// Type: ZuneUI.BurnSessionItem
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    public class BurnSessionItem : ModelItem
    {
        private BurnableCD _burnCD;
        private int _burnListIndex;
        private int _playlistContentId;
        private bool _burnComplete;
        private int _errorCode;
        private int _burnProgress;
        private bool _downloading;
        private int _downloadProgress;
        private bool _burnCanceled;
        private Guid _zuneMediaId;

        internal BurnSessionItem(
          BurnableCD burnCD,
          int burnListIndex,
          int playlistContentId,
          Guid zuneMediaId)
        {
            this._burnCD = burnCD;
            this._burnListIndex = burnListIndex;
            this._playlistContentId = playlistContentId;
            this._burnProgress = -1;
            this._zuneMediaId = zuneMediaId;
        }

        public int PlaylistContentId => this._playlistContentId;

        public Guid ZuneMediaId => this._zuneMediaId;

        public int ErrorCode
        {
            get => this._errorCode;
            internal set
            {
                if (this._errorCode == value)
                    return;
                this._errorCode = value;
                this.FirePropertyChanged(nameof(ErrorCode));
            }
        }

        public int BurnProgress
        {
            get => this._burnProgress;
            internal set
            {
                if (this._burnProgress == value)
                    return;
                this._burnProgress = value;
                this.FirePropertyChanged(nameof(BurnProgress));
            }
        }

        public bool BurnComplete
        {
            get => this._burnComplete;
            internal set
            {
                if (this._burnComplete == value)
                    return;
                this._burnComplete = value;
                this.FirePropertyChanged(nameof(BurnComplete));
            }
        }

        public bool BurnCanceled
        {
            get => this._burnCanceled;
            internal set
            {
                if (this._burnCanceled == value)
                    return;
                this._burnCanceled = value;
                this.FirePropertyChanged(nameof(BurnCanceled));
            }
        }

        public bool Downloading
        {
            get => this._downloading;
            internal set
            {
                if (this._downloading == value)
                    return;
                this._downloading = value;
                this.FirePropertyChanged(nameof(Downloading));
            }
        }

        public int DownloadProgress
        {
            get => this._downloadProgress;
            internal set
            {
                if (this._downloadProgress == value)
                    return;
                this._downloadProgress = value;
                this.FirePropertyChanged(nameof(DownloadProgress));
            }
        }

        internal int BurnListIndex => this._burnListIndex;
    }
}
