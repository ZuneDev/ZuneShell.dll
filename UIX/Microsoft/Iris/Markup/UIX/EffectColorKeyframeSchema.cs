// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EffectColorKeyframeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Drawing;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EffectColorKeyframeSchema
    {
        public static UIXTypeSchema Type;

        private static object GetValue(object instanceObj) => (object)((EffectColorKeyframe)instanceObj).Color;

        private static void SetValue(ref object instanceObj, object valueObj) => ((EffectColorKeyframe)instanceObj).Color = (Color)valueObj;

        private static object Construct() => (object)new EffectColorKeyframe();

        public static void Pass1Initialize() => EffectColorKeyframeSchema.Type = new UIXTypeSchema((short)72, "EffectColorKeyframe", (string)null, (short)130, typeof(EffectColorKeyframe), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)72, "Value", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(EffectColorKeyframeSchema.GetValue), new SetValueHandler(EffectColorKeyframeSchema.SetValue), false);
            EffectColorKeyframeSchema.Type.Initialize(new DefaultConstructHandler(EffectColorKeyframeSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
