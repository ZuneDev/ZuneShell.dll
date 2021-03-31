// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.TextEditingHandlerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.InputHandlers;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class TextEditingHandlerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetAcceptsEnter(object instanceObj) => BooleanBoxes.Box(((TextEditingHandler)instanceObj).AcceptsEnter);

        private static void SetAcceptsEnter(ref object instanceObj, object valueObj) => ((TextEditingHandler)instanceObj).AcceptsEnter = (bool)valueObj;

        private static object GetAcceptsTab(object instanceObj) => BooleanBoxes.Box(((TextEditingHandler)instanceObj).AcceptsTab);

        private static void SetAcceptsTab(ref object instanceObj, object valueObj) => ((TextEditingHandler)instanceObj).AcceptsTab = (bool)valueObj;

        private static object GetCaretInfo(object instanceObj) => (object)((TextEditingHandler)instanceObj).CaretInfo;

        private static object GetEditableTextData(object instanceObj) => (object)((TextEditingHandler)instanceObj).EditableTextData;

        private static void SetEditableTextData(ref object instanceObj, object valueObj) => ((TextEditingHandler)instanceObj).EditableTextData = (EditableTextData)valueObj;

        private static object GetOvertype(object instanceObj) => BooleanBoxes.Box(((TextEditingHandler)instanceObj).Overtype);

        private static void SetOvertype(ref object instanceObj, object valueObj) => ((TextEditingHandler)instanceObj).Overtype = (bool)valueObj;

        private static object GetTextDisplay(object instanceObj) => (object)((TextEditingHandler)instanceObj).TextDisplay;

        private static void SetTextDisplay(ref object instanceObj, object valueObj) => ((TextEditingHandler)instanceObj).TextDisplay = (Text)valueObj;

        private static object GetSelectionRange(object instanceObj) => (object)((TextEditingHandler)instanceObj).SelectionRange;

        private static void SetSelectionRange(ref object instanceObj, object valueObj)
        {
            TextEditingHandler textEditingHandler = (TextEditingHandler)instanceObj;
            Range range = (Range)valueObj;
            int num = 0;
            if (textEditingHandler.EditableTextData != null && textEditingHandler.EditableTextData.Value != null)
                num = textEditingHandler.EditableTextData.Value.Length;
            if (range.Begin < 0 || range.Begin > num)
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)range.Begin, (object)"SelectionRange.Begin");
            if (range.End < 0 || range.End > num)
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)range.End, (object)"SelectionRange.End");
            textEditingHandler.SelectionRange = range;
        }

        private static object GetCopyCommand(object instanceObj) => (object)((TextEditingHandler)instanceObj).CopyCommand;

        private static object GetCutCommand(object instanceObj) => (object)((TextEditingHandler)instanceObj).CutCommand;

        private static object GetDeleteCommand(object instanceObj) => (object)((TextEditingHandler)instanceObj).DeleteCommand;

        private static object GetPasteCommand(object instanceObj) => (object)((TextEditingHandler)instanceObj).PasteCommand;

        private static object GetSelectAllCommand(object instanceObj) => (object)((TextEditingHandler)instanceObj).SelectAllCommand;

        private static object GetUndoCommand(object instanceObj) => (object)((TextEditingHandler)instanceObj).UndoCommand;

        private static object GetHorizontalScrollModel(object instanceObj) => (object)((TextEditingHandler)instanceObj).HorizontalScrollModel;

        private static void SetHorizontalScrollModel(ref object instanceObj, object valueObj) => ((TextEditingHandler)instanceObj).HorizontalScrollModel = (TextScrollModel)valueObj;

        private static object GetVerticalScrollModel(object instanceObj) => (object)((TextEditingHandler)instanceObj).VerticalScrollModel;

        private static void SetVerticalScrollModel(ref object instanceObj, object valueObj) => ((TextEditingHandler)instanceObj).VerticalScrollModel = (TextScrollModel)valueObj;

        private static object GetDetectUrls(object instanceObj) => BooleanBoxes.Box(((TextEditingHandler)instanceObj).DetectUrls);

        private static void SetDetectUrls(ref object instanceObj, object valueObj) => ((TextEditingHandler)instanceObj).DetectUrls = (bool)valueObj;

        private static object GetLinkColor(object instanceObj) => (object)((TextEditingHandler)instanceObj).LinkColor;

        private static void SetLinkColor(ref object instanceObj, object valueObj) => ((TextEditingHandler)instanceObj).LinkColor = (Color)valueObj;

        private static object GetLinkClickedParameter(object instanceObj) => (object)((TextEditingHandler)instanceObj).LinkClickedParameter;

        private static object GetInImeCompositionMode(object instanceObj) => BooleanBoxes.Box(((TextEditingHandler)instanceObj).InImeCompositionMode);

        private static void SetInImeCompositionMode(ref object instanceObj, object valueObj) => ((TextEditingHandler)instanceObj).InImeCompositionMode = (bool)valueObj;

        private static object GetHandlerStage(object instanceObj) => (object)((InputHandler)instanceObj).HandlerStage;

        private static void SetHandlerStage(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).HandlerStage = (InputHandlerStage)valueObj;

        private static object Construct() => (object)new TextEditingHandler();

        private static object CallCopy(object instanceObj, object[] parameters)
        {
            ((TextEditingHandler)instanceObj).Copy();
            return (object)null;
        }

        private static object CallCut(object instanceObj, object[] parameters)
        {
            ((TextEditingHandler)instanceObj).Cut();
            return (object)null;
        }

        private static object CallDelete(object instanceObj, object[] parameters)
        {
            ((TextEditingHandler)instanceObj).Delete();
            return (object)null;
        }

        private static object CallPaste(object instanceObj, object[] parameters)
        {
            ((TextEditingHandler)instanceObj).Paste();
            return (object)null;
        }

        private static object CallSelectAll(object instanceObj, object[] parameters)
        {
            ((TextEditingHandler)instanceObj).SelectAll();
            return (object)null;
        }

        private static object CallUndo(object instanceObj, object[] parameters)
        {
            ((TextEditingHandler)instanceObj).Undo();
            return (object)null;
        }

        public static void Pass1Initialize() => TextEditingHandlerSchema.Type = new UIXTypeSchema((short)214, "TextEditingHandler", (string)null, (short)110, typeof(TextEditingHandler), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)214, "AcceptsEnter", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetAcceptsEnter), new SetValueHandler(TextEditingHandlerSchema.SetAcceptsEnter), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)214, "AcceptsTab", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetAcceptsTab), new SetValueHandler(TextEditingHandlerSchema.SetAcceptsTab), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)214, "CaretInfo", (short)26, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetCaretInfo), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)214, "EditableTextData", (short)68, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetEditableTextData), new SetValueHandler(TextEditingHandlerSchema.SetEditableTextData), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)214, "Overtype", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetOvertype), new SetValueHandler(TextEditingHandlerSchema.SetOvertype), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)214, "TextDisplay", (short)212, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetTextDisplay), new SetValueHandler(TextEditingHandlerSchema.SetTextDisplay), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)214, "SelectionRange", (short)187, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetSelectionRange), new SetValueHandler(TextEditingHandlerSchema.SetSelectionRange), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)214, "CopyCommand", (short)40, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetCopyCommand), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)214, "CutCommand", (short)40, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetCutCommand), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)214, "DeleteCommand", (short)40, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetDeleteCommand), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema((short)214, "PasteCommand", (short)40, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetPasteCommand), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema12 = new UIXPropertySchema((short)214, "SelectAllCommand", (short)40, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetSelectAllCommand), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema13 = new UIXPropertySchema((short)214, "UndoCommand", (short)40, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetUndoCommand), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema14 = new UIXPropertySchema((short)214, "HorizontalScrollModel", (short)218, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetHorizontalScrollModel), new SetValueHandler(TextEditingHandlerSchema.SetHorizontalScrollModel), false);
            UIXPropertySchema uixPropertySchema15 = new UIXPropertySchema((short)214, "VerticalScrollModel", (short)218, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetVerticalScrollModel), new SetValueHandler(TextEditingHandlerSchema.SetVerticalScrollModel), false);
            UIXPropertySchema uixPropertySchema16 = new UIXPropertySchema((short)214, "DetectUrls", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetDetectUrls), new SetValueHandler(TextEditingHandlerSchema.SetDetectUrls), false);
            UIXPropertySchema uixPropertySchema17 = new UIXPropertySchema((short)214, "LinkColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetLinkColor), new SetValueHandler(TextEditingHandlerSchema.SetLinkColor), false);
            UIXPropertySchema uixPropertySchema18 = new UIXPropertySchema((short)214, "LinkClickedParameter", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetLinkClickedParameter), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema19 = new UIXPropertySchema((short)214, "InImeCompositionMode", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetInImeCompositionMode), new SetValueHandler(TextEditingHandlerSchema.SetInImeCompositionMode), false);
            UIXPropertySchema uixPropertySchema20 = new UIXPropertySchema((short)214, "HandlerStage", (short)112, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextEditingHandlerSchema.GetHandlerStage), new SetValueHandler(TextEditingHandlerSchema.SetHandlerStage), false);
            UIXEventSchema uixEventSchema1 = new UIXEventSchema((short)214, "TypingInputRejected");
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)214, "Copy", (short[])null, (short)240, new InvokeHandler(TextEditingHandlerSchema.CallCopy), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)214, "Cut", (short[])null, (short)240, new InvokeHandler(TextEditingHandlerSchema.CallCut), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)214, "Delete", (short[])null, (short)240, new InvokeHandler(TextEditingHandlerSchema.CallDelete), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)214, "Paste", (short[])null, (short)240, new InvokeHandler(TextEditingHandlerSchema.CallPaste), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)214, "SelectAll", (short[])null, (short)240, new InvokeHandler(TextEditingHandlerSchema.CallSelectAll), false);
            UIXMethodSchema uixMethodSchema6 = new UIXMethodSchema((short)214, "Undo", (short[])null, (short)240, new InvokeHandler(TextEditingHandlerSchema.CallUndo), false);
            UIXEventSchema uixEventSchema2 = new UIXEventSchema((short)214, "LinkClicked");
            TextEditingHandlerSchema.Type.Initialize(new DefaultConstructHandler(TextEditingHandlerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[20]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema10,
        (PropertySchema) uixPropertySchema16,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema20,
        (PropertySchema) uixPropertySchema14,
        (PropertySchema) uixPropertySchema19,
        (PropertySchema) uixPropertySchema18,
        (PropertySchema) uixPropertySchema17,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema11,
        (PropertySchema) uixPropertySchema12,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema13,
        (PropertySchema) uixPropertySchema15
            }, new MethodSchema[6]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4,
        (MethodSchema) uixMethodSchema5,
        (MethodSchema) uixMethodSchema6
            }, new EventSchema[2]
            {
        (EventSchema) uixEventSchema1,
        (EventSchema) uixEventSchema2
            }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
