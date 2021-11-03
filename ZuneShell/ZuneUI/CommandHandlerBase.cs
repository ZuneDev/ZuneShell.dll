// Decompiled with JetBrains decompiler
// Type: ZuneUI.CommandHandlerBase
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class CommandHandlerBase : ICommandHandler
    {
        private IDictionary _arguments;
        private ICommand _executeCommand;

        public IDictionary Arguments => this._arguments;

        public ICommand ExecuteCommand
        {
            get => this._executeCommand;
            set
            {
                if (this._executeCommand == value)
                    return;
                this._executeCommand = value;
            }
        }

        public void Execute(string command, IDictionary commandArgs)
        {
            this._arguments = commandArgs;
            if (this._executeCommand == null)
                return;
            this._executeCommand.Invoke();
        }
    }
}
