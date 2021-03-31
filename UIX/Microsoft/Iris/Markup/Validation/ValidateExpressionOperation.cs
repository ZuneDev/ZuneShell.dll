// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpressionOperation
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateExpressionOperation : ValidateExpression
    {
        private ValidateExpression _leftSide;
        private ValidateExpression _rightSide;
        private OperationType _op;
        private TypeSchema _foundOperationTargetType;
        private int _foundOperationTargetTypeIndex = -1;

        public ValidateExpressionOperation(
          SourceMarkupLoader owner,
          ValidateExpression leftSide,
          OperationType op,
          ValidateExpression rightSide,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.Operation)
        {
            this._leftSide = leftSide;
            this._rightSide = rightSide;
            this._op = op;
        }

        public ValidateExpression LeftSide => this._leftSide;

        public ValidateExpression RightSide => this._rightSide;

        public OperationType Op => this._op;

        private static string GetOperationToken(OperationType op)
        {
            string str = null;
            switch (op)
            {
                case OperationType.MathAdd:
                    str = "+";
                    break;
                case OperationType.MathSubtract:
                    str = "-";
                    break;
                case OperationType.MathMultiply:
                    str = "*";
                    break;
                case OperationType.MathDivide:
                    str = "\\";
                    break;
                case OperationType.MathModulus:
                    str = "%";
                    break;
                case OperationType.LogicalAnd:
                    str = "&&";
                    break;
                case OperationType.LogicalOr:
                    str = "||";
                    break;
                case OperationType.RelationalEquals:
                    str = "==";
                    break;
                case OperationType.RelationalNotEquals:
                    str = "!=";
                    break;
                case OperationType.RelationalLessThan:
                    str = "<";
                    break;
                case OperationType.RelationalGreaterThan:
                    str = ">";
                    break;
                case OperationType.RelationalLessThanEquals:
                    str = "<=";
                    break;
                case OperationType.RelationalGreaterThanEquals:
                    str = ">=";
                    break;
                case OperationType.RelationalIs:
                    str = "is";
                    break;
                case OperationType.LogicalNot:
                    str = "!";
                    break;
            }
            return str;
        }

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            if (this._op == OperationType.PostIncrement || this._op == OperationType.PostDecrement)
                this.ReportError("Post increment/decrement operators are not currently supported");
            if (this.Usage == ExpressionUsage.LValue)
                this.ReportError("Expression cannot be used as the target an assignment (related symbol: '{0}')", "Operation");
            this._leftSide.Validate(TypeRestriction.NotVoid, context);
            if (this._leftSide.HasErrors)
                this.MarkHasErrors();
            if (this._rightSide != null)
            {
                this._rightSide.Validate(TypeRestriction.NotVoid, context);
                if (this._rightSide.HasErrors)
                    this.MarkHasErrors();
            }
            if (this.HasErrors)
                return;
            if (this._rightSide != null && !this._leftSide.ObjectType.IsAssignableFrom(this._rightSide.ObjectType))
            {
                if (this._leftSide.ObjectType == NullSchema.Type && this._rightSide.ObjectType.IsNullAssignable || this._rightSide.ObjectType == NullSchema.Type && this._leftSide.ObjectType.IsNullAssignable)
                {
                    this._foundOperationTargetType = NullSchema.Type;
                }
                else
                {
                    this.ReportError("Operator '{0}' cannot be applied to operands of dissimilar types '{1}' and '{2}'", ValidateExpressionOperation.GetOperationToken(this._op), this._leftSide.ObjectType.Name, this._rightSide.ObjectType.Name);
                    return;
                }
            }
            else
                this._foundOperationTargetType = this._leftSide.ObjectType;
            if (!this._foundOperationTargetType.SupportsOperationDeep(this._op))
            {
                this.ReportError("Operator '{0}' cannot be applied to operand of type '{1}'", ValidateExpressionOperation.GetOperationToken(this._op), this._foundOperationTargetType.Name);
            }
            else
            {
                this._foundOperationTargetTypeIndex = this.Owner.TrackImportedType(this._foundOperationTargetType);
                switch (this._op)
                {
                    case OperationType.MathAdd:
                    case OperationType.MathSubtract:
                    case OperationType.MathMultiply:
                    case OperationType.MathDivide:
                    case OperationType.MathModulus:
                    case OperationType.MathNegate:
                    case OperationType.PostIncrement:
                    case OperationType.PostDecrement:
                        this.DeclareEvaluationType(this._foundOperationTargetType, typeRestriction);
                        break;
                    case OperationType.LogicalAnd:
                    case OperationType.LogicalOr:
                    case OperationType.RelationalEquals:
                    case OperationType.RelationalNotEquals:
                    case OperationType.RelationalLessThan:
                    case OperationType.RelationalGreaterThan:
                    case OperationType.RelationalLessThanEquals:
                    case OperationType.RelationalGreaterThanEquals:
                    case OperationType.RelationalIs:
                    case OperationType.LogicalNot:
                        this.DeclareEvaluationType(BooleanSchema.Type, typeRestriction);
                        break;
                }
            }
        }

        public TypeSchema FoundOperationTargetType => this._foundOperationTargetType;

        public int FoundOperationTargetTypeIndex => this._foundOperationTargetTypeIndex;
    }
}
