// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.MathSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class MathSchema
    {
        public static UIXTypeSchema Type;

        private static object CallMinInt32Int32(object instanceObj, object[] parameters) => (int)parameters[0] > (int)parameters[1] ? parameters[1] : parameters[0];

        private static object CallMinSingleSingle(object instanceObj, object[] parameters) => (double)(float)parameters[0] > (double)(float)parameters[1] ? parameters[1] : parameters[0];

        private static object CallMinDoubleDouble(object instanceObj, object[] parameters) => (double)parameters[0] > (double)parameters[1] ? parameters[1] : parameters[0];

        private static object CallMaxInt32Int32(object instanceObj, object[] parameters) => (int)parameters[0] < (int)parameters[1] ? parameters[1] : parameters[0];

        private static object CallMaxSingleSingle(object instanceObj, object[] parameters) => (double)(float)parameters[0] < (double)(float)parameters[1] ? parameters[1] : parameters[0];

        private static object CallMaxDoubleDouble(object instanceObj, object[] parameters) => (double)parameters[0] < (double)parameters[1] ? parameters[1] : parameters[0];

        private static object CallAbsInt32(object instanceObj, object[] parameters)
        {
            int parameter = (int)parameters[0];
            int num = Math.Abs(parameter);
            return num != parameter ? (object)num : parameters[0];
        }

        private static object CallAbsSingle(object instanceObj, object[] parameters)
        {
            float parameter = (float)parameters[0];
            float num = Math.Abs(parameter);
            return (double)num != (double)parameter ? (object)num : parameters[0];
        }

        private static object CallAbsDouble(object instanceObj, object[] parameters)
        {
            double parameter = (double)parameters[0];
            double num = Math.Abs(parameter);
            return num != parameter ? (object)num : parameters[0];
        }

        private static object CallRoundSingle(object instanceObj, object[] parameters)
        {
            float parameter = (float)parameters[0];
            float num = (float)Math.Round((double)parameter);
            return (double)num != (double)parameter ? (object)num : parameters[0];
        }

        private static object CallRoundDouble(object instanceObj, object[] parameters)
        {
            double parameter = (double)parameters[0];
            double num = Math.Round(parameter);
            return num != parameter ? (object)num : parameters[0];
        }

        private static object CallFloorSingle(object instanceObj, object[] parameters)
        {
            float parameter = (float)parameters[0];
            float num = (float)Math.Floor((double)parameter);
            return (double)num != (double)parameter ? (object)num : parameters[0];
        }

        private static object CallFloorDouble(object instanceObj, object[] parameters)
        {
            double parameter = (double)parameters[0];
            double num = Math.Floor(parameter);
            return num != parameter ? (object)num : parameters[0];
        }

        private static object CallCeilingSingle(object instanceObj, object[] parameters)
        {
            float parameter = (float)parameters[0];
            float num = (float)Math.Ceiling((double)parameter);
            return (double)num != (double)parameter ? (object)num : parameters[0];
        }

        private static object CallCeilingDouble(object instanceObj, object[] parameters)
        {
            double parameter = (double)parameters[0];
            double num = Math.Ceiling(parameter);
            return num != parameter ? (object)num : parameters[0];
        }

        private static object CallAcosDouble(object instanceObj, object[] parameters) => (object)Math.Acos((double)parameters[0]);

        private static object CallAsinDouble(object instanceObj, object[] parameters) => (object)Math.Asin((double)parameters[0]);

        private static object CallAtanDouble(object instanceObj, object[] parameters) => (object)Math.Atan((double)parameters[0]);

        private static object CallAtan2DoubleDouble(object instanceObj, object[] parameters) => (object)Math.Atan2((double)parameters[0], (double)parameters[1]);

        private static object CallCosDouble(object instanceObj, object[] parameters) => (object)Math.Cos((double)parameters[0]);

        private static object CallCoshDouble(object instanceObj, object[] parameters) => (object)Math.Cosh((double)parameters[0]);

        private static object CallSinDouble(object instanceObj, object[] parameters) => (object)Math.Sin((double)parameters[0]);

        private static object CallSinhDouble(object instanceObj, object[] parameters) => (object)Math.Sinh((double)parameters[0]);

        private static object CallTanDouble(object instanceObj, object[] parameters) => (object)Math.Tan((double)parameters[0]);

        private static object CallTanhDouble(object instanceObj, object[] parameters) => (object)Math.Tanh((double)parameters[0]);

        private static object CallSqrtDouble(object instanceObj, object[] parameters) => (object)Math.Sqrt((double)parameters[0]);

        private static object CallPowDoubleDouble(object instanceObj, object[] parameters) => (object)Math.Pow((double)parameters[0], (double)parameters[1]);

        private static object CallLogDouble(object instanceObj, object[] parameters) => (object)Math.Log((double)parameters[0]);

        private static object CallLogDoubleDouble(object instanceObj, object[] parameters) => (object)Math.Log((double)parameters[0], (double)parameters[1]);

        private static object CallLog10Double(object instanceObj, object[] parameters) => (object)Math.Log10((double)parameters[0]);

        public static void Pass1Initialize() => MathSchema.Type = new UIXTypeSchema((short)145, "Math", (string)null, (short)153, typeof(object), UIXTypeFlags.Static);

        public static void Pass2Initialize()
        {
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)145, "Min", new short[2]
            {
        (short) 115,
        (short) 115
            }, (short)115, new InvokeHandler(MathSchema.CallMinInt32Int32), true);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)145, "Min", new short[2]
            {
        (short) 194,
        (short) 194
            }, (short)194, new InvokeHandler(MathSchema.CallMinSingleSingle), true);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)145, "Min", new short[2]
            {
        (short) 61,
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallMinDoubleDouble), true);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)145, "Max", new short[2]
            {
        (short) 115,
        (short) 115
            }, (short)115, new InvokeHandler(MathSchema.CallMaxInt32Int32), true);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)145, "Max", new short[2]
            {
        (short) 194,
        (short) 194
            }, (short)194, new InvokeHandler(MathSchema.CallMaxSingleSingle), true);
            UIXMethodSchema uixMethodSchema6 = new UIXMethodSchema((short)145, "Max", new short[2]
            {
        (short) 61,
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallMaxDoubleDouble), true);
            UIXMethodSchema uixMethodSchema7 = new UIXMethodSchema((short)145, "Abs", new short[1]
            {
        (short) 115
            }, (short)115, new InvokeHandler(MathSchema.CallAbsInt32), true);
            UIXMethodSchema uixMethodSchema8 = new UIXMethodSchema((short)145, "Abs", new short[1]
            {
        (short) 194
            }, (short)194, new InvokeHandler(MathSchema.CallAbsSingle), true);
            UIXMethodSchema uixMethodSchema9 = new UIXMethodSchema((short)145, "Abs", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallAbsDouble), true);
            UIXMethodSchema uixMethodSchema10 = new UIXMethodSchema((short)145, "Round", new short[1]
            {
        (short) 194
            }, (short)194, new InvokeHandler(MathSchema.CallRoundSingle), true);
            UIXMethodSchema uixMethodSchema11 = new UIXMethodSchema((short)145, "Round", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallRoundDouble), true);
            UIXMethodSchema uixMethodSchema12 = new UIXMethodSchema((short)145, "Floor", new short[1]
            {
        (short) 194
            }, (short)194, new InvokeHandler(MathSchema.CallFloorSingle), true);
            UIXMethodSchema uixMethodSchema13 = new UIXMethodSchema((short)145, "Floor", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallFloorDouble), true);
            UIXMethodSchema uixMethodSchema14 = new UIXMethodSchema((short)145, "Ceiling", new short[1]
            {
        (short) 194
            }, (short)194, new InvokeHandler(MathSchema.CallCeilingSingle), true);
            UIXMethodSchema uixMethodSchema15 = new UIXMethodSchema((short)145, "Ceiling", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallCeilingDouble), true);
            UIXMethodSchema uixMethodSchema16 = new UIXMethodSchema((short)145, "Acos", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallAcosDouble), true);
            UIXMethodSchema uixMethodSchema17 = new UIXMethodSchema((short)145, "Asin", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallAsinDouble), true);
            UIXMethodSchema uixMethodSchema18 = new UIXMethodSchema((short)145, "Atan", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallAtanDouble), true);
            UIXMethodSchema uixMethodSchema19 = new UIXMethodSchema((short)145, "Atan2", new short[2]
            {
        (short) 61,
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallAtan2DoubleDouble), true);
            UIXMethodSchema uixMethodSchema20 = new UIXMethodSchema((short)145, "Cos", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallCosDouble), true);
            UIXMethodSchema uixMethodSchema21 = new UIXMethodSchema((short)145, "Cosh", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallCoshDouble), true);
            UIXMethodSchema uixMethodSchema22 = new UIXMethodSchema((short)145, "Sin", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallSinDouble), true);
            UIXMethodSchema uixMethodSchema23 = new UIXMethodSchema((short)145, "Sinh", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallSinhDouble), true);
            UIXMethodSchema uixMethodSchema24 = new UIXMethodSchema((short)145, "Tan", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallTanDouble), true);
            UIXMethodSchema uixMethodSchema25 = new UIXMethodSchema((short)145, "Tanh", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallTanhDouble), true);
            UIXMethodSchema uixMethodSchema26 = new UIXMethodSchema((short)145, "Sqrt", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallSqrtDouble), true);
            UIXMethodSchema uixMethodSchema27 = new UIXMethodSchema((short)145, "Pow", new short[2]
            {
        (short) 61,
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallPowDoubleDouble), true);
            UIXMethodSchema uixMethodSchema28 = new UIXMethodSchema((short)145, "Log", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallLogDouble), true);
            UIXMethodSchema uixMethodSchema29 = new UIXMethodSchema((short)145, "Log", new short[2]
            {
        (short) 61,
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallLogDoubleDouble), true);
            UIXMethodSchema uixMethodSchema30 = new UIXMethodSchema((short)145, "Log10", new short[1]
            {
        (short) 61
            }, (short)61, new InvokeHandler(MathSchema.CallLog10Double), true);
            MathSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, (PropertySchema[])null, new MethodSchema[30]
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
        (MethodSchema) uixMethodSchema15,
        (MethodSchema) uixMethodSchema16,
        (MethodSchema) uixMethodSchema17,
        (MethodSchema) uixMethodSchema18,
        (MethodSchema) uixMethodSchema19,
        (MethodSchema) uixMethodSchema20,
        (MethodSchema) uixMethodSchema21,
        (MethodSchema) uixMethodSchema22,
        (MethodSchema) uixMethodSchema23,
        (MethodSchema) uixMethodSchema24,
        (MethodSchema) uixMethodSchema25,
        (MethodSchema) uixMethodSchema26,
        (MethodSchema) uixMethodSchema27,
        (MethodSchema) uixMethodSchema28,
        (MethodSchema) uixMethodSchema29,
        (MethodSchema) uixMethodSchema30
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
