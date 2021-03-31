// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ByteRangedValueSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ByteRangedValueSchema
    {
        public static UIXTypeSchema Type;

        private static object GetMinValue(object instanceObj) => (object)(byte)((IUIRangedValue)instanceObj).MinValue;

        private static void SetMinValue(ref object instanceObj, object valueObj)
        {
            IUIByteRangedValue uiByteRangedValue = (IUIByteRangedValue)instanceObj;
            byte num = (byte)valueObj;
            if ((double)num > (double)uiByteRangedValue.MaxValue)
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)num, (object)"MinValue");
            else
                uiByteRangedValue.MinValue = (float)num;
        }

        private static object GetMaxValue(object instanceObj) => (object)(byte)((IUIRangedValue)instanceObj).MaxValue;

        private static void SetMaxValue(ref object instanceObj, object valueObj)
        {
            IUIByteRangedValue uiByteRangedValue = (IUIByteRangedValue)instanceObj;
            byte num = (byte)valueObj;
            if ((double)num < (double)uiByteRangedValue.MinValue)
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)num, (object)"MaxValue");
            else
                uiByteRangedValue.MaxValue = (float)num;
        }

        private static object GetStep(object instanceObj) => (object)(byte)((IUIRangedValue)instanceObj).Step;

        private static void SetStep(ref object instanceObj, object valueObj) => ((IUIRangedValue)instanceObj).Step = (float)(byte)valueObj;

        private static object GetValue(object instanceObj) => (object)(byte)((IUIRangedValue)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj) => ((IUIRangedValue)instanceObj).Value = (float)(byte)valueObj;

        private static object Construct() => (object)new Microsoft.Iris.ModelItems.ByteRangedValue();

        public static void Pass1Initialize() => ByteRangedValueSchema.Type = new UIXTypeSchema((short)20, "ByteRangedValue", (string)null, (short)168, typeof(IUIByteRangedValue), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)20, "MinValue", (short)19, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ByteRangedValueSchema.GetMinValue), new SetValueHandler(ByteRangedValueSchema.SetMinValue), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)20, "MaxValue", (short)19, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ByteRangedValueSchema.GetMaxValue), new SetValueHandler(ByteRangedValueSchema.SetMaxValue), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)20, "Step", (short)19, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ByteRangedValueSchema.GetStep), new SetValueHandler(ByteRangedValueSchema.SetStep), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)20, "Value", (short)19, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ByteRangedValueSchema.GetValue), new SetValueHandler(ByteRangedValueSchema.SetValue), false);
            ByteRangedValueSchema.Type.Initialize(new DefaultConstructHandler(ByteRangedValueSchema.Construct), (ConstructorSchema[])null, new PropertySchema[4]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
