// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateNamespace
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateNamespace : Microsoft.Iris.Markup.Validation.Validate
    {
        private string _prefix;
        private string _uri;
        private ValidateNamespace _next;

        public ValidateNamespace(
          SourceMarkupLoader owner,
          string prefix,
          string uri,
          int line,
          int column)
          : base(owner, line, column)
        {
            this._prefix = prefix;
            this._uri = uri;
        }

        public string Prefix => this._prefix;

        public ValidateNamespace Next
        {
            get => this._next;
            set => this._next = value;
        }

        public void AppendToEnd(ValidateNamespace item)
        {
            ValidateNamespace validateNamespace = this;
            while (validateNamespace.Next != null)
                validateNamespace = validateNamespace.Next;
            validateNamespace.Next = item;
        }

        public LoadResult Validate()
        {
            if (this._uri == "Me")
                return (LoadResult)this.Owner.LoadResultTarget;
            LoadResult loadResult = MarkupSystem.ResolveLoadResult(this._uri, this.Owner.LoadResultTarget.IslandReferences);
            if (loadResult == null || loadResult is ErrorLoadResult)
                this.ReportError("Unable to load '{0}' (xmlns prefix '{1}')", this._uri, this._prefix);
            else if (loadResult.Status == LoadResultStatus.Error)
                this.MarkHasErrors();
            if (MarkupSystem.CompileMode)
                loadResult.SetCompilerReferenceName(this._uri);
            return loadResult;
        }
    }
}
