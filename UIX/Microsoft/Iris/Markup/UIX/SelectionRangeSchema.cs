// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.SelectionRangeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.ModelItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class SelectionRangeSchema
    {
        public static UIXTypeSchema Type;

        private static object GetBegin(object instanceObj) => (object)((Range)instanceObj).Begin;

        private static void SetBegin(ref object instanceObj, object valueObj)
        {
            Range range = (Range)instanceObj;
            int num = (int)valueObj;
            range.Begin = num;
            instanceObj = (object)range;
        }

        private static object GetEnd(object instanceObj) => (object)((Range)instanceObj).End;

        private static void SetEnd(ref object instanceObj, object valueObj)
        {
            Range range = (Range)instanceObj;
            int num = (int)valueObj;
            range.End = num;
            instanceObj = (object)range;
        }

        private static object GetIsEmpty(object instanceObj) => BooleanBoxes.Box(((Range)instanceObj).IsEmpty);

        private static object Construct() => (object)new Range(0, 0);

        private static object ConstructBeginEnd(object[] parameters)
        {
            object instanceObj = SelectionRangeSchema.Construct();
            SelectionRangeSchema.SetBegin(ref instanceObj, parameters[0]);
            SelectionRangeSchema.SetEnd(ref instanceObj, parameters[1]);
            return instanceObj;
        }

        private static Result ConvertFromStringBeginEnd(string[] splitString, out object instance)
        {
            instance = SelectionRangeSchema.Construct();
            object valueObj1;
            Result result1 = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)Int32Schema.Type, (RangeValidator)null, out valueObj1);
            if (result1.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"SelectionRange", (object)result1.Error);
            SelectionRangeSchema.SetBegin(ref instance, valueObj1);
            object valueObj2;
            Result result2 = UIXLoadResult.ValidateStringAsValue(splitString[1], (TypeSchema)Int32Schema.Type, (RangeValidator)null, out valueObj2);
            if (result2.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"SelectionRange", (object)result2.Error);
            SelectionRangeSchema.SetEnd(ref instance, valueObj2);
            return result2;
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
                if (splitString.Length == 2)
                {
                    result = SelectionRangeSchema.ConvertFromStringBeginEnd(splitString, out instance);
                    if (!result.Failed)
                        return result;
                }
                else
                    result = Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)from.ToString(), (object)"SelectionRange");
            }
            return result;
        }

        public static void Pass1Initialize() => SelectionRangeSchema.Type = new UIXTypeSchema((short)187, "SelectionRange", (string)null, (short)153, typeof(Range), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)187, "Begin", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(SelectionRangeSchema.GetBegin), new SetValueHandler(SelectionRangeSchema.SetBegin), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)187, "End", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(SelectionRangeSchema.GetEnd), new SetValueHandler(SelectionRangeSchema.SetEnd), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)187, "IsEmpty", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(SelectionRangeSchema.GetIsEmpty), (SetValueHandler)null, false);
            UIXConstructorSchema constructorSchema = new UIXConstructorSchema((short)187, new short[2]
            {
        (short) 115,
        (short) 115
            }, new ConstructHandler(SelectionRangeSchema.ConstructBeginEnd));
            SelectionRangeSchema.Type.Initialize(new DefaultConstructHandler(SelectionRangeSchema.Construct), new ConstructorSchema[1]
            {
        (ConstructorSchema) constructorSchema
            }, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(SelectionRangeSchema.TryConvertFrom), new SupportsTypeConversionHandler(SelectionRangeSchema.IsConversionSupported), (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
