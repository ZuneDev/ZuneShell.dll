// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateClass
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;
using System;
using System.Collections;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateClass : ValidateObjectTag
    {
        private ValidateScripts _scripts;
        private ValidateTypeIdentifier _baseTypeIdentifier;
        private ValidateProperty _foundPropertiesValidateProperty;
        private ValidateProperty _foundScriptsProperty;
        private ValidateMethodList _foundMethods;
        private Vector<MarkupMethodSchema> _foundVirtualMethods;
        private Vector<MarkupMethodSchema> _foundVirtualBaseMethods;
        protected MarkupTypeSchema _typeExport;
        private int _typeExportIndex;
        private MarkupTypeSchema _foundBaseType;
        private int _foundBaseTypeIndex = -1;
        private Vector<TriggerRecord> _triggerList;
        private Vector<ValidateCode> _actionList;
        private LoadPass _currentValidationPass;
        private string _previewName;
        private static Map<string, TypeSchema> s_classReservedSymbols = new Map<string, TypeSchema>(1);

        public static void InitializeStatics() => ValidateClass.s_classReservedSymbols["Class"] = (TypeSchema)ClassStateSchema.Type;

        public ValidateClass(
          SourceMarkupLoader owner,
          ValidateTypeIdentifier typeIdentifier,
          int line,
          int offset)
          : base(owner, typeIdentifier, line, offset)
        {
        }

        public void NotifyFoundMethodList(ValidateMethodList methodList)
        {
            if (this._foundMethods != null)
                this.ReportError("Methods may only be specified once");
            else
                this._foundMethods = methodList;
        }

        public override void NotifyParseComplete()
        {
            this._previewName = this.GetInlinePropertyValueNoValidate("Name");
            string propertyValueNoValidate = this.GetInlinePropertyValueNoValidate("Base");
            if (propertyValueNoValidate != null && propertyValueNoValidate.IndexOf(':') != -1)
                this._baseTypeIdentifier = new ValidateTypeIdentifier(this.Owner, propertyValueNoValidate, this.Line, this.Column);
            this.Owner.NotifyClassParseComplete(this._previewName);
        }

        public void Validate(LoadPass currentPass)
        {
            if (this._currentValidationPass >= currentPass)
                return;
            this._currentValidationPass = currentPass;
            if (this._foundBaseType != null && ((MarkupLoadResult)this._foundBaseType.Owner).IsSource)
                ((SourceMarkupLoadResult)this._foundBaseType.Owner).ValidateType(this._foundBaseType, this._currentValidationPass);
            ValidateContext context = new ValidateContext(this, this._foundBaseType, this._currentValidationPass);
            this.Validate(TypeRestriction.None, context);
            int num = this.HasErrors ? 1 : 0;
            if (this._currentValidationPass == LoadPass.DeclareTypes)
                this.DeclareTypes(context);
            else if (this._currentValidationPass == LoadPass.PopulatePublicModel)
            {
                this.PopulatePublicModel(context);
            }
            else
            {
                this.FullValidation(context);
                this.TransferToTypeExport(context);
            }
        }

        private void DeclareTypes(ValidateContext context)
        {
            this.RearrangePropertiesForSymbols();
            this._foundScriptsProperty = this.FindProperty("Scripts", true);
            if (this.PreviewName == null)
                this.ReportError("'Name' property is required and must be provided");
            else if (!ValidateContext.IsValidSymbolName(this.PreviewName))
            {
                this.ReportError("Invalid name \"{0}\".  Valid names must begin with either an alphabetic character or an underscore and can otherwise contain only alphabetic, numeric, or underscore characters", this.PreviewName);
            }
            else
            {
                if (this.Owner.IsTypeNameTaken(this.PreviewName))
                    this.ReportError("Type '{0}' was specified more than once", this.PreviewName);
                this._typeExport = MarkupTypeSchema.Build(this.ObjectType, (MarkupLoadResult)this.Owner.LoadResultTarget, this.PreviewName);
                this._typeExportIndex = this.Owner.RegisterExportedType(this._typeExport);
                this._typeExport.LoadData = (object)this;
            }
        }

        protected virtual void PopulatePublicModel(ValidateContext context)
        {
            if (this._baseTypeIdentifier != null)
            {
                this._baseTypeIdentifier.Validate();
                if (this._baseTypeIdentifier.HasErrors)
                    this.MarkHasErrors();
                else if (!(this._baseTypeIdentifier.FoundType is MarkupTypeSchema foundType))
                {
                    this._baseTypeIdentifier.ReportError("Base type must be a markup-defined type");
                    this.MarkHasErrors();
                }
                else
                {
                    if (this is ValidateUI)
                    {
                        if (!(foundType is UIClassTypeSchema))
                        {
                            this._baseTypeIdentifier.ReportError("Base type must be a markup-defined UI type");
                            this.MarkHasErrors();
                        }
                    }
                    else if (!(foundType is ClassTypeSchema))
                    {
                        this._baseTypeIdentifier.ReportError("Base type must be a markup-defined Class type");
                        this.MarkHasErrors();
                    }
                    if (!this.HasErrors)
                    {
                        bool flag = false;
                        for (TypeSchema typeSchema = (TypeSchema)foundType; typeSchema is MarkupTypeSchema; typeSchema = typeSchema.Base)
                        {
                            if (typeSchema == this._typeExport)
                            {
                                this._baseTypeIdentifier.ReportError("Circular base class dependency involving '{0}' and '{1}'", this._typeExport.Name, foundType.Name);
                                this.MarkHasErrors();
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            this._foundBaseType = foundType;
                            this._foundBaseTypeIndex = this._baseTypeIdentifier.FoundTypeIndex;
                        }
                    }
                }
            }
            ValidateProperty property1 = this.FindProperty("Base", true);
            if (property1 != null)
            {
                if (property1.IsFromStringValue)
                {
                    if (this._baseTypeIdentifier == null)
                    {
                        property1.ReportError("Base must be a type identifier (prefix:Type)");
                        this.MarkHasErrors();
                    }
                }
                else
                {
                    property1.ReportError("Property '{0}' does not support expanded value syntax", property1.PropertyName);
                    this.MarkHasErrors();
                }
            }
            if (this._typeExport != null)
                this._typeExport.SetBaseType(this._foundBaseType);
            bool flag1 = false;
            ValidateProperty property2 = this.FindProperty("Shared", true);
            if (property2 != null)
            {
                property2.Validate((ValidateObjectTag)this, context);
                if (property2.HasErrors)
                    this.MarkHasErrors();
                else if (property2.IsFromStringValue)
                {
                    flag1 = (bool)((ValidateFromString)property2.Value).FromStringInstance;
                    if (flag1 && this is ValidateUI)
                    {
                        flag1 = false;
                        this.ReportError("UI cannot be marked as shared");
                    }
                }
                else
                {
                    property2.ReportError("Property '{0}' does not support expanded value syntax", property2.PropertyName);
                    this.MarkHasErrors();
                }
                if (flag1 && this._typeExport != null)
                    ((ClassTypeSchema)this._typeExport).MarkShareable();
            }
            ValidateProperty property3 = this.FindProperty("Properties");
            if (property3 != null)
            {
                if (property3.Value == null || property3.IsObjectTagValue)
                {
                    this._foundPropertiesValidateProperty = property3;
                    int length1 = 0;
                    for (ValidateObjectTag next = (ValidateObjectTag)property3.Value; next != null; next = next.Next)
                        ++length1;
                    PropertySchema[] properties = new PropertySchema[length1];
                    int length2 = 0;
                    for (ValidateObjectTag next = (ValidateObjectTag)property3.Value; next != null; next = next.Next)
                    {
                        next.Validate(TypeRestriction.None, context);
                        if (next.ObjectType != null)
                        {
                            string propertyValueNoValidate = next.GetInlinePropertyValueNoValidate("Name");
                            if (propertyValueNoValidate == null)
                            {
                                if (!next.HasErrors)
                                {
                                    next.ReportError("'Name' property is required and must be provided");
                                    this.MarkHasErrors();
                                }
                            }
                            else if (this._typeExport != null)
                            {
                                MarkupPropertySchema propertyExport = MarkupPropertySchema.Build(this.ObjectType, this._typeExport, propertyValueNoValidate, next.ObjectType);
                                properties[length2] = (PropertySchema)propertyExport;
                                ++length2;
                                next.PropertySchemaExport = propertyExport;
                                propertyExport.SetRequiredForCreation(next.PropertyIsRequiredForCreation);
                                this.AnnotateProperty(next, context, propertyExport);
                            }
                        }
                    }
                    if (length2 != properties.Length)
                    {
                        PropertySchema[] propertySchemaArray = new PropertySchema[length2];
                        Array.Copy((Array)properties, (Array)propertySchemaArray, length2);
                        properties = propertySchemaArray;
                    }
                    if (this._typeExport != null)
                        this._typeExport.SetPropertyList(properties);
                }
                else
                {
                    property3.ReportError("Property '{0}' values must be in expanded form", property3.PropertyName);
                    this.MarkHasErrors();
                }
            }
            if (this._foundMethods == null)
                return;
            this._foundMethods.Validate(this, context);
            if (this._foundMethods.FoundMethods == null)
                return;
            this._typeExport.SetMethodList(this._foundMethods.FoundMethods);
        }

        protected virtual void AnnotateProperty(
          ValidateObjectTag objectTag,
          ValidateContext context,
          MarkupPropertySchema propertyExport)
        {
        }

        protected virtual void FullValidation(ValidateContext context)
        {
            if (this._foundPropertiesValidateProperty != null)
                this._foundPropertiesValidateProperty.ShouldSkipDictionaryAddIfContains = true;
            if (this._foundBaseType is ClassTypeSchema foundBaseType && foundBaseType.IsShared)
                this.ReportError("Base type cannot be a shared Class");
            ValidateProperty property = this.FindProperty("Locals");
            if (property != null)
            {
                if (property.Value == null || property.IsObjectTagValue)
                {
                    for (ValidateObjectTag next = (ValidateObjectTag)property.Value; next != null; next = next.Next)
                    {
                        if (next.Name == null && !next.HasErrors)
                        {
                            next.ReportError("'Name' property is required and must be provided");
                            this.MarkHasErrors();
                        }
                    }
                }
                else
                {
                    property.ReportError("Property '{0}' values must be in expanded form", property.PropertyName);
                    this.MarkHasErrors();
                }
            }
            if (this._foundMethods != null)
                this._foundMethods.Validate(this, context);
            if (this._foundScriptsProperty != null)
            {
                this._scripts = new ValidateScripts(this._foundScriptsProperty);
                this._scripts.Validate(context);
                if (this._scripts.HasErrors)
                    this.MarkHasErrors();
            }
            if (this._foundVirtualMethods == null)
                return;
            MethodSchema[] virtualMethods = new MethodSchema[this._foundVirtualMethods.Count];
            for (int index = 0; index < virtualMethods.Length; ++index)
                virtualMethods[index] = (MethodSchema)this._foundVirtualMethods[index];
            this._typeExport.SetVirtualMethodList(virtualMethods);
        }

        private void TransferToTypeExport(ValidateContext context)
        {
            if (this._typeExport != null)
            {
                this._typeExport.SetSymbolReferenceTable(context.SymbolReferenceTable);
                this._typeExport.SetInheritableSymbolsTable(context.InheritableSymbolsTable);
                this._typeExport.SetListenerCount(context.NotifierCount);
            }
            this._triggerList = context.TriggerList;
            this._actionList = context.ActionList;
            Map map = new Map();
            for (MarkupTypeSchema typeExport = this.TypeExport; typeExport != null; typeExport = typeExport.Base as MarkupTypeSchema)
            {
                if (typeExport.InheritableSymbolsTable != null)
                {
                    foreach (SymbolRecord symbolRecord in typeExport.InheritableSymbolsTable)
                    {
                        if (symbolRecord.SymbolOrigin == SymbolOrigin.Locals || symbolRecord.SymbolOrigin == SymbolOrigin.Properties)
                            map[(object)symbolRecord.Name] = (object)null;
                    }
                }
            }
            this._typeExport.SetTotalPropertiesAndLocalsCount(map.Count);
        }

        public virtual void RearrangePropertiesForSymbols()
        {
            this.MovePropertyToFront("Locals");
            this.MovePropertyToFront("Properties");
        }

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            if (context.CurrentPass == LoadPass.Full)
                context.DeclareReservedSymbols(ValidateClass.s_classReservedSymbols);
            base.Validate(typeRestriction, context);
        }

        public virtual void NotifyDiscoveredObjectTag(ValidateObjectTag objectTag)
        {
        }

        public virtual void NotifyFoundPropertyAssignment(ValidateExpressionCall call)
        {
        }

        public virtual void NotifyFoundMethodCall(ValidateExpressionCall call)
        {
        }

        public void AddVirtualMethod(
          MarkupMethodSchema methodSchema,
          MarkupMethodSchema baseVirtualMethodSchema)
        {
            if (this._foundVirtualMethods == null)
            {
                this._foundVirtualMethods = new Vector<MarkupMethodSchema>();
                this._foundVirtualBaseMethods = new Vector<MarkupMethodSchema>();
            }
            this._foundVirtualMethods.Add(methodSchema);
            this._foundVirtualBaseMethods.Add(baseVirtualMethodSchema);
        }

        protected override bool ForceAbstractAsConcrete => true;

        public ValidateScripts Scripts => this._scripts;

        internal ValidateProperty FoundScripts => this._foundScriptsProperty;

        public MarkupTypeSchema TypeExport => this._typeExport;

        public int TypeExportIndex => this._typeExportIndex;

        public Vector<TriggerRecord> TriggerList => this._triggerList;

        public Vector<ValidateCode> ActionList => this._actionList;

        public ArrayList MethodList => this._foundMethods == null ? (ArrayList)null : this._foundMethods.Methods;

        public string PreviewName => this._previewName;

        public MarkupTypeSchema FoundBaseType => this._foundBaseType;

        public int FoundBaseTypeIndex => this._foundBaseTypeIndex;

        public ValidateProperty FoundPropertiesValidateProperty => this._foundPropertiesValidateProperty;

        public override string ToString() => this.Name != null ? string.Format("ClassTag : {0}", (object)this.Name) : "Not validated :" + this._previewName;
    }
}
