// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.IndexSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class IndexSchema
    {
        public static UIXTypeSchema Type;

        private static object GetValue(object instanceObj) => (object)((Index)instanceObj).Value;

        private static object GetSourceValue(object instanceObj) => (object)((Index)instanceObj).SourceValue;

        private static object CallGetContainerIndex(object instanceObj, object[] parameters) => (object)((Index)instanceObj).GetContainerIndex();

        public static void Pass1Initialize() => IndexSchema.Type = new UIXTypeSchema((short)109, "Index", (string)null, (short)153, typeof(Index), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)109, "Value", (short)115, (short)-1, ExpressionRestriction.ReadOnly, false, (RangeValidator)null, true, new GetValueHandler(IndexSchema.GetValue), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)109, "SourceValue", (short)115, (short)-1, ExpressionRestriction.ReadOnly, false, (RangeValidator)null, true, new GetValueHandler(IndexSchema.GetSourceValue), (SetValueHandler)null, false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)109, "GetContainerIndex", (short[])null, (short)109, new InvokeHandler(IndexSchema.CallGetContainerIndex), false);
            IndexSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
