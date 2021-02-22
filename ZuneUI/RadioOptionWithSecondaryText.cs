// Decompiled with JetBrains decompiler
// Type: ZuneUI.RadioOptionWithSecondaryText
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    public class RadioOptionWithSecondaryText : Command
    {
        private string _secondaryText;

        public RadioOptionWithSecondaryText(IModelItemOwner owner, string text, string secondaryText)
          : base(owner, text, null)
          => this._secondaryText = secondaryText;

        public string SecondaryText => this._secondaryText;
    }
}
