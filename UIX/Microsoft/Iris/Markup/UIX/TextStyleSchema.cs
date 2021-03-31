// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.TextStyleSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class TextStyleSchema
    {
        public static RangeValidator ValidateFontFace = new RangeValidator(TextStyleSchema.RangeValidateFontFace);
        public static UIXTypeSchema Type;

        private static object GetFontFace(object instanceObj) => (object)((TextStyle)instanceObj).FontFace;

        private static void SetFontFace(ref object instanceObj, object valueObj)
        {
            TextStyle textStyle = (TextStyle)instanceObj;
            string str = (string)valueObj;
            Result result = TextStyleSchema.ValidateFontFace(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                textStyle.FontFace = str;
        }

        private static object GetFontSize(object instanceObj) => (object)((TextStyle)instanceObj).FontSize;

        private static void SetFontSize(ref object instanceObj, object valueObj)
        {
            TextStyle textStyle = (TextStyle)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                textStyle.FontSize = num;
        }

        private static object GetBold(object instanceObj) => BooleanBoxes.Box(((TextStyle)instanceObj).Bold);

        private static void SetBold(ref object instanceObj, object valueObj) => ((TextStyle)instanceObj).Bold = (bool)valueObj;

        private static object GetItalic(object instanceObj) => BooleanBoxes.Box(((TextStyle)instanceObj).Italic);

        private static void SetItalic(ref object instanceObj, object valueObj) => ((TextStyle)instanceObj).Italic = (bool)valueObj;

        private static object GetUnderline(object instanceObj) => BooleanBoxes.Box(((TextStyle)instanceObj).Underline);

        private static void SetUnderline(ref object instanceObj, object valueObj) => ((TextStyle)instanceObj).Underline = (bool)valueObj;

        private static object GetColor(object instanceObj) => (object)((TextStyle)instanceObj).Color;

        private static void SetColor(ref object instanceObj, object valueObj) => ((TextStyle)instanceObj).Color = (Color)valueObj;

        private static object GetLineSpacing(object instanceObj) => (object)((TextStyle)instanceObj).LineSpacing;

        private static void SetLineSpacing(ref object instanceObj, object valueObj)
        {
            TextStyle textStyle = (TextStyle)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                textStyle.LineSpacing = num;
        }

        private static object GetEnableKerning(object instanceObj) => BooleanBoxes.Box(((TextStyle)instanceObj).EnableKerning);

        private static void SetEnableKerning(ref object instanceObj, object valueObj) => ((TextStyle)instanceObj).EnableKerning = (bool)valueObj;

        private static object GetCharacterSpacing(object instanceObj) => (object)((TextStyle)instanceObj).CharacterSpacing;

        private static void SetCharacterSpacing(ref object instanceObj, object valueObj) => ((TextStyle)instanceObj).CharacterSpacing = (float)valueObj;

        private static object GetFragment(object instanceObj) => BooleanBoxes.Box(((TextStyle)instanceObj).Fragment);

        private static void SetFragment(ref object instanceObj, object valueObj) => ((TextStyle)instanceObj).Fragment = (bool)valueObj;

        private static object Construct() => (object)new TextStyle();

        private static Result RangeValidateFontFace(object value)
        {
            string str = (string)value;
            return str.Length > 31 ? Result.Fail("\"{0}\" cannot be longer than {1} characters", (object)str, (object)"31") : Result.Success;
        }

        public static void Pass1Initialize() => TextStyleSchema.Type = new UIXTypeSchema((short)220, "TextStyle", (string)null, (short)153, typeof(TextStyle), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)220, "FontFace", (short)208, (short)-1, ExpressionRestriction.None, false, TextStyleSchema.ValidateFontFace, true, new GetValueHandler(TextStyleSchema.GetFontFace), new SetValueHandler(TextStyleSchema.SetFontFace), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)220, "FontSize", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, true, new GetValueHandler(TextStyleSchema.GetFontSize), new SetValueHandler(TextStyleSchema.SetFontSize), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)220, "Bold", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextStyleSchema.GetBold), new SetValueHandler(TextStyleSchema.SetBold), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)220, "Italic", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextStyleSchema.GetItalic), new SetValueHandler(TextStyleSchema.SetItalic), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)220, "Underline", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextStyleSchema.GetUnderline), new SetValueHandler(TextStyleSchema.SetUnderline), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)220, "Color", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextStyleSchema.GetColor), new SetValueHandler(TextStyleSchema.SetColor), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)220, "LineSpacing", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, true, new GetValueHandler(TextStyleSchema.GetLineSpacing), new SetValueHandler(TextStyleSchema.SetLineSpacing), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)220, "EnableKerning", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextStyleSchema.GetEnableKerning), new SetValueHandler(TextStyleSchema.SetEnableKerning), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)220, "CharacterSpacing", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextStyleSchema.GetCharacterSpacing), new SetValueHandler(TextStyleSchema.SetCharacterSpacing), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)220, "Fragment", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextStyleSchema.GetFragment), new SetValueHandler(TextStyleSchema.SetFragment), false);
            TextStyleSchema.Type.Initialize(new DefaultConstructHandler(TextStyleSchema.Construct), (ConstructorSchema[])null, new PropertySchema[10]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema10,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema5
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
