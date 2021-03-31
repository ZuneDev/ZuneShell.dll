// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.CharSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class CharSchema
    {
        public static UIXTypeSchema Type;

        private static object Construct() => (object)char.MinValue;

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            char ch = (char)instanceObj;
            writer.WriteChar(ch);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)reader.ReadChar();

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string str = (string)valueObj;
            instanceObj = (object)null;
            if (str == null || str.Length != 1)
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)str, (object)"Char");
            char ch = str[0];
            instanceObj = (object)ch;
            return Result.Success;
        }

        private static bool IsConversionSupported(TypeSchema fromType) => StringSchema.Type.IsAssignableFrom(fromType);

        private static Result TryConvertFrom(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result = Result.Fail("Unsupported");
            instance = (object)null;
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                result = CharSchema.ConvertFromString(from, out instance);
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
            char ch1 = (char)leftObj;
            char ch2 = (char)rightObj;
            switch (op)
            {
                case OperationType.RelationalEquals:
                    return BooleanBoxes.Box((int)ch1 == (int)ch2);
                case OperationType.RelationalNotEquals:
                    return BooleanBoxes.Box((int)ch1 != (int)ch2);
                default:
                    return (object)null;
            }
        }

        private static object CallTryParseStringChar(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            char parameter2 = (char)parameters[1];
            object instanceObj1;
            return CharSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        public static void Pass1Initialize() => CharSchema.Type = new UIXTypeSchema((short)27, "Char", "char", (short)153, typeof(char), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)27, "TryParse", new short[2]
            {
        (short) 208,
        (short) 27
            }, (short)27, new InvokeHandler(CharSchema.CallTryParseStringChar), true);
            CharSchema.Type.Initialize(new DefaultConstructHandler(CharSchema.Construct), (ConstructorSchema[])null, (PropertySchema[])null, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(CharSchema.TryConvertFrom), new SupportsTypeConversionHandler(CharSchema.IsConversionSupported), new EncodeBinaryHandler(CharSchema.EncodeBinary), new DecodeBinaryHandler(CharSchema.DecodeBinary), new PerformOperationHandler(CharSchema.ExecuteOperation), new SupportsOperationHandler(CharSchema.IsOperationSupported));
        }
    }
}
