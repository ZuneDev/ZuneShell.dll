// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DictionarySchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Session;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DictionarySchema
    {
        public static UIXTypeSchema Type;

        private static object GetSource(object instanceObj) => (object)(IDictionary)instanceObj;

        private static object Construct() => (object)new Dictionary<object, object>();

        private static object Callget_ItemObject(object instanceObj, object[] parameters)
        {
            IDictionary dictionary = (IDictionary)instanceObj;
            object parameter = parameters[0];
            if (parameter != null)
                return dictionary[parameter];
            ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"key");
            return (object)null;
        }

        private static object CallContainsObject(object instanceObj, object[] parameters)
        {
            IDictionary dictionary = (IDictionary)instanceObj;
            object parameter = parameters[0];
            if (parameter != null)
                return BooleanBoxes.Box(dictionary.Contains(parameter));
            ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"key");
            return (object)false;
        }

        private static object Callset_ItemObjectObject(object instanceObj, object[] parameters)
        {
            IDictionary dictionary = (IDictionary)instanceObj;
            object parameter1 = parameters[0];
            object parameter2 = parameters[1];
            if (parameter1 == null)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"key");
                return (object)null;
            }
            dictionary[parameter1] = parameter2;
            return (object)null;
        }

        public static void Pass1Initialize() => DictionarySchema.Type = new UIXTypeSchema((short)58, "Dictionary", (string)null, (short)153, typeof(IDictionary), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)58, "Source", (short)58, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(DictionarySchema.GetSource), (SetValueHandler)null, false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)58, "get_Item", new short[1]
            {
        (short) 153
            }, (short)153, new InvokeHandler(DictionarySchema.Callget_ItemObject), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)58, "Contains", new short[1]
            {
        (short) 153
            }, (short)15, new InvokeHandler(DictionarySchema.CallContainsObject), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)58, "set_Item", new short[2]
            {
        (short) 153,
        (short) 153
            }, (short)240, new InvokeHandler(DictionarySchema.Callset_ItemObjectObject), false);
            DictionarySchema.Type.Initialize(new DefaultConstructHandler(DictionarySchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, new MethodSchema[3]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
