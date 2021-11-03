// Decompiled with JetBrains decompiler
// Type: ZuneUI.MessageDetailsPanel
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneUI
{
    public class MessageDetailsPanel : LibraryPanel
    {
        private DataProviderObject _selectedItem;
        private string _title;

        internal MessageDetailsPanel(LibraryPage page, bool showHeaderAndFooter)
          : base(page)
        {
        }

        public DataProviderObject SelectedItem
        {
            get => this._selectedItem;
            set
            {
                if (this._selectedItem == value)
                    return;
                this._selectedItem = value;
                this.FirePropertyChanged(nameof(SelectedItem));
                this.Title = null;
            }
        }

        public string Title
        {
            get => this._title;
            set
            {
                if (!(this._title != value))
                    return;
                this._title = value;
                this.FirePropertyChanged(nameof(Title));
            }
        }
    }
}
