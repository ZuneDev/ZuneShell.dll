// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.AnchorEdgeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.Library;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class AnchorEdgeSchema
    {
        public static UIXTypeSchema Type;

        private static object GetId(object instanceObj) => (object)((AnchorEdge)instanceObj).Id;

        private static void SetId(ref object instanceObj, object valueObj) => ((AnchorEdge)instanceObj).Id = (string)valueObj;

        private static object GetPercent(object instanceObj) => (object)((AnchorEdge)instanceObj).Percent;

        private static void SetPercent(ref object instanceObj, object valueObj) => ((AnchorEdge)instanceObj).Percent = (float)valueObj;

        private static object GetOffset(object instanceObj) => (object)((AnchorEdge)instanceObj).Offset;

        private static void SetOffset(ref object instanceObj, object valueObj) => ((AnchorEdge)instanceObj).Offset = (int)valueObj;

        private static object GetMaximumPercent(object instanceObj) => (object)((AnchorEdge)instanceObj).MaximumPercent;

        private static void SetMaximumPercent(ref object instanceObj, object valueObj) => ((AnchorEdge)instanceObj).MaximumPercent = (float)valueObj;

        private static object GetMaximumOffset(object instanceObj) => (object)((AnchorEdge)instanceObj).MaximumOffset;

        private static void SetMaximumOffset(ref object instanceObj, object valueObj) => ((AnchorEdge)instanceObj).MaximumOffset = (int)valueObj;

        private static object GetMinimumPercent(object instanceObj) => (object)((AnchorEdge)instanceObj).MinimumPercent;

        private static void SetMinimumPercent(ref object instanceObj, object valueObj) => ((AnchorEdge)instanceObj).MinimumPercent = (float)valueObj;

        private static object GetMinimumOffset(object instanceObj) => (object)((AnchorEdge)instanceObj).MinimumOffset;

        private static void SetMinimumOffset(ref object instanceObj, object valueObj) => ((AnchorEdge)instanceObj).MinimumOffset = (int)valueObj;

        private static object Construct() => (object)new AnchorEdge();

        private static object ConstructIdPercent(object[] parameters)
        {
            object instanceObj = AnchorEdgeSchema.Construct();
            AnchorEdgeSchema.SetId(ref instanceObj, parameters[0]);
            AnchorEdgeSchema.SetPercent(ref instanceObj, parameters[1]);
            return instanceObj;
        }

        private static Result ConvertFromStringIdPercent(
          string[] splitString,
          out object instance)
        {
            instance = AnchorEdgeSchema.Construct();
            object valueObj1;
            Result result1 = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)StringSchema.Type, (RangeValidator)null, out valueObj1);
            if (result1.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"AnchorEdge", (object)result1.Error);
            AnchorEdgeSchema.SetId(ref instance, valueObj1);
            object valueObj2;
            Result result2 = UIXLoadResult.ValidateStringAsValue(splitString[1], (TypeSchema)SingleSchema.Type, (RangeValidator)null, out valueObj2);
            if (result2.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"AnchorEdge", (object)result2.Error);
            AnchorEdgeSchema.SetPercent(ref instance, valueObj2);
            return result2;
        }

        private static object ConstructIdPercentOffset(object[] parameters)
        {
            object instanceObj = AnchorEdgeSchema.Construct();
            AnchorEdgeSchema.SetId(ref instanceObj, parameters[0]);
            AnchorEdgeSchema.SetPercent(ref instanceObj, parameters[1]);
            AnchorEdgeSchema.SetOffset(ref instanceObj, parameters[2]);
            return instanceObj;
        }

        private static Result ConvertFromStringIdPercentOffset(
          string[] splitString,
          out object instance)
        {
            instance = AnchorEdgeSchema.Construct();
            object valueObj1;
            Result result1 = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)StringSchema.Type, (RangeValidator)null, out valueObj1);
            if (result1.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"AnchorEdge", (object)result1.Error);
            AnchorEdgeSchema.SetId(ref instance, valueObj1);
            object valueObj2;
            Result result2 = UIXLoadResult.ValidateStringAsValue(splitString[1], (TypeSchema)SingleSchema.Type, (RangeValidator)null, out valueObj2);
            if (result2.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"AnchorEdge", (object)result2.Error);
            AnchorEdgeSchema.SetPercent(ref instance, valueObj2);
            object valueObj3;
            Result result3 = UIXLoadResult.ValidateStringAsValue(splitString[2], (TypeSchema)Int32Schema.Type, (RangeValidator)null, out valueObj3);
            if (result3.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"AnchorEdge", (object)result3.Error);
            AnchorEdgeSchema.SetOffset(ref instance, valueObj3);
            return result3;
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
                string[] splitString = StringUtility.SplitAndTrim(',', (string)from);
                switch (splitString.Length)
                {
                    case 2:
                        result = AnchorEdgeSchema.ConvertFromStringIdPercent(splitString, out instance);
                        if (!result.Failed)
                            return result;
                        break;
                    case 3:
                        result = AnchorEdgeSchema.ConvertFromStringIdPercentOffset(splitString, out instance);
                        if (!result.Failed)
                            return result;
                        break;
                    default:
                        result = Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)from.ToString(), (object)"AnchorEdge");
                        break;
                }
            }
            return result;
        }

        public static void Pass1Initialize() => AnchorEdgeSchema.Type = new UIXTypeSchema((short)6, "AnchorEdge", (string)null, (short)153, typeof(AnchorEdge), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)6, "Id", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorEdgeSchema.GetId), new SetValueHandler(AnchorEdgeSchema.SetId), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)6, "Percent", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorEdgeSchema.GetPercent), new SetValueHandler(AnchorEdgeSchema.SetPercent), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)6, "Offset", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorEdgeSchema.GetOffset), new SetValueHandler(AnchorEdgeSchema.SetOffset), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)6, "MaximumPercent", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorEdgeSchema.GetMaximumPercent), new SetValueHandler(AnchorEdgeSchema.SetMaximumPercent), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)6, "MaximumOffset", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorEdgeSchema.GetMaximumOffset), new SetValueHandler(AnchorEdgeSchema.SetMaximumOffset), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)6, "MinimumPercent", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorEdgeSchema.GetMinimumPercent), new SetValueHandler(AnchorEdgeSchema.SetMinimumPercent), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)6, "MinimumOffset", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorEdgeSchema.GetMinimumOffset), new SetValueHandler(AnchorEdgeSchema.SetMinimumOffset), false);
            UIXConstructorSchema constructorSchema1 = new UIXConstructorSchema((short)6, new short[2]
            {
        (short) 208,
        (short) 194
            }, new ConstructHandler(AnchorEdgeSchema.ConstructIdPercent));
            UIXConstructorSchema constructorSchema2 = new UIXConstructorSchema((short)6, new short[3]
            {
        (short) 208,
        (short) 194,
        (short) 115
            }, new ConstructHandler(AnchorEdgeSchema.ConstructIdPercentOffset));
            AnchorEdgeSchema.Type.Initialize(new DefaultConstructHandler(AnchorEdgeSchema.Construct), new ConstructorSchema[2]
            {
        (ConstructorSchema) constructorSchema1,
        (ConstructorSchema) constructorSchema2
            }, new PropertySchema[7]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(AnchorEdgeSchema.TryConvertFrom), new SupportsTypeConversionHandler(AnchorEdgeSchema.IsConversionSupported), (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
