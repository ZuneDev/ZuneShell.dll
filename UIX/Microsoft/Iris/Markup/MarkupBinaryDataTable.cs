// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.MarkupBinaryDataTable
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup
{
    internal class MarkupBinaryDataTable
    {
        private string _uri;
        private Vector<string> _strings;
        private MarkupConstantsTable _constantsTable;
        private MarkupImportTables _importTables;
        private LoadResult[] _sharedDependenciesTable;
        private SourceMarkupImportTables _sourceMarkupImportTables;
        private ByteCodeReader _stringTableReader;

        public MarkupBinaryDataTable(string uri, int stringCount)
        {
            this._uri = uri;
            this._strings = new Vector<string>(stringCount);
            this._strings.ExpandTo(stringCount);
        }

        public MarkupBinaryDataTable(string uri)
          : this(uri, 0)
        {
        }

        public void SetStringTableReader(ByteCodeReader stringTableReader) => this._stringTableReader = stringTableReader;

        public string Uri => this._uri;

        public int GetIndexOrAdd(string s)
        {
            int num = this._strings.IndexOf(s);
            if (num == -1)
            {
                this._strings.Add(s);
                num = this._strings.Count - 1;
            }
            return num;
        }

        public string GetStringByIndex(int index)
        {
            string str = this._strings[index];
            if (str == null)
            {
                this._stringTableReader.CurrentOffset = (uint)(index * 4);
                this._stringTableReader.CurrentOffset = this._stringTableReader.ReadUInt32();
                str = this._stringTableReader.ReadString();
                this._strings[index] = str;
            }
            return str;
        }

        public Vector<string> Strings => this._strings;

        public MarkupConstantsTable ConstantsTable => this._constantsTable;

        public MarkupImportTables ImportTables => this._importTables;

        public SourceMarkupImportTables SourceMarkupImportTables => this._sourceMarkupImportTables;

        public void SetConstantsTable(MarkupConstantsTable constantsTable) => this._constantsTable = constantsTable;

        public void SetImportTables(MarkupImportTables importTables) => this._importTables = importTables;

        public void SetSourceMarkupImportTables(SourceMarkupImportTables sourceMarkupImportTables) => this._sourceMarkupImportTables = sourceMarkupImportTables;

        public LoadResult[] SharedDependenciesTableWithBinaryDataTable
        {
            get => this._sharedDependenciesTable;
            set => this._sharedDependenciesTable = value;
        }
    }
}
