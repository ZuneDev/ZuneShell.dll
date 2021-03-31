// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.CommandSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class CommandSchema
    {
        public static UIXTypeSchema Type;

        private static object GetAvailable(object instanceObj) => BooleanBoxes.Box(((IUICommand)instanceObj).Available);

        private static void SetAvailable(ref object instanceObj, object valueObj) => ((IUICommand)instanceObj).Available = (bool)valueObj;

        private static object GetPriority(object instanceObj) => (object)((IUICommand)instanceObj).Priority;

        private static void SetPriority(ref object instanceObj, object valueObj) => ((IUICommand)instanceObj).Priority = (InvokePriority)valueObj;

        private static object Construct() => (object)new UICommand();

        private static object CallInvoke(object instanceObj, object[] parameters)
        {
            ((IUICommand)instanceObj).Invoke();
            return (object)null;
        }

        public static void Pass1Initialize() => CommandSchema.Type = new UIXTypeSchema((short)40, "Command", (string)null, (short)153, typeof(IUICommand), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)40, "Available", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CommandSchema.GetAvailable), new SetValueHandler(CommandSchema.SetAvailable), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)40, "Priority", (short)126, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(CommandSchema.GetPriority), new SetValueHandler(CommandSchema.SetPriority), false);
            UIXEventSchema uixEventSchema = new UIXEventSchema((short)40, "Invoked");
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)40, "Invoke", (short[])null, (short)240, new InvokeHandler(CommandSchema.CallInvoke), false);
            CommandSchema.Type.Initialize(new DefaultConstructHandler(CommandSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, new EventSchema[1] { (EventSchema)uixEventSchema }, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
