// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.SelectionManagerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;
using System.Collections;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class SelectionManagerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetCount(object instanceObj) => (object)((SelectionManager)instanceObj).Count;

        private static object GetSourceList(object instanceObj) => (object)((SelectionManager)instanceObj).SourceList;

        private static void SetSourceList(ref object instanceObj, object valueObj) => ((SelectionManager)instanceObj).SourceList = (IList)valueObj;

        private static object GetAnchor(object instanceObj) => (object)((SelectionManager)instanceObj).Anchor;

        private static void SetAnchor(ref object instanceObj, object valueObj) => ((SelectionManager)instanceObj).Anchor = (int)valueObj;

        private static object GetSelectedIndices(object instanceObj) => (object)((SelectionManager)instanceObj).SelectedIndices;

        private static object GetSelectedItems(object instanceObj) => (object)((SelectionManager)instanceObj).SelectedItems;

        private static object GetSingleSelect(object instanceObj) => BooleanBoxes.Box(((SelectionManager)instanceObj).SingleSelect);

        private static void SetSingleSelect(ref object instanceObj, object valueObj) => ((SelectionManager)instanceObj).SingleSelect = (bool)valueObj;

        private static object GetSelectedIndex(object instanceObj) => (object)((SelectionManager)instanceObj).SelectedIndex;

        private static void SetSelectedIndex(ref object instanceObj, object valueObj) => ((SelectionManager)instanceObj).SelectedIndex = (int)valueObj;

        private static object GetSelectedItem(object instanceObj) => ((SelectionManager)instanceObj).SelectedItem;

        private static object Construct() => (object)new SelectionManager();

        private static object CallIsSelectedInt32(object instanceObj, object[] parameters) => (object)((SelectionManager)instanceObj).IsSelected((int)parameters[0]);

        private static object CallIsRangeSelectedInt32Int32(object instanceObj, object[] parameters) => (object)((SelectionManager)instanceObj).IsRangeSelected((int)parameters[0], (int)parameters[1]);

        private static object CallClear(object instanceObj, object[] parameters)
        {
            ((SelectionManager)instanceObj).Clear();
            return (object)null;
        }

        private static object CallSelectInt32Boolean(object instanceObj, object[] parameters) => (object)((SelectionManager)instanceObj).Select((int)parameters[0], (bool)parameters[1]);

        private static object CallSelectListBoolean(object instanceObj, object[] parameters)
        {
            SelectionManager selectionManager = (SelectionManager)instanceObj;
            IList parameter1 = (IList)parameters[0];
            bool parameter2 = (bool)parameters[1];
            if (parameter1 == null)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"indices");
                return (object)null;
            }
            foreach (object obj in (IEnumerable)parameter1)
            {
                if (!(obj is int))
                {
                    ErrorManager.ReportError("Script runtime failure: Invalid value '{0}' within list '{1}'", obj, (object)"indices");
                    return (object)null;
                }
            }
            return (object)selectionManager.Select(parameter1, parameter2);
        }

        private static object CallToggleSelectInt32(object instanceObj, object[] parameters) => (object)((SelectionManager)instanceObj).ToggleSelect((int)parameters[0]);

        private static object CallToggleSelectList(object instanceObj, object[] parameters)
        {
            SelectionManager selectionManager = (SelectionManager)instanceObj;
            IList parameter = (IList)parameters[0];
            if (parameter == null)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"items");
                return (object)null;
            }
            foreach (object obj in (IEnumerable)parameter)
            {
                if (!(obj is int))
                {
                    ErrorManager.ReportError("Script runtime failure: Invalid value '{0}' within list '{1}'", obj, (object)"items");
                    return (object)null;
                }
            }
            return (object)selectionManager.ToggleSelect(parameter);
        }

        private static object CallSelectRangeInt32Int32(object instanceObj, object[] parameters) => (object)((SelectionManager)instanceObj).SelectRange((int)parameters[0], (int)parameters[1]);

        private static object CallSelectRangeFromAnchorInt32(object instanceObj, object[] parameters) => (object)((SelectionManager)instanceObj).SelectRangeFromAnchor((int)parameters[0]);

        private static object CallSelectRangeFromAnchorInt32Int32(
          object instanceObj,
          object[] parameters)
        {
            return (object)((SelectionManager)instanceObj).SelectRangeFromAnchor((int)parameters[0], (int)parameters[1]);
        }

        private static object CallToggleSelectRangeInt32Int32(object instanceObj, object[] parameters) => (object)((SelectionManager)instanceObj).ToggleSelectRange((int)parameters[0], (int)parameters[1]);

        public static void Pass1Initialize() => SelectionManagerSchema.Type = new UIXTypeSchema((short)186, "SelectionManager", (string)null, (short)153, typeof(SelectionManager), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)186, "Count", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(SelectionManagerSchema.GetCount), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)186, "SourceList", (short)138, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(SelectionManagerSchema.GetSourceList), new SetValueHandler(SelectionManagerSchema.SetSourceList), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)186, "Anchor", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(SelectionManagerSchema.GetAnchor), new SetValueHandler(SelectionManagerSchema.SetAnchor), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)186, "SelectedIndices", (short)138, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(SelectionManagerSchema.GetSelectedIndices), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)186, "SelectedItems", (short)138, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(SelectionManagerSchema.GetSelectedItems), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)186, "SingleSelect", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(SelectionManagerSchema.GetSingleSelect), new SetValueHandler(SelectionManagerSchema.SetSingleSelect), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)186, "SelectedIndex", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(SelectionManagerSchema.GetSelectedIndex), new SetValueHandler(SelectionManagerSchema.SetSelectedIndex), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)186, "SelectedItem", (short)153, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(SelectionManagerSchema.GetSelectedItem), (SetValueHandler)null, false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)186, "IsSelected", new short[1]
            {
        (short) 115
            }, (short)15, new InvokeHandler(SelectionManagerSchema.CallIsSelectedInt32), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)186, "IsRangeSelected", new short[2]
            {
        (short) 115,
        (short) 115
            }, (short)15, new InvokeHandler(SelectionManagerSchema.CallIsRangeSelectedInt32Int32), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)186, "Clear", (short[])null, (short)240, new InvokeHandler(SelectionManagerSchema.CallClear), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)186, "Select", new short[2]
            {
        (short) 115,
        (short) 15
            }, (short)15, new InvokeHandler(SelectionManagerSchema.CallSelectInt32Boolean), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)186, "Select", new short[2]
            {
        (short) 138,
        (short) 15
            }, (short)15, new InvokeHandler(SelectionManagerSchema.CallSelectListBoolean), false);
            UIXMethodSchema uixMethodSchema6 = new UIXMethodSchema((short)186, "ToggleSelect", new short[1]
            {
        (short) 115
            }, (short)15, new InvokeHandler(SelectionManagerSchema.CallToggleSelectInt32), false);
            UIXMethodSchema uixMethodSchema7 = new UIXMethodSchema((short)186, "ToggleSelect", new short[1]
            {
        (short) 138
            }, (short)15, new InvokeHandler(SelectionManagerSchema.CallToggleSelectList), false);
            UIXMethodSchema uixMethodSchema8 = new UIXMethodSchema((short)186, "SelectRange", new short[2]
            {
        (short) 115,
        (short) 115
            }, (short)15, new InvokeHandler(SelectionManagerSchema.CallSelectRangeInt32Int32), false);
            UIXMethodSchema uixMethodSchema9 = new UIXMethodSchema((short)186, "SelectRangeFromAnchor", new short[1]
            {
        (short) 115
            }, (short)15, new InvokeHandler(SelectionManagerSchema.CallSelectRangeFromAnchorInt32), false);
            UIXMethodSchema uixMethodSchema10 = new UIXMethodSchema((short)186, "SelectRangeFromAnchor", new short[2]
            {
        (short) 115,
        (short) 115
            }, (short)15, new InvokeHandler(SelectionManagerSchema.CallSelectRangeFromAnchorInt32Int32), false);
            UIXMethodSchema uixMethodSchema11 = new UIXMethodSchema((short)186, "ToggleSelectRange", new short[2]
            {
        (short) 115,
        (short) 115
            }, (short)15, new InvokeHandler(SelectionManagerSchema.CallToggleSelectRangeInt32Int32), false);
            SelectionManagerSchema.Type.Initialize(new DefaultConstructHandler(SelectionManagerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[8]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[11]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4,
        (MethodSchema) uixMethodSchema5,
        (MethodSchema) uixMethodSchema6,
        (MethodSchema) uixMethodSchema7,
        (MethodSchema) uixMethodSchema8,
        (MethodSchema) uixMethodSchema9,
        (MethodSchema) uixMethodSchema10,
        (MethodSchema) uixMethodSchema11
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
