// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.PopupLayoutSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layouts;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class PopupLayoutSchema
    {
        public static UIXTypeSchema Type;

        private static object Construct() => (object)new PopupLayout();

        public static void Pass1Initialize() => PopupLayoutSchema.Type = new UIXTypeSchema((short)161, "PopupLayout", (string)null, (short)132, typeof(PopupLayout), UIXTypeFlags.Immutable);

        public static void Pass2Initialize() => PopupLayoutSchema.Type.Initialize(new DefaultConstructHandler(PopupLayoutSchema.Construct), (ConstructorSchema[])null, (PropertySchema[])null, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
    }
}
