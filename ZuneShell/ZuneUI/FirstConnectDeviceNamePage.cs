// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstConnectDeviceNamePage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class FirstConnectDeviceNamePage : FirstConnectPage
    {
        internal FirstConnectDeviceNamePage(Wizard wizard)
          : base(wizard)
          => this.Description = Shell.LoadString(StringId.IDS_NAME_ZUNE_HEADER);

        internal override bool OnMovingNext()
        {
            bool flag = base.OnMovingNext();
            if (this._owner is FirstConnectForPhoneWizard || this._owner is FirstLaunchForPhoneWizard)
                this._owner.CommitChanges();
            return flag;
        }

        public override string UI => "res://ZuneShellResources!FirstConnect.uix#FirstConnectDeviceNamePage";
    }
}
