// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.TransformAnimationSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class TransformAnimationSchema
    {
        public static UIXTypeSchema Type;

        private static object GetDelay(object instanceObj) => (object)((TransformAnimation)instanceObj).Delay;

        private static void SetDelay(ref object instanceObj, object valueObj) => ((TransformAnimation)instanceObj).Delay = (float)valueObj;

        private static object GetFilter(object instanceObj) => (object)((TransformAnimation)instanceObj).Filter;

        private static void SetFilter(ref object instanceObj, object valueObj) => ((TransformAnimation)instanceObj).Filter = (KeyframeFilter)valueObj;

        private static object GetMagnitude(object instanceObj) => (object)((TransformAnimation)instanceObj).Magnitude;

        private static void SetMagnitude(ref object instanceObj, object valueObj) => ((TransformAnimation)instanceObj).Magnitude = (float)valueObj;

        private static object GetTimeScale(object instanceObj) => (object)((TransformAnimation)instanceObj).TimeScale;

        private static void SetTimeScale(ref object instanceObj, object valueObj) => ((TransformAnimation)instanceObj).TimeScale = (float)valueObj;

        private static object GetSource(object instanceObj) => (object)((ReferenceAnimation)instanceObj).Source;

        private static void SetSource(ref object instanceObj, object valueObj) => ((ReferenceAnimation)instanceObj).Source = (IAnimationProvider)valueObj;

        private static object GetType(object instanceObj) => (object)((ReferenceAnimation)instanceObj).Type;

        private static object Construct() => (object)new TransformAnimation();

        public static void Pass1Initialize() => TransformAnimationSchema.Type = new UIXTypeSchema((short)222, "TransformAnimation", (string)null, (short)104, typeof(TransformAnimation), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)222, "Delay", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TransformAnimationSchema.GetDelay), new SetValueHandler(TransformAnimationSchema.SetDelay), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)222, "Filter", (short)131, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TransformAnimationSchema.GetFilter), new SetValueHandler(TransformAnimationSchema.SetFilter), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)222, "Magnitude", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TransformAnimationSchema.GetMagnitude), new SetValueHandler(TransformAnimationSchema.SetMagnitude), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)222, "TimeScale", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TransformAnimationSchema.GetTimeScale), new SetValueHandler(TransformAnimationSchema.SetTimeScale), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)222, "Source", (short)104, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TransformAnimationSchema.GetSource), new SetValueHandler(TransformAnimationSchema.SetSource), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)222, "Type", (short)10, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TransformAnimationSchema.GetType), (SetValueHandler)null, false);
            TransformAnimationSchema.Type.Initialize(new DefaultConstructHandler(TransformAnimationSchema.Construct), (ConstructorSchema[])null, new PropertySchema[6]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema6
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
