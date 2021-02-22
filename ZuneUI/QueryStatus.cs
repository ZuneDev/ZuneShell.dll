// Decompiled with JetBrains decompiler
// Type: ZuneUI.QueryStatus
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class QueryStatus
    {
        private string _name;
        private DataProviderQueryStatus _status;

        public QueryStatus(string name, DataProviderQueryStatus status)
        {
            this._name = name;
            this._status = status;
        }

        public string Title => this._name;

        public DataProviderQueryStatus Status => this._status;
    }
}
