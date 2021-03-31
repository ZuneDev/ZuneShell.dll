// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.PropertyValue
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System;

namespace Microsoft.Iris
{
    public struct PropertyValue
    {
        private string _name;
        private object _value;

        public PropertyValue(string name, object value)
        {
            this._name = name != null ? name : throw new ArgumentNullException(nameof(name));
            this._value = value;
        }

        public string Name => this._name;

        public object Value => this._value;
    }
}
