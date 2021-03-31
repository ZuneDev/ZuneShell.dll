// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.InputHandlerSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.UI;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class InputHandlerSchema
    {
        public static UIXTypeSchema Type;

        private static object GetName(object instanceObj) => (object)((InputHandler)instanceObj).Name;

        private static void SetName(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).Name = (string)valueObj;

        private static object GetEnabled(object instanceObj) => BooleanBoxes.Box(((InputHandler)instanceObj).Enabled);

        private static void SetEnabled(ref object instanceObj, object valueObj) => ((InputHandler)instanceObj).Enabled = (bool)valueObj;

        public static void Pass1Initialize() => InputHandlerSchema.Type = new UIXTypeSchema((short)110, "InputHandler", (string)null, (short)-1, typeof(InputHandler), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)110, "Name", (short)208, (short)-1, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(InputHandlerSchema.GetName), new SetValueHandler(InputHandlerSchema.SetName), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)110, "Enabled", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(InputHandlerSchema.GetEnabled), new SetValueHandler(InputHandlerSchema.SetEnabled), false);
            InputHandlerSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
