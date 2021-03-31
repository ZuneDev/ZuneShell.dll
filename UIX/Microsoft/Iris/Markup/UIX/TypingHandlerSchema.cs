// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.TypingHandlerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class TypingHandlerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetEditableTextData(object instanceObj) => (object)((TypingHandler)instanceObj).EditableTextData;

        private static void SetEditableTextData(ref object instanceObj, object valueObj) => ((TypingHandler)instanceObj).EditableTextData = (EditableTextData)valueObj;

        private static object GetHandlerStage(object instanceObj) => (object)((InputHandler)instanceObj).HandlerStage;

        private static void SetHandlerStage(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).HandlerStage = (InputHandlerStage)valueObj;

        private static object GetSubmitOnEnter(object instanceObj) => BooleanBoxes.Box(((TypingHandler)instanceObj).SubmitOnEnter);

        private static void SetSubmitOnEnter(ref object instanceObj, object valueObj) => ((TypingHandler)instanceObj).SubmitOnEnter = (bool)valueObj;

        private static object GetTreatEscapeAsBackspace(object instanceObj) => BooleanBoxes.Box(((TypingHandler)instanceObj).TreatEscapeAsBackspace);

        private static void SetTreatEscapeAsBackspace(ref object instanceObj, object valueObj) => ((TypingHandler)instanceObj).TreatEscapeAsBackspace = (bool)valueObj;

        private static object Construct() => (object)new TypingHandler();

        public static void Pass1Initialize() => TypingHandlerSchema.Type = new UIXTypeSchema((short)228, "TypingHandler", (string)null, (short)110, typeof(TypingHandler), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)228, "EditableTextData", (short)68, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TypingHandlerSchema.GetEditableTextData), new SetValueHandler(TypingHandlerSchema.SetEditableTextData), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)228, "HandlerStage", (short)112, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TypingHandlerSchema.GetHandlerStage), new SetValueHandler(TypingHandlerSchema.SetHandlerStage), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)228, "SubmitOnEnter", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TypingHandlerSchema.GetSubmitOnEnter), new SetValueHandler(TypingHandlerSchema.SetSubmitOnEnter), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)228, "TreatEscapeAsBackspace", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TypingHandlerSchema.GetTreatEscapeAsBackspace), new SetValueHandler(TypingHandlerSchema.SetTreatEscapeAsBackspace), false);
            UIXEventSchema uixEventSchema = new UIXEventSchema((short)228, "TypingInputRejected");
            TypingHandlerSchema.Type.Initialize(new DefaultConstructHandler(TypingHandlerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[4]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4
            }, (MethodSchema[])null, new EventSchema[1]
            {
        (EventSchema) uixEventSchema
            }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
