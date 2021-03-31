// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.SpotLight2DSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class SpotLight2DSchema
    {
        public static UIXTypeSchema Type;

        private static object GetPosition(object instanceObj) => ((SpotLight2DElement)instanceObj).Position;

        private static void SetPosition(ref object instanceObj, object valueObj) => ((SpotLight2DElement)instanceObj).Position = (Vector3)valueObj;

        private static object GetDirectionAngle(object instanceObj) => ((SpotLight2DElement)instanceObj).DirectionAngle;

        private static void SetDirectionAngle(ref object instanceObj, object valueObj) => ((SpotLight2DElement)instanceObj).DirectionAngle = (float)valueObj;

        private static void SetLightColor(ref object instanceObj, object valueObj) => ((SpotLight2DElement)instanceObj).LightColor = ((Color)valueObj).RenderConvert();

        private static void SetAmbientColor(ref object instanceObj, object valueObj) => ((SpotLight2DElement)instanceObj).AmbientColor = ((Color)valueObj).RenderConvert();

        private static object GetInnerConeAngle(object instanceObj) => ((SpotLight2DElement)instanceObj).InnerConeAngle;

        private static void SetInnerConeAngle(ref object instanceObj, object valueObj)
        {
            SpotLight2DElement spotLight2Delement = (SpotLight2DElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                spotLight2Delement.InnerConeAngle = num;
        }

        private static object GetOuterConeAngle(object instanceObj) => ((SpotLight2DElement)instanceObj).OuterConeAngle;

        private static void SetOuterConeAngle(ref object instanceObj, object valueObj)
        {
            SpotLight2DElement spotLight2Delement = (SpotLight2DElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                spotLight2Delement.OuterConeAngle = num;
        }

        private static object GetIntensity(object instanceObj) => ((SpotLight2DElement)instanceObj).Intensity;

        private static void SetIntensity(ref object instanceObj, object valueObj)
        {
            SpotLight2DElement spotLight2Delement = (SpotLight2DElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                spotLight2Delement.Intensity = num;
        }

        private static object GetAttenuation(object instanceObj) => ((SpotLight2DElement)instanceObj).Attenuation;

        private static void SetAttenuation(ref object instanceObj, object valueObj) => ((SpotLight2DElement)instanceObj).Attenuation = (Vector3)valueObj;

        private static object Construct() => new SpotLight2DElement();

        public static void Pass1Initialize() => SpotLight2DSchema.Type = new UIXTypeSchema(202, "SpotLight2D", null, 77, typeof(SpotLight2DElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema(202, "Position", 234, -1, ExpressionRestriction.None, false, null, false, new GetValueHandler(SpotLight2DSchema.GetPosition), new SetValueHandler(SpotLight2DSchema.SetPosition), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema(202, "DirectionAngle", 194, -1, ExpressionRestriction.None, false, null, false, new GetValueHandler(SpotLight2DSchema.GetDirectionAngle), new SetValueHandler(SpotLight2DSchema.SetDirectionAngle), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema(202, "LightColor", 35, -1, ExpressionRestriction.None, false, null, false, null, new SetValueHandler(SpotLight2DSchema.SetLightColor), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema(202, "AmbientColor", 35, -1, ExpressionRestriction.None, false, null, false, null, new SetValueHandler(SpotLight2DSchema.SetAmbientColor), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema(202, "InnerConeAngle", 194, -1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, new GetValueHandler(SpotLight2DSchema.GetInnerConeAngle), new SetValueHandler(SpotLight2DSchema.SetInnerConeAngle), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema(202, "OuterConeAngle", 194, -1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, new GetValueHandler(SpotLight2DSchema.GetOuterConeAngle), new SetValueHandler(SpotLight2DSchema.SetOuterConeAngle), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema(202, "Intensity", 194, -1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, new GetValueHandler(SpotLight2DSchema.GetIntensity), new SetValueHandler(SpotLight2DSchema.SetIntensity), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema(202, "Attenuation", 234, -1, ExpressionRestriction.None, false, null, false, new GetValueHandler(SpotLight2DSchema.GetAttenuation), new SetValueHandler(SpotLight2DSchema.SetAttenuation), false);
            SpotLight2DSchema.Type.Initialize(new DefaultConstructHandler(SpotLight2DSchema.Construct), null, new PropertySchema[8]
            {
         uixPropertySchema4,
         uixPropertySchema8,
         uixPropertySchema2,
         uixPropertySchema5,
         uixPropertySchema7,
         uixPropertySchema3,
         uixPropertySchema6,
         uixPropertySchema1
            }, null, null, null, null, null, null, null, null, null);
        }
    }
}
