// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.RotationSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using System;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class RotationSchema
    {
        public static UIXTypeSchema Type;

        private static object GetAxis(object instanceObj) => (object)((Rotation)instanceObj).Axis;

        private static void SetAxis(ref object instanceObj, object valueObj)
        {
            Rotation rotation = (Rotation)instanceObj;
            Vector3 vector3 = (Vector3)valueObj;
            rotation.Axis = vector3;
            instanceObj = (object)rotation;
        }

        private static object GetAngleRadians(object instanceObj) => (object)((Rotation)instanceObj).AngleRadians;

        private static void SetAngleRadians(ref object instanceObj, object valueObj)
        {
            Rotation rotation = (Rotation)instanceObj;
            float num = (float)valueObj;
            rotation.AngleRadians = num;
            instanceObj = (object)rotation;
        }

        private static object GetAngleDegrees(object instanceObj) => (object)((Rotation)instanceObj).AngleDegrees;

        private static void SetAngleDegrees(ref object instanceObj, object valueObj)
        {
            Rotation rotation = (Rotation)instanceObj;
            int num = (int)valueObj;
            rotation.AngleDegrees = num;
            instanceObj = (object)rotation;
        }

        private static object Construct() => (object)Rotation.Default;

        private static object ConstructAngleDegrees(object[] parameters)
        {
            object instanceObj = RotationSchema.Construct();
            RotationSchema.SetAngleDegrees(ref instanceObj, parameters[0]);
            return instanceObj;
        }

        private static object ConstructAngleRadians(object[] parameters)
        {
            object instanceObj = RotationSchema.Construct();
            RotationSchema.SetAngleRadians(ref instanceObj, parameters[0]);
            return instanceObj;
        }

        private static object ConstructAngleDegreesAxis(object[] parameters)
        {
            object instanceObj = RotationSchema.Construct();
            RotationSchema.SetAngleDegrees(ref instanceObj, parameters[0]);
            RotationSchema.SetAxis(ref instanceObj, parameters[1]);
            return instanceObj;
        }

        private static object ConstructAngleRadiansAxis(object[] parameters)
        {
            object instanceObj = RotationSchema.Construct();
            RotationSchema.SetAngleRadians(ref instanceObj, parameters[0]);
            RotationSchema.SetAxis(ref instanceObj, parameters[1]);
            return instanceObj;
        }

        private static object ConstructAxis(object[] parameters)
        {
            object instanceObj = RotationSchema.Construct();
            RotationSchema.SetAxis(ref instanceObj, parameters[0]);
            return instanceObj;
        }

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            Rotation rotation = (Rotation)instanceObj;
            Vector3Schema.Type.EncodeBinary(writer, (object)rotation.Axis);
            writer.WriteSingle(rotation.AngleRadians);
        }

        private static object DecodeBinary(ByteCodeReader reader)
        {
            Vector3 axis = (Vector3)Vector3Schema.Type.DecodeBinary(reader);
            return (object)new Rotation(reader.ReadSingle(), axis);
        }

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string str1 = (string)valueObj;
            instanceObj = (object)null;
            Rotation rotation = Rotation.Default;
            string[] strArray = str1.Split(';');
            if (strArray.Length != 2)
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)str1, (object)"Rotation");
            string str2 = strArray[1];
            object instance1;
            Result result1 = Vector3Schema.Type.TypeConverter((object)str2, (TypeSchema)StringSchema.Type, out instance1);
            if (result1.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Rotation", (object)result1.Error);
            rotation.Axis = (Vector3)instance1;
            string str3 = strArray[0];
            if (str3.EndsWith("rad", StringComparison.Ordinal))
            {
                string str4 = str3.Substring(0, str3.Length - 3);
                object instance2;
                Result result2 = SingleSchema.Type.TypeConverter((object)str4, (TypeSchema)StringSchema.Type, out instance2);
                if (result2.Failed)
                    return Result.Fail("Problem converting '{0}' ({1})", (object)"Rotation", (object)result2.Error);
                rotation.AngleRadians = (float)instance2;
            }
            else if (str3.EndsWith("deg", StringComparison.Ordinal))
            {
                string str4 = str3.Substring(0, str3.Length - 3);
                object instance2;
                Result result2 = Int32Schema.Type.TypeConverter((object)str4, (TypeSchema)StringSchema.Type, out instance2);
                if (result2.Failed)
                    return Result.Fail("Problem converting '{0}' ({1})", (object)"Rotation", (object)result2.Error);
                rotation.AngleDegrees = (int)instance2;
            }
            instanceObj = (object)rotation;
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
                result = RotationSchema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            return result;
        }

        private static object CallTryParseStringRotation(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            Rotation parameter2 = (Rotation)parameters[1];
            object instanceObj1;
            return RotationSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        public static void Pass1Initialize() => RotationSchema.Type = new UIXTypeSchema((short)176, "Rotation", (string)null, (short)153, typeof(Rotation), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)176, "Axis", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RotationSchema.GetAxis), new SetValueHandler(RotationSchema.SetAxis), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)176, "AngleRadians", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RotationSchema.GetAngleRadians), new SetValueHandler(RotationSchema.SetAngleRadians), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)176, "AngleDegrees", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RotationSchema.GetAngleDegrees), new SetValueHandler(RotationSchema.SetAngleDegrees), false);
            UIXConstructorSchema constructorSchema1 = new UIXConstructorSchema((short)176, new short[1]
            {
        (short) 115
            }, new ConstructHandler(RotationSchema.ConstructAngleDegrees));
            UIXConstructorSchema constructorSchema2 = new UIXConstructorSchema((short)176, new short[1]
            {
        (short) 194
            }, new ConstructHandler(RotationSchema.ConstructAngleRadians));
            UIXConstructorSchema constructorSchema3 = new UIXConstructorSchema((short)176, new short[2]
            {
        (short) 115,
        (short) 234
            }, new ConstructHandler(RotationSchema.ConstructAngleDegreesAxis));
            UIXConstructorSchema constructorSchema4 = new UIXConstructorSchema((short)176, new short[2]
            {
        (short) 194,
        (short) 234
            }, new ConstructHandler(RotationSchema.ConstructAngleRadiansAxis));
            UIXConstructorSchema constructorSchema5 = new UIXConstructorSchema((short)176, new short[1]
            {
        (short) 234
            }, new ConstructHandler(RotationSchema.ConstructAxis));
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)176, "TryParse", new short[2]
            {
        (short) 208,
        (short) 176
            }, (short)176, new InvokeHandler(RotationSchema.CallTryParseStringRotation), true);
            RotationSchema.Type.Initialize(new DefaultConstructHandler(RotationSchema.Construct), new ConstructorSchema[5]
            {
        (ConstructorSchema) constructorSchema1,
        (ConstructorSchema) constructorSchema2,
        (ConstructorSchema) constructorSchema3,
        (ConstructorSchema) constructorSchema4,
        (ConstructorSchema) constructorSchema5
            }, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(RotationSchema.TryConvertFrom), new SupportsTypeConversionHandler(RotationSchema.IsConversionSupported), new EncodeBinaryHandler(RotationSchema.EncodeBinary), new DecodeBinaryHandler(RotationSchema.DecodeBinary), (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
