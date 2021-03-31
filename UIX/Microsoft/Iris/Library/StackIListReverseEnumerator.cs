// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Library.StackIListReverseEnumerator
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Collections;

namespace Microsoft.Iris.Library
{
    internal struct StackIListReverseEnumerator : IEnumerator
    {
        private IList _list;
        private int _currentIndex;

        public StackIListReverseEnumerator(IList list)
        {
            this._list = list;
            this._currentIndex = list.Count;
        }

        public StackIListReverseEnumerator GetEnumerator() => this;

        public bool MoveNext() => --this._currentIndex >= 0;

        public object Current => this._list[this._currentIndex];

        public void Reset() => this._currentIndex = this._list.Count;
    }
}
