// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.PopupLayoutInputSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layouts;
using Microsoft.Iris.Render;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class PopupLayoutInputSchema
    {
        public static UIXTypeSchema Type;

        private static object GetPlacementTarget(object instanceObj) => (object)((PopupLayoutInput)instanceObj).PlacementTarget;

        private static void SetPlacementTarget(ref object instanceObj, object valueObj) => ((PopupLayoutInput)instanceObj).PlacementTarget = (ViewItem)valueObj;

        private static object GetPlacement(object instanceObj) => (object)((PopupLayoutInput)instanceObj).Placement;

        private static void SetPlacement(ref object instanceObj, object valueObj) => ((PopupLayoutInput)instanceObj).Placement = (PlacementMode)valueObj;

        private static object GetOffset(object instanceObj) => (object)((PopupLayoutInput)instanceObj).Offset;

        private static void SetOffset(ref object instanceObj, object valueObj) => ((PopupLayoutInput)instanceObj).Offset = (Point)valueObj;

        private static object GetStayInBounds(object instanceObj) => BooleanBoxes.Box(((PopupLayoutInput)instanceObj).StayInBounds);

        private static void SetStayInBounds(ref object instanceObj, object valueObj) => ((PopupLayoutInput)instanceObj).StayInBounds = (bool)valueObj;

        private static object GetRespectMenuDropAlignment(object instanceObj) => BooleanBoxes.Box(((PopupLayoutInput)instanceObj).RespectMenuDropAlignment);

        private static void SetRespectMenuDropAlignment(ref object instanceObj, object valueObj) => ((PopupLayoutInput)instanceObj).RespectMenuDropAlignment = (bool)valueObj;

        private static object GetConstrainToTarget(object instanceObj) => BooleanBoxes.Box(((PopupLayoutInput)instanceObj).ConstrainToTarget);

        private static void SetConstrainToTarget(ref object instanceObj, object valueObj) => ((PopupLayoutInput)instanceObj).ConstrainToTarget = (bool)valueObj;

        private static object GetFlippedHorizontally(object instanceObj) => BooleanBoxes.Box(((PopupLayoutInput)instanceObj).FlippedHorizontally);

        private static object GetFlippedVertically(object instanceObj) => BooleanBoxes.Box(((PopupLayoutInput)instanceObj).FlippedVertically);

        private static object Construct() => (object)new PopupLayoutInput();

        public static void Pass1Initialize() => PopupLayoutInputSchema.Type = new UIXTypeSchema((short)162, "PopupLayoutInput", (string)null, (short)133, typeof(PopupLayoutInput), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)162, "PlacementTarget", (short)239, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PopupLayoutInputSchema.GetPlacementTarget), new SetValueHandler(PopupLayoutInputSchema.SetPlacementTarget), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)162, "Placement", (short)157, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PopupLayoutInputSchema.GetPlacement), new SetValueHandler(PopupLayoutInputSchema.SetPlacement), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)162, "Offset", (short)158, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PopupLayoutInputSchema.GetOffset), new SetValueHandler(PopupLayoutInputSchema.SetOffset), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)162, "StayInBounds", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PopupLayoutInputSchema.GetStayInBounds), new SetValueHandler(PopupLayoutInputSchema.SetStayInBounds), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)162, "RespectMenuDropAlignment", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PopupLayoutInputSchema.GetRespectMenuDropAlignment), new SetValueHandler(PopupLayoutInputSchema.SetRespectMenuDropAlignment), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)162, "ConstrainToTarget", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PopupLayoutInputSchema.GetConstrainToTarget), new SetValueHandler(PopupLayoutInputSchema.SetConstrainToTarget), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)162, "FlippedHorizontally", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(PopupLayoutInputSchema.GetFlippedHorizontally), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)162, "FlippedVertically", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(PopupLayoutInputSchema.GetFlippedVertically), (SetValueHandler)null, false);
            PopupLayoutInputSchema.Type.Initialize(new DefaultConstructHandler(PopupLayoutInputSchema.Construct), (ConstructorSchema[])null, new PropertySchema[8]
            {
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema4
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
