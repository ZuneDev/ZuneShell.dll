// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.MajorMinorSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.Library;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class MajorMinorSchema
    {
        public static UIXTypeSchema Type;

        private static object GetMajor(object instanceObj) => (object)((MajorMinor)instanceObj).Major;

        private static void SetMajor(ref object instanceObj, object valueObj)
        {
            MajorMinor majorMinor = (MajorMinor)instanceObj;
            int num = (int)valueObj;
            majorMinor.Major = num;
            instanceObj = (object)majorMinor;
        }

        private static object GetMinor(object instanceObj) => (object)((MajorMinor)instanceObj).Minor;

        private static void SetMinor(ref object instanceObj, object valueObj)
        {
            MajorMinor majorMinor = (MajorMinor)instanceObj;
            int num = (int)valueObj;
            majorMinor.Minor = num;
            instanceObj = (object)majorMinor;
        }

        private static object Construct() => (object)MajorMinor.Zero;

        private static object ConstructMajorMinor(object[] parameters)
        {
            object instanceObj = MajorMinorSchema.Construct();
            MajorMinorSchema.SetMajor(ref instanceObj, parameters[0]);
            MajorMinorSchema.SetMinor(ref instanceObj, parameters[1]);
            return instanceObj;
        }

        private static Result ConvertFromStringMajorMinor(
          string[] splitString,
          out object instance)
        {
            instance = MajorMinorSchema.Construct();
            object valueObj1;
            Result result1 = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)Int32Schema.Type, (RangeValidator)null, out valueObj1);
            if (result1.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"MajorMinor", (object)result1.Error);
            MajorMinorSchema.SetMajor(ref instance, valueObj1);
            object valueObj2;
            Result result2 = UIXLoadResult.ValidateStringAsValue(splitString[1], (TypeSchema)Int32Schema.Type, (RangeValidator)null, out valueObj2);
            if (result2.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"MajorMinor", (object)result2.Error);
            MajorMinorSchema.SetMinor(ref instance, valueObj2);
            return result2;
        }

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            MajorMinor majorMinor = (MajorMinor)instanceObj;
            writer.WriteInt32(majorMinor.Major);
            writer.WriteInt32(majorMinor.Minor);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)new MajorMinor(reader.ReadInt32(), reader.ReadInt32());

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
                string[] splitString = StringUtility.SplitAndTrim(',', (string)from);
                if (splitString.Length == 2)
                {
                    result = MajorMinorSchema.ConvertFromStringMajorMinor(splitString, out instance);
                    if (!result.Failed)
                        return result;
                }
                else
                    result = Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)from.ToString(), (object)"MajorMinor");
            }
            return result;
        }

        public static void Pass1Initialize() => MajorMinorSchema.Type = new UIXTypeSchema((short)139, "MajorMinor", (string)null, (short)153, typeof(MajorMinor), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)139, "Major", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(MajorMinorSchema.GetMajor), new SetValueHandler(MajorMinorSchema.SetMajor), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)139, "Minor", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(MajorMinorSchema.GetMinor), new SetValueHandler(MajorMinorSchema.SetMinor), false);
            UIXConstructorSchema constructorSchema = new UIXConstructorSchema((short)139, new short[2]
            {
        (short) 115,
        (short) 115
            }, new ConstructHandler(MajorMinorSchema.ConstructMajorMinor));
            MajorMinorSchema.Type.Initialize(new DefaultConstructHandler(MajorMinorSchema.Construct), new ConstructorSchema[1]
            {
        (ConstructorSchema) constructorSchema
            }, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(MajorMinorSchema.TryConvertFrom), new SupportsTypeConversionHandler(MajorMinorSchema.IsConversionSupported), new EncodeBinaryHandler(MajorMinorSchema.EncodeBinary), new DecodeBinaryHandler(MajorMinorSchema.DecodeBinary), (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
