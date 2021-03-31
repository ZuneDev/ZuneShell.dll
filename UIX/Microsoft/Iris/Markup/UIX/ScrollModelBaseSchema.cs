// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ScrollModelBaseSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ScrollModelBaseSchema
    {
        public static UIXTypeSchema Type;

        private static object GetScrollStep(object instanceObj) => (object)((ScrollModelBase)instanceObj).ScrollStep;

        private static void SetScrollStep(ref object instanceObj, object valueObj)
        {
            ScrollModelBase scrollModelBase = (ScrollModelBase)instanceObj;
            int num = (int)valueObj;
            Result result = Int32Schema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                scrollModelBase.ScrollStep = num;
        }

        private static object GetCanScrollUp(object instanceObj) => BooleanBoxes.Box(((ScrollModelBase)instanceObj).CanScrollUp);

        private static object GetCanScrollDown(object instanceObj) => BooleanBoxes.Box(((ScrollModelBase)instanceObj).CanScrollDown);

        private static object GetCurrentPage(object instanceObj) => (object)((ScrollModelBase)instanceObj).CurrentPage;

        private static object GetTotalPages(object instanceObj) => (object)((ScrollModelBase)instanceObj).TotalPages;

        private static object GetViewNear(object instanceObj) => (object)((ScrollModelBase)instanceObj).ViewNear;

        private static object GetViewFar(object instanceObj) => (object)((ScrollModelBase)instanceObj).ViewFar;

        private static object CallScrollInt32(object instanceObj, object[] parameters)
        {
            ((ScrollModelBase)instanceObj).Scroll((int)parameters[0]);
            return (object)null;
        }

        private static object CallScrollUp(object instanceObj, object[] parameters)
        {
            ((ScrollModelBase)instanceObj).ScrollUp();
            return (object)null;
        }

        private static object CallScrollDown(object instanceObj, object[] parameters)
        {
            ((ScrollModelBase)instanceObj).ScrollDown();
            return (object)null;
        }

        private static object CallPageUp(object instanceObj, object[] parameters)
        {
            ((ScrollModelBase)instanceObj).PageUp();
            return (object)null;
        }

        private static object CallPageDown(object instanceObj, object[] parameters)
        {
            ((ScrollModelBase)instanceObj).PageDown();
            return (object)null;
        }

        private static object CallHome(object instanceObj, object[] parameters)
        {
            ((ScrollModelBase)instanceObj).Home();
            return (object)null;
        }

        private static object CallEnd(object instanceObj, object[] parameters)
        {
            ((ScrollModelBase)instanceObj).End();
            return (object)null;
        }

        private static object CallScrollToPositionSingle(object instanceObj, object[] parameters)
        {
            ((ScrollModelBase)instanceObj).ScrollToPosition((float)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => ScrollModelBaseSchema.Type = new UIXTypeSchema((short)183, "ScrollModelBase", (string)null, (short)153, typeof(ScrollModelBase), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)183, "ScrollStep", (short)115, (short)-1, ExpressionRestriction.None, false, Int32Schema.ValidateNotNegative, true, new GetValueHandler(ScrollModelBaseSchema.GetScrollStep), new SetValueHandler(ScrollModelBaseSchema.SetScrollStep), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)183, "CanScrollUp", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelBaseSchema.GetCanScrollUp), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)183, "CanScrollDown", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelBaseSchema.GetCanScrollDown), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)183, "CurrentPage", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelBaseSchema.GetCurrentPage), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)183, "TotalPages", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelBaseSchema.GetTotalPages), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)183, "ViewNear", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelBaseSchema.GetViewNear), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)183, "ViewFar", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelBaseSchema.GetViewFar), (SetValueHandler)null, false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)183, "Scroll", new short[1]
            {
        (short) 115
            }, (short)240, new InvokeHandler(ScrollModelBaseSchema.CallScrollInt32), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)183, "ScrollUp", (short[])null, (short)240, new InvokeHandler(ScrollModelBaseSchema.CallScrollUp), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)183, "ScrollDown", (short[])null, (short)240, new InvokeHandler(ScrollModelBaseSchema.CallScrollDown), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)183, "PageUp", (short[])null, (short)240, new InvokeHandler(ScrollModelBaseSchema.CallPageUp), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)183, "PageDown", (short[])null, (short)240, new InvokeHandler(ScrollModelBaseSchema.CallPageDown), false);
            UIXMethodSchema uixMethodSchema6 = new UIXMethodSchema((short)183, "Home", (short[])null, (short)240, new InvokeHandler(ScrollModelBaseSchema.CallHome), false);
            UIXMethodSchema uixMethodSchema7 = new UIXMethodSchema((short)183, "End", (short[])null, (short)240, new InvokeHandler(ScrollModelBaseSchema.CallEnd), false);
            UIXMethodSchema uixMethodSchema8 = new UIXMethodSchema((short)183, "ScrollToPosition", new short[1]
            {
        (short) 194
            }, (short)240, new InvokeHandler(ScrollModelBaseSchema.CallScrollToPositionSingle), false);
            ScrollModelBaseSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[7]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema6
            }, new MethodSchema[8]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4,
        (MethodSchema) uixMethodSchema5,
        (MethodSchema) uixMethodSchema6,
        (MethodSchema) uixMethodSchema7,
        (MethodSchema) uixMethodSchema8
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
