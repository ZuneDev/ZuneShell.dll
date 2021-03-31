// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpressionAs
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateExpressionAs : ValidateExpression
    {
        private ValidateExpression _expression;
        private ValidateTypeIdentifier _typeIdentifier;

        public ValidateExpressionAs(
          SourceMarkupLoader owner,
          ValidateExpression expression,
          ValidateTypeIdentifier typeIdentifier,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.As)
        {
            this._expression = expression;
            this._typeIdentifier = typeIdentifier;
        }

        public ValidateExpression Expression => this._expression;

        public ValidateTypeIdentifier TypeIdentifier => this._typeIdentifier;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            if (this.Usage == ExpressionUsage.LValue)
                this.ReportError("Expression cannot be used as the target an assignment (related symbol: '{0}')", "As");
            this._expression.Validate(TypeRestriction.NotVoid, context);
            if (this._expression.HasErrors)
                this.MarkHasErrors();
            this._typeIdentifier.Validate();
            if (this._typeIdentifier.HasErrors)
                this.MarkHasErrors();
            if (this._typeIdentifier.FoundType != null && !this._typeIdentifier.FoundType.IsNullAssignable)
                this.ReportError("The {0} operator must be used with a reference type ('{1}' is not null assignable)", "as", this._typeIdentifier.FoundType.Name);
            if (this._typeIdentifier.FoundType == null)
                return;
            this.DeclareEvaluationType(this._typeIdentifier.FoundType, typeRestriction);
        }
    }
}
