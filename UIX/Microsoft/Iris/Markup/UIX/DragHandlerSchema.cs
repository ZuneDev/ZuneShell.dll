// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DragHandlerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DragHandlerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetBeginDragPolicy(object instanceObj) => (object)((DragHandler)instanceObj).BeginDragPolicy;

        private static void SetBeginDragPolicy(ref object instanceObj, object valueObj) => ((DragHandler)instanceObj).BeginDragPolicy = (BeginDragPolicy)valueObj;

        private static object GetDragging(object instanceObj) => BooleanBoxes.Box(((DragHandler)instanceObj).Dragging);

        private static object GetBeginPosition(object instanceObj) => (object)((DragHandler)instanceObj).BeginPosition;

        private static object GetEndPosition(object instanceObj) => (object)((DragHandler)instanceObj).EndPosition;

        private static object GetScreenDragSize(object instanceObj) => (object)((DragHandler)instanceObj).ScreenDragSize;

        private static object GetLocalDragSize(object instanceObj) => (object)((DragHandler)instanceObj).LocalDragSize;

        private static object GetRelativeDragSize(object instanceObj) => (object)((DragHandler)instanceObj).RelativeDragSize;

        private static object GetActiveModifiers(object instanceObj) => (object)((DragHandler)instanceObj).ActiveModifiers;

        private static object GetDragCursor(object instanceObj) => (object)((DragHandler)instanceObj).DragCursor;

        private static void SetDragCursor(ref object instanceObj, object valueObj) => ((DragHandler)instanceObj).DragCursor = (CursorID)valueObj;

        private static object GetCancelOnEscape(object instanceObj) => BooleanBoxes.Box(((DragHandler)instanceObj).CancelOnEscape);

        private static void SetCancelOnEscape(ref object instanceObj, object valueObj) => ((DragHandler)instanceObj).CancelOnEscape = (bool)valueObj;

        private static object GetRelativeTo(object instanceObj) => (object)((DragHandler)instanceObj).RelativeTo;

        private static void SetRelativeTo(ref object instanceObj, object valueObj) => ((DragHandler)instanceObj).RelativeTo = (ViewItem)valueObj;

        private static object GetHandlerStage(object instanceObj) => (object)((InputHandler)instanceObj).HandlerStage;

        private static void SetHandlerStage(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).HandlerStage = (InputHandlerStage)valueObj;

        private static object Construct() => (object)new DragHandler();

        private static object CallResetDragOrigin(object instanceObj, object[] parameters)
        {
            ((DragHandler)instanceObj).ResetDragOrigin();
            return (object)null;
        }

        private static object CallCancelDrag(object instanceObj, object[] parameters)
        {
            ((DragHandler)instanceObj).CancelDrag();
            return (object)null;
        }

        private static object CallGetEventContexts(object instanceObj, object[] parameters) => (object)((DragHandler)instanceObj).GetEventContexts();

        private static object CallGetAddedEventContexts(object instanceObj, object[] parameters) => (object)((DragHandler)instanceObj).GetAddedEventContexts();

        private static object CallGetRemovedEventContexts(object instanceObj, object[] parameters) => (object)((DragHandler)instanceObj).GetRemovedEventContexts();

        public static void Pass1Initialize() => DragHandlerSchema.Type = new UIXTypeSchema((short)62, "DragHandler", (string)null, (short)110, typeof(DragHandler), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)62, "BeginDragPolicy", (short)12, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragHandlerSchema.GetBeginDragPolicy), new SetValueHandler(DragHandlerSchema.SetBeginDragPolicy), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)62, "Dragging", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragHandlerSchema.GetDragging), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)62, "BeginPosition", (short)233, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragHandlerSchema.GetBeginPosition), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)62, "EndPosition", (short)233, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragHandlerSchema.GetEndPosition), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)62, "ScreenDragSize", (short)195, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragHandlerSchema.GetScreenDragSize), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)62, "LocalDragSize", (short)233, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragHandlerSchema.GetLocalDragSize), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)62, "RelativeDragSize", (short)233, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragHandlerSchema.GetRelativeDragSize), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)62, "ActiveModifiers", (short)111, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragHandlerSchema.GetActiveModifiers), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)62, "DragCursor", (short)44, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragHandlerSchema.GetDragCursor), new SetValueHandler(DragHandlerSchema.SetDragCursor), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)62, "CancelOnEscape", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragHandlerSchema.GetCancelOnEscape), new SetValueHandler(DragHandlerSchema.SetCancelOnEscape), false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema((short)62, "RelativeTo", (short)239, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragHandlerSchema.GetRelativeTo), new SetValueHandler(DragHandlerSchema.SetRelativeTo), false);
            UIXPropertySchema uixPropertySchema12 = new UIXPropertySchema((short)62, "HandlerStage", (short)112, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DragHandlerSchema.GetHandlerStage), new SetValueHandler(DragHandlerSchema.SetHandlerStage), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)62, "ResetDragOrigin", (short[])null, (short)240, new InvokeHandler(DragHandlerSchema.CallResetDragOrigin), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)62, "CancelDrag", (short[])null, (short)240, new InvokeHandler(DragHandlerSchema.CallCancelDrag), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)62, "GetEventContexts", (short[])null, (short)138, new InvokeHandler(DragHandlerSchema.CallGetEventContexts), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)62, "GetAddedEventContexts", (short[])null, (short)138, new InvokeHandler(DragHandlerSchema.CallGetAddedEventContexts), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)62, "GetRemovedEventContexts", (short[])null, (short)138, new InvokeHandler(DragHandlerSchema.CallGetRemovedEventContexts), false);
            UIXEventSchema uixEventSchema1 = new UIXEventSchema((short)62, "Started");
            UIXEventSchema uixEventSchema2 = new UIXEventSchema((short)62, "Canceled");
            UIXEventSchema uixEventSchema3 = new UIXEventSchema((short)62, "Ended");
            DragHandlerSchema.Type.Initialize(new DefaultConstructHandler(DragHandlerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[12]
            {
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema10,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema12,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema11,
        (PropertySchema) uixPropertySchema5
            }, new MethodSchema[5]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4,
        (MethodSchema) uixMethodSchema5
            }, new EventSchema[3]
            {
        (EventSchema) uixEventSchema1,
        (EventSchema) uixEventSchema2,
        (EventSchema) uixEventSchema3
            }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
