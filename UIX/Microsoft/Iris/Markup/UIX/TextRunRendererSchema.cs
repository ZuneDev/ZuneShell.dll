// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.TextRunRendererSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.UI;
using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class TextRunRendererSchema
    {
        public static UIXTypeSchema Type;

        private static object GetData(object instanceObj) => (object)((TextRunRenderer)instanceObj).Data;

        private static void SetData(ref object instanceObj, object valueObj) => ((TextRunRenderer)instanceObj).Data = (TextRunData)valueObj;

        private static object GetColor(object instanceObj) => (object)((TextRunRenderer)instanceObj).Color;

        private static void SetColor(ref object instanceObj, object valueObj) => ((TextRunRenderer)instanceObj).Color = (Color)valueObj;

        private static object GetEffect(object instanceObj) => (object)((ViewItem)instanceObj).Effect;

        private static void SetEffect(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Effect = (EffectClass)valueObj;

        private static object Construct() => (object)new TextRunRenderer();

        public static void Pass1Initialize() => TextRunRendererSchema.Type = new UIXTypeSchema((short)217, "TextRunRenderer", (string)null, (short)239, typeof(TextRunRenderer), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)217, "Data", (short)216, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextRunRendererSchema.GetData), new SetValueHandler(TextRunRendererSchema.SetData), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)217, "Color", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextRunRendererSchema.GetColor), new SetValueHandler(TextRunRendererSchema.SetColor), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)217, "Effect", (short)78, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TextRunRendererSchema.GetEffect), new SetValueHandler(TextRunRendererSchema.SetEffect), false);
            TextRunRendererSchema.Type.Initialize(new DefaultConstructHandler(TextRunRendererSchema.Construct), (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema3
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
