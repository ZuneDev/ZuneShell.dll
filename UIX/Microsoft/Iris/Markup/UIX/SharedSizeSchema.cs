// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.SharedSizeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class SharedSizeSchema
    {
        public static UIXTypeSchema Type;

        private static object GetMaximumSize(object instanceObj) => (object)((SharedSize)instanceObj).MaximumSize;

        private static void SetMaximumSize(ref object instanceObj, object valueObj)
        {
            SharedSize sharedSize = (SharedSize)instanceObj;
            Size size = (Size)valueObj;
            Result result = SizeSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                sharedSize.MaximumSize = size;
        }

        private static object GetMinimumSize(object instanceObj) => (object)((SharedSize)instanceObj).MinimumSize;

        private static void SetMinimumSize(ref object instanceObj, object valueObj)
        {
            SharedSize sharedSize = (SharedSize)instanceObj;
            Size size = (Size)valueObj;
            Result result = SizeSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                sharedSize.MinimumSize = size;
        }

        private static object GetSize(object instanceObj) => (object)((SharedSize)instanceObj).Size;

        private static void SetSize(ref object instanceObj, object valueObj)
        {
            SharedSize sharedSize = (SharedSize)instanceObj;
            Size size = (Size)valueObj;
            Result result = SizeSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                sharedSize.Size = size;
        }

        private static object Construct() => (object)new SharedSize();

        private static object CallAutoSize(object instanceObj, object[] parameters)
        {
            ((SharedSize)instanceObj).AutoSize();
            return (object)null;
        }

        public static void Pass1Initialize() => SharedSizeSchema.Type = new UIXTypeSchema((short)190, "SharedSize", (string)null, (short)153, typeof(SharedSize), UIXTypeFlags.None);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)190, "MaximumSize", (short)195, (short)-1, ExpressionRestriction.None, false, SizeSchema.ValidateNotNegative, true, new GetValueHandler(SharedSizeSchema.GetMaximumSize), new SetValueHandler(SharedSizeSchema.SetMaximumSize), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)190, "MinimumSize", (short)195, (short)-1, ExpressionRestriction.None, false, SizeSchema.ValidateNotNegative, true, new GetValueHandler(SharedSizeSchema.GetMinimumSize), new SetValueHandler(SharedSizeSchema.SetMinimumSize), false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)190, "Size", (short)195, (short)-1, ExpressionRestriction.None, false, SizeSchema.ValidateNotNegative, true, new GetValueHandler(SharedSizeSchema.GetSize), new SetValueHandler(SharedSizeSchema.SetSize), false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)190, "AutoSize", (short[])null, (short)240, new InvokeHandler(SharedSizeSchema.CallAutoSize), false);
            SharedSizeSchema.Type.Initialize(new DefaultConstructHandler(SharedSizeSchema.Construct), (ConstructorSchema[])null, new PropertySchema[3]
            {
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema3
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
