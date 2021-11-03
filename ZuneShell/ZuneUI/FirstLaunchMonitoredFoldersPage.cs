// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstLaunchMonitoredFoldersPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class FirstLaunchMonitoredFoldersPage : FirstLaunchSoftwareSettingBase
    {
        private bool _activated;

        internal FirstLaunchMonitoredFoldersPage(Wizard wizard)
          : base(wizard)
        {
            this.Description = Shell.LoadString(StringId.IDS_CHOOSE_MEDIA_LOCATIONS);
            this.EnableVerticalScrolling = true;
        }

        public override string UI => "res://ZuneShellResources!FirstLaunch.uix#FirstLaunchMonitoredFoldersPage";

        internal override void Activate()
        {
            if (!this._activated)
            {
                this._activated = true;
                ZuneShell.DefaultInstance.Management.MediaInfoChoice.Value = FeatureEnablement.IsFeatureEnabled(Features.eOptIn);
            }
            base.Activate();
        }
    }
}
