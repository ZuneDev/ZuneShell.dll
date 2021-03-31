// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.VideoStreamSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.UIX
{
    internal static class VideoStreamSchema
    {
        public static UIXTypeSchema Type;

        private static object GetStreamID(object instanceObj) => (object)((VideoStream)instanceObj).StreamID;

        private static object Construct() => (object)new VideoStream();

        public static void Pass1Initialize() => VideoStreamSchema.Type = new UIXTypeSchema((short)238, "VideoStream", (string)null, (short)153, typeof(VideoStream), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)238, "StreamID", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(VideoStreamSchema.GetStreamID), (SetValueHandler)null, false);
            VideoStreamSchema.Type.Initialize(new DefaultConstructHandler(VideoStreamSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
