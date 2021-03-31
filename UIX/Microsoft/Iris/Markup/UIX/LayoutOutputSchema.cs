// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.LayoutOutputSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class LayoutOutputSchema
    {
        public static UIXTypeSchema Type;

        private static object GetSize(object instanceObj) => (object)((LayoutOutput)instanceObj).Size;

        public static void Pass1Initialize() => LayoutOutputSchema.Type = new UIXTypeSchema((short)134, "LayoutOutput", (string)null, (short)153, typeof(LayoutOutput), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)134, "Size", (short)195, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(LayoutOutputSchema.GetSize), (SetValueHandler)null, false);
            LayoutOutputSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
