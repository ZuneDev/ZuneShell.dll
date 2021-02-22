// Decompiled with JetBrains decompiler
// Type: ZuneUI.DataProviderTitleList
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class DataProviderTitleList : IList, ICollection, IEnumerable
    {
        private List<DataProviderObject> _dataProviders = new List<DataProviderObject>();

        public IList DataProviders => (IList)this._dataProviders;

        public void InitializeDataProviders(IList list)
        {
            this._dataProviders = new List<DataProviderObject>();
            foreach (object obj in (IEnumerable)list)
                this._dataProviders.Add(obj as DataProviderObject);
        }

        public int Add(object value) => throw new NotImplementedException();

        public void Clear() => this._dataProviders.Clear();

        public bool Contains(object value) => this.IndexOf(value) >= 0;

        public int IndexOf(object value)
        {
            string b = value as string;
            for (int index = 0; index < this._dataProviders.Count; ++index)
            {
                if (string.Equals((string)this._dataProviders[index].GetProperty("Title"), b, StringComparison.CurrentCultureIgnoreCase))
                    return index;
            }
            return -1;
        }

        public void Insert(int index, object value) => throw new NotImplementedException();

        public bool IsFixedSize => ((IList)this._dataProviders).IsFixedSize;

        public bool IsReadOnly => ((IList)this._dataProviders).IsReadOnly;

        public void Remove(object value)
        {
            int index = this.IndexOf((object)(value as string));
            if (index < 0)
                return;
            this._dataProviders.RemoveAt(index);
        }

        public void RemoveAt(int index) => this._dataProviders.RemoveAt(index);

        public object this[int index]
        {
            get => this._dataProviders[index].GetProperty("Title");
            set => throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index) => throw new NotImplementedException();

        public int Count => this._dataProviders.Count;

        public bool IsSynchronized => ((ICollection)this._dataProviders).IsSynchronized;

        public object SyncRoot => ((ICollection)this._dataProviders).SyncRoot;

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < this._dataProviders.Count; ++i)
                yield return this._dataProviders[i].GetProperty("Title");
        }
    }
}
