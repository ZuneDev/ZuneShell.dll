// Decompiled with JetBrains decompiler
// Type: ZuneUI.ContactInformationWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class ContactInformationWizard : AccountManagementWizard
    {
        private AccountManagementFinishStep _finishStep;
        private AccountManagementErrorPage _errorStep;

        public ContactInformationWizard()
        {
            this._finishStep = new AccountManagementFinishStep((Wizard)this, this.State, Shell.LoadString(StringId.IDS_ACCOUNT_FINISHED_DESCRIPTION), this.State.ContactInfoStep.DetailDescription);
            this._errorStep = new AccountManagementErrorPage((Wizard)this, Shell.LoadString(StringId.IDS_ACCOUNT_CONTACT_INFO_ERROR_TITLE), Shell.LoadString(StringId.IDS_ACCOUNT_CONTACT_INFO_ERROR_DESC));
            this.AddPage((WizardPage)this.State.ContactInfoStep);
            this.AddPage((WizardPage)this._finishStep);
            this.AddPage((WizardPage)this._errorStep);
        }

        protected override void OnAsyncCommitCompleted(bool success)
        {
            base.OnAsyncCommitCompleted(success);
            this._finishStep.ClosingMessage = Shell.LoadString(StringId.IDS_ACCOUNT_CONTACT_INFO_SUCCESS_DESC);
        }
    }
}
