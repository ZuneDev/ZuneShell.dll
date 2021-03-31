// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.TypeConstraintSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.UIX
{
    internal static class TypeConstraintSchema
    {
        public static UIXTypeSchema Type;

        private static object GetType(object instanceObj) => (object)((TypeConstraint)instanceObj).Type;

        private static void SetType(ref object instanceObj, object valueObj) => ((TypeConstraint)instanceObj).Type = (TypeSchema)valueObj;

        private static object GetConstraint(object instanceObj) => (object)((TypeConstraint)instanceObj).Constraint;

        private static void SetConstraint(ref object instanceObj, object valueObj) => ((TypeConstraint)instanceObj).Constraint = (TypeSchema)valueObj;

        private static object Construct() => (object)new TypeConstraint();

        public static void Pass1Initialize() => TypeConstraintSchema.Type = new UIXTypeSchema((short)226, "TypeConstraint", (string)null, (short)153, typeof(TypeConstraint), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)226, "Type", (short)225, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TypeConstraintSchema.GetType), new SetValueHandler(TypeConstraintSchema.SetType), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)226, "Constraint", (short)225, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(TypeConstraintSchema.GetConstraint), new SetValueHandler(TypeConstraintSchema.SetConstraint), false);
            TypeConstraintSchema.Type.Initialize(new DefaultConstructHandler(TypeConstraintSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
