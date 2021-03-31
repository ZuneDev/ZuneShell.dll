// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpressionCast
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateExpressionCast : ValidateExpression
    {
        private ValidateTypeIdentifier _typeCast;
        private ValidateExpression _castee;
        private CastMethod _foundCastMethod;
        private TypeSchema _foundCasteeType;
        private int _foundCasteeTypeIndex = -1;
        private TypeSchema _foundTypeCast;
        private int _foundTypeCastIndex = -1;

        public ValidateExpressionCast(
          SourceMarkupLoader owner,
          ValidateTypeIdentifier typeCast,
          ValidateExpression castee,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.Cast)
        {
            this._typeCast = typeCast;
            this._castee = castee;
        }

        public ValidateExpressionCast(
          SourceMarkupLoader owner,
          ValidateExpression typeCastExpression,
          ValidateExpression castee,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.Cast)
        {
            if (typeCastExpression.ExpressionType != ExpressionType.Symbol)
            {
                this.ReportError("Type cast was expecting symbol, found '{0}'", typeCastExpression.ExpressionType.ToString());
            }
            else
            {
                ValidateExpressionSymbol expressionSymbol = (ValidateExpressionSymbol)typeCastExpression;
                this._typeCast = new ValidateTypeIdentifier(owner, (string)null, expressionSymbol.Symbol, expressionSymbol.Line, expressionSymbol.Column);
            }
            this._castee = castee;
        }

        public ValidateTypeIdentifier TypeCast => this._typeCast;

        public ValidateExpression Castee => this._castee;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            if (this._typeCast == null)
                return;
            this._typeCast.Validate();
            if (this._typeCast.HasErrors)
            {
                this.MarkHasErrors();
            }
            else
            {
                this._foundTypeCast = this._typeCast.FoundType;
                this._foundTypeCastIndex = this._typeCast.FoundTypeIndex;
                this.DeclareEvaluationType(this._foundTypeCast, typeRestriction);
                if (this.Usage == ExpressionUsage.LValue)
                    this.ReportError("Expression cannot be used as the target an assignment (related symbol: '{0}')", this._foundTypeCast.Name);
                this._castee.Validate(TypeRestriction.NotVoid, context);
                if (this._castee.HasErrors)
                {
                    this.MarkHasErrors();
                }
                else
                {
                    this._foundCasteeType = this._castee.ObjectType;
                    if (this._foundTypeCast.Contractual || this._foundCasteeType.Contractual)
                    {
                        this._foundCastMethod = CastMethod.Cast;
                    }
                    else
                    {
                        if (this._foundCasteeType == this._foundTypeCast)
                            return;
                        bool flag1 = this._foundCasteeType.IsAssignableFrom(this._foundTypeCast);
                        bool flag2 = this._foundTypeCast.IsAssignableFrom(this._foundCasteeType);
                        if (!flag1 && !flag2)
                        {
                            if (this._foundTypeCast.SupportsTypeConversion(this._foundCasteeType))
                            {
                                this._foundCastMethod = CastMethod.Conversion;
                                this._foundCasteeTypeIndex = this.Owner.TrackImportedType(this._foundCasteeType);
                            }
                            else
                                this.ReportError("Cannot cast '{0}' to '{1}'", this._foundCasteeType.Name, this._foundTypeCast.Name);
                        }
                        else
                        {
                            if (!flag1)
                                return;
                            this._foundCastMethod = CastMethod.Cast;
                        }
                    }
                }
            }
        }

        public CastMethod FoundCastMethod => this._foundCastMethod;

        public TypeSchema FoundCasteeType => this._foundCasteeType;

        public int FoundCasteeTypeIndex => this._foundCasteeTypeIndex;

        public TypeSchema FoundTypeCast => this._foundTypeCast;

        public int FoundTypeCastIndex => this._foundTypeCastIndex;
    }
}
