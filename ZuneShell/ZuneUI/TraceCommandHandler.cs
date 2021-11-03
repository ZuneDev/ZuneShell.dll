// Decompiled with JetBrains decompiler
// Type: ZuneUI.TraceCommandHandler
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class TraceCommandHandler : ICommandHandler
    {
        public void Execute(string command, IDictionary commandArgs) => Trace(command, commandArgs);

        public static void Trace(string command, IDictionary commandArgs)
        {
            if (commandArgs == null)
                return;
            foreach (object key in commandArgs.Keys)
            {
                object commandArg = commandArgs[key];
            }
        }
    }
}
