// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DebugOutlinesSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Debug;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DebugOutlinesSchema
    {
        public static UIXTypeSchema Type;

        private static object GetRoot(object instanceObj) => (object)((DebugOutlines)instanceObj).Root;

        private static void SetRoot(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).Root = (ViewItem)valueObj;

        private static object GetEnabled(object instanceObj) => (object)DebugOutlines.Enabled;

        private static void SetEnabled(ref object instanceObj, object valueObj)
        {
            DebugOutlines debugOutlines = (DebugOutlines)instanceObj;
            DebugOutlines.Enabled = (bool)valueObj;
        }

        private static object GetOutlineLabel(object instanceObj) => (object)((DebugOutlines)instanceObj).OutlineLabel;

        private static void SetOutlineLabel(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).OutlineLabel = (DebugLabelFormat)valueObj;

        private static object GetOutlineScope(object instanceObj) => (object)((DebugOutlines)instanceObj).OutlineScope;

        private static void SetOutlineScope(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).OutlineScope = (DebugOutlineScope)valueObj;

        private static object GetOutlineColor(object instanceObj) => (object)((DebugOutlines)instanceObj).OutlineColor;

        private static void SetOutlineColor(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).OutlineColor = (Color)valueObj;

        private static object GetHostOutlineColor(object instanceObj) => (object)((DebugOutlines)instanceObj).HostOutlineColor;

        private static void SetHostOutlineColor(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).HostOutlineColor = (Color)valueObj;

        private static object GetTextColor(object instanceObj) => (object)((DebugOutlines)instanceObj).TextColor;

        private static void SetTextColor(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).TextColor = (Color)valueObj;

        private static object GetTextFont(object instanceObj) => (object)((DebugOutlines)instanceObj).TextFont;

        private static void SetTextFont(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).TextFont = (Font)valueObj;

        private static object GetMouseInteractiveImage(object instanceObj) => (object)((DebugOutlines)instanceObj).MouseInteractiveImage;

        private static void SetMouseInteractiveImage(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).MouseInteractiveImage = (UIImage)valueObj;

        private static object GetMouseFocusImage(object instanceObj) => (object)((DebugOutlines)instanceObj).MouseFocusImage;

        private static void SetMouseFocusImage(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).MouseFocusImage = (UIImage)valueObj;

        private static object GetKeyInteractiveImage(object instanceObj) => (object)((DebugOutlines)instanceObj).KeyInteractiveImage;

        private static void SetKeyInteractiveImage(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).KeyInteractiveImage = (UIImage)valueObj;

        private static object GetKeyFocusImage(object instanceObj) => (object)((DebugOutlines)instanceObj).KeyFocusImage;

        private static void SetKeyFocusImage(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).KeyFocusImage = (UIImage)valueObj;

        private static object Construct() => (object)new DebugOutlines();

        private static object CallNextScopeMode(object instanceObj, object[] parameters)
        {
            ((DebugOutlines)instanceObj).NextScopeMode();
            return (object)null;
        }

        private static object CallNextLabelMode(object instanceObj, object[] parameters)
        {
            ((DebugOutlines)instanceObj).NextLabelMode();
            return (object)null;
        }

        public static void Pass1Initialize() => DebugOutlinesSchema.Type = new UIXTypeSchema((short)52, "DebugOutlines", (string)null, (short)239, typeof(DebugOutlines), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)52, "Root", (short)239, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DebugOutlinesSchema.GetRoot), new SetValueHandler(DebugOutlinesSchema.SetRoot), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)52, "Enabled", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(DebugOutlinesSchema.GetEnabled), new SetValueHandler(DebugOutlinesSchema.SetEnabled), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)52, "OutlineLabel", (short)50, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DebugOutlinesSchema.GetOutlineLabel), new SetValueHandler(DebugOutlinesSchema.SetOutlineLabel), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)52, "OutlineScope", (short)51, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DebugOutlinesSchema.GetOutlineScope), new SetValueHandler(DebugOutlinesSchema.SetOutlineScope), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)52, "OutlineColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DebugOutlinesSchema.GetOutlineColor), new SetValueHandler(DebugOutlinesSchema.SetOutlineColor), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)52, "HostOutlineColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DebugOutlinesSchema.GetHostOutlineColor), new SetValueHandler(DebugOutlinesSchema.SetHostOutlineColor), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)52, "TextColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DebugOutlinesSchema.GetTextColor), new SetValueHandler(DebugOutlinesSchema.SetTextColor), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)52, "TextFont", (short)93, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DebugOutlinesSchema.GetTextFont), new SetValueHandler(DebugOutlinesSchema.SetTextFont), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)52, "MouseInteractiveImage", (short)105, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DebugOutlinesSchema.GetMouseInteractiveImage), new SetValueHandler(DebugOutlinesSchema.SetMouseInteractiveImage), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)52, "MouseFocusImage", (short)105, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DebugOutlinesSchema.GetMouseFocusImage), new SetValueHandler(DebugOutlinesSchema.SetMouseFocusImage), false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema((short)52, "KeyInteractiveImage", (short)105, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DebugOutlinesSchema.GetKeyInteractiveImage), new SetValueHandler(DebugOutlinesSchema.SetKeyInteractiveImage), false);
            UIXPropertySchema uixPropertySchema12 = new UIXPropertySchema((short)52, "KeyFocusImage", (short)105, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DebugOutlinesSchema.GetKeyFocusImage), new SetValueHandler(DebugOutlinesSchema.SetKeyFocusImage), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)52, "NextScopeMode", (short[])null, (short)240, new InvokeHandler(DebugOutlinesSchema.CallNextScopeMode), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)52, "NextLabelMode", (short[])null, (short)240, new InvokeHandler(DebugOutlinesSchema.CallNextLabelMode), false);
            DebugOutlinesSchema.Type.Initialize(new DefaultConstructHandler(DebugOutlinesSchema.Construct), (ConstructorSchema[])null, new PropertySchema[12]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema12,
        (PropertySchema) uixPropertySchema11,
        (PropertySchema) uixPropertySchema10,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema8
            }, new MethodSchema[2]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
