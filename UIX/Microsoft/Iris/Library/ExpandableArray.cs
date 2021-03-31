// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Library.ExpandableArray
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System;
using System.Collections;
using System.Linq;

namespace Microsoft.Iris.Library
{
    internal class ExpandableArray
    {
        private object[] _array;
        private int _generation;
        private int _lockCount;

        public ExpandableArray(int initialSize) => this._array = new object[initialSize];

        public int Length => this._array.Length;

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        public object this[int index]
        {
            get => this._array[index];
            set => this.SetItem(index, value);
        }

        public int Add(object value)
        {
            int index = 0;
            while (index < this._array.Length && this._array[index] != null)
                ++index;
            this.SetItem(index, value);
            return index;
        }

        public void Clear()
        {
            this.IncrementGeneration();
            Array.Clear(_array, 0, _array.Length);
        }

        public bool Contains(object value) => this._array.Contains(value);

        public int IndexOf(object value) => Array.IndexOf(_array, value);

        public void Insert(int index, object value)
        {
        }

        public void Remove(object value)
        {
        }

        public void RemoveAt(int index)
        {
        }

        public int Count
        {
            get
            {
                int num = 0;
                for (int index = 0; index < this._array.Length; ++index)
                {
                    if (this._array[index] != null)
                        ++num;
                }
                return num;
            }
        }

        public bool IsSynchronized => false;

        public object SyncRoot => (object)null;

        public void CopyTo(Array array, int index) => this._array.CopyTo(array, index);

        public ExpandableArray.NullSkippingEnumerator GetEnumerator() => new ExpandableArray.NullSkippingEnumerator(this);

        public void LockArray() => ++this._lockCount;

        public void UnlockArray() => --this._lockCount;

        private void IncrementGeneration()
        {
            ++this._generation;
            if (this._generation != int.MaxValue)
                return;
            this._generation = 0;
        }

        private void ExpandArray(int index)
        {
            int length = this._array.Length;
            while (index >= length)
                length <<= 1;
            object[] objArray = new object[length];
            for (int index1 = 0; index1 < this._array.Length; ++index1)
                objArray[index1] = this._array[index1];
            this._array = objArray;
        }

        private void SetItem(int index, object value)
        {
            this.IncrementGeneration();
            if (index >= this._array.Length)
                this.ExpandArray(index);
            this._array[index] = value;
        }

        public struct NullSkippingEnumerator : IEnumerator
        {
            private ExpandableArray _array;
            private int _position;
            private int _generation;

            public NullSkippingEnumerator(ExpandableArray array)
            {
                this._array = array;
                this._position = -1;
                this._generation = array._generation;
            }

            public object Current
            {
                get
                {
                    this.CheckGeneration();
                    return this._array[this._position];
                }
            }

            public bool MoveNext()
            {
                this.CheckGeneration();
                for (++this._position; this._position < this._array.Length; ++this._position)
                {
                    if (this._array[this._position] != null)
                        return true;
                }
                return false;
            }

            public void Reset()
            {
                this.CheckGeneration();
                this._position = -1;
            }

            private void CheckGeneration()
            {
            }
        }
    }
}
