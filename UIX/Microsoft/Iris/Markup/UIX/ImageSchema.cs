// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ImageSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ImageSchema
    {
        public static UIXTypeSchema Type;

        private static object GetSource(object instanceObj) => (object)((UIImage)instanceObj).Source;

        private static void SetSource(ref object instanceObj, object valueObj) => ((UIImage)instanceObj).Source = (string)valueObj;

        private static object GetNineGrid(object instanceObj) => (object)((UIImage)instanceObj).NineGrid;

        private static void SetNineGrid(ref object instanceObj, object valueObj) => ((UIImage)instanceObj).NineGrid = (Inset)valueObj;

        private static object GetMaximumSize(object instanceObj) => (object)((UIImage)instanceObj).MaximumSize;

        private static void SetMaximumSize(ref object instanceObj, object valueObj)
        {
            UIImage uiImage = (UIImage)instanceObj;
            Size size = (Size)valueObj;
            Result result = SizeSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                uiImage.MaximumSize = size;
        }

        private static object GetFlippable(object instanceObj) => BooleanBoxes.Box(((UIImage)instanceObj).Flippable);

        private static void SetFlippable(ref object instanceObj, object valueObj) => ((UIImage)instanceObj).Flippable = (bool)valueObj;

        private static object GetAntialiasEdges(object instanceObj) => BooleanBoxes.Box(((UIImage)instanceObj).AntialiasEdges);

        private static void SetAntialiasEdges(ref object instanceObj, object valueObj) => ((UIImage)instanceObj).AntialiasEdges = (bool)valueObj;

        private static object GetStatus(object instanceObj) => (object)((UIImage)instanceObj).Status;

        private static object GetWidth(object instanceObj) => (object)((UIImage)instanceObj).Width;

        private static object GetHeight(object instanceObj) => (object)((UIImage)instanceObj).Height;

        private static object Construct() => (object)new UriImage();

        private static object CallLoad(object instanceObj, object[] parameters)
        {
            ((UIImage)instanceObj).Load();
            return (object)null;
        }

        private static object ConstructSource(object[] parameters)
        {
            object instanceObj = ImageSchema.Construct();
            ImageSchema.SetSource(ref instanceObj, parameters[0]);
            return instanceObj;
        }

        private static Result ConvertFromStringSource(string[] splitString, out object instance)
        {
            instance = ImageSchema.Construct();
            object valueObj;
            Result result = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)StringSchema.Type, (RangeValidator)null, out valueObj);
            if (result.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result.Error);
            ImageSchema.SetSource(ref instance, valueObj);
            return result;
        }

        private static object ConstructSourceNineGrid(object[] parameters)
        {
            object instanceObj = ImageSchema.Construct();
            ImageSchema.SetSource(ref instanceObj, parameters[0]);
            ImageSchema.SetNineGrid(ref instanceObj, parameters[1]);
            return instanceObj;
        }

        private static Result ConvertFromStringSourceNineGrid(
          string[] splitString,
          out object instance)
        {
            instance = ImageSchema.Construct();
            object valueObj1;
            Result result1 = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)StringSchema.Type, (RangeValidator)null, out valueObj1);
            if (result1.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result1.Error);
            ImageSchema.SetSource(ref instance, valueObj1);
            object valueObj2;
            Result result2 = UIXLoadResult.ValidateStringAsValue(splitString[1], (TypeSchema)InsetSchema.Type, (RangeValidator)null, out valueObj2);
            if (result2.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result2.Error);
            ImageSchema.SetNineGrid(ref instance, valueObj2);
            return result2;
        }

        private static object ConstructSourceNineGridMaximumSize(object[] parameters)
        {
            object instanceObj = ImageSchema.Construct();
            ImageSchema.SetSource(ref instanceObj, parameters[0]);
            ImageSchema.SetNineGrid(ref instanceObj, parameters[1]);
            ImageSchema.SetMaximumSize(ref instanceObj, parameters[2]);
            return instanceObj;
        }

        private static Result ConvertFromStringSourceNineGridMaximumSize(
          string[] splitString,
          out object instance)
        {
            instance = ImageSchema.Construct();
            object valueObj1;
            Result result1 = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)StringSchema.Type, (RangeValidator)null, out valueObj1);
            if (result1.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result1.Error);
            ImageSchema.SetSource(ref instance, valueObj1);
            object valueObj2;
            Result result2 = UIXLoadResult.ValidateStringAsValue(splitString[1], (TypeSchema)InsetSchema.Type, (RangeValidator)null, out valueObj2);
            if (result2.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result2.Error);
            ImageSchema.SetNineGrid(ref instance, valueObj2);
            object valueObj3;
            Result result3 = UIXLoadResult.ValidateStringAsValue(splitString[2], (TypeSchema)SizeSchema.Type, SizeSchema.ValidateNotNegative, out valueObj3);
            if (result3.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result3.Error);
            ImageSchema.SetMaximumSize(ref instance, valueObj3);
            return result3;
        }

        private static object ConstructSourceNineGridMaximumSizeFlippable(object[] parameters)
        {
            object instanceObj = ImageSchema.Construct();
            ImageSchema.SetSource(ref instanceObj, parameters[0]);
            ImageSchema.SetNineGrid(ref instanceObj, parameters[1]);
            ImageSchema.SetMaximumSize(ref instanceObj, parameters[2]);
            ImageSchema.SetFlippable(ref instanceObj, parameters[3]);
            return instanceObj;
        }

        private static Result ConvertFromStringSourceNineGridMaximumSizeFlippable(
          string[] splitString,
          out object instance)
        {
            instance = ImageSchema.Construct();
            object valueObj1;
            Result result1 = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)StringSchema.Type, (RangeValidator)null, out valueObj1);
            if (result1.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result1.Error);
            ImageSchema.SetSource(ref instance, valueObj1);
            object valueObj2;
            Result result2 = UIXLoadResult.ValidateStringAsValue(splitString[1], (TypeSchema)InsetSchema.Type, (RangeValidator)null, out valueObj2);
            if (result2.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result2.Error);
            ImageSchema.SetNineGrid(ref instance, valueObj2);
            object valueObj3;
            Result result3 = UIXLoadResult.ValidateStringAsValue(splitString[2], (TypeSchema)SizeSchema.Type, SizeSchema.ValidateNotNegative, out valueObj3);
            if (result3.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result3.Error);
            ImageSchema.SetMaximumSize(ref instance, valueObj3);
            object valueObj4;
            Result result4 = UIXLoadResult.ValidateStringAsValue(splitString[3], (TypeSchema)BooleanSchema.Type, (RangeValidator)null, out valueObj4);
            if (result4.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result4.Error);
            ImageSchema.SetFlippable(ref instance, valueObj4);
            return result4;
        }

        private static object ConstructSourceNineGridMaximumSizeFlippableAntialiasEdges(
          object[] parameters)
        {
            object instanceObj = ImageSchema.Construct();
            ImageSchema.SetSource(ref instanceObj, parameters[0]);
            ImageSchema.SetNineGrid(ref instanceObj, parameters[1]);
            ImageSchema.SetMaximumSize(ref instanceObj, parameters[2]);
            ImageSchema.SetFlippable(ref instanceObj, parameters[3]);
            ImageSchema.SetAntialiasEdges(ref instanceObj, parameters[4]);
            return instanceObj;
        }

        private static Result ConvertFromStringSourceNineGridMaximumSizeFlippableAntialiasEdges(
          string[] splitString,
          out object instance)
        {
            instance = ImageSchema.Construct();
            object valueObj1;
            Result result1 = UIXLoadResult.ValidateStringAsValue(splitString[0], (TypeSchema)StringSchema.Type, (RangeValidator)null, out valueObj1);
            if (result1.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result1.Error);
            ImageSchema.SetSource(ref instance, valueObj1);
            object valueObj2;
            Result result2 = UIXLoadResult.ValidateStringAsValue(splitString[1], (TypeSchema)InsetSchema.Type, (RangeValidator)null, out valueObj2);
            if (result2.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result2.Error);
            ImageSchema.SetNineGrid(ref instance, valueObj2);
            object valueObj3;
            Result result3 = UIXLoadResult.ValidateStringAsValue(splitString[2], (TypeSchema)SizeSchema.Type, SizeSchema.ValidateNotNegative, out valueObj3);
            if (result3.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result3.Error);
            ImageSchema.SetMaximumSize(ref instance, valueObj3);
            object valueObj4;
            Result result4 = UIXLoadResult.ValidateStringAsValue(splitString[3], (TypeSchema)BooleanSchema.Type, (RangeValidator)null, out valueObj4);
            if (result4.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result4.Error);
            ImageSchema.SetFlippable(ref instance, valueObj4);
            object valueObj5;
            Result result5 = UIXLoadResult.ValidateStringAsValue(splitString[4], (TypeSchema)BooleanSchema.Type, (RangeValidator)null, out valueObj5);
            if (result5.Failed)
                return Result.Fail("Problem converting '{0}' ({1})", (object)"Image", (object)result5.Error);
            ImageSchema.SetAntialiasEdges(ref instance, valueObj5);
            return result5;
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
                    case 1:
                        result = ImageSchema.ConvertFromStringSource(splitString, out instance);
                        if (!result.Failed)
                            return result;
                        break;
                    case 2:
                        result = ImageSchema.ConvertFromStringSourceNineGrid(splitString, out instance);
                        if (!result.Failed)
                            return result;
                        break;
                    case 3:
                        result = ImageSchema.ConvertFromStringSourceNineGridMaximumSize(splitString, out instance);
                        if (!result.Failed)
                            return result;
                        break;
                    case 4:
                        result = ImageSchema.ConvertFromStringSourceNineGridMaximumSizeFlippable(splitString, out instance);
                        if (!result.Failed)
                            return result;
                        break;
                    case 5:
                        result = ImageSchema.ConvertFromStringSourceNineGridMaximumSizeFlippableAntialiasEdges(splitString, out instance);
                        if (!result.Failed)
                            return result;
                        break;
                    default:
                        result = Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)from.ToString(), (object)"Image");
                        break;
                }
            }
            return result;
        }

        public static void Pass1Initialize() => ImageSchema.Type = new UIXTypeSchema((short)105, "Image", (string)null, (short)153, typeof(UIImage), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)105, "Source", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ImageSchema.GetSource), new SetValueHandler(ImageSchema.SetSource), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)105, "NineGrid", (short)114, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ImageSchema.GetNineGrid), new SetValueHandler(ImageSchema.SetNineGrid), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)105, "MaximumSize", (short)195, (short)-1, ExpressionRestriction.None, false, SizeSchema.ValidateNotNegative, false, new GetValueHandler(ImageSchema.GetMaximumSize), new SetValueHandler(ImageSchema.SetMaximumSize), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)105, "Flippable", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ImageSchema.GetFlippable), new SetValueHandler(ImageSchema.SetFlippable), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)105, "AntialiasEdges", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ImageSchema.GetAntialiasEdges), new SetValueHandler(ImageSchema.SetAntialiasEdges), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)105, "Status", (short)108, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ImageSchema.GetStatus), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)105, "Width", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ImageSchema.GetWidth), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)105, "Height", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ImageSchema.GetHeight), (SetValueHandler)null, false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)105, "Load", (short[])null, (short)240, new InvokeHandler(ImageSchema.CallLoad), false);
            UIXConstructorSchema constructorSchema1 = new UIXConstructorSchema((short)105, new short[1]
            {
        (short) 208
            }, new ConstructHandler(ImageSchema.ConstructSource));
            UIXConstructorSchema constructorSchema2 = new UIXConstructorSchema((short)105, new short[2]
            {
        (short) 208,
        (short) 114
            }, new ConstructHandler(ImageSchema.ConstructSourceNineGrid));
            UIXConstructorSchema constructorSchema3 = new UIXConstructorSchema((short)105, new short[3]
            {
        (short) 208,
        (short) 114,
        (short) 195
            }, new ConstructHandler(ImageSchema.ConstructSourceNineGridMaximumSize));
            UIXConstructorSchema constructorSchema4 = new UIXConstructorSchema((short)105, new short[4]
            {
        (short) 208,
        (short) 114,
        (short) 195,
        (short) 15
            }, new ConstructHandler(ImageSchema.ConstructSourceNineGridMaximumSizeFlippable));
            UIXConstructorSchema constructorSchema5 = new UIXConstructorSchema((short)105, new short[5]
            {
        (short) 208,
        (short) 114,
        (short) 195,
        (short) 15,
        (short) 15
            }, new ConstructHandler(ImageSchema.ConstructSourceNineGridMaximumSizeFlippableAntialiasEdges));
            ImageSchema.Type.Initialize(new DefaultConstructHandler(ImageSchema.Construct), new ConstructorSchema[5]
            {
        (ConstructorSchema) constructorSchema1,
        (ConstructorSchema) constructorSchema2,
        (ConstructorSchema) constructorSchema3,
        (ConstructorSchema) constructorSchema4,
        (ConstructorSchema) constructorSchema5
            }, new PropertySchema[8]
            {
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema7
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(ImageSchema.TryConvertFrom), new SupportsTypeConversionHandler(ImageSchema.IsConversionSupported), (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
