// Decompiled with JetBrains decompiler
// Type: ZuneUI.WebUrlCommandHandler
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System;
using System.Collections;

namespace ZuneUI
{
    public class WebUrlCommandHandler : ICommandHandler
    {
        public void Execute(string command, IDictionary commandArgs)
        {
            EPassportPolicyId ePassportPolicy = EPassportPolicyId.None;
            if (commandArgs != null && commandArgs.Contains((object)"PassportPolicyId"))
                ePassportPolicy = (EPassportPolicyId)commandArgs[(object)"PassportPolicyId"];
            else if (SignIn.Instance.SignedIn)
            {
                Uri uri = new Uri(command, UriKind.Absolute);
                if (uri.Host.EndsWith("zune.net", StringComparison.OrdinalIgnoreCase))
                {
                    if (uri.Scheme == "http")
                        ePassportPolicy = EPassportPolicyId.MBI;
                    else if (uri.Scheme == "https")
                        ePassportPolicy = EPassportPolicyId.MBI_SSL;
                }
            }
            ZuneApplication.Service.LaunchBrowserForExternalUrl(command, ePassportPolicy);
        }
    }
}
