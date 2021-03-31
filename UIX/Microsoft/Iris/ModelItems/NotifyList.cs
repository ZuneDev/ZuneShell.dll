// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ModelItems.NotifyList
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using System;
using System.Collections;

namespace Microsoft.Iris.ModelItems
{
    internal class NotifyList : NotifyObjectBase, INotifyList, IList, ICollection, IEnumerable
    {
        private IList _source;

        public NotifyList()
          : this(new ArrayList())
        {
        }

        public NotifyList(IList source) => this._source = source;

        public int Add(object value)
        {
            int newIndex = this._source.Add(value);
            this.FireSetChanged(UIListContentsChangeType.Add, -1, newIndex);
            this.FireNotification(NotificationID.Count);
            return newIndex;
        }

        public void Clear()
        {
            this._source.Clear();
            this.FireSetChanged(UIListContentsChangeType.Clear, -1, -1);
            this.FireNotification(NotificationID.Count);
        }

        public bool Contains(object value) => this._source.Contains(value);

        public int IndexOf(object value) => this._source.IndexOf(value);

        public void Insert(int index, object value)
        {
            this._source.Insert(index, value);
            this.FireSetChanged(UIListContentsChangeType.Insert, -1, index);
            this.FireNotification(NotificationID.Count);
        }

        public void Remove(object value)
        {
            int index = this.IndexOf(value);
            if (index == -1)
                return;
            this.RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            object obj = this[index];
            this._source.RemoveAt(index);
            this.FireSetChanged(UIListContentsChangeType.Remove, index, -1);
            this.FireNotification(NotificationID.Count);
        }

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        public object this[int index]
        {
            get => this._source[index];
            set
            {
                if (this._source[index] == value)
                    return;
                this._source[index] = value;
                this.FireSetChanged(UIListContentsChangeType.Modified, index, index);
            }
        }

        public void CopyTo(Array array, int index) => this._source.CopyTo(array, index);

        public int Count => this._source.Count;

        public bool IsSynchronized => false;

        public object SyncRoot => this._source.SyncRoot;

        public IEnumerator GetEnumerator() => this._source.GetEnumerator();

        public void Move(int oldIndex, int newIndex)
        {
            object obj = this[oldIndex];
            this._source.RemoveAt(oldIndex);
            this._source.Insert(newIndex, obj);
            this.FireSetChanged(UIListContentsChangeType.Move, oldIndex, newIndex);
        }

        public event UIListContentsChangedHandler ContentsChanged;

        private void FireSetChanged(UIListContentsChangeType type, int oldIndex, int newIndex)
        {
            if (this.ContentsChanged == null)
                return;
            this.ContentsChanged(this, new UIListContentsChangedArgs(type, oldIndex, newIndex));
        }
    }
}
