// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.Int64Schema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using System;
using System.Globalization;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class Int64Schema
    {
        public static UIXTypeSchema Type;

        private static object GetMinValue(object instanceObj) => Int64Boxes.MinValueBox;

        private static object GetMaxValue(object instanceObj) => Int64Boxes.MaxValueBox;

        private static object Construct() => Int64Boxes.ZeroBox;

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            long num = (long)instanceObj;
            writer.WriteInt64(num);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)reader.ReadInt64();

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string s = (string)valueObj;
            instanceObj = (object)null;
            long result;
            if (!long.TryParse(s, NumberStyles.Integer, (IFormatProvider)NumberFormatInfo.InvariantInfo, out result))
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)s, (object)"Int64");
            instanceObj = (object)result;
            return Result.Success;
        }

        private static Result ConvertFromBoolean(object valueObj, out object instanceObj)
        {
            bool flag = (bool)valueObj;
            instanceObj = (object)null;
            long num = flag ? 1L : 0L;
            instanceObj = (object)num;
            return Result.Success;
        }

        private static Result ConvertFromByte(object valueObj, out object instanceObj)
        {
            byte num1 = (byte)valueObj;
            instanceObj = (object)null;
            long num2 = (long)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromSingle(object valueObj, out object instanceObj)
        {
            float num1 = (float)valueObj;
            instanceObj = (object)null;
            long num2 = (long)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromInt32(object valueObj, out object instanceObj)
        {
            int num1 = (int)valueObj;
            instanceObj = (object)null;
            long num2 = (long)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromDouble(object valueObj, out object instanceObj)
        {
            double num1 = (double)valueObj;
            instanceObj = (object)null;
            long num2 = (long)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static object CallToStringString(object instanceObj, object[] parameters) => (object)((long)instanceObj).ToString((string)parameters[0]);

        private static bool IsConversionSupported(TypeSchema fromType) => BooleanSchema.Type.IsAssignableFrom(fromType) || ByteSchema.Type.IsAssignableFrom(fromType) || (DoubleSchema.Type.IsAssignableFrom(fromType) || Int32Schema.Type.IsAssignableFrom(fromType)) || (SingleSchema.Type.IsAssignableFrom(fromType) || StringSchema.Type.IsAssignableFrom(fromType));

        private static Result TryConvertFrom(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result = Result.Fail("Unsupported");
            instance = (object)null;
            if (BooleanSchema.Type.IsAssignableFrom(fromType))
            {
                result = Int64Schema.ConvertFromBoolean(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (ByteSchema.Type.IsAssignableFrom(fromType))
            {
                result = Int64Schema.ConvertFromByte(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (DoubleSchema.Type.IsAssignableFrom(fromType))
            {
                result = Int64Schema.ConvertFromDouble(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (Int32Schema.Type.IsAssignableFrom(fromType))
            {
                result = Int64Schema.ConvertFromInt32(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (SingleSchema.Type.IsAssignableFrom(fromType))
            {
                result = Int64Schema.ConvertFromSingle(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                result = Int64Schema.ConvertFromString(from, out instance);
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
            long num1 = (long)leftObj;
            if (op == OperationType.MathNegate)
                return (object)-num1;
            long num2 = (long)rightObj;
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

        private static object CallTryParseStringInt64(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            long parameter2 = (long)parameters[1];
            object instanceObj1;
            return Int64Schema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        public static void Pass1Initialize() => Int64Schema.Type = new UIXTypeSchema((short)116, "Int64", "long", (short)153, typeof(long), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)116, "MinValue", (short)116, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(Int64Schema.GetMinValue), (SetValueHandler)null, true);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)116, "MaxValue", (short)116, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(Int64Schema.GetMaxValue), (SetValueHandler)null, true);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)116, "ToString", new short[1]
            {
        (short) 208
            }, (short)208, new InvokeHandler(Int64Schema.CallToStringString), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)116, "TryParse", new short[2]
            {
        (short) 208,
        (short) 116
            }, (short)116, new InvokeHandler(Int64Schema.CallTryParseStringInt64), true);
            Int64Schema.Type.Initialize(new DefaultConstructHandler(Int64Schema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, new MethodSchema[2]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(Int64Schema.TryConvertFrom), new SupportsTypeConversionHandler(Int64Schema.IsConversionSupported), new EncodeBinaryHandler(Int64Schema.EncodeBinary), new DecodeBinaryHandler(Int64Schema.DecodeBinary), new PerformOperationHandler(Int64Schema.ExecuteOperation), new SupportsOperationHandler(Int64Schema.IsOperationSupported));
        }
    }
}
