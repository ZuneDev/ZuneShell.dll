// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DestinationElementInstanceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Render;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DestinationElementInstanceSchema
    {
        public static UIXTypeSchema Type;

        private static void SetDownsample(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Downsample", (float)valueObj);

        private static void SetUVOffset(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("UVOffset", (Vector2)valueObj);

        private static object CallPlayDownsampleAnimationEffectFloatAnimation(
          object instanceObj,
          object[] parameters)
        {
            ((EffectElementWrapper)instanceObj).PlayAnimation(EffectProperty.Downsample, (EffectAnimation)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => DestinationElementInstanceSchema.Type = new UIXTypeSchema((short)57, "DestinationElementInstance", (string)null, (short)74, typeof(EffectElementWrapper), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)57, "Downsample", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(DestinationElementInstanceSchema.SetDownsample), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)57, "UVOffset", (short)233, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(DestinationElementInstanceSchema.SetUVOffset), false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)57, "PlayDownsampleAnimation", new short[1]
            {
        (short) 75
            }, (short)240, new InvokeHandler(DestinationElementInstanceSchema.CallPlayDownsampleAnimationEffectFloatAnimation), false);
            DestinationElementInstanceSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
