// Decompiled with JetBrains decompiler
// Type: ZuneUI.StringExtractorList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class StringExtractorList : VirtualList, ISearchableList
    {
        private IList _source;
        private ISearchableList _searchableSource;
        private bool _canSearchForString;

        public IList Source
        {
            get => this._source;
            set
            {
                if (this._source == value)
                    return;
                this._source = value;
                this.FirePropertyChanged(nameof(Source));
                this._searchableSource = this._source as ISearchableList;
                this.Reset();
            }
        }

        public bool CanSearchForString
        {
            get => this._canSearchForString;
            set => this._canSearchForString = value;
        }

        protected void Reset()
        {
            this.Clear();
            this.Count = this._source != null ? this._source.Count : 0;
        }

        protected override object OnRequestItem(int index) => this.ExtractString(this._source[index]);

        int ISearchableList.SearchForString(string str) => this._searchableSource != null && this._canSearchForString ? this._searchableSource.SearchForString(str) : -1;

        protected virtual string ExtractString(object item) => item.ToString();
    }
}
