// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DragSourceHandlerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DragSourceHandlerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetAllowedDropActions(object instanceObj) => (object)((DragSourceHandler)instanceObj).AllowedDropActions;

        private static void SetAllowedDropActions(ref object instanceObj, object valueObj) => ((DragSourceHandler)instanceObj).AllowedDropActions = (DropAction)valueObj;

        private static object GetCurrentDropAction(object instanceObj) => (object)((DragSourceHandler)instanceObj).CurrentDropAction;

        private static object GetValue(object instanceObj) => ((DragSourceHandler)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj) => ((DragSourceHandler)instanceObj).Value = valueObj;

        private static object GetDragging(object instanceObj) => BooleanBoxes.Box(((DragSourceHandler)instanceObj).Dragging);

        private static object GetHandlerStage(object instanceObj) => (object)((InputHandler)instanceObj).HandlerStage;

        private static void SetHandlerStage(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).HandlerStage = (InputHandlerStage)valueObj;

        private static object GetMoveCursor(object instanceObj) => (object)((DragSourceHandler)instanceObj).MoveCursor;

        private static void SetMoveCursor(ref object instanceObj, object valueObj) => ((DragSourceHandler)instanceObj).MoveCursor = (CursorID)valueObj;

        private static object GetCopyCursor(object instanceObj) => (object)((DragSourceHandler)instanceObj).CopyCursor;

        private static void SetCopyCursor(ref object instanceObj, object valueObj) => ((DragSourceHandler)instanceObj).CopyCursor = (CursorID)valueObj;

        private static object GetCancelCursor(object instanceObj) => (object)((DragSourceHandler)instanceObj).CancelCursor;

        private static void SetCancelCursor(ref object instanceObj, object valueObj) => ((DragSourceHandler)instanceObj).CancelCursor = (CursorID)valueObj;

        private static object Construct() => (object)new DragSourceHandler();

        public static void Pass1Initialize() => DragSourceHandlerSchema.Type = new UIXTypeSchema((short)63, "DragSourceHandler", (string)null, (short)110, typeof(DragSourceHandler), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)63, "AllowedDropActions", (short)64, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragSourceHandlerSchema.GetAllowedDropActions), new SetValueHandler(DragSourceHandlerSchema.SetAllowedDropActions), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)63, "CurrentDropAction", (short)64, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragSourceHandlerSchema.GetCurrentDropAction), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)63, "Value", (short)153, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragSourceHandlerSchema.GetValue), new SetValueHandler(DragSourceHandlerSchema.SetValue), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)63, "Dragging", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragSourceHandlerSchema.GetDragging), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)63, "HandlerStage", (short)112, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragSourceHandlerSchema.GetHandlerStage), new SetValueHandler(DragSourceHandlerSchema.SetHandlerStage), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)63, "MoveCursor", (short)44, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragSourceHandlerSchema.GetMoveCursor), new SetValueHandler(DragSourceHandlerSchema.SetMoveCursor), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)63, "CopyCursor", (short)44, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragSourceHandlerSchema.GetCopyCursor), new SetValueHandler(DragSourceHandlerSchema.SetCopyCursor), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)63, "CancelCursor", (short)44, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragSourceHandlerSchema.GetCancelCursor), new SetValueHandler(DragSourceHandlerSchema.SetCancelCursor), false);
            UIXEventSchema uixEventSchema1 = new UIXEventSchema((short)63, "Started");
            UIXEventSchema uixEventSchema2 = new UIXEventSchema((short)63, "Moved");
            UIXEventSchema uixEventSchema3 = new UIXEventSchema((short)63, "Copied");
            UIXEventSchema uixEventSchema4 = new UIXEventSchema((short)63, "Canceled");
            DragSourceHandlerSchema.Type.Initialize(new DefaultConstructHandler(DragSourceHandlerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[8]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema3
            }, (MethodSchema[])null, new EventSchema[4]
            {
        (EventSchema) uixEventSchema1,
        (EventSchema) uixEventSchema2,
        (EventSchema) uixEventSchema3,
        (EventSchema) uixEventSchema4
            }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
