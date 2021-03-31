// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.StringSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class StringSchema
    {
        public static UIXTypeSchema Type;

        private static object GetLength(object instanceObj) => (object)((string)instanceObj).Length;

        private static object Construct() => (object)string.Empty;

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            string str = (string)instanceObj;
            writer.WriteString(str);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)reader.ReadString();

        private static Result ConvertFromObject(object valueObj, out object instanceObj)
        {
            object obj = valueObj;
            instanceObj = (object)null;
            string str = obj == null ? (string)null : obj.ToString();
            instanceObj = (object)str;
            return Result.Success;
        }

        private static object CallIsNullOrEmptyString(object instanceObj, object[] parameters) => BooleanBoxes.Box(string.IsNullOrEmpty((string)parameters[0]));

        private static object CallSubstringInt32(object instanceObj, object[] parameters)
        {
            string str = (string)instanceObj;
            int parameter = (int)parameters[0];
            if (parameter >= 0 && parameter <= str.Length)
                return (object)str.Substring(parameter);
            ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)parameter, (object)"startIndex");
            return (object)null;
        }

        private static object CallSubstringInt32Int32(object instanceObj, object[] parameters)
        {
            string str = (string)instanceObj;
            int parameter1 = (int)parameters[0];
            int parameter2 = (int)parameters[1];
            if (parameter1 < 0 || parameter1 > str.Length)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)parameter1, (object)"startIndex");
                return (object)null;
            }
            if (parameter2 >= 0 && parameter1 + parameter2 >= 0 && parameter1 + parameter2 <= str.Length)
                return (object)str.Substring(parameter1, parameter2);
            ErrorManager.ReportError("Script runtime failure: Invalid '{0}' value is out of range for '{1}'", (object)parameter2, (object)"length");
            return (object)null;
        }

        private static object CallTrim(object instanceObj, object[] parameters) => (object)((string)instanceObj).Trim();

        private static object CallToLower(object instanceObj, object[] parameters) => (object)((string)instanceObj).ToLowerInvariant();

        private static object CallToUpper(object instanceObj, object[] parameters) => (object)((string)instanceObj).ToUpperInvariant();

        private static object CallFormatObject(object instanceObj, object[] parameters)
        {
            string format = (string)instanceObj;
            object parameter = parameters[0];
            return (object)string.Format(format, parameters);
        }

        private static object CallFormatObjectObject(object instanceObj, object[] parameters)
        {
            string format = (string)instanceObj;
            object parameter1 = parameters[0];
            object parameter2 = parameters[1];
            return (object)string.Format(format, parameters);
        }

        private static object CallFormatObjectObjectObject(object instanceObj, object[] parameters)
        {
            string format = (string)instanceObj;
            object parameter1 = parameters[0];
            object parameter2 = parameters[1];
            object parameter3 = parameters[2];
            return (object)string.Format(format, parameters);
        }

        private static object CallFormatObjectObjectObjectObject(
          object instanceObj,
          object[] parameters)
        {
            string format = (string)instanceObj;
            object parameter1 = parameters[0];
            object parameter2 = parameters[1];
            object parameter3 = parameters[2];
            object parameter4 = parameters[3];
            return (object)string.Format(format, parameters);
        }

        private static object CallFormatObjectObjectObjectObjectObject(
          object instanceObj,
          object[] parameters)
        {
            string format = (string)instanceObj;
            object parameter1 = parameters[0];
            object parameter2 = parameters[1];
            object parameter3 = parameters[2];
            object parameter4 = parameters[3];
            object parameter5 = parameters[4];
            return (object)string.Format(format, parameters);
        }

        private static bool IsConversionSupported(TypeSchema fromType) => ObjectSchema.Type.IsAssignableFrom(fromType);

        private static Result TryConvertFrom(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result = Result.Fail("Unsupported");
            instance = (object)null;
            if (ObjectSchema.Type.IsAssignableFrom(fromType))
            {
                result = StringSchema.ConvertFromObject(from, out instance);
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
                case OperationType.RelationalEquals:
                case OperationType.RelationalNotEquals:
                    return true;
                default:
                    return false;
            }
        }

        private static object ExecuteOperation(object leftObj, object rightObj, OperationType op)
        {
            string str1 = (string)leftObj;
            string str2 = (string)rightObj;
            switch (op)
            {
                case OperationType.MathAdd:
                    return (object)(str1 + str2);
                case OperationType.RelationalEquals:
                    return BooleanBoxes.Box(str1 == str2);
                case OperationType.RelationalNotEquals:
                    return BooleanBoxes.Box(str1 != str2);
                default:
                    return (object)null;
            }
        }

        public static void Pass1Initialize() => StringSchema.Type = new UIXTypeSchema((short)208, "String", "string", (short)153, typeof(string), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)208, "Length", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(StringSchema.GetLength), (SetValueHandler)null, false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)208, "IsNullOrEmpty", new short[1]
            {
        (short) 208
            }, (short)15, new InvokeHandler(StringSchema.CallIsNullOrEmptyString), true);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)208, "Substring", new short[1]
            {
        (short) 115
            }, (short)208, new InvokeHandler(StringSchema.CallSubstringInt32), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)208, "Substring", new short[2]
            {
        (short) 115,
        (short) 115
            }, (short)208, new InvokeHandler(StringSchema.CallSubstringInt32Int32), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)208, "Trim", (short[])null, (short)208, new InvokeHandler(StringSchema.CallTrim), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)208, "ToLower", (short[])null, (short)208, new InvokeHandler(StringSchema.CallToLower), false);
            UIXMethodSchema uixMethodSchema6 = new UIXMethodSchema((short)208, "ToUpper", (short[])null, (short)208, new InvokeHandler(StringSchema.CallToUpper), false);
            UIXMethodSchema uixMethodSchema7 = new UIXMethodSchema((short)208, "Format", new short[1]
            {
        (short) 153
            }, (short)208, new InvokeHandler(StringSchema.CallFormatObject), false);
            UIXMethodSchema uixMethodSchema8 = new UIXMethodSchema((short)208, "Format", new short[2]
            {
        (short) 153,
        (short) 153
            }, (short)208, new InvokeHandler(StringSchema.CallFormatObjectObject), false);
            UIXMethodSchema uixMethodSchema9 = new UIXMethodSchema((short)208, "Format", new short[3]
            {
        (short) 153,
        (short) 153,
        (short) 153
            }, (short)208, new InvokeHandler(StringSchema.CallFormatObjectObjectObject), false);
            UIXMethodSchema uixMethodSchema10 = new UIXMethodSchema((short)208, "Format", new short[4]
            {
        (short) 153,
        (short) 153,
        (short) 153,
        (short) 153
            }, (short)208, new InvokeHandler(StringSchema.CallFormatObjectObjectObjectObject), false);
            UIXMethodSchema uixMethodSchema11 = new UIXMethodSchema((short)208, "Format", new short[5]
            {
        (short) 153,
        (short) 153,
        (short) 153,
        (short) 153,
        (short) 153
            }, (short)208, new InvokeHandler(StringSchema.CallFormatObjectObjectObjectObjectObject), false);
            StringSchema.Type.Initialize(new DefaultConstructHandler(StringSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, new MethodSchema[11]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4,
        (MethodSchema) uixMethodSchema5,
        (MethodSchema) uixMethodSchema6,
        (MethodSchema) uixMethodSchema7,
        (MethodSchema) uixMethodSchema8,
        (MethodSchema) uixMethodSchema9,
        (MethodSchema) uixMethodSchema10,
        (MethodSchema) uixMethodSchema11
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(StringSchema.TryConvertFrom), new SupportsTypeConversionHandler(StringSchema.IsConversionSupported), new EncodeBinaryHandler(StringSchema.EncodeBinary), new DecodeBinaryHandler(StringSchema.DecodeBinary), new PerformOperationHandler(StringSchema.ExecuteOperation), new SupportsOperationHandler(StringSchema.IsOperationSupported));
        }
    }
}
