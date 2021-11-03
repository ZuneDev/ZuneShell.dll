// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstConnectDeviceCustomPrivacyPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class FirstConnectDeviceCustomPrivacyPage : FirstConnectPage
    {
        internal FirstConnectDeviceCustomPrivacyPage(Wizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_SELECT_YOUR_PRIVACY_OPTIONS_HEADER);

        public override string UI => "res://ZuneShellResources!FirstConnect.uix#FirstConnectDeviceCustomPrivacyPage";
    }
}
