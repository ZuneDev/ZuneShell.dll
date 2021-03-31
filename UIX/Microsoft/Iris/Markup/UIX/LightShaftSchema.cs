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

        private static object GetPosition(object instanceObj) => ((LightShaftElement)instanceObj).Position;

        private static void SetPosition(ref object instanceObj, object valueObj) => ((LightShaftElement)instanceObj).Position = (Vector3)valueObj;

        private static object GetDecay(object instanceObj) => ((LightShaftElement)instanceObj).Decay;

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

        private static object GetDensity(object instanceObj) => ((LightShaftElement)instanceObj).Density;

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

        private static object GetIntensity(object instanceObj) => ((LightShaftElement)instanceObj).Intensity;

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

        private static object GetFallOff(object instanceObj) => ((LightShaftElement)instanceObj).FallOff;

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

        private static object GetWeight(object instanceObj) => ((LightShaftElement)instanceObj).Weight;

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

        private static object Construct() => new LightShaftElement();

        public static void Pass1Initialize() => LightShaftSchema.Type = new UIXTypeSchema(135, "LightShaft", null, 80, typeof(LightShaftElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema(135, "Position", 234, -1, ExpressionRestriction.None, false, null, false, new GetValueHandler(LightShaftSchema.GetPosition), new SetValueHandler(LightShaftSchema.SetPosition), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema(135, "Decay", 194, -1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, false, new GetValueHandler(LightShaftSchema.GetDecay), new SetValueHandler(LightShaftSchema.SetDecay), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema(135, "Density", 194, -1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, false, new GetValueHandler(LightShaftSchema.GetDensity), new SetValueHandler(LightShaftSchema.SetDensity), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema(135, "Intensity", 194, -1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, new GetValueHandler(LightShaftSchema.GetIntensity), new SetValueHandler(LightShaftSchema.SetIntensity), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema(135, "FallOff", 194, -1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, false, new GetValueHandler(LightShaftSchema.GetFallOff), new SetValueHandler(LightShaftSchema.SetFallOff), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema(135, "Weight", 194, -1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, new GetValueHandler(LightShaftSchema.GetWeight), new SetValueHandler(LightShaftSchema.SetWeight), false);
            LightShaftSchema.Type.Initialize(new DefaultConstructHandler(LightShaftSchema.Construct), null, new PropertySchema[6]
            {
         uixPropertySchema2,
         uixPropertySchema3,
         uixPropertySchema5,
         uixPropertySchema4,
         uixPropertySchema1,
         uixPropertySchema6
            }, null, null, null, null, null, null, null, null, null);
        }
    }
}
