// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateTypeConstraint
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateTypeConstraint : ValidateObjectTag
    {
        private const string TypePropertyName = "Type";
        private const string ConstraintPropertyName = "Constraint";
        private TypeSchema _foundUseType;
        private TypeSchema _foundConstraintType;

        public ValidateTypeConstraint(
          SourceMarkupLoader owner,
          ValidateTypeIdentifier typeIdentifier,
          int line,
          int offset)
          : base(owner, typeIdentifier, line, offset)
        {
        }

        public override void NotifyParseComplete()
        {
            ValidateTypeIdentifier.PromoteSimplifiedTypeSyntax(this.FindProperty("Type"));
            ValidateTypeIdentifier.PromoteSimplifiedTypeSyntax(this.FindProperty("Constraint"));
        }

        protected override void ValidateProperties(ValidateContext context)
        {
            base.ValidateProperties(context);
            ValidateProperty property1 = this.FindProperty("Type");
            ValidateProperty property2 = this.FindProperty("Constraint");
            if (!property1.IsExpressionValue || ((ValidateExpression)property1.Value).ExpressionType != ExpressionType.TypeOf)
            {
                this.ReportError("Type parameter property '{0}' must be supplied a type constant (by name, or via 'typeof')", property1.PropertyName);
            }
            else
            {
                this._foundUseType = ((ValidateExpressionTypeOf)property1.Value).TypeIdentifier.FoundType;
                if (property2 != null)
                {
                    if (!property2.IsExpressionValue || ((ValidateExpression)property2.Value).ExpressionType != ExpressionType.TypeOf)
                    {
                        this.ReportError("Type parameter property '{0}' must be supplied a type constant (by name, or via 'typeof')", property2.PropertyName);
                        return;
                    }
                    this._foundConstraintType = ((ValidateExpressionTypeOf)property2.Value).TypeIdentifier.FoundType;
                }
                else
                    this._foundConstraintType = this._foundUseType;
                if (this._foundConstraintType.IsAssignableFrom(this._foundUseType))
                    return;
                this.ReportError("Type '{0}' does not match specified constraint '{1}'", this._foundUseType.Name, this._foundConstraintType.Name);
            }
        }

        public override PropertyOverrideCriteria PropertyOverrideCriteria => (PropertyOverrideCriteria)new PropertyOverrideCriteriaTypeConstraint(this._foundUseType, this._foundConstraintType);
    }
}
