// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.RelativeToSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Data;
using Microsoft.Iris.Library;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class RelativeToSchema
    {
        public static UIXTypeSchema Type;

        private static object GetSourceId(object instanceObj) => (object)((RelativeTo)instanceObj).SourceId;

        private static void SetSourceId(ref object instanceObj, object valueObj) => ((RelativeTo)instanceObj).SourceId = (int)valueObj;

        private static object GetProperty(object instanceObj) => (object)((RelativeTo)instanceObj).Property;

        private static void SetProperty(ref object instanceObj, object valueObj) => ((RelativeTo)instanceObj).Property = (string)valueObj;

        private static object GetSnapshot(object instanceObj) => (object)((RelativeTo)instanceObj).Snapshot;

        private static void SetSnapshot(ref object instanceObj, object valueObj) => ((RelativeTo)instanceObj).Snapshot = (SnapshotPolicy)valueObj;

        private static object GetPower(object instanceObj) => (object)((RelativeTo)instanceObj).Power;

        private static void SetPower(ref object instanceObj, object valueObj) => ((RelativeTo)instanceObj).Power = (int)valueObj;

        private static object GetMultiply(object instanceObj) => (object)((RelativeTo)instanceObj).Multiply;

        private static void SetMultiply(ref object instanceObj, object valueObj) => ((RelativeTo)instanceObj).Multiply = (float)valueObj;

        private static object GetAdd(object instanceObj) => (object)((RelativeTo)instanceObj).Add;

        private static void SetAdd(ref object instanceObj, object valueObj) => ((RelativeTo)instanceObj).Add = (float)valueObj;

        private static object Construct() => (object)new RelativeTo();

        private static object ConstructSourceIdProperty(object[] parameters)
        {
            object instanceObj = RelativeToSchema.Construct();
            RelativeToSchema.SetSourceId(ref instanceObj, parameters[0]);
            RelativeToSchema.SetProperty(ref instanceObj, parameters[1]);
            return instanceObj;
        }

        private static Result ConvertFromStringSourceIdProperty(
          string[] splitString,
          out object instance)
        {
            instance = RelativeToSchema.Construct();
            object valueObj1;
            Result result1 = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)Int32Schema.Type, (RangeValidator)null, out valueObj1);
            if (result1.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"RelativeTo", (object)result1.Error);
            RelativeToSchema.SetSourceId(ref instance, valueObj1);
            object valueObj2;
            Result result2 = UIXLoadResult.ValidateStringAsValue(splitString[1], (TypeSchema)StringSchema.Type, (RangeValidator)null, out valueObj2);
            if (result2.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"RelativeTo", (object)result2.Error);
            RelativeToSchema.SetProperty(ref instance, valueObj2);
            return result2;
        }

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string str = (string)valueObj;
            instanceObj = (object)null;
            RelativeTo instance = RelativeToSchema.StringToInstance(str);
            if (instance == null)
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)str, (object)"RelativeTo");
            instanceObj = (object)instance;
            return Result.Success;
        }

        private static object FindCanonicalInstance(string name) => (object)RelativeToSchema.StringToInstance(name);

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
                result = RelativeToSchema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                string[] splitString = StringUtility.SplitAndTrim(',', (string)from);
                if (splitString.Length == 2)
                {
                    result = RelativeToSchema.ConvertFromStringSourceIdProperty(splitString, out instance);
                    if (!result.Failed)
                        return result;
                }
                else
                    result = Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)from.ToString(), (object)"RelativeTo");
            }
            return result;
        }

        private static object CallTryParseStringRelativeTo(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            RelativeTo parameter2 = (RelativeTo)parameters[1];
            object instanceObj1;
            return RelativeToSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        private static RelativeTo StringToInstance(string value)
        {
            if (value == "Absolute")
                return RelativeTo.Absolute;
            if (value == "Current")
                return RelativeTo.Current;
            if (value == "CurrentSnapshotOnLoop")
                return RelativeTo.CurrentSnapshotOnLoop;
            return value == "Final" ? RelativeTo.Final : (RelativeTo)null;
        }

        public static void Pass1Initialize() => RelativeToSchema.Type = new UIXTypeSchema((short)171, "RelativeTo", (string)null, (short)153, typeof(RelativeTo), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)171, "SourceId", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RelativeToSchema.GetSourceId), new SetValueHandler(RelativeToSchema.SetSourceId), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)171, "Property", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RelativeToSchema.GetProperty), new SetValueHandler(RelativeToSchema.SetProperty), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)171, "Snapshot", (short)200, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RelativeToSchema.GetSnapshot), new SetValueHandler(RelativeToSchema.SetSnapshot), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)171, "Power", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RelativeToSchema.GetPower), new SetValueHandler(RelativeToSchema.SetPower), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)171, "Multiply", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RelativeToSchema.GetMultiply), new SetValueHandler(RelativeToSchema.SetMultiply), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)171, "Add", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(RelativeToSchema.GetAdd), new SetValueHandler(RelativeToSchema.SetAdd), false);
            UIXConstructorSchema constructorSchema = new UIXConstructorSchema((short)171, new short[2]
            {
        (short) 115,
        (short) 208
            }, new ConstructHandler(RelativeToSchema.ConstructSourceIdProperty));
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)171, "TryParse", new short[2]
            {
        (short) 208,
        (short) 171
            }, (short)171, new InvokeHandler(RelativeToSchema.CallTryParseStringRelativeTo), true);
            RelativeToSchema.Type.Initialize(new DefaultConstructHandler(RelativeToSchema.Construct), new ConstructorSchema[1]
            {
        (ConstructorSchema) constructorSchema
            }, new PropertySchema[6]
            {
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, new FindCanonicalInstanceHandler(RelativeToSchema.FindCanonicalInstance), new TypeConverterHandler(RelativeToSchema.TryConvertFrom), new SupportsTypeConversionHandler(RelativeToSchema.IsConversionSupported), (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
