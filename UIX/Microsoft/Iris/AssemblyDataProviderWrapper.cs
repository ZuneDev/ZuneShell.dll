// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.AssemblyDataProviderWrapper
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup;

namespace Microsoft.Iris
{
    internal class AssemblyDataProviderWrapper : IDataProvider
    {
        private string _name;
        private DataProviderQueryFactory _factory;

        public AssemblyDataProviderWrapper(string name, DataProviderQueryFactory factory)
        {
            this._name = name;
            this._factory = factory;
        }

        public string Name => this._name;

        public MarkupDataQuery Build(MarkupDataQuerySchema querySchema) => (MarkupDataQuery)new AssemblyMarkupDataQuery(querySchema, this);

        public IDataProviderQuery ConstructQuery(object queryTypeCookie) => (IDataProviderQuery)this._factory(queryTypeCookie);

        public override string ToString() => string.Format("{0} ({1})", (object)this._name, (object)this._factory);
    }
}
