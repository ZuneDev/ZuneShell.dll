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

        private static object GetRoot(object instanceObj) => ((DebugOutlines)instanceObj).Root;

        private static void SetRoot(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).Root = (ViewItem)valueObj;

        private static object GetEnabled(object instanceObj) => DebugOutlines.Enabled;

        private static void SetEnabled(ref object instanceObj, object valueObj)
        {
            DebugOutlines debugOutlines = (DebugOutlines)instanceObj;
            DebugOutlines.Enabled = (bool)valueObj;
        }

        private static object GetOutlineLabel(object instanceObj) => ((DebugOutlines)instanceObj).OutlineLabel;

        private static void SetOutlineLabel(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).OutlineLabel = (DebugLabelFormat)valueObj;

        private static object GetOutlineScope(object instanceObj) => ((DebugOutlines)instanceObj).OutlineScope;

        private static void SetOutlineScope(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).OutlineScope = (DebugOutlineScope)valueObj;

        private static object GetOutlineColor(object instanceObj) => ((DebugOutlines)instanceObj).OutlineColor;

        private static void SetOutlineColor(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).OutlineColor = (Color)valueObj;

        private static object GetHostOutlineColor(object instanceObj) => ((DebugOutlines)instanceObj).HostOutlineColor;

        private static void SetHostOutlineColor(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).HostOutlineColor = (Color)valueObj;

        private static object GetTextColor(object instanceObj) => ((DebugOutlines)instanceObj).TextColor;

        private static void SetTextColor(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).TextColor = (Color)valueObj;

        private static object GetTextFont(object instanceObj) => ((DebugOutlines)instanceObj).TextFont;

        private static void SetTextFont(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).TextFont = (Font)valueObj;

        private static object GetMouseInteractiveImage(object instanceObj) => ((DebugOutlines)instanceObj).MouseInteractiveImage;

        private static void SetMouseInteractiveImage(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).MouseInteractiveImage = (UIImage)valueObj;

        private static object GetMouseFocusImage(object instanceObj) => ((DebugOutlines)instanceObj).MouseFocusImage;

        private static void SetMouseFocusImage(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).MouseFocusImage = (UIImage)valueObj;

        private static object GetKeyInteractiveImage(object instanceObj) => ((DebugOutlines)instanceObj).KeyInteractiveImage;

        private static void SetKeyInteractiveImage(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).KeyInteractiveImage = (UIImage)valueObj;

        private static object GetKeyFocusImage(object instanceObj) => ((DebugOutlines)instanceObj).KeyFocusImage;

        private static void SetKeyFocusImage(ref object instanceObj, object valueObj) => ((DebugOutlines)instanceObj).KeyFocusImage = (UIImage)valueObj;

        private static object Construct() => new DebugOutlines();

        private static object CallNextScopeMode(object instanceObj, object[] parameters)
        {
            ((DebugOutlines)instanceObj).NextScopeMode();
            return null;
        }

        private static object CallNextLabelMode(object instanceObj, object[] parameters)
        {
            ((DebugOutlines)instanceObj).NextLabelMode();
            return null;
        }

        public static void Pass1Initialize() => DebugOutlinesSchema.Type = new UIXTypeSchema(52, "DebugOutlines", null, 239, typeof(DebugOutlines), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema(52, "Root", 239, -1, ExpressionRestriction.None, false, null, true, new GetValueHandler(DebugOutlinesSchema.GetRoot), new SetValueHandler(DebugOutlinesSchema.SetRoot), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema(52, "Enabled", 15, -1, ExpressionRestriction.None, false, null, false, new GetValueHandler(DebugOutlinesSchema.GetEnabled), new SetValueHandler(DebugOutlinesSchema.SetEnabled), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema(52, "OutlineLabel", 50, -1, ExpressionRestriction.None, false, null, true, new GetValueHandler(DebugOutlinesSchema.GetOutlineLabel), new SetValueHandler(DebugOutlinesSchema.SetOutlineLabel), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema(52, "OutlineScope", 51, -1, ExpressionRestriction.None, false, null, true, new GetValueHandler(DebugOutlinesSchema.GetOutlineScope), new SetValueHandler(DebugOutlinesSchema.SetOutlineScope), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema(52, "OutlineColor", 35, -1, ExpressionRestriction.None, false, null, true, new GetValueHandler(DebugOutlinesSchema.GetOutlineColor), new SetValueHandler(DebugOutlinesSchema.SetOutlineColor), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema(52, "HostOutlineColor", 35, -1, ExpressionRestriction.None, false, null, true, new GetValueHandler(DebugOutlinesSchema.GetHostOutlineColor), new SetValueHandler(DebugOutlinesSchema.SetHostOutlineColor), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema(52, "TextColor", 35, -1, ExpressionRestriction.None, false, null, true, new GetValueHandler(DebugOutlinesSchema.GetTextColor), new SetValueHandler(DebugOutlinesSchema.SetTextColor), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema(52, "TextFont", 93, -1, ExpressionRestriction.None, false, null, true, new GetValueHandler(DebugOutlinesSchema.GetTextFont), new SetValueHandler(DebugOutlinesSchema.SetTextFont), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema(52, "MouseInteractiveImage", 105, -1, ExpressionRestriction.None, false, null, true, new GetValueHandler(DebugOutlinesSchema.GetMouseInteractiveImage), new SetValueHandler(DebugOutlinesSchema.SetMouseInteractiveImage), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema(52, "MouseFocusImage", 105, -1, ExpressionRestriction.None, false, null, true, new GetValueHandler(DebugOutlinesSchema.GetMouseFocusImage), new SetValueHandler(DebugOutlinesSchema.SetMouseFocusImage), false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema(52, "KeyInteractiveImage", 105, -1, ExpressionRestriction.None, false, null, true, new GetValueHandler(DebugOutlinesSchema.GetKeyInteractiveImage), new SetValueHandler(DebugOutlinesSchema.SetKeyInteractiveImage), false);
            UIXPropertySchema uixPropertySchema12 = new UIXPropertySchema(52, "KeyFocusImage", 105, -1, ExpressionRestriction.None, false, null, true, new GetValueHandler(DebugOutlinesSchema.GetKeyFocusImage), new SetValueHandler(DebugOutlinesSchema.SetKeyFocusImage), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema(52, "NextScopeMode", null, 240, new InvokeHandler(DebugOutlinesSchema.CallNextScopeMode), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema(52, "NextLabelMode", null, 240, new InvokeHandler(DebugOutlinesSchema.CallNextLabelMode), false);
            DebugOutlinesSchema.Type.Initialize(new DefaultConstructHandler(DebugOutlinesSchema.Construct), null, new PropertySchema[12]
            {
         uixPropertySchema2,
         uixPropertySchema6,
         uixPropertySchema12,
         uixPropertySchema11,
         uixPropertySchema10,
         uixPropertySchema9,
         uixPropertySchema5,
         uixPropertySchema3,
         uixPropertySchema4,
         uixPropertySchema1,
         uixPropertySchema7,
         uixPropertySchema8
            }, new MethodSchema[2]
            {
         uixMethodSchema1,
         uixMethodSchema2
            }, null, null, null, null, null, null, null, null);
        }
    }
}
