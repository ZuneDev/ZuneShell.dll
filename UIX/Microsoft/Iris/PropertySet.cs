// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.PropertySet
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Iris
{
    public class PropertySet : ModelItem, IDictionary, ICollection, IEnumerable
    {
        private Dictionary<object, object> _valuesTable = new Dictionary<object, object>();

        public PropertySet(IModelItemOwner owner, string description)
          : base(owner, description)
        {
        }

        public PropertySet(IModelItemOwner owner)
          : this(owner, (string)null)
        {
        }

        public PropertySet()
          : this((IModelItemOwner)null)
        {
        }

        public IDictionary Entries
        {
            get
            {
                using (this.ThreadValidator)
                    return (IDictionary)this;
            }
        }

        public object this[object key]
        {
            get
            {
                using (this.ThreadValidator)
                {
                    object obj;
                    return this._valuesTable.TryGetValue(key, out obj) ? obj : (object)null;
                }
            }
            set
            {
                using (this.ThreadValidator)
                {
                    object a;
                    if (this._valuesTable.TryGetValue(key, out a) && PropertySet.IsEqual(a, value))
                        return;
                    this._valuesTable[key] = value;
                    this.NotifyEntryChange(key);
                }
            }
        }

        bool IDictionary.Contains(object key)
        {
            using (this.ThreadValidator)
                return this._valuesTable.ContainsKey(key);
        }

        void IDictionary.Add(object key, object value)
        {
            using (this.ThreadValidator)
            {
                if (this._valuesTable.ContainsKey(key))
                    return;
                this._valuesTable.Add(key, value);
                this.NotifyEntryChange(key);
            }
        }

        void IDictionary.Remove(object key)
        {
            using (this.ThreadValidator)
            {
                if (this._valuesTable.ContainsKey(key))
                    return;
                this._valuesTable.Remove(key);
                this.NotifyEntryChange(key);
            }
        }

        void IDictionary.Clear()
        {
            using (this.ThreadValidator)
            {
                foreach (KeyValuePair<object, object> keyValuePair in this._valuesTable)
                    this.NotifyEntryChange(keyValuePair.Key);
                this._valuesTable.Clear();
            }
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            using (this.ThreadValidator)
                return (IDictionaryEnumerator)this._valuesTable.GetEnumerator();
        }

        bool IDictionary.IsFixedSize => false;

        bool IDictionary.IsReadOnly => false;

        ICollection IDictionary.Keys
        {
            get
            {
                using (this.ThreadValidator)
                    return (ICollection)this._valuesTable.Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                using (this.ThreadValidator)
                    return (ICollection)this._valuesTable.Values;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            using (this.ThreadValidator)
                ((ICollection)this._valuesTable).CopyTo(array, index);
        }

        int ICollection.Count
        {
            get
            {
                using (this.ThreadValidator)
                    return this._valuesTable.Count;
            }
        }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot
        {
            get
            {
                using (this.ThreadValidator)
                    return (object)this._valuesTable;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            using (this.ThreadValidator)
                return (IEnumerator)this._valuesTable.GetEnumerator();
        }

        private void NotifyEntryChange(object key) => this.FirePropertyChanged("#" + key.ToString());

        private static bool IsEqual(object a, object b) => a == null ? b == null : a.Equals(b);
    }
}
