// Decompiled with JetBrains decompiler
// Type: ZuneUI.WirelessType
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using MicrosoftZuneLibrary;

namespace ZuneUI
{
    internal class WirelessType
    {
        private WlanAuthCipherPair _type;
        private string _description;
        private bool _newGroup;
        private bool _alwaysSupported;
        private bool _displayType;

        public WirelessType(
          WlanAuthCipherPair type,
          string description,
          bool displayType,
          bool alwaysSupported,
          bool newGroup)
        {
            this._type = type;
            this._description = description;
            this._alwaysSupported = alwaysSupported;
            this._newGroup = newGroup;
            this._displayType = displayType;
        }

        public bool NewGroup => this._newGroup;

        public bool AlwaysSupported => this._alwaysSupported;

        public WlanAuthCipherPair Type => this._type;

        public string Description => this._description;

        public bool DisplayType => this._displayType;
    }
}
