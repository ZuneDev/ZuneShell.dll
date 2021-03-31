// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EffectFloatAnimationSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EffectFloatAnimationSchema
    {
        public static UIXTypeSchema Type;

        private static object GetKeyframes(object instanceObj) => (object)((AnimationTemplate)instanceObj).Keyframes;

        private static object Construct() => (object)new EffectAnimation();

        public static void Pass1Initialize() => EffectFloatAnimationSchema.Type = new UIXTypeSchema((short)75, "EffectFloatAnimation", (string)null, (short)70, typeof(EffectAnimation), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)75, "Keyframes", (short)138, (short)76, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(EffectFloatAnimationSchema.GetKeyframes), (SetValueHandler)null, false);
            EffectFloatAnimationSchema.Type.Initialize(new DefaultConstructHandler(EffectFloatAnimationSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
