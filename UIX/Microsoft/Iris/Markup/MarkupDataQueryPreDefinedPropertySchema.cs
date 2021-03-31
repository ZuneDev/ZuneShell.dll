// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.MarkupDataQueryPreDefinedPropertySchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup
{
    internal class MarkupDataQueryPreDefinedPropertySchema : MarkupDataQueryPropertySchema
    {
        private GetValueHandler _getValueHandler;
        private SetValueHandler _setValueHandler;

        public MarkupDataQueryPreDefinedPropertySchema(
          MarkupDataQuerySchema owner,
          TypeSchema propertyType,
          string name,
          GetValueHandler getValueHandler,
          SetValueHandler setValueHandler)
          : base((MarkupTypeSchema)owner, name, propertyType)
        {
            this.InvalidatesQuery = false;
            this._getValueHandler = getValueHandler;
            this._setValueHandler = setValueHandler;
        }

        public override bool CanWrite => this._setValueHandler != null;

        public override ExpressionRestriction ExpressionRestriction => !this.CanWrite ? ExpressionRestriction.ReadOnly : ExpressionRestriction.None;

        public override object GetValue(object instance) => this._getValueHandler(instance);

        public override void SetValue(ref object instance, object value) => this._setValueHandler(ref instance, value);
    }
}
