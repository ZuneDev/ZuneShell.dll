// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpressionTypeOf
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateExpressionTypeOf : ValidateExpression
    {
        private ValidateTypeIdentifier _typeIdentifier;

        public ValidateExpressionTypeOf(
          SourceMarkupLoader owner,
          ValidateTypeIdentifier typeIdentifier,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.TypeOf)
        {
            this._typeIdentifier = typeIdentifier;
        }

        public ValidateTypeIdentifier TypeIdentifier => this._typeIdentifier;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            if (this.Usage == ExpressionUsage.LValue)
                this.ReportError("Expression cannot be used as the target an assignment (related symbol: '{0}')", "Operation");
            this._typeIdentifier.Validate();
            if (this._typeIdentifier.HasErrors)
                this.MarkHasErrors();
            this.DeclareEvaluationType(TypeSchemaDefinition.Type, typeRestriction);
        }
    }
}
