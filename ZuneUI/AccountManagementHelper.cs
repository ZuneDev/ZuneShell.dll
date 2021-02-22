// Decompiled with JetBrains decompiler
// Type: ZuneUI.AccountManagementHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;

namespace ZuneUI
{
    internal static class AccountManagementHelper
    {
        internal static HRESULT GetPassportIdentity(
          string username,
          string password,
          out PassportIdentity passportIdentity)
        {
            return Microsoft.Zune.Service.Service.Instance.AuthenticatePassport(username, password, EPassportPolicyId.MBI_SSL, out passportIdentity);
        }
    }
}
