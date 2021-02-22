// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Shell.ViewTimeLogger
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;

namespace Microsoft.Zune.Shell
{
    public class ViewTimeLogger
    {
        private bool _inCompactMode;
        private bool _shutdown;
        private LogHelper _compactModeLog = new LogHelper(SQMDataId.CompactModeViewTime);
        private LogHelper _mixViewLog = new LogHelper(SQMDataId.MixViewTime);
        private LogHelper _collectionViewLog;
        private static ViewTimeLogger _viewTimeLogger;

        private ViewTimeLogger()
        {
        }

        public void Shutdown()
        {
            this._compactModeLog.Stop();
            this._mixViewLog.Stop();
            if (this._collectionViewLog != null)
                this._collectionViewLog.Stop();
            this._shutdown = true;
        }

        public static ViewTimeLogger Instance
        {
            get
            {
                if (_viewTimeLogger == null)
                    _viewTimeLogger = new ViewTimeLogger();
                return _viewTimeLogger;
            }
        }

        public void ViewChanged(SQMDataId viewSQMId)
        {
            if (this._collectionViewLog != null && viewSQMId != this._collectionViewLog.LogId)
            {
                this._collectionViewLog.Stop();
                this._collectionViewLog = null;
            }
            if (viewSQMId == SQMDataId.Invalid || this._collectionViewLog != null)
                return;
            this._collectionViewLog = new LogHelper(viewSQMId);
            if (this._inCompactMode)
                return;
            this._collectionViewLog.Start();
        }

        public void InCompactMode(bool inCompactMode)
        {
            this._inCompactMode = inCompactMode;
            if (this._inCompactMode)
            {
                this._compactModeLog.Start();
                if (this._collectionViewLog == null)
                    return;
                this._collectionViewLog.Stop();
            }
            else
            {
                this._compactModeLog.Stop();
                if (this._collectionViewLog == null)
                    return;
                this._collectionViewLog.Start();
            }
        }

        public void EnterMixView() => this._mixViewLog.Start();

        public void LeaveMixView() => this._mixViewLog.Stop();

        private class LogHelper
        {
            private readonly SQMDataId _logId;
            private DateTime _start;

            public LogHelper(SQMDataId logId) => this._logId = logId;

            public void Start() => this._start = DateTime.Now;

            public void Stop()
            {
                if (!(this._start != DateTime.MinValue))
                    return;
                SQMLog.Log(this._logId, (int)DateTime.Now.Subtract(this._start).TotalSeconds);
                this._start = DateTime.MinValue;
            }

            public SQMDataId LogId => this._logId;
        }
    }
}
