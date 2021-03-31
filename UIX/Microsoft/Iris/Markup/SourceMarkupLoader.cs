// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.SourceMarkupLoader
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.Markup.Validation;
using Microsoft.Iris.Session;
using System.Collections.Generic;

namespace Microsoft.Iris.Markup
{
    internal class SourceMarkupLoader
    {
        private const string MePrefix = "me";
        public const string MeNamespace = "Me";
        private ParseResult _parseResult;
        private SourceMarkupLoadResult _loadResultTarget;
        private Vector _validateObjects = new Vector();
        private Vector _foundExportedTypes = new Vector();
        private Vector _foundAliasMappings;
        private bool _hasErrors;
        public Dictionary<object, object> _importedNamespaces = new Dictionary<object, object>();
        private bool _usingSharedBinaryDataTable;
        private SourceMarkupImportTables _importTables;
        private Dictionary<string, object> _referencedNamespaces = new Dictionary<string, object>();
        private LoadPass _currentValidationPass;

        public static SourceMarkupLoader Load(
          SourceMarkupLoadResult loadResult,
          Resource resource)
        {
            SourceMarkupLoader owner = new SourceMarkupLoader(loadResult);
            ParseResult parseResult = Parser.Invoke(owner, resource);
            if (parseResult.HasErrors)
                owner.MarkHasErrors();
            else
                owner.SetParseResult(parseResult);
            return owner;
        }

        protected SourceMarkupLoader(SourceMarkupLoadResult loadResultTarget) => this._loadResultTarget = loadResultTarget;

        public SourceMarkupLoadResult LoadResultTarget => this._loadResultTarget;

        public void SetParseResult(ParseResult parseResult) => this._parseResult = parseResult;

        public LoadResult FindDependency(string prefix)
        {
            if (prefix == null)
                return MarkupSystem.UIXGlobal;
            object obj;
            this._importedNamespaces.TryGetValue(prefix, out obj);
            return (LoadResult)obj;
        }

        public void Validate(LoadPass currentPass)
        {
            if (this._currentValidationPass >= currentPass)
                return;
            this._currentValidationPass = currentPass;
            if (this._currentValidationPass == LoadPass.DeclareTypes)
            {
                if (this._parseResult == null)
                    return;
                if (this._parseResult.Root != "UIX")
                {
                    this.ReportError("Unknown markup format. Expecting <UIX> ... </UIX> root markup tags", -1, -1);
                    return;
                }
                if (this._parseResult.Version != "http://schemas.microsoft.com/2007/uix")
                {
                    this.ReportError(string.Format("Unsupported version of markup: '{0}'", _parseResult.Version), -1, -1);
                    return;
                }
                if (this._loadResultTarget.BinaryDataTable != null)
                {
                    this._importTables = this._loadResultTarget.BinaryDataTable.SourceMarkupImportTables;
                    this._usingSharedBinaryDataTable = true;
                }
                else
                    this._importTables = new SourceMarkupImportTables();
                for (ValidateNamespace validateNamespace = this._parseResult.XmlnsList; validateNamespace != null; validateNamespace = validateNamespace.Next)
                {
                    LoadResult loadResult = validateNamespace.Validate();
                    if (loadResult != null)
                    {
                        this._importedNamespaces[validateNamespace.Prefix] = loadResult;
                        this.TrackImportedLoadResult(loadResult);
                    }
                }
            }
            foreach (LoadResult loadResult in this._importedNamespaces.Values)
            {
                if (loadResult != this._loadResultTarget && loadResult != MarkupSystem.UIXGlobal)
                {
                    loadResult.Load(this._currentValidationPass);
                    if (loadResult.Status == LoadResultStatus.Error)
                        this.MarkHasErrors();
                }
            }
            if (this._parseResult != null && currentPass != LoadPass.Done)
            {
                foreach (ValidateClass validateClass in this._parseResult.ClassList)
                    validateClass.Validate(this._currentValidationPass);
                foreach (ValidateDataMapping dataMapping in this._parseResult.DataMappingList)
                    dataMapping.Validate(this._currentValidationPass);
                foreach (ValidateAlias alias in this._parseResult.AliasList)
                    alias.Validate(this._currentValidationPass);
                if (this._currentValidationPass == LoadPass.Full)
                {
                    for (ValidateNamespace validateNamespace = this._parseResult.XmlnsList; validateNamespace != null; validateNamespace = validateNamespace.Next)
                    {
                        if (!this._referencedNamespaces.ContainsKey(validateNamespace.Prefix))
                            ErrorManager.ReportWarning(validateNamespace.Line, validateNamespace.Column, "Unreferenced namespace {0}", validateNamespace.Prefix);
                    }
                }
            }
            this.CompleteValidationPass();
        }

        public void CompleteValidationPass()
        {
            if (this._currentValidationPass == LoadPass.DeclareTypes)
            {
                this._loadResultTarget.SetExportTable(this.PrepareExportTable());
                this._loadResultTarget.SetAliasTable(this.PrepareAliasTable());
            }
            else if (this._currentValidationPass == LoadPass.PopulatePublicModel)
            {
                if (this._parseResult == null)
                    return;
                foreach (ValidateClass validateClass in this._parseResult.ClassList)
                {
                    if (validateClass.TypeExport != null)
                        validateClass.TypeExport.BuildProperties();
                }
            }
            else
            {
                if (this._currentValidationPass != LoadPass.Full)
                    return;
                if (this.HasErrors)
                    this._loadResultTarget.MarkLoadFailed();
                MarkupImportTables importTables = null;
                if (this._importTables != null)
                {
                    importTables = this._importTables.PrepareImportTables();
                    this._loadResultTarget.SetImportTables(importTables);
                }
                MarkupLineNumberTable lineNumberTable = new MarkupLineNumberTable();
                MarkupConstantsTable constantsTable = this._loadResultTarget.BinaryDataTable == null ? new MarkupConstantsTable() : this._loadResultTarget.BinaryDataTable.ConstantsTable;
                this._loadResultTarget.SetDataMappingsTable(this.PrepareDataMappingTable());
                this._loadResultTarget.ValidationComplete();
                ByteCodeReader reader = null;
                if (!this.HasErrors)
                    reader = new MarkupEncoder(importTables, constantsTable, lineNumberTable).EncodeOBJECTSection(this._parseResult, this._loadResultTarget.Uri, null);
                if (!this._usingSharedBinaryDataTable)
                {
                    constantsTable.PrepareForRuntimeUse();
                    this._loadResultTarget.SetConstantsTable(constantsTable);
                }
                lineNumberTable.PrepareForRuntimeUse();
                this._loadResultTarget.SetLineNumberTable(lineNumberTable);
                if (reader != null)
                    this._loadResultTarget.SetObjectSection(reader);
                this._loadResultTarget.SetDependenciesTable(this.PrepareDependenciesTable());
                if (!MarkupSystem.TrackAdditionalMetadata)
                    this._parseResult = null;
                foreach (DisposableObject validateObject in this._validateObjects)
                    validateObject.Dispose(this);
            }
        }

        public int RegisterExportedType(MarkupTypeSchema type)
        {
            int count = this._foundExportedTypes.Count;
            this._foundExportedTypes.Add(type);
            return count;
        }

        public int RegisterAlias(string alias, LoadResult loadResult, string targetType)
        {
            this.TrackImportedLoadResult(loadResult);
            if (this._foundAliasMappings == null)
                this._foundAliasMappings = new Vector();
            int count = this._foundAliasMappings.Count;
            this._foundAliasMappings.Add(new AliasMapping(alias, loadResult, targetType));
            return count;
        }

        public bool IsTypeNameTaken(string typeName)
        {
            foreach (TypeSchema foundExportedType in this._foundExportedTypes)
            {
                if (foundExportedType.Name == typeName)
                    return true;
            }
            if (this._foundAliasMappings != null)
            {
                foreach (AliasMapping foundAliasMapping in this._foundAliasMappings)
                {
                    if (foundAliasMapping.Alias == typeName)
                        return true;
                }
            }
            return false;
        }

        public void TrackValidateObject(Microsoft.Iris.Markup.Validation.Validate validate)
        {
            this._validateObjects.Add(validate);
            validate.DeclareOwner(this);
        }

        public void TrackImportedLoadResult(LoadResult loadResult)
        {
            if (loadResult == MarkupSystem.UIXGlobal)
                return;
            for (int index = 0; index < this._importTables.ImportedLoadResults.Count; ++index)
            {
                if ((LoadResult)this._importTables.ImportedLoadResults[index] == loadResult)
                    return;
            }
            this._importTables.ImportedLoadResults.Add(loadResult);
        }

        public int TrackImportedType(TypeSchema type)
        {
            this.TrackImportedLoadResult(type.Owner);
            return this.TrackImportedSchema(this._importTables.ImportedTypes, type);
        }

        public int TrackImportedConstructor(ConstructorSchema constructor)
        {
            int num = this.TrackImportedSchema(this._importTables.ImportedConstructors, constructor);
            this.TrackImportedType(constructor.Owner);
            foreach (TypeSchema parameterType in constructor.ParameterTypes)
                this.TrackImportedType(parameterType);
            return num;
        }

        public int TrackImportedProperty(PropertySchema property)
        {
            int num = this.TrackImportedSchema(this._importTables.ImportedProperties, property);
            this.TrackImportedType(property.Owner);
            return num;
        }

        public int TrackImportedMethod(MethodSchema method)
        {
            int num = this.TrackImportedSchema(this._importTables.ImportedMethods, method);
            this.TrackImportedType(method.Owner);
            foreach (TypeSchema parameterType in method.ParameterTypes)
                this.TrackImportedType(parameterType);
            return num;
        }

        public int TrackImportedEvent(EventSchema evt)
        {
            int num = this.TrackImportedSchema(this._importTables.ImportedEvents, evt);
            this.TrackImportedType(evt.Owner);
            return num;
        }

        public int TrackImportedSchema(Vector importList, object schema)
        {
            for (int index = 0; index < importList.Count; ++index)
            {
                if (importList[index] == schema)
                    return index;
            }
            int count = importList.Count;
            importList.Add(schema);
            return count;
        }

        public void ReportError(string error, int line, int column)
        {
            this.MarkHasErrors();
            ErrorManager.ReportError(line, column, error);
        }

        public void MarkHasErrors()
        {
            if (this._hasErrors)
                return;
            this._hasErrors = true;
            this._loadResultTarget.MarkLoadFailed();
        }

        public LoadResult[] PrepareDependenciesTable()
        {
            LoadResult[] loadResultArray = LoadResult.EmptyList;
            int length = 0;
            if (this._importTables != null)
            {
                foreach (LoadResult importedLoadResult in this._importTables.ImportedLoadResults)
                {
                    if (importedLoadResult != this._loadResultTarget)
                        ++length;
                }
            }
            if (length != 0)
            {
                loadResultArray = new LoadResult[length];
                int index = 0;
                foreach (LoadResult importedLoadResult in this._importTables.ImportedLoadResults)
                {
                    if (importedLoadResult != this._loadResultTarget)
                    {
                        loadResultArray[index] = importedLoadResult;
                        ++index;
                    }
                }
            }
            return loadResultArray;
        }

        public TypeSchema[] PrepareExportTable()
        {
            TypeSchema[] typeSchemaArray = TypeSchema.EmptyList;
            if (this._foundExportedTypes.Count > 0)
            {
                typeSchemaArray = new TypeSchema[this._foundExportedTypes.Count];
                for (int index = 0; index < this._foundExportedTypes.Count; ++index)
                {
                    MarkupTypeSchema foundExportedType = (MarkupTypeSchema)this._foundExportedTypes[index];
                    typeSchemaArray[index] = foundExportedType;
                }
            }
            return typeSchemaArray;
        }

        private AliasMapping[] PrepareAliasTable()
        {
            AliasMapping[] aliasMappingArray = null;
            if (this._foundAliasMappings != null)
            {
                aliasMappingArray = new AliasMapping[this._foundAliasMappings.Count];
                for (int index = 0; index < this._foundAliasMappings.Count; ++index)
                    aliasMappingArray[index] = (AliasMapping)this._foundAliasMappings[index];
            }
            return aliasMappingArray;
        }

        private MarkupDataMapping[] PrepareDataMappingTable()
        {
            MarkupDataMapping[] dataMappingTable = null;
            int length = this.PrepareDataMappingTableHelper(null);
            if (length > 0)
            {
                dataMappingTable = new MarkupDataMapping[length];
                this.PrepareDataMappingTableHelper(dataMappingTable);
            }
            return dataMappingTable;
        }

        private int PrepareDataMappingTableHelper(MarkupDataMapping[] dataMappingTable)
        {
            int dataMappingCount = 0;
            if (this._parseResult != null)
            {
                foreach (ValidateClass validateClass in this._parseResult.ClassList)
                {
                    if (validateClass is ValidateDataType validateDataType)
                        this.AddDataMappingsHelper(validateDataType.FoundDataMappingSet, dataMappingTable, ref dataMappingCount);
                }
                foreach (ValidateDataMapping dataMapping in this._parseResult.DataMappingList)
                    this.AddDataMappingsHelper(dataMapping.FoundDataMappingSet, dataMappingTable, ref dataMappingCount);
            }
            return dataMappingCount;
        }

        private void AddDataMappingsHelper(
          Vector<MarkupDataMapping> dataMappingSet,
          MarkupDataMapping[] dataMappingTable,
          ref int dataMappingCount)
        {
            if (dataMappingSet == null)
                return;
            foreach (MarkupDataMapping dataMapping in dataMappingSet)
            {
                if (dataMappingTable != null)
                    dataMappingTable[dataMappingCount] = dataMapping;
                ++dataMappingCount;
            }
        }

        public void NotifyTypeIdentifierFound(string prefix, string type)
        {
            if (prefix == null)
                return;
            this._referencedNamespaces[prefix] = null;
        }

        public ValidateObjectTag CreateObjectTagValidator(
          ValidateTypeIdentifier typeIdentifier,
          int line,
          int offset,
          bool isRootTag)
        {
            ValidateObjectTag validateObjectTag = null;
            if (isRootTag)
            {
                if (typeIdentifier.TypeName == ClassSchema.Type.Name)
                    validateObjectTag = new ValidateClass(this, typeIdentifier, line, offset);
                else if (typeIdentifier.TypeName == UISchema.Type.Name)
                    validateObjectTag = new ValidateUI(this, typeIdentifier, line, offset);
                else if (typeIdentifier.TypeName == EffectSchema.Type.Name)
                    validateObjectTag = new ValidateEffect(this, typeIdentifier, line, offset);
                else if (typeIdentifier.TypeName == AliasSchema.Type.Name)
                    validateObjectTag = new ValidateAlias(this, typeIdentifier, line, offset);
                else if (typeIdentifier.TypeName == DataTypeSchema.Type.Name)
                    validateObjectTag = new ValidateDataType(this, typeIdentifier, line, offset);
                else if (typeIdentifier.TypeName == DataQuerySchema.Type.Name)
                    validateObjectTag = new ValidateDataQuery(this, typeIdentifier, line, offset);
                else if (typeIdentifier.TypeName == DataMappingSchema.Type.Name)
                    validateObjectTag = new ValidateDataMapping(this, typeIdentifier, line, offset);
            }
            else if (typeIdentifier.Prefix == null)
            {
                if (typeIdentifier.TypeName == TypeConstraintSchema.Type.Name)
                {
                    validateObjectTag = new ValidateTypeConstraint(this, typeIdentifier, line, offset);
                }
                else
                {
                    TypeSchema type = MarkupSystem.UIXGlobal.FindType(typeIdentifier.TypeName);
                    if (EffectElementSchema.Type.IsAssignableFrom(type))
                        validateObjectTag = new ValidateEffectElement(this, typeIdentifier, line, offset);
                }
            }
            if (validateObjectTag == null)
                validateObjectTag = new ValidateObjectTag(this, typeIdentifier, line, offset);
            return validateObjectTag;
        }

        public void NotifyClassParseComplete(string className)
        {
        }

        public bool HasErrors => this._hasErrors;

        internal ParseResult ParseResult => this._parseResult;
    }
}
