// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EffectSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EffectSchema
    {
        public static UIXTypeSchema Type;

        private static object GetTechniques(object instanceObj) => (object)null;

        public static void Pass1Initialize() => EffectSchema.Type = new UIXTypeSchema((short)69, "Effect", (string)null, (short)29, typeof(EffectClass), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)69, "Techniques", (short)138, (short)77, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(EffectSchema.GetTechniques), (SetValueHandler)null, false);
            EffectSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
