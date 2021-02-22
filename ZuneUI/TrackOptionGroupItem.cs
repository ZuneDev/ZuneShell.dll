// Decompiled with JetBrains decompiler
// Type: ZuneUI.TrackOptionGroupItem
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class TrackOptionGroupItem
    {
        private TrackMetadata _trackMetadata;
        private bool _original;

        public TrackMetadata TrackMetadata
        {
            get => this._trackMetadata;
            set => this._trackMetadata = value;
        }

        public bool Original
        {
            get => this._original;
            set => this._original = value;
        }
    }
}
