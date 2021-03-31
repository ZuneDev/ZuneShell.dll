// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.CaretInfoSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class CaretInfoSchema
    {
        public static UIXTypeSchema Type;

        private static object GetBlinkTime(object instanceObj) => (object)((CaretInfo)instanceObj).BlinkTime;

        private static object GetIdealWidth(object instanceObj) => (object)((CaretInfo)instanceObj).IdealWidth;

        private static void SetIdealWidth(ref object instanceObj, object valueObj) => ((CaretInfo)instanceObj).IdealWidth = (int)valueObj;

        private static object GetVisible(object instanceObj) => BooleanBoxes.Box(((CaretInfo)instanceObj).Visible);

        private static object GetPosition(object instanceObj) => (object)((CaretInfo)instanceObj).Position;

        private static object GetSuggestedSize(object instanceObj) => (object)((CaretInfo)instanceObj).SuggestedSize;

        private static object Construct() => (object)new CaretInfo();

        public static void Pass1Initialize() => CaretInfoSchema.Type = new UIXTypeSchema((short)26, "CaretInfo", (string)null, (short)153, typeof(CaretInfo), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)26, "BlinkTime", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CaretInfoSchema.GetBlinkTime), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)26, "IdealWidth", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CaretInfoSchema.GetIdealWidth), new SetValueHandler(CaretInfoSchema.SetIdealWidth), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)26, "Visible", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CaretInfoSchema.GetVisible), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)26, "Position", (short)158, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CaretInfoSchema.GetPosition), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)26, "SuggestedSize", (short)195, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CaretInfoSchema.GetSuggestedSize), (SetValueHandler)null, false);
            CaretInfoSchema.Type.Initialize(new DefaultConstructHandler(CaretInfoSchema.Construct), (ConstructorSchema[])null, new PropertySchema[5]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema3
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
