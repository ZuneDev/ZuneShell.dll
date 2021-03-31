// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.LightShaftSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class LightShaftSchema
    {
        public static UIXTypeSchema Type;

        private static object GetPosition(object instanceObj) => (object)((LightShaftElement)instanceObj).Position;

        private static void SetPosition(ref object instanceObj, object valueObj) => ((LightShaftElement)instanceObj).Position = (Vector3)valueObj;

        private static object GetDecay(object instanceObj) => (object)((LightShaftElement)instanceObj).Decay;

        private static void SetDecay(ref object instanceObj, object valueObj)
        {
            LightShaftElement lightShaftElement = (LightShaftElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.Validate0to1(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                lightShaftElement.Decay = num;
        }

        private static object GetDensity(object instanceObj) => (object)((LightShaftElement)instanceObj).Density;

        private static void SetDensity(ref object instanceObj, object valueObj)
        {
            LightShaftElement lightShaftElement = (LightShaftElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.Validate0to1(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                lightShaftElement.Density = num;
        }

        private static object GetIntensity(object instanceObj) => (object)((LightShaftElement)instanceObj).Intensity;

        private static void SetIntensity(ref object instanceObj, object valueObj)
        {
            LightShaftElement lightShaftElement = (LightShaftElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                lightShaftElement.Intensity = num;
        }

        private static object GetFallOff(object instanceObj) => (object)((LightShaftElement)instanceObj).FallOff;

        private static void SetFallOff(ref object instanceObj, object valueObj)
        {
            LightShaftElement lightShaftElement = (LightShaftElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.Validate0to1(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                lightShaftElement.FallOff = num;
        }

        private static object GetWeight(object instanceObj) => (object)((LightShaftElement)instanceObj).Weight;

        private static void SetWeight(ref object instanceObj, object valueObj)
        {
            LightShaftElement lightShaftElement = (LightShaftElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                lightShaftElement.Weight = num;
        }

        private static object Construct() => (object)new LightShaftElement();

        public static void Pass1Initialize() => LightShaftSchema.Type = new UIXTypeSchema((short)135, "LightShaft", (string)null, (short)80, typeof(LightShaftElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)135, "Position", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(LightShaftSchema.GetPosition), new SetValueHandler(LightShaftSchema.SetPosition), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)135, "Decay", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, false, new GetValueHandler(LightShaftSchema.GetDecay), new SetValueHandler(LightShaftSchema.SetDecay), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)135, "Density", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, false, new GetValueHandler(LightShaftSchema.GetDensity), new SetValueHandler(LightShaftSchema.SetDensity), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)135, "Intensity", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, new GetValueHandler(LightShaftSchema.GetIntensity), new SetValueHandler(LightShaftSchema.SetIntensity), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)135, "FallOff", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, false, new GetValueHandler(LightShaftSchema.GetFallOff), new SetValueHandler(LightShaftSchema.SetFallOff), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)135, "Weight", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, new GetValueHandler(LightShaftSchema.GetWeight), new SetValueHandler(LightShaftSchema.SetWeight), false);
            LightShaftSchema.Type.Initialize(new DefaultConstructHandler(LightShaftSchema.Construct), (ConstructorSchema[])null, new PropertySchema[6]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema6
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
