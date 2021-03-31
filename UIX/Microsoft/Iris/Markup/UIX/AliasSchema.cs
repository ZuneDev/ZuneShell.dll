// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.AliasSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.UIX
{
    internal static class AliasSchema
    {
        public static UIXTypeSchema Type;

        private static void SetType(ref object instanceObj, object valueObj)
        {
        }

        public static void Pass1Initialize() => AliasSchema.Type = new UIXTypeSchema((short)2, "Alias", (string)null, (short)-1, typeof(object), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)2, "Type", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(AliasSchema.SetType), false);
            AliasSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
