// Decompiled with JetBrains decompiler
// Type: ZuneUI.MixPriorityList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;

namespace ZuneUI
{
    public abstract class MixPriorityList
    {
        private MixResult _mixResultSeed;
        private System.Collections.Generic.List<PriorityResult> _list;
        private System.Collections.Generic.List<MixResult> _sortedList;

        protected MixPriorityList(MixResult mixResultSeed)
        {
            this._list = new System.Collections.Generic.List<PriorityResult>();
            this._mixResultSeed = mixResultSeed;
        }

        public void Add(DataProviderObject item, string reason) => this.AddList(new System.Collections.Generic.List<DataProviderObject>()
    {
      item
    }, reason, 1);

        public abstract void AddList(IList sourceList, string reason, int maxItems);

        public IList List
        {
            get
            {
                if (this._sortedList == null)
                {
                    this._sortedList = new System.Collections.Generic.List<MixResult>(this._list.Count);
                    this._list.Sort();
                    foreach (PriorityResult priorityResult in this._list)
                        this._sortedList.Add(priorityResult.Result);
                }
                return _sortedList;
            }
        }

        protected void AddList(
          IList sourceList,
          string reason,
          int maxItems,
          GetItemPriorityDelegate getItemPriorityDelegate,
          CreateItemInstanceDelegate createItemInstanceDelegate)
        {
            int startPriority = 0;
            foreach (DataProviderObject source in sourceList)
            {
                if (this.Add(createItemInstanceDelegate(source, reason), getItemPriorityDelegate(source, startPriority)))
                    ++startPriority;
                if (startPriority >= maxItems)
                    break;
            }
        }

        protected bool Add(MixResult newResult, int priority)
        {
            bool flag = false;
            if (newResult.IsDuplicate(this._mixResultSeed))
                flag = true;
            if (!flag && newResult.ResultType == MixResultType.Profile)
                flag = SignIn.Instance.IsSignedInUser(newResult.Id);
            if (!flag)
            {
                foreach (PriorityResult priorityResult in this._list)
                {
                    if (newResult.IsDuplicate(priorityResult.Result))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
            {
                this._list.Add(new PriorityResult(priority, newResult));
                this._sortedList = null;
            }
            return !flag;
        }

        protected class PriorityResult : IComparable
        {
            public int Priority;
            public MixResult Result;

            public PriorityResult(int priority, MixResult result)
            {
                this.Priority = priority;
                this.Result = result;
            }

            public int CompareTo(object obj) => this.Priority - (obj as PriorityResult).Priority;
        }

        protected delegate int GetItemPriorityDelegate(DataProviderObject item, int startPriority);

        protected delegate MixResult CreateItemInstanceDelegate(
          DataProviderObject item,
          string reason);
    }
}
