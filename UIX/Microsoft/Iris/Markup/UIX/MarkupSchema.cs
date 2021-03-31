// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.MarkupSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class MarkupSchema
    {
        public static UIXTypeSchema Type;

        private static object GetErrors(object instanceObj) => (object)((MarkupServices)instanceObj).Errors;

        private static object GetWarningsOnly(object instanceObj) => BooleanBoxes.Box(((MarkupServices)instanceObj).WarningsOnly);

        private static object Construct() => (object)MarkupServices.Instance;

        private static object CallClearErrors(object instanceObj, object[] parameters)
        {
            ((MarkupServices)instanceObj).ClearErrors();
            return (object)null;
        }

        private static object CallIsDisposedObject(object instanceObj, object[] parameters)
        {
            object parameter = parameters[0];
            return parameter == null ? (object)true : BooleanBoxes.Box(parameter is IDisposableObject disposableObject && disposableObject.IsDisposed);
        }

        public static void Pass1Initialize() => MarkupSchema.Type = new UIXTypeSchema((short)141, "Markup", (string)null, (short)153, typeof(MarkupServices), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)141, "Errors", (short)138, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(MarkupSchema.GetErrors), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)141, "WarningsOnly", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(MarkupSchema.GetWarningsOnly), (SetValueHandler)null, false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)141, "ClearErrors", (short[])null, (short)240, new InvokeHandler(MarkupSchema.CallClearErrors), false);
            UIXEventSchema uixEventSchema = new UIXEventSchema((short)141, "ErrorsDetected");
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)141, "IsDisposed", new short[1]
            {
        (short) 153
            }, (short)15, new InvokeHandler(MarkupSchema.CallIsDisposedObject), true);
            MarkupSchema.Type.Initialize(new DefaultConstructHandler(MarkupSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[2]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2
            }, new EventSchema[1] { (EventSchema)uixEventSchema }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
