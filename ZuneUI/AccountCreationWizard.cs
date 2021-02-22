// Decompiled with JetBrains decompiler
// Type: ZuneUI.AccountCreationWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    public class AccountCreationWizard : AccountManagementWizard
    {
        private AccountCreationFinishStep _finishStep;
        private AccountManagementErrorPage _errorStep;
        private static bool _inProgress;

        public AccountCreationWizard()
        {
            this.RequiresSignIn = false;
            this.AddPage((WizardPage)this.State.EmailSelectionStep);
            this.AddPage((WizardPage)this.State.PassportPasswordStep);
            this.AddPage((WizardPage)this.State.BasicAccountInfoStep);
            this.AddPage((WizardPage)this.State.CreatePassportStep);
            this.AddPage((WizardPage)this.State.HipPassportStep);
            this.AddPage((WizardPage)this.State.EmailSelectionParentStep);
            this.AddPage((WizardPage)this.State.PassportPasswordParentStep);
            this.AddPage((WizardPage)this.State.CreatePassportParentStep);
            this.AddPage((WizardPage)this.State.HipPassportParentStep);
            this.AddPage((WizardPage)this.State.ContactInfoParentStep);
            this.AddPage((WizardPage)this.State.PaymentInstrumentParentStep);
            this.AddPage((WizardPage)this.State.PrivacyInfoParentStep);
            this.AddPage((WizardPage)this.State.PrivacyInfoStep);
            this.AddPage((WizardPage)this.State.ZuneTagStep);
            this._finishStep = new AccountCreationFinishStep((Wizard)this, this.State);
            this.AddPage((WizardPage)this._finishStep);
            this._errorStep = new AccountManagementErrorPage((Wizard)this, Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ERROR_TITLE), Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_ERROR_DESC));
            this.AddPage((WizardPage)this._errorStep);
        }

        public void InitializeDefaults(
          string defaultUsername,
          string defaultPassword,
          bool lockUsername)
        {
            if (string.IsNullOrEmpty(defaultUsername) || string.IsNullOrEmpty(defaultPassword))
                return;
            this.State.EmailSelectionStep.PageLocked = lockUsername;
            this.State.EmailSelectionStep.SkipOnce = !lockUsername;
            this.State.EmailSelectionStep.HasEmail = true;
            this.State.EmailSelectionStep.Email = defaultUsername;
            this.State.PassportPasswordStep.SkipOnce = true;
            this.State.PassportPasswordStep.CommittedEmail = defaultUsername;
            this.State.PassportPasswordStep.CommittedPassword = defaultPassword;
            this.State.PassportPasswordStep.LockEmail = lockUsername;
        }

        public bool ShowNextSteps
        {
            get => this._finishStep.NextSteps != AccountCreationNextSteps.None;
            set
            {
                AccountCreationNextSteps creationNextSteps = AccountCreationNextSteps.EditTile;
                if (!value)
                    creationNextSteps = AccountCreationNextSteps.None;
                if (this._finishStep.NextSteps == creationNextSteps)
                    return;
                this._finishStep.NextSteps = creationNextSteps;
                this.FirePropertyChanged(nameof(ShowNextSteps));
            }
        }

        public bool HideOnComplete
        {
            get => this._finishStep.HideOnComplete;
            set
            {
                if (this._finishStep.HideOnComplete == value)
                    return;
                this._finishStep.HideOnComplete = value;
                this.FirePropertyChanged(nameof(HideOnComplete));
            }
        }

        protected override bool OnStart()
        {
            this.CurrentPageIndex = -1;
            AccountCreationWizard.AccountCreationInProgress = true;
            return base.OnStart();
        }

        protected override void OnAsyncCommitCompleted(bool success)
        {
            base.OnAsyncCommitCompleted(success);
            if (!this.CommitSucceeded)
                return;
            if (this.State.PassportPasswordStep.IsZuneAccount)
                this._finishStep.ClosingMessage = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_SUCCESS_EXISTING);
            else
                this._finishStep.ClosingMessage = Shell.LoadString(StringId.IDS_ACCOUNT_CREATION_SUCCESS_NEW);
        }

        public override void Cancel()
        {
            AccountCreationWizard.WizardClosed(true);
            base.Cancel();
        }

        public static bool AccountCreationInProgress
        {
            get => AccountCreationWizard._inProgress;
            private set => AccountCreationWizard._inProgress = value;
        }

        public static void WizardClosed(bool isCancelled)
        {
            if (!AccountCreationWizard.AccountCreationInProgress)
                return;
            AccountCreationWizard.AccountCreationInProgress = false;
            if (isCancelled || AccountCreationWizard.CreationCompleted == null)
                return;
            AccountCreationWizard.CreationCompleted((object)null, (EventArgs)null);
        }

        public static event EventHandler CreationCompleted;
    }
}
