﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.VideoElementInstanceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.RenderAPI.VideoPlayback;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class VideoElementInstanceSchema
    {
        public static UIXTypeSchema Type;

        private static void SetVideoStream(ref object instanceObj, object valueObj) => ((EffectElementWrapper)instanceObj).SetProperty("Video", (IUIVideoStream)valueObj);

        public static void Pass1Initialize() => VideoElementInstanceSchema.Type = new UIXTypeSchema((short)237, "VideoElementInstance", (string)null, (short)74, typeof(EffectElementWrapper), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)237, "VideoStream", (short)238, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(VideoElementInstanceSchema.SetVideoStream), false);
            VideoElementInstanceSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}