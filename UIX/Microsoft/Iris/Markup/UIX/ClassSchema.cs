// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ClassSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ClassSchema
    {
        public static UIXTypeSchema Type;

        private static void SetShared(ref object instanceObj, object valueObj)
        {
            Class @class = (Class)instanceObj;
            int num = (bool)valueObj ? 1 : 0;
        }

        private static void SetBase(ref object instanceObj, object valueObj)
        {
            Class @class = (Class)instanceObj;
        }

        private static object GetProperties(object instanceObj) => (object)((Class)instanceObj).Storage;

        private static object GetLocals(object instanceObj) => (object)((Class)instanceObj).Storage;

        private static object GetScripts(object instanceObj) => (object)null;

        public static void Pass1Initialize() => ClassSchema.Type = new UIXTypeSchema((short)29, "Class", (string)null, (short)-1, typeof(Class), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)29, "Shared", (short)15, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(ClassSchema.SetShared), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)29, "Base", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(ClassSchema.SetBase), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)29, "Properties", (short)58, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(ClassSchema.GetProperties), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)29, "Locals", (short)58, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(ClassSchema.GetLocals), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)29, "Scripts", (short)138, (short)240, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(ClassSchema.GetScripts), (SetValueHandler)null, false);
            ClassSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[5]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
