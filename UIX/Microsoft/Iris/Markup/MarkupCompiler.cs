// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.MarkupCompiler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.OS;
using Microsoft.Iris.Session;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Iris.Markup
{
    internal class MarkupCompiler
    {
        private ByteCodeWriter _writer;
        private MarkupLoadResult _loadResult;
        private MarkupBinaryDataTable _binaryDataTable;
        private bool _usingSharedBinaryDataTable;
        private uint _objectSectionStartFixup;
        private uint _objectSectionEndFixup;
        private uint _lineNumberTableStartFixup;
        private uint _lineNumberTableEndFixup;
        private uint _binaryDataTableSectionOffsetFixup = uint.MaxValue;
        private bool _binaryDataTablePersisted;

        private MarkupCompiler()
        {
        }

        public static bool Compile(CompilerInput[] compilands, CompilerInput dataTableCompiland)
        {
            ErrorWatermark watermark1 = ErrorManager.Watermark;
            MarkupBinaryDataTable markupBinaryDataTable = null;
            if (dataTableCompiland.SourceFileName != null)
            {
                markupBinaryDataTable = new MarkupBinaryDataTable(dataTableCompiland.SourceFileName);
                MarkupConstantsTable constantsTable = new MarkupConstantsTable();
                markupBinaryDataTable.SetConstantsTable(constantsTable);
                markupBinaryDataTable.SetSourceMarkupImportTables(new SourceMarkupImportTables());
            }
            Vector vector = new Vector();
            foreach (CompilerInput compiland in compilands)
            {
                if (compiland.SourceFileName.IndexOf("://", StringComparison.Ordinal) != -1)
                    ErrorManager.ReportError($"'{compiland.SourceFileName}' is not a valid filename");
                string uri = "file://" + compiland.SourceFileName;
                LoadResult loadResult = MarkupSystem.ResolveLoadResult(uri, MarkupSystem.RootIslandId);
                if (loadResult != null && loadResult.Status != LoadResultStatus.Error)
                {
                    if (!(loadResult is MarkupLoadResult markupLoadResult) || !markupLoadResult.IsSource)
                    {
                        ErrorManager.ReportError($"'{uri}' is not markup, it cannot be compiled");
                    }
                    else
                    {
                        vector.Add(loadResult);
                        if (compiland.IdentityUri != null)
                            loadResult.SetCompilerReferenceName(compiland.IdentityUri);
                        if (markupBinaryDataTable != null)
                            markupLoadResult.SetBinaryDataTable(markupBinaryDataTable);
                    }
                }
            }
            foreach (LoadResult loadResult in vector)
                MarkupSystem.Load(loadResult.Uri, MarkupSystem.RootIslandId);
            if (!watermark1.ErrorsDetected)
            {
                for (int index = 0; index < compilands.Length; ++index)
                {
                    ErrorWatermark watermark2 = ErrorManager.Watermark;
                    ByteCodeWriter writer = Run((MarkupLoadResult)vector[index], markupBinaryDataTable);
                    if (!watermark2.ErrorsDetected)
                        SaveCompiledOutput(writer, compilands[index].OutputFileName);
                }
                if (markupBinaryDataTable != null)
                    SaveCompiledOutput(CompileBinaryDataTable(markupBinaryDataTable), dataTableCompiland.OutputFileName);
            }
            return !watermark1.ErrorsDetected;
        }

        private static void SaveCompiledOutput(ByteCodeWriter writer, string outputFile)
        {
            ErrorWatermark watermark = ErrorManager.Watermark;
            IntPtr invalidHandleValue = Win32Api.INVALID_HANDLE_VALUE;
            ByteCodeReader reader = writer.CreateReader();
            reader.DeclareOwner(typeof(MarkupSystem));
            IntPtr file = Win32Api.CreateFile(outputFile, 1073741824U, 0U, IntPtr.Zero, 2U, 0U, IntPtr.Zero);
            if (file == Win32Api.INVALID_HANDLE_VALUE)
                ErrorManager.ReportError("Unable to open output file '{0}'.  Error code {1}", outputFile, Marshal.GetLastWin32Error());
            if (!watermark.ErrorsDetected)
            {
                uint size = 0;
                IntPtr intPtr = reader.ToIntPtr(out size);
                uint lpNumberOfBytesWritten = 0;
                if (!Win32Api.WriteFile(file, intPtr, size, out lpNumberOfBytesWritten, IntPtr.Zero))
                    ErrorManager.ReportError("An error occurred while saving data to output file '{0}'.  Error code {1}", outputFile, Marshal.GetLastWin32Error());
            }
            if (file != Win32Api.INVALID_HANDLE_VALUE)
                Win32Api.CloseHandle(file);
            reader?.Dispose(typeof(MarkupSystem));
        }

        public static ByteCodeWriter Run(
          MarkupLoadResult loadResult,
          MarkupBinaryDataTable sharedBinaryDataTable)
        {
            return new MarkupCompiler().InternalRun(loadResult, sharedBinaryDataTable);
        }

        private ByteCodeWriter InternalRun(
          MarkupLoadResult loadResult,
          MarkupBinaryDataTable sharedBinaryDataTable)
        {
            this._writer = new ByteCodeWriter();
            this._loadResult = loadResult;
            this.PersistHeader();
            this.PersistTableOfContents(sharedBinaryDataTable);
            this.PersistDependencies();
            this.PersistTypeExportDeclarations();
            this.PersistTypeImportTable();
            this.PersistTypeExportDefinitions();
            this.PersistMemberImportTables();
            this.PersistDataMappingsTable();
            this.PersistConstantsTable();
            this.PersistBinaryDataTable();
            this.PersistObjectSection();
            this.PersistLineNumberTable();
            return this._writer;
        }

        private void PersistHeader()
        {
            this._writer.WriteUInt32(440551765U);
            this._writer.WriteUInt32(1012U);
        }

        private void PersistTableOfContents(MarkupBinaryDataTable sharedBinaryDataTable)
        {
            this._objectSectionStartFixup = this._writer.DataSize;
            this._writer.WriteUInt32(uint.MaxValue);
            this._objectSectionEndFixup = this._writer.DataSize;
            this._writer.WriteUInt32(uint.MaxValue);
            this._lineNumberTableStartFixup = this._writer.DataSize;
            this._writer.WriteUInt32(uint.MaxValue);
            this._lineNumberTableEndFixup = this._writer.DataSize;
            this._writer.WriteUInt32(uint.MaxValue);
            if (sharedBinaryDataTable != null)
            {
                this._binaryDataTable = sharedBinaryDataTable;
                this._writer.WriteString(this._binaryDataTable.Uri);
                this._usingSharedBinaryDataTable = true;
            }
            else
            {
                this._writer.WriteString(null);
                this._usingSharedBinaryDataTable = false;
                this._binaryDataTable = this._loadResult.BinaryDataTable;
                if (this._binaryDataTable == null)
                    this._binaryDataTable = new MarkupBinaryDataTable(null, 0);
                this._binaryDataTableSectionOffsetFixup = this._writer.DataSize;
                this._writer.WriteUInt32(uint.MaxValue);
            }
        }

        private void PersistDependencies()
        {
            if (this._usingSharedBinaryDataTable)
                return;
            this._writer.WriteUInt16(this._loadResult.Dependencies.Length);
            foreach (LoadResult dependency in this._loadResult.Dependencies)
            {
                this._writer.WriteBool(dependency is MarkupLoadResult);
                this.WriteDataTableString(dependency.GetCompilerReferenceName() ?? dependency.Uri);
            }
        }

        private void PersistTypeExportDeclarations()
        {
            this._writer.WriteUInt16(this._loadResult.ExportTable.Length);
            foreach (MarkupTypeSchema markupTypeSchema in this._loadResult.ExportTable)
            {
                MarkupType markupType = markupTypeSchema.MarkupType;
                this.WriteDataTableString(markupTypeSchema.Name);
                this._writer.WriteInt32((int)markupType);
            }
            if (this._loadResult.AliasTable == null)
            {
                this._writer.WriteUInt16(0);
            }
            else
            {
                this._writer.WriteUInt16(this._loadResult.AliasTable.Length);
                foreach (AliasMapping aliasMapping in this._loadResult.AliasTable)
                {
                    this.WriteDataTableString(aliasMapping.Alias);
                    this._writer.WriteUInt16(this.MapDependentToIndex(aliasMapping.LoadResult));
                    this.WriteDataTableString(aliasMapping.TargetType);
                }
            }
        }

        private void PersistTypeImportTable()
        {
            if (this._usingSharedBinaryDataTable)
                return;
            this._writer.WriteUInt16(this._loadResult.ImportTables.TypeImports.Length);
            foreach (TypeSchema typeImport in this._loadResult.ImportTables.TypeImports)
            {
                this._writer.WriteUInt16(this.MapDependentToIndex(typeImport.Owner));
                this.WriteDataTableString(typeImport.Name);
            }
        }

        private void PersistTypeExportDefinitions()
        {
            foreach (MarkupTypeSchema markupTypeSchema in this._loadResult.ExportTable)
            {
                MarkupType markupType = markupTypeSchema.MarkupType;
                this._writer.WriteUInt16(markupTypeSchema.TypeDepth);
                if (markupTypeSchema.TypeDepth > 1U)
                    this._writer.WriteUInt16(this.MapTypeToIndex(markupTypeSchema.Base));
                this._writer.WriteUInt32(markupTypeSchema.InitializePropertiesOffset);
                this._writer.WriteUInt32(markupTypeSchema.InitializeLocalsInputOffset);
                this._writer.WriteUInt32(markupTypeSchema.InitializeContentOffset);
                this.WriteUInt32ArrayHelper(markupTypeSchema.InitialEvaluateOffsets, "InitialEvaluateOffsets");
                this.WriteUInt32ArrayHelper(markupTypeSchema.FinalEvaluateOffsets, "FinalEvaluateOffsets");
                this.WriteUInt32ArrayHelper(markupTypeSchema.RefreshGroupOffsets, "RefreshGroupOffsets");
                this._writer.WriteUInt32(markupTypeSchema.ListenerCount);
                uint num1 = 0;
                if (markupTypeSchema.SymbolReferenceTable != null)
                    num1 = (uint)markupTypeSchema.SymbolReferenceTable.Length;
                this._writer.WriteUInt32(num1);
                if (num1 != 0U)
                {
                    foreach (SymbolReference symbolReference in markupTypeSchema.SymbolReferenceTable)
                    {
                        this.WriteDataTableString(symbolReference.Symbol);
                        this._writer.WriteByte((byte)symbolReference.Origin);
                    }
                }
                uint dataSize1 = this._writer.DataSize;
                this._writer.WriteUInt32(uint.MaxValue);
                uint dataSize2 = this._writer.DataSize;
                uint num2 = 0;
                if (markupTypeSchema.InheritableSymbolsTable != null)
                    num2 = (uint)markupTypeSchema.InheritableSymbolsTable.Length;
                this._writer.WriteUInt32(num2);
                if (num2 != 0U)
                {
                    foreach (SymbolRecord symbolRecord in markupTypeSchema.InheritableSymbolsTable)
                    {
                        this.WriteDataTableString(symbolRecord.Name);
                        this._writer.WriteByte((byte)symbolRecord.SymbolOrigin);
                        this._writer.WriteUInt16(this.MapTypeToIndex(symbolRecord.Type));
                    }
                }
                uint num3 = this._writer.DataSize - dataSize2;
                this._writer.Overwrite(dataSize1, num3);
                this._writer.WriteInt32(markupTypeSchema.TotalPropertiesAndLocalsCount);
                if (markupType == MarkupType.UI)
                {
                    UIClassTypeSchema uiClassTypeSchema = (UIClassTypeSchema)markupTypeSchema;
                    ushort num4 = 0;
                    if (uiClassTypeSchema.NamedContentTable != null)
                        num4 = (ushort)uiClassTypeSchema.NamedContentTable.Length;
                    this._writer.WriteUInt16(num4);
                    if (num4 != 0)
                    {
                        foreach (NamedContentRecord namedContentRecord in uiClassTypeSchema.NamedContentTable)
                        {
                            this.WriteDataTableString(namedContentRecord.Name);
                            this._writer.WriteUInt32(namedContentRecord.Offset);
                        }
                    }
                }
                else
                {
                    this._writer.WriteBool(((ClassTypeSchema)markupTypeSchema).IsShared);
                    if (markupType == MarkupType.Effect)
                    {
                        EffectClassTypeSchema effectClassTypeSchema = (EffectClassTypeSchema)markupTypeSchema;
                        this.WriteUInt32ArrayHelper(effectClassTypeSchema.TechniqueOffsets, "TechniqueOffsets");
                        this.WriteUInt32ArrayHelper(effectClassTypeSchema.InstancePropertyAssignments, "InstancePropertyAssignments");
                        this.WriteStringArrayHelper(effectClassTypeSchema.DynamicElementAssignments, "DynamicElementAssignments");
                        this._writer.WriteInt32(effectClassTypeSchema.DefaultElementSymbolIndex);
                    }
                    if (markupType == MarkupType.DataQuery)
                    {
                        MarkupDataQuerySchema markupDataQuerySchema = (MarkupDataQuerySchema)markupTypeSchema;
                        this.WriteDataTableString(markupDataQuerySchema.ProviderName);
                        this._writer.WriteUInt16(this.MapTypeToIndex(markupDataQuerySchema.ResultType));
                    }
                }
                PropertySchema[] properties = markupTypeSchema.Properties;
                this._writer.WriteUInt16(properties.Length);
                foreach (MarkupPropertySchema markupPropertySchema in properties)
                {
                    this.WriteDataTableString(markupPropertySchema.Name);
                    this._writer.WriteBool(markupPropertySchema.RequiredForCreation);
                    if (markupPropertySchema.OverrideCriteria != null)
                    {
                        PropertyOverrideCriteriaTypeConstraint overrideCriteria = (PropertyOverrideCriteriaTypeConstraint)markupPropertySchema.OverrideCriteria;
                        this._writer.WriteBool(true);
                        ushort index1 = this.MapTypeToIndex(overrideCriteria.Constraint);
                        ushort index2 = this.MapTypeToIndex(overrideCriteria.Use);
                        this._writer.WriteUInt16(index1);
                        this._writer.WriteUInt16(index2);
                    }
                    else
                        this._writer.WriteBool(false);
                    this._writer.WriteUInt16(this.MapTypeToIndex(markupPropertySchema.PropertyType));
                    if (markupType == MarkupType.DataType)
                    {
                        MarkupDataTypePropertySchema typePropertySchema = (MarkupDataTypePropertySchema)markupPropertySchema;
                        this._writer.WriteUInt16(typePropertySchema.AlternateType != null ? this.MapTypeToIndex(typePropertySchema.AlternateType) : ushort.MaxValue);
                    }
                    if (markupType == MarkupType.DataQuery)
                    {
                        MarkupDataQueryPropertySchema queryPropertySchema = (MarkupDataQueryPropertySchema)markupPropertySchema;
                        if (queryPropertySchema.DefaultValue != null)
                        {
                            this._writer.WriteBool(true);
                            queryPropertySchema.PropertyType.EncodeBinary(this._writer, queryPropertySchema.DefaultValue);
                        }
                        else
                            this._writer.WriteBool(false);
                        this._writer.WriteBool(queryPropertySchema.InvalidatesQuery);
                        this._writer.WriteUInt16(queryPropertySchema.AlternateType != null ? this.MapTypeToIndex(queryPropertySchema.AlternateType) : ushort.MaxValue);
                    }
                }
                this.WriteMarkupMethodArrayHelper(markupTypeSchema.Methods, "METHODS");
                this.WriteMarkupMethodArrayHelper(markupTypeSchema.VirtualMethods, "VIRTUAL METHODS");
            }
        }

        private void PersistMemberImportTables()
        {
            if (this._usingSharedBinaryDataTable)
                return;
            this._writer.WriteUInt16(this._loadResult.ImportTables.ConstructorImports.Length);
            foreach (ConstructorSchema constructorImport in this._loadResult.ImportTables.ConstructorImports)
            {
                this._writer.WriteUInt16(this.MapTypeToIndex(constructorImport.Owner));
                this._writer.WriteUInt16(constructorImport.ParameterTypes.Length);
                foreach (TypeSchema parameterType in constructorImport.ParameterTypes)
                    this._writer.WriteUInt16(this.MapTypeToIndex(parameterType));
            }
            this._writer.WriteUInt16(this._loadResult.ImportTables.PropertyImports.Length);
            foreach (PropertySchema propertyImport in this._loadResult.ImportTables.PropertyImports)
            {
                this._writer.WriteUInt16(this.MapTypeToIndex(propertyImport.Owner));
                this.WriteDataTableString(propertyImport.Name);
            }
            this._writer.WriteUInt16(this._loadResult.ImportTables.MethodImports.Length);
            foreach (MethodSchema methodImport in this._loadResult.ImportTables.MethodImports)
            {
                this._writer.WriteUInt16(this.MapTypeToIndex(methodImport.Owner));
                bool flag = false;
                MarkupMethodSchema markupMethodSchema = null;
                if (methodImport is MarkupMethodSchema)
                {
                    markupMethodSchema = (MarkupMethodSchema)methodImport;
                    flag = markupMethodSchema.IsVirtual && !markupMethodSchema.IsVirtualThunk;
                }
                this._writer.WriteBool(flag);
                if (!flag)
                {
                    this.WriteDataTableString(methodImport.Name);
                    this._writer.WriteUInt16(methodImport.ParameterTypes.Length);
                    foreach (TypeSchema parameterType in methodImport.ParameterTypes)
                        this._writer.WriteUInt16(this.MapTypeToIndex(parameterType));
                }
                else
                    this._writer.WriteInt32(markupMethodSchema.VirtualId);
            }
            this._writer.WriteUInt16(this._loadResult.ImportTables.EventImports.Length);
            foreach (EventSchema eventImport in this._loadResult.ImportTables.EventImports)
            {
                this._writer.WriteUInt16(this.MapTypeToIndex(eventImport.Owner));
                this.WriteDataTableString(eventImport.Name);
            }
        }

        private void PersistDataMappingsTable()
        {
            if (this._loadResult.DataMappingsTable == null)
            {
                this._writer.WriteUInt16(0);
            }
            else
            {
                this._writer.WriteUInt16(this._loadResult.DataMappingsTable.Length);
                foreach (MarkupDataMapping markupDataMapping in this._loadResult.DataMappingsTable)
                {
                    this._writer.WriteUInt16(this.MapTypeToIndex(markupDataMapping.TargetType));
                    this.WriteDataTableString(markupDataMapping.Provider);
                    this._writer.WriteUInt16(markupDataMapping.Mappings.Length);
                    foreach (MarkupDataMappingEntry mapping in markupDataMapping.Mappings)
                    {
                        this.WriteDataTableString(mapping.Source);
                        this.WriteDataTableString(mapping.Target);
                        this._writer.WriteUInt16(this.MapPropertyToIndex(mapping.Property));
                        if (mapping.DefaultValue != null && mapping.Property.PropertyType.SupportsBinaryEncoding)
                        {
                            this._writer.WriteBool(true);
                            mapping.Property.PropertyType.EncodeBinary(this._writer, mapping.DefaultValue);
                        }
                        else
                            this._writer.WriteBool(false);
                    }
                }
            }
        }

        private void PersistConstantsTable()
        {
            if (!this._usingSharedBinaryDataTable)
            {
                int length = this._loadResult.ConstantsTable.PersistList.Length;
                this._writer.WriteUInt16(length);
                uint dataSize = this._writer.DataSize;
                uint offset = dataSize;
                for (int index = 0; index <= length; ++index)
                    this._writer.WriteUInt32(uint.MaxValue);
                foreach (MarkupConstantPersist persist in this._loadResult.ConstantsTable.PersistList)
                {
                    this._writer.Overwrite(offset, this._writer.DataSize - dataSize);
                    offset += 4U;
                    ushort index = this.MapTypeToIndex(persist.Type);
                    MarkupConstantPersistMode constantPersistMode = persist.Mode;
                    if (persist.Type == StringSchema.Type)
                        constantPersistMode = MarkupConstantPersistMode.FromString;
                    this._writer.WriteUInt16(index);
                    this._writer.WriteByte((byte)constantPersistMode);
                    switch (constantPersistMode)
                    {
                        case MarkupConstantPersistMode.Binary:
                            this._loadResult.ImportTables.TypeImports[index].EncodeBinary(this._writer, persist.Data);
                            break;
                        case MarkupConstantPersistMode.FromString:
                        case MarkupConstantPersistMode.Canonical:
                            this.WriteDataTableString((string)persist.Data);
                            break;
                    }
                }
                this._writer.Overwrite(offset, this._writer.DataSize - dataSize);
            }
            else
                this._writer.WriteUInt16(0);
        }

        private void PersistBinaryDataTable()
        {
            if (this._binaryDataTableSectionOffsetFixup != uint.MaxValue)
            {
                uint dataSize1 = this._writer.DataSize;
                Vector<string> strings = this._binaryDataTable.Strings;
                this._writer.WriteInt32(strings.Count);
                uint dataSize2 = this._writer.DataSize;
                uint offset = dataSize2;
                for (int index = 0; index <= strings.Count; ++index)
                    this._writer.WriteUInt32(uint.MaxValue);
                foreach (string str in strings)
                {
                    this._writer.Overwrite(offset, this._writer.DataSize - dataSize2);
                    offset += 4U;
                    this._writer.WriteString(str);
                }
                this._writer.Overwrite(offset, this._writer.DataSize - dataSize2);
                this._writer.Overwrite(this._binaryDataTableSectionOffsetFixup, dataSize1);
            }
            this._binaryDataTablePersisted = true;
        }

        private void PersistLineNumberTable()
        {
            this._writer.Overwrite(this._lineNumberTableStartFixup, this._writer.DataSize);
            this._writer.WriteUInt16(this._loadResult.LineNumberTable.PersistList.Length);
            foreach (ulong persist in this._loadResult.LineNumberTable.PersistList)
                this._writer.WriteUInt64(persist);
            this._writer.Overwrite(this._lineNumberTableEndFixup, this._writer.DataSize);
        }

        private void PersistObjectSection()
        {
            this._writer.Overwrite(this._objectSectionStartFixup, this._writer.DataSize);
            this._writer.Write(this._loadResult.ObjectSection);
            this._loadResult.ObjectSection.ToIntPtr(out uint _);
            this._writer.Overwrite(this._objectSectionEndFixup, this._writer.DataSize);
        }

        private ushort MapDependentToIndex(LoadResult dependent)
        {
            if (dependent == MarkupSystem.UIXGlobal)
                return 65534;
            if (dependent == this._loadResult)
                return 65533;
            int num = this._usingSharedBinaryDataTable ? this._binaryDataTable.SourceMarkupImportTables.ImportedLoadResults.IndexOf(dependent) : Array.IndexOf<LoadResult>(this._loadResult.Dependencies, dependent);
            return num >= 0 ? (ushort)num : ushort.MaxValue;
        }

        private ushort MapTypeToIndex(TypeSchema type)
        {
            for (ushort index = 0; index < (ushort)this._loadResult.ImportTables.TypeImports.Length; ++index)
            {
                TypeSchema typeImport = this._loadResult.ImportTables.TypeImports[index];
                if (type == typeImport)
                    return index;
            }
            return ushort.MaxValue;
        }

        private ushort MapPropertyToIndex(PropertySchema property)
        {
            for (ushort index = 0; index < (ushort)this._loadResult.ImportTables.PropertyImports.Length; ++index)
            {
                PropertySchema propertyImport = this._loadResult.ImportTables.PropertyImports[index];
                if (property == propertyImport)
                    return index;
            }
            return ushort.MaxValue;
        }

        private ushort MapMethodToIndex(MethodSchema method)
        {
            for (ushort index = 0; index < (ushort)this._loadResult.ImportTables.MethodImports.Length; ++index)
            {
                MethodSchema methodImport = this._loadResult.ImportTables.MethodImports[index];
                if (method == methodImport)
                    return index;
            }
            return ushort.MaxValue;
        }

        private void WriteDataTableString(string s) => this._writer.WriteInt32(this._binaryDataTable.GetIndexOrAdd(s));

        private void WriteUInt32ArrayHelper(uint[] array, string debugName)
        {
            uint num1 = 0;
            if (array != null)
                num1 = (uint)array.Length;
            this._writer.WriteUInt32(num1);
            if (num1 == 0U)
                return;
            foreach (uint num2 in array)
                this._writer.WriteUInt32(num2);
        }

        private void WriteStringArrayHelper(string[] array, string debugName)
        {
            uint num = 0;
            if (array != null)
                num = (uint)array.Length;
            this._writer.WriteUInt32(num);
            if (num == 0U)
                return;
            foreach (string s in array)
                this.WriteDataTableString(s);
        }

        private void WriteMarkupMethodArrayHelper(MethodSchema[] methods, string debugName)
        {
            this._writer.WriteUInt16(methods.Length);
            foreach (MarkupMethodSchema method in methods)
            {
                this.WriteDataTableString(method.Name);
                this._writer.WriteUInt16(this.MapTypeToIndex(method.ReturnType));
                this._writer.WriteInt32(method.ParameterTypes.Length);
                foreach (TypeSchema parameterType in method.ParameterTypes)
                    this._writer.WriteUInt16(this.MapTypeToIndex(parameterType));
                this.WriteStringArrayHelper(method.ParameterNames, "ParameterNames");
                this._writer.WriteInt32(method.VirtualId);
                this._writer.WriteBool(method.IsVirtualThunk);
                this._writer.WriteUInt32(method.CodeOffset);
            }
        }

        public static ByteCodeWriter CompileBinaryDataTable(
          MarkupBinaryDataTable binaryDataTable)
        {
            SourceMarkupLoadResult markupLoadResult = new SourceMarkupLoadResult(binaryDataTable.Uri);
            markupLoadResult.RegisterUsage(markupLoadResult);
            binaryDataTable.ConstantsTable.PrepareForRuntimeUse();
            MarkupImportTables importTables = binaryDataTable.SourceMarkupImportTables.PrepareImportTables();
            Vector importedLoadResults = binaryDataTable.SourceMarkupImportTables.ImportedLoadResults;
            LoadResult[] dependenciesTable = new LoadResult[importedLoadResults.Count];
            for (int index = 0; index < dependenciesTable.Length; ++index)
                dependenciesTable[index] = (LoadResult)importedLoadResults[index];
            markupLoadResult.SetBinaryDataTable(binaryDataTable);
            markupLoadResult.SetConstantsTable(binaryDataTable.ConstantsTable);
            markupLoadResult.SetDependenciesTable(dependenciesTable, false);
            markupLoadResult.SetImportTables(importTables);
            markupLoadResult.SetLineNumberTable(new MarkupLineNumberTable());
            markupLoadResult.LineNumberTable.PrepareForRuntimeUse();
            markupLoadResult.SetObjectSection(new ByteCodeWriter().CreateReader());
            ByteCodeWriter byteCodeWriter = Run(markupLoadResult, null);
            markupLoadResult.UnregisterUsage(markupLoadResult);
            return byteCodeWriter;
        }
    }
}
