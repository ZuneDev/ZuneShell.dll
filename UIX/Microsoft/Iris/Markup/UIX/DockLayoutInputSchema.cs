// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.DockLayoutInputSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layouts;
using Microsoft.Iris.Library;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class DockLayoutInputSchema
    {
        public static UIXTypeSchema Type;

        private static object Construct() => (object)DockLayoutInput.Client;

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string str = (string)valueObj;
            instanceObj = (object)null;
            DockLayoutInput instance = DockLayoutInputSchema.StringToInstance(str);
            if (instance == null)
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)str, (object)"DockLayoutInput");
            instanceObj = (object)instance;
            return Result.Success;
        }

        private static object FindCanonicalInstance(string name) => (object)DockLayoutInputSchema.StringToInstance(name);

        private static bool IsConversionSupported(TypeSchema fromType) => StringSchema.Type.IsAssignableFrom(fromType);

        private static Result TryConvertFrom(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result = Result.Fail("Unsupported");
            instance = (object)null;
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                result = DockLayoutInputSchema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            return result;
        }

        private static object CallTryParseStringDockLayoutInput(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            DockLayoutInput parameter2 = (DockLayoutInput)parameters[1];
            object instanceObj1;
            return DockLayoutInputSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        private static DockLayoutInput StringToInstance(string value)
        {
            if (value == "Left")
                return DockLayoutInput.Left;
            if (value == "Top")
                return DockLayoutInput.Top;
            if (value == "Right")
                return DockLayoutInput.Right;
            if (value == "Bottom")
                return DockLayoutInput.Bottom;
            return value == "Client" ? DockLayoutInput.Client : (DockLayoutInput)null;
        }

        public static void Pass1Initialize() => DockLayoutInputSchema.Type = new UIXTypeSchema((short)60, "DockLayoutInput", (string)null, (short)133, typeof(DockLayoutInput), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)60, "TryParse", new short[2]
            {
        (short) 208,
        (short) 60
            }, (short)60, new InvokeHandler(DockLayoutInputSchema.CallTryParseStringDockLayoutInput), true);
            DockLayoutInputSchema.Type.Initialize(new DefaultConstructHandler(DockLayoutInputSchema.Construct), (ConstructorSchema[])null, (PropertySchema[])null, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, new FindCanonicalInstanceHandler(DockLayoutInputSchema.FindCanonicalInstance), new TypeConverterHandler(DockLayoutInputSchema.TryConvertFrom), new SupportsTypeConversionHandler(DockLayoutInputSchema.IsConversionSupported), (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
