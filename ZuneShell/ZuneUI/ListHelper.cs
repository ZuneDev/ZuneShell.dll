// Decompiled with JetBrains decompiler
// Type: ZuneUI.ListHelper
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public static class ListHelper
    {
        public static IList Merge(
          IList list1,
          IList list2,
          bool matchesOnly,
          IComparer comparer,
          MergeObjects mergeDelegate)
        {
            ArrayList arrayList = new ArrayList();
            if (list1 != null && list2 != null)
            {
                for (int index1 = 0; index1 < list1.Count; ++index1)
                {
                    bool flag = false;
                    for (int index2 = 0; index2 < list2.Count; ++index2)
                    {
                        if (comparer.Compare(list1[index1], list2[index2]) == 0)
                        {
                            arrayList.Add(mergeDelegate(list1[index1], list2[index2]));
                            list2.RemoveAt(index2);
                            flag = true;
                            break;
                        }
                    }
                    if (!flag && !matchesOnly)
                        arrayList.Add(mergeDelegate(list1[index1], null));
                }
                if (!matchesOnly)
                {
                    foreach (object obj in list2)
                        arrayList.Add(mergeDelegate(obj, null));
                }
            }
            else if (!matchesOnly && (list1 != null || list2 != null))
            {
                foreach (object obj in list1 != null ? list1 : list2)
                    arrayList.Add(mergeDelegate(obj, null));
            }
            return arrayList;
        }

        public delegate object MergeObjects(object item1, object item2);
    }
}
