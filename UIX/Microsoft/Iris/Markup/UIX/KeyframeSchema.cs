﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.KeyframeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class KeyframeSchema
    {
        public static UIXTypeSchema Type;

        private static object GetTime(object instanceObj) => (object)((BaseKeyframe)instanceObj).Time;

        private static void SetTime(ref object instanceObj, object valueObj) => ((BaseKeyframe)instanceObj).Time = (float)valueObj;

        private static object GetRelativeTo(object instanceObj) => (object)((BaseKeyframe)instanceObj).RelativeTo;

        private static void SetRelativeTo(ref object instanceObj, object valueObj) => ((BaseKeyframe)instanceObj).RelativeTo = (RelativeTo)valueObj;

        private static object GetInterpolation(object instanceObj) => (object)((BaseKeyframe)instanceObj).Interpolation;

        private static void SetInterpolation(ref object instanceObj, object valueObj) => ((BaseKeyframe)instanceObj).Interpolation = (Interpolation)valueObj;

        public static void Pass1Initialize() => KeyframeSchema.Type = new UIXTypeSchema((short)130, "Keyframe", (string)null, (short)153, typeof(BaseKeyframe), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)130, "Time", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(KeyframeSchema.GetTime), new SetValueHandler(KeyframeSchema.SetTime), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)130, "RelativeTo", (short)171, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(KeyframeSchema.GetRelativeTo), new SetValueHandler(KeyframeSchema.SetRelativeTo), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)130, "Interpolation", (short)121, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(KeyframeSchema.GetInterpolation), new SetValueHandler(KeyframeSchema.SetInterpolation), false);
            KeyframeSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}