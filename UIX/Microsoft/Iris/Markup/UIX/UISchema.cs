// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.UISchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class UISchema
    {
        public static UIXTypeSchema Type;

        private static object GetProperties(object instanceObj) => (object)((UIClass)instanceObj).Storage;

        private static object GetLocals(object instanceObj) => (object)((UIClass)instanceObj).Storage;

        private static object GetInput(object instanceObj) => (object)((UIClass)instanceObj).EnsureInputHandlerStorage();

        private static object GetContent(object instanceObj) => (object)((UIClass)instanceObj).RootItem;

        private static void SetContent(ref object instanceObj, object valueObj) => ((UIClass)instanceObj).SetRootItem((ViewItem)valueObj);

        private static object GetFlippable(object instanceObj) => BooleanBoxes.Box(((UIClass)instanceObj).Flippable);

        private static void SetFlippable(ref object instanceObj, object valueObj) => ((UIClass)instanceObj).Flippable = (bool)valueObj;

        private static void SetBase(ref object instanceObj, object valueObj)
        {
            UIClass uiClass = (UIClass)instanceObj;
        }

        private static object GetScripts(object instanceObj) => (object)null;

        public static void Pass1Initialize() => UISchema.Type = new UIXTypeSchema((short)229, "UI", (string)null, (short)-1, typeof(UIClass), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)229, "Properties", (short)58, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(UISchema.GetProperties), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)229, "Locals", (short)58, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(UISchema.GetLocals), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)229, "Input", (short)138, (short)110, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(UISchema.GetInput), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)229, "Content", (short)239, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(UISchema.GetContent), new SetValueHandler(UISchema.SetContent), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)229, "Flippable", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(UISchema.GetFlippable), new SetValueHandler(UISchema.SetFlippable), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)229, "Base", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(UISchema.SetBase), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)229, "Scripts", (short)138, (short)240, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(UISchema.GetScripts), (SetValueHandler)null, false);
            UISchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[7]
            {
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema7
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
