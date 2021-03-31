// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.EdgeDetectionSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class EdgeDetectionSchema
    {
        public static UIXTypeSchema Type;

        private static object GetEdgeLimit(object instanceObj) => (object)((EdgeDetectionElement)instanceObj).EdgeLimit;

        private static void SetEdgeLimit(ref object instanceObj, object valueObj)
        {
            EdgeDetectionElement detectionElement = (EdgeDetectionElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.Validate0to1(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                detectionElement.EdgeLimit = num;
        }

        private static object Construct() => (object)new EdgeDetectionElement();

        public static void Pass1Initialize() => EdgeDetectionSchema.Type = new UIXTypeSchema((short)66, "EdgeDetection", (string)null, (short)80, typeof(EdgeDetectionElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)66, "EdgeLimit", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, false, new GetValueHandler(EdgeDetectionSchema.GetEdgeLimit), new SetValueHandler(EdgeDetectionSchema.SetEdgeLimit), false);
            EdgeDetectionSchema.Type.Initialize(new DefaultConstructHandler(EdgeDetectionSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
