// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.BrightnessInstanceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class BrightnessInstanceSchema
    {
        public static UIXTypeSchema Type;

        private static void SetBrightness(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Brightness", (float)valueObj);

        private static object CallPlayBrightnessAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Brightness, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => BrightnessInstanceSchema.Type = new UIXTypeSchema((short)18, "BrightnessInstance", (string)null, (short)74, typeof(EffectElementWrapper), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)18, "Brightness", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(BrightnessInstanceSchema.SetBrightness), false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)18, "PlayBrightnessAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(BrightnessInstanceSchema.CallPlayBrightnessAnimationEffectFloatAnimation), false);
            BrightnessInstanceSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
