// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateProperty
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateProperty : Microsoft.Iris.Markup.Validation.Validate
    {
        public const string c_requiredValue = "$Required";
        public const string c_overrideProperty = "Override";
        private string _propertyName;
        private ValidateObject _value;
        private bool _multipleValueProperty;
        private int _valueCount;
        private ValueApplyMode _valueApplyMode;
        private PropertyAttribute _propertyAttributeList;
        private bool _allowPropertyAttributes;
        private PropertySchema _foundProperty;
        private int _foundPropertyIndex;
        private int _foundPropertyTypeIndex;
        private bool _isPseudo;
        private bool _shouldSkipDictionaryAddIfContains;
        private ValidateProperty _next;

        public ValidateProperty(SourceMarkupLoader owner, string propertyName, int line, int column)
          : this(owner, propertyName, null, line, column)
        {
        }

        public ValidateProperty(
          SourceMarkupLoader owner,
          string propertyName,
          ValidateObject value,
          int line,
          int column)
          : base(owner, line, column)
        {
            this._propertyName = propertyName;
            this._value = value;
        }

        public string PropertyName => this._propertyName;

        public ValidateObject Value
        {
            get => this._value;
            set => this._value = value;
        }

        public PropertyAttribute PropertyAttributeList => this._propertyAttributeList;

        public void AllowPropertyAttributes() => this._allowPropertyAttributes = true;

        public ValidateProperty Next
        {
            get => this._next;
            set => this._next = value;
        }

        public void AppendToEnd(ValidateProperty item)
        {
            ValidateProperty validateProperty = this;
            while (validateProperty.Next != null)
                validateProperty = validateProperty.Next;
            validateProperty.Next = item;
        }

        public void AddValue(ValidateObject value)
        {
            if (this._value == null)
                this._value = value;
            else if (this._value is ValidateObjectTag && value is ValidateObjectTag)
                this._value.AppendToEnd(value);
            else if (this._value is ValidateCode && value is ValidateCode)
                this._value.AppendToEnd(value);
            else
                this.ReportError("Property '{0}' cannot contain both object-tag and script block values", this._propertyName);
        }

        public void AddAttribute(PropertyAttribute attribute)
        {
            attribute.Next = this._propertyAttributeList;
            this._propertyAttributeList = attribute;
        }

        public void Validate(ValidateObjectTag targetObject, ValidateContext context)
        {
            TypeSchema objectType = targetObject.ObjectType;
            if (this._propertyName == "Override" && this.ValidateOverrideProperty(targetObject, context))
                return;
            string name = this._propertyName;
            int length = this._propertyName.IndexOf('.');
            if (length >= 0 && this._propertyName.Length > length + 1 && this._propertyName.Substring(0, length) == targetObject.TypeIdentifier.TypeName)
                name = this._propertyName.Substring(length + 1);
            this._foundProperty = objectType.FindPropertyDeep(name);
            if (this._foundProperty == null)
            {
                if (this._propertyName == "Name")
                    this.ValidateNameProperty(true, targetObject, context);
                else if (this._propertyName == targetObject.TypeIdentifier.TypeName)
                    this.ValidatePseudoConstructionProperty(targetObject, context);
                else
                    this.ReportError("Property '{0}' does not exist on '{1}'", this._propertyName, objectType.Name);
            }
            else
            {
                if (this._propertyName == "Name")
                    this.ValidateNameProperty(false, targetObject, context);
                this.UpdateFoundProperty(this._foundProperty);
                if (this._propertyAttributeList != null && !this._allowPropertyAttributes)
                    this.ReportError("Property attributes are not supported on property '{0}'", this._propertyName);
                if (this.IsCodeValue && ((ValidateCode)this._value).Next != null)
                {
                    this.ReportError("Property '{0}' does not support multi-value script blocks", this._propertyName);
                }
                else
                {
                    bool flag1 = DictionarySchema.Type.IsAssignableFrom(this._foundProperty.PropertyType);
                    bool flag2 = ListSchema.Type.IsAssignableFrom(this._foundProperty.PropertyType);
                    this._multipleValueProperty = flag1 || flag2;
                    bool flag3 = this.IsObjectTagValue && ((ValidateObjectTag)this._value).Next != null;
                    if (!this._multipleValueProperty && flag3)
                    {
                        this.ReportError("Property '{0}' does not support multiple values (property type is '{1}')", this._propertyName, this._foundProperty.PropertyType.Name);
                    }
                    else
                    {
                        context.NotifyPropertyScopeEnter(this);
                        try
                        {
                            if (!this._multipleValueProperty)
                            {
                                if (this._value == null)
                                {
                                    this.ReportError("Property '{0}' requires a value to be provided", this._propertyName);
                                    return;
                                }
                                this._valueCount = 1;
                                this._valueApplyMode = ValueApplyMode.SingleValueSet;
                                this._value.Validate(new TypeRestriction(this._foundProperty.PropertyType, this._foundProperty.AlternateType), context);
                                if (this._value.HasErrors)
                                {
                                    this.MarkHasErrors();
                                    return;
                                }
                                if (this._foundProperty.RangeValidator != null && this.IsFromStringValue)
                                {
                                    Result result = this._foundProperty.RangeValidator(((ValidateFromString)this._value).FromStringInstance);
                                    if (result.Failed)
                                        this.ReportError(result.Error);
                                }
                            }
                            else
                            {
                                TypeSchema primary = this._foundProperty.AlternateType ?? ObjectSchema.Type;
                                this._valueApplyMode = !flag1 ? ValueApplyMode.MultiValueList : ValueApplyMode.MultiValueDictionary;
                                if (this._foundProperty.CanWrite && this._foundProperty.PropertyType.HasDefaultConstructor)
                                    this._valueApplyMode |= ValueApplyMode.CollectionPopulateAndSet;
                                else
                                    this._valueApplyMode |= ValueApplyMode.CollectionAdd;
                                for (ValidateObject validateObject = this._value; validateObject != null; validateObject = validateObject.ObjectSourceType != ObjectSourceType.ObjectTag ? null : (ValidateObject)((ValidateObjectTag)validateObject).Next)
                                {
                                    ++this._valueCount;
                                    TypeRestriction typeRestriction = !flag3 ? new TypeRestriction(primary, this._foundProperty.PropertyType) : new TypeRestriction(primary);
                                    validateObject.Validate(typeRestriction, context);
                                    if (validateObject.HasErrors)
                                    {
                                        this.MarkHasErrors();
                                    }
                                    else
                                    {
                                        if (!flag3 && this._foundProperty.PropertyType.IsAssignableFrom(validateObject.ObjectType))
                                        {
                                            bool flag4 = true;
                                            if (flag1)
                                            {
                                                if (validateObject.ObjectSourceType == ObjectSourceType.ObjectTag && ((ValidateObjectTag)validateObject).Name != null)
                                                    flag4 = false;
                                            }
                                            else if (!this._foundProperty.CanWrite)
                                                flag4 = false;
                                            if (flag4)
                                            {
                                                this._valueApplyMode = ValueApplyMode.SingleValueSet;
                                                break;
                                            }
                                        }
                                        if (validateObject.ObjectSourceType != ObjectSourceType.ObjectTag)
                                        {
                                            this.ReportError("Collection property '{0}' may only have multiple values specified in expanded form", this._propertyName);
                                            break;
                                        }
                                        if ((this._valueApplyMode & ValueApplyMode.MultiValueDictionary) != ValueApplyMode.SingleValueSet && ((ValidateObjectTag)validateObject).Name == null)
                                            this.ReportError("Dictionary property '{0}' requires all values be named", this._propertyName);
                                    }
                                }
                            }
                            if (this._valueApplyMode == ValueApplyMode.SingleValueSet)
                            {
                                if (this._foundProperty.CanWrite)
                                    return;
                                this.ReportError("Property '{0}' does not support setting", this._foundProperty.Name);
                            }
                            else
                            {
                                if (this._foundProperty.CanRead)
                                    return;
                                this.ReportError("Property '{0}' must support reading to retrieve its collection for multiple value adds", this._foundProperty.Name);
                            }
                        }
                        finally
                        {
                            context.NotifyPropertyScopeExit(this);
                        }
                    }
                }
            }
        }

        private void ValidateNameProperty(
          bool isPseudo,
          ValidateObjectTag targetObject,
          ValidateContext context)
        {
            if (isPseudo)
                this.MarkAsPseudo();
            if (!this.IsFromStringValue)
            {
                this.ReportError("Property '{0}' does not support expanded value syntax", this._propertyName);
            }
            else
            {
                ValidateFromString validateFromString = (ValidateFromString)this._value;
                validateFromString.Validate(new TypeRestriction(StringSchema.Type), context);
                if (validateFromString.HasErrors)
                    this.MarkHasErrors();
                else
                    targetObject.Name = validateFromString.FromString;
            }
        }

        private void ValidatePseudoConstructionProperty(
          ValidateObjectTag targetObject,
          ValidateContext context)
        {
            bool flag = false;
            this.MarkAsPseudo();
            if (this.IsObjectTagValue && ((ValidateObjectTag)this._value).Next != null)
            {
                this.ReportError("Property '{0}' does not support multiple values (property type is '{1}')", this._propertyName, targetObject.ObjectType.Name);
            }
            else
            {
                if (this.IsFromStringValue && ((ValidateFromString)this._value).FromString == "$Required")
                {
                    flag = true;
                    targetObject.PropertyIsRequiredForCreation = flag;
                }
                if (flag)
                    return;
                this._value.Validate(new TypeRestriction(targetObject.ObjectType, TypeSchemaDefinition.Type), context);
                if (this._value.HasErrors)
                    this.MarkHasErrors();
                else if (targetObject.ObjectType.IsAssignableFrom(this._value.ObjectType))
                    targetObject.IndirectedObject = this._value;
                else
                    targetObject.DynamicConstructionType = this._value;
            }
        }

        private bool ValidateOverrideProperty(ValidateObjectTag targetObject, ValidateContext context)
        {
            if (context.ActiveSymbolScope != SymbolOrigin.Properties)
                return false;
            bool result;
            if (this.IsFromStringValue && bool.TryParse(((ValidateFromString)this._value).FromString, out result))
            {
                if (targetObject.PropertySchemaExport != null && context.Owner.TypeExport != null)
                {
                    PropertySchema propertySchema = null;
                    if (context.Owner.TypeExport.MarkupTypeBase != null)
                        propertySchema = context.Owner.TypeExport.MarkupTypeBase.FindPropertyDeep(targetObject.PropertySchemaExport.Name);
                    if (result && propertySchema == null)
                        this.ReportError("Property '{0}' was specified as Override='true' but no property was found to override", targetObject.PropertySchemaExport.Name);
                    else if (!result && propertySchema != null)
                        this.ReportError("Property '{0}' was specified as Override='false' but a base property was found", targetObject.PropertySchemaExport.Name);
                }
            }
            else
                this.ReportError("Property '{0}' must be Boolean 'true' or 'false'", "Override");
            this.MarkAsPseudo();
            return true;
        }

        public bool IsPseudo => this._isPseudo;

        public void MarkAsPseudo() => this._isPseudo = true;

        public void RepurposeProperty(string newName, PropertyAttribute newAttributes)
        {
            this._propertyName = newName;
            this._propertyAttributeList = newAttributes;
        }

        public bool IsObjectTagValue => this._value != null && this._value.ObjectSourceType == ObjectSourceType.ObjectTag;

        public bool IsFromStringValue => this._value != null && this._value.ObjectSourceType == ObjectSourceType.FromString;

        public bool IsCodeValue => this._value != null && this._value.ObjectSourceType == ObjectSourceType.Code;

        public bool IsExpressionValue => this._value != null && this._value.ObjectSourceType == ObjectSourceType.Expression;

        public void UpdateFoundProperty(PropertySchema newFoundProperty)
        {
            this._foundProperty = newFoundProperty;
            this._foundPropertyIndex = this.Owner.TrackImportedProperty(this._foundProperty);
            this._foundPropertyTypeIndex = this.Owner.TrackImportedType(this._foundProperty.PropertyType);
        }

        public PropertySchema FoundProperty => this._foundProperty;

        public int FoundPropertyIndex => this._foundPropertyIndex;

        public int FoundPropertyTypeIndex => this._foundPropertyTypeIndex;

        public int ValueCount => this._valueCount;

        public bool ShouldSkipDictionaryAddIfContains
        {
            get => this._shouldSkipDictionaryAddIfContains;
            set => this._shouldSkipDictionaryAddIfContains = value;
        }

        public ValueApplyMode ValueApplyMode => this._valueApplyMode;

        public void ReversePropertyValues()
        {
            ValidateObjectTag next;
            for (ValidateObjectTag validateObjectTag = (ValidateObjectTag)this._value; validateObjectTag != null; validateObjectTag = next)
            {
                next = validateObjectTag.Next;
                validateObjectTag.Next = null;
                if (validateObjectTag != this._value)
                {
                    validateObjectTag.Next = (ValidateObjectTag)this._value;
                    this._value = validateObjectTag;
                }
            }
        }

        public override string ToString() => this._value != null ? string.Format("{0} = {1}", _propertyName, this._value.ToString()) : "Unavailable";
    }
}
