// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ColorElementSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Render;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ColorElementSchema
    {
        public static UIXTypeSchema Type;

        private static void SetColor(ref object instanceObj, object valueObj) => ((ColorElement)instanceObj).Color = ((Color)valueObj).RenderConvert();

        private static object Construct() => (object)new ColorElement();

        public static void Pass1Initialize() => ColorElementSchema.Type = new UIXTypeSchema((short)36, "ColorElement", (string)null, (short)77, typeof(ColorElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)36, "Color", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(ColorElementSchema.SetColor), false);
            ColorElementSchema.Type.Initialize(new DefaultConstructHandler(ColorElementSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
