// Decompiled with JetBrains decompiler
// Type: ZuneUI.ListPanel
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System.Collections;

namespace ZuneUI
{
    public class ListPanel : LibraryPanel
    {
        private IList _content;
        private object _selectedItem;

        public ListPanel()
          : this(null)
        {
        }

        public ListPanel(IModelItemOwner owner)
          : base(owner)
        {
        }

        public IList Content
        {
            get => this._content;
            set
            {
                if (this._content == value)
                    return;
                this._content = value;
                this.FirePropertyChanged(nameof(Content));
            }
        }

        public object SelectedItem
        {
            get => this._selectedItem;
            set
            {
                if (this._selectedItem == value)
                    return;
                if (this._selectedItem is ModelItem selectedItem)
                    selectedItem.Selected = false;
                if (value is ModelItem modelItem)
                    modelItem.Selected = true;
                this._selectedItem = value;
                this.FirePropertyChanged(nameof(SelectedItem));
            }
        }

        public virtual IList SelectedLibraryIds
        {
            get => (IList)null;
            set
            {
            }
        }

        public int GetIndexFromLibraryId(int libraryIdToFind)
        {
            int num1 = -1;
            int num2 = 0;
            if (this.Content != null)
            {
                foreach (object obj in Content)
                {
                    if (obj is LibraryDataProviderItemBase providerItemBase && (int)providerItemBase.GetProperty("LibraryId") == libraryIdToFind)
                    {
                        num1 = num2;
                        break;
                    }
                    ++num2;
                }
            }
            return num1;
        }

        public IList ComputeSelectedIndicies()
        {
            ArrayList arrayList1;
            if (this.SelectedLibraryIds != null && this.Content != null)
            {
                arrayList1 = new ArrayList(this.SelectedLibraryIds.Count);
                if (!this.IsContentDisposed())
                {
                    ArrayList arrayList2 = new ArrayList(SelectedLibraryIds);
                    int num = 0;
                    foreach (object obj in Content)
                    {
                        if (obj is LibraryDataProviderItemBase providerItemBase)
                        {
                            int property = (int)providerItemBase.GetProperty("LibraryId");
                            if (arrayList2.Contains(property))
                            {
                                arrayList1.Add(num);
                                arrayList2.Remove(property);
                                if (arrayList2.Count == 0)
                                    break;
                            }
                        }
                        ++num;
                    }
                }
            }
            else
                arrayList1 = new ArrayList();
            return arrayList1;
        }

        internal override void Release()
        {
            this.Content = null;
            base.Release();
        }

        private bool IsContentDisposed()
        {
            if (this.Content is LibraryVirtualList content)
            {
                ZuneQueryList queryList = content.QueryList;
                if (queryList != null)
                    return queryList.IsDisposed;
            }
            return false;
        }
    }
}
