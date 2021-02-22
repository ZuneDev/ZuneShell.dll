// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstLaunchDownloadFoldersPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class FirstLaunchDownloadFoldersPage : FirstLaunchSoftwareSettingBase
    {
        internal FirstLaunchDownloadFoldersPage(Wizard wizard)
          : base(wizard)
        {
            this.Description = Shell.LoadString(StringId.IDS_CHANGE_WHERE_ZUNE_STORES_MEDIA);
            this.EnableVerticalScrolling = true;
        }

        public override bool IsEnabled
        {
            get
            {
                if (ZuneShell.DefaultInstance.Management.UsingWin7Libraries)
                    return false;
                return !(this._owner is FirstLaunchForPhoneWizard) || ((FirstLaunchForPhoneWizard)this._owner).IsSoftwareSettingsEnabled;
            }
        }

        public override string UI => "res://ZuneShellResources!FirstLaunch.uix#FirstLaunchDownloadFoldersPage";
    }
}
