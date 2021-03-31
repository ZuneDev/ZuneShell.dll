// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpressionAssignment
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateExpressionAssignment : ValidateExpression
    {
        private ValidateExpression _lvalue;
        private ValidateExpression _rvalue;
        private bool _isIndexAssignment;

        public ValidateExpressionAssignment(
          SourceMarkupLoader owner,
          ValidateExpression lvalue,
          ValidateExpression rvalue,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.Assignment)
        {
            this._lvalue = lvalue;
            this._rvalue = rvalue;
        }

        public ValidateExpression LValue => this._lvalue;

        public ValidateExpression RValue => this._rvalue;

        public bool IsIndexAssignment => this._isIndexAssignment;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            if (this._lvalue.ExpressionType == ExpressionType.Index)
            {
                this._isIndexAssignment = true;
                ((ValidateExpressionIndex)this._lvalue).SetAssignmentValue(this._rvalue);
            }
            this._lvalue.MakeLValueUsage();
            this._lvalue.Validate(TypeRestriction.NotVoid, context);
            if (this._lvalue.HasErrors)
                this.MarkHasErrors();
            if (this._lvalue.ObjectType != null)
                this.DeclareEvaluationType(this._lvalue.ObjectType, typeRestriction);
            if (!this._rvalue.HasErrors && this._rvalue.ObjectType == null)
                this._rvalue.Validate(TypeRestriction.NotVoid, context);
            if (this._rvalue.HasErrors)
                this.MarkHasErrors();
            if (this.HasErrors || this._lvalue.ObjectType.IsAssignableFrom(this._rvalue.ObjectType))
                return;
            this.ReportError("Invalid assignment: Type '{0}' cannot be assigned to type '{1}'", this._rvalue.ObjectType.Name, this._lvalue.ObjectType.Name);
        }
    }
}
