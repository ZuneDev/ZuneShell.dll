// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.SwitchAnimationSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.ModelItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class SwitchAnimationSchema
    {
        public static UIXTypeSchema Type;

        private static object GetExpression(object instanceObj) => (object)((SwitchAnimation)instanceObj).Expression;

        private static void SetExpression(ref object instanceObj, object valueObj) => ((SwitchAnimation)instanceObj).Expression = (IUIValueRange)valueObj;

        private static object GetOptions(object instanceObj) => (object)((SwitchAnimation)instanceObj).Options;

        private static object GetType(object instanceObj) => (object)((SwitchAnimation)instanceObj).Type;

        private static void SetType(ref object instanceObj, object valueObj) => ((SwitchAnimation)instanceObj).Type = (AnimationEventType)valueObj;

        private static object Construct() => (object)new SwitchAnimation();

        public static void Pass1Initialize() => SwitchAnimationSchema.Type = new UIXTypeSchema((short)210, "SwitchAnimation", (string)null, (short)104, typeof(SwitchAnimation), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)210, "Expression", (short)231, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(SwitchAnimationSchema.GetExpression), new SetValueHandler(SwitchAnimationSchema.SetExpression), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)210, "Options", (short)58, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(SwitchAnimationSchema.GetOptions), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)210, "Type", (short)10, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(SwitchAnimationSchema.GetType), new SetValueHandler(SwitchAnimationSchema.SetType), false);
            SwitchAnimationSchema.Type.Initialize(new DefaultConstructHandler(SwitchAnimationSchema.Construct), (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
