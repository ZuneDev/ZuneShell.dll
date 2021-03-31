// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.RangedValueSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class RangedValueSchema
    {
        public static UIXTypeSchema Type;

        private static object GetMinValue(object instanceObj) => (object)((IUIRangedValue)instanceObj).MinValue;

        private static void SetMinValue(ref object instanceObj, object valueObj)
        {
            IUIRangedValue uiRangedValue = (IUIRangedValue)instanceObj;
            float num = (float)valueObj;
            if ((double)num > (double)uiRangedValue.MaxValue)
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)num, (object)"MinValue");
            else
                uiRangedValue.MinValue = num;
        }

        private static object GetMaxValue(object instanceObj) => (object)((IUIRangedValue)instanceObj).MaxValue;

        private static void SetMaxValue(ref object instanceObj, object valueObj)
        {
            IUIRangedValue uiRangedValue = (IUIRangedValue)instanceObj;
            float num = (float)valueObj;
            if ((double)num < (double)uiRangedValue.MinValue)
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)num, (object)"MaxValue");
            else
                uiRangedValue.MaxValue = num;
        }

        private static object GetStep(object instanceObj) => (object)((IUIRangedValue)instanceObj).Step;

        private static void SetStep(ref object instanceObj, object valueObj) => ((IUIRangedValue)instanceObj).Step = (float)valueObj;

        private static object GetRange(object instanceObj) => (object)((IUIRangedValue)instanceObj).Range;

        private static object GetValue(object instanceObj) => (object)((IUIRangedValue)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj) => ((IUIRangedValue)instanceObj).Value = (float)valueObj;

        private static object Construct() => (object)new Microsoft.Iris.ModelItems.RangedValue();

        public static void Pass1Initialize() => RangedValueSchema.Type = new UIXTypeSchema((short)168, "RangedValue", (string)null, (short)231, typeof(IUIRangedValue), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)168, "MinValue", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(RangedValueSchema.GetMinValue), new SetValueHandler(RangedValueSchema.SetMinValue), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)168, "MaxValue", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(RangedValueSchema.GetMaxValue), new SetValueHandler(RangedValueSchema.SetMaxValue), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)168, "Step", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(RangedValueSchema.GetStep), new SetValueHandler(RangedValueSchema.SetStep), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)168, "Range", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(RangedValueSchema.GetRange), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)168, "Value", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(RangedValueSchema.GetValue), new SetValueHandler(RangedValueSchema.SetValue), false);
            RangedValueSchema.Type.Initialize(new DefaultConstructHandler(RangedValueSchema.Construct), (ConstructorSchema[])null, new PropertySchema[5]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema5
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
