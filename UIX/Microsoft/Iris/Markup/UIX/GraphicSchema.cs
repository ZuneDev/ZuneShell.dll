// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.GraphicSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.UI;
using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class GraphicSchema
    {
        public static UIXTypeSchema Type;

        private static object GetChildren(object instanceObj) => (object)ViewItemSchema.ListProxy.GetChildren((ViewItem)instanceObj);

        private static object GetContent(object instanceObj) => (object)((Graphic)instanceObj).Content;

        private static void SetContent(ref object instanceObj, object valueObj) => ((Graphic)instanceObj).Content = (UIImage)valueObj;

        private static object GetPreloadContent(object instanceObj) => (object)((Graphic)instanceObj).PreloadContent;

        private static void SetPreloadContent(ref object instanceObj, object valueObj) => ((Graphic)instanceObj).PreloadContent = (UIImage)valueObj;

        private static object GetEffect(object instanceObj) => (object)((ViewItem)instanceObj).Effect;

        private static void SetEffect(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Effect = (EffectClass)valueObj;

        private static object GetAcquiringImage(object instanceObj) => (object)((Graphic)instanceObj).AcquiringImage;

        private static void SetAcquiringImage(ref object instanceObj, object valueObj) => ((Graphic)instanceObj).AcquiringImage = (UIImage)valueObj;

        private static object GetErrorImage(object instanceObj) => (object)((Graphic)instanceObj).ErrorImage;

        private static void SetErrorImage(ref object instanceObj, object valueObj) => ((Graphic)instanceObj).ErrorImage = (UIImage)valueObj;

        private static object GetSizingPolicy(object instanceObj) => (object)((Graphic)instanceObj).SizingPolicy;

        private static void SetSizingPolicy(ref object instanceObj, object valueObj) => ((Graphic)instanceObj).SizingPolicy = (SizingPolicy)valueObj;

        private static object GetStretchingPolicy(object instanceObj) => (object)((Graphic)instanceObj).StretchingPolicy;

        private static void SetStretchingPolicy(ref object instanceObj, object valueObj) => ((Graphic)instanceObj).StretchingPolicy = (StretchingPolicy)valueObj;

        private static object GetHorizontalAlignment(object instanceObj) => (object)((Graphic)instanceObj).HorizontalAlignment;

        private static void SetHorizontalAlignment(ref object instanceObj, object valueObj) => ((Graphic)instanceObj).HorizontalAlignment = (StripAlignment)valueObj;

        private static object GetVerticalAlignment(object instanceObj) => (object)((Graphic)instanceObj).VerticalAlignment;

        private static void SetVerticalAlignment(ref object instanceObj, object valueObj) => ((Graphic)instanceObj).VerticalAlignment = (StripAlignment)valueObj;

        private static object Construct() => (object)new Graphic();

        private static object CallCommitPreload(object instanceObj, object[] parameters)
        {
            ((Graphic)instanceObj).CommitPreload();
            return (object)null;
        }

        public static void Pass1Initialize() => GraphicSchema.Type = new UIXTypeSchema((short)97, "Graphic", (string)null, (short)239, typeof(Graphic), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)97, "Children", (short)138, (short)239, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(GraphicSchema.GetChildren), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)97, "Content", (short)105, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(GraphicSchema.GetContent), new SetValueHandler(GraphicSchema.SetContent), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)97, "PreloadContent", (short)105, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(GraphicSchema.GetPreloadContent), new SetValueHandler(GraphicSchema.SetPreloadContent), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)97, "Effect", (short)78, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(GraphicSchema.GetEffect), new SetValueHandler(GraphicSchema.SetEffect), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)97, "AcquiringImage", (short)105, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(GraphicSchema.GetAcquiringImage), new SetValueHandler(GraphicSchema.SetAcquiringImage), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)97, "ErrorImage", (short)105, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(GraphicSchema.GetErrorImage), new SetValueHandler(GraphicSchema.SetErrorImage), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)97, "SizingPolicy", (short)199, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(GraphicSchema.GetSizingPolicy), new SetValueHandler(GraphicSchema.SetSizingPolicy), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)97, "StretchingPolicy", (short)207, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(GraphicSchema.GetStretchingPolicy), new SetValueHandler(GraphicSchema.SetStretchingPolicy), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)97, "HorizontalAlignment", (short)209, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(GraphicSchema.GetHorizontalAlignment), new SetValueHandler(GraphicSchema.SetHorizontalAlignment), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)97, "VerticalAlignment", (short)209, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(GraphicSchema.GetVerticalAlignment), new SetValueHandler(GraphicSchema.SetVerticalAlignment), false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)97, "CommitPreload", (short[])null, (short)240, new InvokeHandler(GraphicSchema.CallCommitPreload), false);
            GraphicSchema.Type.Initialize(new DefaultConstructHandler(GraphicSchema.Construct), (ConstructorSchema[])null, new PropertySchema[10]
            {
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema10
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
