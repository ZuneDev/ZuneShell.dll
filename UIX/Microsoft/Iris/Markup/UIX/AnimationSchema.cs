// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.AnimationSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class AnimationSchema
    {
        public static RangeValidator ValidateLoopValue = new RangeValidator(AnimationSchema.RangeValidateLoopValue);
        public static UIXTypeSchema Type;

        private static object GetCenterPointPercent(object instanceObj) => (object)((Animation)instanceObj).CenterPointPercent;

        private static void SetCenterPointPercent(ref object instanceObj, object valueObj) => ((Animation)instanceObj).CenterPointPercent = (Vector3)valueObj;

        private static object GetDisableMouseInput(object instanceObj) => BooleanBoxes.Box(((Animation)instanceObj).DisableMouseInput);

        private static void SetDisableMouseInput(ref object instanceObj, object valueObj) => ((Animation)instanceObj).DisableMouseInput = (bool)valueObj;

        private static object GetKeyframes(object instanceObj) => (object)((AnimationTemplate)instanceObj).Keyframes;

        private static object GetLoop(object instanceObj) => (object)((AnimationTemplate)instanceObj).Loop;

        private static void SetLoop(ref object instanceObj, object valueObj)
        {
            Animation animation = (Animation)instanceObj;
            int num = (int)valueObj;
            Result result = AnimationSchema.ValidateLoopValue(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                animation.Loop = num;
        }

        private static object GetRotationAxis(object instanceObj) => (object)((Animation)instanceObj).RotationAxis;

        private static void SetRotationAxis(ref object instanceObj, object valueObj) => ((Animation)instanceObj).RotationAxis = (Vector3)valueObj;

        private static object GetType(object instanceObj) => (object)((Animation)instanceObj).Type;

        private static void SetType(ref object instanceObj, object valueObj) => ((Animation)instanceObj).Type = (AnimationEventType)valueObj;

        private static object Construct() => (object)new Animation();

        private static Result RangeValidateLoopValue(object value)
        {
            int num = (int)value;
            return num < -1 ? Result.Fail("Expecting a value no smaller than {0}, but got {1}", (object)"-1", (object)num.ToString()) : Result.Success;
        }

        public static void Pass1Initialize() => AnimationSchema.Type = new UIXTypeSchema((short)9, "Animation", (string)null, (short)104, typeof(Animation), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)9, "CenterPointPercent", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnimationSchema.GetCenterPointPercent), new SetValueHandler(AnimationSchema.SetCenterPointPercent), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)9, "DisableMouseInput", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnimationSchema.GetDisableMouseInput), new SetValueHandler(AnimationSchema.SetDisableMouseInput), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)9, "Keyframes", (short)138, (short)130, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnimationSchema.GetKeyframes), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)9, "Loop", (short)115, (short)-1, ExpressionRestriction.None, false, AnimationSchema.ValidateLoopValue, false, new GetValueHandler(AnimationSchema.GetLoop), new SetValueHandler(AnimationSchema.SetLoop), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)9, "RotationAxis", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnimationSchema.GetRotationAxis), new SetValueHandler(AnimationSchema.SetRotationAxis), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)9, "Type", (short)10, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnimationSchema.GetType), new SetValueHandler(AnimationSchema.SetType), false);
            AnimationSchema.Type.Initialize(new DefaultConstructHandler(AnimationSchema.Construct), (ConstructorSchema[])null, new PropertySchema[6]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema6
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
