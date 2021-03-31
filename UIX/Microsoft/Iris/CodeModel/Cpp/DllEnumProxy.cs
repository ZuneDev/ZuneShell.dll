// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.CodeModel.Cpp.DllEnumProxy
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.CodeModel.Cpp
{
    internal class DllEnumProxy
    {
        private int _value;
        private DllEnumSchema _schema;

        public DllEnumProxy(DllEnumSchema schema, int value)
        {
            this._value = value;
            this._schema = schema;
        }

        public int Value => this._value;

        public DllEnumSchema Type => this._schema;

        public override string ToString() => this._schema.InvokeToString(this);

        public override int GetHashCode() => this._value.GetHashCode();

        public override bool Equals(object other)
        {
            bool flag = false;
            if (other is DllEnumProxy dllEnumProxy)
                flag = this._schema == dllEnumProxy._schema && this._value == dllEnumProxy._value;
            return flag;
        }
    }
}
