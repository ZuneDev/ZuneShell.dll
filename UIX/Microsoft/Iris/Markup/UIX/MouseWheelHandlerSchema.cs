// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.MouseWheelHandlerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class MouseWheelHandlerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetHandle(object instanceObj) => BooleanBoxes.Box(((MouseWheelHandler)instanceObj).Handle);

        private static void SetHandle(ref object instanceObj, object valueObj) => ((MouseWheelHandler)instanceObj).Handle = (bool)valueObj;

        private static object GetHandlerStage(object instanceObj) => (object)((InputHandler)instanceObj).HandlerStage;

        private static void SetHandlerStage(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).HandlerStage = (InputHandlerStage)valueObj;

        private static object Construct() => (object)new MouseWheelHandler();

        public static void Pass1Initialize() => MouseWheelHandlerSchema.Type = new UIXTypeSchema((short)150, "MouseWheelHandler", (string)null, (short)110, typeof(MouseWheelHandler), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)150, "Handle", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(MouseWheelHandlerSchema.GetHandle), new SetValueHandler(MouseWheelHandlerSchema.SetHandle), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)150, "HandlerStage", (short)112, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(MouseWheelHandlerSchema.GetHandlerStage), new SetValueHandler(MouseWheelHandlerSchema.SetHandlerStage), false);
            UIXEventSchema uixEventSchema1 = new UIXEventSchema((short)150, "UpInvoked");
            UIXEventSchema uixEventSchema2 = new UIXEventSchema((short)150, "DownInvoked");
            MouseWheelHandlerSchema.Type.Initialize(new DefaultConstructHandler(MouseWheelHandlerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, new EventSchema[2]
            {
        (EventSchema) uixEventSchema1,
        (EventSchema) uixEventSchema2
            }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
