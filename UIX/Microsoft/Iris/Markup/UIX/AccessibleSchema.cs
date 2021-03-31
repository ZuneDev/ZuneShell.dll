// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.AccessibleSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Accessibility;
using Microsoft.Iris.ModelItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class AccessibleSchema
    {
        public static UIXTypeSchema Type;

        private static object GetEnabled(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).Enabled);

        private static object GetDefaultAction(object instanceObj) => (object)((Accessible)instanceObj).DefaultAction;

        private static void SetDefaultAction(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).DefaultAction = (string)valueObj;

        private static object GetDefaultActionCommand(object instanceObj) => (object)((Accessible)instanceObj).DefaultActionCommand;

        private static void SetDefaultActionCommand(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).DefaultActionCommand = (IUICommand)valueObj;

        private static object GetDescription(object instanceObj) => (object)((Accessible)instanceObj).Description;

        private static void SetDescription(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).Description = (string)valueObj;

        private static object GetHasPopup(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).HasPopup);

        private static void SetHasPopup(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).HasPopup = (bool)valueObj;

        private static object GetHelp(object instanceObj) => (object)((Accessible)instanceObj).Help;

        private static void SetHelp(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).Help = (string)valueObj;

        private static object GetHelpTopic(object instanceObj) => (object)((Accessible)instanceObj).HelpTopic;

        private static void SetHelpTopic(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).HelpTopic = (int)valueObj;

        private static object GetIsAnimated(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsAnimated);

        private static void SetIsAnimated(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsAnimated = (bool)valueObj;

        private static object GetIsBusy(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsBusy);

        private static void SetIsBusy(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsBusy = (bool)valueObj;

        private static object GetIsChecked(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsChecked);

        private static void SetIsChecked(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsChecked = (bool)valueObj;

        private static object GetIsCollapsed(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsCollapsed);

        private static void SetIsCollapsed(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsCollapsed = (bool)valueObj;

        private static object GetIsDefault(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsDefault);

        private static void SetIsDefault(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsDefault = (bool)valueObj;

        private static object GetIsExpanded(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsExpanded);

        private static void SetIsExpanded(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsExpanded = (bool)valueObj;

        private static object GetIsMarquee(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsMarquee);

        private static void SetIsMarquee(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsMarquee = (bool)valueObj;

        private static object GetIsMixed(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsMixed);

        private static void SetIsMixed(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsMixed = (bool)valueObj;

        private static object GetIsMultiSelectable(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsMultiSelectable);

        private static void SetIsMultiSelectable(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsMultiSelectable = (bool)valueObj;

        private static object GetIsPressed(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsPressed);

        private static void SetIsPressed(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsPressed = (bool)valueObj;

        private static object GetIsProtected(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsProtected);

        private static void SetIsProtected(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsProtected = (bool)valueObj;

        private static object GetIsSelectable(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsSelectable);

        private static void SetIsSelectable(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsSelectable = (bool)valueObj;

        private static object GetIsSelected(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsSelected);

        private static void SetIsSelected(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsSelected = (bool)valueObj;

        private static object GetIsTraversed(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsTraversed);

        private static void SetIsTraversed(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsTraversed = (bool)valueObj;

        private static object GetIsUnavailable(object instanceObj) => BooleanBoxes.Box(((Accessible)instanceObj).IsUnavailable);

        private static void SetIsUnavailable(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).IsUnavailable = (bool)valueObj;

        private static object GetKeyboardShortcut(object instanceObj) => (object)((Accessible)instanceObj).KeyboardShortcut;

        private static void SetKeyboardShortcut(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).KeyboardShortcut = (string)valueObj;

        private static object GetName(object instanceObj) => (object)((Accessible)instanceObj).Name;

        private static void SetName(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).Name = (string)valueObj;

        private static object GetRole(object instanceObj) => (object)((Accessible)instanceObj).Role;

        private static void SetRole(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).Role = (AccRole)valueObj;

        private static object GetValue(object instanceObj) => (object)((Accessible)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj) => ((Accessible)instanceObj).Value = (string)valueObj;

        private static object Construct() => (object)new Accessible();

        public static void Pass1Initialize() => AccessibleSchema.Type = new UIXTypeSchema((short)0, "Accessible", (string)null, (short)153, typeof(Accessible), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)0, "Enabled", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetEnabled), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)0, "DefaultAction", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetDefaultAction), new SetValueHandler(AccessibleSchema.SetDefaultAction), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)0, "DefaultActionCommand", (short)40, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetDefaultActionCommand), new SetValueHandler(AccessibleSchema.SetDefaultActionCommand), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)0, "Description", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetDescription), new SetValueHandler(AccessibleSchema.SetDescription), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)0, "HasPopup", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetHasPopup), new SetValueHandler(AccessibleSchema.SetHasPopup), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)0, "Help", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetHelp), new SetValueHandler(AccessibleSchema.SetHelp), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)0, "HelpTopic", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetHelpTopic), new SetValueHandler(AccessibleSchema.SetHelpTopic), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)0, "IsAnimated", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsAnimated), new SetValueHandler(AccessibleSchema.SetIsAnimated), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)0, "IsBusy", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsBusy), new SetValueHandler(AccessibleSchema.SetIsBusy), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)0, "IsChecked", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsChecked), new SetValueHandler(AccessibleSchema.SetIsChecked), false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema((short)0, "IsCollapsed", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsCollapsed), new SetValueHandler(AccessibleSchema.SetIsCollapsed), false);
            UIXPropertySchema uixPropertySchema12 = new UIXPropertySchema((short)0, "IsDefault", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsDefault), new SetValueHandler(AccessibleSchema.SetIsDefault), false);
            UIXPropertySchema uixPropertySchema13 = new UIXPropertySchema((short)0, "IsExpanded", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsExpanded), new SetValueHandler(AccessibleSchema.SetIsExpanded), false);
            UIXPropertySchema uixPropertySchema14 = new UIXPropertySchema((short)0, "IsMarquee", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsMarquee), new SetValueHandler(AccessibleSchema.SetIsMarquee), false);
            UIXPropertySchema uixPropertySchema15 = new UIXPropertySchema((short)0, "IsMixed", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsMixed), new SetValueHandler(AccessibleSchema.SetIsMixed), false);
            UIXPropertySchema uixPropertySchema16 = new UIXPropertySchema((short)0, "IsMultiSelectable", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsMultiSelectable), new SetValueHandler(AccessibleSchema.SetIsMultiSelectable), false);
            UIXPropertySchema uixPropertySchema17 = new UIXPropertySchema((short)0, "IsPressed", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsPressed), new SetValueHandler(AccessibleSchema.SetIsPressed), false);
            UIXPropertySchema uixPropertySchema18 = new UIXPropertySchema((short)0, "IsProtected", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsProtected), new SetValueHandler(AccessibleSchema.SetIsProtected), false);
            UIXPropertySchema uixPropertySchema19 = new UIXPropertySchema((short)0, "IsSelectable", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsSelectable), new SetValueHandler(AccessibleSchema.SetIsSelectable), false);
            UIXPropertySchema uixPropertySchema20 = new UIXPropertySchema((short)0, "IsSelected", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsSelected), new SetValueHandler(AccessibleSchema.SetIsSelected), false);
            UIXPropertySchema uixPropertySchema21 = new UIXPropertySchema((short)0, "IsTraversed", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsTraversed), new SetValueHandler(AccessibleSchema.SetIsTraversed), false);
            UIXPropertySchema uixPropertySchema22 = new UIXPropertySchema((short)0, "IsUnavailable", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetIsUnavailable), new SetValueHandler(AccessibleSchema.SetIsUnavailable), false);
            UIXPropertySchema uixPropertySchema23 = new UIXPropertySchema((short)0, "KeyboardShortcut", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetKeyboardShortcut), new SetValueHandler(AccessibleSchema.SetKeyboardShortcut), false);
            UIXPropertySchema uixPropertySchema24 = new UIXPropertySchema((short)0, "Name", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetName), new SetValueHandler(AccessibleSchema.SetName), false);
            UIXPropertySchema uixPropertySchema25 = new UIXPropertySchema((short)0, "Role", (short)1, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetRole), new SetValueHandler(AccessibleSchema.SetRole), false);
            UIXPropertySchema uixPropertySchema26 = new UIXPropertySchema((short)0, "Value", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(AccessibleSchema.GetValue), new SetValueHandler(AccessibleSchema.SetValue), false);
            AccessibleSchema.Type.Initialize(new DefaultConstructHandler(AccessibleSchema.Construct), (ConstructorSchema[])null, new PropertySchema[26]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema10,
        (PropertySchema) uixPropertySchema11,
        (PropertySchema) uixPropertySchema12,
        (PropertySchema) uixPropertySchema13,
        (PropertySchema) uixPropertySchema14,
        (PropertySchema) uixPropertySchema15,
        (PropertySchema) uixPropertySchema16,
        (PropertySchema) uixPropertySchema17,
        (PropertySchema) uixPropertySchema18,
        (PropertySchema) uixPropertySchema19,
        (PropertySchema) uixPropertySchema20,
        (PropertySchema) uixPropertySchema21,
        (PropertySchema) uixPropertySchema22,
        (PropertySchema) uixPropertySchema23,
        (PropertySchema) uixPropertySchema24,
        (PropertySchema) uixPropertySchema25,
        (PropertySchema) uixPropertySchema26
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
