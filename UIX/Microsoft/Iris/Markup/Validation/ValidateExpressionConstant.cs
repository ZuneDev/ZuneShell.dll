// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpressionConstant
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateExpressionConstant : ValidateExpression
    {
        private string _constantInline;
        private ConstantType _constantType;
        private object _foundConstant;
        private int _foundTypeIndex;

        public ValidateExpressionConstant(
          SourceMarkupLoader owner,
          string constantInline,
          ConstantType constantType,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.Constant)
        {
            this._constantInline = constantInline;
            this._constantType = constantType;
        }

        public ValidateExpressionConstant(
          SourceMarkupLoader owner,
          bool constantValue,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.Constant)
        {
            this._constantInline = null;
            this._constantType = ConstantType.Boolean;
            this._foundConstant = BooleanBoxes.Box(constantValue);
        }

        public ConstantType ConstantType => this._constantType;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            if (this.Usage == ExpressionUsage.LValue)
                this.ReportError("Expression cannot be used as the target an assignment (related symbol: '{0}')", this._constantInline);
            TypeSchema evaluationType;
            switch (this._constantType)
            {
                case ConstantType.String:
                    int errorIndex;
                    string invalidSequence;
                    this._foundConstant = StringUtility.Unescape(this._constantInline, out errorIndex, out invalidSequence);
                    if (this._foundConstant == null)
                        this.ReportErrorWithAdjustedPosition(string.Format("Invalid escape sequence '{0}' in string literal", invalidSequence), 0, errorIndex + 1);
                    evaluationType = StringSchema.Type;
                    break;
                case ConstantType.StringLiteral:
                    this._foundConstant = _constantInline;
                    this._constantType = ConstantType.String;
                    evaluationType = StringSchema.Type;
                    break;
                case ConstantType.Integer:
                    Result result1 = Int32Schema.Type.TypeConverter(_constantInline, StringSchema.Type, out this._foundConstant);
                    if (result1.Failed)
                        this.ReportError(result1.Error);
                    evaluationType = Int32Schema.Type;
                    break;
                case ConstantType.LongInteger:
                    Result result2 = Int64Schema.Type.TypeConverter(_constantInline, StringSchema.Type, out this._foundConstant);
                    if (result2.Failed)
                        this.ReportError(result2.Error);
                    evaluationType = Int64Schema.Type;
                    break;
                case ConstantType.Float:
                    Result result3 = SingleSchema.Type.TypeConverter(_constantInline, StringSchema.Type, out this._foundConstant);
                    if (result3.Failed)
                        this.ReportError(result3.Error);
                    evaluationType = SingleSchema.Type;
                    break;
                case ConstantType.Boolean:
                    evaluationType = BooleanSchema.Type;
                    break;
                case ConstantType.Null:
                    this._foundConstant = null;
                    evaluationType = NullSchema.Type;
                    break;
                default:
                    evaluationType = null;
                    break;
            }
            this.DeclareEvaluationType(evaluationType, typeRestriction);
            if (this.ObjectType == null)
                this.MarkHasErrors();
            else
                this._foundTypeIndex = this.Owner.TrackImportedType(this.ObjectType);
        }

        public object FoundConstant => this._foundConstant;

        public int FoundTypeIndex => this._foundTypeIndex;
    }
}
