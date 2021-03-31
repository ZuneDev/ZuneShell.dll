// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.CompiledMarkupLoader
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.OS;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.Markup
{
    internal class CompiledMarkupLoader
    {
        private const uint c_LineNumberRecordSize = 12;
        private CompiledMarkupLoadResult _loadResultTarget;
        private ByteCodeReader _reader;
        private bool _hasErrors;
        private LoadPass _currentDepersistPass;
        private MarkupLoadResult _binaryDataTableLoadResult;
        private MarkupBinaryDataTable _binaryDataTable;
        private bool _usingSharedDataTable;
        private uint _objectSectionStart;
        private uint _objectSectionEnd;
        private uint _lineNumberTableStart;
        private uint _lineNumberTableEnd;
        private TypeSchema[][] _typeSchemaArrays;

        public static CompiledMarkupLoader Load(
          CompiledMarkupLoadResult loadResult,
          Resource resource)
        {
            return new CompiledMarkupLoader(loadResult, resource);
        }

        private CompiledMarkupLoader(CompiledMarkupLoadResult loadResultTarget, Resource resource)
        {
            this._loadResultTarget = loadResultTarget;
            this._reader = new ByteCodeReader(resource.Buffer, resource.Length, false);
            if (resource is DllResource)
                this._reader.MarkAsInFixedMemory();
            uint num1 = this._reader.ReadUInt32();
            uint num2 = this._reader.ReadUInt32();
            if (num1 != 440551765U)
                this.ReportError("Invalid compiled UIX file");
            if (num2 == 1012U)
                return;
            this.ReportError("Compiled UIX file '{0}' was compiled for the runtime with version {1}, but the current runtime is version {2}", resource.Uri, num2.ToString(), 1012U.ToString());
        }

        public void Depersist(LoadPass currentPass)
        {
            if (this._currentDepersistPass >= currentPass)
                return;
            this._currentDepersistPass = currentPass;
            if (this._binaryDataTableLoadResult != null)
                this._binaryDataTableLoadResult.Load(this._currentDepersistPass);
            if (this._currentDepersistPass == LoadPass.DeclareTypes)
                this.DoLoadPassDeclareTypes();
            else if (this._currentDepersistPass == LoadPass.PopulatePublicModel)
                this.DoLoadPassPopulatePublicModel();
            else if (this._currentDepersistPass == LoadPass.Full)
            {
                this.DoLoadPassFull();
            }
            else
            {
                foreach (MarkupTypeSchema markupTypeSchema in this._loadResultTarget.ExportTable)
                    markupTypeSchema.Seal();
                this._reader = null;
            }
            foreach (LoadResult dependency in this._loadResultTarget.Dependencies)
            {
                if (dependency != this._loadResultTarget)
                {
                    dependency.Load(this._currentDepersistPass);
                    if (dependency.Status == LoadResultStatus.Error)
                        this.MarkHasErrors();
                }
            }
        }

        private void DoLoadPassDeclareTypes()
        {
            if (this.HasErrors)
                return;
            this.DepersistTableOfContents();
            if (this.HasErrors)
                return;
            this.DepersistDependencies();
            if (this.HasErrors)
                return;
            this.DepersistTypeExportDeclarations();
            if (!this.HasErrors)
                ;
        }

        private void DoLoadPassPopulatePublicModel()
        {
            if (this.HasErrors)
                return;
            this.DepersistTypeImportTable();
            if (this.HasErrors)
                return;
            this.DepersistTypeExportDefinitions();
            if (!this.HasErrors)
                ;
        }

        private void DoLoadPassFull()
        {
            if (this.HasErrors)
                return;
            this.DepersistMemberImportTables();
            if (this.HasErrors)
                return;
            this.DepersistDataMappingsTable();
            if (this.HasErrors)
                return;
            this.DepersistConstantsTable();
            if (this.HasErrors)
                return;
            this.DepersistLineNumberTable();
            if (this.HasErrors)
                return;
            this.DepersistObjectSection();
            if (!this.HasErrors)
                ;
        }

        private void DepersistTableOfContents()
        {
            this._objectSectionStart = this._reader.ReadUInt32();
            this._objectSectionEnd = this._reader.ReadUInt32();
            this._lineNumberTableStart = this._reader.ReadUInt32();
            this._lineNumberTableEnd = this._reader.ReadUInt32();
            string uri = this._reader.ReadString();
            if (uri != null)
            {
                this._binaryDataTableLoadResult = MarkupSystem.ResolveLoadResult(uri, this._loadResultTarget.IslandReferences) as MarkupLoadResult;
                if (this._binaryDataTableLoadResult != null)
                {
                    this._binaryDataTableLoadResult.Load(this._currentDepersistPass);
                    this._binaryDataTable = this._binaryDataTableLoadResult.BinaryDataTable;
                    this._loadResultTarget.SetBinaryDataTable(this._binaryDataTable);
                    if (this._binaryDataTable.SharedDependenciesTableWithBinaryDataTable == null)
                        this._binaryDataTable.SharedDependenciesTableWithBinaryDataTable = new LoadResult[1]
                        {
               _binaryDataTableLoadResult
                        };
                    this._loadResultTarget.SetDependenciesTable(this._binaryDataTable.SharedDependenciesTableWithBinaryDataTable);
                    this._usingSharedDataTable = true;
                }
                else
                    this.MarkHasErrors();
            }
            else
                this.DepersistBinaryDataTable(this._reader.ReadUInt32());
        }

        private void DepersistDependencies()
        {
            if (this._usingSharedDataTable)
                return;
            ushort num = this._reader.ReadUInt16();
            LoadResult[] dependenciesTable = new LoadResult[num];
            for (ushort index = 0; index < num; ++index)
            {
                this._reader.ReadBool();
                string uri = this.ReadDataTableString();
                LoadResult loadResult = MarkupSystem.ResolveLoadResult(uri, this._loadResultTarget.IslandReferences);
                if (loadResult == null || loadResult.Status == LoadResultStatus.Error)
                {
                    this.ReportError("Import of '{0}' failed", uri);
                    return;
                }
                dependenciesTable[index] = loadResult;
            }
            this._loadResultTarget.SetDependenciesTable(dependenciesTable);
        }

        private void DepersistTypeExportDeclarations()
        {
            ushort num1 = this._reader.ReadUInt16();
            TypeSchema[] exportTable = new TypeSchema[num1];
            for (int index = 0; index < num1; ++index)
            {
                string name = this.ReadDataTableString();
                MarkupTypeSchema markupTypeSchema = MarkupTypeSchema.Build(this.MarkupTypeToDefinition((MarkupType)this._reader.ReadInt32()), _loadResultTarget, name);
                exportTable[index] = markupTypeSchema;
            }
            this._loadResultTarget.SetExportTable(exportTable);
            ushort num2 = this._reader.ReadUInt16();
            if (num2 <= 0)
                return;
            AliasMapping[] aliasTable = new AliasMapping[num2];
            for (int index1 = 0; index1 < num2; ++index1)
            {
                string alias = this.ReadDataTableString();
                ushort index2 = this._reader.ReadUInt16();
                string targetType = this.ReadDataTableString();
                LoadResult dependent = this.MapIndexToDependent(index2);
                aliasTable[index1] = new AliasMapping(alias, dependent, targetType);
            }
            this._loadResultTarget.SetAliasTable(aliasTable);
        }

        private void DepersistTypeImportTable()
        {
            if (this._usingSharedDataTable)
                return;
            MarkupImportTables importTables = new MarkupImportTables();
            this._loadResultTarget.BinaryDataTable.SetImportTables(importTables);
            ushort num = this._reader.ReadUInt16();
            if (num <= 0)
                return;
            TypeSchema[] typeSchemaArray = new TypeSchema[num];
            for (ushort index = 0; index < num; ++index)
            {
                LoadResult dependent = this.MapIndexToDependent(this._reader.ReadUInt16());
                string name = this.ReadDataTableString();
                TypeSchema type = dependent.FindType(name);
                if (type == null)
                    this.ReportError("Import of {0} named '{1}' from '{2}' failed", "type", name, dependent.Uri);
                else
                    typeSchemaArray[index] = type;
            }
            importTables.TypeImports = typeSchemaArray;
        }

        private void DepersistTypeExportDefinitions()
        {
            TypeSchema[] typeImports = this._loadResultTarget.ImportTables.TypeImports;
            foreach (MarkupTypeSchema markupTypeSchema in this._loadResultTarget.ExportTable)
            {
                MarkupType markupType = markupTypeSchema.MarkupType;
                TypeSchema definition = this.MarkupTypeToDefinition(markupType);
                uint typeDepth = this._reader.ReadUInt16();
                markupTypeSchema.SetTypeDepth(typeDepth);
                if (typeDepth > 1U)
                {
                    ushort num = this._reader.ReadUInt16();
                    TypeSchema typeSchema = typeImports[num];
                    markupTypeSchema.SetBaseType((MarkupTypeSchema)typeSchema);
                }
                uint offset1 = this._reader.ReadUInt32();
                markupTypeSchema.SetInitializePropertiesOffset(offset1);
                uint offset2 = this._reader.ReadUInt32();
                markupTypeSchema.SetInitializeLocalsInputOffset(offset2);
                uint offset3 = this._reader.ReadUInt32();
                markupTypeSchema.SetInitializeContentOffset(offset3);
                markupTypeSchema.SetInitialEvaluateOffsets(this.ReadUInt32ArrayHelper());
                markupTypeSchema.SetFinalEvaluateOffsets(this.ReadUInt32ArrayHelper());
                markupTypeSchema.SetRefreshListenerGroupOffsets(this.ReadUInt32ArrayHelper());
                uint listenerCount = this._reader.ReadUInt32();
                markupTypeSchema.SetListenerCount(listenerCount);
                uint num1 = this._reader.ReadUInt32();
                if (num1 > 0U)
                {
                    SymbolReference[] symbolTable = new SymbolReference[num1];
                    for (int index = 0; index < num1; ++index)
                    {
                        SymbolReference symbolReference = new SymbolReference(this.ReadDataTableString(), (SymbolOrigin)this._reader.ReadByte());
                        symbolTable[index] = symbolReference;
                    }
                    markupTypeSchema.SetSymbolReferenceTable(symbolTable);
                }
                this.DepersistInheritedSymbolTable(markupTypeSchema);
                int totalPropertiesAndLocalsCount = this._reader.ReadInt32();
                markupTypeSchema.SetTotalPropertiesAndLocalsCount(totalPropertiesAndLocalsCount);
                if (markupType == MarkupType.UI)
                {
                    ushort num2 = this._reader.ReadUInt16();
                    if (num2 > 0)
                    {
                        NamedContentRecord[] namedContentTable = new NamedContentRecord[num2];
                        for (int index = 0; index < num2; ++index)
                        {
                            string name = this.ReadDataTableString();
                            uint offset4 = this._reader.ReadUInt32();
                            namedContentTable[index] = new NamedContentRecord(name);
                            namedContentTable[index].SetOffset(offset4);
                        }
                      ((UIClassTypeSchema)markupTypeSchema).SetNamedContentTable(namedContentTable);
                    }
                }
                else
                {
                    if (this._reader.ReadBool())
                        ((ClassTypeSchema)markupTypeSchema).MarkShareable();
                    if (markupType == MarkupType.Effect)
                    {
                        EffectClassTypeSchema effectClassTypeSchema = (EffectClassTypeSchema)markupTypeSchema;
                        effectClassTypeSchema.SetTechniqueOffsets(this.ReadUInt32ArrayHelper());
                        effectClassTypeSchema.SetInstancePropertyAssignments(this.ReadUInt32ArrayHelper());
                        effectClassTypeSchema.SetDynamicElementAssignments(this.ReadStringArrayHelper());
                        int symbolIndex = this._reader.ReadInt32();
                        effectClassTypeSchema.SetDefaultElementSymbolIndex(symbolIndex);
                    }
                    if (markupType == MarkupType.DataQuery)
                    {
                        MarkupDataQuerySchema markupDataQuerySchema = (MarkupDataQuerySchema)markupTypeSchema;
                        markupDataQuerySchema.ProviderName = this.ReadDataTableString();
                        ushort index = this._reader.ReadUInt16();
                        markupDataQuerySchema.ResultType = this.MapIndexToType(index);
                    }
                }
                ushort num3 = this._reader.ReadUInt16();
                if (num3 > 0)
                {
                    PropertySchema[] properties = new PropertySchema[num3];
                    for (int index1 = 0; index1 < num3; ++index1)
                    {
                        string name = this.ReadDataTableString();
                        bool requiredForCreation = this._reader.ReadBool();
                        bool flag = this._reader.ReadBool();
                        PropertyOverrideCriteriaTypeConstraint criteriaTypeConstraint = null;
                        if (flag)
                        {
                            ushort num2 = this._reader.ReadUInt16();
                            ushort num4 = this._reader.ReadUInt16();
                            TypeSchema constraint = typeImports[num2];
                            criteriaTypeConstraint = new PropertyOverrideCriteriaTypeConstraint(typeImports[num4], constraint);
                        }
                        ushort num5 = this._reader.ReadUInt16();
                        TypeSchema propertyType = typeImports[num5];
                        MarkupPropertySchema markupPropertySchema = MarkupPropertySchema.Build(definition, markupTypeSchema, name, propertyType);
                        markupPropertySchema.SetRequiredForCreation(requiredForCreation);
                        markupPropertySchema.SetOverrideCriteria(criteriaTypeConstraint);
                        if (markupType == MarkupType.DataType)
                        {
                            MarkupDataTypePropertySchema typePropertySchema = (MarkupDataTypePropertySchema)markupPropertySchema;
                            ushort index2 = this._reader.ReadUInt16();
                            if (index2 != ushort.MaxValue)
                                typePropertySchema.SetUnderlyingCollectionType(this.MapIndexToType(index2));
                        }
                        if (markupType == MarkupType.DataQuery)
                        {
                            MarkupDataQueryPropertySchema queryPropertySchema = (MarkupDataQueryPropertySchema)markupPropertySchema;
                            if (this._reader.ReadBool())
                                queryPropertySchema.DefaultValue = queryPropertySchema.PropertyType.DecodeBinary(this._reader);
                            queryPropertySchema.InvalidatesQuery = this._reader.ReadBool();
                            ushort index2 = this._reader.ReadUInt16();
                            if (index2 != ushort.MaxValue)
                                queryPropertySchema.SetUnderlyingCollectionType(this.MapIndexToType(index2));
                        }
                        properties[index1] = markupPropertySchema;
                    }
                    markupTypeSchema.SetPropertyList(properties);
                }
                MethodSchema[] methods = this.ReadMarkupMethodArrayHelper(definition, markupTypeSchema);
                if (methods != null)
                    markupTypeSchema.SetMethodList(methods);
                MethodSchema[] virtualMethods = this.ReadMarkupMethodArrayHelper(definition, markupTypeSchema);
                if (virtualMethods != null)
                    markupTypeSchema.SetVirtualMethodList(virtualMethods);
            }
            foreach (MarkupTypeSchema markupTypeSchema in this._loadResultTarget.ExportTable)
                markupTypeSchema.BuildProperties();
        }

        private void DepersistMemberImportTables()
        {
            if (this._usingSharedDataTable)
                return;
            MarkupImportTables importTables = this._loadResultTarget.ImportTables;
            ushort num1 = this._reader.ReadUInt16();
            if (num1 > 0)
            {
                ConstructorSchema[] constructorSchemaArray = new ConstructorSchema[num1];
                for (int index1 = 0; index1 < num1; ++index1)
                {
                    TypeSchema type = this.MapIndexToType(this._reader.ReadUInt16());
                    ushort num2 = this._reader.ReadUInt16();
                    TypeSchema[] parameters = TypeSchema.EmptyList;
                    if (num2 > 0)
                    {
                        parameters = this.GetTempParameterArray(num2);
                        for (ushort index2 = 0; index2 < num2; ++index2)
                        {
                            ushort index3 = this._reader.ReadUInt16();
                            parameters[index2] = this.MapIndexToType(index3);
                        }
                    }
                    ConstructorSchema constructor = type.FindConstructor(parameters);
                    if (constructor == null)
                        this.ReportError("Import of {0} named '{1}' from '{2}' failed", "constructor", type.Name, type.Owner.Uri);
                    else
                        constructorSchemaArray[index1] = constructor;
                }
                importTables.ConstructorImports = constructorSchemaArray;
            }
            ushort num3 = this._reader.ReadUInt16();
            if (num3 > 0)
            {
                PropertySchema[] propertySchemaArray = new PropertySchema[num3];
                for (int index = 0; index < num3; ++index)
                {
                    TypeSchema type = this.MapIndexToType(this._reader.ReadUInt16());
                    string name = this.ReadDataTableString();
                    PropertySchema property = type.FindProperty(name);
                    if (property == null)
                        this.ReportError("Import of {0} named '{1}' from '{2}' failed", "property", name, type.Name);
                    else
                        propertySchemaArray[index] = property;
                }
                importTables.PropertyImports = propertySchemaArray;
            }
            ushort num4 = this._reader.ReadUInt16();
            if (num4 > 0)
            {
                MethodSchema[] methodSchemaArray = new MethodSchema[num4];
                for (int index1 = 0; index1 < num4; ++index1)
                {
                    MethodSchema methodSchema = null;
                    TypeSchema type = this.MapIndexToType(this._reader.ReadUInt16());
                    if (!this._reader.ReadBool())
                    {
                        string name = this.ReadDataTableString();
                        ushort num2 = this._reader.ReadUInt16();
                        TypeSchema[] parameters = TypeSchema.EmptyList;
                        if (num2 > 0)
                        {
                            parameters = this.GetTempParameterArray(num2);
                            for (ushort index2 = 0; index2 < num2; ++index2)
                            {
                                ushort index3 = this._reader.ReadUInt16();
                                parameters[index2] = this.MapIndexToType(index3);
                            }
                        }
                        methodSchema = type.FindMethod(name, parameters);
                        if (methodSchema == null)
                            this.ReportError("Import of {0} named '{1}' from '{2}' failed", "method", name, type.Name);
                    }
                    else
                    {
                        int num2 = this._reader.ReadInt32();
                        if (type is MarkupTypeSchema markupTypeSchema)
                        {
                            foreach (MarkupMethodSchema virtualMethod in markupTypeSchema.VirtualMethods)
                            {
                                if (virtualMethod.VirtualId == num2)
                                {
                                    methodSchema = virtualMethod;
                                    break;
                                }
                            }
                        }
                        if (methodSchema == null)
                            this.ReportError("Import of virtual method with index {0} from '{1}' failed", num2.ToString(), type.Name);
                    }
                    methodSchemaArray[index1] = methodSchema;
                }
                importTables.MethodImports = methodSchemaArray;
            }
            ushort num5 = this._reader.ReadUInt16();
            if (num5 <= 0)
                return;
            EventSchema[] eventSchemaArray = new EventSchema[num5];
            for (int index = 0; index < num5; ++index)
            {
                TypeSchema type = this.MapIndexToType(this._reader.ReadUInt16());
                string name = this.ReadDataTableString();
                EventSchema eventSchema = type.FindEvent(name);
                if (eventSchema == null)
                    this.ReportError("Import of {0} named '{1}' from '{2}' failed", "event", name, type.Name);
                else
                    eventSchemaArray[index] = eventSchema;
            }
            importTables.EventImports = eventSchemaArray;
        }

        private void DepersistDataMappingsTable()
        {
            ushort num1 = this._reader.ReadUInt16();
            if (num1 <= 0)
                return;
            MarkupDataMapping[] dataMappingsTable = new MarkupDataMapping[num1];
            for (int index1 = 0; index1 < num1; ++index1)
            {
                MarkupDataMapping markupDataMapping = new MarkupDataMapping(null);
                ushort index2 = this._reader.ReadUInt16();
                markupDataMapping.TargetType = (MarkupDataTypeSchema)this.MapIndexToType(index2);
                markupDataMapping.Provider = this.ReadDataTableString();
                ushort num2 = this._reader.ReadUInt16();
                markupDataMapping.Mappings = new MarkupDataMappingEntry[num2];
                for (int index3 = 0; index3 < num2; ++index3)
                {
                    MarkupDataMappingEntry dataMappingEntry = new MarkupDataMappingEntry();
                    dataMappingEntry.Source = this.ReadDataTableString();
                    dataMappingEntry.Target = this.ReadDataTableString();
                    ushort num3 = this._reader.ReadUInt16();
                    dataMappingEntry.Property = (MarkupDataTypePropertySchema)this._loadResultTarget.ImportTables.PropertyImports[num3];
                    dataMappingEntry.DefaultValue = !this._reader.ReadBool() ? MarkupDataProvider.GetDefaultValueForType(dataMappingEntry.Property.PropertyType) : dataMappingEntry.Property.PropertyType.DecodeBinary(this._reader);
                    markupDataMapping.Mappings[index3] = dataMappingEntry;
                }
                dataMappingsTable[index1] = markupDataMapping;
            }
            this._loadResultTarget.SetDataMappingsTable(dataMappingsTable);
        }

        public static void DecodeInheritableSymbolTable(
          MarkupTypeSchema typeExport,
          ByteCodeReader reader,
          IntPtr startAddress)
        {
            if (reader == null)
            {
                uint size = ByteCodeReader.ReadUInt32(startAddress);
                reader = new ByteCodeReader(new IntPtr(startAddress.ToInt64() + 4L), size, false);
            }
            else
            {
                int num1 = (int)reader.ReadUInt32();
            }
            MarkupLoadResult owner = typeExport.Owner as MarkupLoadResult;
            MarkupBinaryDataTable binaryDataTable = owner.BinaryDataTable;
            uint num2 = reader.ReadUInt32();
            SymbolRecord[] symbolTable;
            if (num2 > 0U)
            {
                symbolTable = new SymbolRecord[num2];
                for (int index = 0; index < num2; ++index)
                {
                    SymbolRecord symbolRecord = new SymbolRecord();
                    symbolRecord.Name = ReadDataTableString(reader, binaryDataTable);
                    symbolRecord.SymbolOrigin = (SymbolOrigin)reader.ReadByte();
                    ushort num3 = reader.ReadUInt16();
                    symbolRecord.Type = owner.ImportTables.TypeImports[num3];
                    symbolTable[index] = symbolRecord;
                }
            }
            else
                symbolTable = SymbolRecord.EmptyList;
            typeExport.SetInheritableSymbolsTable(symbolTable);
        }

        private void DepersistInheritedSymbolTable(MarkupTypeSchema typeExport)
        {
            if (this._reader.IsInFixedMemory)
            {
                typeExport.SetAddressOfInheritableSymbolTable(this._reader.CurrentAddress);
                this._reader.CurrentOffset += this._reader.ReadUInt32();
            }
            else
                DecodeInheritableSymbolTable(typeExport, this._reader, IntPtr.Zero);
        }

        private void DepersistConstantsTable()
        {
            ushort num = this._reader.ReadUInt16();
            if (num <= 0)
                return;
            object[] runtimeList = new object[num];
            MarkupConstantsTable constantsTable = new MarkupConstantsTable(runtimeList);
            if (!this._reader.IsInFixedMemory)
            {
                this._reader.CurrentOffset += (uint)((num + 1) * 4);
                for (int index = 0; index < num; ++index)
                {
                    object obj = DepersistConstant(this._reader, _loadResultTarget);
                    runtimeList[index] = obj;
                }
            }
            else
            {
                ByteCodeReader constantsTableReader = new ByteCodeReader(this._reader.CurrentAddress, ByteCodeReader.ReadUInt32(this._reader.GetAddress(this._reader.CurrentOffset + num * 4U)), false);
                constantsTable.SetConstantsTableReader(constantsTableReader, _loadResultTarget);
            }
            this._loadResultTarget.BinaryDataTable.SetConstantsTable(constantsTable);
        }

        public static object DepersistConstant(ByteCodeReader reader, MarkupLoadResult loadResult)
        {
            ushort num = reader.ReadUInt16();
            TypeSchema typeImport = loadResult.ImportTables.TypeImports[num];
            MarkupConstantPersistMode constantPersistMode = (MarkupConstantPersistMode)reader.ReadByte();
            object instance = null;
            switch (constantPersistMode)
            {
                case MarkupConstantPersistMode.Binary:
                    instance = typeImport.DecodeBinary(reader);
                    break;
                case MarkupConstantPersistMode.FromString:
                    string str = ReadDataTableString(reader, loadResult.BinaryDataTable);
                    typeImport.TypeConverter(str, StringSchema.Type, out instance);
                    break;
                case MarkupConstantPersistMode.Canonical:
                    string name = ReadDataTableString(reader, loadResult.BinaryDataTable);
                    instance = typeImport.FindCanonicalInstance(name);
                    break;
            }
            return instance;
        }

        private static MarkupLineNumberTable DecodeLineNumberTable(
          ByteCodeReader reader)
        {
            ushort num = reader.ReadUInt16();
            ulong[] runtimeList = new ulong[num];
            for (int index = 0; index < num; ++index)
                runtimeList[index] = reader.ReadUInt64();
            return new MarkupLineNumberTable(runtimeList);
        }

        public static MarkupLineNumberTable DecodeLineNumberTable(IntPtr address)
        {
            uint size = (uint)(ByteCodeReader.ReadUInt16(address) * 12 + 2);
            return DecodeLineNumberTable(new ByteCodeReader(address, size, false));
        }

        private void DepersistLineNumberTable()
        {
            if (this._reader.IsInFixedMemory)
            {
                this._loadResultTarget.SetAddressOfLineNumberData(this._reader.GetAddress(this._lineNumberTableStart));
            }
            else
            {
                uint currentOffset = this._reader.CurrentOffset;
                this._reader.CurrentOffset = this._lineNumberTableStart;
                this._loadResultTarget.SetLineNumberTable(DecodeLineNumberTable(this._reader));
                this._reader.CurrentOffset = currentOffset;
            }
        }

        private void DepersistObjectSection()
        {
            uint num = this._objectSectionEnd - this._objectSectionStart;
            ByteCodeReader reader;
            if (this._reader.IsInFixedMemory)
            {
                reader = new ByteCodeReader(this._reader.GetAddress(this._objectSectionStart), num, false);
            }
            else
            {
                ByteCodeWriter byteCodeWriter = new ByteCodeWriter();
                byteCodeWriter.Write(this._reader.GetAddress(this._objectSectionStart), num);
                reader = byteCodeWriter.CreateReader();
            }
            this._loadResultTarget.SetObjectSection(reader);
        }

        private void DepersistBinaryDataTable(uint binaryDataTableOffset)
        {
            uint currentOffset = this._reader.CurrentOffset;
            this._reader.CurrentOffset = binaryDataTableOffset;
            int stringCount = this._reader.ReadInt32();
            this._binaryDataTable = new MarkupBinaryDataTable(null, stringCount);
            if (!this._reader.IsInFixedMemory)
            {
                this._reader.CurrentOffset += (uint)((stringCount + 1) * 4);
                Vector<string> strings = this._binaryDataTable.Strings;
                for (int index = 0; index < stringCount; ++index)
                {
                    string str = this._reader.ReadString();
                    strings[index] = str;
                }
            }
            else
                this._binaryDataTable.SetStringTableReader(new ByteCodeReader(this._reader.CurrentAddress, ByteCodeReader.ReadUInt32(this._reader.GetAddress(this._reader.CurrentOffset + (uint)(stringCount * 4))), false));
            this._loadResultTarget.SetBinaryDataTable(this._binaryDataTable);
            this._reader.CurrentOffset = currentOffset;
        }

        private LoadResult MapIndexToDependent(ushort index)
        {
            LoadResult loadResult;
            switch (index)
            {
                case 65533:
                    loadResult = _loadResultTarget;
                    break;
                case 65534:
                    loadResult = MarkupSystem.UIXGlobal;
                    break;
                default:
                    loadResult = this._binaryDataTableLoadResult == null ? this._loadResultTarget.Dependencies[index] : this._binaryDataTableLoadResult.Dependencies[index];
                    break;
            }
            return loadResult;
        }

        private TypeSchema MapIndexToType(ushort index) => this._loadResultTarget.ImportTables.TypeImports[index];

        private string ReadDataTableString() => this._binaryDataTable.GetStringByIndex(this._reader.ReadInt32());

        private static string ReadDataTableString(
          ByteCodeReader reader,
          MarkupBinaryDataTable binaryDataTable)
        {
            int index = reader.ReadInt32();
            return binaryDataTable.GetStringByIndex(index);
        }

        private uint[] ReadUInt32ArrayHelper()
        {
            uint num = this._reader.ReadUInt32();
            if (num <= 0U)
                return null;
            uint[] numArray = new uint[num];
            for (int index = 0; index < num; ++index)
                numArray[index] = this._reader.ReadUInt32();
            return numArray;
        }

        private string[] ReadStringArrayHelper()
        {
            uint num = this._reader.ReadUInt32();
            if (num <= 0U)
                return null;
            string[] strArray = new string[num];
            for (int index = 0; index < num; ++index)
                strArray[index] = this.ReadDataTableString();
            return strArray;
        }

        private MethodSchema[] ReadMarkupMethodArrayHelper(
          TypeSchema markupTypeDefinition,
          MarkupTypeSchema typeExport)
        {
            MethodSchema[] methodSchemaArray = null;
            ushort num1 = this._reader.ReadUInt16();
            if (num1 > 0)
            {
                methodSchemaArray = new MethodSchema[num1];
                for (int index1 = 0; index1 < num1; ++index1)
                {
                    string name = this.ReadDataTableString();
                    TypeSchema type = this.MapIndexToType(this._reader.ReadUInt16());
                    uint num2 = this._reader.ReadUInt32();
                    TypeSchema[] parameterTypes = TypeSchema.EmptyList;
                    if (num2 > 0U)
                    {
                        parameterTypes = new TypeSchema[num2];
                        for (int index2 = 0; index2 < num2; ++index2)
                        {
                            ushort index3 = this._reader.ReadUInt16();
                            parameterTypes[index2] = this.MapIndexToType(index3);
                        }
                    }
                    string[] parameterNames = this.ReadStringArrayHelper() ?? MarkupMethodSchema.s_emptyStringArray;
                    int virtualId = this._reader.ReadInt32();
                    bool isVirtualThunk = this._reader.ReadBool();
                    uint codeOffset = this._reader.ReadUInt32();
                    MarkupMethodSchema markupMethodSchema = MarkupMethodSchema.Build(markupTypeDefinition, typeExport, name, type, parameterTypes, parameterNames, isVirtualThunk);
                    markupMethodSchema.SetCodeOffset(codeOffset);
                    markupMethodSchema.SetVirtualId(virtualId);
                    methodSchemaArray[index1] = markupMethodSchema;
                }
            }
            return methodSchemaArray;
        }

        private TypeSchema MarkupTypeToDefinition(MarkupType markupType)
        {
            switch (markupType)
            {
                case MarkupType.UI:
                    return UISchema.Type;
                case MarkupType.Effect:
                    return EffectSchema.Type;
                case MarkupType.DataType:
                    return DataTypeSchema.Type;
                case MarkupType.DataQuery:
                    return DataQuerySchema.Type;
                default:
                    return ClassSchema.Type;
            }
        }

        private TypeSchema[] GetTempParameterArray(int count)
        {
            if (this._typeSchemaArrays == null)
                this._typeSchemaArrays = new TypeSchema[5][];
            if (count >= this._typeSchemaArrays.Length)
                return new TypeSchema[count];
            int index = count - 1;
            if (this._typeSchemaArrays[index] == null)
                this._typeSchemaArrays[index] = new TypeSchema[count];
            return this._typeSchemaArrays[index];
        }

        public void ReportError(string error, string param0, string param1, string param2) => this.ReportError(string.Format(error, param0, param1, param2));

        public void ReportError(string error, string param0, string param1) => this.ReportError(string.Format(error, param0, param1));

        public void ReportError(string error, string param0) => this.ReportError(string.Format(error, param0));

        public void ReportError(string error)
        {
            this.MarkHasErrors();
            ErrorManager.ReportError(error);
        }

        public void MarkHasErrors()
        {
            if (this._hasErrors)
                return;
            this._hasErrors = true;
            this._loadResultTarget.MarkLoadFailed();
        }

        public bool HasErrors => this._hasErrors;

        public static bool IsUIB(Resource resource)
        {
            bool flag = false;
            if (resource.Length > 4U && ByteCodeReader.ReadUInt32(resource.Buffer) == 440551765U)
                flag = true;
            return flag;
        }
    }
}
