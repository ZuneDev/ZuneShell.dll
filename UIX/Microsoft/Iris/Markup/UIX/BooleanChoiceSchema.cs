// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.BooleanChoiceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class BooleanChoiceSchema
    {
        public static UIXTypeSchema Type;

        private static object GetValue(object instanceObj) => BooleanBoxes.Box(((IUIChoice)instanceObj).ChosenIndex == 1);

        private static void SetValue(ref object instanceObj, object valueObj)
        {
            IUIBooleanChoice uiBooleanChoice = (IUIBooleanChoice)instanceObj;
            if ((bool)valueObj)
                uiBooleanChoice.ChosenIndex = 1;
            else
                uiBooleanChoice.ChosenIndex = 0;
        }

        private static object Construct() => (object)new Microsoft.Iris.ModelItems.BooleanChoice();

        public static void Pass1Initialize() => BooleanChoiceSchema.Type = new UIXTypeSchema((short)16, "BooleanChoice", (string)null, (short)28, typeof(IUIBooleanChoice), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)16, "Value", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(BooleanChoiceSchema.GetValue), new SetValueHandler(BooleanChoiceSchema.SetValue), false);
            BooleanChoiceSchema.Type.Initialize(new DefaultConstructHandler(BooleanChoiceSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
