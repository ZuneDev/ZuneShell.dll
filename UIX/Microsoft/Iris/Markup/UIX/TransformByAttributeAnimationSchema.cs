// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.TransformByAttributeAnimationSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class TransformByAttributeAnimationSchema
    {
        public static UIXTypeSchema Type;

        private static object GetAttribute(object instanceObj) => (object)((TransformByAttributeAnimation)instanceObj).Attribute;

        private static void SetAttribute(ref object instanceObj, object valueObj) => ((TransformByAttributeAnimation)instanceObj).Attribute = (TransformAttribute)valueObj;

        private static object GetMaxTimeScale(object instanceObj) => (object)((TransformByAttributeAnimation)instanceObj).MaxTimeScale;

        private static void SetMaxTimeScale(ref object instanceObj, object valueObj) => ((TransformByAttributeAnimation)instanceObj).MaxTimeScale = (float)valueObj;

        private static object GetMaxDelay(object instanceObj) => (object)((TransformByAttributeAnimation)instanceObj).MaxDelay;

        private static void SetMaxDelay(ref object instanceObj, object valueObj) => ((TransformByAttributeAnimation)instanceObj).MaxDelay = (float)valueObj;

        private static object GetMaxMagnitude(object instanceObj) => (object)((TransformByAttributeAnimation)instanceObj).MaxMagnitude;

        private static void SetMaxMagnitude(ref object instanceObj, object valueObj) => ((TransformByAttributeAnimation)instanceObj).MaxMagnitude = (float)valueObj;

        private static object GetOverride(object instanceObj) => (object)((TransformByAttributeAnimation)instanceObj).Override;

        private static void SetOverride(ref object instanceObj, object valueObj) => ((TransformByAttributeAnimation)instanceObj).Override = (float)valueObj;

        private static object GetValueTransformer(object instanceObj) => (object)((TransformByAttributeAnimation)instanceObj).ValueTransformer;

        private static void SetValueTransformer(ref object instanceObj, object valueObj) => ((TransformByAttributeAnimation)instanceObj).ValueTransformer = (ValueTransformer)valueObj;

        private static object Construct() => (object)new TransformByAttributeAnimation();

        public static void Pass1Initialize() => TransformByAttributeAnimationSchema.Type = new UIXTypeSchema((short)224, "TransformByAttributeAnimation", (string)null, (short)222, typeof(TransformByAttributeAnimation), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)224, "Attribute", (short)223, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TransformByAttributeAnimationSchema.GetAttribute), new SetValueHandler(TransformByAttributeAnimationSchema.SetAttribute), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)224, "MaxTimeScale", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TransformByAttributeAnimationSchema.GetMaxTimeScale), new SetValueHandler(TransformByAttributeAnimationSchema.SetMaxTimeScale), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)224, "MaxDelay", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TransformByAttributeAnimationSchema.GetMaxDelay), new SetValueHandler(TransformByAttributeAnimationSchema.SetMaxDelay), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)224, "MaxMagnitude", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TransformByAttributeAnimationSchema.GetMaxMagnitude), new SetValueHandler(TransformByAttributeAnimationSchema.SetMaxMagnitude), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)224, "Override", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TransformByAttributeAnimationSchema.GetOverride), new SetValueHandler(TransformByAttributeAnimationSchema.SetOverride), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)224, "ValueTransformer", (short)232, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TransformByAttributeAnimationSchema.GetValueTransformer), new SetValueHandler(TransformByAttributeAnimationSchema.SetValueTransformer), false);
            TransformByAttributeAnimationSchema.Type.Initialize(new DefaultConstructHandler(TransformByAttributeAnimationSchema.Construct), (ConstructorSchema[])null, new PropertySchema[6]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema6
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
