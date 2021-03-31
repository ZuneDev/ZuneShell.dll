// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.SepiaInstanceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class SepiaInstanceSchema
    {
        public static UIXTypeSchema Type;

        private static void SetLightColor(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Color", (Color)valueObj);

        private static void SetDarkColor(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Color", (Color)valueObj);

        private static void SetDesaturate(ref object instanceObj, object valueObj)
        {
            EffectElementWrapper effectElementWrapper = (EffectElementWrapper)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                effectElementWrapper.SetProperty("Desaturate", num);
        }

        private static void SetTone(ref object instanceObj, object valueObj)
        {
            EffectElementWrapper effectElementWrapper = (EffectElementWrapper)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                effectElementWrapper.SetProperty("Tone", num);
        }

        private static object CallPlayLightColorAnimationEffectColorAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.LightColor, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayDarkColorAnimationEffectColorAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.DarkColor, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayDesaturateAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Desaturate, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayToneAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Tone, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => SepiaInstanceSchema.Type = new UIXTypeSchema((short)189, "SepiaInstance", (string)null, (short)74, typeof(EffectElementWrapper), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)189, "LightColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(SepiaInstanceSchema.SetLightColor), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)189, "DarkColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(SepiaInstanceSchema.SetDarkColor), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)189, "Desaturate", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, (GetValueHandler)null, new SetValueHandler(SepiaInstanceSchema.SetDesaturate), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)189, "Tone", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, (GetValueHandler)null, new SetValueHandler(SepiaInstanceSchema.SetTone), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)189, "PlayLightColorAnimation", new short[1]
            {
        (short) 71
            }, (short)240, new InvokeHandler(SepiaInstanceSchema.CallPlayLightColorAnimationEffectColorAnimation), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)189, "PlayDarkColorAnimation", new short[1]
            {
        (short) 71
            }, (short)240, new InvokeHandler(SepiaInstanceSchema.CallPlayDarkColorAnimationEffectColorAnimation), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)189, "PlayDesaturateAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(SepiaInstanceSchema.CallPlayDesaturateAnimationEffectFloatAnimation), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)189, "PlayToneAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(SepiaInstanceSchema.CallPlayToneAnimationEffectFloatAnimation), false);
            SepiaInstanceSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[4]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema4
            }, new MethodSchema[4]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
