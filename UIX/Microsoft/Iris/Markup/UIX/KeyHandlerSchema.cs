// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.KeyHandlerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System.Collections;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class KeyHandlerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetCommand(object instanceObj) => (object)((KeyHandler)instanceObj).Command;

        private static void SetCommand(ref object instanceObj, object valueObj) => ((KeyHandler)instanceObj).Command = (IUICommand)valueObj;

        private static object GetHandle(object instanceObj) => BooleanBoxes.Box(((KeyHandler)instanceObj).Handle);

        private static void SetHandle(ref object instanceObj, object valueObj) => ((KeyHandler)instanceObj).Handle = (bool)valueObj;

        private static object GetStopRoute(object instanceObj) => BooleanBoxes.Box(((KeyHandler)instanceObj).StopRoute);

        private static void SetStopRoute(ref object instanceObj, object valueObj) => ((KeyHandler)instanceObj).StopRoute = (bool)valueObj;

        private static object GetKey(object instanceObj) => (object)((KeyHandler)instanceObj).Key;

        private static void SetKey(ref object instanceObj, object valueObj) => ((KeyHandler)instanceObj).Key = (KeyHandlerKey)valueObj;

        private static object GetHandlerTransition(object instanceObj) => (object)((ModifierInputHandler)instanceObj).HandlerTransition;

        private static void SetHandlerTransition(ref object instanceObj, object valueObj) => ((ModifierInputHandler)instanceObj).HandlerTransition = (InputHandlerTransition)valueObj;

        private static object GetRequiredModifiers(object instanceObj) => (object)((ModifierInputHandler)instanceObj).RequiredModifiers;

        private static void SetRequiredModifiers(ref object instanceObj, object valueObj) => ((ModifierInputHandler)instanceObj).RequiredModifiers = (InputHandlerModifiers)valueObj;

        private static object GetDisallowedModifiers(object instanceObj) => (object)((ModifierInputHandler)instanceObj).DisallowedModifiers;

        private static void SetDisallowedModifiers(ref object instanceObj, object valueObj) => ((ModifierInputHandler)instanceObj).DisallowedModifiers = (InputHandlerModifiers)valueObj;

        private static object GetPressing(object instanceObj) => BooleanBoxes.Box(((KeyHandler)instanceObj).Pressing);

        private static object GetRepeat(object instanceObj) => BooleanBoxes.Box(((KeyHandler)instanceObj).Repeat);

        private static void SetRepeat(ref object instanceObj, object valueObj) => ((KeyHandler)instanceObj).Repeat = (bool)valueObj;

        private static object GetTrackInvokedKeys(object instanceObj) => BooleanBoxes.Box(((KeyHandler)instanceObj).TrackInvokedKeys);

        private static void SetTrackInvokedKeys(ref object instanceObj, object valueObj) => ((KeyHandler)instanceObj).TrackInvokedKeys = (bool)valueObj;

        private static object GetHandlerStage(object instanceObj) => (object)((InputHandler)instanceObj).HandlerStage;

        private static void SetHandlerStage(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).HandlerStage = (InputHandlerStage)valueObj;

        private static object GetEventContext(object instanceObj) => ((KeyHandler)instanceObj).EventContext;

        private static object Construct() => (object)new KeyHandler();

        private static object CallGetInvokedKeys(object instanceObj, object[] parameters)
        {
            KeyHandler keyHandler = (KeyHandler)instanceObj;
            ArrayList arrayList = new ArrayList();
            keyHandler.GetInvokedKeys((IList)arrayList);
            return (object)arrayList;
        }

        private static object CallGetInvokedKeysList(object instanceObj, object[] parameters)
        {
            KeyHandler keyHandler = (KeyHandler)instanceObj;
            IList parameter = (IList)parameters[0];
            if (parameter != null)
                keyHandler.GetInvokedKeys(parameter);
            else
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"copyTo");
            return (object)null;
        }

        public static void Pass1Initialize() => KeyHandlerSchema.Type = new UIXTypeSchema((short)128, "KeyHandler", (string)null, (short)110, typeof(KeyHandler), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)128, "Command", (short)40, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(KeyHandlerSchema.GetCommand), new SetValueHandler(KeyHandlerSchema.SetCommand), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)128, "Handle", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(KeyHandlerSchema.GetHandle), new SetValueHandler(KeyHandlerSchema.SetHandle), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)128, "StopRoute", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(KeyHandlerSchema.GetStopRoute), new SetValueHandler(KeyHandlerSchema.SetStopRoute), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)128, "Key", (short)129, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(KeyHandlerSchema.GetKey), new SetValueHandler(KeyHandlerSchema.SetKey), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)128, "HandlerTransition", (short)113, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(KeyHandlerSchema.GetHandlerTransition), new SetValueHandler(KeyHandlerSchema.SetHandlerTransition), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)128, "RequiredModifiers", (short)111, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(KeyHandlerSchema.GetRequiredModifiers), new SetValueHandler(KeyHandlerSchema.SetRequiredModifiers), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)128, "DisallowedModifiers", (short)111, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(KeyHandlerSchema.GetDisallowedModifiers), new SetValueHandler(KeyHandlerSchema.SetDisallowedModifiers), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)128, "Pressing", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(KeyHandlerSchema.GetPressing), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)128, "Repeat", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(KeyHandlerSchema.GetRepeat), new SetValueHandler(KeyHandlerSchema.SetRepeat), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)128, "TrackInvokedKeys", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(KeyHandlerSchema.GetTrackInvokedKeys), new SetValueHandler(KeyHandlerSchema.SetTrackInvokedKeys), false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema((short)128, "HandlerStage", (short)112, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(KeyHandlerSchema.GetHandlerStage), new SetValueHandler(KeyHandlerSchema.SetHandlerStage), false);
            UIXPropertySchema uixPropertySchema12 = new UIXPropertySchema((short)128, "EventContext", (short)153, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(KeyHandlerSchema.GetEventContext), (SetValueHandler)null, false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)128, "GetInvokedKeys", (short[])null, (short)138, new InvokeHandler(KeyHandlerSchema.CallGetInvokedKeys), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)128, "GetInvokedKeys", new short[1]
            {
        (short) 138
            }, (short)240, new InvokeHandler(KeyHandlerSchema.CallGetInvokedKeysList), false);
            UIXEventSchema uixEventSchema = new UIXEventSchema((short)128, "Invoked");
            KeyHandlerSchema.Type.Initialize(new DefaultConstructHandler(KeyHandlerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[12]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema12,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema11,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema10
            }, new MethodSchema[2]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2
            }, new EventSchema[1] { (EventSchema)uixEventSchema }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
