// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Library.SmartMap
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System;

namespace Microsoft.Iris.Library
{
    internal struct SmartMap
    {
        private SmartMap.Entry[] _listEntries;

        public int Count
        {
            get
            {
                int num = 0;
                if (this._listEntries != null)
                    num = this._listEntries.Length;
                return num;
            }
        }

        public bool IsEmpty => this.Count == 0;

        public object this[uint key]
        {
            get
            {
                int index = this.IndexOf(key);
                return index < 0 ? (object)null : this._listEntries[index].dataObject;
            }
            set
            {
                int slotIndex = this.IndexOf(key);
                if (value != null)
                {
                    if (slotIndex >= 0)
                        this._listEntries[slotIndex].dataObject = value;
                    else
                        this.Add(key, value, ~slotIndex);
                }
                else
                {
                    if (slotIndex < 0)
                        return;
                    this.Remove(key, slotIndex);
                }
            }
        }

        public uint[] Keys
        {
            get
            {
                uint[] numArray = (uint[])null;
                if (this._listEntries != null)
                {
                    numArray = new uint[this._listEntries.Length];
                    for (int index = 0; index < this._listEntries.Length; ++index)
                        numArray[index] = this._listEntries[index].key;
                }
                return numArray;
            }
        }

        public object[] Values
        {
            get
            {
                object[] objArray = (object[])null;
                if (this._listEntries != null)
                {
                    objArray = new object[this._listEntries.Length];
                    for (int index = 0; index < this._listEntries.Length; ++index)
                        objArray[index] = this._listEntries[index].dataObject;
                }
                return objArray;
            }
        }

        public bool Lookup(object desired, out uint key)
        {
            if (this._listEntries != null)
            {
                foreach (SmartMap.Entry listEntry in this._listEntries)
                {
                    if (listEntry.dataObject == desired)
                    {
                        key = listEntry.key;
                        return true;
                    }
                }
            }
            key = 0U;
            return false;
        }

        private int IndexOf(uint key)
        {
            if (this._listEntries == null)
                return -1;
            uint num1 = uint.MaxValue;
            int index1 = 0;
            if (this._listEntries.Length > 4)
            {
                int num2 = 0;
                int num3 = this._listEntries.Length - 1;
                while (num2 <= num3)
                {
                    index1 = (num3 + num2) / 2;
                    num1 = this._listEntries[index1].key;
                    if ((int)key == (int)num1)
                        return index1;
                    if (key < num1)
                        num3 = index1 - 1;
                    else
                        num2 = index1 + 1;
                }
            }
            else
            {
                for (int index2 = 0; index2 < this._listEntries.Length; ++index2)
                {
                    index1 = index2;
                    num1 = this._listEntries[index1].key;
                    if ((int)key == (int)num1)
                        return index1;
                    if (key < num1)
                        break;
                }
            }
            if (key > num1)
                ++index1;
            return ~index1;
        }

        private void Add(uint key, object dataObject, int slotIndex)
        {
            SmartMap.Entry[] entryArray;
            if (this._listEntries == null)
            {
                entryArray = new SmartMap.Entry[1];
            }
            else
            {
                int length = this._listEntries.Length;
                entryArray = new SmartMap.Entry[length + 1];
                Array.Copy((Array)this._listEntries, (Array)entryArray, slotIndex);
                Array.Copy((Array)this._listEntries, slotIndex, (Array)entryArray, slotIndex + 1, length - slotIndex);
            }
            this._listEntries = entryArray;
            this._listEntries[slotIndex].key = key;
            this._listEntries[slotIndex].dataObject = dataObject;
        }

        private void Remove(uint key, int slotIndex)
        {
            int length = this._listEntries.Length;
            if (length > 0)
            {
                SmartMap.Entry[] entryArray = new SmartMap.Entry[length - 1];
                Array.Copy((Array)this._listEntries, (Array)entryArray, slotIndex);
                Array.Copy((Array)this._listEntries, slotIndex + 1, (Array)entryArray, slotIndex, length - (slotIndex + 1));
                this._listEntries = entryArray;
            }
            else
                this._listEntries = (SmartMap.Entry[])null;
        }

        private struct Entry
        {
            internal uint key;
            internal object dataObject;
        }
    }
}
