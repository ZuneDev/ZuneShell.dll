// Decompiled with JetBrains decompiler
// Type: ZuneUI.SearchableAggregateList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class SearchableAggregateList : AggregateList, ISearchableList
    {
        private IList[] _lists;
        private int _searchListIndex;

        public SearchableAggregateList(IList list1, IList list2, int searchListIndex)
          : base(list1, list2)
        {
            this._lists = new IList[2] { list1, list2 };
            this._searchListIndex = searchListIndex;
        }

        int ISearchableList.SearchForString(string str)
        {
            int num = -1;
            if (this._lists[this._searchListIndex] is ISearchableList list)
            {
                num = list.SearchForString(str);
                if (num >= 0)
                {
                    for (int index = 0; index < this._searchListIndex; ++index)
                        num += this._lists[index].Count;
                }
            }
            return num;
        }
    }
}
