// Decompiled with JetBrains decompiler
// Type: ZuneUI.ProfileMerger
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneUI
{
    public static class ProfileMerger
    {
        public static IList Merge(IList profiles1, IList profiles2, bool matchesOnly) => Merge(profiles1, profiles2, matchesOnly, new DataProviderPropertyComparer()
        {
            PropertyName = "ZuneTag"
        }, new MergeObjects(MergeProfile));

        public static IList MergeWithFriends(IList profiles, IList friends, bool matchesOnly) => Merge(profiles, friends, matchesOnly, new DataProviderPropertyComparer()
        {
            PropertyName = "ZuneTag"
        }, new MergeObjects(MergeWithFriend));

        public static ProfileCardData MergeWithFriends(
          DataProviderObject profile,
          IList friends)
        {
            return MergeWithFriends(new object[1]
            {
         profile
            }, friends, false)[0] as ProfileCardData;
        }

        public static object MergeProfile(object item1, object item2) => ProfileCardData.Create(item1, item2);

        private static object MergeWithFriend(object profile, object friend)
        {
            ProfileCardData profileCardData = ProfileCardData.Create(profile, friend);
            if (friend != null)
                profileCardData.IsFriend = true;
            return profileCardData;
        }

        private static IList Merge(
          IList list1,
          IList list2,
          bool matchesOnly,
          IComparer comparer,
          MergeObjects mergeDelegate)
        {
            ArrayList arrayList;
            if (list1 != null && list2 != null)
            {
                arrayList = new ArrayList(Math.Min(list1.Count, list2.Count));
                Hashtable hashtable = new Hashtable();
                int num = 0;
                for (int index1 = 0; index1 < list1.Count; ++index1)
                {
                    bool flag = false;
                    for (int index2 = num; index2 < list2.Count; ++index2)
                    {
                        if (!hashtable.ContainsKey(index2) && comparer.Compare(list1[index1], list2[index2]) == 0)
                        {
                            arrayList.Add(mergeDelegate(list1[index1], list2[index2]));
                            hashtable.Add(index2, true);
                            if (num == index2)
                                ++num;
                            flag = true;
                            break;
                        }
                    }
                    if (!flag && !matchesOnly)
                        arrayList.Add(mergeDelegate(list1[index1], null));
                }
            }
            else if (!matchesOnly && list1 != null)
            {
                arrayList = new ArrayList(list1.Count);
                foreach (object obj in list1)
                    arrayList.Add(mergeDelegate(obj, null));
            }
            else
                arrayList = new ArrayList();
            return arrayList;
        }

        public delegate object MergeObjects(object item1, object item2);
    }
}
