// Decompiled with JetBrains decompiler
// Type: ZuneUI.SortCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class SortCommand : Command
    {
        private string _sort;
        private bool _supportsJumpInList;

        public SortCommand()
          : this((string)null, (string)null, false)
        {
        }

        public SortCommand(string description, string sort, bool supportsJumpInList)
        {
            this.Description = description;
            this.Sort = sort;
            this.SupportsJumpInList = supportsJumpInList;
        }

        public string Sort
        {
            get => this._sort;
            set
            {
                if (!(this._sort != value))
                    return;
                this._sort = value;
                this.FirePropertyChanged(nameof(Sort));
            }
        }

        public bool SupportsJumpInList
        {
            get => this._supportsJumpInList;
            set
            {
                if (this._supportsJumpInList == value)
                    return;
                this._supportsJumpInList = value;
                this.FirePropertyChanged(nameof(SupportsJumpInList));
            }
        }
    }
}
