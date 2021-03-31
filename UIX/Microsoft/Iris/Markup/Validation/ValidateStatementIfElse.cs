// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateStatementIfElse
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateStatementIfElse : ValidateStatement
    {
        private ValidateExpression _condition;
        private ValidateStatementCompound _statementCompoundTrue;
        private ValidateStatementCompound _statementCompoundFalse;

        public ValidateStatementIfElse(
          SourceMarkupLoader owner,
          ValidateExpression condition,
          ValidateStatementCompound statementCompoundTrue,
          ValidateStatementCompound statementCompoundFalse,
          int line,
          int column)
          : base(owner, line, column, StatementType.IfElse)
        {
            this._condition = condition;
            this._statementCompoundTrue = statementCompoundTrue;
            this._statementCompoundFalse = statementCompoundFalse;
        }

        public ValidateExpression Condition => this._condition;

        public ValidateStatement StatementCompoundTrue => _statementCompoundTrue;

        public ValidateStatement StatementCompoundFalse => _statementCompoundFalse;

        public override void Validate(ValidateCode container, ValidateContext context)
        {
            this._condition.Validate(new TypeRestriction(BooleanSchema.Type), context);
            if (this._condition.HasErrors)
                this.MarkHasErrors();
            this._statementCompoundTrue.Validate(container, context);
            if (this._statementCompoundTrue.HasErrors)
                this.MarkHasErrors();
            this._statementCompoundFalse.Validate(container, context);
            if (!this._statementCompoundFalse.HasErrors)
                return;
            this.MarkHasErrors();
        }
    }
}
