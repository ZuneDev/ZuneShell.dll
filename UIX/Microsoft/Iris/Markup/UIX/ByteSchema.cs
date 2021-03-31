// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ByteSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using System;
using System.Globalization;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ByteSchema
    {
        public static UIXTypeSchema Type;

        private static object Construct() => (object)(byte)0;

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            byte num = (byte)instanceObj;
            writer.WriteByte(num);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)reader.ReadByte();

        private static object CallToStringString(object instanceObj, object[] parameters) => (object)((byte)instanceObj).ToString((string)parameters[0]);

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string s = (string)valueObj;
            instanceObj = (object)null;
            byte result;
            if (!byte.TryParse(s, NumberStyles.Integer, (IFormatProvider)NumberFormatInfo.InvariantInfo, out result))
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)s, (object)"Byte");
            instanceObj = (object)result;
            return Result.Success;
        }

        private static Result ConvertFromBoolean(object valueObj, out object instanceObj)
        {
            bool flag = (bool)valueObj;
            instanceObj = (object)null;
            byte num = flag ? (byte)1 : (byte)0;
            instanceObj = (object)num;
            return Result.Success;
        }

        private static Result ConvertFromInt32(object valueObj, out object instanceObj)
        {
            int num1 = (int)valueObj;
            instanceObj = (object)null;
            byte num2 = (byte)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromInt64(object valueObj, out object instanceObj)
        {
            long num1 = (long)valueObj;
            instanceObj = (object)null;
            byte num2 = (byte)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromSingle(object valueObj, out object instanceObj)
        {
            float num1 = (float)valueObj;
            instanceObj = (object)null;
            byte num2 = (byte)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static Result ConvertFromDouble(object valueObj, out object instanceObj)
        {
            double num1 = (double)valueObj;
            instanceObj = (object)null;
            byte num2 = (byte)num1;
            instanceObj = (object)num2;
            return Result.Success;
        }

        private static bool IsConversionSupported(TypeSchema fromType) => BooleanSchema.Type.IsAssignableFrom(fromType) || DoubleSchema.Type.IsAssignableFrom(fromType) || (Int32Schema.Type.IsAssignableFrom(fromType) || Int64Schema.Type.IsAssignableFrom(fromType)) || (SingleSchema.Type.IsAssignableFrom(fromType) || StringSchema.Type.IsAssignableFrom(fromType));

        private static Result TryConvertFrom(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result = Result.Fail("Unsupported");
            instance = (object)null;
            if (BooleanSchema.Type.IsAssignableFrom(fromType))
            {
                result = ByteSchema.ConvertFromBoolean(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (DoubleSchema.Type.IsAssignableFrom(fromType))
            {
                result = ByteSchema.ConvertFromDouble(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (Int32Schema.Type.IsAssignableFrom(fromType))
            {
                result = ByteSchema.ConvertFromInt32(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (Int64Schema.Type.IsAssignableFrom(fromType))
            {
                result = ByteSchema.ConvertFromInt64(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (SingleSchema.Type.IsAssignableFrom(fromType))
            {
                result = ByteSchema.ConvertFromSingle(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                result = ByteSchema.ConvertFromString(from, out instance);
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
                    return true;
                default:
                    return false;
            }
        }

        private static object ExecuteOperation(object leftObj, object rightObj, OperationType op)
        {
            byte num1 = (byte)leftObj;
            byte num2 = (byte)rightObj;
            switch (op)
            {
                case OperationType.MathAdd:
                    return (object)(byte)((uint)num1 + (uint)num2);
                case OperationType.MathSubtract:
                    return (object)(byte)((uint)num1 - (uint)num2);
                case OperationType.MathMultiply:
                    return (object)(byte)((uint)num1 * (uint)num2);
                case OperationType.MathDivide:
                    return (object)(byte)((uint)num1 / (uint)num2);
                case OperationType.MathModulus:
                    return (object)(byte)((uint)num1 % (uint)num2);
                case OperationType.RelationalEquals:
                    return BooleanBoxes.Box((int)num1 == (int)num2);
                case OperationType.RelationalNotEquals:
                    return BooleanBoxes.Box((int)num1 != (int)num2);
                case OperationType.RelationalLessThan:
                    return BooleanBoxes.Box((int)num1 < (int)num2);
                case OperationType.RelationalGreaterThan:
                    return BooleanBoxes.Box((int)num1 > (int)num2);
                case OperationType.RelationalLessThanEquals:
                    return BooleanBoxes.Box((int)num1 <= (int)num2);
                case OperationType.RelationalGreaterThanEquals:
                    return BooleanBoxes.Box((int)num1 >= (int)num2);
                default:
                    return (object)null;
            }
        }

        private static object CallTryParseStringByte(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            byte parameter2 = (byte)parameters[1];
            object instanceObj1;
            return ByteSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        public static void Pass1Initialize() => ByteSchema.Type = new UIXTypeSchema((short)19, "Byte", "byte", (short)153, typeof(byte), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)19, "ToString", new short[1]
            {
        (short) 208
            }, (short)208, new InvokeHandler(ByteSchema.CallToStringString), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)19, "TryParse", new short[2]
            {
        (short) 208,
        (short) 19
            }, (short)19, new InvokeHandler(ByteSchema.CallTryParseStringByte), true);
            ByteSchema.Type.Initialize(new DefaultConstructHandler(ByteSchema.Construct), (ConstructorSchema[])null, (PropertySchema[])null, new MethodSchema[2]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(ByteSchema.TryConvertFrom), new SupportsTypeConversionHandler(ByteSchema.IsConversionSupported), new EncodeBinaryHandler(ByteSchema.EncodeBinary), new DecodeBinaryHandler(ByteSchema.DecodeBinary), new PerformOperationHandler(ByteSchema.ExecuteOperation), new SupportsOperationHandler(ByteSchema.IsOperationSupported));
        }
    }
}
