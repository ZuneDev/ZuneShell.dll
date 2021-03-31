// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.FocusHandlerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class FocusHandlerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetReason(object instanceObj) => (object)((FocusHandler)instanceObj).Reason;

        private static void SetReason(ref object instanceObj, object valueObj) => ((FocusHandler)instanceObj).Reason = (FocusChangeReason)valueObj;

        private static object GetRequiredModifiers(object instanceObj) => (object)((ModifierInputHandler)instanceObj).RequiredModifiers;

        private static void SetRequiredModifiers(ref object instanceObj, object valueObj) => ((ModifierInputHandler)instanceObj).RequiredModifiers = (InputHandlerModifiers)valueObj;

        private static object GetDisallowedModifiers(object instanceObj) => (object)((ModifierInputHandler)instanceObj).DisallowedModifiers;

        private static void SetDisallowedModifiers(ref object instanceObj, object valueObj) => ((ModifierInputHandler)instanceObj).DisallowedModifiers = (InputHandlerModifiers)valueObj;

        private static object GetHandlerStage(object instanceObj) => (object)((InputHandler)instanceObj).HandlerStage;

        private static void SetHandlerStage(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).HandlerStage = (InputHandlerStage)valueObj;

        private static object GetGainedEventContext(object instanceObj) => ((FocusHandler)instanceObj).GainedEventContext;

        private static object GetLostEventContext(object instanceObj) => ((FocusHandler)instanceObj).LostEventContext;

        private static object Construct() => (object)new FocusHandler();

        public static void Pass1Initialize() => FocusHandlerSchema.Type = new UIXTypeSchema((short)92, "FocusHandler", (string)null, (short)110, typeof(FocusHandler), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)92, "Reason", (short)91, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(FocusHandlerSchema.GetReason), new SetValueHandler(FocusHandlerSchema.SetReason), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)92, "RequiredModifiers", (short)111, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(FocusHandlerSchema.GetRequiredModifiers), new SetValueHandler(FocusHandlerSchema.SetRequiredModifiers), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)92, "DisallowedModifiers", (short)111, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(FocusHandlerSchema.GetDisallowedModifiers), new SetValueHandler(FocusHandlerSchema.SetDisallowedModifiers), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)92, "HandlerStage", (short)112, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(FocusHandlerSchema.GetHandlerStage), new SetValueHandler(FocusHandlerSchema.SetHandlerStage), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)92, "GainedEventContext", (short)153, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(FocusHandlerSchema.GetGainedEventContext), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)92, "LostEventContext", (short)153, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(FocusHandlerSchema.GetLostEventContext), (SetValueHandler)null, false);
            UIXEventSchema uixEventSchema1 = new UIXEventSchema((short)92, "GainedFocus");
            UIXEventSchema uixEventSchema2 = new UIXEventSchema((short)92, "LostFocus");
            FocusHandlerSchema.Type.Initialize(new DefaultConstructHandler(FocusHandlerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[6]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema6,
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
