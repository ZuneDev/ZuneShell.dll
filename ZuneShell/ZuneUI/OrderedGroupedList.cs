// Decompiled with JetBrains decompiler
// Type: ZuneUI.OrderedGroupedList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneUI
{
    public class OrderedGroupedList : SearchableGroupedList
    {
        public OrderedGroupedList(IList source, IComparer comparer, int count)
          : base(null, comparer, count)
          => this.Reorder(source, count);

        protected override void OnDispose(bool disposing)
        {
            if (disposing && this.Source is ArrayListDataSet)
                ((ModelItem)this.Source).Dispose();
            base.OnDispose(disposing);
        }

        public void Reorder(IList source, int count)
        {
            ArrayListDataSet arrayListDataSet = null;
            if (source != null)
            {
                arrayListDataSet = new ArrayListDataSet();
                arrayListDataSet.CopyFrom(source);
                int newIndex;
                for (int itemIndex = 0; itemIndex < arrayListDataSet.Count; itemIndex = newIndex + 1)
                {
                    newIndex = itemIndex;
                    for (int index = itemIndex + 1; index < arrayListDataSet.Count; ++index)
                    {
                        if (this.Comparer.Compare(arrayListDataSet[itemIndex], arrayListDataSet[index]) == 0)
                        {
                            ++newIndex;
                            if (newIndex != index)
                                arrayListDataSet.Move(index, newIndex);
                        }
                    }
                }
            }
            if (this.Source is ArrayListDataSet)
                ((ModelItem)this.Source).Dispose();
            this.SetSource(arrayListDataSet, count);
        }
    }
}
