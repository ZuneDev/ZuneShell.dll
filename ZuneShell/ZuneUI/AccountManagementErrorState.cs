// Decompiled with JetBrains decompiler
// Type: ZuneUI.AccountManagementErrorState
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneUI
{
    public class AccountManagementErrorState
    {
        private bool _parentAccount;
        private ServiceError _serviceError;

        internal AccountManagementErrorState(bool parentAccount, ServiceError serviceError)
        {
            this._parentAccount = parentAccount;
            this._serviceError = serviceError;
        }

        public bool ParentAccount => this._parentAccount;

        public ServiceError ServiceError => this._serviceError;
    }
}
