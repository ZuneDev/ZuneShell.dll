// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpressionSymbol
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateExpressionSymbol : ValidateExpression
    {
        private string _symbol;
        private int _foundSymbolIndex = -1;
        private SymbolOrigin _foundSymbolOrigin;
        private bool _foundSymbolIsType;

        public ValidateExpressionSymbol(SourceMarkupLoader owner, string symbol, int line, int column)
          : base(owner, line, column, ExpressionType.Symbol)
          => this._symbol = symbol;

        public string Symbol => this._symbol;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context) => this.Validate(typeRestriction, context, false);

        public void Validate(
          TypeRestriction typeRestriction,
          ValidateContext context,
          bool allowTypeSymbols)
        {
            ExpressionRestriction expressionRestriction;
            TypeSchema evaluationType = context.ResolveSymbol(this._symbol, out this._foundSymbolOrigin, out expressionRestriction);
            if (evaluationType == null)
            {
                if (allowTypeSymbols)
                {
                    ValidateTypeIdentifier validateTypeIdentifier = new ValidateTypeIdentifier(this.Owner, (string)null, this._symbol, this.Line, this.Column);
                    validateTypeIdentifier.Validate();
                    if (validateTypeIdentifier.HasErrors)
                    {
                        this.MarkHasErrors();
                        return;
                    }
                    evaluationType = validateTypeIdentifier.FoundType;
                    this._foundSymbolIndex = validateTypeIdentifier.FoundTypeIndex;
                    this._foundSymbolIsType = true;
                }
                else
                {
                    this.ReportError("Unable to locate symbol \"{0}\" within Properties, Locals, Input, or Content", this._symbol);
                    return;
                }
            }
            if (this._foundSymbolOrigin != SymbolOrigin.None)
                this._foundSymbolIndex = context.TrackSymbolUsage(this._symbol, this._foundSymbolOrigin);
            this.DeclareEvaluationType(evaluationType, typeRestriction);
            if (expressionRestriction == ExpressionRestriction.NoAccess)
            {
                this.ReportError("Expression access to '{0}' is not available", this._symbol);
            }
            else
            {
                if (this.Usage == ExpressionUsage.LValue && expressionRestriction != ExpressionRestriction.None)
                    this.ReportError("Expression access to '{0}' only supports read operations", this._symbol);
                if (expressionRestriction != ExpressionRestriction.None)
                    return;
                this.DeclareNotifies(context);
            }
        }

        public int FoundSymbolIndex => this._foundSymbolIndex;

        public SymbolOrigin FoundSymbolOrigin => this._foundSymbolOrigin;

        public bool FoundSymbolIsType => this._foundSymbolIsType;
    }
}
