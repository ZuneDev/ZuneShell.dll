// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.LayoutInputSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class LayoutInputSchema
    {
        public static UIXTypeSchema Type;

        public static void Pass1Initialize() => LayoutInputSchema.Type = new UIXTypeSchema((short)133, "LayoutInput", (string)null, (short)153, typeof(ILayoutInput), UIXTypeFlags.None);

        public static void Pass2Initialize() => LayoutInputSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, (PropertySchema[])null, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
    }
}
