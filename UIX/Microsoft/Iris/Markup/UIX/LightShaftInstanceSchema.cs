// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.LightShaftInstanceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Render;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class LightShaftInstanceSchema
    {
        public static UIXTypeSchema Type;

        private static void SetPosition(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Position", (Vector3)valueObj);

        private static void SetDecay(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Decay", (float)valueObj);

        private static void SetDensity(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Density", (float)valueObj);

        private static void SetFallOff(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("FallOff", (float)valueObj);

        private static void SetIntensity(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Intensity", (float)valueObj);

        private static void SetWeight(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Weight", (float)valueObj);

        private static object CallPlayPositionAnimationEffectVector3Animation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Position, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayDecayAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Decay, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayDensityAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Density, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayIntensityAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Intensity, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayFallOffAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.FallOff, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayWeightAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Weight, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => LightShaftInstanceSchema.Type = new UIXTypeSchema((short)136, "LightShaftInstance", (string)null, (short)74, typeof(EffectElementWrapper), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)136, "Position", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(LightShaftInstanceSchema.SetPosition), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)136, "Decay", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(LightShaftInstanceSchema.SetDecay), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)136, "Density", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(LightShaftInstanceSchema.SetDensity), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)136, "FallOff", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(LightShaftInstanceSchema.SetFallOff), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)136, "Intensity", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(LightShaftInstanceSchema.SetIntensity), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)136, "Weight", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(LightShaftInstanceSchema.SetWeight), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)136, "PlayPositionAnimation", new short[1]
            {
        (short) 81
            }, (short)240, new InvokeHandler(LightShaftInstanceSchema.CallPlayPositionAnimationEffectVector3Animation), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)136, "PlayDecayAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(LightShaftInstanceSchema.CallPlayDecayAnimationEffectFloatAnimation), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)136, "PlayDensityAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(LightShaftInstanceSchema.CallPlayDensityAnimationEffectFloatAnimation), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)136, "PlayIntensityAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(LightShaftInstanceSchema.CallPlayIntensityAnimationEffectFloatAnimation), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)136, "PlayFallOffAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(LightShaftInstanceSchema.CallPlayFallOffAnimationEffectFloatAnimation), false);
            UIXMethodSchema uixMethodSchema6 = new UIXMethodSchema((short)136, "PlayWeightAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(LightShaftInstanceSchema.CallPlayWeightAnimationEffectFloatAnimation), false);
            LightShaftInstanceSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[6]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema6
            }, new MethodSchema[6]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4,
        (MethodSchema) uixMethodSchema5,
        (MethodSchema) uixMethodSchema6
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
