// Decompiled with JetBrains decompiler
// Type: ZuneUI.HelpCommandHandler
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Win32;
using Microsoft.Zune.Service;
using Microsoft.Zune.Shell;
using System.Collections;

namespace ZuneUI
{
    public class HelpCommandHandler : ICommandHandler
    {
        public void Execute(string command, IDictionary commandArgs) => ZuneApplication.Service.LaunchBrowserForExternalUrl((string)Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Zune").GetValue("Installation Directory") + command, EPassportPolicyId.None);
    }
}
