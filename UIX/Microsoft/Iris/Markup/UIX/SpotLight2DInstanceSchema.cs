// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.SpotLight2DInstanceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.Render;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class SpotLight2DInstanceSchema
    {
        public static UIXTypeSchema Type;

        private static void SetPosition(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Position", (Vector3)valueObj);

        private static void SetDirectionAngle(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("DirectionAngle", (float)valueObj);

        private static void SetLightColor(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("LightColor", (Color)valueObj);

        private static void SetAmbientColor(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("AmbientColor", (Color)valueObj);

        private static void SetInnerConeAngle(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("InnerConeAngle", (float)valueObj);

        private static void SetOuterConeAngle(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("OuterConeAngle", (float)valueObj);

        private static void SetIntensity(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Intensity", (float)valueObj);

        private static void SetAttenuation(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Attenuation", (Vector3)valueObj);

        private static object CallPlayPositionAnimationEffectVector3Animation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Position, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayDirectionAngleAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.DirectionAngle, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayLightColorAnimationEffectColorAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.LightColor, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayAmbientColorAnimationEffectColorAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.AmbientColor, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayInnerConeAngleAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.InnerConeAngle, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayOuterConeAngleAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.OuterConeAngle, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayIntensityAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Intensity, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayAttenuationAnimationEffectVector3Animation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Attenuation, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => SpotLight2DInstanceSchema.Type = new UIXTypeSchema((short)203, "SpotLight2DInstance", (string)null, (short)74, typeof(EffectElementWrapper), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)203, "Position", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(SpotLight2DInstanceSchema.SetPosition), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)203, "DirectionAngle", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(SpotLight2DInstanceSchema.SetDirectionAngle), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)203, "LightColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(SpotLight2DInstanceSchema.SetLightColor), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)203, "AmbientColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(SpotLight2DInstanceSchema.SetAmbientColor), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)203, "InnerConeAngle", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(SpotLight2DInstanceSchema.SetInnerConeAngle), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)203, "OuterConeAngle", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(SpotLight2DInstanceSchema.SetOuterConeAngle), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)203, "Intensity", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(SpotLight2DInstanceSchema.SetIntensity), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)203, "Attenuation", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(SpotLight2DInstanceSchema.SetAttenuation), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)203, "PlayPositionAnimation", new short[1]
            {
        (short) 81
            }, (short)240, new InvokeHandler(SpotLight2DInstanceSchema.CallPlayPositionAnimationEffectVector3Animation), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)203, "PlayDirectionAngleAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(SpotLight2DInstanceSchema.CallPlayDirectionAngleAnimationEffectFloatAnimation), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)203, "PlayLightColorAnimation", new short[1]
            {
        (short) 71
            }, (short)240, new InvokeHandler(SpotLight2DInstanceSchema.CallPlayLightColorAnimationEffectColorAnimation), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)203, "PlayAmbientColorAnimation", new short[1]
            {
        (short) 71
            }, (short)240, new InvokeHandler(SpotLight2DInstanceSchema.CallPlayAmbientColorAnimationEffectColorAnimation), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)203, "PlayInnerConeAngleAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(SpotLight2DInstanceSchema.CallPlayInnerConeAngleAnimationEffectFloatAnimation), false);
            UIXMethodSchema uixMethodSchema6 = new UIXMethodSchema((short)203, "PlayOuterConeAngleAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(SpotLight2DInstanceSchema.CallPlayOuterConeAngleAnimationEffectFloatAnimation), false);
            UIXMethodSchema uixMethodSchema7 = new UIXMethodSchema((short)203, "PlayIntensityAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(SpotLight2DInstanceSchema.CallPlayIntensityAnimationEffectFloatAnimation), false);
            UIXMethodSchema uixMethodSchema8 = new UIXMethodSchema((short)203, "PlayAttenuationAnimation", new short[1]
            {
        (short) 81
            }, (short)240, new InvokeHandler(SpotLight2DInstanceSchema.CallPlayAttenuationAnimationEffectVector3Animation), false);
            SpotLight2DInstanceSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[8]
            {
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema1
            }, new MethodSchema[8]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4,
        (MethodSchema) uixMethodSchema5,
        (MethodSchema) uixMethodSchema6,
        (MethodSchema) uixMethodSchema7,
        (MethodSchema) uixMethodSchema8
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
