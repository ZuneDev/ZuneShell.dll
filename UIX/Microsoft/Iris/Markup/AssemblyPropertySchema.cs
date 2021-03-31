// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.AssemblyPropertySchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Reflection;

namespace Microsoft.Iris.Markup
{
    internal class AssemblyPropertySchema : PropertySchema
    {
        private PropertyInfo _propertyInfo;
        private TypeSchema _propertyTypeSchema;
        private bool _isStatic;
        private FastReflectionInvokeHandler _getMethod;
        private FastReflectionInvokeHandler _setMethod;
        private object[] _setMethodParams;

        public AssemblyPropertySchema(AssemblyTypeSchema owner, PropertyInfo propertyInfo)
          : base((TypeSchema)owner)
        {
            this._propertyInfo = propertyInfo;
            this._propertyTypeSchema = (TypeSchema)AssemblyLoadResult.MapType(this._propertyInfo.PropertyType);
            this._isStatic = (this._propertyInfo.GetGetMethod() ?? this._propertyInfo.GetSetMethod()).IsStatic;
        }

        public override string Name => this._propertyInfo.Name;

        public override TypeSchema PropertyType => this._propertyTypeSchema;

        public override TypeSchema AlternateType => (TypeSchema)null;

        public override bool CanRead => this._propertyInfo.CanRead;

        public override bool CanWrite => this._propertyInfo.CanWrite;

        public override bool IsStatic => this._isStatic;

        public override ExpressionRestriction ExpressionRestriction => ExpressionRestriction.None;

        public override bool RequiredForCreation => false;

        public override RangeValidator RangeValidator => (RangeValidator)null;

        public override bool NotifiesOnChange
        {
            get
            {
                AssemblyTypeSchema owner = (AssemblyTypeSchema)this.Owner;
                return !this._isStatic && owner.NotifiesOnChange;
            }
        }

        public override object GetValue(object instance)
        {
            object target = AssemblyLoadResult.UnwrapObject(instance);
            if (this._getMethod == null)
                this._getMethod = ReflectionHelper.CreateMethodInvoke((MethodBase)this._propertyInfo.GetGetMethod());
            return AssemblyLoadResult.WrapObject(this._propertyTypeSchema, this._getMethod(target, (object[])null));
        }

        public override void SetValue(ref object instance, object value)
        {
            object target = AssemblyLoadResult.UnwrapObject(instance);
            object obj1 = AssemblyLoadResult.UnwrapObject(value);
            if (this._setMethod == null)
            {
                this._setMethod = ReflectionHelper.CreateMethodInvoke((MethodBase)this._propertyInfo.GetSetMethod());
                this._setMethodParams = new object[1];
            }
            this._setMethodParams[0] = obj1;
            object obj2 = this._setMethod(target, this._setMethodParams);
            this._setMethodParams[0] = (object)null;
        }
    }
}
