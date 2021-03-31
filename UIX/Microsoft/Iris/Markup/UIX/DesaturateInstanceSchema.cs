// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DesaturateInstanceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Library;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DesaturateInstanceSchema
    {
        public static UIXTypeSchema Type;

        private static void SetDesaturate(ref object instanceObj, object valueObj)
        {
            EffectElementWrapper effectElementWrapper = (EffectElementWrapper)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.Validate0to1(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                effectElementWrapper.SetProperty("Desaturate", num);
        }

        private static object CallPlayDesaturateAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Desaturate, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => DesaturateInstanceSchema.Type = new UIXTypeSchema((short)55, "DesaturateInstance", (string)null, (short)74, typeof(EffectElementWrapper), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)55, "Desaturate", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, false, (GetValueHandler)null, new SetValueHandler(DesaturateInstanceSchema.SetDesaturate), false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)55, "PlayDesaturateAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(DesaturateInstanceSchema.CallPlayDesaturateAnimationEffectFloatAnimation), false);
            DesaturateInstanceSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
