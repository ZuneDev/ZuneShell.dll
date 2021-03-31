// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.HwndHostSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class HwndHostSchema
    {
        public static UIXTypeSchema Type;

        private static object GetHandle(object instanceObj) => (object)((HwndHost)instanceObj).Handle;

        private static object GetChildHandle(object instanceObj) => (object)((HwndHost)instanceObj).ChildHandle;

        private static void SetChildHandle(ref object instanceObj, object valueObj) => ((HwndHost)instanceObj).ChildHandle = (long)valueObj;

        private static object Construct() => (object)new HwndHost();

        public static void Pass1Initialize() => HwndHostSchema.Type = new UIXTypeSchema((short)103, "HwndHost", (string)null, (short)239, typeof(HwndHost), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)103, "Handle", (short)116, (short)-1, ExpressionRestriction.ReadOnly, false, (RangeValidator)null, true, new GetValueHandler(HwndHostSchema.GetHandle), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)103, "ChildHandle", (short)116, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(HwndHostSchema.GetChildHandle), new SetValueHandler(HwndHostSchema.SetChildHandle), false);
            HwndHostSchema.Type.Initialize(new DefaultConstructHandler(HwndHostSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
