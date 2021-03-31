// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateStatementAssignment
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateStatementAssignment : ValidateStatement
    {
        private ValidateStatementScopedLocal _declaredScopedLocal;
        private ValidateExpression _lvalue;
        private ValidateExpression _rvalue;

        public ValidateStatementAssignment(
          SourceMarkupLoader owner,
          ValidateStatementScopedLocal declaredScopedLocal,
          ValidateExpression lvalue,
          ValidateExpression rvalue,
          int line,
          int column)
          : base(owner, line, column, StatementType.Assignment)
        {
            this._declaredScopedLocal = declaredScopedLocal;
            this._lvalue = lvalue;
            this._rvalue = rvalue;
        }

        public ValidateStatementScopedLocal DeclaredScopedLocal => this._declaredScopedLocal;

        public ValidateExpression LValue => this._lvalue;

        public ValidateExpression RValue => this._rvalue;

        public override void Validate(ValidateCode container, ValidateContext context)
        {
            if (this._declaredScopedLocal != null)
            {
                this._declaredScopedLocal.Validate(container, context);
                if (this._declaredScopedLocal.HasErrors)
                {
                    this.MarkHasErrors();
                    return;
                }
                this._declaredScopedLocal.HasInitialAssignment = true;
            }
            this._lvalue.MakeLValueUsage();
            this._lvalue.Validate(TypeRestriction.NotVoid, context);
            if (this._lvalue.HasErrors)
                this.MarkHasErrors();
            this._rvalue.Validate(TypeRestriction.NotVoid, context);
            if (this._rvalue.HasErrors)
                this.MarkHasErrors();
            if (this.HasErrors || this._lvalue.ObjectType.IsAssignableFrom(this._rvalue.ObjectType))
                return;
            this.ReportError("Invalid assignment: Type '{0}' cannot be assigned to type '{1}'", this._rvalue.ObjectType.Name, this._lvalue.ObjectType.Name);
        }
    }
}
