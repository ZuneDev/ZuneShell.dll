// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.MarkupDataQuerySchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.Markup
{
    internal class MarkupDataQuerySchema : ClassTypeSchema
    {
        private TypeSchema _resultType;
        private string _providerName;
        private MarkupDataQueryPreDefinedPropertySchema _resultProperty;
        private MarkupDataQueryPreDefinedPropertySchema[] _predefinedProperties;
        private MarkupDataQueryRefreshMethodSchema _refreshMethod;

        public MarkupDataQuerySchema(MarkupLoadResult owner, string name)
          : base(owner, name)
        {
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            if (this._predefinedProperties != null)
            {
                foreach (DisposableObject predefinedProperty in this._predefinedProperties)
                    predefinedProperty.Dispose((object)this);
            }
            if (this._refreshMethod == null)
                return;
            this._refreshMethod.Dispose((object)this);
        }

        public override void BuildProperties()
        {
            base.BuildProperties();
            this._predefinedProperties = new MarkupDataQueryPreDefinedPropertySchema[3]
            {
        this._resultProperty = new MarkupDataQueryPreDefinedPropertySchema(this, this._resultType != null ? this._resultType : (TypeSchema) ObjectSchema.Type, "Result", new GetValueHandler(MarkupDataQuerySchema.GetResultProperty), (SetValueHandler) null),
        new MarkupDataQueryPreDefinedPropertySchema(this, UIXLoadResultExports.DataQueryStatusType, "Status", new GetValueHandler(MarkupDataQuerySchema.GetStatusProperty), (SetValueHandler) null),
        new MarkupDataQueryPreDefinedPropertySchema(this, (TypeSchema) BooleanSchema.Type, "Enabled", new GetValueHandler(MarkupDataQuerySchema.GetEnabledProperty), new SetValueHandler(MarkupDataQuerySchema.SetEnabledProperty))
            };
            this._refreshMethod = new MarkupDataQueryRefreshMethodSchema(this);
        }

        public override MarkupType MarkupType => MarkupType.DataQuery;

        protected override TypeSchema DefaultBase => (TypeSchema)MarkupDataQueryInstanceSchema.Type;

        public override Type RuntimeType => typeof(MarkupDataQuery);

        public override object ConstructDefault()
        {
            IDataProvider dataProvider = MarkupDataProvider.GetDataProvider(this._providerName);
            if (dataProvider != null)
                return (object)dataProvider.Build(this);
            ErrorManager.ReportError("Could not find provider '{0}'; verify that it has been registered", (object)this._providerName);
            return (object)null;
        }

        public override PropertySchema FindProperty(string name)
        {
            foreach (PropertySchema predefinedProperty in this._predefinedProperties)
            {
                if (name == predefinedProperty.Name)
                    return predefinedProperty;
            }
            return base.FindProperty(name);
        }

        public override MethodSchema FindMethod(string name, TypeSchema[] parameters) => this._refreshMethod.Name == name && parameters.Length == 0 ? (MethodSchema)this._refreshMethod : base.FindMethod(name, parameters);

        public string ProviderName
        {
            get => this._providerName;
            set => this._providerName = value;
        }

        public TypeSchema ResultType
        {
            get => this._resultType;
            set => this._resultType = value;
        }

        public bool InvalidatesQuery(string propertyName) => this.FindPropertyDeep(propertyName) is MarkupDataQueryPropertySchema propertyDeep && propertyDeep.InvalidatesQuery;

        private static object GetResultProperty(object instance) => ((MarkupDataQuery)instance).Result;

        private static object GetStatusProperty(object instance) => (object)((MarkupDataQuery)instance).Status;

        private static object GetEnabledProperty(object instance) => (object)((MarkupDataQuery)instance).Enabled;

        private static void SetEnabledProperty(ref object instance, object value) => ((MarkupDataQuery)instance).Enabled = (bool)value;
    }
}
