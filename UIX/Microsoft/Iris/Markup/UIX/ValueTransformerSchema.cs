// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ValueTransformerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Library;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ValueTransformerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetAdd(object instanceObj) => (object)((ValueTransformer)instanceObj).Add;

        private static void SetAdd(ref object instanceObj, object valueObj) => ((ValueTransformer)instanceObj).Add = (float)valueObj;

        private static object GetSubtract(object instanceObj) => (object)((ValueTransformer)instanceObj).Subtract;

        private static void SetSubtract(ref object instanceObj, object valueObj) => ((ValueTransformer)instanceObj).Subtract = (float)valueObj;

        private static object GetMultiply(object instanceObj) => (object)((ValueTransformer)instanceObj).Multiply;

        private static void SetMultiply(ref object instanceObj, object valueObj) => ((ValueTransformer)instanceObj).Multiply = (float)valueObj;

        private static object GetDivide(object instanceObj) => (object)((ValueTransformer)instanceObj).Divide;

        private static void SetDivide(ref object instanceObj, object valueObj)
        {
            ValueTransformer valueTransformer = (ValueTransformer)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotZero(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                valueTransformer.Divide = num;
        }

        private static object GetMod(object instanceObj) => (object)((ValueTransformer)instanceObj).Mod;

        private static void SetMod(ref object instanceObj, object valueObj)
        {
            ValueTransformer valueTransformer = (ValueTransformer)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotZero(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                valueTransformer.Mod = num;
        }

        private static object GetAbsolute(object instanceObj) => BooleanBoxes.Box(((ValueTransformer)instanceObj).Absolute);

        private static void SetAbsolute(ref object instanceObj, object valueObj) => ((ValueTransformer)instanceObj).Absolute = (bool)valueObj;

        private static object Construct() => (object)new ValueTransformer();

        public static void Pass1Initialize() => ValueTransformerSchema.Type = new UIXTypeSchema((short)232, "ValueTransformer", (string)null, (short)153, typeof(ValueTransformer), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)232, "Add", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ValueTransformerSchema.GetAdd), new SetValueHandler(ValueTransformerSchema.SetAdd), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)232, "Subtract", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ValueTransformerSchema.GetSubtract), new SetValueHandler(ValueTransformerSchema.SetSubtract), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)232, "Multiply", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ValueTransformerSchema.GetMultiply), new SetValueHandler(ValueTransformerSchema.SetMultiply), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)232, "Divide", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotZero, false, new GetValueHandler(ValueTransformerSchema.GetDivide), new SetValueHandler(ValueTransformerSchema.SetDivide), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)232, "Mod", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotZero, false, new GetValueHandler(ValueTransformerSchema.GetMod), new SetValueHandler(ValueTransformerSchema.SetMod), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)232, "Absolute", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ValueTransformerSchema.GetAbsolute), new SetValueHandler(ValueTransformerSchema.SetAbsolute), false);
            ValueTransformerSchema.Type.Initialize(new DefaultConstructHandler(ValueTransformerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[6]
            {
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
