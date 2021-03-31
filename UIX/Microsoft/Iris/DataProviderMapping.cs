// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.DataProviderMapping
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup;
using Microsoft.Iris.Markup.UIX;
using System;

namespace Microsoft.Iris
{
    public class DataProviderMapping
    {
        private string _source;
        private string _target;
        private object _defaultValue;
        private PropertySchema _propertySchema;
        private Type _assemblyPropertyType;
        private Type _assemblyAlternateType;

        public object PropertyTypeCookie => _propertySchema.PropertyType;

        public object UnderlyingCollectionTypeCookie => _propertySchema.AlternateType;

        public string PropertyName => this._propertySchema.Name;

        public string PropertyTypeName => DataProviderMapping.GetCanonicalTypeName(this._propertySchema.PropertyType);

        public Type PropertyType => this._assemblyPropertyType;

        public string UnderlyingCollectionTypeName => DataProviderMapping.GetCanonicalTypeName(this._propertySchema.AlternateType);

        public Type UnderlyingCollectionType => this._assemblyAlternateType;

        public string Source => this._source;

        public string Target => this._target;

        public object DefaultValue => this._defaultValue;

        internal DataProviderMapping(PropertySchema propertySchema, object defaultValue)
          : this(propertySchema, null, null, defaultValue)
        {
        }

        internal DataProviderMapping(
          PropertySchema propertySchema,
          string source,
          string target,
          object defaultValue)
        {
            this._propertySchema = propertySchema;
            this._source = source;
            this._target = target;
            this._defaultValue = defaultValue;
            this._assemblyPropertyType = AssemblyLoadResult.MapType(this._propertySchema.PropertyType);
            if (this._propertySchema.AlternateType == null)
                return;
            this._assemblyAlternateType = AssemblyLoadResult.MapType(this._propertySchema.AlternateType);
        }

        internal static string GetCanonicalTypeName(TypeSchema typeSchema)
        {
            if (typeSchema == null)
                return null;
            return typeSchema == ListSchema.Type ? "List" : typeSchema.Name;
        }
    }
}
