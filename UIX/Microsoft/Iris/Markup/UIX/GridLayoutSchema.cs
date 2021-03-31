// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.GridLayoutSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class GridLayoutSchema
    {
        public static UIXTypeSchema Type;

        private static object GetOrientation(object instanceObj) => (object)((GridLayout)instanceObj).Orientation;

        private static void SetOrientation(ref object instanceObj, object valueObj) => ((GridLayout)instanceObj).Orientation = (Orientation)valueObj;

        private static object GetAllowWrap(object instanceObj) => BooleanBoxes.Box(((GridLayout)instanceObj).AllowWrap);

        private static void SetAllowWrap(ref object instanceObj, object valueObj) => ((GridLayout)instanceObj).AllowWrap = (bool)valueObj;

        private static object GetReferenceSize(object instanceObj) => (object)((GridLayout)instanceObj).ReferenceSize;

        private static void SetReferenceSize(ref object instanceObj, object valueObj) => ((GridLayout)instanceObj).ReferenceSize = (Size)valueObj;

        private static object GetSpacing(object instanceObj) => (object)((GridLayout)instanceObj).Spacing;

        private static void SetSpacing(ref object instanceObj, object valueObj) => ((GridLayout)instanceObj).Spacing = (Size)valueObj;

        private static object GetRows(object instanceObj) => (object)((GridLayout)instanceObj).Rows;

        private static void SetRows(ref object instanceObj, object valueObj)
        {
            GridLayout gridLayout = (GridLayout)instanceObj;
            int num = (int)valueObj;
            Result result = Int32Schema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                gridLayout.Rows = num;
        }

        private static object GetColumns(object instanceObj) => (object)((GridLayout)instanceObj).Columns;

        private static void SetColumns(ref object instanceObj, object valueObj)
        {
            GridLayout gridLayout = (GridLayout)instanceObj;
            int num = (int)valueObj;
            Result result = Int32Schema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                gridLayout.Columns = num;
        }

        private static object GetRepeat(object instanceObj) => (object)((GridLayout)instanceObj).Repeat;

        private static void SetRepeat(ref object instanceObj, object valueObj) => ((GridLayout)instanceObj).Repeat = (RepeatPolicy)valueObj;

        private static object GetRepeatGap(object instanceObj) => (object)((GridLayout)instanceObj).RepeatGap;

        private static void SetRepeatGap(ref object instanceObj, object valueObj) => ((GridLayout)instanceObj).RepeatGap = (int)valueObj;

        private static object GetDefaultChildAlignment(object instanceObj) => (object)((GridLayout)instanceObj).DefaultChildAlignment;

        private static void SetDefaultChildAlignment(ref object instanceObj, object valueObj) => ((GridLayout)instanceObj).DefaultChildAlignment = (ItemAlignment)valueObj;

        private static object Construct() => (object)new GridLayout();

        public static void Pass1Initialize() => GridLayoutSchema.Type = new UIXTypeSchema((short)99, "GridLayout", (string)null, (short)132, typeof(GridLayout), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)99, "Orientation", (short)154, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(GridLayoutSchema.GetOrientation), new SetValueHandler(GridLayoutSchema.SetOrientation), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)99, "AllowWrap", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(GridLayoutSchema.GetAllowWrap), new SetValueHandler(GridLayoutSchema.SetAllowWrap), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)99, "ReferenceSize", (short)195, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(GridLayoutSchema.GetReferenceSize), new SetValueHandler(GridLayoutSchema.SetReferenceSize), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)99, "Spacing", (short)195, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(GridLayoutSchema.GetSpacing), new SetValueHandler(GridLayoutSchema.SetSpacing), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)99, "Rows", (short)115, (short)-1, ExpressionRestriction.None, false, Int32Schema.ValidateNotNegative, false, new GetValueHandler(GridLayoutSchema.GetRows), new SetValueHandler(GridLayoutSchema.SetRows), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)99, "Columns", (short)115, (short)-1, ExpressionRestriction.None, false, Int32Schema.ValidateNotNegative, false, new GetValueHandler(GridLayoutSchema.GetColumns), new SetValueHandler(GridLayoutSchema.SetColumns), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)99, "Repeat", (short)172, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(GridLayoutSchema.GetRepeat), new SetValueHandler(GridLayoutSchema.SetRepeat), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)99, "RepeatGap", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(GridLayoutSchema.GetRepeatGap), new SetValueHandler(GridLayoutSchema.SetRepeatGap), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)99, "DefaultChildAlignment", (short)sbyte.MaxValue, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(GridLayoutSchema.GetDefaultChildAlignment), new SetValueHandler(GridLayoutSchema.SetDefaultChildAlignment), false);
            GridLayoutSchema.Type.Initialize(new DefaultConstructHandler(GridLayoutSchema.Construct), (ConstructorSchema[])null, new PropertySchema[9]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema4
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
