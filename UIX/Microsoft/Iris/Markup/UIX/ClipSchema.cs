// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ClipSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ClipSchema
    {
        public static UIXTypeSchema Type;

        private static object GetChildren(object instanceObj) => (object)ViewItemSchema.ListProxy.GetChildren((ViewItem)instanceObj);

        private static object GetOrientation(object instanceObj) => (object)((Clip)instanceObj).Orientation;

        private static void SetOrientation(ref object instanceObj, object valueObj) => ((Clip)instanceObj).Orientation = (Orientation)valueObj;

        private static object GetFadeSize(object instanceObj) => (object)((Clip)instanceObj).FadeSize;

        private static void SetFadeSize(ref object instanceObj, object valueObj) => ((Clip)instanceObj).FadeSize = (float)valueObj;

        private static object GetNearOffset(object instanceObj) => (object)((Clip)instanceObj).NearOffset;

        private static void SetNearOffset(ref object instanceObj, object valueObj) => ((Clip)instanceObj).NearOffset = (float)valueObj;

        private static object GetFarOffset(object instanceObj) => (object)((Clip)instanceObj).FarOffset;

        private static void SetFarOffset(ref object instanceObj, object valueObj) => ((Clip)instanceObj).FarOffset = (float)valueObj;

        private static object GetNearPercent(object instanceObj) => (object)((Clip)instanceObj).NearPercent;

        private static void SetNearPercent(ref object instanceObj, object valueObj) => ((Clip)instanceObj).NearPercent = (float)valueObj;

        private static object GetFarPercent(object instanceObj) => (object)((Clip)instanceObj).FarPercent;

        private static void SetFarPercent(ref object instanceObj, object valueObj) => ((Clip)instanceObj).FarPercent = (float)valueObj;

        private static object GetShowNear(object instanceObj) => BooleanBoxes.Box(((Clip)instanceObj).ShowNear);

        private static void SetShowNear(ref object instanceObj, object valueObj) => ((Clip)instanceObj).ShowNear = (bool)valueObj;

        private static object GetShowFar(object instanceObj) => BooleanBoxes.Box(((Clip)instanceObj).ShowFar);

        private static void SetShowFar(ref object instanceObj, object valueObj) => ((Clip)instanceObj).ShowFar = (bool)valueObj;

        private static object GetColorMask(object instanceObj) => (object)((Clip)instanceObj).ColorMask;

        private static void SetColorMask(ref object instanceObj, object valueObj) => ((Clip)instanceObj).ColorMask = (Color)valueObj;

        private static object GetFadeAmount(object instanceObj) => (object)((Clip)instanceObj).FadeAmount;

        private static void SetFadeAmount(ref object instanceObj, object valueObj)
        {
            Clip clip = (Clip)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.Validate0to1(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                clip.FadeAmount = num;
        }

        private static object Construct() => (object)new Clip();

        public static void Pass1Initialize() => ClipSchema.Type = new UIXTypeSchema((short)34, "Clip", (string)null, (short)239, typeof(Clip), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)34, "Children", (short)138, (short)239, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(ClipSchema.GetChildren), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)34, "Orientation", (short)154, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClipSchema.GetOrientation), new SetValueHandler(ClipSchema.SetOrientation), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)34, "FadeSize", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClipSchema.GetFadeSize), new SetValueHandler(ClipSchema.SetFadeSize), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)34, "NearOffset", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClipSchema.GetNearOffset), new SetValueHandler(ClipSchema.SetNearOffset), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)34, "FarOffset", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClipSchema.GetFarOffset), new SetValueHandler(ClipSchema.SetFarOffset), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)34, "NearPercent", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClipSchema.GetNearPercent), new SetValueHandler(ClipSchema.SetNearPercent), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)34, "FarPercent", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClipSchema.GetFarPercent), new SetValueHandler(ClipSchema.SetFarPercent), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)34, "ShowNear", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClipSchema.GetShowNear), new SetValueHandler(ClipSchema.SetShowNear), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)34, "ShowFar", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClipSchema.GetShowFar), new SetValueHandler(ClipSchema.SetShowFar), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)34, "ColorMask", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ClipSchema.GetColorMask), new SetValueHandler(ClipSchema.SetColorMask), false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema((short)34, "FadeAmount", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, true, new GetValueHandler(ClipSchema.GetFadeAmount), new SetValueHandler(ClipSchema.SetFadeAmount), false);
            ClipSchema.Type.Initialize(new DefaultConstructHandler(ClipSchema.Construct), (ConstructorSchema[])null, new PropertySchema[11]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema10,
        (PropertySchema) uixPropertySchema11,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema8
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
