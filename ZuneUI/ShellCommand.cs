// Decompiled with JetBrains decompiler
// Type: ZuneUI.ShellCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public class ShellCommand : Microsoft.Iris.Command
    {
        private string _command;
        private IDictionary _commandArguments;

        public string Command
        {
            get => this._command;
            set
            {
                if (!(this._command != value))
                    return;
                this._command = value;
                this.FirePropertyChanged(nameof(Command));
            }
        }

        public IDictionary CommandArguments
        {
            get
            {
                if (this._commandArguments == null)
                    this.CommandArguments = (IDictionary)new Hashtable();
                return this._commandArguments;
            }
            set
            {
                if (this._commandArguments == value)
                    return;
                this._commandArguments = value;
                this.FirePropertyChanged(nameof(CommandArguments));
            }
        }

        protected override void OnInvoked()
        {
            if (this._command != null)
                ZuneShell.DefaultInstance?.Execute(this._command, this._commandArguments);
            base.OnInvoked();
        }
    }
}
