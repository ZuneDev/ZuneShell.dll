﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ScaleXKeyframeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ScaleXKeyframeSchema
    {
        public static UIXTypeSchema Type;

        private static object GetValue(object instanceObj) => (object)((BaseFloatKeyframe)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj) => ((BaseFloatKeyframe)instanceObj).Value = (float)valueObj;

        private static object Construct() => (object)new ScaleXKeyframe();

        public static void Pass1Initialize() => ScaleXKeyframeSchema.Type = new UIXTypeSchema((short)180, "ScaleXKeyframe", (string)null, (short)130, typeof(ScaleXKeyframe), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)180, "Value", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ScaleXKeyframeSchema.GetValue), new SetValueHandler(ScaleXKeyframeSchema.SetValue), false);
            ScaleXKeyframeSchema.Type.Initialize(new DefaultConstructHandler(ScaleXKeyframeSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}