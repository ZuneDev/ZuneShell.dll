// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.TextFragmentSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class TextFragmentSchema
    {
        public static UIXTypeSchema Type;

        private static object GetRuns(object instanceObj) => (object)((TextFragment)instanceObj).Runs;

        private static object GetTagName(object instanceObj) => (object)((TextFragment)instanceObj).TagName;

        private static object GetContent(object instanceObj) => (object)((TextFragment)instanceObj).Content;

        private static object GetAttributes(object instanceObj) => (object)((TextFragment)instanceObj).Attributes;

        public static void Pass1Initialize() => TextFragmentSchema.Type = new UIXTypeSchema((short)215, "TextFragment", (string)null, (short)153, typeof(TextFragment), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)215, "Runs", (short)138, (short)216, ExpressionRestriction.ReadOnly, false, (RangeValidator)null, false, new GetValueHandler(TextFragmentSchema.GetRuns), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)215, "TagName", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TextFragmentSchema.GetTagName), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)215, "Content", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TextFragmentSchema.GetContent), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)215, "Attributes", (short)58, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TextFragmentSchema.GetAttributes), (SetValueHandler)null, false);
            TextFragmentSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[4]
            {
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
