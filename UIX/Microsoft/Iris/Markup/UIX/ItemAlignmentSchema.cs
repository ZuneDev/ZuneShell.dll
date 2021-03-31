// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ItemAlignmentSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;
using Microsoft.Iris.Library;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ItemAlignmentSchema
    {
        public static UIXTypeSchema Type;

        private static object GetHorizontal(object instanceObj) => (object)((ItemAlignment)instanceObj).Horizontal;

        private static void SetHorizontal(ref object instanceObj, object valueObj)
        {
            ItemAlignment itemAlignment = (ItemAlignment)instanceObj;
            Alignment alignment = (Alignment)valueObj;
            itemAlignment.Horizontal = alignment;
            instanceObj = (object)itemAlignment;
        }

        private static object GetVertical(object instanceObj) => (object)((ItemAlignment)instanceObj).Vertical;

        private static void SetVertical(ref object instanceObj, object valueObj)
        {
            ItemAlignment itemAlignment = (ItemAlignment)instanceObj;
            Alignment alignment = (Alignment)valueObj;
            itemAlignment.Vertical = alignment;
            instanceObj = (object)itemAlignment;
        }

        private static object Construct() => (object)ItemAlignment.Default;

        private static object ConstructAlignment(object[] parameters)
        {
            Alignment parameter = (Alignment)parameters[0];
            return (object)new ItemAlignment(parameter, parameter);
        }

        private static object ConstructAlignmentAlignment(object[] parameters) => (object)new ItemAlignment((Alignment)parameters[0], (Alignment)parameters[1]);

        private static void EncodeBinary(ByteCodeWriter writer, object instanceObj)
        {
            ItemAlignment itemAlignment = (ItemAlignment)instanceObj;
            writer.WriteByte((byte)itemAlignment.Horizontal);
            writer.WriteByte((byte)itemAlignment.Vertical);
        }

        private static object DecodeBinary(ByteCodeReader reader) => (object)new ItemAlignment((Alignment)reader.ReadByte(), (Alignment)reader.ReadByte());

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string str = (string)valueObj;
            instanceObj = (object)null;
            Alignment alignment1;
            Alignment alignment2;
            if (str.IndexOf(',') >= 0)
            {
                string[] strArray = str.Split(',');
                if (strArray.Length != 2)
                    return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)str, (object)"ItemAlignment");
                Result alignment3 = ItemAlignmentSchema.ParseAlignment(strArray[0], out alignment1);
                if (alignment3.Failed)
                    return alignment3;
                alignment3 = ItemAlignmentSchema.ParseAlignment(strArray[1], out alignment2);
                if (alignment3.Failed)
                    return alignment3;
            }
            else
            {
                Result alignment3 = ItemAlignmentSchema.ParseAlignment(str, out alignment1);
                if (alignment3.Failed)
                    return alignment3;
                alignment2 = alignment1;
            }
            ItemAlignment itemAlignment = new ItemAlignment(alignment1, alignment2);
            instanceObj = (object)itemAlignment;
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
                result = ItemAlignmentSchema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            return result;
        }

        private static object CallTryParseStringItemAlignment(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            ItemAlignment parameter2 = (ItemAlignment)parameters[1];
            object instanceObj1;
            return ItemAlignmentSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        private static Result ParseAlignment(string value, out Alignment alignment)
        {
            value = value.Trim();
            alignment = Alignment.Unspecified;
            if (value != "-")
            {
                object instance;
                Result result = UIXLoadResultExports.AlignmentType.TypeConverter((object)value, (TypeSchema)StringSchema.Type, out instance);
                if (result.Failed)
                    return Result.Fail("Problem converting '{0}' ({1})", (object)"ItemAlignment", (object)result.Error);
                alignment = (Alignment)instance;
            }
            return Result.Success;
        }

        public static void Pass1Initialize() => ItemAlignmentSchema.Type = new UIXTypeSchema((short)sbyte.MaxValue, "ItemAlignment", (string)null, (short)153, typeof(ItemAlignment), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)sbyte.MaxValue, "Horizontal", (short)3, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ItemAlignmentSchema.GetHorizontal), new SetValueHandler(ItemAlignmentSchema.SetHorizontal), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)sbyte.MaxValue, "Vertical", (short)3, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ItemAlignmentSchema.GetVertical), new SetValueHandler(ItemAlignmentSchema.SetVertical), false);
            UIXConstructorSchema constructorSchema1 = new UIXConstructorSchema((short)sbyte.MaxValue, new short[1]
            {
        (short) 3
            }, new ConstructHandler(ItemAlignmentSchema.ConstructAlignment));
            UIXConstructorSchema constructorSchema2 = new UIXConstructorSchema((short)sbyte.MaxValue, new short[2]
            {
        (short) 3,
        (short) 3
            }, new ConstructHandler(ItemAlignmentSchema.ConstructAlignmentAlignment));
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)sbyte.MaxValue, "TryParse", new short[2]
            {
        (short) 208,
        (short) sbyte.MaxValue
            }, (short)sbyte.MaxValue, new InvokeHandler(ItemAlignmentSchema.CallTryParseStringItemAlignment), true);
            ItemAlignmentSchema.Type.Initialize(new DefaultConstructHandler(ItemAlignmentSchema.Construct), new ConstructorSchema[2]
            {
        (ConstructorSchema) constructorSchema1,
        (ConstructorSchema) constructorSchema2
            }, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(ItemAlignmentSchema.TryConvertFrom), new SupportsTypeConversionHandler(ItemAlignmentSchema.IsConversionSupported), new EncodeBinaryHandler(ItemAlignmentSchema.EncodeBinary), new DecodeBinaryHandler(ItemAlignmentSchema.DecodeBinary), (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
