// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.Vector2Schema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using System;
using System.Globalization;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class Vector2Schema
    {
        public static RangeValidator ValidateNotNegative = new RangeValidator(Vector2Schema.RangeValidateNotNegative);
        public static UIXTypeSchema Type;

        private static object GetX(object instanceObj) => (object)((Vector2)instanceObj).X;

        private static void SetX(ref object instanceObj, object valueObj)
        {
            Vector2 vector2 = (Vector2)instanceObj;
            float num = (float)valueObj;
            vector2.X = num;
            instanceObj = (object)vector2;
        }

        private static object GetY(object instanceObj) => (object)((Vector2)instanceObj).Y;

        private static void SetY(ref object instanceObj, object valueObj)
        {
            Vector2 vector2 = (Vector2)instanceObj;
            float num = (float)valueObj;
            vector2.Y = num;
            instanceObj = (object)vector2;
        }

        private static object Construct() => (object)Vector2.Zero;

        private static object ConstructXY(object[] parameters)
        {
            object instanceObj = Vector2Schema.Construct();
            Vector2Schema.SetX(ref instanceObj, parameters[0]);
            Vector2Schema.SetY(ref instanceObj, parameters[1]);
            return instanceObj;
        }

        private static Result ConvertFromStringXY(string[] splitString, out object instance)
        {
            instance = Vector2Schema.Construct();
            object valueObj1;
            Result result1 = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)SingleSchema.Type, (RangeValidator)null, out valueObj1);
            if (result1.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Vector2", (object)result1.Error);
            Vector2Schema.SetX(ref instance, valueObj1);
            object valueObj2;
            Result result2 = UIXLoadResult.ValidateStringAsValue(splitString[1], (TypeSchema)SingleSchema.Type, (RangeValidator)null, out valueObj2);
            if (result2.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Vector2", (object)result2.Error);
            Vector2Schema.SetY(ref instance, valueObj2);
            return result2;
        }

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            Vector2 vector2 = (Vector2)instanceObj;
            writer.WriteSingle(vector2.X);
            writer.WriteSingle(vector2.Y);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)new Vector2(reader.ReadSingle(), reader.ReadSingle());

        private static Result ConvertFromSize(object valueObj, out object instanceObj)
        {
            Size size = (Size)valueObj;
            instanceObj = (object)null;
            Vector2 vector2 = new Vector2((float)size.Width, (float)size.Height);
            instanceObj = (object)vector2;
            return Result.Success;
        }

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string s = (string)valueObj;
            instanceObj = (object)null;
            float result;
            if (!float.TryParse(s, NumberStyles.Float, (IFormatProvider)NumberFormatInfo.InvariantInfo, out result))
                return Result.Fail("");
            instanceObj = (object)new Vector2()
            {
                X = result,
                Y = result
            };
            return Result.Success;
        }

        private static Result ConvertFromSingle(object valueObj, out object instanceObj)
        {
            float num = (float)valueObj;
            instanceObj = (object)null;
            instanceObj = (object)new Vector2()
            {
                X = num,
                Y = num
            };
            return Result.Success;
        }

        private static bool IsConversionSupported(TypeSchema fromType) => SingleSchema.Type.IsAssignableFrom(fromType) || SizeSchema.Type.IsAssignableFrom(fromType) || StringSchema.Type.IsAssignableFrom(fromType);

        private static Result TryConvertFrom(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result = Result.Fail("Unsupported");
            instance = (object)null;
            if (SingleSchema.Type.IsAssignableFrom(fromType))
            {
                result = Vector2Schema.ConvertFromSingle(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (SizeSchema.Type.IsAssignableFrom(fromType))
            {
                result = Vector2Schema.ConvertFromSize(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                result = Vector2Schema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                string[] splitString = StringUtility.SplitAndTrim(',', (string)from);
                if (splitString.Length == 2)
                {
                    result = Vector2Schema.ConvertFromStringXY(splitString, out instance);
                    if (!result.Failed)
                        return result;
                }
                else
                    result = Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)from.ToString(), (object)"Vector2");
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
                case OperationType.RelationalEquals:
                case OperationType.RelationalNotEquals:
                case OperationType.MathNegate:
                    return true;
                default:
                    return false;
            }
        }

        private static object ExecuteOperation(object leftObj, object rightObj, OperationType op)
        {
            Vector2 vector2_1 = (Vector2)leftObj;
            if (op == OperationType.MathNegate)
                return (object)-vector2_1;
            Vector2 vector2_2 = (Vector2)rightObj;
            switch (op - 1)
            {
                case (OperationType)0:
                    return (object)(vector2_1 + vector2_2);
                case OperationType.MathAdd:
                    return (object)(vector2_1 - vector2_2);
                case OperationType.MathSubtract:
                    return (object)(vector2_1 * vector2_2);
                case OperationType.MathMultiply:
                    return (object)(vector2_1 / vector2_2);
                case OperationType.LogicalOr:
                    return BooleanBoxes.Box(vector2_1 == vector2_2);
                case OperationType.RelationalEquals:
                    return BooleanBoxes.Box(vector2_1 != vector2_2);
                default:
                    return (object)null;
            }
        }

        private static object CallTryParseStringVector2(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            Vector2 parameter2 = (Vector2)parameters[1];
            object instanceObj1;
            return Vector2Schema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        private static Result RangeValidateNotNegative(object value)
        {
            Vector2 vector2 = (Vector2)value;
            return (double)vector2.X < 0.0 || (double)vector2.Y < 0.0 ? Result.Fail("Expecting a non-negative value, but got {0}", (object)vector2.ToString()) : Result.Success;
        }

        public static void Pass1Initialize() => Vector2Schema.Type = new UIXTypeSchema((short)233, "Vector2", (string)null, (short)153, typeof(Vector2), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)233, "X", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(Vector2Schema.GetX), new SetValueHandler(Vector2Schema.SetX), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)233, "Y", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(Vector2Schema.GetY), new SetValueHandler(Vector2Schema.SetY), false);
            UIXConstructorSchema constructorSchema = new UIXConstructorSchema((short)233, new short[2]
            {
        (short) 194,
        (short) 194
            }, new ConstructHandler(Vector2Schema.ConstructXY));
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)233, "TryParse", new short[2]
            {
        (short) 208,
        (short) 233
            }, (short)233, new InvokeHandler(Vector2Schema.CallTryParseStringVector2), true);
            Vector2Schema.Type.Initialize(new DefaultConstructHandler(Vector2Schema.Construct), new ConstructorSchema[1]
            {
        (ConstructorSchema) constructorSchema
            }, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(Vector2Schema.TryConvertFrom), new SupportsTypeConversionHandler(Vector2Schema.IsConversionSupported), new EncodeBinaryHandler(Vector2Schema.EncodeBinary), new DecodeBinaryHandler(Vector2Schema.DecodeBinary), new PerformOperationHandler(Vector2Schema.ExecuteOperation), new SupportsOperationHandler(Vector2Schema.IsOperationSupported));
        }
    }
}
