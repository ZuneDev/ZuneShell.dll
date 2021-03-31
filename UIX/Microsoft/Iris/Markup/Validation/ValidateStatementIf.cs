// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateStatementIf
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateStatementIf : ValidateStatement
    {
        private ValidateExpression _condition;
        private ValidateStatementCompound _statementCompound;

        public ValidateStatementIf(
          SourceMarkupLoader owner,
          ValidateExpression condition,
          ValidateStatementCompound statementCompound,
          int line,
          int column)
          : base(owner, line, column, StatementType.If)
        {
            this._condition = condition;
            this._statementCompound = statementCompound;
        }

        public ValidateExpression Condition => this._condition;

        public ValidateStatementCompound StatementCompound => this._statementCompound;

        public override void Validate(ValidateCode container, ValidateContext context)
        {
            this._condition.Validate(new TypeRestriction(BooleanSchema.Type), context);
            if (this._condition.HasErrors)
                this.MarkHasErrors();
            this._statementCompound.Validate(container, context);
            if (!this._statementCompound.HasErrors)
                return;
            this.MarkHasErrors();
        }
    }
}
