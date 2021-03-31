// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EffectAnimationSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EffectAnimationSchema
    {
        public static UIXTypeSchema Type;

        private static object GetLoop(object instanceObj) => (object)((AnimationTemplate)instanceObj).Loop;

        private static void SetLoop(ref object instanceObj, object valueObj) => ((AnimationTemplate)instanceObj).Loop = (int)valueObj;

        public static void Pass1Initialize() => EffectAnimationSchema.Type = new UIXTypeSchema((short)70, "EffectAnimation", (string)null, (short)153, typeof(EffectAnimation), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)70, "Loop", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(EffectAnimationSchema.GetLoop), new SetValueHandler(EffectAnimationSchema.SetLoop), false);
            EffectAnimationSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
