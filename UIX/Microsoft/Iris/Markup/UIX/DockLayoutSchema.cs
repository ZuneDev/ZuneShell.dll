// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DockLayoutSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;
using Microsoft.Iris.Layouts;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DockLayoutSchema
    {
        public static UIXTypeSchema Type;

        private static object GetDefaultLayoutInput(object instanceObj) => (object)((DockLayout)instanceObj).DefaultLayoutInput;

        private static void SetDefaultLayoutInput(ref object instanceObj, object valueObj) => ((DockLayout)instanceObj).DefaultLayoutInput = (DockLayoutInput)valueObj;

        private static object GetDefaultChildAlignment(object instanceObj) => (object)((DockLayout)instanceObj).DefaultChildAlignment;

        private static void SetDefaultChildAlignment(ref object instanceObj, object valueObj) => ((DockLayout)instanceObj).DefaultChildAlignment = (ItemAlignment)valueObj;

        private static object Construct() => (object)new DockLayout();

        public static void Pass1Initialize() => DockLayoutSchema.Type = new UIXTypeSchema((short)59, "DockLayout", (string)null, (short)132, typeof(DockLayout), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)59, "DefaultLayoutInput", (short)60, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(DockLayoutSchema.GetDefaultLayoutInput), new SetValueHandler(DockLayoutSchema.SetDefaultLayoutInput), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)59, "DefaultChildAlignment", (short)sbyte.MaxValue, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(DockLayoutSchema.GetDefaultChildAlignment), new SetValueHandler(DockLayoutSchema.SetDefaultChildAlignment), false);
            DockLayoutSchema.Type.Initialize(new DefaultConstructHandler(DockLayoutSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
