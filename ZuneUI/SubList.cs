// Decompiled with JetBrains decompiler
// Type: ZuneUI.SubList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public abstract class SubList : ModelItem
    {
        private IList _source;
        private List<IList> _subLists;

        public IList Source
        {
            get => this._source;
            set
            {
                if (this._source == value)
                    return;
                this._source = value;
                this.FirePropertyChanged(nameof(Source));
                this.ProduceSubLists();
            }
        }

        public List<IList> SubLists
        {
            get => this._subLists;
            private set
            {
                if (this._subLists == value)
                    return;
                this._subLists = value;
                this.FirePropertyChanged(nameof(SubLists));
            }
        }

        protected void ProduceSubLists()
        {
            int num1 = 0;
            if (this._source != null)
                num1 = this._source.Count;
            List<IList> listList = new List<IList>();
            List<int> splits = this.GetSplits();
            int num2 = 0;
            if (splits != null)
            {
                for (int index = 0; index < splits.Count; ++index)
                {
                    int val1 = splits[index];
                    int begin = Math.Min(num2, num1);
                    int end = Math.Min(val1, num1);
                    listList.Add(new SubListSection(this, begin, end, this._source));
                    num2 = end;
                }
            }
            if (num2 < num1 || num1 == 0)
                listList.Add(new SubListSection(this, num2, num1, this._source));
            this.SubLists = listList;
        }

        protected abstract List<int> GetSplits();

        protected void ValidateSplits(List<int> splits)
        {
            int num = 0;
            foreach (int split in splits)
                num = split >= num ? split : throw new ArgumentException("Split values must be sequential");
        }

        private class SubListSection : VirtualList
        {
            private int _begin;
            private int _end;
            private IList _source;

            public SubListSection(IModelItemOwner owner, int begin, int end, IList source)
              : base(owner, false, null)
            {
                this._begin = begin;
                this._end = end;
                this._source = source;
                this.Count = this._end - this._begin;
            }

            protected override object OnRequestItem(int index) => this._source[index + this._begin];
        }
    }
}
