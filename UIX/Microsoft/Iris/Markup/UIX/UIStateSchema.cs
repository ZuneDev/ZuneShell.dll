﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.UIStateSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class UIStateSchema
    {
        public static UIXTypeSchema Type;

        private static object GetCreateInterestOnFocus(object instanceObj) => BooleanBoxes.Box(((UIClass)instanceObj).CreateInterestOnFocus);

        private static void SetCreateInterestOnFocus(ref object instanceObj, object valueObj) => ((UIClass)instanceObj).CreateInterestOnFocus = (bool)valueObj;

        private static object GetCursor(object instanceObj) => (object)((UIClass)instanceObj).Cursor;

        private static void SetCursor(ref object instanceObj, object valueObj) => ((UIClass)instanceObj).Cursor = (CursorID)valueObj;

        private static object GetDirectKeyFocus(object instanceObj) => BooleanBoxes.Box(((UIClass)instanceObj).DirectKeyFocus);

        private static object GetDirectMouseFocus(object instanceObj) => BooleanBoxes.Box(((UIClass)instanceObj).DirectMouseFocus);

        private static object GetFocusInterestTarget(object instanceObj) => (object)((UIClass)instanceObj).FocusInterestTarget;

        private static void SetFocusInterestTarget(ref object instanceObj, object valueObj) => ((UIClass)instanceObj).FocusInterestTarget = (ViewItem)valueObj;

        private static object GetFocusInterestTargetMargins(object instanceObj) => (object)((UIClass)instanceObj).FocusInterestTargetMargins;

        private static void SetFocusInterestTargetMargins(ref object instanceObj, object valueObj) => ((UIClass)instanceObj).FocusInterestTargetMargins = (Inset)valueObj;

        private static object GetKeyFocus(object instanceObj) => BooleanBoxes.Box(((UIClass)instanceObj).KeyFocus);

        private static object GetKeyFocusOnMouseDown(object instanceObj) => BooleanBoxes.Box(((UIClass)instanceObj).KeyFocusOnMouseDown);

        private static void SetKeyFocusOnMouseDown(ref object instanceObj, object valueObj) => ((UIClass)instanceObj).KeyFocusOnMouseDown = (bool)valueObj;

        private static object GetKeyFocusOnMouseEnter(object instanceObj) => BooleanBoxes.Box(((UIClass)instanceObj).KeyFocusOnMouseEnter);

        private static void SetKeyFocusOnMouseEnter(ref object instanceObj, object valueObj) => ((UIClass)instanceObj).KeyFocusOnMouseEnter = (bool)valueObj;

        private static object GetKeyInteractive(object instanceObj) => BooleanBoxes.Box(((UIClass)instanceObj).KeyInteractive);

        private static void SetKeyInteractive(ref object instanceObj, object valueObj) => ((UIClass)instanceObj).KeyInteractive = (bool)valueObj;

        private static object GetMouseFocus(object instanceObj) => BooleanBoxes.Box(((UIClass)instanceObj).MouseFocus);

        private static object GetMouseInteractive(object instanceObj) => (object)((UIClass)instanceObj).MouseInteractive;

        private static void SetMouseInteractive(ref object instanceObj, object valueObj) => ((UIClass)instanceObj).SetMouseInteractive((bool)valueObj, true);

        private static object GetEnabled(object instanceObj) => BooleanBoxes.Box(((UIClass)instanceObj).Enabled);

        private static void SetEnabled(ref object instanceObj, object valueObj) => ((UIClass)instanceObj).Enabled = (bool)valueObj;

        private static object GetFullyEnabled(object instanceObj) => BooleanBoxes.Box(((UIClass)instanceObj).FullyEnabled);

        private static object GetAllowDoubleClicks(object instanceObj) => BooleanBoxes.Box(((UIClass)instanceObj).AllowDoubleClicks);

        private static void SetAllowDoubleClicks(ref object instanceObj, object valueObj) => ((UIClass)instanceObj).AllowDoubleClicks = (bool)valueObj;

        private static object GetPaintOrder(object instanceObj) => (object)(int)((UIClass)instanceObj).PaintOrder;

        private static void SetPaintOrder(ref object instanceObj, object valueObj)
        {
            UIClass uiClass = (UIClass)instanceObj;
            int num = (int)valueObj;
            Result result = Int32Schema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                uiClass.PaintOrder = (uint)num;
        }

        private static object CallDisposeOwnedObjectObject(object instanceObj, object[] parameters)
        {
            UIClass uiClass = (UIClass)instanceObj;
            object parameter = parameters[0];
            if (parameter == null)
                return (object)null;
            if (!(parameter is IDisposableObject disposable))
            {
                ErrorManager.ReportError("Attempt to dispose an object '{0}' that isn't disposable", (object)TypeSchema.NameFromInstance(parameter));
                return (object)null;
            }
            if (!uiClass.UnregisterDisposable(ref disposable))
            {
                ErrorManager.ReportError("Attempt to dispose an object '{0}' that '{1}' doesn't own", (object)TypeSchema.NameFromInstance((object)disposable), (object)uiClass.TypeSchema.Name);
                return (object)null;
            }
            disposable.Dispose((object)uiClass);
            return (object)null;
        }

        private static object CallNavigateInto(object instanceObj, object[] parameters)
        {
            ((UIClass)instanceObj).NavigateInto();
            return (object)null;
        }

        private static object CallNavigateIntoBoolean(object instanceObj, object[] parameters)
        {
            ((UIClass)instanceObj).NavigateInto((bool)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => UIStateSchema.Type = new UIXTypeSchema((short)230, "UIState", (string)null, (short)-1, typeof(UIClass), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)230, "CreateInterestOnFocus", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetCreateInterestOnFocus), new SetValueHandler(UIStateSchema.SetCreateInterestOnFocus), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)230, "Cursor", (short)44, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetCursor), new SetValueHandler(UIStateSchema.SetCursor), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)230, "DirectKeyFocus", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetDirectKeyFocus), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)230, "DirectMouseFocus", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetDirectMouseFocus), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)230, "FocusInterestTarget", (short)239, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetFocusInterestTarget), new SetValueHandler(UIStateSchema.SetFocusInterestTarget), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)230, "FocusInterestTargetMargins", (short)114, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetFocusInterestTargetMargins), new SetValueHandler(UIStateSchema.SetFocusInterestTargetMargins), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)230, "KeyFocus", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetKeyFocus), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)230, "KeyFocusOnMouseDown", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetKeyFocusOnMouseDown), new SetValueHandler(UIStateSchema.SetKeyFocusOnMouseDown), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)230, "KeyFocusOnMouseEnter", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetKeyFocusOnMouseEnter), new SetValueHandler(UIStateSchema.SetKeyFocusOnMouseEnter), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)230, "KeyInteractive", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetKeyInteractive), new SetValueHandler(UIStateSchema.SetKeyInteractive), false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema((short)230, "MouseFocus", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetMouseFocus), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema12 = new UIXPropertySchema((short)230, "MouseInteractive", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetMouseInteractive), new SetValueHandler(UIStateSchema.SetMouseInteractive), false);
            UIXPropertySchema uixPropertySchema13 = new UIXPropertySchema((short)230, "Enabled", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetEnabled), new SetValueHandler(UIStateSchema.SetEnabled), false);
            UIXPropertySchema uixPropertySchema14 = new UIXPropertySchema((short)230, "FullyEnabled", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetFullyEnabled), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema15 = new UIXPropertySchema((short)230, "AllowDoubleClicks", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UIStateSchema.GetAllowDoubleClicks), new SetValueHandler(UIStateSchema.SetAllowDoubleClicks), false);
            UIXPropertySchema uixPropertySchema16 = new UIXPropertySchema((short)230, "PaintOrder", (short)115, (short)-1, ExpressionRestriction.None, false, Int32Schema.ValidateNotNegative, false, new GetValueHandler(UIStateSchema.GetPaintOrder), new SetValueHandler(UIStateSchema.SetPaintOrder), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)230, "DisposeOwnedObject", new short[1]
            {
        (short) 153
            }, (short)240, new InvokeHandler(UIStateSchema.CallDisposeOwnedObjectObject), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)230, "NavigateInto", (short[])null, (short)240, new InvokeHandler(UIStateSchema.CallNavigateInto), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)230, "NavigateInto", new short[1]
            {
        (short) 15
            }, (short)240, new InvokeHandler(UIStateSchema.CallNavigateIntoBoolean), false);
            UIStateSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[16]
            {
        (PropertySchema) uixPropertySchema15,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema13,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema14,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema10,
        (PropertySchema) uixPropertySchema11,
        (PropertySchema) uixPropertySchema12,
        (PropertySchema) uixPropertySchema16
            }, new MethodSchema[3]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}