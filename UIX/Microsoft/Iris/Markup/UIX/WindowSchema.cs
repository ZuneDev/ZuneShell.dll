// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.WindowSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class WindowSchema
    {
        public static UIXTypeSchema Type;

        private static object GetMainWindow(object instanceObj) => (object)UISession.Default.Form;

        private static object GetCaption(object instanceObj) => (object)((UIForm)instanceObj).Caption;

        private static void SetCaption(ref object instanceObj, object valueObj) => ((UIForm)instanceObj).Caption = (string)valueObj;

        private static object GetWindowState(object instanceObj) => (object)((Form)instanceObj).WindowState;

        private static void SetWindowState(ref object instanceObj, object valueObj) => ((Form)instanceObj).WindowState = (Microsoft.Iris.WindowState)valueObj;

        private static object GetActive(object instanceObj) => BooleanBoxes.Box(((Form)instanceObj).ActivationState);

        private static object GetMouseActive(object instanceObj) => BooleanBoxes.Box(!((UIForm)instanceObj).MouseIsIdle);

        private static object GetShowWindowFrame(object instanceObj) => BooleanBoxes.Box(((UIForm)instanceObj).ShowWindowFrame);

        private static void SetShowWindowFrame(ref object instanceObj, object valueObj) => ((UIForm)instanceObj).ShowWindowFrame = (bool)valueObj;

        private static object GetHideMouseOnIdle(object instanceObj) => BooleanBoxes.Box(((UIForm)instanceObj).HideMouseOnIdle);

        private static void SetHideMouseOnIdle(ref object instanceObj, object valueObj) => ((UIForm)instanceObj).HideMouseOnIdle = (bool)valueObj;

        private static object GetMouseIdleTimeout(object instanceObj) => (object)((UIForm)instanceObj).MouseIdleTimeout;

        private static void SetMouseIdleTimeout(ref object instanceObj, object valueObj)
        {
            UIForm uiForm = (UIForm)instanceObj;
            int num = (int)valueObj;
            Result result = Int32Schema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                uiForm.MouseIdleTimeout = num;
        }

        private static object GetAlwaysOnTop(object instanceObj) => BooleanBoxes.Box(((UIForm)instanceObj).AlwaysOnTop);

        private static void SetAlwaysOnTop(ref object instanceObj, object valueObj) => ((UIForm)instanceObj).AlwaysOnTop = (bool)valueObj;

        private static object GetShowInTaskbar(object instanceObj) => BooleanBoxes.Box(((UIForm)instanceObj).ShowInTaskbar);

        private static void SetShowInTaskbar(ref object instanceObj, object valueObj) => ((UIForm)instanceObj).ShowInTaskbar = (bool)valueObj;

        private static object GetPreventInterruption(object instanceObj) => BooleanBoxes.Box(((UIForm)instanceObj).PreventInterruption);

        private static void SetPreventInterruption(ref object instanceObj, object valueObj) => ((UIForm)instanceObj).PreventInterruption = (bool)valueObj;

        private static object GetMaximizeMode(object instanceObj) => (object)((UIForm)instanceObj).MaximizeMode;

        private static void SetMaximizeMode(ref object instanceObj, object valueObj) => ((UIForm)instanceObj).MaximizeMode = (MaximizeMode)valueObj;

        private static object GetClientSize(object instanceObj) => (object)((Form)instanceObj).ClientSize;

        private static void SetClientSize(ref object instanceObj, object valueObj) => ((Form)instanceObj).ClientSize = (Size)valueObj;

        private static object GetPosition(object instanceObj) => (object)((Form)instanceObj).Position;

        private static void SetPosition(ref object instanceObj, object valueObj) => ((Form)instanceObj).Position = (Point)valueObj;

        private static object GetVisible(object instanceObj) => BooleanBoxes.Box(((Form)instanceObj).Visible);

        private static void SetVisible(ref object instanceObj, object valueObj) => ((Form)instanceObj).Visible = (bool)valueObj;

        private static object Construct() => (object)UISession.Default.Form;

        private static object CallClose(object instanceObj, object[] parameters)
        {
            ((UIForm)instanceObj).Close();
            return (object)null;
        }

        private static object CallForceClose(object instanceObj, object[] parameters)
        {
            ((Form)instanceObj).ForceClose();
            return (object)null;
        }

        private static object CallSaveKeyFocus(object instanceObj, object[] parameters) => (object)((UIForm)instanceObj).SaveKeyFocus();

        private static object CallRestoreKeyFocusSavedKeyFocus(object instanceObj, object[] parameters)
        {
            ((UIForm)instanceObj).RestoreKeyFocus((SavedKeyFocus)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => WindowSchema.Type = new UIXTypeSchema((short)241, "Window", (string)null, (short)153, typeof(UIForm), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)241, "MainWindow", (short)241, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(WindowSchema.GetMainWindow), (SetValueHandler)null, true);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)241, "Caption", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetCaption), new SetValueHandler(WindowSchema.SetCaption), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)241, "WindowState", (short)242, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetWindowState), new SetValueHandler(WindowSchema.SetWindowState), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)241, "Active", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetActive), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)241, "MouseActive", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetMouseActive), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)241, "ShowWindowFrame", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetShowWindowFrame), new SetValueHandler(WindowSchema.SetShowWindowFrame), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)241, "HideMouseOnIdle", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetHideMouseOnIdle), new SetValueHandler(WindowSchema.SetHideMouseOnIdle), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)241, "MouseIdleTimeout", (short)115, (short)-1, ExpressionRestriction.None, false, Int32Schema.ValidateNotNegative, true, new GetValueHandler(WindowSchema.GetMouseIdleTimeout), new SetValueHandler(WindowSchema.SetMouseIdleTimeout), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)241, "AlwaysOnTop", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetAlwaysOnTop), new SetValueHandler(WindowSchema.SetAlwaysOnTop), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)241, "ShowInTaskbar", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetShowInTaskbar), new SetValueHandler(WindowSchema.SetShowInTaskbar), false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema((short)241, "PreventInterruption", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetPreventInterruption), new SetValueHandler(WindowSchema.SetPreventInterruption), false);
            UIXPropertySchema uixPropertySchema12 = new UIXPropertySchema((short)241, "MaximizeMode", (short)146, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetMaximizeMode), new SetValueHandler(WindowSchema.SetMaximizeMode), false);
            UIXPropertySchema uixPropertySchema13 = new UIXPropertySchema((short)241, "ClientSize", (short)195, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetClientSize), new SetValueHandler(WindowSchema.SetClientSize), false);
            UIXPropertySchema uixPropertySchema14 = new UIXPropertySchema((short)241, "Position", (short)158, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetPosition), new SetValueHandler(WindowSchema.SetPosition), false);
            UIXPropertySchema uixPropertySchema15 = new UIXPropertySchema((short)241, "Visible", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(WindowSchema.GetVisible), new SetValueHandler(WindowSchema.SetVisible), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)241, "Close", (short[])null, (short)240, new InvokeHandler(WindowSchema.CallClose), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)241, "ForceClose", (short[])null, (short)240, new InvokeHandler(WindowSchema.CallForceClose), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)241, "SaveKeyFocus", (short[])null, (short)177, new InvokeHandler(WindowSchema.CallSaveKeyFocus), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)241, "RestoreKeyFocus", new short[1]
            {
        (short) 177
            }, (short)240, new InvokeHandler(WindowSchema.CallRestoreKeyFocusSavedKeyFocus), false);
            WindowSchema.Type.Initialize(new DefaultConstructHandler(WindowSchema.Construct), (ConstructorSchema[])null, new PropertySchema[15]
            {
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema13,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema12,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema14,
        (PropertySchema) uixPropertySchema11,
        (PropertySchema) uixPropertySchema10,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema15,
        (PropertySchema) uixPropertySchema3
            }, new MethodSchema[4]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
