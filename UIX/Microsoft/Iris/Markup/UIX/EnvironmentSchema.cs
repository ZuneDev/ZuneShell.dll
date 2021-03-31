// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EnvironmentSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EnvironmentSchema
    {
        public static UIXTypeSchema Type;

        private static object GetIsRightToLeft(object instanceObj) => BooleanBoxes.Box(((Environment)instanceObj).IsRightToLeft);

        private static object GetColorScheme(object instanceObj) => (object)((Environment)instanceObj).ColorScheme;

        private static object GetAnimationSpeed(object instanceObj) => (object)((Environment)instanceObj).AnimationSpeed;

        private static void SetAnimationSpeed(ref object instanceObj, object valueObj) => ((Environment)instanceObj).AnimationSpeed = (float)valueObj;

        private static object GetAnimationUpdatesPerSecond(object instanceObj) => (object)((Environment)instanceObj).AnimationUpdatesPerSecond;

        private static void SetAnimationUpdatesPerSecond(ref object instanceObj, object valueObj) => ((Environment)instanceObj).AnimationUpdatesPerSecond = (int)valueObj;

        private static object GetDpiScale(object instanceObj) => (object)Environment.DpiScale;

        private static object GetGraphicsDeviceType(object instanceObj)
        {
            RenderingType renderingType;
            switch (UISession.Default.RenderSession.GraphicsDevice.DeviceType)
            {
                case GraphicsDeviceType.Gdi:
                    renderingType = RenderingType.GDI;
                    break;
                case GraphicsDeviceType.Direct3D9:
                case GraphicsDeviceType.XeDirectX9:
                    renderingType = RenderingType.DX9;
                    break;
                default:
                    renderingType = RenderingType.Default;
                    break;
            }
            return (object)renderingType;
        }

        private static object Construct() => (object)Environment.Instance;

        private static object CallAnimationAdvanceInt32(object instanceObj, object[] parameters)
        {
            ((Environment)instanceObj).AnimationAdvance((int)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => EnvironmentSchema.Type = new UIXTypeSchema((short)87, "Environment", (string)null, (short)153, typeof(Environment), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)87, "IsRightToLeft", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(EnvironmentSchema.GetIsRightToLeft), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)87, "ColorScheme", (short)39, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(EnvironmentSchema.GetColorScheme), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)87, "AnimationSpeed", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(EnvironmentSchema.GetAnimationSpeed), new SetValueHandler(EnvironmentSchema.SetAnimationSpeed), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)87, "AnimationUpdatesPerSecond", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(EnvironmentSchema.GetAnimationUpdatesPerSecond), new SetValueHandler(EnvironmentSchema.SetAnimationUpdatesPerSecond), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)87, "DpiScale", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(EnvironmentSchema.GetDpiScale), (SetValueHandler)null, true);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)87, "GraphicsDeviceType", (short)98, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(EnvironmentSchema.GetGraphicsDeviceType), (SetValueHandler)null, true);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)87, "AnimationAdvance", new short[1]
            {
        (short) 115
            }, (short)240, new InvokeHandler(EnvironmentSchema.CallAnimationAdvanceInt32), false);
            EnvironmentSchema.Type.Initialize(new DefaultConstructHandler(EnvironmentSchema.Construct), (ConstructorSchema[])null, new PropertySchema[6]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema1
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
