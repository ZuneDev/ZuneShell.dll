// Decompiled with JetBrains decompiler
// Type: ZuneUI.WlanCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class WlanCommand : Command
    {
        private WlanProfile _profile;

        public WlanCommand(WlanProfile profile)
        {
            this._profile = profile;
            this.Description = profile.SSID;
        }

        public WlanProfile Profile => this._profile;

        public bool NeedsKey => this.Profile != null && this.Profile.Cipher != WirelessCiphers.None && (!this.Profile.Encrypted && string.IsNullOrEmpty(this.Profile.Key) || this.Profile.Encrypted && (this.Profile.EncryptedKey == null || this.Profile.EncryptedKey.Length == 0));

        public override string ToString() => this.Description;
    }
}
