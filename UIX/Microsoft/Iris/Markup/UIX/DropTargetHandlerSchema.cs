// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DropTargetHandlerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DropTargetHandlerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetAllowedDropActions(object instanceObj) => (object)((DropTargetHandler)instanceObj).AllowedDropActions;

        private static void SetAllowedDropActions(ref object instanceObj, object valueObj) => ((DropTargetHandler)instanceObj).AllowedDropActions = (DropAction)valueObj;

        private static object GetDragging(object instanceObj) => BooleanBoxes.Box(((DropTargetHandler)instanceObj).Dragging);

        private static object GetHandlerStage(object instanceObj) => (object)((InputHandler)instanceObj).HandlerStage;

        private static void SetHandlerStage(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).HandlerStage = (InputHandlerStage)valueObj;

        private static object GetEventContext(object instanceObj) => ((DropTargetHandler)instanceObj).EventContext;

        private static object Construct() => (object)new DropTargetHandler();

        private static object CallGetValue(object instanceObj, object[] parameters) => ((DropTargetHandler)instanceObj).GetValue();

        public static void Pass1Initialize() => DropTargetHandlerSchema.Type = new UIXTypeSchema((short)65, "DropTargetHandler", (string)null, (short)110, typeof(DropTargetHandler), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)65, "AllowedDropActions", (short)64, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DropTargetHandlerSchema.GetAllowedDropActions), new SetValueHandler(DropTargetHandlerSchema.SetAllowedDropActions), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)65, "Dragging", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DropTargetHandlerSchema.GetDragging), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)65, "HandlerStage", (short)112, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DropTargetHandlerSchema.GetHandlerStage), new SetValueHandler(DropTargetHandlerSchema.SetHandlerStage), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)65, "EventContext", (short)153, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DropTargetHandlerSchema.GetEventContext), (SetValueHandler)null, false);
            UIXEventSchema uixEventSchema1 = new UIXEventSchema((short)65, "DragEnter");
            UIXEventSchema uixEventSchema2 = new UIXEventSchema((short)65, "DragOver");
            UIXEventSchema uixEventSchema3 = new UIXEventSchema((short)65, "DragLeave");
            UIXEventSchema uixEventSchema4 = new UIXEventSchema((short)65, "Dropped");
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)65, "GetValue", (short[])null, (short)153, new InvokeHandler(DropTargetHandlerSchema.CallGetValue), false);
            DropTargetHandlerSchema.Type.Initialize(new DefaultConstructHandler(DropTargetHandlerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[4]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema3
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, new EventSchema[4]
            {
        (EventSchema) uixEventSchema1,
        (EventSchema) uixEventSchema2,
        (EventSchema) uixEventSchema3,
        (EventSchema) uixEventSchema4
            }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
