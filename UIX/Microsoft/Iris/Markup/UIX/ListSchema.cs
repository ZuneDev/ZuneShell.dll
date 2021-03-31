// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ListSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;
using System.Collections;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ListSchema
    {
        public static UIXTypeSchema Type;

        private static object GetCount(object instanceObj) => (object)((ICollection)instanceObj).Count;

        private static object GetSource(object instanceObj) => (object)(IList)instanceObj;

        private static object GetCanSearch(object instanceObj) => (IList)instanceObj is IUIList uiList ? (object)uiList.CanSearch : (object)false;

        private static object Construct() => (object)new NotifyList();

        private static object CallIsNullOrEmptyList(object instanceObj, object[] parameters)
        {
            IList parameter = (IList)parameters[0];
            return BooleanBoxes.Box(parameter == null || parameter.Count == 0);
        }

        private static object Callget_ItemInt32(object instanceObj, object[] parameters)
        {
            IList list = (IList)instanceObj;
            int parameter = (int)parameters[0];
            if (parameter >= 0 && parameter < list.Count)
                return list[parameter];
            ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)parameter, (object)"index");
            return (object)null;
        }

        private static object Callset_ItemInt32Object(object instanceObj, object[] parameters)
        {
            IList list = (IList)instanceObj;
            int parameter1 = (int)parameters[0];
            object parameter2 = parameters[1];
            if (parameter1 < 0 || parameter1 >= list.Count)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)parameter1, (object)"index");
                return (object)null;
            }
            list[parameter1] = parameter2;
            return (object)null;
        }

        private static object CallClear(object instanceObj, object[] parameters)
        {
            ((IList)instanceObj).Clear();
            return (object)null;
        }

        private static object CallAddObject(object instanceObj, object[] parameters)
        {
            ((IList)instanceObj).Add(parameters[0]);
            return (object)null;
        }

        private static object CallRemoveObject(object instanceObj, object[] parameters)
        {
            ((IList)instanceObj).Remove(parameters[0]);
            return (object)null;
        }

        private static object CallContainsObject(object instanceObj, object[] parameters) => (object)((IList)instanceObj).Contains(parameters[0]);

        private static object CallIndexOfObject(object instanceObj, object[] parameters) => (object)((IList)instanceObj).IndexOf(parameters[0]);

        private static object CallInsertInt32Object(object instanceObj, object[] parameters)
        {
            IList list = (IList)instanceObj;
            int parameter1 = (int)parameters[0];
            object parameter2 = parameters[1];
            if (parameter1 < 0 || parameter1 > list.Count)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)parameter1, (object)"index");
                return (object)null;
            }
            list.Insert(parameter1, parameter2);
            return (object)null;
        }

        private static object CallRemoveAtInt32(object instanceObj, object[] parameters)
        {
            IList list = (IList)instanceObj;
            int parameter = (int)parameters[0];
            if (parameter < 0 || parameter >= list.Count)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)parameter, (object)"index");
                return (object)null;
            }
            list.RemoveAt(parameter);
            return (object)null;
        }

        private static object CallSearchForStringString(object instanceObj, object[] parameters)
        {
            IList list = (IList)instanceObj;
            string parameter = (string)parameters[0];
            return list is IUIList uiList ? (object)uiList.SearchForString(parameter) : (object)-1;
        }

        private static object CallMoveInt32Int32(object instanceObj, object[] parameters)
        {
            IList list = (IList)instanceObj;
            int parameter1 = (int)parameters[0];
            int parameter2 = (int)parameters[1];
            if (parameter1 < 0 || parameter1 >= list.Count)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)parameter1, (object)"oldIndex");
                return (object)null;
            }
            if (parameter2 < 0 || parameter2 >= list.Count)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)parameter2, (object)"newIndex");
                return (object)null;
            }
            switch (list)
            {
                case IUIList uiList:
                    uiList.Move(parameter1, parameter2);
                    break;
                case INotifyList notifyList:
                    notifyList.Move(parameter1, parameter2);
                    break;
                default:
                    object obj = list[parameter1];
                    list.RemoveAt(parameter1);
                    list.Insert(parameter2, obj);
                    break;
            }
            return (object)null;
        }

        private static object CallGetEnumerator(object instanceObj, object[] parameters) => (object)((IEnumerable)instanceObj).GetEnumerator();

        public static void Pass1Initialize() => ListSchema.Type = new UIXTypeSchema((short)138, "List", (string)null, (short)153, typeof(IList), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)138, "Count", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ListSchema.GetCount), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)138, "Source", (short)138, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ListSchema.GetSource), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)138, "CanSearch", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ListSchema.GetCanSearch), (SetValueHandler)null, false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)138, "IsNullOrEmpty", new short[1]
            {
        (short) 138
            }, (short)15, new InvokeHandler(ListSchema.CallIsNullOrEmptyList), true);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)138, "get_Item", new short[1]
            {
        (short) 115
            }, (short)153, new InvokeHandler(ListSchema.Callget_ItemInt32), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)138, "set_Item", new short[2]
            {
        (short) 115,
        (short) 153
            }, (short)240, new InvokeHandler(ListSchema.Callset_ItemInt32Object), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)138, "Clear", (short[])null, (short)240, new InvokeHandler(ListSchema.CallClear), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)138, "Add", new short[1]
            {
        (short) 153
            }, (short)240, new InvokeHandler(ListSchema.CallAddObject), false);
            UIXMethodSchema uixMethodSchema6 = new UIXMethodSchema((short)138, "Remove", new short[1]
            {
        (short) 153
            }, (short)240, new InvokeHandler(ListSchema.CallRemoveObject), false);
            UIXMethodSchema uixMethodSchema7 = new UIXMethodSchema((short)138, "Contains", new short[1]
            {
        (short) 153
            }, (short)15, new InvokeHandler(ListSchema.CallContainsObject), false);
            UIXMethodSchema uixMethodSchema8 = new UIXMethodSchema((short)138, "IndexOf", new short[1]
            {
        (short) 153
            }, (short)115, new InvokeHandler(ListSchema.CallIndexOfObject), false);
            UIXMethodSchema uixMethodSchema9 = new UIXMethodSchema((short)138, "Insert", new short[2]
            {
        (short) 115,
        (short) 153
            }, (short)240, new InvokeHandler(ListSchema.CallInsertInt32Object), false);
            UIXMethodSchema uixMethodSchema10 = new UIXMethodSchema((short)138, "RemoveAt", new short[1]
            {
        (short) 115
            }, (short)240, new InvokeHandler(ListSchema.CallRemoveAtInt32), false);
            UIXMethodSchema uixMethodSchema11 = new UIXMethodSchema((short)138, "SearchForString", new short[1]
            {
        (short) 208
            }, (short)115, new InvokeHandler(ListSchema.CallSearchForStringString), false);
            UIXMethodSchema uixMethodSchema12 = new UIXMethodSchema((short)138, "Move", new short[2]
            {
        (short) 115,
        (short) 115
            }, (short)240, new InvokeHandler(ListSchema.CallMoveInt32Int32), false);
            UIXMethodSchema uixMethodSchema13 = new UIXMethodSchema((short)138, "GetEnumerator", (short[])null, (short)86, new InvokeHandler(ListSchema.CallGetEnumerator), false);
            ListSchema.Type.Initialize(new DefaultConstructHandler(ListSchema.Construct), (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[13]
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
        (MethodSchema) uixMethodSchema11,
        (MethodSchema) uixMethodSchema12,
        (MethodSchema) uixMethodSchema13
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
