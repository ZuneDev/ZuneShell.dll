// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ChoiceSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;
using System.Collections;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ChoiceSchema
    {
        public static UIXTypeSchema Type;

        private static object GetChosenValue(object instanceObj) => ((IUIChoice)instanceObj).ChosenValue;

        private static void SetChosenValue(ref object instanceObj, object valueObj)
        {
            IUIChoice uiChoice = (IUIChoice)instanceObj;
            object obj = valueObj;
            int index;
            string error;
            if (uiChoice.ValidateOption(obj, out index, out error))
                uiChoice.ChosenIndex = index;
            else
                ErrorManager.ReportError(error);
        }

        private static object GetChosenIndex(object instanceObj) => (object)((IUIChoice)instanceObj).ChosenIndex;

        private static void SetChosenIndex(ref object instanceObj, object valueObj)
        {
            IUIChoice uiChoice = (IUIChoice)instanceObj;
            int index = (int)valueObj;
            string error;
            if (uiChoice.ValidateIndex(index, out error))
                uiChoice.ChosenIndex = index;
            else
                ErrorManager.ReportError(error);
        }

        private static object GetDefaultIndex(object instanceObj) => (object)((IUIChoice)instanceObj).DefaultIndex;

        private static void SetDefaultIndex(ref object instanceObj, object valueObj) => ((IUIChoice)instanceObj).DefaultIndex = (int)valueObj;

        private static object GetOptions(object instanceObj) => (object)((IUIChoice)instanceObj).Options;

        private static void SetOptions(ref object instanceObj, object valueObj)
        {
            IUIChoice uiChoice = (IUIChoice)instanceObj;
            IList list = (IList)valueObj;
            string error;
            if (uiChoice.ValidateOptionsList(list, out error))
                uiChoice.Options = list;
            else
                ErrorManager.ReportError(error);
        }

        private static object GetHasSelection(object instanceObj) => BooleanBoxes.Box(((IUIChoice)instanceObj).HasSelection);

        private static object GetWrap(object instanceObj) => BooleanBoxes.Box(((IUIChoice)instanceObj).Wrap);

        private static void SetWrap(ref object instanceObj, object valueObj) => ((IUIChoice)instanceObj).Wrap = (bool)valueObj;

        private static object GetHasPreviousValue(object instanceObj) => BooleanBoxes.Box(((IUIValueRange)instanceObj).HasPreviousValue);

        private static object GetHasNextValue(object instanceObj) => BooleanBoxes.Box(((IUIValueRange)instanceObj).HasNextValue);

        private static object Construct() => (object)new Microsoft.Iris.ModelItems.Choice();

        private static object CallPreviousValue(object instanceObj, object[] parameters)
        {
            ((IUIValueRange)instanceObj).PreviousValue();
            return (object)null;
        }

        private static object CallPreviousValueBoolean(object instanceObj, object[] parameters)
        {
            ((IUIChoice)instanceObj).PreviousValue((bool)parameters[0]);
            return (object)null;
        }

        private static object CallNextValue(object instanceObj, object[] parameters)
        {
            ((IUIValueRange)instanceObj).NextValue();
            return (object)null;
        }

        private static object CallNextValueBoolean(object instanceObj, object[] parameters)
        {
            ((IUIChoice)instanceObj).NextValue((bool)parameters[0]);
            return (object)null;
        }

        private static object CallDefaultValue(object instanceObj, object[] parameters)
        {
            ((IUIChoice)instanceObj).DefaultValue();
            return (object)null;
        }

        private static object CallClear(object instanceObj, object[] parameters)
        {
            ((IUIChoice)instanceObj).Clear();
            return (object)null;
        }

        public static void Pass1Initialize() => ChoiceSchema.Type = new UIXTypeSchema((short)28, "Choice", (string)null, (short)231, typeof(IUIChoice), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)28, "ChosenValue", (short)153, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ChoiceSchema.GetChosenValue), new SetValueHandler(ChoiceSchema.SetChosenValue), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)28, "ChosenIndex", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ChoiceSchema.GetChosenIndex), new SetValueHandler(ChoiceSchema.SetChosenIndex), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)28, "DefaultIndex", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ChoiceSchema.GetDefaultIndex), new SetValueHandler(ChoiceSchema.SetDefaultIndex), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)28, "Options", (short)138, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ChoiceSchema.GetOptions), new SetValueHandler(ChoiceSchema.SetOptions), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)28, "HasSelection", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ChoiceSchema.GetHasSelection), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)28, "Wrap", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ChoiceSchema.GetWrap), new SetValueHandler(ChoiceSchema.SetWrap), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)28, "HasPreviousValue", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ChoiceSchema.GetHasPreviousValue), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)28, "HasNextValue", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ChoiceSchema.GetHasNextValue), (SetValueHandler)null, false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)28, "PreviousValue", (short[])null, (short)240, new InvokeHandler(ChoiceSchema.CallPreviousValue), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)28, "PreviousValue", new short[1]
            {
        (short) 15
            }, (short)240, new InvokeHandler(ChoiceSchema.CallPreviousValueBoolean), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)28, "NextValue", (short[])null, (short)240, new InvokeHandler(ChoiceSchema.CallNextValue), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)28, "NextValue", new short[1]
            {
        (short) 15
            }, (short)240, new InvokeHandler(ChoiceSchema.CallNextValueBoolean), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)28, "DefaultValue", (short[])null, (short)240, new InvokeHandler(ChoiceSchema.CallDefaultValue), false);
            UIXMethodSchema uixMethodSchema6 = new UIXMethodSchema((short)28, "Clear", (short[])null, (short)240, new InvokeHandler(ChoiceSchema.CallClear), false);
            ChoiceSchema.Type.Initialize(new DefaultConstructHandler(ChoiceSchema.Construct), (ConstructorSchema[])null, new PropertySchema[8]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema6
            }, new MethodSchema[6]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4,
        (MethodSchema) uixMethodSchema5,
        (MethodSchema) uixMethodSchema6
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
