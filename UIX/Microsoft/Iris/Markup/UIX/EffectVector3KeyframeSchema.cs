// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EffectVector3KeyframeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Render;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EffectVector3KeyframeSchema
    {
        public static UIXTypeSchema Type;

        private static object GetValue(object instanceObj) => (object)((BaseVector3Keyframe)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj) => ((BaseVector3Keyframe)instanceObj).Value = (Vector3)valueObj;

        private static object Construct() => (object)new EffectVector3Keyframe();

        public static void Pass1Initialize() => EffectVector3KeyframeSchema.Type = new UIXTypeSchema((short)82, "EffectVector3Keyframe", (string)null, (short)130, typeof(EffectVector3Keyframe), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)82, "Value", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(EffectVector3KeyframeSchema.GetValue), new SetValueHandler(EffectVector3KeyframeSchema.SetValue), false);
            EffectVector3KeyframeSchema.Type.Initialize(new DefaultConstructHandler(EffectVector3KeyframeSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
