// Decompiled with JetBrains decompiler
// Type: ZuneUI.DatapointInfo
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    internal class DatapointInfo
    {
        private ETelemetryEvent _event;
        private string _typeName;
        private bool _fSession;

        public DatapointInfo(ETelemetryEvent evt)
        {
            this._event = evt;
            this._typeName = "";
            this._fSession = false;
        }

        public DatapointInfo(ETelemetryEvent evt, string typeName, bool fSess)
        {
            this._event = evt;
            this._typeName = typeName;
            this._fSession = fSess;
        }

        public ETelemetryEvent Event
        {
            get => this._event;
            set => this._event = value;
        }

        public string TypeName
        {
            get => this._typeName;
            set => this._typeName = value;
        }

        public bool IsSession
        {
            get => this._fSession;
            set => this._fSession = value;
        }
    }
}
