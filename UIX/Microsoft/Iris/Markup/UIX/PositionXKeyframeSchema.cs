﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.PositionXKeyframeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class PositionXKeyframeSchema
    {
        public static UIXTypeSchema Type;

        private static object GetValue(object instanceObj) => (object)((BaseFloatKeyframe)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj) => ((BaseFloatKeyframe)instanceObj).Value = (float)valueObj;

        private static object Construct() => (object)new PositionXKeyframe();

        public static void Pass1Initialize() => PositionXKeyframeSchema.Type = new UIXTypeSchema((short)165, "PositionXKeyframe", (string)null, (short)130, typeof(PositionXKeyframe), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)165, "Value", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PositionXKeyframeSchema.GetValue), new SetValueHandler(PositionXKeyframeSchema.SetValue), false);
            PositionXKeyframeSchema.Type.Initialize(new DefaultConstructHandler(PositionXKeyframeSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}