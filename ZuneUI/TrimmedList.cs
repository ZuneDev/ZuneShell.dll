// Decompiled with JetBrains decompiler
// Type: ZuneUI.TrimmedList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneUI
{
    public class TrimmedList : VirtualList
    {
        private IList _source;
        private int _maxCount;

        public IList Source
        {
            get => this._source;
            set
            {
                if (this._source == value)
                    return;
                this._source = value;
                this.FirePropertyChanged(nameof(Source));
                this.Reset();
            }
        }

        public int MaxCount
        {
            get => this._maxCount;
            set
            {
                if (this._maxCount == value)
                    return;
                this._maxCount = value;
                this.FirePropertyChanged(nameof(MaxCount));
                this.Reset();
            }
        }

        protected void Reset()
        {
            this.Clear();
            this.Count = this._source != null ? Math.Min(this._source.Count, this._maxCount) : 0;
        }

        protected override object OnRequestItem(int index) => this._source[index];
    }
}
