// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EditableTextDataSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EditableTextDataSchema
    {
        public static UIXTypeSchema Type;

        private static object GetValue(object instanceObj) => (object)((EditableTextData)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj) => ((EditableTextData)instanceObj).Value = (string)valueObj;

        private static object GetMaxLength(object instanceObj) => (object)((EditableTextData)instanceObj).MaxLength;

        private static void SetMaxLength(ref object instanceObj, object valueObj)
        {
            EditableTextData editableTextData = (EditableTextData)instanceObj;
            int num = (int)valueObj;
            Result result = Int32Schema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                editableTextData.MaxLength = num;
        }

        private static object GetReadOnly(object instanceObj) => BooleanBoxes.Box(((EditableTextData)instanceObj).ReadOnly);

        private static void SetReadOnly(ref object instanceObj, object valueObj) => ((EditableTextData)instanceObj).ReadOnly = (bool)valueObj;

        private static object Construct() => (object)new EditableTextData();

        private static object CallSubmit(object instanceObj, object[] parameters)
        {
            ((EditableTextData)instanceObj).Submit();
            return (object)null;
        }

        public static void Pass1Initialize() => EditableTextDataSchema.Type = new UIXTypeSchema((short)68, "EditableTextData", (string)null, (short)153, typeof(EditableTextData), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)68, "Value", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(EditableTextDataSchema.GetValue), new SetValueHandler(EditableTextDataSchema.SetValue), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)68, "MaxLength", (short)115, (short)-1, ExpressionRestriction.None, false, Int32Schema.ValidateNotNegative, true, new GetValueHandler(EditableTextDataSchema.GetMaxLength), new SetValueHandler(EditableTextDataSchema.SetMaxLength), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)68, "ReadOnly", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(EditableTextDataSchema.GetReadOnly), new SetValueHandler(EditableTextDataSchema.SetReadOnly), false);
            UIXEventSchema uixEventSchema = new UIXEventSchema((short)68, "Submitted");
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)68, "Submit", (short[])null, (short)240, new InvokeHandler(EditableTextDataSchema.CallSubmit), false);
            EditableTextDataSchema.Type.Initialize(new DefaultConstructHandler(EditableTextDataSchema.Construct), (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, new EventSchema[1] { (EventSchema)uixEventSchema }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
