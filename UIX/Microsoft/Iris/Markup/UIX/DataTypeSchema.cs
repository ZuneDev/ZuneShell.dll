// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DataTypeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DataTypeSchema
    {
        public static UIXTypeSchema Type;

        private static void SetProvider(ref object instanceObj, object valueObj)
        {
            MarkupDataType markupDataType = (MarkupDataType)instanceObj;
        }

        public static void Pass1Initialize() => DataTypeSchema.Type = new UIXTypeSchema((short)48, "DataType", (string)null, (short)29, typeof(MarkupDataType), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)48, "Provider", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(DataTypeSchema.SetProvider), false);
            DataTypeSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
