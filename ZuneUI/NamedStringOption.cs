// Decompiled with JetBrains decompiler
// Type: ZuneUI.NamedStringOption
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    public class NamedStringOption : Command
    {
        private string _value;

        public NamedStringOption()
        {
        }

        public NamedStringOption(string description, string value)
          : base((IModelItemOwner)null, description, (EventHandler)null)
          => this._value = value;

        public string Value
        {
            get => this._value;
            set
            {
                if (!(this._value != value))
                    return;
                this._value = value;
                this.FirePropertyChanged(nameof(Value));
            }
        }
    }
}
