// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateAlias
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateAlias : ValidateObjectTag
    {
        private string _aliasName;
        private ValidateTypeIdentifier _typeIdentifier;
        private LoadPass _currentValidationPass;

        public ValidateAlias(
          SourceMarkupLoader owner,
          ValidateTypeIdentifier typeIdentifier,
          int line,
          int offset)
          : base(owner, typeIdentifier, line, offset)
        {
        }

        public void Validate(LoadPass currentPass)
        {
            if (this._currentValidationPass >= currentPass)
                return;
            this._currentValidationPass = currentPass;
            if (this._currentValidationPass == LoadPass.DeclareTypes)
            {
                ValidateContext context = new ValidateContext(null, null, this._currentValidationPass);
                this.Validate(TypeRestriction.None, context);
                this._aliasName = this.GetInlinePropertyValueNoValidate("Name");
                string propertyValueNoValidate = this.GetInlinePropertyValueNoValidate("Type");
                if (propertyValueNoValidate != null)
                {
                    this._typeIdentifier = new ValidateTypeIdentifier(this.Owner, propertyValueNoValidate, this.Line, this.Column);
                    this.Owner.NotifyTypeIdentifierFound(this._typeIdentifier.Prefix, this._typeIdentifier.TypeName);
                    string typeName = this._typeIdentifier.TypeName;
                    if (this._aliasName == null)
                        this._aliasName = typeName;
                    LoadResult dependency = this.Owner.FindDependency(this._typeIdentifier.Prefix);
                    if (dependency == null)
                        this.ReportError("Xmlns prefix '{0}' was not found", this._typeIdentifier.Prefix);
                    else if (!ValidateContext.IsValidSymbolName(this._aliasName))
                    {
                        this.ReportError("Invalid name \"{0}\".  Valid names must begin with either an alphabetic character or an underscore and can otherwise contain only alphabetic, numeric, or underscore characters", this._aliasName);
                    }
                    else
                    {
                        if (this.Owner.IsTypeNameTaken(this._aliasName))
                            this.ReportError("Type '{0}' was specified more than once", this._aliasName);
                        this.Owner.RegisterAlias(this._aliasName, dependency, typeName);
                    }
                }
                else
                    this.ReportError("Alias Type property must be provided");
            }
            else
            {
                if (this._currentValidationPass != LoadPass.Full || this.Owner.LoadResultTarget.FindType(this._aliasName) != null)
                    return;
                this.ReportError("Target type for alias '{0}' could not be found", this._aliasName);
            }
        }
    }
}
