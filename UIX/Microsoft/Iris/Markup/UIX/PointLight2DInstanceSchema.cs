// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.PointLight2DInstanceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.Render;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class PointLight2DInstanceSchema
    {
        public static UIXTypeSchema Type;

        private static void SetPosition(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Position", (Vector3)valueObj);

        private static void SetRadius(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Radius", (float)valueObj);

        private static void SetLightColor(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("LightColor", (Color)valueObj);

        private static void SetAmbientColor(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("AmbientColor", (Color)valueObj);

        private static void SetAttenuation(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Attenuation", (Vector3)valueObj);

        private static object CallPlayPositionAnimationEffectVector3Animation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Position, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        private static object CallPlayRadiusAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Radius, (EffectAnimation)parameters[0]);
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

        private static object CallPlayAttenuationAnimationEffectVector3Animation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Attenuation, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => PointLight2DInstanceSchema.Type = new UIXTypeSchema((short)160, "PointLight2DInstance", (string)null, (short)74, typeof(EffectElementWrapper), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)160, "Position", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(PointLight2DInstanceSchema.SetPosition), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)160, "Radius", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(PointLight2DInstanceSchema.SetRadius), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)160, "LightColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(PointLight2DInstanceSchema.SetLightColor), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)160, "AmbientColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(PointLight2DInstanceSchema.SetAmbientColor), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)160, "Attenuation", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(PointLight2DInstanceSchema.SetAttenuation), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)160, "PlayPositionAnimation", new short[1]
            {
        (short) 81
            }, (short)240, new InvokeHandler(PointLight2DInstanceSchema.CallPlayPositionAnimationEffectVector3Animation), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)160, "PlayRadiusAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(PointLight2DInstanceSchema.CallPlayRadiusAnimationEffectFloatAnimation), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)160, "PlayLightColorAnimation", new short[1]
            {
        (short) 71
            }, (short)240, new InvokeHandler(PointLight2DInstanceSchema.CallPlayLightColorAnimationEffectColorAnimation), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)160, "PlayAmbientColorAnimation", new short[1]
            {
        (short) 71
            }, (short)240, new InvokeHandler(PointLight2DInstanceSchema.CallPlayAmbientColorAnimationEffectColorAnimation), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)160, "PlayAttenuationAnimation", new short[1]
            {
        (short) 81
            }, (short)240, new InvokeHandler(PointLight2DInstanceSchema.CallPlayAttenuationAnimationEffectVector3Animation), false);
            PointLight2DInstanceSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[5]
            {
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[5]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4,
        (MethodSchema) uixMethodSchema5
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
