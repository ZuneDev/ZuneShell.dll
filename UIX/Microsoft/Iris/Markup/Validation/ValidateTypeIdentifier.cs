// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateTypeIdentifier
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateTypeIdentifier : Microsoft.Iris.Markup.Validation.Validate
    {
        private string _prefix;
        private string _typeName;
        private TypeSchema _foundType;
        private int _foundTypeIndex = -1;

        public ValidateTypeIdentifier(
          SourceMarkupLoader owner,
          string prefix,
          string typeName,
          int line,
          int column)
          : base(owner, line, column)
        {
            this._prefix = prefix;
            this._typeName = typeName;
            if (this._prefix == string.Empty)
                this._prefix = (string)null;
            owner.NotifyTypeIdentifierFound(this._prefix, this._typeName);
        }

        public ValidateTypeIdentifier(SourceMarkupLoader owner, string typeFQN, int line, int column)
          : base(owner, line, column)
        {
            int length = typeFQN.IndexOf(':');
            if (length >= 0)
            {
                this._prefix = typeFQN.Substring(0, length);
                this._typeName = typeFQN.Substring(length + 1);
            }
            else
                this._typeName = typeFQN;
            owner.NotifyTypeIdentifierFound(this._prefix, this._typeName);
        }

        public static void PromoteSimplifiedTypeSyntax(ValidateProperty property)
        {
            if (property == null || !property.IsFromStringValue)
                return;
            string fromString = ((ValidateFromString)property.Value).FromString;
            ValidateTypeIdentifier typeIdentifier = new ValidateTypeIdentifier(property.Owner, fromString, property.Line, property.Column);
            property.Value = (ValidateObject)new ValidateExpressionTypeOf(property.Owner, typeIdentifier, property.Line, property.Column);
        }

        public string Prefix => this._prefix;

        public string TypeName => this._typeName;

        public void Validate()
        {
            LoadResult dependency = this.Owner.FindDependency(this._prefix);
            if (dependency == null)
            {
                this.ReportError("Xmlns prefix '{0}' was not found", this._prefix);
            }
            else
            {
                this._foundType = dependency.FindType(this._typeName);
                if (this._foundType == null)
                    this.ReportError("Type '{0}' was not found within '{1}'", this._typeName, dependency.Uri);
                else
                    this._foundTypeIndex = this.Owner.TrackImportedType(this._foundType);
            }
        }

        public TypeSchema FoundType => this._foundType;

        public int FoundTypeIndex => this._foundTypeIndex;

        public bool Validated => this.FoundType != null || this.HasErrors;

        public override string ToString() => this._foundType != null ? this._foundType.ToString() : this._typeName + " (Unresolved)";
    }
}
