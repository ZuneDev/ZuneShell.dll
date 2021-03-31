// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.IntRangedValueSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class IntRangedValueSchema
    {
        public static UIXTypeSchema Type;

        private static object GetMinValue(object instanceObj) => (object)(int)((IUIRangedValue)instanceObj).MinValue;

        private static void SetMinValue(ref object instanceObj, object valueObj)
        {
            IUIIntRangedValue uiIntRangedValue = (IUIIntRangedValue)instanceObj;
            int num = (int)valueObj;
            if ((double)num > (double)uiIntRangedValue.MaxValue)
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)num, (object)"MinValue");
            else
                uiIntRangedValue.MinValue = (float)num;
        }

        private static object GetMaxValue(object instanceObj) => (object)(int)((IUIRangedValue)instanceObj).MaxValue;

        private static void SetMaxValue(ref object instanceObj, object valueObj)
        {
            IUIIntRangedValue uiIntRangedValue = (IUIIntRangedValue)instanceObj;
            int num = (int)valueObj;
            if ((double)num < (double)uiIntRangedValue.MinValue)
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)num, (object)"MaxValue");
            else
                uiIntRangedValue.MaxValue = (float)num;
        }

        private static object GetStep(object instanceObj) => (object)(int)((IUIRangedValue)instanceObj).Step;

        private static void SetStep(ref object instanceObj, object valueObj) => ((IUIRangedValue)instanceObj).Step = (float)(int)valueObj;

        private static object GetValue(object instanceObj) => (object)(int)((IUIRangedValue)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj) => ((IUIRangedValue)instanceObj).Value = (float)(int)valueObj;

        private static object Construct() => (object)new Microsoft.Iris.ModelItems.IntRangedValue();

        public static void Pass1Initialize() => IntRangedValueSchema.Type = new UIXTypeSchema((short)117, "IntRangedValue", (string)null, (short)168, typeof(IUIIntRangedValue), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)117, "MinValue", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(IntRangedValueSchema.GetMinValue), new SetValueHandler(IntRangedValueSchema.SetMinValue), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)117, "MaxValue", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(IntRangedValueSchema.GetMaxValue), new SetValueHandler(IntRangedValueSchema.SetMaxValue), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)117, "Step", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(IntRangedValueSchema.GetStep), new SetValueHandler(IntRangedValueSchema.SetStep), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)117, "Value", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(IntRangedValueSchema.GetValue), new SetValueHandler(IntRangedValueSchema.SetValue), false);
            IntRangedValueSchema.Type.Initialize(new DefaultConstructHandler(IntRangedValueSchema.Construct), (ConstructorSchema[])null, new PropertySchema[4]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
