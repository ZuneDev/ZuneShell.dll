// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIXPropertySchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup
{
    internal class UIXPropertySchema : PropertySchema
    {
        private string _name;
        private TypeSchema _propertyType;
        private TypeSchema _alternateType;
        private bool _isStatic;
        private ExpressionRestriction _expressionRestriction;
        private bool _requiredForCreation;
        private RangeValidator _rangeValidator;
        private bool _notifiesOnChange;
        private GetValueHandler _getValue;
        private SetValueHandler _setValue;

        public UIXPropertySchema(
          short ownerTypeID,
          string name,
          short typeID,
          short alternateTypeID,
          ExpressionRestriction expressionRestriction,
          bool requiredForCreation,
          RangeValidator rangeValidator,
          bool notifiesOnChange,
          GetValueHandler getValue,
          SetValueHandler setValue,
          bool isStatic)
          : base(UIXTypes.MapIDToType(ownerTypeID))
        {
            this._name = name;
            this._propertyType = UIXTypes.MapIDToType(typeID);
            this._alternateType = UIXTypes.MapIDToType(alternateTypeID);
            this._expressionRestriction = expressionRestriction;
            this._requiredForCreation = requiredForCreation;
            this._rangeValidator = rangeValidator;
            this._notifiesOnChange = notifiesOnChange;
            this._getValue = getValue;
            this._setValue = setValue;
            this._isStatic = isStatic;
        }

        public override string Name => this._name;

        public override TypeSchema PropertyType => this._propertyType;

        public override TypeSchema AlternateType => this._alternateType;

        public override bool CanRead => this._getValue != null;

        public override bool CanWrite => this._setValue != null;

        public override bool IsStatic => this._isStatic;

        public override ExpressionRestriction ExpressionRestriction => this._expressionRestriction;

        public override bool RequiredForCreation => this._requiredForCreation;

        public override RangeValidator RangeValidator => this._rangeValidator;

        public override bool NotifiesOnChange => this._notifiesOnChange;

        public override object GetValue(object instance) => this._getValue(instance);

        public override void SetValue(ref object instance, object value) => this._setValue(ref instance, value);
    }
}
