// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.AggregateList
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using System;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.Iris
{
    public class AggregateList : VirtualList
    {
        private IList[] _lists;

        public AggregateList()
          : this((IList[])null)
        {
        }

        public AggregateList(IList list1)
          : this(new IList[1] { list1 })
        {
        }

        public AggregateList(IList list1, IList list2)
          : this(new IList[2] { list1, list2 })
        {
        }

        public AggregateList(IList list1, IList list2, IList list3)
          : this(new IList[3] { list1, list2, list3 })
        {
        }

        public AggregateList(IList list1, IList list2, IList list3, IList list4)
          : this(new IList[4] { list1, list2, list3, list4 })
        {
        }

        public AggregateList(IList[] lists)
          : base(true)
        {
            if (lists == null)
                lists = new IList[0];
            this._lists = lists;
            UIListContentsChangedHandler contentsChangedHandler = new UIListContentsChangedHandler(this.ChildListModified);
            SlowDataAcquireCompleteHandler acquireCompleteHandler = new SlowDataAcquireCompleteHandler(this.ChildListSlowDataAcquired);
            for (int index = 0; index < this._lists.Length; ++index)
            {
                if (this._lists[index] == null)
                    this._lists[index] = (IList)new ArrayList();
                IList list = this._lists[index];
                if (list is INotifyList notifyList)
                    notifyList.ContentsChanged += contentsChangedHandler;
                if (list is IVirtualList virtualList && virtualList.SlowDataRequestsEnabled)
                    virtualList.SlowDataAcquireCompleteHandler = acquireCompleteHandler;
            }
            this.Count = this.ItemCount;
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                UIListContentsChangedHandler contentsChangedHandler = new UIListContentsChangedHandler(this.ChildListModified);
                foreach (IList list in this._lists)
                {
                    if (list is INotifyList notifyList)
                        notifyList.ContentsChanged -= contentsChangedHandler;
                    if (list is IVirtualList virtualList && virtualList.SlowDataRequestsEnabled)
                        virtualList.SlowDataAcquireCompleteHandler = (SlowDataAcquireCompleteHandler)null;
                }
            }
            base.OnDispose(disposing);
        }

        protected override object OnRequestItem(int index)
        {
            foreach (IList list in this._lists)
            {
                if (index < list.Count)
                    return list[index];
                index -= list.Count;
            }
            throw new IndexOutOfRangeException(nameof(index));
        }

        protected override void OnRequestSlowData(int index)
        {
            foreach (IList list in this._lists)
            {
                if (index < list.Count)
                {
                    if (list is IVirtualList virtualList && virtualList.SlowDataRequestsEnabled)
                    {
                        virtualList.NotifyRequestSlowData(index);
                        break;
                    }
                    break;
                }
                index -= list.Count;
            }
            this.NotifySlowDataAcquireComplete(index);
        }

        private bool ChildListSlowDataAcquired(IVirtualList childList, int index)
        {
            this.NotifySlowDataAcquireComplete(this.ListIndexToMasterIndex((IList)childList, index));
            return true;
        }

        private void ChildListModified(IList listSender, UIListContentsChangedArgs args)
        {
            int masterIndex1 = this.ListIndexToMasterIndex(listSender, args.OldIndex);
            int masterIndex2 = this.ListIndexToMasterIndex(listSender, args.NewIndex);
            switch (args.Type)
            {
                case UIListContentsChangeType.Add:
                case UIListContentsChangeType.Insert:
                    this.Insert(masterIndex2);
                    break;
                case UIListContentsChangeType.AddRange:
                case UIListContentsChangeType.InsertRange:
                    this.InsertRange(masterIndex2, args.Count);
                    break;
                case UIListContentsChangeType.Remove:
                    this.RemoveAt(masterIndex1);
                    break;
                case UIListContentsChangeType.Move:
                    if (masterIndex1 == masterIndex2)
                        break;
                    this.Move(masterIndex1, masterIndex2);
                    break;
                case UIListContentsChangeType.Modified:
                    this.Modified(masterIndex2);
                    break;
                default:
                    this.Clear();
                    this.Count = this.ItemCount;
                    break;
            }
        }

        [Conditional("DEBUG")]
        private void DEBUG_ValidateConsistency()
        {
            int index1 = 0;
            for (int index2 = 0; index2 < this._lists.Length; ++index2)
            {
                for (int index3 = 0; index3 < this._lists[index2].Count; ++index3)
                {
                    object obj1 = this._lists[index2][index3];
                    object obj2 = this[index1];
                    ++index1;
                }
            }
        }

        private int ListIndexToMasterIndex(IList list, int index)
        {
            int num = 0;
            for (int index1 = 0; index1 < this._lists.Length && this._lists[index1] != list; ++index1)
                num += this._lists[index1].Count;
            return num + index;
        }

        private void MasterIndexToListIndex(int masterIndex, out IList list, out int index)
        {
            int num = 0;
            int index1 = 0;
            list = (IList)null;
            for (; index1 < this._lists.Length; ++index1)
            {
                list = this._lists[index1];
                if (num + list.Count <= masterIndex)
                    num += list.Count;
                else
                    break;
            }
            index = masterIndex - num;
        }

        private int ItemCount
        {
            get
            {
                int num = 0;
                foreach (IList list in this._lists)
                    num += list.Count;
                return num;
            }
        }
    }
}
