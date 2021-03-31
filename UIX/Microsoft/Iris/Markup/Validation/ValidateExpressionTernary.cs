// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpressionTernary
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateExpressionTernary : ValidateExpression
    {
        private ValidateExpression _condition;
        private ValidateExpression _trueClause;
        private ValidateExpression _falseClause;

        public ValidateExpressionTernary(
          SourceMarkupLoader owner,
          ValidateExpression condition,
          ValidateExpression trueClause,
          ValidateExpression falseClause,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.Ternary)
        {
            this._condition = condition;
            this._trueClause = trueClause;
            this._falseClause = falseClause;
        }

        public ValidateExpression Condition => this._condition;

        public ValidateExpression TrueClause => this._trueClause;

        public ValidateExpression FalseClause => this._falseClause;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            if (this.Usage == ExpressionUsage.LValue)
                this.ReportError("Expression cannot be used as the target an assignment (related symbol: '{0}')", "Ternary");
            this._condition.Validate(new TypeRestriction(BooleanSchema.Type), context);
            if (this._condition.HasErrors)
                this.MarkHasErrors();
            this._trueClause.Validate(typeRestriction, context);
            if (this._trueClause.HasErrors)
                this.MarkHasErrors();
            this._falseClause.Validate(typeRestriction, context);
            if (this._falseClause.HasErrors)
                this.MarkHasErrors();
            if (this._trueClause.ObjectType == null || this._falseClause.ObjectType == null)
                return;
            if (this._trueClause.ObjectType.IsAssignableFrom(this._falseClause.ObjectType))
                this.DeclareEvaluationType(this._trueClause.ObjectType, typeRestriction);
            else if (this._falseClause.ObjectType.IsAssignableFrom(this._trueClause.ObjectType))
                this.DeclareEvaluationType(this._falseClause.ObjectType, typeRestriction);
            else
                this.ReportError("Both expressions for the {0} operator must match: '{1}' and '{2}' are not compatible", "?:", this._trueClause.ObjectType.Name, this._falseClause.ObjectType.Name);
        }
    }
}
