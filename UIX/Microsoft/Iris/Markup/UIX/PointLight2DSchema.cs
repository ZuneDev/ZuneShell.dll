// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.PointLight2DSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class PointLight2DSchema
    {
        public static UIXTypeSchema Type;

        private static object GetPosition(object instanceObj) => (object)((PointLight2DElement)instanceObj).Position;

        private static void SetPosition(ref object instanceObj, object valueObj) => ((PointLight2DElement)instanceObj).Position = (Vector3)valueObj;

        private static object GetRadius(object instanceObj) => (object)((PointLight2DElement)instanceObj).Radius;

        private static void SetRadius(ref object instanceObj, object valueObj)
        {
            PointLight2DElement pointLight2Delement = (PointLight2DElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                pointLight2Delement.Radius = num;
        }

        private static void SetLightColor(ref object instanceObj, object valueObj) => ((PointLight2DElement)instanceObj).LightColor = ((Color)valueObj).RenderConvert();

        private static void SetAmbientColor(ref object instanceObj, object valueObj) => ((PointLight2DElement)instanceObj).AmbientColor = ((Color)valueObj).RenderConvert();

        private static object GetAttenuation(object instanceObj) => (object)((PointLight2DElement)instanceObj).Attenuation;

        private static void SetAttenuation(ref object instanceObj, object valueObj) => ((PointLight2DElement)instanceObj).Attenuation = (Vector3)valueObj;

        private static object Construct() => (object)new PointLight2DElement();

        public static void Pass1Initialize() => PointLight2DSchema.Type = new UIXTypeSchema((short)159, "PointLight2D", (string)null, (short)77, typeof(PointLight2DElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)159, "Position", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PointLight2DSchema.GetPosition), new SetValueHandler(PointLight2DSchema.SetPosition), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)159, "Radius", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.ValidateNotNegative, false, new GetValueHandler(PointLight2DSchema.GetRadius), new SetValueHandler(PointLight2DSchema.SetRadius), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)159, "LightColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(PointLight2DSchema.SetLightColor), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)159, "AmbientColor", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, (GetValueHandler)null, new SetValueHandler(PointLight2DSchema.SetAmbientColor), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)159, "Attenuation", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PointLight2DSchema.GetAttenuation), new SetValueHandler(PointLight2DSchema.SetAttenuation), false);
            PointLight2DSchema.Type.Initialize(new DefaultConstructHandler(PointLight2DSchema.Construct), (ConstructorSchema[])null, new PropertySchema[5]
            {
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
