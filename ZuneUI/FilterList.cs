// Decompiled with JetBrains decompiler
// Type: ZuneUI.FilterList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public abstract class FilterList : ModelItem
    {
        private IList _source;
        private IList _filteredList;

        public IList Source
        {
            get => this._source;
            set
            {
                if (this._source == value)
                    return;
                this._source = value;
                this.FirePropertyChanged(nameof(Source));
                this.ProduceFilteredList();
            }
        }

        public IList FilteredList
        {
            get => this._filteredList;
            private set
            {
                if (this._filteredList == value)
                    return;
                this._filteredList = value;
                this.FirePropertyChanged(nameof(FilteredList));
            }
        }

        protected void ProduceFilteredList()
        {
            IList list = (IList)null;
            if (this._source != null)
            {
                list = (IList)new ArrayListDataSet();
                for (int index = 0; index < this._source.Count; ++index)
                {
                    object obj = this._source[index];
                    int count = list.Count;
                    if (this.ShouldIncludeItem(index, count, obj))
                        list.Insert(count, obj);
                }
            }
            this.FilteredList = list;
        }

        protected abstract bool ShouldIncludeItem(int sourceIndex, int targetIndex, object item);
    }
}
