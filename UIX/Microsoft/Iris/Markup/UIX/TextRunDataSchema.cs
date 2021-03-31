// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.TextRunDataSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class TextRunDataSchema
    {
        public static UIXTypeSchema Type;

        private static object GetPosition(object instanceObj) => (object)((TextRunData)instanceObj).Position;

        private static object GetSize(object instanceObj) => (object)((TextRunData)instanceObj).Size;

        private static object GetColor(object instanceObj) => (object)((TextRunData)instanceObj).Color;

        private static object GetLineNumber(object instanceObj) => (object)((TextRunData)instanceObj).LineNumber;

        public static void Pass1Initialize() => TextRunDataSchema.Type = new UIXTypeSchema((short)216, "TextRunData", (string)null, (short)153, typeof(TextRunData), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)216, "Position", (short)158, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TextRunDataSchema.GetPosition), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)216, "Size", (short)195, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TextRunDataSchema.GetSize), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)216, "Color", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TextRunDataSchema.GetColor), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)216, "LineNumber", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TextRunDataSchema.GetLineNumber), (SetValueHandler)null, false);
            TextRunDataSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[4]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
