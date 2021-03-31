// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.GroupSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class GroupSchema
    {
        public static UIXTypeSchema Type;

        private static object GetStartIndex(object instanceObj) => (object)((IUIGroup)instanceObj).StartIndex;

        private static object GetEndIndex(object instanceObj) => (object)((IUIGroup)instanceObj).EndIndex;

        public static void Pass1Initialize() => GroupSchema.Type = new UIXTypeSchema((short)100, "Group", (string)null, (short)138, typeof(IUIGroup), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)100, "StartIndex", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(GroupSchema.GetStartIndex), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)100, "EndIndex", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(GroupSchema.GetEndIndex), (SetValueHandler)null, false);
            GroupSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
