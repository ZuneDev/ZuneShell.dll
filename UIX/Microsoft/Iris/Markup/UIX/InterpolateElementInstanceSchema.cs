// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.InterpolateElementInstanceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Library;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class InterpolateElementInstanceSchema
    {
        public static UIXTypeSchema Type;

        private static void SetValue(ref object instanceObj, object valueObj)
        {
            EffectElementWrapper effectElementWrapper = (EffectElementWrapper)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.Validate0to1(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                effectElementWrapper.SetProperty("Value", num);
        }

        private static object CallPlayValueAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Value, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => InterpolateElementInstanceSchema.Type = new UIXTypeSchema((short)120, "InterpolateElementInstance", (string)null, (short)74, typeof(EffectElementWrapper), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)120, "Value", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, false, (GetValueHandler)null, new SetValueHandler(InterpolateElementInstanceSchema.SetValue), false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)120, "PlayValueAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(InterpolateElementInstanceSchema.CallPlayValueAnimationEffectFloatAnimation), false);
            InterpolateElementInstanceSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
