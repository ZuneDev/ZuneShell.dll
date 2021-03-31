// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.InterpolateElementSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class InterpolateElementSchema
    {
        public static UIXTypeSchema Type;

        private static object GetInput1(object instanceObj) => (object)((InterpolateElement)instanceObj).Input1;

        private static void SetInput1(ref object instanceObj, object valueObj) => ((InterpolateElement)instanceObj).Input1 = (EffectInput)valueObj;

        private static object GetInput2(object instanceObj) => (object)((InterpolateElement)instanceObj).Input2;

        private static void SetInput2(ref object instanceObj, object valueObj) => ((InterpolateElement)instanceObj).Input2 = (EffectInput)valueObj;

        private static object GetValue(object instanceObj) => (object)((InterpolateElement)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj)
        {
            InterpolateElement interpolateElement = (InterpolateElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.Validate0to1(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                interpolateElement.Value = num;
        }

        private static object Construct() => (object)new InterpolateElement();

        public static void Pass1Initialize() => InterpolateElementSchema.Type = new UIXTypeSchema((short)119, "InterpolateElement", (string)null, (short)77, typeof(InterpolateElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)119, "Input1", (short)77, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(InterpolateElementSchema.GetInput1), new SetValueHandler(InterpolateElementSchema.SetInput1), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)119, "Input2", (short)77, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(InterpolateElementSchema.GetInput2), new SetValueHandler(InterpolateElementSchema.SetInput2), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)119, "Value", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, false, new GetValueHandler(InterpolateElementSchema.GetValue), new SetValueHandler(InterpolateElementSchema.SetValue), false);
            InterpolateElementSchema.Type.Initialize(new DefaultConstructHandler(InterpolateElementSchema.Construct), (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
