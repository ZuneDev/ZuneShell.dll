// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.BrightnessSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class BrightnessSchema
    {
        public static UIXTypeSchema Type;

        private static object GetBrightness(object instanceObj) => (object)((BrightnessElement)instanceObj).Brightness;

        private static void SetBrightness(ref object instanceObj, object valueObj) => ((BrightnessElement)instanceObj).Brightness = (float)valueObj;

        private static object Construct() => (object)new BrightnessElement();

        public static void Pass1Initialize() => BrightnessSchema.Type = new UIXTypeSchema((short)17, "Brightness", (string)null, (short)80, typeof(BrightnessElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)17, "Brightness", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(BrightnessSchema.GetBrightness), new SetValueHandler(BrightnessSchema.SetBrightness), false);
            BrightnessSchema.Type.Initialize(new DefaultConstructHandler(BrightnessSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
