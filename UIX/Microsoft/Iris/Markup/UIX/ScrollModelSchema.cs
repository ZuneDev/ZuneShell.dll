// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ScrollModelSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layouts;
using Microsoft.Iris.Library;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ScrollModelSchema
    {
        public static UIXTypeSchema Type;

        private static object GetEnabled(object instanceObj) => BooleanBoxes.Box(((ScrollModel)instanceObj).Enabled);

        private static void SetEnabled(ref object instanceObj, object valueObj) => ((ScrollModel)instanceObj).Enabled = (bool)valueObj;

        private static object GetPageStep(object instanceObj) => (object)((ScrollModel)instanceObj).PageStep;

        private static void SetPageStep(ref object instanceObj, object valueObj)
        {
            ScrollModel scrollModel = (ScrollModel)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                scrollModel.PageStep = num;
        }

        private static object GetPageSizedScrollStep(object instanceObj) => BooleanBoxes.Box(((ScrollModel)instanceObj).PageSizedScrollStep);

        private static void SetPageSizedScrollStep(ref object instanceObj, object valueObj) => ((ScrollModel)instanceObj).PageSizedScrollStep = (bool)valueObj;

        private static object GetBeginPadding(object instanceObj) => (object)((ScrollModel)instanceObj).BeginPadding;

        private static void SetBeginPadding(ref object instanceObj, object valueObj) => ((ScrollModel)instanceObj).BeginPadding = (int)valueObj;

        private static object GetEndPadding(object instanceObj) => (object)((ScrollModel)instanceObj).EndPadding;

        private static void SetEndPadding(ref object instanceObj, object valueObj) => ((ScrollModel)instanceObj).EndPadding = (int)valueObj;

        private static object GetBeginPaddingRelativeTo(object instanceObj) => (object)((ScrollModel)instanceObj).BeginPaddingRelativeTo;

        private static void SetBeginPaddingRelativeTo(ref object instanceObj, object valueObj) => ((ScrollModel)instanceObj).BeginPaddingRelativeTo = (RelativeEdge)valueObj;

        private static object GetEndPaddingRelativeTo(object instanceObj) => (object)((ScrollModel)instanceObj).EndPaddingRelativeTo;

        private static void SetEndPaddingRelativeTo(ref object instanceObj, object valueObj) => ((ScrollModel)instanceObj).EndPaddingRelativeTo = (RelativeEdge)valueObj;

        private static object GetLocked(object instanceObj) => BooleanBoxes.Box(((ScrollModel)instanceObj).Locked);

        private static void SetLocked(ref object instanceObj, object valueObj) => ((ScrollModel)instanceObj).Locked = (bool)valueObj;

        private static object GetLockedPosition(object instanceObj) => (object)((ScrollModel)instanceObj).LockedPosition;

        private static void SetLockedPosition(ref object instanceObj, object valueObj) => ((ScrollModel)instanceObj).LockedPosition = (float)valueObj;

        private static object GetLockedAlignment(object instanceObj) => (object)((ScrollModel)instanceObj).LockedAlignment;

        private static void SetLockedAlignment(ref object instanceObj, object valueObj) => ((ScrollModel)instanceObj).LockedAlignment = (float)valueObj;

        private static object GetContentPositioningBehavior(object instanceObj) => (object)((ScrollModel)instanceObj).ContentPositioningBehavior;

        private static void SetContentPositioningBehavior(ref object instanceObj, object valueObj) => ((ScrollModel)instanceObj).ContentPositioningBehavior = (ContentPositioningPolicy)valueObj;

        private static object Construct() => (object)new ScrollModel();

        private static object CallScrollFocusIntoView(object instanceObj, object[] parameters)
        {
            ((ScrollModel)instanceObj).ScrollFocusIntoView();
            return (object)null;
        }

        public static void Pass1Initialize() => ScrollModelSchema.Type = new UIXTypeSchema((short)182, "ScrollModel", (string)null, (short)183, typeof(ScrollModel), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)182, "Enabled", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelSchema.GetEnabled), new SetValueHandler(ScrollModelSchema.SetEnabled), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)182, "PageStep", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, true, new GetValueHandler(ScrollModelSchema.GetPageStep), new SetValueHandler(ScrollModelSchema.SetPageStep), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)182, "PageSizedScrollStep", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelSchema.GetPageSizedScrollStep), new SetValueHandler(ScrollModelSchema.SetPageSizedScrollStep), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)182, "BeginPadding", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelSchema.GetBeginPadding), new SetValueHandler(ScrollModelSchema.SetBeginPadding), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)182, "EndPadding", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelSchema.GetEndPadding), new SetValueHandler(ScrollModelSchema.SetEndPadding), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)182, "BeginPaddingRelativeTo", (short)170, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelSchema.GetBeginPaddingRelativeTo), new SetValueHandler(ScrollModelSchema.SetBeginPaddingRelativeTo), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)182, "EndPaddingRelativeTo", (short)170, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelSchema.GetEndPaddingRelativeTo), new SetValueHandler(ScrollModelSchema.SetEndPaddingRelativeTo), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)182, "Locked", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelSchema.GetLocked), new SetValueHandler(ScrollModelSchema.SetLocked), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)182, "LockedPosition", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelSchema.GetLockedPosition), new SetValueHandler(ScrollModelSchema.SetLockedPosition), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)182, "LockedAlignment", (short)194, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelSchema.GetLockedAlignment), new SetValueHandler(ScrollModelSchema.SetLockedAlignment), false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema((short)182, "ContentPositioningBehavior", (short)41, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollModelSchema.GetContentPositioningBehavior), new SetValueHandler(ScrollModelSchema.SetContentPositioningBehavior), false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)182, "ScrollFocusIntoView", (short[])null, (short)240, new InvokeHandler(ScrollModelSchema.CallScrollFocusIntoView), false);
            ScrollModelSchema.Type.Initialize(new DefaultConstructHandler(ScrollModelSchema.Construct), (ConstructorSchema[])null, new PropertySchema[11]
            {
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema11,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema10,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
