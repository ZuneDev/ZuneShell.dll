// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.MarkupLoadResult
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup
{
    internal abstract class MarkupLoadResult : LoadResult
    {
        private LoadResult[] _dependenciesTable = LoadResult.EmptyList;
        private ByteCodeReader _reader;
        protected MarkupBinaryDataTable _binaryDataTable;
        protected MarkupLineNumberTable _lineNumberTable;
        private TypeSchema[] _exportTable = TypeSchema.EmptyList;
        private AliasMapping[] _aliasTable;
        private Map<string, TypeSchema> _resolvedAliases;
        private MarkupDataMapping[] _dataMappingsTable;
        private LoadResultStatus _status;

        public static LoadResult Create(string uri, Resource resource) => !CompiledMarkupLoader.IsUIB(resource) ? new SourceMarkupLoadResult(resource, uri) : (LoadResult)new CompiledMarkupLoadResult(resource, uri);

        public MarkupLoadResult(string uri)
          : base(uri)
          => this._status = LoadResultStatus.Loading;

        private void OnLoaded()
        {
            if (this._dataMappingsTable == null)
                return;
            foreach (MarkupDataMapping mapping in this._dataMappingsTable)
                MarkupDataProvider.AddDataMapping(mapping);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            if (this._dataMappingsTable != null)
            {
                foreach (MarkupDataMapping mapping in this._dataMappingsTable)
                    MarkupDataProvider.RemoveDataMapping(mapping);
            }
            if (this._reader != null)
                this._reader.Dispose(this);
            foreach (DisposableObject disposableObject in this._exportTable)
                disposableObject.Dispose(this);
            this._exportTable = null;
        }

        public override TypeSchema FindType(string name)
        {
            TypeSchema typeSchema = this.FindTypeWorker(name);
            if (typeSchema == null)
            {
                typeSchema = this.ResolveAlias(name);
                if (typeSchema != null)
                    this._resolvedAliases[name] = typeSchema;
            }
            return typeSchema;
        }

        private TypeSchema FindTypeWorker(string name)
        {
            for (int index = 0; index < this._exportTable.Length; ++index)
            {
                TypeSchema typeSchema = this._exportTable[index];
                if (name == typeSchema.Name)
                    return typeSchema;
            }
            TypeSchema typeSchema1;
            return this._resolvedAliases != null && this._resolvedAliases.TryGetValue(name, out typeSchema1) ? typeSchema1 : null;
        }

        private TypeSchema ResolveAlias(string name)
        {
            int num = 100;
            var markupLoadResult = this;
            while (num-- > 0)
            {
                AliasMapping aliasMapping1 = null;
                if (markupLoadResult._aliasTable != null)
                {
                    foreach (AliasMapping aliasMapping2 in markupLoadResult._aliasTable)
                    {
                        if (aliasMapping2.Alias == name)
                        {
                            aliasMapping1 = aliasMapping2;
                            break;
                        }
                    }
                }
                if (aliasMapping1 != null)
                {
                    if (!(aliasMapping1.LoadResult is MarkupLoadResult))
                        return aliasMapping1.LoadResult.FindType(aliasMapping1.TargetType);
                    markupLoadResult = (MarkupLoadResult)aliasMapping1.LoadResult;
                    TypeSchema typeWorker = markupLoadResult.FindTypeWorker(aliasMapping1.TargetType);
                    if (typeWorker != null)
                        return typeWorker;
                    name = aliasMapping1.TargetType;
                }
                else
                    break;
            }
            if (num <= 0)
                ErrorManager.ReportError("Alias cycle detected: {0} {1}", this.ToString(), name);
            return null;
        }

        public abstract bool IsSource { get; }

        public override LoadResult[] Dependencies => this._dependenciesTable;

        public abstract MarkupImportTables ImportTables { get; }

        public ByteCodeReader ObjectSection => this._reader;

        public override TypeSchema[] ExportTable => this._exportTable;

        public AliasMapping[] AliasTable => this._aliasTable;

        public MarkupDataMapping[] DataMappingsTable => this._dataMappingsTable;

        public MarkupBinaryDataTable BinaryDataTable => this._binaryDataTable;

        public abstract MarkupConstantsTable ConstantsTable { get; }

        public virtual MarkupLineNumberTable LineNumberTable => this._lineNumberTable;

        public void SetDependenciesTable(LoadResult[] dependenciesTable, bool registerDependencies)
        {
            this._dependenciesTable = dependenciesTable;
            if (!registerDependencies)
                return;
            this.RegisterDependenciesUsage();
        }

        public void SetDependenciesTable(LoadResult[] dependenciesTable) => this.SetDependenciesTable(dependenciesTable, true);

        public void SetObjectSection(ByteCodeReader reader)
        {
            reader.DeclareOwner(this);
            this._reader = reader;
        }

        public void SetBinaryDataTable(MarkupBinaryDataTable binaryDataTable) => this._binaryDataTable = binaryDataTable;

        public void SetLineNumberTable(MarkupLineNumberTable lineNumberTable) => this._lineNumberTable = lineNumberTable;

        public void SetExportTable(TypeSchema[] exportTable) => this._exportTable = exportTable;

        public void SetAliasTable(AliasMapping[] aliasTable)
        {
            this._aliasTable = aliasTable;
            if (this._aliasTable == null)
                return;
            this._resolvedAliases = new Map<string, TypeSchema>(this._aliasTable.Length);
        }

        public void SetDataMappingsTable(MarkupDataMapping[] dataMappingsTable) => this._dataMappingsTable = dataMappingsTable;

        public override LoadResultStatus Status => this._status;

        public void SetStatus(LoadResultStatus status)
        {
            if (this._status == status)
                return;
            this._status = status;
            if (this._status != LoadResultStatus.Success)
                return;
            this.OnLoaded();
        }

        public virtual void MarkLoadFailed() => this._status = LoadResultStatus.Error;
    }
}
