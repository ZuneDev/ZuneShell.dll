// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.CameraAtKeyframeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Render;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class CameraAtKeyframeSchema
    {
        public static UIXTypeSchema Type;

        private static object GetValue(object instanceObj) => (object)((BaseVector3Keyframe)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj) => ((BaseVector3Keyframe)instanceObj).Value = (Vector3)valueObj;

        private static object Construct() => (object)new CameraAtKeyframe();

        public static void Pass1Initialize() => CameraAtKeyframeSchema.Type = new UIXTypeSchema((short)22, "CameraAtKeyframe", (string)null, (short)130, typeof(CameraAtKeyframe), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)22, "Value", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(CameraAtKeyframeSchema.GetValue), new SetValueHandler(CameraAtKeyframeSchema.SetValue), false);
            CameraAtKeyframeSchema.Type.Initialize(new DefaultConstructHandler(CameraAtKeyframeSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
