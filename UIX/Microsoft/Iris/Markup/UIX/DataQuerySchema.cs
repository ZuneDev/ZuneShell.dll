// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DataQuerySchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DataQuerySchema
    {
        public static UIXTypeSchema Type;

        private static void SetProvider(ref object instanceObj, object valueObj)
        {
            MarkupDataQuery markupDataQuery = (MarkupDataQuery)instanceObj;
        }

        private static void SetResultType(ref object instanceObj, object valueObj)
        {
            MarkupDataQuery markupDataQuery = (MarkupDataQuery)instanceObj;
        }

        public static void Pass1Initialize() => DataQuerySchema.Type = new UIXTypeSchema((short)46, "DataQuery", (string)null, (short)29, typeof(MarkupDataQuery), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)46, "Provider", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(DataQuerySchema.SetProvider), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)46, "ResultType", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(DataQuerySchema.SetResultType), false);
            DataQuerySchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
