// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.CameraSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class CameraSchema
    {
        public static UIXTypeSchema Type;

        private static object GetEye(object instanceObj) => (object)((Camera)instanceObj).Eye;

        private static void SetEye(ref object instanceObj, object valueObj) => ((Camera)instanceObj).Eye = (Vector3)valueObj;

        private static object GetAt(object instanceObj) => (object)((Camera)instanceObj).At;

        private static void SetAt(ref object instanceObj, object valueObj) => ((Camera)instanceObj).At = (Vector3)valueObj;

        private static object GetUp(object instanceObj) => (object)((Camera)instanceObj).Up;

        private static void SetUp(ref object instanceObj, object valueObj) => ((Camera)instanceObj).Up = (Vector3)valueObj;

        private static object GetZn(object instanceObj) => (object)((Camera)instanceObj).Zn;

        private static void SetZn(ref object instanceObj, object valueObj) => ((Camera)instanceObj).Zn = (float)valueObj;

        private static object GetEyeAnimation(object instanceObj) => (object)((Camera)instanceObj).EyeAnimation;

        private static void SetEyeAnimation(ref object instanceObj, object valueObj) => ((Camera)instanceObj).EyeAnimation = (IAnimationProvider)valueObj;

        private static object GetAtAnimation(object instanceObj) => (object)((Camera)instanceObj).AtAnimation;

        private static void SetAtAnimation(ref object instanceObj, object valueObj) => ((Camera)instanceObj).AtAnimation = (IAnimationProvider)valueObj;

        private static object GetUpAnimation(object instanceObj) => (object)((Camera)instanceObj).UpAnimation;

        private static void SetUpAnimation(ref object instanceObj, object valueObj) => ((Camera)instanceObj).UpAnimation = (IAnimationProvider)valueObj;

        private static object GetZnAnimation(object instanceObj) => (object)((Camera)instanceObj).ZnAnimation;

        private static void SetZnAnimation(ref object instanceObj, object valueObj) => ((Camera)instanceObj).ZnAnimation = (IAnimationProvider)valueObj;

        private static object GetPerspective(object instanceObj) => BooleanBoxes.Box(((Camera)instanceObj).Perspective);

        private static void SetPerspective(ref object instanceObj, object valueObj) => ((Camera)instanceObj).Perspective = (bool)valueObj;

        private static object Construct() => (object)new Camera();

        private static object CallPlayAnimationIAnimation(object instanceObj, object[] parameters)
        {
            Camera camera = (Camera)instanceObj;
            IAnimationProvider parameter = (IAnimationProvider)parameters[0];
            if (parameter == null)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"animation");
                return (object)null;
            }
            camera.PlayAnimation(parameter, (AnimationHandle)null);
            return (object)null;
        }

        private static object CallPlayAnimationIAnimationAnimationHandle(
          object instanceObj,
          object[] parameters)
        {
            Camera camera = (Camera)instanceObj;
            IAnimationProvider parameter1 = (IAnimationProvider)parameters[0];
            AnimationHandle parameter2 = (AnimationHandle)parameters[1];
            if (parameter1 == null)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"animation");
                return (object)null;
            }
            if (parameter2 == null)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"handle");
                return (object)null;
            }
            camera.PlayAnimation(parameter1, parameter2);
            return (object)null;
        }

        public static void Pass1Initialize() => CameraSchema.Type = new UIXTypeSchema((short)21, "Camera", (string)null, (short)153, typeof(Camera), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)21, "Eye", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CameraSchema.GetEye), new SetValueHandler(CameraSchema.SetEye), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)21, "At", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CameraSchema.GetAt), new SetValueHandler(CameraSchema.SetAt), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)21, "Up", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CameraSchema.GetUp), new SetValueHandler(CameraSchema.SetUp), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)21, "Zn", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CameraSchema.GetZn), new SetValueHandler(CameraSchema.SetZn), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)21, "EyeAnimation", (short)104, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CameraSchema.GetEyeAnimation), new SetValueHandler(CameraSchema.SetEyeAnimation), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)21, "AtAnimation", (short)104, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CameraSchema.GetAtAnimation), new SetValueHandler(CameraSchema.SetAtAnimation), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)21, "UpAnimation", (short)104, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CameraSchema.GetUpAnimation), new SetValueHandler(CameraSchema.SetUpAnimation), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)21, "ZnAnimation", (short)104, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CameraSchema.GetZnAnimation), new SetValueHandler(CameraSchema.SetZnAnimation), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)21, "Perspective", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CameraSchema.GetPerspective), new SetValueHandler(CameraSchema.SetPerspective), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)21, "PlayAnimation", new short[1]
            {
        (short) 104
            }, (short)240, new InvokeHandler(CameraSchema.CallPlayAnimationIAnimation), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)21, "PlayAnimation", new short[2]
            {
        (short) 104,
        (short) 11
            }, (short)240, new InvokeHandler(CameraSchema.CallPlayAnimationIAnimationAnimationHandle), false);
            CameraSchema.Type.Initialize(new DefaultConstructHandler(CameraSchema.Construct), (ConstructorSchema[])null, new PropertySchema[9]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema8
            }, new MethodSchema[2]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
