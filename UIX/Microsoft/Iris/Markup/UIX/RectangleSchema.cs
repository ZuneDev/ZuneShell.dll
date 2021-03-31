// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.RectangleSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using System;
using System.Globalization;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class RectangleSchema
    {
        public static UIXTypeSchema Type;

        private static object GetX(object instanceObj) => (object)((Rectangle)instanceObj).X;

        private static void SetX(ref object instanceObj, object valueObj)
        {
            Rectangle rectangle = (Rectangle)instanceObj;
            int num = (int)valueObj;
            rectangle.X = num;
            instanceObj = (object)rectangle;
        }

        private static object GetY(object instanceObj) => (object)((Rectangle)instanceObj).Y;

        private static void SetY(ref object instanceObj, object valueObj)
        {
            Rectangle rectangle = (Rectangle)instanceObj;
            int num = (int)valueObj;
            rectangle.Y = num;
            instanceObj = (object)rectangle;
        }

        private static object GetWidth(object instanceObj) => (object)((Rectangle)instanceObj).Width;

        private static void SetWidth(ref object instanceObj, object valueObj)
        {
            Rectangle rectangle = (Rectangle)instanceObj;
            int num = (int)valueObj;
            rectangle.Width = num;
            instanceObj = (object)rectangle;
        }

        private static object GetHeight(object instanceObj) => (object)((Rectangle)instanceObj).Height;

        private static void SetHeight(ref object instanceObj, object valueObj)
        {
            Rectangle rectangle = (Rectangle)instanceObj;
            int num = (int)valueObj;
            rectangle.Height = num;
            instanceObj = (object)rectangle;
        }

        private static object GetLeft(object instanceObj) => (object)((Rectangle)instanceObj).Left;

        private static object GetTop(object instanceObj) => (object)((Rectangle)instanceObj).Top;

        private static object GetRight(object instanceObj) => (object)((Rectangle)instanceObj).Right;

        private static object GetBottom(object instanceObj) => (object)((Rectangle)instanceObj).Bottom;

        private static object Construct() => (object)Rectangle.Zero;

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string s = (string)valueObj;
            instanceObj = (object)null;
            int result;
            if (!int.TryParse(s, NumberStyles.Integer, (IFormatProvider)NumberFormatInfo.InvariantInfo, out result))
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)s, (object)"Int32");
            Rectangle rectangle1 = new Rectangle();
            Rectangle rectangle2 = Rectangle.FromLTRB(result, result, result, result);
            instanceObj = (object)rectangle2;
            return Result.Success;
        }

        private static Result ConvertFromInt32(object valueObj, out object instanceObj)
        {
            int num1 = (int)valueObj;
            instanceObj = (object)null;
            int num2 = num1;
            Rectangle rectangle = Rectangle.FromLTRB(num2, num2, num2, num2);
            instanceObj = (object)rectangle;
            return Result.Success;
        }

        private static Result ConvertFromSingle(object valueObj, out object instanceObj)
        {
            float num1 = (float)valueObj;
            instanceObj = (object)null;
            int num2 = (int)num1;
            Rectangle rectangle = Rectangle.FromLTRB(num2, num2, num2, num2);
            instanceObj = (object)rectangle;
            return Result.Success;
        }

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            Rectangle rectangle = (Rectangle)instanceObj;
            writer.WriteInt32(rectangle.Left);
            writer.WriteInt32(rectangle.Top);
            writer.WriteInt32(rectangle.Right);
            writer.WriteInt32(rectangle.Bottom);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)Rectangle.FromLTRB(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

        private static object CallContainsPoint(object instanceObj, object[] parameters) => BooleanBoxes.Box(((Rectangle)instanceObj).Contains((Point)parameters[0]));

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
                result = RectangleSchema.ConvertFromInt32(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (SingleSchema.Type.IsAssignableFrom(fromType))
            {
                result = RectangleSchema.ConvertFromSingle(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                result = RectangleSchema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            return result;
        }

        private static bool IsOperationSupported(OperationType op)
        {
            switch (op)
            {
                case OperationType.RelationalEquals:
                case OperationType.RelationalNotEquals:
                    return true;
                default:
                    return false;
            }
        }

        private static object ExecuteOperation(object leftObj, object rightObj, OperationType op)
        {
            Rectangle rectangle1 = (Rectangle)leftObj;
            Rectangle rectangle2 = (Rectangle)rightObj;
            switch (op)
            {
                case OperationType.RelationalEquals:
                    return BooleanBoxes.Box(rectangle1 == rectangle2);
                case OperationType.RelationalNotEquals:
                    return BooleanBoxes.Box(rectangle1 != rectangle2);
                default:
                    return (object)null;
            }
        }

        private static object CallTryParseStringRectangle(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            Rectangle parameter2 = (Rectangle)parameters[1];
            object instanceObj1;
            return RectangleSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        public static void Pass1Initialize() => RectangleSchema.Type = new UIXTypeSchema((short)169, "Rectangle", (string)null, (short)153, typeof(Rectangle), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)169, "X", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RectangleSchema.GetX), new SetValueHandler(RectangleSchema.SetX), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)169, "Y", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RectangleSchema.GetY), new SetValueHandler(RectangleSchema.SetY), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)169, "Width", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RectangleSchema.GetWidth), new SetValueHandler(RectangleSchema.SetWidth), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)169, "Height", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RectangleSchema.GetHeight), new SetValueHandler(RectangleSchema.SetHeight), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)169, "Left", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RectangleSchema.GetLeft), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)169, "Top", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RectangleSchema.GetTop), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)169, "Right", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RectangleSchema.GetRight), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)169, "Bottom", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RectangleSchema.GetBottom), (SetValueHandler)null, false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)169, "Contains", new short[1]
            {
        (short) 158
            }, (short)15, new InvokeHandler(RectangleSchema.CallContainsPoint), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)169, "TryParse", new short[2]
            {
        (short) 208,
        (short) 169
            }, (short)169, new InvokeHandler(RectangleSchema.CallTryParseStringRectangle), true);
            RectangleSchema.Type.Initialize(new DefaultConstructHandler(RectangleSchema.Construct), (ConstructorSchema[])null, new PropertySchema[8]
            {
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[2]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(RectangleSchema.TryConvertFrom), new SupportsTypeConversionHandler(RectangleSchema.IsConversionSupported), new EncodeBinaryHandler(RectangleSchema.EncodeBinary), new DecodeBinaryHandler(RectangleSchema.DecodeBinary), new PerformOperationHandler(RectangleSchema.ExecuteOperation), new SupportsOperationHandler(RectangleSchema.IsOperationSupported));
        }
    }
}
