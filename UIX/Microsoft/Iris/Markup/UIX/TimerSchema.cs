// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.TimerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class TimerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetInterval(object instanceObj) => (object)((UITimer)instanceObj).Interval;

        private static void SetInterval(ref object instanceObj, object valueObj)
        {
            UITimer uiTimer = (UITimer)instanceObj;
            int num = (int)valueObj;
            Result result = Int32Schema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                uiTimer.Interval = num;
        }

        private static object GetEnabled(object instanceObj) => BooleanBoxes.Box(((UITimer)instanceObj).Enabled);

        private static void SetEnabled(ref object instanceObj, object valueObj) => ((UITimer)instanceObj).Enabled = (bool)valueObj;

        private static object GetAutoRepeat(object instanceObj) => BooleanBoxes.Box(((UITimer)instanceObj).AutoRepeat);

        private static void SetAutoRepeat(ref object instanceObj, object valueObj) => ((UITimer)instanceObj).AutoRepeat = (bool)valueObj;

        private static object Construct() => (object)new UITimer();

        private static object CallStart(object instanceObj, object[] parameters)
        {
            ((UITimer)instanceObj).Start();
            return (object)null;
        }

        private static object CallStop(object instanceObj, object[] parameters)
        {
            ((UITimer)instanceObj).Stop();
            return (object)null;
        }

        public static void Pass1Initialize() => TimerSchema.Type = new UIXTypeSchema((short)221, "Timer", (string)null, (short)153, typeof(UITimer), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)221, "Interval", (short)115, (short)-1, ExpressionRestriction.None, false, Int32Schema.ValidateNotNegative, true, new GetValueHandler(TimerSchema.GetInterval), new SetValueHandler(TimerSchema.SetInterval), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)221, "Enabled", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TimerSchema.GetEnabled), new SetValueHandler(TimerSchema.SetEnabled), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)221, "AutoRepeat", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(TimerSchema.GetAutoRepeat), new SetValueHandler(TimerSchema.SetAutoRepeat), false);
            UIXEventSchema uixEventSchema = new UIXEventSchema((short)221, "Tick");
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)221, "Start", (short[])null, (short)240, new InvokeHandler(TimerSchema.CallStart), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)221, "Stop", (short[])null, (short)240, new InvokeHandler(TimerSchema.CallStop), false);
            TimerSchema.Type.Initialize(new DefaultConstructHandler(TimerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, new MethodSchema[2]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2
            }, new EventSchema[1] { (EventSchema)uixEventSchema }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
