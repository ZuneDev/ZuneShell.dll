// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ClickHandlerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.Library;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ClickHandlerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetClicking(object instanceObj) => BooleanBoxes.Box(((ClickHandler)instanceObj).Clicking);

        private static object GetClickCount(object instanceObj) => (object)((ClickHandler)instanceObj).ClickCount;

        private static void SetClickCount(ref object instanceObj, object valueObj) => ((ClickHandler)instanceObj).ClickCount = (ClickCount)valueObj;

        private static object GetClickType(object instanceObj) => (object)((ClickHandler)instanceObj).ClickType;

        private static void SetClickType(ref object instanceObj, object valueObj) => ((ClickHandler)instanceObj).ClickType = (ClickType)valueObj;

        private static object GetCommand(object instanceObj) => (object)((ClickHandler)instanceObj).Command;

        private static void SetCommand(ref object instanceObj, object valueObj) => ((ClickHandler)instanceObj).Command = (IUICommand)valueObj;

        private static object GetHandle(object instanceObj) => BooleanBoxes.Box(((ClickHandler)instanceObj).Handle);

        private static void SetHandle(ref object instanceObj, object valueObj) => ((ClickHandler)instanceObj).Handle = (bool)valueObj;

        private static object GetHandlerTransition(object instanceObj) => (object)((ModifierInputHandler)instanceObj).HandlerTransition;

        private static void SetHandlerTransition(ref object instanceObj, object valueObj) => ((ModifierInputHandler)instanceObj).HandlerTransition = (InputHandlerTransition)valueObj;

        private static object GetRequiredModifiers(object instanceObj) => (object)((ModifierInputHandler)instanceObj).RequiredModifiers;

        private static void SetRequiredModifiers(ref object instanceObj, object valueObj) => ((ModifierInputHandler)instanceObj).RequiredModifiers = (InputHandlerModifiers)valueObj;

        private static object GetDisallowedModifiers(object instanceObj) => (object)((ModifierInputHandler)instanceObj).DisallowedModifiers;

        private static void SetDisallowedModifiers(ref object instanceObj, object valueObj) => ((ModifierInputHandler)instanceObj).DisallowedModifiers = (InputHandlerModifiers)valueObj;

        private static object GetRepeat(object instanceObj) => BooleanBoxes.Box(((ClickHandler)instanceObj).Repeat);

        private static void SetRepeat(ref object instanceObj, object valueObj) => ((ClickHandler)instanceObj).Repeat = (bool)valueObj;

        private static object GetRepeatDelay(object instanceObj) => (object)((ClickHandler)instanceObj).RepeatDelay;

        private static void SetRepeatDelay(ref object instanceObj, object valueObj)
        {
            ClickHandler clickHandler = (ClickHandler)instanceObj;
            int num = (int)valueObj;
            Result result = Int32Schema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                clickHandler.RepeatDelay = num;
        }

        private static object GetRepeatRate(object instanceObj) => (object)((ClickHandler)instanceObj).RepeatRate;

        private static void SetRepeatRate(ref object instanceObj, object valueObj)
        {
            ClickHandler clickHandler = (ClickHandler)instanceObj;
            int num = (int)valueObj;
            Result result = Int32Schema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                clickHandler.RepeatRate = num;
        }

        private static object GetHandlerStage(object instanceObj) => (object)((InputHandler)instanceObj).HandlerStage;

        private static void SetHandlerStage(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).HandlerStage = (InputHandlerStage)valueObj;

        private static object GetEventContext(object instanceObj) => ((ClickHandler)instanceObj).EventContext;

        private static object Construct() => (object)new ClickHandler();

        public static void Pass1Initialize() => ClickHandlerSchema.Type = new UIXTypeSchema((short)32, "ClickHandler", (string)null, (short)110, typeof(ClickHandler), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)32, "Clicking", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClickHandlerSchema.GetClicking), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)32, "ClickCount", (short)31, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClickHandlerSchema.GetClickCount), new SetValueHandler(ClickHandlerSchema.SetClickCount), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)32, "ClickType", (short)33, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClickHandlerSchema.GetClickType), new SetValueHandler(ClickHandlerSchema.SetClickType), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)32, "Command", (short)40, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClickHandlerSchema.GetCommand), new SetValueHandler(ClickHandlerSchema.SetCommand), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)32, "Handle", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClickHandlerSchema.GetHandle), new SetValueHandler(ClickHandlerSchema.SetHandle), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)32, "HandlerTransition", (short)113, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClickHandlerSchema.GetHandlerTransition), new SetValueHandler(ClickHandlerSchema.SetHandlerTransition), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)32, "RequiredModifiers", (short)111, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClickHandlerSchema.GetRequiredModifiers), new SetValueHandler(ClickHandlerSchema.SetRequiredModifiers), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)32, "DisallowedModifiers", (short)111, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClickHandlerSchema.GetDisallowedModifiers), new SetValueHandler(ClickHandlerSchema.SetDisallowedModifiers), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)32, "Repeat", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClickHandlerSchema.GetRepeat), new SetValueHandler(ClickHandlerSchema.SetRepeat), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)32, "RepeatDelay", (short)115, (short)-1, ExpressionRestriction.None, false, Int32Schema.ValidateNotNegative, true, new GetValueHandler(ClickHandlerSchema.GetRepeatDelay), new SetValueHandler(ClickHandlerSchema.SetRepeatDelay), false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema((short)32, "RepeatRate", (short)115, (short)-1, ExpressionRestriction.None, false, Int32Schema.ValidateNotNegative, true, new GetValueHandler(ClickHandlerSchema.GetRepeatRate), new SetValueHandler(ClickHandlerSchema.SetRepeatRate), false);
            UIXPropertySchema uixPropertySchema12 = new UIXPropertySchema((short)32, "HandlerStage", (short)112, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClickHandlerSchema.GetHandlerStage), new SetValueHandler(ClickHandlerSchema.SetHandlerStage), false);
            UIXPropertySchema uixPropertySchema13 = new UIXPropertySchema((short)32, "EventContext", (short)153, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClickHandlerSchema.GetEventContext), (SetValueHandler)null, false);
            UIXEventSchema uixEventSchema = new UIXEventSchema((short)32, "Invoked");
            ClickHandlerSchema.Type.Initialize(new DefaultConstructHandler(ClickHandlerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[13]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema13,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema12,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema10,
        (PropertySchema) uixPropertySchema11,
        (PropertySchema) uixPropertySchema7
            }, (MethodSchema[])null, new EventSchema[1]
            {
        (EventSchema) uixEventSchema
            }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
