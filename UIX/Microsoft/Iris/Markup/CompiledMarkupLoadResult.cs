// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.CompiledMarkupLoadResult
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.Markup
{
    internal class CompiledMarkupLoadResult : MarkupLoadResult
    {
        private Resource _resource;
        private CompiledMarkupLoader _loader;
        private IntPtr _addressOfLineNumberData;

        public CompiledMarkupLoadResult(Resource resource, string uri)
          : base(uri)
        {
            this._resource = resource;
            if (uri != resource.Uri)
                this._uriUnderlying = resource.Uri;
            this._loader = CompiledMarkupLoader.Load(this, resource);
        }

        public override bool IsSource => false;

        public override MarkupConstantsTable ConstantsTable => this._binaryDataTable.ConstantsTable;

        public override MarkupImportTables ImportTables => this._binaryDataTable.ImportTables;

        public void SetAddressOfLineNumberData(IntPtr address) => this._addressOfLineNumberData = address;

        public override MarkupLineNumberTable LineNumberTable
        {
            get
            {
                if (this._lineNumberTable == null)
                    this._lineNumberTable = CompiledMarkupLoader.DecodeLineNumberTable(this._addressOfLineNumberData);
                return this._lineNumberTable;
            }
        }

        public override void Load(LoadPass currentPass)
        {
            if (this._loader == null)
                return;
            ErrorManager.EnterContext((object)this.ErrorContextUri);
            this._loader.Depersist(currentPass);
            ErrorManager.ExitContext();
            if (currentPass != LoadPass.Done)
                return;
            this.LoadComplete();
        }

        private void LoadComplete()
        {
            if (this._resource != null)
            {
                this._resource.Free();
                this._resource = (Resource)null;
            }
            this._loader = (CompiledMarkupLoader)null;
            if (this.Status != LoadResultStatus.Loading)
                return;
            this.SetStatus(LoadResultStatus.Success);
        }
    }
}
