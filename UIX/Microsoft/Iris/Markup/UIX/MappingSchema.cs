// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.MappingSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.UIX
{
    internal static class MappingSchema
    {
        public static UIXTypeSchema Type;

        private static void SetProperty(ref object instanceObj, object valueObj)
        {
        }

        private static void SetSource(ref object instanceObj, object valueObj)
        {
        }

        private static void SetTarget(ref object instanceObj, object valueObj)
        {
        }

        private static void SetDefaultValue(ref object instanceObj, object valueObj)
        {
        }

        private static object Construct() => new object();

        public static void Pass1Initialize() => MappingSchema.Type = new UIXTypeSchema((short)140, "Mapping", (string)null, (short)-1, typeof(object), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)140, "Property", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(MappingSchema.SetProperty), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)140, "Source", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(MappingSchema.SetSource), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)140, "Target", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(MappingSchema.SetTarget), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)140, "DefaultValue", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(MappingSchema.SetDefaultValue), false);
            MappingSchema.Type.Initialize(new DefaultConstructHandler(MappingSchema.Construct), (ConstructorSchema[])null, new PropertySchema[4]
            {
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
