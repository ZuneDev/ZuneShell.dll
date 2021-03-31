﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DoubleSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using System;
using System.Globalization;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DoubleSchema
    {
        public static UIXTypeSchema Type;

        private static object Construct() => (object)0.0;

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            double num = (double)instanceObj;
            writer.WriteDouble(num);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)reader.ReadDouble();

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string s = (string)valueObj;
            instanceObj = (object)null;
            double result;
            if (!double.TryParse(s, NumberStyles.Float, (IFormatProvider)NumberFormatInfo.InvariantInfo, out result))
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)s, (object)"Double");
            instanceObj = (object)result;
            return Result.Success;
        }

        private static Result ConvertFromBoolean(object valueObj, out object instanceObj)
        {
            bool flag = (bool)valueObj;
            instanceObj = (object)null;
            double num = flag ? 1.0 : 0.0;
            instanceObj = (object)num;
            return Result.Success;
        }

        private static Result ConvertFromByte(object valueObj, out object instanceObj)
        {
            byte num1 = (byte)valueObj;
            instanceObj = (object)null;
            double num2 = (double)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromInt32(object valueObj, out object instanceObj)
        {
            int num1 = (int)valueObj;
            instanceObj = (object)null;
            double num2 = (double)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromInt64(object valueObj, out object instanceObj)
        {
            long num1 = (long)valueObj;
            instanceObj = (object)null;
            double num2 = (double)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromSingle(object valueObj, out object instanceObj)
        {
            float num1 = (float)valueObj;
            instanceObj = (object)null;
            double num2 = (double)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static object CallToStringString(object instanceObj, object[] parameters) => (object)((double)instanceObj).ToString((string)parameters[0]);

        private static object CallIsNaNDouble(object instanceObj, object[] parameters) => BooleanBoxes.Box(double.IsNaN((double)parameters[0]));

        private static object CallIsNegativeInfinityDouble(object instanceObj, object[] parameters) => BooleanBoxes.Box(double.IsNegativeInfinity((double)parameters[0]));

        private static object CallIsPositiveInfinityDouble(object instanceObj, object[] parameters) => BooleanBoxes.Box(double.IsPositiveInfinity((double)parameters[0]));

        private static bool IsConversionSupported(TypeSchema fromType) => BooleanSchema.Type.IsAssignableFrom(fromType) || ByteSchema.Type.IsAssignableFrom(fromType) || (Int32Schema.Type.IsAssignableFrom(fromType) || Int64Schema.Type.IsAssignableFrom(fromType)) || (SingleSchema.Type.IsAssignableFrom(fromType) || StringSchema.Type.IsAssignableFrom(fromType));

        private static Result TryConvertFrom(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result = Result.Fail("Unsupported");
            instance = (object)null;
            if (BooleanSchema.Type.IsAssignableFrom(fromType))
            {
                result = DoubleSchema.ConvertFromBoolean(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (ByteSchema.Type.IsAssignableFrom(fromType))
            {
                result = DoubleSchema.ConvertFromByte(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (Int32Schema.Type.IsAssignableFrom(fromType))
            {
                result = DoubleSchema.ConvertFromInt32(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (Int64Schema.Type.IsAssignableFrom(fromType))
            {
                result = DoubleSchema.ConvertFromInt64(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (SingleSchema.Type.IsAssignableFrom(fromType))
            {
                result = DoubleSchema.ConvertFromSingle(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                result = DoubleSchema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            return result;
        }

        private static bool IsOperationSupported(OperationType op)
        {
            switch (op)
            {
                case OperationType.MathAdd:
                case OperationType.MathSubtract:
                case OperationType.MathMultiply:
                case OperationType.MathDivide:
                case OperationType.MathModulus:
                case OperationType.RelationalEquals:
                case OperationType.RelationalNotEquals:
                case OperationType.RelationalLessThan:
                case OperationType.RelationalGreaterThan:
                case OperationType.RelationalLessThanEquals:
                case OperationType.RelationalGreaterThanEquals:
                case OperationType.MathNegate:
                    return true;
                default:
                    return false;
            }
        }

        private static object ExecuteOperation(object leftObj, object rightObj, OperationType op)
        {
            double num1 = (double)leftObj;
            if (op == OperationType.MathNegate)
                return (object)-num1;
            double num2 = (double)rightObj;
            switch (op - 1)
            {
                case (OperationType)0:
                    return (object)(num1 + num2);
                case OperationType.MathAdd:
                    return (object)(num1 - num2);
                case OperationType.MathSubtract:
                    return (object)(num1 * num2);
                case OperationType.MathMultiply:
                    return (object)(num1 / num2);
                case OperationType.MathDivide:
                    return (object)(num1 % num2);
                case OperationType.LogicalOr:
                    return BooleanBoxes.Box(num1 == num2);
                case OperationType.RelationalEquals:
                    return BooleanBoxes.Box(num1 != num2);
                case OperationType.RelationalNotEquals:
                    return BooleanBoxes.Box(num1 < num2);
                case OperationType.RelationalLessThan:
                    return BooleanBoxes.Box(num1 > num2);
                case OperationType.RelationalGreaterThan:
                    return BooleanBoxes.Box(num1 <= num2);
                case OperationType.RelationalLessThanEquals:
                    return BooleanBoxes.Box(num1 >= num2);
                default:
                    return (object)null;
            }
        }

        private static object CallTryParseStringDouble(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            double parameter2 = (double)parameters[1];
            object instanceObj1;
            return DoubleSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        public static void Pass1Initialize() => DoubleSchema.Type = new UIXTypeSchema((short)61, "Double", "double", (short)153, typeof(double), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)61, "ToString", new short[1]
            {
        (short) 208
            }, (short)208, new InvokeHandler(DoubleSchema.CallToStringString), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)61, "IsNaN", new short[1]
            {
        (short) 61
            }, (short)15, new InvokeHandler(DoubleSchema.CallIsNaNDouble), true);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)61, "IsNegativeInfinity", new short[1]
            {
        (short) 61
            }, (short)15, new InvokeHandler(DoubleSchema.CallIsNegativeInfinityDouble), true);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)61, "IsPositiveInfinity", new short[1]
            {
        (short) 61
            }, (short)15, new InvokeHandler(DoubleSchema.CallIsPositiveInfinityDouble), true);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)61, "TryParse", new short[2]
            {
        (short) 208,
        (short) 61
            }, (short)61, new InvokeHandler(DoubleSchema.CallTryParseStringDouble), true);
            DoubleSchema.Type.Initialize(new DefaultConstructHandler(DoubleSchema.Construct), (ConstructorSchema[])null, (PropertySchema[])null, new MethodSchema[5]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4,
        (MethodSchema) uixMethodSchema5
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(DoubleSchema.TryConvertFrom), new SupportsTypeConversionHandler(DoubleSchema.IsConversionSupported), new EncodeBinaryHandler(DoubleSchema.EncodeBinary), new DecodeBinaryHandler(DoubleSchema.DecodeBinary), new PerformOperationHandler(DoubleSchema.ExecuteOperation), new SupportsOperationHandler(DoubleSchema.IsOperationSupported));
        }
    }
}