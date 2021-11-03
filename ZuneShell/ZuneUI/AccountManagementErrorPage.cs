// Decompiled with JetBrains decompiler
// Type: ZuneUI.AccountManagementErrorPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneUI
{
    public class AccountManagementErrorPage : AccountManagementStep
    {
        private HRESULT hr = HRESULT._S_OK;
        private ServiceError serviceError;

        public AccountManagementErrorPage(Wizard owner, string description, string detailedDescription)
          : base(owner, null, false)
        {
            this.Description = description;
            this.DetailDescription = detailedDescription;
        }

        public override bool IsEnabled => this.hr.IsError || this.serviceError != null && this.serviceError.RootError.IsError;

        public HRESULT Hr => this.serviceError == null || !this.serviceError.RootError.IsError ? this.hr : this.serviceError.RootError;

        public override string UI => "res://ZuneShellResources!AccountCreation.uix#AccountManagementErrorPage";

        internal override bool HandleError(HRESULT hr, AccountManagementErrorState errorState)
        {
            bool flag = false;
            if (hr.IsError || errorState != null && errorState.ServiceError != null && errorState.ServiceError.RootError.IsError)
            {
                flag = true;
                this.hr = hr;
                if (errorState != null)
                    this.serviceError = errorState.ServiceError;
            }
            return flag;
        }
    }
}
