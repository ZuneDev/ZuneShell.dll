// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ListDataSet
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using System;
using System.Collections;

namespace Microsoft.Iris
{
    public class ListDataSet : ModelItem, INotifyList, IList, ICollection, IEnumerable
    {
        private static readonly EventCookie s_listContentsChangedEvent = EventCookie.ReserveSlot();
        private IList _sourceList;

        protected ListDataSet()
          : this(null)
        {
        }

        public ListDataSet(IList source)
          : this(null, source)
        {
        }

        public ListDataSet(IModelItemOwner owner, IList source)
          : base(owner)
          => this._sourceList = source;

        public IList Source
        {
            get
            {
                using (this.ThreadValidator)
                    return this._sourceList;
            }
            set
            {
                using (this.ThreadValidator)
                {
                    if (this._sourceList == value)
                        return;
                    this._sourceList = !(this._sourceList is IVirtualList) ? value : throw new ArgumentException(InvariantString.Format("ListDataSet does not support IVirtualList.  Cannot associate with source list: {0}", value));
                    this.FirePropertyChanged(nameof(Source));
                    this.FirePropertyChanged("Count");
                    this.FireSetChanged(UIListContentsChangeType.Reset, -1, -1);
                }
            }
        }

        public virtual int Count
        {
            get
            {
                using (this.ThreadValidator)
                    return this._sourceList == null ? 0 : this._sourceList.Count;
            }
        }

        public virtual bool IsSynchronized
        {
            get
            {
                using (this.ThreadValidator)
                    return this._sourceList != null && this._sourceList.IsSynchronized;
            }
        }

        public virtual object SyncRoot
        {
            get
            {
                using (this.ThreadValidator)
                    return this._sourceList == null ? null : this._sourceList.SyncRoot;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                using (this.ThreadValidator)
                    return this._sourceList == null || this._sourceList.IsReadOnly;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                using (this.ThreadValidator)
                    return this._sourceList == null || this._sourceList.IsFixedSize;
            }
        }

        public object this[int itemIndex]
        {
            get
            {
                using (this.ThreadValidator)
                    return this._sourceList == null ? null : this._sourceList[itemIndex];
            }
            set
            {
                using (this.ThreadValidator)
                {
                    if (this._sourceList == null)
                        throw new InvalidOperationException("Cannot use the this indexer without first specifying a Source for this ListDataSet.");
                    if (itemIndex < 0 || itemIndex >= this.Count)
                        throw new ArgumentOutOfRangeException(nameof(itemIndex), itemIndex, "Given index is out of the range of this list.");
                    if (this._sourceList[itemIndex] == value)
                        return;
                    this._sourceList[itemIndex] = value;
                    this.FireSetChanged(UIListContentsChangeType.Modified, itemIndex, itemIndex);
                }
            }
        }

        public virtual void Clear()
        {
            using (this.ThreadValidator)
            {
                if (this._sourceList == null)
                    return;
                this._sourceList.Clear();
                this.FireSetChanged(UIListContentsChangeType.Clear, -1, -1);
                this.FirePropertyChanged("Count");
            }
        }

        public virtual void CopyTo(Array array, int index)
        {
            using (this.ThreadValidator)
            {
                if (this._sourceList == null)
                    throw new NotImplementedException("The \"CopyTo\" operation is not supported by this ListDataSet");
                this._sourceList.CopyTo(array, index);
            }
        }

        public void CopyFrom(IEnumerable source)
        {
            using (this.ThreadValidator)
            {
                if (source == null)
                    throw new ArgumentNullException(nameof(source));
                foreach (object obj in source)
                    this.Add(obj);
            }
        }

        public virtual IEnumerator GetEnumerator()
        {
            using (this.ThreadValidator)
                return this._sourceList != null ? this._sourceList.GetEnumerator() : throw new NotImplementedException("The \"GetEnumerator\" operation is not supported by this UIDataSet");
        }

        public bool Contains(object item)
        {
            using (this.ThreadValidator)
                return this._sourceList != null && this._sourceList.Contains(item);
        }

        public int IndexOf(object item)
        {
            using (this.ThreadValidator)
                return this._sourceList == null ? -1 : this._sourceList.IndexOf(item);
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
                if (this._sourceList == null)
                    throw new ArgumentException(InvariantString.Format("Empty list cannot remove item at index {0}", index));
                object obj = this[index];
                this._sourceList.RemoveAt(index);
                this.FireSetChanged(UIListContentsChangeType.Remove, index, -1);
                this.FirePropertyChanged("Count");
            }
        }

        public void Move(int oldIndex, int newIndex)
        {
            using (this.ThreadValidator)
            {
                if (this._sourceList == null)
                    throw new ArgumentException(InvariantString.Format("Empty list cannot move item from {0} to {1}", oldIndex, newIndex));
                object obj = this[oldIndex];
                if (this._sourceList is INotifyList sourceList)
                {
                    sourceList.Move(oldIndex, newIndex);
                }
                else
                {
                    this._sourceList.RemoveAt(oldIndex);
                    this._sourceList.Insert(newIndex, obj);
                }
                this.FireSetChanged(UIListContentsChangeType.Move, oldIndex, newIndex);
            }
        }

        public int Reorder(IList indices, int newIndex)
        {
            using (this.ThreadValidator)
            {
                if (indices == null)
                    throw new ArgumentNullException(nameof(indices));
                if (newIndex < 0 || newIndex > this.Count)
                    throw new ArgumentOutOfRangeException(nameof(newIndex), "newIndex must be greater than 0 and less than or equal to the size of the collection");
                int[] numArray = null;
                if (indices.IsReadOnly)
                    numArray = new int[indices.Count];
                int index1 = 0;
                foreach (object index2 in indices)
                {
                    if (!(index2 is int num))
                        throw new ArgumentException("indices[" + index1 + "] does not contain an int", nameof(indices));
                    if (num < 0 || num >= this.Count)
                        throw new ArgumentOutOfRangeException(nameof(indices), "indices[" + index1 + "] must be greater than 0 and less than the size of the collection");
                    if (numArray != null)
                        numArray[index1] = num;
                    ++index1;
                }
                if (numArray != null)
                    indices = numArray;
                int num1 = newIndex;
                for (int index2 = 0; index2 < indices.Count; ++index2)
                {
                    int index3 = (int)indices[index2];
                    if (index3 < newIndex)
                    {
                        --newIndex;
                        --num1;
                    }
                    if (index3 != newIndex)
                    {
                        this.Move(index3, newIndex);
                        for (int index4 = index2 + 1; index4 < indices.Count; ++index4)
                        {
                            int index5 = (int)indices[index4];
                            if (index3 < index5 && index5 <= newIndex)
                                indices[index4] = index5 - 1;
                            else if (newIndex <= index5 && index5 < index3)
                                indices[index4] = index5 + 1;
                            else if (index5 == index3)
                                indices[index4] = newIndex;
                        }
                    }
                    ++newIndex;
                }
                return num1;
            }
        }

        public void Insert(int index, object item)
        {
            using (this.ThreadValidator)
            {
                this._sourceList.Insert(index, item);
                this.FireSetChanged(UIListContentsChangeType.Insert, -1, index);
                this.FirePropertyChanged("Count");
            }
        }

        public int Add(object item)
        {
            using (this.ThreadValidator)
            {
                int newIndex = this._sourceList != null ? this._sourceList.Add(item) : throw new ArgumentException("Empty list cannot add items.");
                this.FireSetChanged(UIListContentsChangeType.Add, -1, newIndex);
                this.FirePropertyChanged("Count");
                return newIndex;
            }
        }

        public void Sort(IComparer comparer)
        {
            using (this.ThreadValidator)
            {
                if (comparer == null)
                    throw new ArgumentException("Must provide a valid IComparer");
                this.SortWorker(comparer);
            }
        }

        public void Sort()
        {
            using (this.ThreadValidator)
                this.SortWorker(null);
        }

        private void SortWorker(IComparer cmp)
        {
            for (int index1 = 0; index1 < this.Count; ++index1)
            {
                for (int index2 = index1 + 1; index2 < this.Count; ++index2)
                {
                    if (this.Compare(this[index1], this[index2], cmp) > 0)
                        this.Move(index2, index1);
                }
            }
        }

        private int Compare(object objA, object objB, IComparer cmp)
        {
            if (cmp != null)
                return cmp.Compare(objA, objB);
            return objA is IComparable comparable ? comparable.CompareTo(objB) : 0;
        }

        event UIListContentsChangedHandler INotifyList.ContentsChanged
        {
            add
            {
                using (this.ThreadValidator)
                    this.AddEventHandler(ListDataSet.s_listContentsChangedEvent, value);
            }
            remove
            {
                using (this.ThreadValidator)
                    this.RemoveEventHandler(ListDataSet.s_listContentsChangedEvent, value);
            }
        }

        public event ListContentsChangedHandler ContentsChanged
        {
            add
            {
                using (this.ThreadValidator)
                    this.AddEventHandler(ListDataSet.s_listContentsChangedEvent, ListContentsChangedProxy.Thunk(value));
            }
            remove
            {
                using (this.ThreadValidator)
                    this.RemoveEventHandler(ListDataSet.s_listContentsChangedEvent, ListContentsChangedProxy.Thunk(value));
            }
        }

        internal void FireSetChanged(UIListContentsChangeType type, int oldIndex, int newIndex)
        {
            UIListContentsChangedHandler eventHandler = (UIListContentsChangedHandler)this.GetEventHandler(ListDataSet.s_listContentsChangedEvent);
            if (eventHandler != null)
            {
                UIListContentsChangedArgs args = new UIListContentsChangedArgs(type, oldIndex, newIndex);
                eventHandler(this, args);
            }
            this.FirePropertyChanged("ContentsChanged");
        }
    }
}
