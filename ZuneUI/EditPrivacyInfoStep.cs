// Decompiled with JetBrains decompiler
// Type: ZuneUI.EditPrivacyInfoStep
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using Microsoft.Zune.Util;

namespace ZuneUI
{
    public class EditPrivacyInfoStep : PrivacyInfoStep
    {
        public EditPrivacyInfoStep(
          Wizard owner,
          AccountManagementWizardState state,
          bool parentAccount,
          PrivacyInfoSettings showSettings)
          : base(owner, state, parentAccount, showSettings)
        {
        }

        public override bool IsEnabled => this._owner.CurrentPage != this || !this.ServiceRequestWorking;

        public override bool ShowPrivacyStatement => true;

        protected override bool OnCommitChanges()
        {
            bool flag = true;
            if (this.CommittedSettings != null)
            {
                HRESULT hr = HRESULT._S_OK;
                ServiceError serviceError = null;
                if (this.PrivacySettings != null && this.PrivacySettings.Count > 0)
                    hr = this.State.AccountManagement.SetPrivacySettings(this.CommittedSettings, this.State.PassportPasswordParentStep.PassportIdentity, out serviceError);
                if (hr.IsSuccess && this.FamilySettings != null && (this.FamilySettings.Settings != null && this.FamilySettings.Settings.Count > 0))
                    this.FamilySettings.CommitSettings();
                if (SignIn.Instance.FamilySettings != null)
                    SignIn.Instance.FamilySettings.ReloadSettings();
                if (hr.IsSuccess && (this.AllowMicrosoftCommunications != null || this.AllowPartnerCommunications != null))
                    hr = this.State.AccountManagement.SetNewsLetterSettings(this.CommittedSettings, out serviceError);
                if (!hr.IsSuccess)
                {
                    flag = false;
                    this.SetError(hr, serviceError);
                }
            }
            return flag;
        }

        protected override void OnActivate()
        {
            this.ServiceActivationRequestsDone = this.CommittedSettings != null;
            base.OnActivate();
        }

        protected override void OnStartActivationRequests(object state)
        {
            this.InitializeHelpersOnWorkerThread();
            this.EndActivationRequests(this.ObtainAccountUser());
        }

        protected override void OnEndActivationRequests(object args)
        {
            if (args == null)
            {
                this.NavigateToErrorHandler();
            }
            else
            {
                this.InitializeSettings();
                AccountUser accountUser = (AccountUser)args;
                this.CommittedSettings = accountUser.AccountSettings;
                if (this.ShowSettings != PrivacyInfoSettings.None)
                    return;
                if (accountUser.AccountUserType == AccountUserType.Adult)
                    this.ShowSettings = PrivacyInfoSettings.SocialSettings;
                else if (accountUser.AccountUserType == AccountUserType.ChildWithSocial)
                    this.ShowSettings = FeatureEnablement.IsFeatureEnabled(Features.eSocial) ? PrivacyInfoSettings.CreateChildAccountWithSocial : PrivacyInfoSettings.CreateChildAccount;
                else
                    this.ShowSettings = PrivacyInfoSettings.CreateChildAccount;
            }
        }

        private AccountUser ObtainAccountUser()
        {
            AccountUser accountUser = null;
            ServiceError serviceError = null;
            HRESULT account = this.State.AccountManagement.GetAccount(null, out accountUser, out serviceError);
            if (account.IsError)
            {
                accountUser = null;
                this.SetError(account, serviceError);
            }
            return accountUser;
        }
    }
}
