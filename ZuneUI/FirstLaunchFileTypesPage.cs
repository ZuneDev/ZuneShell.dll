// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstLaunchFileTypesPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class FirstLaunchFileTypesPage : FirstLaunchSoftwareSettingBase
    {
        internal FirstLaunchFileTypesPage(Wizard wizard)
          : base(wizard)
        {
            this.Description = Shell.LoadString(StringId.IDS_ASSOCIATE_FILE_TYPES);
            this.EnableVerticalScrolling = true;
        }

        public override string UI => "res://ZuneShellResources!FirstLaunch.uix#FirstLaunchFileTypesPage";
    }
}
