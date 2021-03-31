// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ColorElementInstanceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ColorElementInstanceSchema
    {
        public static UIXTypeSchema Type;

        private static void SetColor(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Color", (Color)valueObj);

        private static object CallPlayColorAnimationEffectColorAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Color, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => ColorElementInstanceSchema.Type = new UIXTypeSchema((short)37, "ColorElementInstance", (string)null, (short)74, typeof(EffectElementWrapper), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)37, "Color", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(ColorElementInstanceSchema.SetColor), false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)37, "PlayColorAnimation", new short[1]
            {
        (short) 71
            }, (short)240, new InvokeHandler(ColorElementInstanceSchema.CallPlayColorAnimationEffectColorAnimation), false);
            ColorElementInstanceSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
