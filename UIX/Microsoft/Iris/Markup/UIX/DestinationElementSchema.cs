// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DestinationElementSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DestinationElementSchema
    {
        public static UIXTypeSchema Type;

        private static object GetDownsample(object instanceObj) => (object)((DestinationElement)instanceObj).Downsample;

        private static void SetDownsample(ref object instanceObj, object valueObj)
        {
            DestinationElement destinationElement = (DestinationElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.Validate0to1(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                destinationElement.Downsample = num;
        }

        private static object GetUVOffset(object instanceObj) => (object)((DestinationElement)instanceObj).UVOffset;

        private static void SetUVOffset(ref object instanceObj, object valueObj) => ((DestinationElement)instanceObj).UVOffset = (Vector2)valueObj;

        private static object Construct() => (object)new DestinationElement();

        public static void Pass1Initialize() => DestinationElementSchema.Type = new UIXTypeSchema((short)56, "DestinationElement", (string)null, (short)77, typeof(DestinationElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)56, "Downsample", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, false, new GetValueHandler(DestinationElementSchema.GetDownsample), new SetValueHandler(DestinationElementSchema.SetDownsample), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)56, "UVOffset", (short)233, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(DestinationElementSchema.GetUVOffset), new SetValueHandler(DestinationElementSchema.SetUVOffset), false);
            DestinationElementSchema.Type.Initialize(new DefaultConstructHandler(DestinationElementSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
