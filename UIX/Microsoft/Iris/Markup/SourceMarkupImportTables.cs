// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.SourceMarkupImportTables
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup
{
    internal class SourceMarkupImportTables
    {
        public Vector ImportedLoadResults = new Vector();
        public Vector ImportedTypes = new Vector();
        public Vector ImportedConstructors = new Vector();
        public Vector ImportedProperties = new Vector();
        public Vector ImportedEvents = new Vector();
        public Vector ImportedMethods = new Vector();

        public MarkupImportTables PrepareImportTables()
        {
            MarkupImportTables markupImportTables = new MarkupImportTables();
            if (this.ImportedTypes.Count > 0)
            {
                TypeSchema[] typeSchemaArray = new TypeSchema[this.ImportedTypes.Count];
                for (int index = 0; index < this.ImportedTypes.Count; ++index)
                    typeSchemaArray[index] = (TypeSchema)this.ImportedTypes[index];
                markupImportTables.TypeImports = typeSchemaArray;
            }
            if (this.ImportedConstructors.Count > 0)
            {
                ConstructorSchema[] constructorSchemaArray = new ConstructorSchema[this.ImportedConstructors.Count];
                for (int index = 0; index < this.ImportedConstructors.Count; ++index)
                    constructorSchemaArray[index] = (ConstructorSchema)this.ImportedConstructors[index];
                markupImportTables.ConstructorImports = constructorSchemaArray;
            }
            if (this.ImportedProperties.Count > 0)
            {
                PropertySchema[] propertySchemaArray = new PropertySchema[this.ImportedProperties.Count];
                for (int index = 0; index < this.ImportedProperties.Count; ++index)
                    propertySchemaArray[index] = (PropertySchema)this.ImportedProperties[index];
                markupImportTables.PropertyImports = propertySchemaArray;
            }
            if (this.ImportedMethods.Count > 0)
            {
                MethodSchema[] methodSchemaArray = new MethodSchema[this.ImportedMethods.Count];
                for (int index = 0; index < this.ImportedMethods.Count; ++index)
                    methodSchemaArray[index] = (MethodSchema)this.ImportedMethods[index];
                markupImportTables.MethodImports = methodSchemaArray;
            }
            if (this.ImportedEvents.Count > 0)
            {
                EventSchema[] eventSchemaArray = new EventSchema[this.ImportedEvents.Count];
                for (int index = 0; index < this.ImportedEvents.Count; ++index)
                    eventSchemaArray[index] = (EventSchema)this.ImportedEvents[index];
                markupImportTables.EventImports = eventSchemaArray;
            }
            return markupImportTables;
        }
    }
}
