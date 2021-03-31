// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.Validate
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;

namespace Microsoft.Iris.Markup.Validation
{
    internal class Validate : DisposableObject
    {
        private SourceMarkupLoader _owner;
        private int _line;
        private int _column;
        private ValidateMetadata _metadata;
        private bool _hasErrors;

        public Validate(SourceMarkupLoader owner, int line, int column)
        {
            this._owner = owner;
            this._line = line;
            this._column = column;
            this._owner.TrackValidateObject(this);
        }

        protected Validate()
        {
        }

        public SourceMarkupLoader Owner => this._owner;

        public int Line => this._line;

        public int Column => this._column;

        public ValidateMetadata Metadata
        {
            get
            {
                if (this._metadata == null)
                    this._metadata = new ValidateMetadata();
                return this._metadata;
            }
        }

        public bool HasErrors => this._hasErrors;

        public void MarkHasErrors()
        {
            this._owner.MarkHasErrors();
            this._hasErrors = true;
        }

        public void ReportError(
          string error,
          string param0,
          string param1,
          string param2,
          string param3)
        {
            this.ReportError(string.Format(error, (object)param0, (object)param1, (object)param2, (object)param3));
        }

        public void ReportError(string error, string param0, string param1, string param2) => this.ReportError(string.Format(error, (object)param0, (object)param1, (object)param2));

        public void ReportError(string error, string param0, string param1) => this.ReportError(string.Format(error, (object)param0, (object)param1));

        public void ReportError(string error, string param0) => this.ReportError(string.Format(error, (object)param0));

        public void ReportError(string error)
        {
            this.MarkHasErrors();
            this._owner.ReportError(error, this._line, this._column);
        }

        public void ReportErrorWithAdjustedPosition(string error, int lineOffset, int columnOffset)
        {
            this.MarkHasErrors();
            this._owner.ReportError(error, this._line + lineOffset, this._column + columnOffset);
        }
    }
}
