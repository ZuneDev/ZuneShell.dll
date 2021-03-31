// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateDataQuery
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateDataQuery : ValidateClass
    {
        public ValidateDataQuery(
          SourceMarkupLoader owner,
          ValidateTypeIdentifier typeIdentifier,
          int line,
          int offset)
          : base(owner, typeIdentifier, line, offset)
        {
        }

        protected override void PopulatePublicModel(ValidateContext context)
        {
            base.PopulatePublicModel(context);
            if (this._typeExport == null)
                return;
            ((MarkupDataQuerySchema)this._typeExport).ProviderName = this.ExtractStringProperty("Provider", true);
            ((MarkupDataQuerySchema)this._typeExport).ResultType = this.ExtractTypeSchemaProperty("ResultType", context, true);
        }

        protected override void AnnotateProperty(
          ValidateObjectTag objectTag,
          ValidateContext context,
          MarkupPropertySchema propertyExport)
        {
            string stringProperty = objectTag.ExtractStringProperty("DefaultValue", false);
            object obj = ValidateDataMapping.ConvertDefaultValue((Validate)this, propertyExport.PropertyType, stringProperty);
            ((MarkupDataQueryPropertySchema)propertyExport).DefaultValue = obj;
            bool flag;
            if (objectTag.ExtractBooleanProperty("InvalidatesQuery", context, false, out flag))
                ((MarkupDataQueryPropertySchema)propertyExport).InvalidatesQuery = flag;
            if (ListSchema.Type.IsAssignableFrom(objectTag.FoundType))
            {
                TypeSchema typeSchemaProperty = objectTag.ExtractTypeSchemaProperty("UnderlyingCollectionType", context, false);
                if (typeSchemaProperty != null)
                    ((MarkupDataQueryPropertySchema)propertyExport).SetUnderlyingCollectionType(typeSchemaProperty);
            }
            if (!objectTag.HasErrors)
                return;
            this.MarkHasErrors();
        }
    }
}
