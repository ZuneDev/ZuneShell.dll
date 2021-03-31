// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.TypeSelectorSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class TypeSelectorSchema
    {
        public static UIXTypeSchema Type;

        private static object GetType(object instanceObj) => (object)((TypeSelector)instanceObj).Type;

        private static void SetType(ref object instanceObj, object valueObj) => ((TypeSelector)instanceObj).Type = (TypeSchema)valueObj;

        private static object GetContentName(object instanceObj) => (object)((TypeSelector)instanceObj).ContentName;

        private static void SetContentName(ref object instanceObj, object valueObj) => ((TypeSelector)instanceObj).ContentName = (string)valueObj;

        private static object Construct() => (object)new TypeSelector();

        public static void Pass1Initialize() => TypeSelectorSchema.Type = new UIXTypeSchema((short)227, "TypeSelector", (string)null, (short)153, typeof(TypeSelector), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)227, "Type", (short)225, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TypeSelectorSchema.GetType), new SetValueHandler(TypeSelectorSchema.SetType), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)227, "ContentName", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TypeSelectorSchema.GetContentName), new SetValueHandler(TypeSelectorSchema.SetContentName), false);
            TypeSelectorSchema.Type.Initialize(new DefaultConstructHandler(TypeSelectorSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
