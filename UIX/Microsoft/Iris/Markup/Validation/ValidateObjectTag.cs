// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateObjectTag
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateObjectTag : ValidateObject
    {
        private ValidateTypeIdentifier _typeIdentifier;
        private ValidateProperty _propertyList;
        private TypeSchema _foundType;
        private int _foundTypeIndex;
        private int _propertyCount;
        private string _name;
        private ValidateObject _indirectedObject;
        private ValidateObject _dynamicConstructionType;
        private bool _isRequired;
        private MarkupPropertySchema _propertySchemaExport;

        public ValidateObjectTag(
          SourceMarkupLoader owner,
          ValidateTypeIdentifier typeIdentifier,
          int line,
          int offset)
          : base(owner, line, offset, ObjectSourceType.ObjectTag)
        {
            this._typeIdentifier = typeIdentifier;
        }

        public ValidateTypeIdentifier TypeIdentifier => this._typeIdentifier;

        public ValidateProperty PropertyList => this._propertyList;

        public ValidateObjectTag Next
        {
            get => (ValidateObjectTag)base.Next;
            set => base.Next = (ValidateObject)value;
        }

        public void AddProperty(ValidateProperty property)
        {
            if (this._propertyList == null)
                this._propertyList = property;
            else
                this._propertyList.AppendToEnd(property);
        }

        public override TypeSchema ObjectType => this._foundType;

        public string Name
        {
            get => this._name;
            set => this._name = value;
        }

        public ValidateObject DynamicConstructionType
        {
            get => this._dynamicConstructionType;
            set => this._dynamicConstructionType = value;
        }

        public ValidateObject IndirectedObject
        {
            get => this._indirectedObject;
            set => this._indirectedObject = value;
        }

        public bool PropertyIsRequiredForCreation
        {
            get => this._isRequired;
            set => this._isRequired = value;
        }

        public virtual PropertyOverrideCriteria PropertyOverrideCriteria => (PropertyOverrideCriteria)null;

        public MarkupPropertySchema PropertySchemaExport
        {
            get => this._propertySchemaExport;
            set => this._propertySchemaExport = value;
        }

        public virtual void NotifyParseComplete()
        {
        }

        protected bool ValidateObjectTagType()
        {
            if (this._typeIdentifier.HasErrors || this._typeIdentifier.FoundType != null)
                return !this._typeIdentifier.HasErrors;
            this._typeIdentifier.Validate();
            if (this._typeIdentifier.HasErrors)
            {
                this.MarkHasErrors();
                return false;
            }
            this._foundType = this._typeIdentifier.FoundType;
            this._foundTypeIndex = this._typeIdentifier.FoundTypeIndex;
            return true;
        }

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            if (!this.ValidateObjectTagType())
                return;
            if (context.CurrentPass == LoadPass.PopulatePublicModel)
            {
                if (this._foundType == null || !(this.GetInlinePropertyValueNoValidate(this._foundType.Name) == "$Required"))
                    return;
                this.PropertyIsRequiredForCreation = true;
            }
            else
            {
                if (context.CurrentPass != LoadPass.Full || !typeRestriction.Check((ValidateObject)this, this._foundType))
                    return;
                if (!this._foundType.HasDefaultConstructor && !this.ForceAbstractAsConcrete && this.FindProperty(this._foundType.Name) == null)
                {
                    this.ReportError("Type '{0}' may only be created via indirection (i.e. <{0} {0}=\"...\"/>)", this._foundType.Name);
                }
                else
                {
                    context.NotifyObjectTagScopeEnter(this);
                    try
                    {
                        this.ValidateProperties(context);
                    }
                    finally
                    {
                        Result result = context.NotifyObjectTagScopeExit(this);
                        if (result.Failed)
                            this.ReportError(result.Error);
                    }
                    if (this._propertySchemaExport == null)
                        return;
                    this._propertySchemaExport.SetOverrideCriteria(this.PropertyOverrideCriteria);
                }
            }
        }

        protected virtual void ValidateProperties(ValidateContext context)
        {
            this._propertyCount = 0;
            for (ValidateProperty property = this._propertyList; property != null; property = property.Next)
            {
                this.ValidateDuplicateProperties(property);
                property.Validate(this, context);
                if (property.HasErrors)
                    this.MarkHasErrors();
                if (!property.IsPseudo)
                    ++this._propertyCount;
            }
            if (this.PropertyIsRequiredForCreation)
            {
                PropertySchema activePropertyScope = context.GetActivePropertyScope();
                if (activePropertyScope == ValidateContext.ClassPropertiesProperty || activePropertyScope == ValidateContext.UIPropertiesProperty)
                    return;
                this.ReportError("'$Required' can't be used in this context. It may only be used for declaring required properties on Classes and UIs");
            }
            else if (this._indirectedObject != null)
            {
                if (this._indirectedObject.HasErrors)
                    return;
                if (!this._foundType.IsAssignableFrom(this._indirectedObject.ObjectType))
                {
                    this.ReportError("'{0}' cannot be used in this context (expecting types compatible with '{1}')", this._indirectedObject.ObjectType.Name, this._foundType.Name);
                }
                else
                {
                    for (ValidateProperty validateProperty = this._propertyList; validateProperty != null; validateProperty = validateProperty.Next)
                    {
                        if (!validateProperty.IsPseudo)
                        {
                            if (validateProperty.PropertyName != "Name")
                            {
                                this.ReportError("Property sets are not supported on type indirection tag. Property specified was '{0}'", validateProperty.PropertyName);
                                break;
                            }
                            validateProperty.MarkAsPseudo();
                        }
                    }
                }
            }
            else
                this.ValidateRequiredProperties();
        }

        private void ValidateDuplicateProperties(ValidateProperty property)
        {
            bool flag = false;
            for (ValidateProperty validateProperty = this._propertyList; validateProperty != null; validateProperty = validateProperty.Next)
            {
                if (validateProperty.PropertyName == property.PropertyName)
                {
                    if (flag)
                    {
                        property.ReportError("Property '{0}' was specified more than once", property.PropertyName);
                        this.MarkHasErrors();
                        break;
                    }
                    flag = true;
                }
            }
        }

        private void ValidateRequiredProperties()
        {
            foreach (string str in this._foundType.FindRequiredPropertyNamesDeep())
            {
                bool flag = false;
                for (ValidateProperty validateProperty = this._propertyList; validateProperty != null; validateProperty = validateProperty.Next)
                {
                    if (validateProperty.PropertyName == str)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    this.ReportError("Property '{0}' must be specified for '{1}' to function", str, this._foundType.Name);
                    this.MarkHasErrors();
                }
            }
        }

        protected virtual bool ForceAbstractAsConcrete => false;

        public ValidateProperty FindProperty(string propertyName) => this.FindProperty(propertyName, false);

        public ValidateProperty FindProperty(string propertyName, bool remove)
        {
            ValidateProperty validateProperty1 = this._propertyList;
            ValidateProperty validateProperty2 = (ValidateProperty)null;
            for (; validateProperty1 != null; validateProperty1 = validateProperty1.Next)
            {
                if (propertyName == validateProperty1.PropertyName)
                {
                    if (remove)
                    {
                        if (validateProperty2 != null)
                            validateProperty2.Next = validateProperty1.Next;
                        if (validateProperty1 == this._propertyList)
                            this._propertyList = validateProperty1.Next;
                        validateProperty1.Next = (ValidateProperty)null;
                    }
                    return validateProperty1;
                }
                validateProperty2 = validateProperty1;
            }
            return (ValidateProperty)null;
        }

        public void RemoveProperty(ValidateProperty propertyRemove)
        {
            ValidateProperty validateProperty1 = this._propertyList;
            ValidateProperty validateProperty2 = (ValidateProperty)null;
            for (; validateProperty1 != null; validateProperty1 = validateProperty1.Next)
            {
                if (validateProperty1 == propertyRemove)
                {
                    if (validateProperty2 != null)
                        validateProperty2.Next = validateProperty1.Next;
                    if (validateProperty1 == this._propertyList)
                        this._propertyList = validateProperty1.Next;
                    validateProperty1.Next = (ValidateProperty)null;
                    break;
                }
                validateProperty2 = validateProperty1;
            }
        }

        public string GetInlinePropertyValueNoValidate(string propertyName)
        {
            ValidateProperty property = this.FindProperty(propertyName);
            return property != null && property.IsFromStringValue ? ((ValidateFromString)property.Value).FromString : (string)null;
        }

        public void MovePropertyToFront(string propertyName)
        {
            ValidateProperty property = this.FindProperty(propertyName, true);
            if (property == null)
                return;
            property.Next = this._propertyList;
            this._propertyList = property;
        }

        public void AddStringProperty(string propertyName, string value)
        {
            ValidateFromString validateFromString = new ValidateFromString(this.Owner, value, false, this.Line, this.Column);
            ValidateProperty validateProperty = new ValidateProperty(this.Owner, propertyName, (ValidateObject)validateFromString, this.Line, this.Column);
            if (this._propertyList == null)
                this._propertyList = validateProperty;
            else
                this._propertyList.AppendToEnd(validateProperty);
        }

        public TypeSchema ExtractTypeSchemaProperty(
          string propertyName,
          ValidateContext context,
          bool required)
        {
            ValidateProperty property = this.FindProperty(propertyName, true);
            if (property != null)
            {
                ValidateTypeIdentifier.PromoteSimplifiedTypeSyntax(property);
                if (property.IsExpressionValue)
                {
                    ValidateExpression validateExpression = (ValidateExpression)property.Value;
                    if (validateExpression.ExpressionType == ExpressionType.TypeOf)
                    {
                        validateExpression.Validate(new TypeRestriction((TypeSchema)TypeSchemaDefinition.Type), context);
                        if (!validateExpression.HasErrors)
                            return ((ValidateExpressionTypeOf)validateExpression).TypeIdentifier.FoundType;
                        this.MarkHasErrors();
                    }
                    else
                        this.ReportError("Invalid expression for '{0}', expecting plain type identifier", property.PropertyName);
                }
                else if (property.IsFromStringValue)
                    this.ReportError("Invalid value '{0}' for attribute '{1}'", ((ValidateFromString)property.Value).FromString, property.PropertyName);
                else
                    this.ReportError("Property '{0}' does not support expanded value syntax", property.PropertyName);
            }
            else if (required)
                this.ReportError("Property '{0}' must be specified", propertyName);
            return (TypeSchema)null;
        }

        public bool ExtractBooleanProperty(
          string propertyName,
          ValidateContext context,
          bool required,
          out bool value)
        {
            ValidateProperty property = this.FindProperty(propertyName, true);
            if (property != null)
            {
                if (property.IsFromStringValue)
                {
                    ValidateFromString validateFromString = (ValidateFromString)property.Value;
                    validateFromString.Validate(new TypeRestriction((TypeSchema)BooleanSchema.Type), context);
                    if (!validateFromString.HasErrors)
                    {
                        value = (bool)validateFromString.FromStringInstance;
                        return true;
                    }
                    this.MarkHasErrors();
                }
                else
                    this.ReportError("Property '{0}' does not support expanded value syntax", property.PropertyName);
            }
            else if (required)
                this.ReportError("Property '{0}' must be specified", propertyName);
            value = false;
            return false;
        }

        public string ExtractStringProperty(string propertyName, bool required)
        {
            ValidateProperty property = this.FindProperty(propertyName, true);
            if (property != null)
            {
                if (property.IsFromStringValue)
                    return ((ValidateFromString)property.Value).FromString;
                this.ReportError("Property '{0}' does not support expanded value syntax", propertyName);
            }
            else if (required)
                this.ReportError("Property '{0}' must be specified", propertyName);
            return (string)null;
        }

        public TypeSchema FoundType => this._foundType;

        public int FoundTypeIndex => this._foundTypeIndex;

        public int PropertyCount => this._propertyCount;

        public override string ToString()
        {
            string str = "";
            if (this._name != null)
                str += string.Format("Name='{0}'", (object)this._name);
            if (this._indirectedObject != null)
            {
                if (this._name != null)
                    str += ", ";
                str += string.Format("Indirected=[{0}]", (object)this._indirectedObject.ToString());
            }
            return string.Format("Tag : {0} {1}", (object)this._typeIdentifier, (object)str);
        }
    }
}
