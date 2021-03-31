// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpressionIndex
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateExpressionIndex : ValidateExpression
    {
        private ValidateExpression _indexee;
        private ValidateParameter _index;
        private ValidateExpressionCall _call;
        private ValidateExpression _assignmentValue;

        public ValidateExpressionIndex(
          SourceMarkupLoader owner,
          ValidateExpression indexee,
          ValidateParameter index,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.Index)
        {
            this._indexee = indexee;
            this._index = index;
        }

        public ValidateExpression CallExpression => (ValidateExpression)this._call;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            if (this.Usage == ExpressionUsage.RValue || this.Usage == ExpressionUsage.DeclareTrigger)
            {
                this._call = new ValidateExpressionCall(this.Owner, this._indexee, "get_Item", this._index, this.Line, this.Column);
                this._call.Validate(TypeRestriction.NotVoid, context);
                if (this._call.HasErrors)
                    this.MarkHasErrors();
                else
                    this.DeclareEvaluationType(this._call.ObjectType, typeRestriction);
            }
            else
            {
                this._index.AppendToEnd(new ValidateParameter(this.Owner, this._assignmentValue, this._assignmentValue.Line, this._assignmentValue.Column));
                this._call = new ValidateExpressionCall(this.Owner, this._indexee, "set_Item", this._index, this.Line, this.Column);
                this._call.SetAsIndexAssignment();
                this._call.Validate(new TypeRestriction((TypeSchema)VoidSchema.Type), context);
                if (this._call.HasErrors)
                    this.MarkHasErrors();
                else
                    this.DeclareEvaluationType(this._assignmentValue.ObjectType, typeRestriction);
            }
        }

        public void SetAssignmentValue(ValidateExpression assignmentValue) => this._assignmentValue = assignmentValue;
    }
}
