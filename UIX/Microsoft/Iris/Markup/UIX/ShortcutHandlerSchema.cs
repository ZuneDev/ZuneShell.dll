// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ShortcutHandlerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ShortcutHandlerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetShortcut(object instanceObj) => (object)((ShortcutHandler)instanceObj).Shortcut;

        private static void SetShortcut(ref object instanceObj, object valueObj) => ((ShortcutHandler)instanceObj).Shortcut = (ShortcutHandlerCommand)valueObj;

        private static object GetCommand(object instanceObj) => (object)((ShortcutHandler)instanceObj).Command;

        private static void SetCommand(ref object instanceObj, object valueObj) => ((ShortcutHandler)instanceObj).Command = (IUICommand)valueObj;

        private static object GetHandle(object instanceObj) => BooleanBoxes.Box(((ShortcutHandler)instanceObj).Handle);

        private static void SetHandle(ref object instanceObj, object valueObj) => ((ShortcutHandler)instanceObj).Handle = (bool)valueObj;

        private static object GetHandlerStage(object instanceObj) => (object)((InputHandler)instanceObj).HandlerStage;

        private static void SetHandlerStage(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).HandlerStage = (InputHandlerStage)valueObj;

        private static object Construct() => (object)new ShortcutHandler();

        public static void Pass1Initialize() => ShortcutHandlerSchema.Type = new UIXTypeSchema((short)192, "ShortcutHandler", (string)null, (short)110, typeof(ShortcutHandler), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)192, "Shortcut", (short)193, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ShortcutHandlerSchema.GetShortcut), new SetValueHandler(ShortcutHandlerSchema.SetShortcut), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)192, "Command", (short)40, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ShortcutHandlerSchema.GetCommand), new SetValueHandler(ShortcutHandlerSchema.SetCommand), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)192, "Handle", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ShortcutHandlerSchema.GetHandle), new SetValueHandler(ShortcutHandlerSchema.SetHandle), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)192, "HandlerStage", (short)112, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ShortcutHandlerSchema.GetHandlerStage), new SetValueHandler(ShortcutHandlerSchema.SetHandlerStage), false);
            UIXEventSchema uixEventSchema = new UIXEventSchema((short)192, "Invoked");
            ShortcutHandlerSchema.Type.Initialize(new DefaultConstructHandler(ShortcutHandlerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[4]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, new EventSchema[1]
            {
        (EventSchema) uixEventSchema
            }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
