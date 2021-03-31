// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ScrollerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;
using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ScrollerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetScrollModel(object instanceObj) => (object)((Scroller)instanceObj).ScrollModel;

        private static void SetScrollModel(ref object instanceObj, object valueObj) => ((Scroller)instanceObj).ScrollModel = (ScrollModel)valueObj;

        private static object GetPrefetch(object instanceObj) => (object)((Scroller)instanceObj).Prefetch;

        private static void SetPrefetch(ref object instanceObj, object valueObj)
        {
            Scroller scroller = (Scroller)instanceObj;
            int num = (int)valueObj;
            Result result = Int32Schema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                scroller.Prefetch = num;
        }

        private static object Construct() => (object)new Scroller();

        public static void Pass1Initialize() => ScrollerSchema.Type = new UIXTypeSchema((short)184, "Scroller", (string)null, (short)34, typeof(Scroller), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)184, "ScrollModel", (short)182, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ScrollerSchema.GetScrollModel), new SetValueHandler(ScrollerSchema.SetScrollModel), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)184, "Prefetch", (short)115, (short)-1, ExpressionRestriction.None, false, Int32Schema.ValidateNotNegative, true, new GetValueHandler(ScrollerSchema.GetPrefetch), new SetValueHandler(ScrollerSchema.SetPrefetch), false);
            ScrollerSchema.Type.Initialize(new DefaultConstructHandler(ScrollerSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
