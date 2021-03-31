// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.MarkupErrorSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.UIX
{
    internal static class MarkupErrorSchema
    {
        public static UIXTypeSchema Type;

        private static object GetContext(object instanceObj) => (object)((MarkupError)instanceObj).Context;

        private static object GetMessage(object instanceObj) => (object)((MarkupError)instanceObj).Message;

        private static object GetUri(object instanceObj) => (object)((MarkupError)instanceObj).Uri;

        private static object GetLine(object instanceObj) => (object)((MarkupError)instanceObj).Line;

        private static object GetColumn(object instanceObj) => (object)((MarkupError)instanceObj).Column;

        private static object GetIsError(object instanceObj) => BooleanBoxes.Box(((MarkupError)instanceObj).IsError);

        public static void Pass1Initialize() => MarkupErrorSchema.Type = new UIXTypeSchema((short)144, "MarkupError", (string)null, (short)153, typeof(MarkupError), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)144, "Context", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(MarkupErrorSchema.GetContext), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)144, "Message", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(MarkupErrorSchema.GetMessage), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)144, "Uri", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(MarkupErrorSchema.GetUri), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)144, "Line", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(MarkupErrorSchema.GetLine), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)144, "Column", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(MarkupErrorSchema.GetColumn), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)144, "IsError", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(MarkupErrorSchema.GetIsError), (SetValueHandler)null, false);
            MarkupErrorSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[6]
            {
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
