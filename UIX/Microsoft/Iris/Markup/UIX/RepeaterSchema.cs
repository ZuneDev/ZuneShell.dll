// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.RepeaterSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.UI;
using Microsoft.Iris.ViewItems;
using System.Collections;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class RepeaterSchema
    {
        public static UIXTypeSchema Type;

        private static object GetContentName(object instanceObj) => (object)((Repeater)instanceObj).ContentName;

        private static void SetContentName(ref object instanceObj, object valueObj) => ((Repeater)instanceObj).ContentName = (string)valueObj;

        private static object GetDividerName(object instanceObj) => (object)((Repeater)instanceObj).DividerName;

        private static void SetDividerName(ref object instanceObj, object valueObj) => ((Repeater)instanceObj).DividerName = (string)valueObj;

        private static object GetSource(object instanceObj) => (object)((Repeater)instanceObj).Source;

        private static void SetSource(ref object instanceObj, object valueObj) => ((Repeater)instanceObj).Source = (IList)valueObj;

        private static object GetDefaultFocusIndex(object instanceObj) => (object)((Repeater)instanceObj).DefaultFocusIndex;

        private static void SetDefaultFocusIndex(ref object instanceObj, object valueObj) => ((Repeater)instanceObj).DefaultFocusIndex = (int)valueObj;

        private static void SetContent(ref object instanceObj, object valueObj)
        {
            Repeater repeater = (Repeater)instanceObj;
        }

        private static void SetDivider(ref object instanceObj, object valueObj)
        {
            Repeater repeater = (Repeater)instanceObj;
        }

        private static object GetDiscardOffscreenVisuals(object instanceObj) => BooleanBoxes.Box(((ViewItem)instanceObj).DiscardOffscreenVisuals);

        private static void SetDiscardOffscreenVisuals(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).DiscardOffscreenVisuals = (bool)valueObj;

        private static object GetContentSelectors(object instanceObj) => (object)((Repeater)instanceObj).ContentSelectors;

        private static object GetMaintainFocusedItemOnSourceChanges(object instanceObj) => BooleanBoxes.Box(((Repeater)instanceObj).MaintainFocusedItemOnSourceChanges);

        private static void SetMaintainFocusedItemOnSourceChanges(
          ref object instanceObj,
          object valueObj)
        {
            ((Repeater)instanceObj).MaintainFocusedItemOnSourceChanges = (bool)valueObj;
        }

        private static object Construct() => (object)new Repeater();

        private static object CallNavigateIntoIndexInt32(object instanceObj, object[] parameters)
        {
            ((Repeater)instanceObj).NavigateIntoIndex((int)parameters[0]);
            return (object)null;
        }

        private static object CallScrollIndexIntoViewInt32(object instanceObj, object[] parameters)
        {
            ((Repeater)instanceObj).ScrollIndexIntoView((int)parameters[0]);
            return (object)null;
        }

        public static void Pass1Initialize() => RepeaterSchema.Type = new UIXTypeSchema((short)173, "Repeater", (string)null, (short)239, typeof(Repeater), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)173, "ContentName", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(RepeaterSchema.GetContentName), new SetValueHandler(RepeaterSchema.SetContentName), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)173, "DividerName", (short)208, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(RepeaterSchema.GetDividerName), new SetValueHandler(RepeaterSchema.SetDividerName), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)173, "Source", (short)138, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(RepeaterSchema.GetSource), new SetValueHandler(RepeaterSchema.SetSource), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)173, "DefaultFocusIndex", (short)115, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(RepeaterSchema.GetDefaultFocusIndex), new SetValueHandler(RepeaterSchema.SetDefaultFocusIndex), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)173, "Content", (short)239, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(RepeaterSchema.SetContent), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)173, "Divider", (short)239, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(RepeaterSchema.SetDivider), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)173, "DiscardOffscreenVisuals", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(RepeaterSchema.GetDiscardOffscreenVisuals), new SetValueHandler(RepeaterSchema.SetDiscardOffscreenVisuals), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)173, "ContentSelectors", (short)138, (short)227, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(RepeaterSchema.GetContentSelectors), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)173, "MaintainFocusedItemOnSourceChanges", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(RepeaterSchema.GetMaintainFocusedItemOnSourceChanges), new SetValueHandler(RepeaterSchema.SetMaintainFocusedItemOnSourceChanges), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)173, "NavigateIntoIndex", new short[1]
            {
        (short) 115
            }, (short)240, new InvokeHandler(RepeaterSchema.CallNavigateIntoIndexInt32), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)173, "ScrollIndexIntoView", new short[1]
            {
        (short) 115
            }, (short)240, new InvokeHandler(RepeaterSchema.CallScrollIndexIntoViewInt32), false);
            UIXEventSchema uixEventSchema = new UIXEventSchema((short)173, "FocusedItemDiscarded");
            RepeaterSchema.Type.Initialize(new DefaultConstructHandler(RepeaterSchema.Construct), (ConstructorSchema[])null, new PropertySchema[9]
            {
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema3
            }, new MethodSchema[2]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2
            }, new EventSchema[1] { (EventSchema)uixEventSchema }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
