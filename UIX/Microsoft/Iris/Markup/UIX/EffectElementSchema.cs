// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EffectElementSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EffectElementSchema
    {
        public static UIXTypeSchema Type;

        private static object GetName(object instanceObj) => (object)((EffectElement)instanceObj).Name;

        private static void SetName(ref object instanceObj, object valueObj) => ((EffectElement)instanceObj).Name = (string)valueObj;

        public static void Pass1Initialize() => EffectElementSchema.Type = new UIXTypeSchema((short)73, "EffectElement", (string)null, (short)-1, typeof(EffectElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)73, "Name", (short)208, (short)-1, ExpressionRestriction.ReadOnly, false, (RangeValidator)null, false, new GetValueHandler(EffectElementSchema.GetName), new SetValueHandler(EffectElementSchema.SetName), false);
            EffectElementSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
