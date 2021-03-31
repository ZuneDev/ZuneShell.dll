// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.FlowLayoutSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.Render;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class FlowLayoutSchema
    {
        public static UIXTypeSchema Type;

        private static object GetOrientation(object instanceObj) => (object)((FlowLayout)instanceObj).Orientation;

        private static void SetOrientation(ref object instanceObj, object valueObj) => ((FlowLayout)instanceObj).Orientation = (Orientation)valueObj;

        private static object GetSpacing(object instanceObj) => (object)((FlowLayout)instanceObj).Spacing;

        private static void SetSpacing(ref object instanceObj, object valueObj) => ((FlowLayout)instanceObj).Spacing = (MajorMinor)valueObj;

        private static object GetAllowWrap(object instanceObj) => BooleanBoxes.Box(((FlowLayout)instanceObj).AllowWrap);

        private static void SetAllowWrap(ref object instanceObj, object valueObj) => ((FlowLayout)instanceObj).AllowWrap = (bool)valueObj;

        private static object GetStripAlignment(object instanceObj) => (object)((FlowLayout)instanceObj).StripAlignment;

        private static void SetStripAlignment(ref object instanceObj, object valueObj) => ((FlowLayout)instanceObj).StripAlignment = (StripAlignment)valueObj;

        private static object GetRepeat(object instanceObj) => (object)((FlowLayout)instanceObj).Repeat;

        private static void SetRepeat(ref object instanceObj, object valueObj) => ((FlowLayout)instanceObj).Repeat = (RepeatPolicy)valueObj;

        private static object GetRepeatGap(object instanceObj) => (object)((FlowLayout)instanceObj).RepeatGap;

        private static void SetRepeatGap(ref object instanceObj, object valueObj) => ((FlowLayout)instanceObj).RepeatGap = (MajorMinor)valueObj;

        private static object GetMissingItemPolicy(object instanceObj) => (object)((FlowLayout)instanceObj).MissingItemPolicy;

        private static void SetMissingItemPolicy(ref object instanceObj, object valueObj) => ((FlowLayout)instanceObj).MissingItemPolicy = (MissingItemPolicy)valueObj;

        private static object GetMinimumSampleSize(object instanceObj) => (object)((FlowLayout)instanceObj).MinimumSampleSize;

        private static void SetMinimumSampleSize(ref object instanceObj, object valueObj) => ((FlowLayout)instanceObj).MinimumSampleSize = (int)valueObj;

        private static object GetDefaultChildAlignment(object instanceObj) => (object)((FlowLayout)instanceObj).DefaultChildAlignment;

        private static void SetDefaultChildAlignment(ref object instanceObj, object valueObj) => ((FlowLayout)instanceObj).DefaultChildAlignment = (ItemAlignment)valueObj;

        private static object Construct() => (object)new FlowLayout();

        public static void Pass1Initialize() => FlowLayoutSchema.Type = new UIXTypeSchema((short)90, "FlowLayout", (string)null, (short)132, typeof(FlowLayout), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)90, "Orientation", (short)154, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(FlowLayoutSchema.GetOrientation), new SetValueHandler(FlowLayoutSchema.SetOrientation), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)90, "Spacing", (short)139, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(FlowLayoutSchema.GetSpacing), new SetValueHandler(FlowLayoutSchema.SetSpacing), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)90, "AllowWrap", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(FlowLayoutSchema.GetAllowWrap), new SetValueHandler(FlowLayoutSchema.SetAllowWrap), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)90, "StripAlignment", (short)209, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(FlowLayoutSchema.GetStripAlignment), new SetValueHandler(FlowLayoutSchema.SetStripAlignment), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)90, "Repeat", (short)172, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(FlowLayoutSchema.GetRepeat), new SetValueHandler(FlowLayoutSchema.SetRepeat), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)90, "RepeatGap", (short)139, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(FlowLayoutSchema.GetRepeatGap), new SetValueHandler(FlowLayoutSchema.SetRepeatGap), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)90, "MissingItemPolicy", (short)148, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(FlowLayoutSchema.GetMissingItemPolicy), new SetValueHandler(FlowLayoutSchema.SetMissingItemPolicy), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)90, "MinimumSampleSize", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(FlowLayoutSchema.GetMinimumSampleSize), new SetValueHandler(FlowLayoutSchema.SetMinimumSampleSize), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)90, "DefaultChildAlignment", (short)sbyte.MaxValue, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(FlowLayoutSchema.GetDefaultChildAlignment), new SetValueHandler(FlowLayoutSchema.SetDefaultChildAlignment), false);
            FlowLayoutSchema.Type.Initialize(new DefaultConstructHandler(FlowLayoutSchema.Construct), (ConstructorSchema[])null, new PropertySchema[9]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema4
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
