// Decompiled with JetBrains decompiler
// Type: Microsoft.Zune.Shell.CommandLineArgument
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Zune.Shell
{
    internal struct CommandLineArgument
    {
        public string Name;
        public string Value;

        public CommandLineArgument(string name, string value)
        {
            this.Name = name != null ? name.ToLower(CultureInfo.InvariantCulture) : throw new ArgumentNullException(nameof(name));
            this.Value = value;
        }

        public static CommandLineArgument[] ParseArgs(
          string[] arArgs,
          string stDefaultName)
        {
            List<CommandLineArgument> commandLineArgumentList = new List<CommandLineArgument>();
            foreach (string arArg in arArgs)
            {
                if (string.IsNullOrEmpty(arArg))
                    throw new ArgumentException(nameof(arArgs));
                string str = null;
                string name;
                if (arArg[0] == '-' || arArg[0] == '/')
                    name = arArg.Substring(1, arArg.Length - 1);
                else if (stDefaultName != null)
                    name = stDefaultName + ":" + arArg;
                else
                    continue;
                int length = name.IndexOf(':');
                if (length != -1)
                {
                    str = name.Substring(length + 1).Trim('"');
                    name = name.Substring(0, length);
                }
                CommandLineArgument commandLineArgument = new CommandLineArgument(name, str);
                commandLineArgumentList.Add(commandLineArgument);
            }
            return commandLineArgumentList.ToArray();
        }
    }
}
