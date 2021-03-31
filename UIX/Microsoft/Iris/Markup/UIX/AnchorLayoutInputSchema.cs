// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.AnchorLayoutInputSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layouts;
using Microsoft.Iris.Library;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class AnchorLayoutInputSchema
    {
        public static UIXTypeSchema Type;

        private static object GetLeft(object instanceObj) => (object)((AnchorLayoutInput)instanceObj).Left;

        private static void SetLeft(ref object instanceObj, object valueObj) => ((AnchorLayoutInput)instanceObj).Left = (AnchorEdge)valueObj;

        private static object GetTop(object instanceObj) => (object)((AnchorLayoutInput)instanceObj).Top;

        private static void SetTop(ref object instanceObj, object valueObj) => ((AnchorLayoutInput)instanceObj).Top = (AnchorEdge)valueObj;

        private static object GetRight(object instanceObj) => (object)((AnchorLayoutInput)instanceObj).Right;

        private static void SetRight(ref object instanceObj, object valueObj) => ((AnchorLayoutInput)instanceObj).Right = (AnchorEdge)valueObj;

        private static object GetBottom(object instanceObj) => (object)((AnchorLayoutInput)instanceObj).Bottom;

        private static void SetBottom(ref object instanceObj, object valueObj) => ((AnchorLayoutInput)instanceObj).Bottom = (AnchorEdge)valueObj;

        private static object GetContributesToWidth(object instanceObj) => BooleanBoxes.Box(((AnchorLayoutInput)instanceObj).ContributesToWidth);

        private static void SetContributesToWidth(ref object instanceObj, object valueObj) => ((AnchorLayoutInput)instanceObj).ContributesToWidth = (bool)valueObj;

        private static object GetContributesToHeight(object instanceObj) => BooleanBoxes.Box(((AnchorLayoutInput)instanceObj).ContributesToHeight);

        private static void SetContributesToHeight(ref object instanceObj, object valueObj) => ((AnchorLayoutInput)instanceObj).ContributesToHeight = (bool)valueObj;

        private static object Construct() => (object)new AnchorLayoutInput();

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string id = (string)valueObj;
            instanceObj = (object)null;
            instanceObj = (object)new AnchorLayoutInput()
            {
                Left = new AnchorEdge(id, 0.0f),
                Top = new AnchorEdge(id, 0.0f),
                Right = new AnchorEdge(id, 1f),
                Bottom = new AnchorEdge(id, 1f)
            };
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
                result = AnchorLayoutInputSchema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            return result;
        }

        private static object CallTryParseStringAnchorLayoutInput(
          object instanceObj,
          object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            AnchorLayoutInput parameter2 = (AnchorLayoutInput)parameters[1];
            object instanceObj1;
            return AnchorLayoutInputSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        public static void Pass1Initialize() => AnchorLayoutInputSchema.Type = new UIXTypeSchema((short)8, "AnchorLayoutInput", (string)null, (short)133, typeof(AnchorLayoutInput), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)8, "Left", (short)6, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorLayoutInputSchema.GetLeft), new SetValueHandler(AnchorLayoutInputSchema.SetLeft), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)8, "Top", (short)6, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorLayoutInputSchema.GetTop), new SetValueHandler(AnchorLayoutInputSchema.SetTop), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)8, "Right", (short)6, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorLayoutInputSchema.GetRight), new SetValueHandler(AnchorLayoutInputSchema.SetRight), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)8, "Bottom", (short)6, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorLayoutInputSchema.GetBottom), new SetValueHandler(AnchorLayoutInputSchema.SetBottom), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)8, "ContributesToWidth", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorLayoutInputSchema.GetContributesToWidth), new SetValueHandler(AnchorLayoutInputSchema.SetContributesToWidth), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)8, "ContributesToHeight", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(AnchorLayoutInputSchema.GetContributesToHeight), new SetValueHandler(AnchorLayoutInputSchema.SetContributesToHeight), false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)8, "TryParse", new short[2]
            {
        (short) 208,
        (short) 8
            }, (short)8, new InvokeHandler(AnchorLayoutInputSchema.CallTryParseStringAnchorLayoutInput), true);
            AnchorLayoutInputSchema.Type.Initialize(new DefaultConstructHandler(AnchorLayoutInputSchema.Construct), (ConstructorSchema[])null, new PropertySchema[6]
            {
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, new TypeConverterHandler(AnchorLayoutInputSchema.TryConvertFrom), new SupportsTypeConversionHandler(AnchorLayoutInputSchema.IsConversionSupported), (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
