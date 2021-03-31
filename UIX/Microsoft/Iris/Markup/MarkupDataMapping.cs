// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.MarkupDataMapping
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup
{
    internal class MarkupDataMapping
    {
        private string _name;
        private MarkupDataMappingEntry[] _mappings;
        private MarkupDataTypeSchema _targetType;
        private string _provider;
        private object _assemblyDataProviderCookie;

        public MarkupDataMapping(string name) => this._name = name;

        public MarkupDataTypeSchema TargetType
        {
            get => this._targetType;
            set => this._targetType = value;
        }

        public string Provider
        {
            get => this._provider;
            set => this._provider = value;
        }

        public MarkupDataMappingEntry[] Mappings
        {
            get => this._mappings;
            set => this._mappings = value;
        }

        public object AssemblyDataProviderCookie
        {
            get => this._assemblyDataProviderCookie;
            set => this._assemblyDataProviderCookie = value;
        }

        public override string ToString() => string.Format("({0}, {1})", _provider, _targetType);
    }
}
