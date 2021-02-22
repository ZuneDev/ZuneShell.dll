// Decompiled with JetBrains decompiler
// Type: ZuneUI.ConstantSplitSubList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;
using System.Collections.Generic;

namespace ZuneUI
{
    public class ConstantSplitSubList : SubList
    {
        private int _splitSize = 1;

        public int SplitSize
        {
            get => this._splitSize;
            set
            {
                if (this._splitSize == value)
                    return;
                this._splitSize = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(value), "SplitSize must be positive.");
                this.FirePropertyChanged(nameof(SplitSize));
                this.ProduceSubLists();
            }
        }

        protected override List<int> GetSplits()
        {
            if (this.Source == null)
                return (List<int>)null;
            int capacity = this.Source.Count / this._splitSize;
            List<int> intList = new List<int>(capacity);
            for (int index = 0; index < capacity; ++index)
            {
                int num = (index + 1) * this._splitSize;
                intList.Add(num);
            }
            return intList;
        }
    }
}
