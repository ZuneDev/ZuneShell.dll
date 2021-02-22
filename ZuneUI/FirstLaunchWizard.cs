// Decompiled with JetBrains decompiler
// Type: ZuneUI.FirstLaunchWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class FirstLaunchWizard : Wizard
    {
        public FirstLaunchWizard()
        {
            this.AddPage((WizardPage)new FirstLaunchWelcomePage((Wizard)this));
            this.AddPage((WizardPage)new FirstLaunchMonitoredFoldersPage((Wizard)this));
            this.AddPage((WizardPage)new FirstLaunchDownloadFoldersPage((Wizard)this));
            this.AddPage((WizardPage)new FirstLaunchFileTypesPage((Wizard)this));
            this.AddPage((WizardPage)new FirstLaunchPrivacyPage((Wizard)this));
        }

        public override bool CanCommitChanges
        {
            get
            {
                if (!this.IsValid)
                    return false;
                return !this.CanAdvancePageIndex || this.CurrentPage is FirstLaunchWelcomePage;
            }
        }

        protected override bool OnCommitChanges()
        {
            if (this.CurrentPage is FirstLaunchWelcomePage)
            {
                ZuneShell.DefaultInstance.Management.MediaInfoChoice.Value = true;
                ZuneShell.DefaultInstance.Management.SqmChoice.Value = true;
                SQMLog.Log(SQMDataId.UserConfiguredSettingsFUE, 0);
            }
            else
                SQMLog.Log(SQMDataId.UserConfiguredSettingsFUE, 1);
            ZuneShell.DefaultInstance.Management.CommitListSave();
            Fue.Instance.MigrateLegacyConfiguration();
            Fue.Instance.CompleteFUE();
            return base.OnCommitChanges();
        }
    }
}
