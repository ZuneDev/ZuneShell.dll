// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.MergeAnimationSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class MergeAnimationSchema
    {
        public static UIXTypeSchema Type;

        private static object GetSources(object instanceObj) => (object)((MergeAnimation)instanceObj).Sources;

        private static object GetType(object instanceObj) => (object)((MergeAnimation)instanceObj).Type;

        private static void SetType(ref object instanceObj, object valueObj) => ((MergeAnimation)instanceObj).Type = (AnimationEventType)valueObj;

        private static object Construct() => (object)new MergeAnimation();

        public static void Pass1Initialize() => MergeAnimationSchema.Type = new UIXTypeSchema((short)147, "MergeAnimation", (string)null, (short)104, typeof(MergeAnimation), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)147, "Sources", (short)138, (short)104, ExpressionRestriction.NoAccess, false, (RangeValidator)null, false, new GetValueHandler(MergeAnimationSchema.GetSources), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)147, "Type", (short)10, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(MergeAnimationSchema.GetType), new SetValueHandler(MergeAnimationSchema.SetType), false);
            MergeAnimationSchema.Type.Initialize(new DefaultConstructHandler(MergeAnimationSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
