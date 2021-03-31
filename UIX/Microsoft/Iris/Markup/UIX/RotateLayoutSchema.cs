// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.RotateLayoutSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layouts;
using Microsoft.Iris.Library;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class RotateLayoutSchema
    {
        public static RangeValidator ValidateRightAngle = new RangeValidator(RotateLayoutSchema.RangeValidateRightAngle);
        public static UIXTypeSchema Type;

        private static object GetAngleDegrees(object instanceObj) => (object)((RotateLayout)instanceObj).AngleDegrees;

        private static void SetAngleDegrees(ref object instanceObj, object valueObj)
        {
            RotateLayout rotateLayout = (RotateLayout)instanceObj;
            int num = (int)valueObj;
            Result result = RotateLayoutSchema.ValidateRightAngle(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                rotateLayout.AngleDegrees = num;
        }

        private static object Construct() => (object)new RotateLayout();

        private static Result RangeValidateRightAngle(object value)
        {
            int num = (int)value;
            switch (num)
            {
                case 0:
                case 90:
                case 180:
                case 270:
                    return Result.Success;
                default:
                    return Result.Fail("Expecting a value of 0, 90, 180, or 270, but got {0}", (object)num.ToString());
            }
        }

        public static void Pass1Initialize() => RotateLayoutSchema.Type = new UIXTypeSchema((short)175, "RotateLayout", (string)null, (short)132, typeof(RotateLayout), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)175, "AngleDegrees", (short)115, (short)-1, ExpressionRestriction.None, false, RotateLayoutSchema.ValidateRightAngle, false, new GetValueHandler(RotateLayoutSchema.GetAngleDegrees), new SetValueHandler(RotateLayoutSchema.SetAngleDegrees), false);
            RotateLayoutSchema.Type.Initialize(new DefaultConstructHandler(RotateLayoutSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
