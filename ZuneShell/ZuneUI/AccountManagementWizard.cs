// Decompiled with JetBrains decompiler
// Type: ZuneUI.AccountManagementWizard
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class AccountManagementWizard : Wizard
    {
        private bool _commitSucceeded;
        private bool _commitFailed;
        private bool _requiresSignIn = true;
        private AccountManagementWizardState _state;
        private AccountManagementErrorState _lastErrorState;

        protected AccountManagementWizard()
        {
        }

        public AccountManagementWizardState State
        {
            get
            {
                if (this._state == null)
                    this._state = new AccountManagementWizardState(this);
                return this._state;
            }
        }

        public override bool CanStart => !this.RequiresSignIn || SignIn.Instance.SignedIn;

        public bool RequiresSignIn
        {
            get => this._requiresSignIn;
            set
            {
                if (this._requiresSignIn == value)
                    return;
                this._requiresSignIn = value;
                this.FirePropertyChanged(nameof(RequiresSignIn));
            }
        }

        public bool CommitSucceeded
        {
            get => this._commitSucceeded;
            private set
            {
                if (this._commitSucceeded == value)
                    return;
                this._commitSucceeded = value;
                this.FirePropertyChanged(nameof(CommitSucceeded));
            }
        }

        public bool CommitFailed
        {
            get => this._commitFailed;
            private set
            {
                if (this._commitFailed == value)
                    return;
                this._commitFailed = value;
                this.FirePropertyChanged(nameof(CommitFailed));
            }
        }

        public AccountManagementErrorState LastErrorState
        {
            get => this._lastErrorState;
            private set
            {
                if (this._lastErrorState == value)
                    return;
                this._lastErrorState = value;
                this.FirePropertyChanged("LastServiceError");
            }
        }

        public void NavigateToErrorHandler() => this.NavigateToErrorHandler(this.Error, this.LastErrorState);

        public void NavigateToErrorHandler(HRESULT hr, AccountManagementErrorState errorState)
        {
            int num = -1;
            IList pages = this.Pages;
            int currentPageIndex = this.CurrentPageIndex;
            for (int index1 = 0; index1 < pages.Count; ++index1)
            {
                int index2 = (currentPageIndex + index1) % pages.Count;
                if (num == -1 && pages[index2] is AccountManagementErrorPage)
                    num = index2;
                else if (pages[index2] is AccountManagementStep && ((AccountManagementStep)pages[index2]).HandleError(hr, errorState))
                {
                    num = index2;
                    break;
                }
            }
            if (num < 0)
                return;
            this.CurrentPageIndex = num;
        }

        protected override void OnAsyncCommitCompleted(bool success)
        {
            this.CommitSucceeded = success;
            this.CommitFailed = !success;
        }

        protected override void OnSetError(HRESULT hr, object state) => this.LastErrorState = state as AccountManagementErrorState;
    }
}
