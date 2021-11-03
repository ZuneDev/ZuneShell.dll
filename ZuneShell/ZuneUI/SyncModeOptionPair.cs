// Decompiled with JetBrains decompiler
// Type: ZuneUI.SyncModeOptionPair
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class SyncModeOptionPair
    {
        private string _name;
        private SyncMode _mode;

        public SyncModeOptionPair(string name, SyncMode mode)
        {
            this._name = name;
            this._mode = mode;
        }

        public string Name
        {
            get => this._name;
            set => this._name = value;
        }

        public SyncMode Mode
        {
            get => this._mode;
            set => this._mode = value;
        }

        public override string ToString() => this.Name;

        public static implicit operator string(SyncModeOptionPair pair) => pair.ToString();
    }
}
