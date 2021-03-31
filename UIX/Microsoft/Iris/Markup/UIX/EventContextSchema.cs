﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EventContextSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.InputHandlers;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EventContextSchema
    {
        public static UIXTypeSchema Type;

        private static object GetValue(object instanceObj) => ((EventContext)instanceObj).Value;

        private static void SetValue(ref object instanceObj, object valueObj) => ((EventContext)instanceObj).Value = valueObj;

        private static object Construct() => (object)new EventContext();

        public static void Pass1Initialize() => EventContextSchema.Type = new UIXTypeSchema((short)88, "EventContext", (string)null, (short)110, typeof(EventContext), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)88, "Value", (short)153, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(EventContextSchema.GetValue), new SetValueHandler(EventContextSchema.SetValue), false);
            EventContextSchema.Type.Initialize(new DefaultConstructHandler(EventContextSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}