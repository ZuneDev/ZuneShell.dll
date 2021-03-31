// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.RandomSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class RandomSchema
    {
        public static UIXTypeSchema Type;

        private static object Construct() => (object)new Random();

        private static object ConstructInt32(object[] parameters) => (object)new Random((int)parameters[0]);

        private static object CallNext(object instanceObj, object[] parameters) => (object)((Random)instanceObj).Next();

        private static object CallNextInt32(object instanceObj, object[] parameters)
        {
            Random random = (Random)instanceObj;
            int parameter = (int)parameters[0];
            if (parameter >= 0)
                return (object)random.Next(parameter);
            ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)parameter, (object)"maxValue");
            return (object)null;
        }

        private static object CallNextInt32Int32(object instanceObj, object[] parameters)
        {
            Random random = (Random)instanceObj;
            int parameter1 = (int)parameters[0];
            int parameter2 = (int)parameters[1];
            if (parameter1 <= parameter2)
                return (object)random.Next(parameter1, parameter2);
            ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)parameter1, (object)"minValue");
            return (object)null;
        }

        private static object CallNextDouble(object instanceObj, object[] parameters) => (object)((Random)instanceObj).NextDouble();

        private static object CallNextDoubleDoubleDouble(object instanceObj, object[] parameters)
        {
            Random random = (Random)instanceObj;
            double parameter1 = (double)parameters[0];
            double parameter2 = (double)parameters[1];
            return (object)(random.NextDouble() * (parameter2 - parameter1) + parameter1);
        }

        private static object CallNextSingle(object instanceObj, object[] parameters) => (object)(float)((Random)instanceObj).NextDouble();

        private static object CallNextSingleSingleSingle(object instanceObj, object[] parameters)
        {
            Random random = (Random)instanceObj;
            float parameter1 = (float)parameters[0];
            float parameter2 = (float)parameters[1];
            return (object)((float)(random.NextDouble() * ((double)parameter2 - (double)parameter1)) + parameter1);
        }

        public static void Pass1Initialize() => RandomSchema.Type = new UIXTypeSchema((short)167, "Random", (string)null, (short)153, typeof(Random), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXConstructorSchema constructorSchema = new UIXConstructorSchema((short)167, new short[1]
            {
        (short) 115
            }, new ConstructHandler(RandomSchema.ConstructInt32));
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)167, "Next", (short[])null, (short)115, new InvokeHandler(RandomSchema.CallNext), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)167, "Next", new short[1]
            {
        (short) 115
            }, (short)115, new InvokeHandler(RandomSchema.CallNextInt32), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)167, "Next", new short[2]
            {
        (short) 115,
        (short) 115
            }, (short)115, new InvokeHandler(RandomSchema.CallNextInt32Int32), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)167, "NextDouble", (short[])null, (short)61, new InvokeHandler(RandomSchema.CallNextDouble), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)167, "NextDouble", new short[2]
            {
        (short) 61,
        (short) 61
            }, (short)61, new InvokeHandler(RandomSchema.CallNextDoubleDoubleDouble), false);
            UIXMethodSchema uixMethodSchema6 = new UIXMethodSchema((short)167, "NextSingle", (short[])null, (short)194, new InvokeHandler(RandomSchema.CallNextSingle), false);
            UIXMethodSchema uixMethodSchema7 = new UIXMethodSchema((short)167, "NextSingle", new short[2]
            {
        (short) 194,
        (short) 194
            }, (short)194, new InvokeHandler(RandomSchema.CallNextSingleSingleSingle), false);
            RandomSchema.Type.Initialize(new DefaultConstructHandler(RandomSchema.Construct), new ConstructorSchema[1]
            {
        (ConstructorSchema) constructorSchema
            }, (PropertySchema[])null, new MethodSchema[7]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4,
        (MethodSchema) uixMethodSchema5,
        (MethodSchema) uixMethodSchema6,
        (MethodSchema) uixMethodSchema7
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
