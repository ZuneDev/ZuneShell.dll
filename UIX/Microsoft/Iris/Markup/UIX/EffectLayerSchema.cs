// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EffectLayerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;
using System.Collections;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EffectLayerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetInput(object instanceObj) => (object)((EffectLayer)instanceObj).Input;

        private static void SetInput(ref object instanceObj, object valueObj) => ((EffectLayer)instanceObj).Input = (EffectInput)valueObj;

        private static object GetOperations(object instanceObj) => (object)((EffectLayer)instanceObj).Operations;

        private static void SetOperations(ref object instanceObj, object valueObj) => ((EffectLayer)instanceObj).Operations = (IList)valueObj;

        private static object Construct() => (object)new EffectLayer();

        public static void Pass1Initialize() => EffectLayerSchema.Type = new UIXTypeSchema((short)79, "EffectLayer", (string)null, (short)77, typeof(EffectLayer), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)79, "Input", (short)77, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(EffectLayerSchema.GetInput), new SetValueHandler(EffectLayerSchema.SetInput), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)79, "Operations", (short)138, (short)80, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(EffectLayerSchema.GetOperations), new SetValueHandler(EffectLayerSchema.SetOperations), false);
            EffectLayerSchema.Type.Initialize(new DefaultConstructHandler(EffectLayerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
