// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.MarkupDataQueryInstanceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.UIX
{
    internal static class MarkupDataQueryInstanceSchema
    {
        public static UIXTypeSchema Type;

        private static object GetStatus(object instanceObj) => (object)((MarkupDataQuery)instanceObj).Status;

        private static object GetResult(object instanceObj) => ((MarkupDataQuery)instanceObj).Result;

        private static object GetEnabled(object instanceObj) => BooleanBoxes.Box(((MarkupDataQuery)instanceObj).Enabled);

        private static void SetEnabled(ref object instanceObj, object valueObj) => ((MarkupDataQuery)instanceObj).Enabled = (bool)valueObj;

        private static object CallRefresh(object instanceObj, object[] parameters)
        {
            ((MarkupDataQuery)instanceObj).Refresh();
            return (object)null;
        }

        public static void Pass1Initialize() => MarkupDataQueryInstanceSchema.Type = new UIXTypeSchema((short)142, "MarkupDataQueryInstance", (string)null, (short)153, typeof(MarkupDataQuery), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)142, "Status", (short)47, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(MarkupDataQueryInstanceSchema.GetStatus), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)142, "Result", (short)143, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(MarkupDataQueryInstanceSchema.GetResult), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)142, "Enabled", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(MarkupDataQueryInstanceSchema.GetEnabled), new SetValueHandler(MarkupDataQueryInstanceSchema.SetEnabled), false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)142, "Refresh", (short[])null, (short)240, new InvokeHandler(MarkupDataQueryInstanceSchema.CallRefresh), false);
            MarkupDataQueryInstanceSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
