// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpressionNullCoalescing
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateExpressionNullCoalescing : ValidateExpression
    {
        private ValidateExpression _condition;
        private ValidateExpression _nullClause;

        public ValidateExpressionNullCoalescing(
          SourceMarkupLoader owner,
          ValidateExpression condition,
          ValidateExpression nullClause,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.NullCoalescing)
        {
            this._condition = condition;
            this._nullClause = nullClause;
        }

        public ValidateExpression Condition => this._condition;

        public ValidateExpression NullClause => this._nullClause;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            if (this.Usage == ExpressionUsage.LValue)
                this.ReportError("Expression cannot be used as the target an assignment (related symbol: '{0}')", "Null coalescing");
            this._condition.Validate(typeRestriction, context);
            if (this._condition.HasErrors)
                this.MarkHasErrors();
            if (this._condition.ObjectType != null)
                this.DeclareEvaluationType(this._condition.ObjectType, typeRestriction);
            this._nullClause.Validate(typeRestriction, context);
            if (this._condition.HasErrors)
                this.MarkHasErrors();
            if (this._condition.ObjectType == null || this._nullClause.ObjectType == null)
                return;
            if (this._condition.ObjectType == NullSchema.Type)
            {
                this.ReportError("Null constant may not be used as the condition for ?? operator");
            }
            else
            {
                if (!this._condition.ObjectType.IsNullAssignable)
                    this.ReportError("The {0} operator must be used with a reference type ('{1}' is not null assignable)", "??", this._condition.ObjectType.Name);
                if (this._condition.ObjectType.IsAssignableFrom(this._nullClause.ObjectType))
                    return;
                this.ReportError("Both expressions for the {0} operator must match: '{1}' and '{2}' are not compatible", "??", this._condition.ObjectType.Name, this._nullClause.ObjectType.Name);
            }
        }
    }
}
