// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ScrollingHandlerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ScrollingHandlerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetHandleDirectionalKeys(object instanceObj) => BooleanBoxes.Box(((ScrollingHandler)instanceObj).HandleDirectionalKeys);

        private static void SetHandleDirectionalKeys(ref object instanceObj, object valueObj) => ((ScrollingHandler)instanceObj).HandleDirectionalKeys = (bool)valueObj;

        private static object GetHandlePageKeys(object instanceObj) => BooleanBoxes.Box(((ScrollingHandler)instanceObj).HandlePageKeys);

        private static void SetHandlePageKeys(ref object instanceObj, object valueObj) => ((ScrollingHandler)instanceObj).HandlePageKeys = (bool)valueObj;

        private static object GetHandleHomeEndKeys(object instanceObj) => BooleanBoxes.Box(((ScrollingHandler)instanceObj).HandleHomeEndKeys);

        private static void SetHandleHomeEndKeys(ref object instanceObj, object valueObj) => ((ScrollingHandler)instanceObj).HandleHomeEndKeys = (bool)valueObj;

        private static object GetHandlePageCommands(object instanceObj) => BooleanBoxes.Box(((ScrollingHandler)instanceObj).HandlePageCommands);

        private static void SetHandlePageCommands(ref object instanceObj, object valueObj) => ((ScrollingHandler)instanceObj).HandlePageCommands = (bool)valueObj;

        private static object GetHandleMouseWheel(object instanceObj) => BooleanBoxes.Box(((ScrollingHandler)instanceObj).HandleMouseWheel);

        private static void SetHandleMouseWheel(ref object instanceObj, object valueObj) => ((ScrollingHandler)instanceObj).HandleMouseWheel = (bool)valueObj;

        private static object GetScrollModel(object instanceObj) => (object)((ScrollingHandler)instanceObj).ScrollModel;

        private static void SetScrollModel(ref object instanceObj, object valueObj) => ((ScrollingHandler)instanceObj).ScrollModel = (ScrollModel)valueObj;

        private static object GetUseFocusBehavior(object instanceObj) => BooleanBoxes.Box(((ScrollingHandler)instanceObj).UseFocusBehavior);

        private static void SetUseFocusBehavior(ref object instanceObj, object valueObj) => ((ScrollingHandler)instanceObj).UseFocusBehavior = (bool)valueObj;

        private static object GetHandlerStage(object instanceObj) => (object)((InputHandler)instanceObj).HandlerStage;

        private static void SetHandlerStage(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).HandlerStage = (InputHandlerStage)valueObj;

        private static object Construct() => (object)new ScrollingHandler();

        public static void Pass1Initialize() => ScrollingHandlerSchema.Type = new UIXTypeSchema((short)185, "ScrollingHandler", (string)null, (short)110, typeof(ScrollingHandler), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)185, "HandleDirectionalKeys", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollingHandlerSchema.GetHandleDirectionalKeys), new SetValueHandler(ScrollingHandlerSchema.SetHandleDirectionalKeys), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)185, "HandlePageKeys", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollingHandlerSchema.GetHandlePageKeys), new SetValueHandler(ScrollingHandlerSchema.SetHandlePageKeys), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)185, "HandleHomeEndKeys", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollingHandlerSchema.GetHandleHomeEndKeys), new SetValueHandler(ScrollingHandlerSchema.SetHandleHomeEndKeys), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)185, "HandlePageCommands", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollingHandlerSchema.GetHandlePageCommands), new SetValueHandler(ScrollingHandlerSchema.SetHandlePageCommands), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)185, "HandleMouseWheel", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollingHandlerSchema.GetHandleMouseWheel), new SetValueHandler(ScrollingHandlerSchema.SetHandleMouseWheel), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)185, "ScrollModel", (short)182, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollingHandlerSchema.GetScrollModel), new SetValueHandler(ScrollingHandlerSchema.SetScrollModel), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)185, "UseFocusBehavior", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollingHandlerSchema.GetUseFocusBehavior), new SetValueHandler(ScrollingHandlerSchema.SetUseFocusBehavior), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)185, "HandlerStage", (short)112, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollingHandlerSchema.GetHandlerStage), new SetValueHandler(ScrollingHandlerSchema.SetHandlerStage), false);
            ScrollingHandlerSchema.Type.Initialize(new DefaultConstructHandler(ScrollingHandlerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[8]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema7
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
