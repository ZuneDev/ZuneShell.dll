// Decompiled with JetBrains decompiler
// Type: ZuneUI.SearchResultFilterCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    public class SearchResultFilterCommand : Command
    {
        private SearchResultFilterType _type = SearchResultFilterType.Undefined;
        private bool _hasResults;

        internal SearchResultFilterCommand(
          ModelItem owner,
          string description,
          SearchResultFilterType type)
          : base((IModelItemOwner)owner, description, (EventHandler)null)
        {
            this._type = type;
        }

        public SearchResultFilterType Type => this._type;

        public bool HasResults
        {
            get => this._hasResults;
            set
            {
                if (this._hasResults == value)
                    return;
                this._hasResults = value;
                this.FirePropertyChanged(nameof(HasResults));
            }
        }
    }
}
