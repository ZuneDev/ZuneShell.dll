// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.Int32Schema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.CodeModel.Cpp;
using Microsoft.Iris.Library;
using System;
using System.Globalization;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class Int32Schema
    {
        public static RangeValidator ValidateNotNegative = new RangeValidator(Int32Schema.RangeValidateNotNegative);
        public static UIXTypeSchema Type;

        private static object GetMinValue(object instanceObj) => Int32Boxes.MinValueBox;

        private static object GetMaxValue(object instanceObj) => Int32Boxes.MaxValueBox;

        private static object Construct() => Int32Boxes.ZeroBox;

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            int num = (int)instanceObj;
            writer.WriteInt32(num);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)reader.ReadInt32();

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string s = (string)valueObj;
            instanceObj = (object)null;
            int result;
            if (!int.TryParse(s, NumberStyles.Integer, (IFormatProvider)NumberFormatInfo.InvariantInfo, out result))
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)s, (object)"Int32");
            instanceObj = (object)result;
            return Result.Success;
        }

        private static Result ConvertFromBoolean(object valueObj, out object instanceObj)
        {
            bool flag = (bool)valueObj;
            instanceObj = (object)null;
            int num = flag ? 1 : 0;
            instanceObj = (object)num;
            return Result.Success;
        }

        private static Result ConvertFromByte(object valueObj, out object instanceObj)
        {
            byte num1 = (byte)valueObj;
            instanceObj = (object)null;
            int num2 = (int)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromSingle(object valueObj, out object instanceObj)
        {
            float num1 = (float)valueObj;
            instanceObj = (object)null;
            int num2 = (int)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromInt64(object valueObj, out object instanceObj)
        {
            long num1 = (long)valueObj;
            instanceObj = (object)null;
            int num2 = (int)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromDouble(object valueObj, out object instanceObj)
        {
            double num1 = (double)valueObj;
            instanceObj = (object)null;
            int num2 = (int)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static object CallToStringString(object instanceObj, object[] parameters) => (object)((int)instanceObj).ToString((string)parameters[0]);

        private static bool IsConversionSupported(TypeSchema fromType) => BooleanSchema.Type.IsAssignableFrom(fromType) || ByteSchema.Type.IsAssignableFrom(fromType) || (DoubleSchema.Type.IsAssignableFrom(fromType) || Int64Schema.Type.IsAssignableFrom(fromType)) || (SingleSchema.Type.IsAssignableFrom(fromType) || StringSchema.Type.IsAssignableFrom(fromType) || fromType.IsEnum);

        private static Result TryConvertFrom(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result = Result.Fail("Unsupported");
            instance = (object)null;
            if (BooleanSchema.Type.IsAssignableFrom(fromType))
            {
                result = Int32Schema.ConvertFromBoolean(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (ByteSchema.Type.IsAssignableFrom(fromType))
            {
                result = Int32Schema.ConvertFromByte(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (DoubleSchema.Type.IsAssignableFrom(fromType))
            {
                result = Int32Schema.ConvertFromDouble(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (Int64Schema.Type.IsAssignableFrom(fromType))
            {
                result = Int32Schema.ConvertFromInt64(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (SingleSchema.Type.IsAssignableFrom(fromType))
            {
                result = Int32Schema.ConvertFromSingle(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                result = Int32Schema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (!fromType.IsEnum)
                return result;
            instance = !(from is DllEnumProxy dllEnumProxy) ? (object)(int)from : (object)dllEnumProxy.Value;
            return Result.Success;
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
            int num1 = (int)leftObj;
            if (op == OperationType.MathNegate)
                return (object)-num1;
            int num2 = (int)rightObj;
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

        private static object CallTryParseStringInt32(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            int parameter2 = (int)parameters[1];
            object instanceObj1;
            return Int32Schema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        private static Result RangeValidateNotNegative(object value)
        {
            int num = (int)value;
            return num < 0 ? Result.Fail("Expecting a non-negative value, but got {0}", (object)num.ToString()) : Result.Success;
        }

        public static void Pass1Initialize() => Int32Schema.Type = new UIXTypeSchema((short)115, "Int32", "int", (short)153, typeof(int), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)115, "MinValue", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(Int32Schema.GetMinValue), (SetValueHandler)null, true);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)115, "MaxValue", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(Int32Schema.GetMaxValue), (SetValueHandler)null, true);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)115, "ToString", new short[1]
            {
        (short) 208
            }, (short)208, new InvokeHandler(Int32Schema.CallToStringString), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)115, "TryParse", new short[2]
            {
        (short) 208,
        (short) 115
            }, (short)115, new InvokeHandler(Int32Schema.CallTryParseStringInt32), true);
            Int32Schema.Type.Initialize(new DefaultConstructHandler(Int32Schema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, new MethodSchema[2]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(Int32Schema.TryConvertFrom), new SupportsTypeConversionHandler(Int32Schema.IsConversionSupported), new EncodeBinaryHandler(Int32Schema.EncodeBinary), new DecodeBinaryHandler(Int32Schema.DecodeBinary), new PerformOperationHandler(Int32Schema.ExecuteOperation), new SupportsOperationHandler(Int32Schema.IsOperationSupported));
        }
    }
}
