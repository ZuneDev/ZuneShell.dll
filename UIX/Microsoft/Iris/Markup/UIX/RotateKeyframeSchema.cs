// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.RotateKeyframeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Drawing;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class RotateKeyframeSchema
    {
        public static UIXTypeSchema Type;

        private static object GetValue(object instanceObj) => (object)((BaseRotationKeyframe)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj) => ((BaseRotationKeyframe)instanceObj).Value = (Rotation)valueObj;

        private static object Construct() => (object)new RotateKeyframe();

        public static void Pass1Initialize() => RotateKeyframeSchema.Type = new UIXTypeSchema((short)174, "RotateKeyframe", (string)null, (short)130, typeof(RotateKeyframe), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)174, "Value", (short)176, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RotateKeyframeSchema.GetValue), new SetValueHandler(RotateKeyframeSchema.SetValue), false);
            RotateKeyframeSchema.Type.Initialize(new DefaultConstructHandler(RotateKeyframeSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
