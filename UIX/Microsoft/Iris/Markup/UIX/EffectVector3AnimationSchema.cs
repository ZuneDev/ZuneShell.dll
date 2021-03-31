// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EffectVector3AnimationSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EffectVector3AnimationSchema
    {
        public static UIXTypeSchema Type;

        private static object GetKeyframes(object instanceObj) => (object)((AnimationTemplate)instanceObj).Keyframes;

        private static object Construct() => (object)new EffectAnimation();

        public static void Pass1Initialize() => EffectVector3AnimationSchema.Type = new UIXTypeSchema((short)81, "EffectVector3Animation", (string)null, (short)70, typeof(EffectAnimation), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)81, "Keyframes", (short)138, (short)82, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(EffectVector3AnimationSchema.GetKeyframes), (SetValueHandler)null, false);
            EffectVector3AnimationSchema.Type.Initialize(new DefaultConstructHandler(EffectVector3AnimationSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
