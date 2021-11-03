// Decompiled with JetBrains decompiler
// Type: ZuneUI.SearchableGroupedList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class SearchableGroupedList : GroupedList, ISearchableList
    {
        public SearchableGroupedList(IList source, IComparer comparer, int count)
          : base(source, comparer, count)
        {
        }

        int ISearchableList.SearchForString(string str)
        {
            int num = -1;
            if (this.Source is ISearchableList source)
                num = source.SearchForString(str);
            return num;
        }
    }
}
