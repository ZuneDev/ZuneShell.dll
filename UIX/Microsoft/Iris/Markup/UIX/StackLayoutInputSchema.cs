// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.StackLayoutInputSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layouts;
using Microsoft.Iris.Render;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class StackLayoutInputSchema
    {
        public static UIXTypeSchema Type;

        private static object GetPriority(object instanceObj) => (object)((StackLayoutInput)instanceObj).Priority;

        private static void SetPriority(ref object instanceObj, object valueObj) => ((StackLayoutInput)instanceObj).Priority = (StackPriority)valueObj;

        private static object GetMinimumSize(object instanceObj) => (object)((StackLayoutInput)instanceObj).MinimumSize;

        private static void SetMinimumSize(ref object instanceObj, object valueObj) => ((StackLayoutInput)instanceObj).MinimumSize = (Size)valueObj;

        private static object Construct() => (object)new StackLayoutInput();

        public static void Pass1Initialize() => StackLayoutInputSchema.Type = new UIXTypeSchema((short)205, "StackLayoutInput", (string)null, (short)133, typeof(StackLayoutInput), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)205, "Priority", (short)206, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(StackLayoutInputSchema.GetPriority), new SetValueHandler(StackLayoutInputSchema.SetPriority), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)205, "MinimumSize", (short)195, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(StackLayoutInputSchema.GetMinimumSize), new SetValueHandler(StackLayoutInputSchema.SetMinimumSize), false);
            StackLayoutInputSchema.Type.Initialize(new DefaultConstructHandler(StackLayoutInputSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
