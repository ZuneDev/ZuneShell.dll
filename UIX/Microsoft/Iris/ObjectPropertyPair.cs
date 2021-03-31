// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ObjectPropertyPair
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris
{
    public class ObjectPropertyPair
    {
        private object _obj;
        private string _property;

        public ObjectPropertyPair(object obj, string property)
        {
            this._obj = obj;
            this._property = property;
        }

        public object Object => this._obj;

        public string PropertyName => this._property;
    }
}
