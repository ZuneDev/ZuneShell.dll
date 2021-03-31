// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.SepiaSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class SepiaSchema
    {
        public static UIXTypeSchema Type;

        private static void SetLightColor(ref object instanceObj, object valueObj) => ((SepiaElement)instanceObj).LightColor = ((Color)valueObj).RenderConvert();

        private static void SetDarkColor(ref object instanceObj, object valueObj) => ((SepiaElement)instanceObj).DarkColor = ((Color)valueObj).RenderConvert();

        private static object GetDesaturate(object instanceObj) => (object)((SepiaElement)instanceObj).Desaturate;

        private static void SetDesaturate(ref object instanceObj, object valueObj)
        {
            SepiaElement sepiaElement = (SepiaElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                sepiaElement.Desaturate = num;
        }

        private static object GetTone(object instanceObj) => (object)((SepiaElement)instanceObj).Tone;

        private static void SetTone(ref object instanceObj, object valueObj)
        {
            SepiaElement sepiaElement = (SepiaElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                sepiaElement.Tone = num;
        }

        private static object Construct() => (object)new SepiaElement();

        public static void Pass1Initialize() => SepiaSchema.Type = new UIXTypeSchema((short)188, "Sepia", (string)null, (short)80, typeof(SepiaElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)188, "LightColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(SepiaSchema.SetLightColor), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)188, "DarkColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(SepiaSchema.SetDarkColor), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)188, "Desaturate", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, new GetValueHandler(SepiaSchema.GetDesaturate), new SetValueHandler(SepiaSchema.SetDesaturate), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)188, "Tone", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, new GetValueHandler(SepiaSchema.GetTone), new SetValueHandler(SepiaSchema.SetTone), false);
            SepiaSchema.Type.Initialize(new DefaultConstructHandler(SepiaSchema.Construct), (ConstructorSchema[])null, new PropertySchema[4]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema4
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
