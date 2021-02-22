// Decompiled with JetBrains decompiler
// Type: ZuneUI.RichLayoutCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    public class RichLayoutCommand : Command
    {
        private bool _hasRichLayout;

        public RichLayoutCommand(IModelItemOwner owner, string description, bool hasRichLayout)
          : base(owner, description, null)
          => this._hasRichLayout = hasRichLayout;

        public bool HasRichLayout => this._hasRichLayout;
    }
}
