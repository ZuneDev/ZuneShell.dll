// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DesaturateSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DesaturateSchema
    {
        public static UIXTypeSchema Type;

        private static object GetDesaturate(object instanceObj) => (object)((DesaturateElement)instanceObj).Desaturate;

        private static void SetDesaturate(ref object instanceObj, object valueObj)
        {
            DesaturateElement desaturateElement = (DesaturateElement)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.Validate0to1(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                desaturateElement.Desaturate = num;
        }

        private static object Construct() => (object)new DesaturateElement();

        public static void Pass1Initialize() => DesaturateSchema.Type = new UIXTypeSchema((short)54, "Desaturate", (string)null, (short)80, typeof(DesaturateElement), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)54, "Desaturate", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, false, new GetValueHandler(DesaturateSchema.GetDesaturate), new SetValueHandler(DesaturateSchema.SetDesaturate), false);
            DesaturateSchema.Type.Initialize(new DefaultConstructHandler(DesaturateSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
