// Decompiled with JetBrains decompiler
// Type: ZuneUI.CurrentPageCommandHandler
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;

namespace ZuneUI
{
    public class CurrentPageCommandHandler : ICommandHandler
    {
        public void Execute(string command, IDictionary commandArgs)
        {
            ZuneShell defaultInstance = ZuneShell.DefaultInstance;
            if (defaultInstance == null)
                throw new InvalidOperationException("No Shell instance has been registered.  Unable to perform navigation.");
            defaultInstance.CurrentPage.CommandHandler?.Execute(command, commandArgs);
        }
    }
}
