// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.SingleSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using System;
using System.Globalization;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class SingleSchema
    {
        public static RangeValidator Validate0to1 = new RangeValidator(SingleSchema.RangeValidate0to1);
        public static RangeValidator ValidateNotNegative = new RangeValidator(SingleSchema.RangeValidateNotNegative);
        public static RangeValidator ValidateNotZero = new RangeValidator(SingleSchema.RangeValidateNotZero);
        public static UIXTypeSchema Type;

        private static object Construct() => (object)0.0f;

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            float num = (float)instanceObj;
            writer.WriteSingle(num);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)reader.ReadSingle();

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string s = (string)valueObj;
            instanceObj = (object)null;
            float result;
            if (!float.TryParse(s, NumberStyles.Float, (IFormatProvider)NumberFormatInfo.InvariantInfo, out result))
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)s, (object)"Single");
            instanceObj = (object)result;
            return Result.Success;
        }

        private static Result ConvertFromBoolean(object valueObj, out object instanceObj)
        {
            bool flag = (bool)valueObj;
            instanceObj = (object)null;
            float num = flag ? 1f : 0.0f;
            instanceObj = (object)num;
            return Result.Success;
        }

        private static Result ConvertFromByte(object valueObj, out object instanceObj)
        {
            byte num1 = (byte)valueObj;
            instanceObj = (object)null;
            float num2 = (float)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromInt32(object valueObj, out object instanceObj)
        {
            int num1 = (int)valueObj;
            instanceObj = (object)null;
            float num2 = (float)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromInt64(object valueObj, out object instanceObj)
        {
            long num1 = (long)valueObj;
            instanceObj = (object)null;
            float num2 = (float)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromDouble(object valueObj, out object instanceObj)
        {
            double num1 = (double)valueObj;
            instanceObj = (object)null;
            float num2 = (float)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static object CallToStringString(object instanceObj, object[] parameters) => (object)((float)instanceObj).ToString((string)parameters[0]);

        private static bool IsConversionSupported(TypeSchema fromType) => BooleanSchema.Type.IsAssignableFrom(fromType) || ByteSchema.Type.IsAssignableFrom(fromType) || (DoubleSchema.Type.IsAssignableFrom(fromType) || Int32Schema.Type.IsAssignableFrom(fromType)) || (Int64Schema.Type.IsAssignableFrom(fromType) || StringSchema.Type.IsAssignableFrom(fromType));

        private static Result TryConvertFrom(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result = Result.Fail("Unsupported");
            instance = (object)null;
            if (BooleanSchema.Type.IsAssignableFrom(fromType))
            {
                result = SingleSchema.ConvertFromBoolean(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (ByteSchema.Type.IsAssignableFrom(fromType))
            {
                result = SingleSchema.ConvertFromByte(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (DoubleSchema.Type.IsAssignableFrom(fromType))
            {
                result = SingleSchema.ConvertFromDouble(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (Int32Schema.Type.IsAssignableFrom(fromType))
            {
                result = SingleSchema.ConvertFromInt32(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (Int64Schema.Type.IsAssignableFrom(fromType))
            {
                result = SingleSchema.ConvertFromInt64(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                result = SingleSchema.ConvertFromString(from, out instance);
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
            float num1 = (float)leftObj;
            if (op == OperationType.MathNegate)
                return (object)(float)-(double)num1;
            float num2 = (float)rightObj;
            switch (op - 1)
            {
                case (OperationType)0:
                    return (object)(float)((double)num1 + (double)num2);
                case OperationType.MathAdd:
                    return (object)(float)((double)num1 - (double)num2);
                case OperationType.MathSubtract:
                    return (object)(float)((double)num1 * (double)num2);
                case OperationType.MathMultiply:
                    return (object)(float)((double)num1 / (double)num2);
                case OperationType.MathDivide:
                    return (object)(float)((double)num1 % (double)num2);
                case OperationType.LogicalOr:
                    return BooleanBoxes.Box((double)num1 == (double)num2);
                case OperationType.RelationalEquals:
                    return BooleanBoxes.Box((double)num1 != (double)num2);
                case OperationType.RelationalNotEquals:
                    return BooleanBoxes.Box((double)num1 < (double)num2);
                case OperationType.RelationalLessThan:
                    return BooleanBoxes.Box((double)num1 > (double)num2);
                case OperationType.RelationalGreaterThan:
                    return BooleanBoxes.Box((double)num1 <= (double)num2);
                case OperationType.RelationalLessThanEquals:
                    return BooleanBoxes.Box((double)num1 >= (double)num2);
                default:
                    return (object)null;
            }
        }

        private static object CallTryParseStringSingle(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            float parameter2 = (float)parameters[1];
            object instanceObj1;
            return SingleSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        private static Result RangeValidate0to1(object value)
        {
            float num = (float)value;
            return (double)num < 0.0 || (double)num > 1.0 ? Result.Fail("Expecting a value between {0} and {1}, but got {2}", (object)"0.0", (object)"1.0", (object)num.ToString()) : Result.Success;
        }

        private static Result RangeValidateNotNegative(object value)
        {
            float num = (float)value;
            return (double)num < 0.0 ? Result.Fail("Expecting a non-negative value, but got {0}", (object)num.ToString()) : Result.Success;
        }

        private static Result RangeValidateNotZero(object value)
        {
            float num = (float)value;
            return (double)num == 0.0 ? Result.Fail("Specified value '{0}' is not valid", (object)num.ToString()) : Result.Success;
        }

        public static void Pass1Initialize() => SingleSchema.Type = new UIXTypeSchema((short)194, "Single", "float", (short)153, typeof(float), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)194, "ToString", new short[1]
            {
        (short) 208
            }, (short)208, new InvokeHandler(SingleSchema.CallToStringString), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)194, "TryParse", new short[2]
            {
        (short) 208,
        (short) 194
            }, (short)194, new InvokeHandler(SingleSchema.CallTryParseStringSingle), true);
            SingleSchema.Type.Initialize(new DefaultConstructHandler(SingleSchema.Construct), (ConstructorSchema[])null, (PropertySchema[])null, new MethodSchema[2]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(SingleSchema.TryConvertFrom), new SupportsTypeConversionHandler(SingleSchema.IsConversionSupported), new EncodeBinaryHandler(SingleSchema.EncodeBinary), new DecodeBinaryHandler(SingleSchema.DecodeBinary), new PerformOperationHandler(SingleSchema.ExecuteOperation), new SupportsOperationHandler(SingleSchema.IsOperationSupported));
        }
    }
}
