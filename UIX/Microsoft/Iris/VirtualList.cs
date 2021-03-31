// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.VirtualList
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Session;
using Microsoft.Iris.ViewItems;
using System;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.Iris
{
    public class VirtualList : ModelItem, IVirtualList, INotifyList, IList, ICollection, IEnumerable
    {
        private static readonly EventCookie s_listContentsChangedEvent = EventCookie.ReserveSlot();
        private int _count;
        private IndexedTree _items;
        private ItemCountHandler _itemCountHandler;
        private RequestItemHandler _requestItemHandler;
        private RequestSlowDataHandler _requestSlowDataHandler;
        private SlowDataAcquireCompleteHandler _slowDataAcquireCompleteHandler;
        private Vector<int> _callbackIndexesList = new Vector<int>(1);
        private UpdateHelper _updater;
        private ReleaseBehavior _releaseBehavior;
        private bool _storeQueryResults;
        private bool _countInitialized;
        private Repeater _repeater;

        public VirtualList(
          IModelItemOwner owner,
          bool enableSlowDataRequests,
          ItemCountHandler countHandler)
          : base(owner)
        {
            this._items = new IndexedTree();
            this._itemCountHandler = countHandler;
            this._storeQueryResults = true;
            if (enableSlowDataRequests)
                this._updater = new UpdateHelper(this);
            this._releaseBehavior = ReleaseBehavior.KeepReference;
        }

        public VirtualList(bool enableSlowDataRequests)
          : this(null, enableSlowDataRequests, null)
        {
        }

        public VirtualList(ItemCountHandler countHandler)
          : this(null, false, countHandler)
        {
        }

        public VirtualList()
          : this(null, false, null)
        {
        }

        public int Count
        {
            get
            {
                using (this.ThreadValidator)
                {
                    if (this._itemCountHandler != null)
                    {
                        ItemCountHandler itemCountHandler = this._itemCountHandler;
                        this._itemCountHandler = null;
                        itemCountHandler(this);
                    }
                    return this._count;
                }
            }
            set
            {
                using (this.ThreadValidator)
                {
                    if (this._count < 0)
                        throw new ArgumentException("Must specify a non-negative Count");
                    this.EnsureNotInCallback("Invalid to set count on virtual list while inside a GetItem callback");
                    this._itemCountHandler = null;
                    if (this._count == value && this._countInitialized)
                        return;
                    this._countInitialized = true;
                    this.Clear();
                    this.SetCount(value);
                    this.FireSetChanged(UIListContentsChangeType.Reset, -1, -1);
                }
            }
        }

        private void SetCount(int value)
        {
            if (this._count == value)
                return;
            this._count = value;
            this.FirePropertyChanged("Count");
            this.OnCountChanged();
        }

        internal virtual void OnCountChanged()
        {
        }

        public int UnsafeGetCount()
        {
            if (this._itemCountHandler != null)
                throw new InvalidOperationException("Cannot get the count since the list is in virtualized (callback) count mode");
            return this._count;
        }

        public bool IsSynchronized
        {
            get
            {
                using (this.ThreadValidator)
                    return false;
            }
        }

        public object SyncRoot
        {
            get
            {
                using (this.ThreadValidator)
                    return null;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                using (this.ThreadValidator)
                    return false;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                using (this.ThreadValidator)
                    return false;
            }
        }

        public object this[int index]
        {
            get
            {
                using (this.ThreadValidator)
                {
                    if (this.IsItemAvailable(index))
                        return this._items[index];
                    this.EnsureNotInCallback(index, "Invalid to fetch item {0} on a VirtualList while already inside a callback to create that item", index);
                    this._callbackIndexesList.Add(index);
                    object obj;
                    try
                    {
                        obj = this._requestItemHandler == null ? this.OnRequestItem(index) : this._requestItemHandler(this, index);
                        if (this._storeQueryResults)
                            this.ModifiedWorker(index, true, obj);
                    }
                    finally
                    {
                        this._callbackIndexesList.Remove(index);
                    }
                    return obj;
                }
            }
            set
            {
                using (this.ThreadValidator)
                {
                    this.EnsureNotInCallback("Invalid to store items on a VirtualList while inside a callback to create an item.  Instead, set StoreQueryResults to true");
                    this.ModifiedWorker(index, true, value);
                }
            }
        }

        public RequestItemHandler RequestItemHandler
        {
            get
            {
                using (this.ThreadValidator)
                    return this._requestItemHandler;
            }
            set
            {
                using (this.ThreadValidator)
                    this._requestItemHandler = value == null || this._requestItemHandler == null ? value : throw new InvalidOperationException("Only one handler may be attached at a time");
            }
        }

        public bool SlowDataRequestsEnabled
        {
            get
            {
                using (this.ThreadValidator)
                    return this._updater != null;
            }
        }

        bool IVirtualList.SlowDataRequestsEnabled => this._updater != null;

        public int SlowDataRequestThrottle
        {
            get
            {
                using (this.ThreadValidator)
                    return this._updater != null ? this._updater.Throttle : int.MaxValue;
            }
            set
            {
                using (this.ThreadValidator)
                {
                    if (this._updater == null)
                        throw new InvalidOperationException("VirtualList is not configured for slow data notifications");
                    this._updater.Throttle = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        public RequestSlowDataHandler RequestSlowDataHandler
        {
            get
            {
                using (this.ThreadValidator)
                    return this._requestSlowDataHandler;
            }
            set
            {
                using (this.ThreadValidator)
                {
                    if (this._updater == null)
                        throw new InvalidOperationException("VirtualList is not configured for slow data notifications");
                    this._requestSlowDataHandler = value == null || this._requestSlowDataHandler == null ? value : throw new InvalidOperationException("Only one handler may be attached at a time");
                }
            }
        }

        public bool StoreQueryResults
        {
            get
            {
                using (this.ThreadValidator)
                    return this._storeQueryResults;
            }
            set
            {
                using (this.ThreadValidator)
                    this._storeQueryResults = value;
            }
        }

        public ReleaseBehavior VisualReleaseBehavior
        {
            get
            {
                using (this.ThreadValidator)
                    return this._releaseBehavior;
            }
            set
            {
                using (this.ThreadValidator)
                    this._releaseBehavior = value;
            }
        }

        public static object UnavailableItem => Repeater.UnavailableItem;

        Repeater IVirtualList.RepeaterHost
        {
            get
            {
                using (this.ThreadValidator)
                    return this._repeater;
            }
            set
            {
                using (this.ThreadValidator)
                {
                    if (value == this._repeater)
                        return;
                    this._repeater = value;
                    if (this._updater == null)
                        return;
                    this._updater.Clear();
                }
            }
        }

        void IVirtualList.RequestItem(int index, ItemRequestCallback callback)
        {
            using (this.ThreadValidator)
            {
                this.ValidateIndex(index);
                if (this.IsItemAvailable(index))
                {
                    callback(this, index, this[index]);
                }
                else
                {
                    this.EnsureNotInCallback(index, "Invalid to fetch item {0} on a VirtualList while already inside a callback to create that item", index);
                    this._callbackIndexesList.Add(index);
                    try
                    {
                        object obj = this._requestItemHandler == null ? this.OnRequestItem(index) : this._requestItemHandler(this, index);
                        if (this._storeQueryResults)
                            this.ModifiedWorker(index, true, obj);
                        callback(this, index, obj);
                    }
                    finally
                    {
                        this._callbackIndexesList.Remove(index);
                    }
                }
            }
        }

        public bool IsItemAvailable(int index)
        {
            using (this.ThreadValidator)
            {
                this.ValidateIndex(index);
                return this.ContainsDataForIndex(index);
            }
        }

        public bool UnsafeGetItem(int index, out object item)
        {
            int count = this.UnsafeGetCount();
            if (!ListUtility.IsValidIndex(index, count))
                throw new IndexOutOfRangeException();
            item = null;
            if (!this._items.Contains(index))
                return false;
            item = this._items[index];
            return true;
        }

        protected virtual object OnRequestItem(int index)
        {
            using (this.ThreadValidator)
            {
                object obj = null;
                if (this.IsItemAvailable(index))
                    obj = this._items[index];
                return obj;
            }
        }

        protected virtual void OnRequestSlowData(int index)
        {
        }

        public void NotifySlowDataAcquireComplete(int index)
        {
            using (this.ThreadValidator)
            {
                if (this.IsDisposed)
                    return;
                if (this._updater == null)
                    throw new InvalidOperationException("VirtualList is not configured for slow data notifications");
                bool flag = false;
                if (this._slowDataAcquireCompleteHandler != null)
                    flag = this._slowDataAcquireCompleteHandler(this, index);
                if (flag)
                    return;
                this._updater.NotifySlowDataAcquireComplete(index);
            }
        }

        SlowDataAcquireCompleteHandler IVirtualList.SlowDataAcquireCompleteHandler
        {
            get => this._slowDataAcquireCompleteHandler;
            set => this._slowDataAcquireCompleteHandler = value;
        }

        void IVirtualList.NotifyRequestSlowData(int index)
        {
            if (this._requestSlowDataHandler != null)
                this._requestSlowDataHandler(this, index);
            this.OnRequestSlowData(index);
        }

        protected virtual void OnVisualsCreated(int index)
        {
        }

        void IVirtualList.NotifyVisualsCreated(int index)
        {
            using (this.ThreadValidator)
            {
                if (this._updater != null)
                    this._updater.AddIndex(index);
                this.OnVisualsCreated(index);
            }
        }

        protected virtual void OnVisualsReleased(int index)
        {
        }

        void IVirtualList.NotifyVisualsReleased(int index)
        {
            using (this.ThreadValidator)
            {
                this.ValidateIndex(index);
                if (this._items.Contains(index))
                {
                    switch (this._releaseBehavior)
                    {
                        case ReleaseBehavior.ReleaseReference:
                            if (this._updater != null)
                                this._updater.RemoveIndex(index);
                            this._items.Remove(index);
                            break;
                        case ReleaseBehavior.Dispose:
                            if (this._updater != null)
                                this._updater.RemoveIndex(index);
                            object obj = this._items[index];
                            this._items.Remove(index);
                            this.DisposeItem(obj);
                            break;
                    }
                }
                this.OnVisualsReleased(index);
            }
        }

        public void Clear()
        {
            using (this.ThreadValidator)
            {
                this.EnsureNotInCallback("Invalid to clear list on a VirtualList while inside a callback to fetch items");
                if (this._releaseBehavior == ReleaseBehavior.Dispose)
                {
                    foreach (IndexedTree.TreeEntry treeEntry in this._items)
                        this.DisposeItem(treeEntry.Value);
                }
                this._items.Clear();
                if (this._updater != null)
                    this._updater.Clear();
                this.SetCount(0);
                this.FireSetChanged(UIListContentsChangeType.Clear, -1, -1);
            }
        }

        public void CopyTo(Array array, int index)
        {
            using (this.ThreadValidator)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                int count = this.Count;
                if (array.Rank != 1 || index >= array.Length || array.Length - index < count)
                    throw new ArgumentException("Invalid array and index specified");
                foreach (object obj in this)
                {
                    array.SetValue(obj, index);
                    ++index;
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            using (this.ThreadValidator)
                return new StackIListEnumerator(this);
        }

        public bool Contains(object item)
        {
            using (this.ThreadValidator)
            {
                bool flag = false;
                foreach (object obj in this)
                {
                    if (obj != null && obj.Equals(item) || obj == item)
                    {
                        flag = true;
                        break;
                    }
                }
                return flag;
            }
        }

        public int IndexOf(object item)
        {
            using (this.ThreadValidator)
            {
                IndexedTree.TreeEnumerator enumerator = this._items.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IndexedTree.TreeEntry current = enumerator.Current;
                    if (current.Value != null && current.Value.Equals(item) || current.Value == item)
                        return current.Index;
                }
                return -1;
            }
        }

        public void Remove(object item)
        {
            using (this.ThreadValidator)
            {
                int index = this.IndexOf(item);
                if (index <= -1)
                    return;
                this.RemoveAt(index);
            }
        }

        public void RemoveAt(int index)
        {
            using (this.ThreadValidator)
            {
                this.EnsureNotInCallback("Invalid to remove item {0} from a VirtualList while inside a GetItem callback", index);
                if (index < 0 || index >= this._count)
                    throw new ArgumentException(InvariantString.Format("Invalid index '{0}' passed to RemoveAt", index));
                object obj = null;
                if (this._items.Contains(index))
                {
                    obj = this._items[index];
                    this._items.RemoveIndex(index);
                }
                if (this._updater != null)
                {
                    this._updater.RemoveIndex(index);
                    this._updater.AdjustIndices(index, -1);
                }
                this.SetCount(this._count - 1);
                if (this._releaseBehavior == ReleaseBehavior.Dispose && obj != null)
                    this.DisposeItem(obj);
                this.FireSetChanged(UIListContentsChangeType.Remove, index, -1);
            }
        }

        public void Insert(int index, object item)
        {
            using (this.ThreadValidator)
                this.InsertWorker(index, true, item);
        }

        public void Insert(int index)
        {
            using (this.ThreadValidator)
                this.InsertWorker(index, false, null);
        }

        public int Add()
        {
            using (this.ThreadValidator)
                return this.AddWorker(false, null);
        }

        public int Add(object item)
        {
            using (this.ThreadValidator)
                return this.AddWorker(true, item);
        }

        public void InsertRange(int index, IList items)
        {
            using (this.ThreadValidator)
            {
                if (items == null)
                    throw new ArgumentException("items should be non-null");
                if (items == this)
                    throw new ArgumentException("can't insert a list onto itself");
                if (items.Count <= 0)
                    return;
                this.InsertRangeWorker(index, items, items.Count);
            }
        }

        public void InsertRange(int index, int count)
        {
            using (this.ThreadValidator)
            {
                if (count < 0)
                    throw new ArgumentException("count should be non-negative");
                if (count <= 0)
                    return;
                this.InsertRangeWorker(index, null, count);
            }
        }

        public void AddRange(IList items)
        {
            using (this.ThreadValidator)
            {
                if (items == null)
                    throw new ArgumentException("items should be non-null");
                if (items == this)
                    throw new ArgumentException("can't add an item to itself");
                if (items.Count <= 0)
                    return;
                this.AddRangeWorker(items, items.Count);
            }
        }

        public void AddRange(int count)
        {
            using (this.ThreadValidator)
            {
                if (count < 0)
                    throw new ArgumentException("count should be non-negative");
                if (count <= 0)
                    return;
                this.AddRangeWorker(null, count);
            }
        }

        public void Move(int oldIndex, int newIndex)
        {
            using (this.ThreadValidator)
            {
                object data = this[oldIndex];
                this._items.RemoveIndex(oldIndex);
                this._items.Insert(newIndex, true, data);
                if (this._updater != null)
                {
                    int lowThreshold;
                    int highThreshold;
                    int amt;
                    if (oldIndex < newIndex)
                    {
                        lowThreshold = oldIndex;
                        highThreshold = newIndex;
                        amt = -1;
                    }
                    else
                    {
                        lowThreshold = newIndex;
                        highThreshold = oldIndex;
                        amt = 1;
                    }
                    this._updater.RemoveIndex(oldIndex);
                    this._updater.AdjustIndices(lowThreshold, highThreshold, amt);
                    this._updater.AddIndex(newIndex);
                }
                this.FireSetChanged(UIListContentsChangeType.Move, oldIndex, newIndex);
            }
        }

        public void Modified(int index)
        {
            using (this.ThreadValidator)
                this.ModifiedWorker(index, false, null);
        }

        [Conditional("DEBUG")]
        internal void DumpListContents()
        {
            foreach (IndexedTree.TreeEntry treeEntry in this._items)
                ;
        }

        private void ValidateIndex(int index)
        {
            if (!ListUtility.IsValidIndex(index, this.Count))
                throw new IndexOutOfRangeException();
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                using (this.ThreadValidator)
                {
                    if (this._updater != null)
                    {
                        this._updater.Dispose();
                        this._updater = null;
                    }
                    this._items.Clear();
                }
            }
            base.OnDispose(disposing);
        }

        protected bool ContainsDataForIndex(int index)
        {
            using (this.ThreadValidator)
                return this._items.Contains(index);
        }

        private void DisposeItem(object obj)
        {
            if (obj is IDisposable disposable)
                disposable.Dispose();
            else if (obj != VirtualList.UnavailableItem)
                throw new InvalidOperationException(InvariantString.Format("VirtualList {0} was configured with the {1} ReleaseBehavior.  This is only valid if the contents of the list implement IDisposable.  Unable to dispose object: {2}.", this, _releaseBehavior, obj));
        }

        private void InsertWorker(int index, bool setValue, object obj)
        {
            this.EnsureNotInCallback("Invalid to insert item {0} on VirtualList while inside a GetItem callback", index);
            if (index != this.Count)
                this.ValidateIndex(index);
            if (this._updater != null)
                this._updater.AdjustIndices(index, 1);
            this._items.Insert(index, setValue, obj);
            this.SetCount(this._count + 1);
            this.FireSetChanged(UIListContentsChangeType.Insert, -1, index);
        }

        private void InsertRangeWorker(int index, IList values, int count)
        {
            this.EnsureNotInCallback("Invalid to insert items into VirtualList while inside a GetItem callback");
            if (index != this.Count)
                this.ValidateIndex(index);
            if (this._updater != null)
                this._updater.AdjustIndices(index, count);
            if (values != null)
                this.CopyRange(values, index);
            else
                this._items.InsertRange(index, count);
            this.SetCount(this._count + count);
            this.FireSetChanged(UIListContentsChangeType.InsertRange, -1, index, count);
        }

        private int AddWorker(bool setValue, object obj)
        {
            this.EnsureNotInCallback("Invalid to add an item to VirtualList while inside a GetItem callback");
            int count = this._count;
            if (setValue)
                this._items[count] = obj;
            this.SetCount(this._count + 1);
            this.FireSetChanged(UIListContentsChangeType.Add, -1, count);
            return count;
        }

        private void AddRangeWorker(IList values, int count)
        {
            this.EnsureNotInCallback("Invalid to add an item to VirtualList while inside a GetItem callback");
            int count1 = this._count;
            if (values != null)
                this.CopyRange(values, this._count);
            this.SetCount(this._count + count);
            this.FireSetChanged(UIListContentsChangeType.AddRange, -1, count1, count);
        }

        private void CopyRange(IList values, int startIndex)
        {
            int count = values.Count;
            for (int index = 0; index < count; ++index)
                this._items.Insert(startIndex + index, true, values[index]);
        }

        private void ModifiedWorker(int index, bool setValue, object value)
        {
            this.ValidateIndex(index);
            bool flag = this.ContainsDataForIndex(index);
            object obj = this._items[index];
            if (setValue)
                this._items[index] = value;
            else if (this._items.Contains(index))
                this._items.Remove(index);
            if (!flag)
                return;
            this.FireSetChanged(UIListContentsChangeType.Modified, index, index);
        }

        event UIListContentsChangedHandler INotifyList.ContentsChanged
        {
            add
            {
                using (this.ThreadValidator)
                    this.AddEventHandler(VirtualList.s_listContentsChangedEvent, value);
            }
            remove
            {
                using (this.ThreadValidator)
                    this.RemoveEventHandler(VirtualList.s_listContentsChangedEvent, value);
            }
        }

        private event ListContentsChangedHandler ContentsChanged
        {
            add
            {
                using (this.ThreadValidator)
                    this.AddEventHandler(VirtualList.s_listContentsChangedEvent, ListContentsChangedProxy.Thunk(value));
            }
            remove
            {
                using (this.ThreadValidator)
                    this.RemoveEventHandler(VirtualList.s_listContentsChangedEvent, ListContentsChangedProxy.Thunk(value));
            }
        }

        internal void FireSetChanged(UIListContentsChangeType type, int oldIndex, int newIndex) => this.FireSetChanged(type, oldIndex, newIndex, 1);

        internal void FireSetChanged(
          UIListContentsChangeType type,
          int oldIndex,
          int newIndex,
          int count)
        {
            UIDispatcher.VerifyOnApplicationThread();
            UIListContentsChangedHandler eventHandler = (UIListContentsChangedHandler)this.GetEventHandler(VirtualList.s_listContentsChangedEvent);
            if (eventHandler != null)
            {
                UIListContentsChangedArgs args = new UIListContentsChangedArgs(type, oldIndex, newIndex, count);
                eventHandler(this, args);
            }
            this.FirePropertyChanged("ContentsChanged");
        }

        private void EnsureNotInCallback(int indexToVerify, string message)
        {
            if (this._callbackIndexesList.Contains(indexToVerify))
                throw new InvalidOperationException(message);
        }

        private void EnsureNotInCallback(int indexToVerify, string message, int param)
        {
            if (this._callbackIndexesList.Contains(indexToVerify))
                throw new InvalidOperationException(InvariantString.Format(message, param));
        }

        private void EnsureNotInCallback(string message)
        {
            if (this._callbackIndexesList.Count > 0)
                throw new InvalidOperationException(message);
        }

        private void EnsureNotInCallback(string message, int param)
        {
            if (this._callbackIndexesList.Count > 0)
                throw new InvalidOperationException(InvariantString.Format(message, param));
        }
    }
}
