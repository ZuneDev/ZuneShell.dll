// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateStatementScopedLocal
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateStatementScopedLocal : ValidateStatement
    {
        private string _name;
        private ValidateTypeIdentifier _typeIdentifier;
        private bool _hasInitialAssignment;
        private TypeSchema _foundType;
        private int _foundTypeIndex;
        private int _foundSymbolIndex;

        public ValidateStatementScopedLocal(
          SourceMarkupLoader owner,
          string name,
          ValidateTypeIdentifier typeIdentifier,
          int line,
          int column)
          : base(owner, line, column, StatementType.ScopedLocal)
        {
            this._name = name;
            this._typeIdentifier = typeIdentifier;
        }

        public string Name => this._name;

        public ValidateTypeIdentifier TypeIdentifier => this._typeIdentifier;

        public override void Validate(ValidateCode container, ValidateContext context)
        {
            if (context.CurrentPass >= LoadPass.PopulatePublicModel && !this._typeIdentifier.Validated)
            {
                this._typeIdentifier.Validate();
                if (this._typeIdentifier.HasErrors)
                {
                    this.MarkHasErrors();
                    return;
                }
                this._foundType = this._typeIdentifier.FoundType;
                this._foundTypeIndex = this._typeIdentifier.FoundTypeIndex;
            }
            if (context.CurrentPass != LoadPass.Full)
                return;
            Result result = context.NotifyScopedLocal(this._name, this._foundType);
            if (result.Failed)
                this.ReportError(result.Error);
            else
                this._foundSymbolIndex = context.TrackSymbolUsage(this._name, SymbolOrigin.ScopedLocal);
        }

        public bool HasInitialAssignment
        {
            get => this._hasInitialAssignment;
            set => this._hasInitialAssignment = value;
        }

        public TypeSchema FoundType => this._foundType;

        public int FoundTypeIndex => this._foundTypeIndex;

        public int FoundSymbolIndex => this._foundSymbolIndex;
    }
}
