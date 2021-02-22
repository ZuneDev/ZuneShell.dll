// Decompiled with JetBrains decompiler
// Type: ZuneUI.TermsOfServiceWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class TermsOfServiceWizard : AccountManagementWizard
    {
        private TermsOfServiceFinishStep _finishStep;
        private AccountManagementErrorPage _errorStep;

        public TermsOfServiceWizard()
        {
            this.RequiresSignIn = false;
            this.State.PassportPasswordParentStep.DetailDescription = Shell.LoadString(StringId.IDS_ACCOUNT_TOS_STEP_PARENT_NEEDED);
            this._finishStep = new TermsOfServiceFinishStep((Wizard)this, this.State);
            this._errorStep = new AccountManagementErrorPage((Wizard)this, Shell.LoadString(StringId.IDS_ACCOUNT_TOS_ERROR_TITLE), Shell.LoadString(StringId.IDS_ACCOUNT_TOS_ERROR_DESC));
            SignIn.Instance.SignInStatusUpdatedEvent += new EventHandler(this.OnSignInStatusUpdatedEvent);
        }

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            if (!disposing)
                return;
            SignIn.Instance.SignInStatusUpdatedEvent -= new EventHandler(this.OnSignInStatusUpdatedEvent);
        }

        public bool ChildAccount
        {
            get => this.State.PassportPasswordParentStep.ForceEnable;
            private set => this.State.PassportPasswordParentStep.ForceEnable = value;
        }

        public string Username
        {
            get => this.State.TermsOfServiceStep.Username;
            private set => this.State.TermsOfServiceStep.Username = value;
        }

        public string Password
        {
            get => this.State.TermsOfServiceStep.Password;
            private set => this.State.TermsOfServiceStep.Password = value;
        }

        public void Initialize(string username, string password, bool childAccount)
        {
            this.Username = username;
            this.Password = password;
            this.ChildAccount = childAccount;
            this.AddPage((WizardPage)this.State.PassportPasswordParentStep);
            this.AddPage((WizardPage)this.State.TermsOfServiceStep);
            this.AddPage((WizardPage)this.State.PrivacyInfoParentStep);
            this.AddPage((WizardPage)this.State.PrivacyInfoStep);
            this.AddPage((WizardPage)this._finishStep);
            this.AddPage((WizardPage)this._errorStep);
        }

        protected override void OnAsyncCommitCompleted(bool success)
        {
            if (success && !SignIn.Instance.SignedIn)
            {
                SignIn.Instance.SignInUser(this.Username, this.Password);
                this._finishStep.LoadStatus = Shell.LoadString(StringId.IDS_LOGON_STATUS_BUTTON);
            }
            else
                base.OnAsyncCommitCompleted(success);
        }

        private void OnSignInStatusUpdatedEvent(object sender, EventArgs e)
        {
            if (!SignIn.Instance.SignInError.IsError && !SignIn.Instance.SignedIn)
                return;
            this._finishStep.ClosingMessage = Shell.LoadString(StringId.IDS_ACCOUNT_TOS_ERROR_SUCCESS_DESC);
            if (!SignIn.Instance.SignInError.IsError)
                this.State.SaveFamilySettings();
            base.OnAsyncCommitCompleted(true);
        }
    }
}
