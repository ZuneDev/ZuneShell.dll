// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DataMappingSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DataMappingSchema
    {
        public static UIXTypeSchema Type;

        private static void SetTargetType(ref object instanceObj, object valueObj)
        {
        }

        private static void SetProvider(ref object instanceObj, object valueObj)
        {
        }

        private static object GetMappings(object instanceObj) => (object)null;

        public static void Pass1Initialize() => DataMappingSchema.Type = new UIXTypeSchema((short)45, "DataMapping", (string)null, (short)-1, typeof(object), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)45, "TargetType", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(DataMappingSchema.SetTargetType), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)45, "Provider", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(DataMappingSchema.SetProvider), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)45, "Mappings", (short)138, (short)140, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(DataMappingSchema.GetMappings), (SetValueHandler)null, false);
            DataMappingSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
