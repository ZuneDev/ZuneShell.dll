// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.BlendSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class BlendSchema
    {
        public static UIXTypeSchema Type;

        private static object GetInput1(object instanceObj) => (object)((BlendElement)instanceObj).Input1;

        private static void SetInput1(ref object instanceObj, object valueObj) => ((BlendElement)instanceObj).Input1 = (EffectInput)valueObj;

        private static object GetInput2(object instanceObj) => (object)((BlendElement)instanceObj).Input2;

        private static void SetInput2(ref object instanceObj, object valueObj) => ((BlendElement)instanceObj).Input2 = (EffectInput)valueObj;

        private static object GetColorOperation(object instanceObj) => (object)((BlendElement)instanceObj).ColorOperation;

        private static void SetColorOperation(ref object instanceObj, object valueObj) => ((BlendElement)instanceObj).ColorOperation = (ColorOperation)valueObj;

        private static object GetAlphaOperation(object instanceObj) => (object)((BlendElement)instanceObj).AlphaOperation;

        private static void SetAlphaOperation(ref object instanceObj, object valueObj) => ((BlendElement)instanceObj).AlphaOperation = (AlphaOperation)valueObj;

        private static object Construct() => (object)new BlendElement();

        public static void Pass1Initialize() => BlendSchema.Type = new UIXTypeSchema((short)13, "Blend", (string)null, (short)77, typeof(BlendElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)13, "Input1", (short)77, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(BlendSchema.GetInput1), new SetValueHandler(BlendSchema.SetInput1), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)13, "Input2", (short)77, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(BlendSchema.GetInput2), new SetValueHandler(BlendSchema.SetInput2), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)13, "ColorOperation", (short)38, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(BlendSchema.GetColorOperation), new SetValueHandler(BlendSchema.SetColorOperation), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)13, "AlphaOperation", (short)5, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(BlendSchema.GetAlphaOperation), new SetValueHandler(BlendSchema.SetAlphaOperation), false);
            BlendSchema.Type.Initialize(new DefaultConstructHandler(BlendSchema.Construct), (ConstructorSchema[])null, new PropertySchema[4]
            {
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
