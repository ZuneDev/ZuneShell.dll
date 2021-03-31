// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.HostSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Session;
using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class HostSchema
    {
        private static object[] s_paramsList = new object[10];
        public static UIXTypeSchema Type;

        private static object GetNewContentOnTop(object instanceObj) => BooleanBoxes.Box(((Host)instanceObj).NewContentOnTop);

        private static void SetNewContentOnTop(ref object instanceObj, object valueObj) => ((Host)instanceObj).NewContentOnTop = (bool)valueObj;

        private static object GetSource(object instanceObj) => (object)((Host)instanceObj).Source;

        private static object GetSourceType(object instanceObj) => (object)((Host)instanceObj).SourceType;

        private static object GetStatus(object instanceObj) => (object)((Host)instanceObj).Status;

        private static object GetInputEnabled(object instanceObj) => BooleanBoxes.Box(((Host)instanceObj).InputEnabled);

        private static void SetInputEnabled(ref object instanceObj, object valueObj) => ((Host)instanceObj).InputEnabled = (bool)valueObj;

        private static object GetUnloadable(object instanceObj) => BooleanBoxes.Box(((Host)instanceObj).Unloadable);

        private static void SetUnloadable(ref object instanceObj, object valueObj) => ((Host)instanceObj).Unloadable = (bool)valueObj;

        private static object Construct() => (object)new Host();

        private static object CallUnloadAll(object instanceObj, object[] parameters)
        {
            ((Host)instanceObj).UnloadAll();
            return (object)null;
        }

        private static object CallForceRefresh(object instanceObj, object[] parameters)
        {
            ((Host)instanceObj).ForceRefresh();
            return (object)null;
        }

        private static object CallForceRefreshBoolean(object instanceObj, object[] parameters)
        {
            ((Host)instanceObj).ForceRefresh((bool)parameters[0]);
            return (object)null;
        }

        private static object CallRequestSourceString(object instanceObj, object[] parameters)
        {
            ((Host)instanceObj).RequestSource((string)parameters[0], (TypeSchema)null, (Vector<UIPropertyRecord>)null);
            return (object)null;
        }

        private static object CallRequestSourceStringStringObject(
          object instanceObj,
          object[] parameters)
        {
            Host instance = (Host)instanceObj;
            string parameter1 = (string)parameters[0];
            string parameter2 = (string)parameters[1];
            object parameter3 = parameters[2];
            HostSchema.s_paramsList[0] = (object)parameter2;
            HostSchema.s_paramsList[1] = parameter3;
            HostSchema.RequestSourceCallBuilder(instance, parameter1, (TypeSchema)null, 1);
            return (object)null;
        }

        private static object CallRequestSourceStringStringObjectStringObject(
          object instanceObj,
          object[] parameters)
        {
            Host instance = (Host)instanceObj;
            string parameter1 = (string)parameters[0];
            string parameter2 = (string)parameters[1];
            object parameter3 = parameters[2];
            string parameter4 = (string)parameters[3];
            object parameter5 = parameters[4];
            HostSchema.s_paramsList[0] = (object)parameter2;
            HostSchema.s_paramsList[1] = parameter3;
            HostSchema.s_paramsList[2] = (object)parameter4;
            HostSchema.s_paramsList[3] = parameter5;
            HostSchema.RequestSourceCallBuilder(instance, parameter1, (TypeSchema)null, 2);
            return (object)null;
        }

        private static object CallRequestSourceStringStringObjectStringObjectStringObject(
          object instanceObj,
          object[] parameters)
        {
            Host instance = (Host)instanceObj;
            string parameter1 = (string)parameters[0];
            string parameter2 = (string)parameters[1];
            object parameter3 = parameters[2];
            string parameter4 = (string)parameters[3];
            object parameter5 = parameters[4];
            string parameter6 = (string)parameters[5];
            object parameter7 = parameters[6];
            HostSchema.s_paramsList[0] = (object)parameter2;
            HostSchema.s_paramsList[1] = parameter3;
            HostSchema.s_paramsList[2] = (object)parameter4;
            HostSchema.s_paramsList[3] = parameter5;
            HostSchema.s_paramsList[4] = (object)parameter6;
            HostSchema.s_paramsList[5] = parameter7;
            HostSchema.RequestSourceCallBuilder(instance, parameter1, (TypeSchema)null, 3);
            return (object)null;
        }

        private static object CallRequestSourceStringStringObjectStringObjectStringObjectStringObject(
          object instanceObj,
          object[] parameters)
        {
            Host instance = (Host)instanceObj;
            string parameter1 = (string)parameters[0];
            string parameter2 = (string)parameters[1];
            object parameter3 = parameters[2];
            string parameter4 = (string)parameters[3];
            object parameter5 = parameters[4];
            string parameter6 = (string)parameters[5];
            object parameter7 = parameters[6];
            string parameter8 = (string)parameters[7];
            object parameter9 = parameters[8];
            HostSchema.s_paramsList[0] = (object)parameter2;
            HostSchema.s_paramsList[1] = parameter3;
            HostSchema.s_paramsList[2] = (object)parameter4;
            HostSchema.s_paramsList[3] = parameter5;
            HostSchema.s_paramsList[4] = (object)parameter6;
            HostSchema.s_paramsList[5] = parameter7;
            HostSchema.s_paramsList[6] = (object)parameter8;
            HostSchema.s_paramsList[7] = parameter9;
            HostSchema.RequestSourceCallBuilder(instance, parameter1, (TypeSchema)null, 4);
            return (object)null;
        }

        private static object CallRequestSourceStringStringObjectStringObjectStringObjectStringObjectStringObject(
          object instanceObj,
          object[] parameters)
        {
            Host instance = (Host)instanceObj;
            string parameter1 = (string)parameters[0];
            string parameter2 = (string)parameters[1];
            object parameter3 = parameters[2];
            string parameter4 = (string)parameters[3];
            object parameter5 = parameters[4];
            string parameter6 = (string)parameters[5];
            object parameter7 = parameters[6];
            string parameter8 = (string)parameters[7];
            object parameter9 = parameters[8];
            string parameter10 = (string)parameters[9];
            object parameter11 = parameters[10];
            HostSchema.s_paramsList[0] = (object)parameter2;
            HostSchema.s_paramsList[1] = parameter3;
            HostSchema.s_paramsList[2] = (object)parameter4;
            HostSchema.s_paramsList[3] = parameter5;
            HostSchema.s_paramsList[4] = (object)parameter6;
            HostSchema.s_paramsList[5] = parameter7;
            HostSchema.s_paramsList[6] = (object)parameter8;
            HostSchema.s_paramsList[7] = parameter9;
            HostSchema.s_paramsList[8] = (object)parameter10;
            HostSchema.s_paramsList[9] = parameter11;
            HostSchema.RequestSourceCallBuilder(instance, parameter1, (TypeSchema)null, 5);
            return (object)null;
        }

        private static object CallRequestSourceType(object instanceObj, object[] parameters)
        {
            ((Host)instanceObj).RequestSource((string)null, (TypeSchema)parameters[0], (Vector<UIPropertyRecord>)null);
            return (object)null;
        }

        private static object CallRequestSourceTypeStringObject(object instanceObj, object[] parameters)
        {
            Host instance = (Host)instanceObj;
            TypeSchema parameter1 = (TypeSchema)parameters[0];
            string parameter2 = (string)parameters[1];
            object parameter3 = parameters[2];
            HostSchema.s_paramsList[0] = (object)parameter2;
            HostSchema.s_paramsList[1] = parameter3;
            HostSchema.RequestSourceCallBuilder(instance, (string)null, parameter1, 1);
            return (object)null;
        }

        private static object CallRequestSourceTypeStringObjectStringObject(
          object instanceObj,
          object[] parameters)
        {
            Host instance = (Host)instanceObj;
            TypeSchema parameter1 = (TypeSchema)parameters[0];
            string parameter2 = (string)parameters[1];
            object parameter3 = parameters[2];
            string parameter4 = (string)parameters[3];
            object parameter5 = parameters[4];
            HostSchema.s_paramsList[0] = (object)parameter2;
            HostSchema.s_paramsList[1] = parameter3;
            HostSchema.s_paramsList[2] = (object)parameter4;
            HostSchema.s_paramsList[3] = parameter5;
            HostSchema.RequestSourceCallBuilder(instance, (string)null, parameter1, 2);
            return (object)null;
        }

        private static object CallRequestSourceTypeStringObjectStringObjectStringObject(
          object instanceObj,
          object[] parameters)
        {
            Host instance = (Host)instanceObj;
            TypeSchema parameter1 = (TypeSchema)parameters[0];
            string parameter2 = (string)parameters[1];
            object parameter3 = parameters[2];
            string parameter4 = (string)parameters[3];
            object parameter5 = parameters[4];
            string parameter6 = (string)parameters[5];
            object parameter7 = parameters[6];
            HostSchema.s_paramsList[0] = (object)parameter2;
            HostSchema.s_paramsList[1] = parameter3;
            HostSchema.s_paramsList[2] = (object)parameter4;
            HostSchema.s_paramsList[3] = parameter5;
            HostSchema.s_paramsList[4] = (object)parameter6;
            HostSchema.s_paramsList[5] = parameter7;
            HostSchema.RequestSourceCallBuilder(instance, (string)null, parameter1, 3);
            return (object)null;
        }

        private static object CallRequestSourceTypeStringObjectStringObjectStringObjectStringObject(
          object instanceObj,
          object[] parameters)
        {
            Host instance = (Host)instanceObj;
            TypeSchema parameter1 = (TypeSchema)parameters[0];
            string parameter2 = (string)parameters[1];
            object parameter3 = parameters[2];
            string parameter4 = (string)parameters[3];
            object parameter5 = parameters[4];
            string parameter6 = (string)parameters[5];
            object parameter7 = parameters[6];
            string parameter8 = (string)parameters[7];
            object parameter9 = parameters[8];
            HostSchema.s_paramsList[0] = (object)parameter2;
            HostSchema.s_paramsList[1] = parameter3;
            HostSchema.s_paramsList[2] = (object)parameter4;
            HostSchema.s_paramsList[3] = parameter5;
            HostSchema.s_paramsList[4] = (object)parameter6;
            HostSchema.s_paramsList[5] = parameter7;
            HostSchema.s_paramsList[6] = (object)parameter8;
            HostSchema.s_paramsList[7] = parameter9;
            HostSchema.RequestSourceCallBuilder(instance, (string)null, parameter1, 4);
            return (object)null;
        }

        private static object CallRequestSourceTypeStringObjectStringObjectStringObjectStringObjectStringObject(
          object instanceObj,
          object[] parameters)
        {
            Host instance = (Host)instanceObj;
            TypeSchema parameter1 = (TypeSchema)parameters[0];
            string parameter2 = (string)parameters[1];
            object parameter3 = parameters[2];
            string parameter4 = (string)parameters[3];
            object parameter5 = parameters[4];
            string parameter6 = (string)parameters[5];
            object parameter7 = parameters[6];
            string parameter8 = (string)parameters[7];
            object parameter9 = parameters[8];
            string parameter10 = (string)parameters[9];
            object parameter11 = parameters[10];
            HostSchema.s_paramsList[0] = (object)parameter2;
            HostSchema.s_paramsList[1] = parameter3;
            HostSchema.s_paramsList[2] = (object)parameter4;
            HostSchema.s_paramsList[3] = parameter5;
            HostSchema.s_paramsList[4] = (object)parameter6;
            HostSchema.s_paramsList[5] = parameter7;
            HostSchema.s_paramsList[6] = (object)parameter8;
            HostSchema.s_paramsList[7] = parameter9;
            HostSchema.s_paramsList[8] = (object)parameter10;
            HostSchema.s_paramsList[9] = parameter11;
            HostSchema.RequestSourceCallBuilder(instance, (string)null, parameter1, 5);
            return (object)null;
        }

        private static void RequestSourceCallBuilder(
          Host instance,
          string source,
          TypeSchema type,
          int numPairs)
        {
            ErrorWatermark watermark = ErrorManager.Watermark;
            Vector<UIPropertyRecord> vector = new Vector<UIPropertyRecord>(numPairs);
            for (int index = 0; index < numPairs; ++index)
            {
                string name = (string)HostSchema.s_paramsList[index * 2];
                object obj = HostSchema.s_paramsList[index * 2 + 1];
                if (name == null)
                    ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)("property" + index.ToString()));
                else
                    UIPropertyRecord.AddToList(vector, name, obj);
            }
            for (int index = 0; index < numPairs * 2; ++index)
                HostSchema.s_paramsList[index] = (object)null;
            if (watermark.ErrorsDetected)
                return;
            instance.RequestSource(source, type, vector);
        }

        public static void Pass1Initialize() => HostSchema.Type = new UIXTypeSchema((short)101, "Host", (string)null, (short)239, typeof(Host), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)101, "NewContentOnTop", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(HostSchema.GetNewContentOnTop), new SetValueHandler(HostSchema.SetNewContentOnTop), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)101, "Source", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(HostSchema.GetSource), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)101, "SourceType", (short)225, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(HostSchema.GetSourceType), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)101, "Status", (short)102, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(HostSchema.GetStatus), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)101, "InputEnabled", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(HostSchema.GetInputEnabled), new SetValueHandler(HostSchema.SetInputEnabled), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)101, "Unloadable", (short)15, (short)-1, ExpressionRestriction.ReadOnly, false, (RangeValidator)null, true, new GetValueHandler(HostSchema.GetUnloadable), new SetValueHandler(HostSchema.SetUnloadable), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)101, "UnloadAll", (short[])null, (short)240, new InvokeHandler(HostSchema.CallUnloadAll), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)101, "ForceRefresh", (short[])null, (short)240, new InvokeHandler(HostSchema.CallForceRefresh), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)101, "ForceRefresh", new short[1]
            {
        (short) 15
            }, (short)240, new InvokeHandler(HostSchema.CallForceRefreshBoolean), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)101, "RequestSource", new short[1]
            {
        (short) 208
            }, (short)240, new InvokeHandler(HostSchema.CallRequestSourceString), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)101, "RequestSource", new short[3]
            {
        (short) 208,
        (short) 208,
        (short) 153
            }, (short)240, new InvokeHandler(HostSchema.CallRequestSourceStringStringObject), false);
            UIXMethodSchema uixMethodSchema6 = new UIXMethodSchema((short)101, "RequestSource", new short[5]
            {
        (short) 208,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153
            }, (short)240, new InvokeHandler(HostSchema.CallRequestSourceStringStringObjectStringObject), false);
            UIXMethodSchema uixMethodSchema7 = new UIXMethodSchema((short)101, "RequestSource", new short[7]
            {
        (short) 208,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153
            }, (short)240, new InvokeHandler(HostSchema.CallRequestSourceStringStringObjectStringObjectStringObject), false);
            UIXMethodSchema uixMethodSchema8 = new UIXMethodSchema((short)101, "RequestSource", new short[9]
            {
        (short) 208,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153
            }, (short)240, new InvokeHandler(HostSchema.CallRequestSourceStringStringObjectStringObjectStringObjectStringObject), false);
            UIXMethodSchema uixMethodSchema9 = new UIXMethodSchema((short)101, "RequestSource", new short[11]
            {
        (short) 208,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153
            }, (short)240, new InvokeHandler(HostSchema.CallRequestSourceStringStringObjectStringObjectStringObjectStringObjectStringObject), false);
            UIXMethodSchema uixMethodSchema10 = new UIXMethodSchema((short)101, "RequestSource", new short[1]
            {
        (short) 225
            }, (short)240, new InvokeHandler(HostSchema.CallRequestSourceType), false);
            UIXMethodSchema uixMethodSchema11 = new UIXMethodSchema((short)101, "RequestSource", new short[3]
            {
        (short) 225,
        (short) 208,
        (short) 153
            }, (short)240, new InvokeHandler(HostSchema.CallRequestSourceTypeStringObject), false);
            UIXMethodSchema uixMethodSchema12 = new UIXMethodSchema((short)101, "RequestSource", new short[5]
            {
        (short) 225,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153
            }, (short)240, new InvokeHandler(HostSchema.CallRequestSourceTypeStringObjectStringObject), false);
            UIXMethodSchema uixMethodSchema13 = new UIXMethodSchema((short)101, "RequestSource", new short[7]
            {
        (short) 225,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153
            }, (short)240, new InvokeHandler(HostSchema.CallRequestSourceTypeStringObjectStringObjectStringObject), false);
            UIXMethodSchema uixMethodSchema14 = new UIXMethodSchema((short)101, "RequestSource", new short[9]
            {
        (short) 225,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153
            }, (short)240, new InvokeHandler(HostSchema.CallRequestSourceTypeStringObjectStringObjectStringObjectStringObject), false);
            UIXMethodSchema uixMethodSchema15 = new UIXMethodSchema((short)101, "RequestSource", new short[11]
            {
        (short) 225,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153,
        (short) 208,
        (short) 153
            }, (short)240, new InvokeHandler(HostSchema.CallRequestSourceTypeStringObjectStringObjectStringObjectStringObjectStringObject), false);
            HostSchema.Type.Initialize(new DefaultConstructHandler(HostSchema.Construct), (ConstructorSchema[])null, new PropertySchema[6]
            {
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema6
            }, new MethodSchema[15]
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
        (MethodSchema) uixMethodSchema13,
        (MethodSchema) uixMethodSchema14,
        (MethodSchema) uixMethodSchema15
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
