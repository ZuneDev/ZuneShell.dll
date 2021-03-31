// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DefaultLayoutSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layouts;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DefaultLayoutSchema
    {
        public static UIXTypeSchema Type;

        private static object Construct() => (object)DefaultLayout.Instance;

        public static void Pass1Initialize() => DefaultLayoutSchema.Type = new UIXTypeSchema((short)53, "DefaultLayout", (string)null, (short)132, typeof(DefaultLayout), UIXTypeFlags.Immutable);

        public static void Pass2Initialize() => DefaultLayoutSchema.Type.Initialize(new DefaultConstructHandler(DefaultLayoutSchema.Construct), (ConstructorSchema[])null, (PropertySchema[])null, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
    }
}
