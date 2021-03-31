// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.InterpolationSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Library;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class InterpolationSchema
    {
        public static RangeValidator ValidateEasePercent = new RangeValidator(InterpolationSchema.RangeValidateEasePercent);
        public static UIXTypeSchema Type;

        private static object GetType(object instanceObj) => (object)((Interpolation)instanceObj).Type;

        private static void SetType(ref object instanceObj, object valueObj) => ((Interpolation)instanceObj).Type = (InterpolationType)valueObj;

        private static object GetWeight(object instanceObj) => (object)((Interpolation)instanceObj).Weight;

        private static void SetWeight(ref object instanceObj, object valueObj)
        {
            Interpolation interpolation = (Interpolation)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                interpolation.Weight = num;
        }

        private static object GetBezierHandle1(object instanceObj) => (object)((Interpolation)instanceObj).BezierHandle1;

        private static void SetBezierHandle1(ref object instanceObj, object valueObj) => ((Interpolation)instanceObj).BezierHandle1 = (float)valueObj;

        private static object GetBezierHandle2(object instanceObj) => (object)((Interpolation)instanceObj).BezierHandle2;

        private static void SetBezierHandle2(ref object instanceObj, object valueObj) => ((Interpolation)instanceObj).BezierHandle2 = (float)valueObj;

        private static object GetEasePercent(object instanceObj) => (object)((Interpolation)instanceObj).EasePercent;

        private static void SetEasePercent(ref object instanceObj, object valueObj)
        {
            Interpolation interpolation = (Interpolation)instanceObj;
            float num = (float)valueObj;
            Result result = InterpolationSchema.ValidateEasePercent(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                interpolation.EasePercent = num;
        }

        private static object Construct() => (object)new Interpolation();

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            Interpolation interpolation = (Interpolation)instanceObj;
            writer.WriteInt32((int)interpolation.Type);
            writer.WriteSingle(interpolation.Weight);
            writer.WriteSingle(interpolation.BezierHandle1);
            writer.WriteSingle(interpolation.BezierHandle2);
            writer.WriteSingle(interpolation.EasePercent);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)new Interpolation()
        {
            Type = (InterpolationType)reader.ReadInt32(),
            Weight = reader.ReadSingle(),
            BezierHandle1 = reader.ReadSingle(),
            BezierHandle2 = reader.ReadSingle(),
            EasePercent = reader.ReadSingle()
        };

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string str = (string)valueObj;
            instanceObj = (object)null;
            string[] strArray = str.Split(',');
            if (strArray.Length < 1)
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)str, (object)"Interpolation");
            Interpolation interpolation = new Interpolation();
            instanceObj = (object)interpolation;
            object valueObj1;
            Result result = UIXLoadResult.ValidateStringAsValue(strArray[0], UIXLoadResultExports.InterpolationTypeType, (RangeValidator)null, out valueObj1);
            if (result.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Interpolation", (object)result.Error);
            InterpolationSchema.SetType(ref instanceObj, valueObj1);
            if (strArray.Length == 2)
            {
                object valueObj2;
                result = UIXLoadResult.ValidateStringAsValue(strArray[1], (TypeSchema)SingleSchema.Type, SingleSchema.ValidateNotNegative, out valueObj2);
                if (result.Failed)
                    return Result.Fail("Problem converting '{0}' ({1})", (object)"Interpolation", (object)result.Error);
                InterpolationSchema.SetWeight(ref instanceObj, valueObj2);
            }
            else if (strArray.Length == 3)
            {
                if (interpolation.Type == InterpolationType.Bezier)
                {
                    object valueObj2;
                    result = UIXLoadResult.ValidateStringAsValue(strArray[1], (TypeSchema)SingleSchema.Type, (RangeValidator)null, out valueObj2);
                    if (result.Failed)
                        return Result.Fail("Problem converting '{0}' ({1})", (object)"Interpolation", (object)result.Error);
                    InterpolationSchema.SetBezierHandle1(ref instanceObj, valueObj2);
                    object valueObj3;
                    result = UIXLoadResult.ValidateStringAsValue(strArray[2], (TypeSchema)SingleSchema.Type, (RangeValidator)null, out valueObj3);
                    if (result.Failed)
                        return Result.Fail("Problem converting '{0}' ({1})", (object)"Interpolation", (object)result.Error);
                    InterpolationSchema.SetBezierHandle2(ref instanceObj, valueObj3);
                }
                else
                {
                    object valueObj2;
                    result = UIXLoadResult.ValidateStringAsValue(strArray[1], (TypeSchema)SingleSchema.Type, SingleSchema.ValidateNotNegative, out valueObj2);
                    if (result.Failed)
                        return Result.Fail("Problem converting '{0}' ({1})", (object)"Interpolation", (object)result.Error);
                    InterpolationSchema.SetWeight(ref instanceObj, valueObj2);
                    object valueObj3;
                    result = UIXLoadResult.ValidateStringAsValue(strArray[2], (TypeSchema)SingleSchema.Type, InterpolationSchema.ValidateEasePercent, out valueObj3);
                    if (result.Failed)
                        return Result.Fail("Problem converting '{0}' ({1})", (object)"Interpolation", (object)result.Error);
                    InterpolationSchema.SetEasePercent(ref instanceObj, valueObj3);
                }
            }
            else if (strArray.Length >= 4)
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)str, (object)"Interpolation");
            instanceObj = (object)interpolation;
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
                result = InterpolationSchema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            return result;
        }

        private static object CallTryParseStringInterpolation(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            Interpolation parameter2 = (Interpolation)parameters[1];
            object instanceObj1;
            return InterpolationSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        private static Result RangeValidateEasePercent(object value)
        {
            float num = (float)value;
            return (double)num <= 0.0 || (double)num >= 1.0 ? Result.Fail("Expecting a value between {0} and {1} (exclusive), but got {2}", (object)"0.0", (object)"1.0", (object)num.ToString()) : Result.Success;
        }

        public static void Pass1Initialize() => InterpolationSchema.Type = new UIXTypeSchema((short)121, "Interpolation", (string)null, (short)153, typeof(Interpolation), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)121, "Type", (short)122, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(InterpolationSchema.GetType), new SetValueHandler(InterpolationSchema.SetType), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)121, "Weight", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, new GetValueHandler(InterpolationSchema.GetWeight), new SetValueHandler(InterpolationSchema.SetWeight), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)121, "BezierHandle1", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(InterpolationSchema.GetBezierHandle1), new SetValueHandler(InterpolationSchema.SetBezierHandle1), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)121, "BezierHandle2", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(InterpolationSchema.GetBezierHandle2), new SetValueHandler(InterpolationSchema.SetBezierHandle2), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)121, "EasePercent", (short)194, (short)-1, ExpressionRestriction.None, false, InterpolationSchema.ValidateEasePercent, false, new GetValueHandler(InterpolationSchema.GetEasePercent), new SetValueHandler(InterpolationSchema.SetEasePercent), false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)121, "TryParse", new short[2]
            {
        (short) 208,
        (short) 121
            }, (short)121, new InvokeHandler(InterpolationSchema.CallTryParseStringInterpolation), true);
            InterpolationSchema.Type.Initialize(new DefaultConstructHandler(InterpolationSchema.Construct), (ConstructorSchema[])null, new PropertySchema[5]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(InterpolationSchema.TryConvertFrom), new SupportsTypeConversionHandler(InterpolationSchema.IsConversionSupported), new EncodeBinaryHandler(InterpolationSchema.EncodeBinary), new DecodeBinaryHandler(InterpolationSchema.DecodeBinary), (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
