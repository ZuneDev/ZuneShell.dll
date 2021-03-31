// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.PopupPositionSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layouts;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class PopupPositionSchema
    {
        public static UIXTypeSchema Type;

        private static object GetTarget(object instanceObj) => (object)((PopupPosition)instanceObj).Target;

        private static void SetTarget(ref object instanceObj, object valueObj)
        {
            PopupPosition popupPosition = (PopupPosition)instanceObj;
            InterestPoint interestPoint = (InterestPoint)valueObj;
            popupPosition.Target = interestPoint;
            instanceObj = (object)popupPosition;
        }

        private static object GetPopup(object instanceObj) => (object)((PopupPosition)instanceObj).Popup;

        private static void SetPopup(ref object instanceObj, object valueObj)
        {
            PopupPosition popupPosition = (PopupPosition)instanceObj;
            InterestPoint interestPoint = (InterestPoint)valueObj;
            popupPosition.Popup = interestPoint;
            instanceObj = (object)popupPosition;
        }

        private static object GetFlipped(object instanceObj) => (object)((PopupPosition)instanceObj).Flipped;

        private static void SetFlipped(ref object instanceObj, object valueObj)
        {
            PopupPosition popupPosition = (PopupPosition)instanceObj;
            FlipDirection flipDirection = (FlipDirection)valueObj;
            popupPosition.Flipped = flipDirection;
            instanceObj = (object)popupPosition;
        }

        private static object Construct() => (object)new PopupPosition();

        public static void Pass1Initialize() => PopupPositionSchema.Type = new UIXTypeSchema((short)163, "PopupPosition", (string)null, (short)153, typeof(PopupPosition), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)163, "Target", (short)118, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PopupPositionSchema.GetTarget), new SetValueHandler(PopupPositionSchema.SetTarget), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)163, "Popup", (short)118, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PopupPositionSchema.GetPopup), new SetValueHandler(PopupPositionSchema.SetPopup), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)163, "Flipped", (short)89, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PopupPositionSchema.GetFlipped), new SetValueHandler(PopupPositionSchema.SetFlipped), false);
            PopupPositionSchema.Type.Initialize(new DefaultConstructHandler(PopupPositionSchema.Construct), (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
