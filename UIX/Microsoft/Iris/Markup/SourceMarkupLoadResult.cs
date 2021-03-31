// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.SourceMarkupLoadResult
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Markup.Validation;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup
{
    internal class SourceMarkupLoadResult : MarkupLoadResult
    {
        private MarkupConstantsTable _constantsTable;
        private MarkupImportTables _importTables;
        private Resource _resource;
        private SourceMarkupLoader _loader;
        private bool _doneWithLoader;

        public SourceMarkupLoadResult(Resource resource, string uri)
          : base(uri)
        {
            this._resource = resource;
            if (uri != resource.Uri)
                this._uriUnderlying = resource.Uri;
            this._loader = SourceMarkupLoader.Load(this, resource);
        }

        public SourceMarkupLoadResult(string uri)
          : base(uri)
        {
        }

        public override bool IsSource => true;

        internal SourceMarkupLoader Loader => this._loader;

        public override MarkupConstantsTable ConstantsTable => this._constantsTable;

        public override MarkupImportTables ImportTables => this._importTables;

        public override void Load(LoadPass currentPass)
        {
            if (this._doneWithLoader)
                return;
            ErrorManager.EnterContext((object)this.ErrorContextUri);
            this._loader.Validate(currentPass);
            ErrorManager.ExitContext();
            if (currentPass != LoadPass.Done)
                return;
            if (!MarkupSystem.TrackAdditionalMetadata)
                this._loader = (SourceMarkupLoader)null;
            this._doneWithLoader = true;
        }

        public void ValidateType(MarkupTypeSchema typeSchema, LoadPass currentPass)
        {
            ValidateClass loadData = (ValidateClass)typeSchema.LoadData;
            if (loadData == null)
                return;
            ErrorManager.EnterContext((object)this.ErrorContextUri);
            loadData.Validate(currentPass);
            ErrorManager.ExitContext();
        }

        public void ValidationComplete()
        {
            if (this._resource != null)
            {
                this._resource.Free();
                this._resource = (Resource)null;
            }
            if (this.Status == LoadResultStatus.Loading)
                this.SetStatus(LoadResultStatus.Success);
            foreach (MarkupTypeSchema markupTypeSchema in this.ExportTable)
                markupTypeSchema.Seal();
        }

        public void SetConstantsTable(MarkupConstantsTable constantsTable) => this._constantsTable = constantsTable;

        public void SetImportTables(MarkupImportTables importTables) => this._importTables = importTables;
    }
}
