// Decompiled with JetBrains decompiler
// Type: ZuneUI.PrivacySettingsWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

namespace ZuneUI
{
    public class PrivacySettingsWizard : AccountManagementWizard
    {
        private AccountManagementFinishStep _finishStep;
        private AccountManagementErrorPage _errorStep;
        private EditPrivacyInfoStep _editPrivacyInfoStep;

        public PrivacySettingsWizard()
        {
            this._editPrivacyInfoStep = new EditPrivacyInfoStep(this, this.State, false, PrivacyInfoSettings.None);
            this._finishStep = new AccountManagementFinishStep(this, this.State, Shell.LoadString(StringId.IDS_ACCOUNT_FINISHED_DESCRIPTION), this.State.PrivacyInfoStep.DetailDescription);
            this._errorStep = new AccountManagementErrorPage(this, Shell.LoadString(StringId.IDS_ACCOUNT_PRIVACY_UPDATE_ERROR_TITLE), Shell.LoadString(StringId.IDS_ACCOUNT_PRIVACY_UPDATE_ERROR_DESC));
            this.AddPage(State.PassportPasswordParentStep);
            this.AddPage(_editPrivacyInfoStep);
            this.AddPage(_finishStep);
            this.AddPage(_errorStep);
        }

        protected override void OnAsyncCommitCompleted(bool success)
        {
            base.OnAsyncCommitCompleted(success);
            this._finishStep.ClosingMessage = Shell.LoadString(StringId.IDS_ACCOUNT_PRIVACY_UPDATE_SUCCESS_DESC);
            if (!success)
                return;
            SignIn.Instance.RefreshAccount();
        }
    }
}
