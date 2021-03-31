// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.VideoSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.RenderAPI.VideoPlayback;
using Microsoft.Iris.UI;
using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class VideoSchema
    {
        public static UIXTypeSchema Type;

        private static object GetChildren(object instanceObj) => (object)ViewItemSchema.ListProxy.GetChildren((ViewItem)instanceObj);

        private static object GetVideoStream(object instanceObj) => (object)((Video)instanceObj).VideoStream;

        private static void SetVideoStream(ref object instanceObj, object valueObj) => ((Video)instanceObj).VideoStream = (IUIVideoStream)valueObj;

        private static object GetLetterboxColor(object instanceObj) => (object)((Video)instanceObj).LetterboxColor;

        private static void SetLetterboxColor(ref object instanceObj, object valueObj) => ((Video)instanceObj).LetterboxColor = (Color)valueObj;

        private static object Construct() => (object)new Video();

        public static void Pass1Initialize() => VideoSchema.Type = new UIXTypeSchema((short)235, "Video", (string)null, (short)239, typeof(Video), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)235, "Children", (short)138, (short)239, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(VideoSchema.GetChildren), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)235, "VideoStream", (short)238, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(VideoSchema.GetVideoStream), new SetValueHandler(VideoSchema.SetVideoStream), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)235, "LetterboxColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(VideoSchema.GetLetterboxColor), new SetValueHandler(VideoSchema.SetLetterboxColor), false);
            VideoSchema.Type.Initialize(new DefaultConstructHandler(VideoSchema.Construct), (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
