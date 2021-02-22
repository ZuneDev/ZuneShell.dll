// Decompiled with JetBrains decompiler
// Type: ZuneUI.DictionaryCommandHandler
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class DictionaryCommandHandler : ICommandHandler
    {
        private Dictionary<string, ICommandHandler> _handlers;
        private string _divider;

        public DictionaryCommandHandler()
        {
            this._handlers = new Dictionary<string, ICommandHandler>();
            this._divider = "\\";
        }

        public IDictionary Handlers => (IDictionary)this._handlers;

        public string Divider
        {
            get => this._divider;
            set => this._divider = !string.IsNullOrEmpty(value) ? value : throw new ArgumentException("Must provide a non-empty divider.", nameof(value));
        }

        public void Execute(string command, IDictionary commandArgs)
        {
            string prefix;
            string suffix;
            this.SplitCommand(command, out prefix, out suffix);
            ICommandHandler commandHandler = (ICommandHandler)null;
            if (this._handlers.ContainsKey(prefix))
                commandHandler = this._handlers[prefix];
            if (commandHandler == null)
                throw new ArgumentException("Unknown prefix: " + prefix, "prefix");
            commandHandler.Execute(suffix, commandArgs);
        }

        private void SplitCommand(string command, out string prefix, out string suffix)
        {
            int length = !string.IsNullOrEmpty(command) ? command.IndexOf(this._divider) : throw new ArgumentException("Must provide a non-empty command", nameof(command));
            if (length < 0)
            {
                prefix = command;
                suffix = (string)null;
            }
            else
            {
                prefix = command.Substring(0, length);
                suffix = command.Substring(length + this._divider.Length);
            }
        }
    }
}
