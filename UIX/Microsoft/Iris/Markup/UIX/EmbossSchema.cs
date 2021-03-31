// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EmbossSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EmbossSchema
    {
        public static UIXTypeSchema Type;

        private static object GetDirection(object instanceObj) => (object)((EmbossElement)instanceObj).Direction;

        private static void SetDirection(ref object instanceObj, object valueObj) => ((EmbossElement)instanceObj).Direction = (EmbossDirection)valueObj;

        private static object Construct() => (object)new EmbossElement();

        public static void Pass1Initialize() => EmbossSchema.Type = new UIXTypeSchema((short)83, "Emboss", (string)null, (short)80, typeof(EmbossElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)83, "Direction", (short)84, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(EmbossSchema.GetDirection), new SetValueHandler(EmbossSchema.SetDirection), false);
            EmbossSchema.Type.Initialize(new DefaultConstructHandler(EmbossSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
