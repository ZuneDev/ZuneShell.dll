// Decompiled with JetBrains decompiler
// Type: ZuneUI.CustomSubList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class CustomSubList : SubList
    {
        private List<int> _splits;

        public static void AssignSplits(CustomSubList list, IList inList)
        {
            List<int> intList = new List<int>(inList.Count);
            foreach (object obj in inList)
            {
                if (obj is int num)
                    intList.Add(num);
            }
            list.Splits = intList;
        }

        public List<int> Splits
        {
            get => this._splits;
            set
            {
                if (this._splits == value)
                    return;
                if (value != null)
                    this.ValidateSplits(value);
                this._splits = value;
                this.FirePropertyChanged(nameof(Splits));
                this.ProduceSubLists();
            }
        }

        protected override List<int> GetSplits() => this.Splits;
    }
}
