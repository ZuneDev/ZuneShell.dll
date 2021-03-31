// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateFromString
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateFromString : ValidateObject
    {
        private string _fromString;
        private bool _expandEscapes;
        private TypeSchema _typeHint;
        private int _typeHintIndex;
        private object _fromStringInstance;

        public ValidateFromString(
          SourceMarkupLoader owner,
          string fromString,
          bool expandEscapes,
          int line,
          int column)
          : base(owner, line, column, ObjectSourceType.FromString)
        {
            this._fromString = fromString;
            this._expandEscapes = expandEscapes;
        }

        public string FromString => this._fromString;

        public override TypeSchema ObjectType => this._typeHint;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            this._typeHint = typeRestriction.Primary;
            if (!this._typeHint.SupportsTypeConversion((TypeSchema)StringSchema.Type))
            {
                this.ReportError("String conversion is not available for '{0}'", this._typeHint.Name);
            }
            else
            {
                if (this._expandEscapes)
                {
                    string invalidSequence;
                    this._fromString = StringUtility.Unescape(this._fromString, out int _, out invalidSequence);
                    if (this._fromString == null)
                    {
                        this.ReportError("Invalid escape sequence '{0}' in string literal", invalidSequence);
                        return;
                    }
                }
                Result result = this._typeHint.TypeConverter((object)this._fromString, (TypeSchema)StringSchema.Type, out this._fromStringInstance);
                if (result.Failed)
                    this.ReportError(result.Error);
                else
                    this._typeHintIndex = this.Owner.TrackImportedType(this._typeHint);
            }
        }

        public object FromStringInstance => this._fromStringInstance;

        public int TypeHintIndex => this._typeHintIndex;

        public override string ToString() => this._typeHint != null ? string.Format("FromString : '{0}' {1}", (object)this._fromString, (object)this._typeHint) : "Unavailable";
    }
}
