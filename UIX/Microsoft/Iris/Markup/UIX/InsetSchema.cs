// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.InsetSchema
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
    internal static class InsetSchema
    {
        private static readonly object s_Default = (object)Inset.Zero;
        public static UIXTypeSchema Type;

        private static object GetLeft(object instanceObj) => (object)((Inset)instanceObj).Left;

        private static void SetLeft(ref object instanceObj, object valueObj)
        {
            Inset inset = (Inset)instanceObj;
            int num = (int)valueObj;
            inset.Left = num;
            instanceObj = (object)inset;
        }

        private static object GetTop(object instanceObj) => (object)((Inset)instanceObj).Top;

        private static void SetTop(ref object instanceObj, object valueObj)
        {
            Inset inset = (Inset)instanceObj;
            int num = (int)valueObj;
            inset.Top = num;
            instanceObj = (object)inset;
        }

        private static object GetRight(object instanceObj) => (object)((Inset)instanceObj).Right;

        private static void SetRight(ref object instanceObj, object valueObj)
        {
            Inset inset = (Inset)instanceObj;
            int num = (int)valueObj;
            inset.Right = num;
            instanceObj = (object)inset;
        }

        private static object GetBottom(object instanceObj) => (object)((Inset)instanceObj).Bottom;

        private static void SetBottom(ref object instanceObj, object valueObj)
        {
            Inset inset = (Inset)instanceObj;
            int num = (int)valueObj;
            inset.Bottom = num;
            instanceObj = (object)inset;
        }

        private static object Construct() => InsetSchema.s_Default;

        private static object ConstructInt32(object[] parameters)
        {
            int parameter = (int)parameters[0];
            return (object)new Inset(parameter, parameter, parameter, parameter);
        }

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            Inset inset = (Inset)instanceObj;
            writer.WriteInt32(inset.Left);
            writer.WriteInt32(inset.Top);
            writer.WriteInt32(inset.Right);
            writer.WriteInt32(inset.Bottom);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)new Inset(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string s = (string)valueObj;
            instanceObj = (object)null;
            int result;
            if (!int.TryParse(s, NumberStyles.Integer, (IFormatProvider)NumberFormatInfo.InvariantInfo, out result))
                return Result.Fail("");
            instanceObj = (object)new Inset()
            {
                Left = result,
                Top = result,
                Right = result,
                Bottom = result
            };
            return Result.Success;
        }

        private static Result ConvertFromInt32(object valueObj, out object instanceObj)
        {
            int num = (int)valueObj;
            instanceObj = (object)null;
            instanceObj = (object)new Inset()
            {
                Left = num,
                Top = num,
                Right = num,
                Bottom = num
            };
            return Result.Success;
        }

        private static Result ConvertFromSingle(object valueObj, out object instanceObj)
        {
            float num1 = (float)valueObj;
            instanceObj = (object)null;
            Inset inset = new Inset();
            int num2 = (int)num1;
            inset.Left = num2;
            inset.Top = num2;
            inset.Right = num2;
            inset.Bottom = num2;
            instanceObj = (object)inset;
            return Result.Success;
        }

        private static object ConstructLeftTopRightBottom(object[] parameters)
        {
            object instanceObj = InsetSchema.Construct();
            InsetSchema.SetLeft(ref instanceObj, parameters[0]);
            InsetSchema.SetTop(ref instanceObj, parameters[1]);
            InsetSchema.SetRight(ref instanceObj, parameters[2]);
            InsetSchema.SetBottom(ref instanceObj, parameters[3]);
            return instanceObj;
        }

        private static Result ConvertFromStringLeftTopRightBottom(
          string[] splitString,
          out object instance)
        {
            instance = InsetSchema.Construct();
            object valueObj1;
            Result result1 = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)Int32Schema.Type, (RangeValidator)null, out valueObj1);
            if (result1.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Inset", (object)result1.Error);
            InsetSchema.SetLeft(ref instance, valueObj1);
            object valueObj2;
            Result result2 = UIXLoadResult.ValidateStringAsValue(splitString[1], (TypeSchema)Int32Schema.Type, (RangeValidator)null, out valueObj2);
            if (result2.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Inset", (object)result2.Error);
            InsetSchema.SetTop(ref instance, valueObj2);
            object valueObj3;
            Result result3 = UIXLoadResult.ValidateStringAsValue(splitString[2], (TypeSchema)Int32Schema.Type, (RangeValidator)null, out valueObj3);
            if (result3.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Inset", (object)result3.Error);
            InsetSchema.SetRight(ref instance, valueObj3);
            object valueObj4;
            Result result4 = UIXLoadResult.ValidateStringAsValue(splitString[3], (TypeSchema)Int32Schema.Type, (RangeValidator)null, out valueObj4);
            if (result4.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Inset", (object)result4.Error);
            InsetSchema.SetBottom(ref instance, valueObj4);
            return result4;
        }

        private static bool IsConversionSupported(TypeSchema fromType) => Int32Schema.Type.IsAssignableFrom(fromType) || SingleSchema.Type.IsAssignableFrom(fromType) || StringSchema.Type.IsAssignableFrom(fromType);

        private static Result TryConvertFrom(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result = Result.Fail("Unsupported");
            instance = (object)null;
            if (Int32Schema.Type.IsAssignableFrom(fromType))
            {
                result = InsetSchema.ConvertFromInt32(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (SingleSchema.Type.IsAssignableFrom(fromType))
            {
                result = InsetSchema.ConvertFromSingle(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                result = InsetSchema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                string[] splitString = StringUtility.SplitAndTrim(',', (string)from);
                if (splitString.Length == 4)
                {
                    result = InsetSchema.ConvertFromStringLeftTopRightBottom(splitString, out instance);
                    if (!result.Failed)
                        return result;
                }
                else
                    result = Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)from.ToString(), (object)"Inset");
            }
            return result;
        }

        private static bool IsOperationSupported(OperationType op)
        {
            switch (op)
            {
                case OperationType.MathAdd:
                case OperationType.MathSubtract:
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
            Inset inset1 = (Inset)leftObj;
            if (op == OperationType.MathNegate)
                return (object)-inset1;
            Inset inset2 = (Inset)rightObj;
            switch (op)
            {
                case OperationType.MathAdd:
                    return (object)(inset1 + inset2);
                case OperationType.MathSubtract:
                    return (object)(inset1 - inset2);
                case OperationType.RelationalEquals:
                    return BooleanBoxes.Box(inset1 == inset2);
                case OperationType.RelationalNotEquals:
                    return BooleanBoxes.Box(inset1 != inset2);
                default:
                    return (object)null;
            }
        }

        private static object CallTryParseStringInset(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            Inset parameter2 = (Inset)parameters[1];
            object instanceObj1;
            return InsetSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        public static void Pass1Initialize() => InsetSchema.Type = new UIXTypeSchema((short)114, "Inset", (string)null, (short)153, typeof(Inset), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)114, "Left", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(InsetSchema.GetLeft), new SetValueHandler(InsetSchema.SetLeft), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)114, "Top", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(InsetSchema.GetTop), new SetValueHandler(InsetSchema.SetTop), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)114, "Right", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(InsetSchema.GetRight), new SetValueHandler(InsetSchema.SetRight), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)114, "Bottom", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(InsetSchema.GetBottom), new SetValueHandler(InsetSchema.SetBottom), false);
            UIXConstructorSchema constructorSchema1 = new UIXConstructorSchema((short)114, new short[1]
            {
        (short) 115
            }, new ConstructHandler(InsetSchema.ConstructInt32));
            UIXConstructorSchema constructorSchema2 = new UIXConstructorSchema((short)114, new short[4]
            {
        (short) 115,
        (short) 115,
        (short) 115,
        (short) 115
            }, new ConstructHandler(InsetSchema.ConstructLeftTopRightBottom));
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)114, "TryParse", new short[2]
            {
        (short) 208,
        (short) 114
            }, (short)114, new InvokeHandler(InsetSchema.CallTryParseStringInset), true);
            InsetSchema.Type.Initialize(new DefaultConstructHandler(InsetSchema.Construct), new ConstructorSchema[2]
            {
        (ConstructorSchema) constructorSchema1,
        (ConstructorSchema) constructorSchema2
            }, new PropertySchema[4]
            {
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(InsetSchema.TryConvertFrom), new SupportsTypeConversionHandler(InsetSchema.IsConversionSupported), new EncodeBinaryHandler(InsetSchema.EncodeBinary), new DecodeBinaryHandler(InsetSchema.DecodeBinary), new PerformOperationHandler(InsetSchema.ExecuteOperation), new SupportsOperationHandler(InsetSchema.IsOperationSupported));
        }
    }
}
