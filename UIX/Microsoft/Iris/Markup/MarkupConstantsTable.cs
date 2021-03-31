// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.MarkupConstantsTable
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Iris.Markup
{
    internal class MarkupConstantsTable
    {
        private object[] _runtimeList;
        private MarkupConstantPersist[] _persistList;
        private Dictionary<MarkupConstantsTable.MarkupConstant, int> _lookupTable;
        private ByteCodeReader _constantsTableReader;
        private MarkupLoadResult _loadResultOwner;

        public MarkupConstantsTable() => this._lookupTable = new Dictionary<MarkupConstantsTable.MarkupConstant, int>(new MarkupConstantsTable.MarkupConstantEqualityComparer());

        public MarkupConstantsTable(object[] runtimeList) => this._runtimeList = runtimeList;

        public void SetConstantsTableReader(
          ByteCodeReader constantsTableReader,
          MarkupLoadResult loadResultOwner)
        {
            this._constantsTableReader = constantsTableReader;
            this._loadResultOwner = loadResultOwner;
        }

        public int Add(TypeSchema type, object value, MarkupConstantPersistMode persistMode) => this.Add(type, value, persistMode, value);

        public int Add(
          TypeSchema type,
          object value,
          MarkupConstantPersistMode persistMode,
          object persistData)
        {
            MarkupConstantsTable.MarkupConstant key;
            key.Type = type;
            key.Value = value;
            key.HashCode = value.GetHashCode();
            if (MarkupSystem.CompileMode)
            {
                key.Persist.Mode = persistMode;
                key.Persist.Data = persistData;
                key.Persist.Type = type;
            }
            else
            {
                key.Persist.Mode = MarkupConstantPersistMode.Binary;
                key.Persist.Data = null;
                key.Persist.Type = null;
            }
            int count;
            if (!this._lookupTable.TryGetValue(key, out count))
            {
                count = this._lookupTable.Count;
                this._lookupTable.Add(key, count);
            }
            return count;
        }

        public object Get(int index)
        {
            object obj = this._runtimeList[index];
            if (obj == null)
            {
                this._constantsTableReader.CurrentOffset = (uint)(index * 4);
                this._constantsTableReader.CurrentOffset = this._constantsTableReader.ReadUInt32();
                obj = CompiledMarkupLoader.DepersistConstant(this._constantsTableReader, this._loadResultOwner);
                this._runtimeList[index] = obj;
            }
            return obj;
        }

        public void PrepareForRuntimeUse()
        {
            this._runtimeList = new object[this._lookupTable.Count];
            if (MarkupSystem.CompileMode)
                this._persistList = new MarkupConstantPersist[this._lookupTable.Count];
            foreach (KeyValuePair<MarkupConstantsTable.MarkupConstant, int> keyValuePair in this._lookupTable)
            {
                this._runtimeList[keyValuePair.Value] = keyValuePair.Key.Value;
                if (MarkupSystem.CompileMode)
                    this._persistList[keyValuePair.Value] = keyValuePair.Key.Persist;
            }
            this._lookupTable = null;
        }

        public MarkupConstantPersist[] PersistList => this._persistList;

        [Conditional("DEBUG")]
        public void DEBUG_DumpTable()
        {
        }

        private class MarkupConstantEqualityComparer :
          IEqualityComparer<MarkupConstantsTable.MarkupConstant>
        {
            bool IEqualityComparer<MarkupConstantsTable.MarkupConstant>.Equals(
              MarkupConstantsTable.MarkupConstant x,
              MarkupConstantsTable.MarkupConstant y)
            {
                bool flag = false;
                if (x.Type == y.Type)
                    flag = (bool)x.Type.PerformOperationDeep(x.Value, y.Value, OperationType.RelationalEquals);
                return flag;
            }

            int IEqualityComparer<MarkupConstantsTable.MarkupConstant>.GetHashCode(
              MarkupConstantsTable.MarkupConstant obj)
            {
                return obj.HashCode;
            }
        }

        private struct MarkupConstant
        {
            public TypeSchema Type;
            public object Value;
            public int HashCode;
            public MarkupConstantPersist Persist;
        }
    }
}
