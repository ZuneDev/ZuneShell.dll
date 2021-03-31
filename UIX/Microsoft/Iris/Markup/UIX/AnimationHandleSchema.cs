// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.AnimationHandleSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class AnimationHandleSchema
    {
        public static UIXTypeSchema Type;

        private static object GetPlaying(object instanceObj) => BooleanBoxes.Box(((AnimationHandle)instanceObj).Playing);

        private static object Construct() => (object)new AnimationHandle();

        public static void Pass1Initialize() => AnimationHandleSchema.Type = new UIXTypeSchema((short)11, "AnimationHandle", (string)null, (short)153, typeof(AnimationHandle), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)11, "Playing", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AnimationHandleSchema.GetPlaying), (SetValueHandler)null, false);
            UIXEventSchema uixEventSchema = new UIXEventSchema((short)11, "Completed");
            AnimationHandleSchema.Type.Initialize(new DefaultConstructHandler(AnimationHandleSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, new EventSchema[1]
            {
        (EventSchema) uixEventSchema
            }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
