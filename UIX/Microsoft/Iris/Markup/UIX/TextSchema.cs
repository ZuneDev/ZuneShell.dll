// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.TextSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using Microsoft.Iris.ViewItems;
using System.Collections;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class TextSchema
    {
        public static UIXTypeSchema Type;

        private static object GetContent(object instanceObj) => (object)((Text)instanceObj).Content;

        private static void SetContent(ref object instanceObj, object valueObj) => ((Text)instanceObj).Content = (string)valueObj;

        private static object GetFont(object instanceObj) => (object)((Text)instanceObj).Font;

        private static void SetFont(ref object instanceObj, object valueObj) => ((Text)instanceObj).Font = (Font)valueObj;

        private static object GetColor(object instanceObj) => (object)((Text)instanceObj).Color;

        private static void SetColor(ref object instanceObj, object valueObj) => ((Text)instanceObj).Color = (Color)valueObj;

        private static object GetWordWrap(object instanceObj) => BooleanBoxes.Box(((Text)instanceObj).WordWrap);

        private static void SetWordWrap(ref object instanceObj, object valueObj) => ((Text)instanceObj).WordWrap = (bool)valueObj;

        private static object GetMaximumLines(object instanceObj) => (object)((Text)instanceObj).MaximumLines;

        private static void SetMaximumLines(ref object instanceObj, object valueObj)
        {
            Text text = (Text)instanceObj;
            int num = (int)valueObj;
            Result result = Int32Schema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                text.MaximumLines = num;
        }

        private static object GetLineAlignment(object instanceObj) => (object)((Text)instanceObj).LineAlignment;

        private static void SetLineAlignment(ref object instanceObj, object valueObj) => ((Text)instanceObj).LineAlignment = (LineAlignment)valueObj;

        private static object GetLineSpacing(object instanceObj) => (object)((Text)instanceObj).LineSpacing;

        private static void SetLineSpacing(ref object instanceObj, object valueObj) => ((Text)instanceObj).LineSpacing = (float)valueObj;

        private static object GetCharacterSpacing(object instanceObj) => (object)((Text)instanceObj).CharacterSpacing;

        private static void SetCharacterSpacing(ref object instanceObj, object valueObj) => ((Text)instanceObj).CharacterSpacing = (float)valueObj;

        private static object GetEnableKerning(object instanceObj) => BooleanBoxes.Box(((Text)instanceObj).EnableKerning);

        private static void SetEnableKerning(ref object instanceObj, object valueObj) => ((Text)instanceObj).EnableKerning = (bool)valueObj;

        private static object GetLastLineBounds(object instanceObj) => (object)((Text)instanceObj).LastLineBounds;

        private static object GetFadeSize(object instanceObj) => (object)((Text)instanceObj).FadeSize;

        private static void SetFadeSize(ref object instanceObj, object valueObj) => ((Text)instanceObj).FadeSize = (float)valueObj;

        private static object GetStyle(object instanceObj) => (object)((Text)instanceObj).Style;

        private static void SetStyle(ref object instanceObj, object valueObj) => ((Text)instanceObj).Style = (TextStyle)valueObj;

        private static object GetNamedStyles(object instanceObj) => (object)((Text)instanceObj).NamedStyles;

        private static void SetNamedStyles(ref object instanceObj, object valueObj) => ((Text)instanceObj).NamedStyles = (IDictionary)valueObj;

        private static object GetFragments(object instanceObj) => (object)((Text)instanceObj).Fragments;

        private static object GetTextSharpness(object instanceObj) => (object)((Text)instanceObj).TextSharpness;

        private static void SetTextSharpness(ref object instanceObj, object valueObj) => ((Text)instanceObj).TextSharpness = (TextSharpness)valueObj;

        private static object GetClipped(object instanceObj) => BooleanBoxes.Box(((Text)instanceObj).Clipped);

        private static object GetContributesToWidth(object instanceObj) => BooleanBoxes.Box(((Text)instanceObj).ContributesToWidth);

        private static void SetContributesToWidth(ref object instanceObj, object valueObj) => ((Text)instanceObj).ContributesToWidth = (bool)valueObj;

        private static object GetBoundsType(object instanceObj) => (object)((Text)instanceObj).BoundsType;

        private static void SetBoundsType(ref object instanceObj, object valueObj) => ((Text)instanceObj).BoundsType = (TextBounds)valueObj;

        private static object GetEffect(object instanceObj) => (object)((ViewItem)instanceObj).Effect;

        private static void SetEffect(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Effect = (EffectClass)valueObj;

        private static object GetDisableIme(object instanceObj) => BooleanBoxes.Box(((Text)instanceObj).DisableIme);

        private static void SetDisableIme(ref object instanceObj, object valueObj) => ((Text)instanceObj).DisableIme = (bool)valueObj;

        private static object GetHighlightColor(object instanceObj) => (object)((Text)instanceObj).HighlightColor;

        private static void SetHighlightColor(ref object instanceObj, object valueObj) => ((Text)instanceObj).HighlightColor = (Color)valueObj;

        private static object GetTextHighlightColor(object instanceObj) => (object)((Text)instanceObj).TextHighlightColor;

        private static void SetTextHighlightColor(ref object instanceObj, object valueObj) => ((Text)instanceObj).TextHighlightColor = (Color)valueObj;

        private static object GetUsePasswordMask(object instanceObj) => BooleanBoxes.Box(((Text)instanceObj).UsePasswordMask);

        private static void SetUsePasswordMask(ref object instanceObj, object valueObj) => ((Text)instanceObj).UsePasswordMask = (bool)valueObj;

        private static object GetPasswordMask(object instanceObj) => (object)((Text)instanceObj).PasswordMask;

        private static void SetPasswordMask(ref object instanceObj, object valueObj) => ((Text)instanceObj).PasswordMask = (char)valueObj;

        private static object Construct() => (object)new Text();

        public static void Pass1Initialize() => TextSchema.Type = new UIXTypeSchema((short)212, "Text", (string)null, (short)239, typeof(Text), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)212, "Content", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetContent), new SetValueHandler(TextSchema.SetContent), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)212, "Font", (short)93, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetFont), new SetValueHandler(TextSchema.SetFont), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)212, "Color", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetColor), new SetValueHandler(TextSchema.SetColor), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)212, "WordWrap", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetWordWrap), new SetValueHandler(TextSchema.SetWordWrap), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)212, "MaximumLines", (short)115, (short)-1, ExpressionRestriction.None, false, Int32Schema.ValidateNotNegative, true, new GetValueHandler(TextSchema.GetMaximumLines), new SetValueHandler(TextSchema.SetMaximumLines), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)212, "LineAlignment", (short)137, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetLineAlignment), new SetValueHandler(TextSchema.SetLineAlignment), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)212, "LineSpacing", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetLineSpacing), new SetValueHandler(TextSchema.SetLineSpacing), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)212, "CharacterSpacing", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetCharacterSpacing), new SetValueHandler(TextSchema.SetCharacterSpacing), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)212, "EnableKerning", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetEnableKerning), new SetValueHandler(TextSchema.SetEnableKerning), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)212, "LastLineBounds", (short)169, (short)-1, ExpressionRestriction.ReadOnly, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetLastLineBounds), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema((short)212, "FadeSize", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetFadeSize), new SetValueHandler(TextSchema.SetFadeSize), false);
            UIXPropertySchema uixPropertySchema12 = new UIXPropertySchema((short)212, "Style", (short)220, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetStyle), new SetValueHandler(TextSchema.SetStyle), false);
            UIXPropertySchema uixPropertySchema13 = new UIXPropertySchema((short)212, "NamedStyles", (short)58, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetNamedStyles), new SetValueHandler(TextSchema.SetNamedStyles), false);
            UIXPropertySchema uixPropertySchema14 = new UIXPropertySchema((short)212, "Fragments", (short)138, (short)215, ExpressionRestriction.ReadOnly, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetFragments), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema15 = new UIXPropertySchema((short)212, "TextSharpness", (short)219, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetTextSharpness), new SetValueHandler(TextSchema.SetTextSharpness), false);
            UIXPropertySchema uixPropertySchema16 = new UIXPropertySchema((short)212, "Clipped", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetClipped), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema17 = new UIXPropertySchema((short)212, "ContributesToWidth", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetContributesToWidth), new SetValueHandler(TextSchema.SetContributesToWidth), false);
            UIXPropertySchema uixPropertySchema18 = new UIXPropertySchema((short)212, "BoundsType", (short)213, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetBoundsType), new SetValueHandler(TextSchema.SetBoundsType), false);
            UIXPropertySchema uixPropertySchema19 = new UIXPropertySchema((short)212, "Effect", (short)78, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetEffect), new SetValueHandler(TextSchema.SetEffect), false);
            UIXPropertySchema uixPropertySchema20 = new UIXPropertySchema((short)212, "DisableIme", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetDisableIme), new SetValueHandler(TextSchema.SetDisableIme), false);
            UIXPropertySchema uixPropertySchema21 = new UIXPropertySchema((short)212, "HighlightColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetHighlightColor), new SetValueHandler(TextSchema.SetHighlightColor), false);
            UIXPropertySchema uixPropertySchema22 = new UIXPropertySchema((short)212, "TextHighlightColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetTextHighlightColor), new SetValueHandler(TextSchema.SetTextHighlightColor), false);
            UIXPropertySchema uixPropertySchema23 = new UIXPropertySchema((short)212, "UsePasswordMask", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetUsePasswordMask), new SetValueHandler(TextSchema.SetUsePasswordMask), false);
            UIXPropertySchema uixPropertySchema24 = new UIXPropertySchema((short)212, "PasswordMask", (short)27, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextSchema.GetPasswordMask), new SetValueHandler(TextSchema.SetPasswordMask), false);
            TextSchema.Type.Initialize(new DefaultConstructHandler(TextSchema.Construct), (ConstructorSchema[])null, new PropertySchema[24]
            {
        (PropertySchema) uixPropertySchema18,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema16,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema17,
        (PropertySchema) uixPropertySchema20,
        (PropertySchema) uixPropertySchema19,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema11,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema14,
        (PropertySchema) uixPropertySchema21,
        (PropertySchema) uixPropertySchema10,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema13,
        (PropertySchema) uixPropertySchema24,
        (PropertySchema) uixPropertySchema12,
        (PropertySchema) uixPropertySchema22,
        (PropertySchema) uixPropertySchema15,
        (PropertySchema) uixPropertySchema23,
        (PropertySchema) uixPropertySchema4
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
