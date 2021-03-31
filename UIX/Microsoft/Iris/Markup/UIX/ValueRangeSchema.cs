// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ValueRangeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ValueRangeSchema
    {
        public static UIXTypeSchema Type;

        private static object GetObjectValue(object instanceObj) => ((IUIValueRange)instanceObj).ObjectValue;

        private static object GetHasPreviousValue(object instanceObj) => BooleanBoxes.Box(((IUIValueRange)instanceObj).HasPreviousValue);

        private static object GetHasNextValue(object instanceObj) => BooleanBoxes.Box(((IUIValueRange)instanceObj).HasNextValue);

        private static object CallPreviousValue(object instanceObj, object[] parameters)
        {
            ((IUIValueRange)instanceObj).PreviousValue();
            return (object)null;
        }

        private static object CallNextValue(object instanceObj, object[] parameters)
        {
            ((IUIValueRange)instanceObj).NextValue();
            return (object)null;
        }

        public static void Pass1Initialize() => ValueRangeSchema.Type = new UIXTypeSchema((short)231, "ValueRange", (string)null, (short)153, typeof(IUIValueRange), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)231, "ObjectValue", (short)153, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ValueRangeSchema.GetObjectValue), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)231, "HasPreviousValue", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ValueRangeSchema.GetHasPreviousValue), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)231, "HasNextValue", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ValueRangeSchema.GetHasNextValue), (SetValueHandler)null, false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)231, "PreviousValue", (short[])null, (short)240, new InvokeHandler(ValueRangeSchema.CallPreviousValue), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)231, "NextValue", (short[])null, (short)240, new InvokeHandler(ValueRangeSchema.CallNextValue), false);
            ValueRangeSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, new MethodSchema[2]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
