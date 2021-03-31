// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.LayoutSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using System.Collections.Generic;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class LayoutSchema
    {
        private static Dictionary<string, object> s_NameToLayoutMap = new Dictionary<string, object>(10);
        public static UIXTypeSchema Type;

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string str = (string)valueObj;
            instanceObj = null;
            object obj;
            if (!LayoutSchema.s_NameToLayoutMap.TryGetValue(str.ToLowerInvariant(), out obj))
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", str, "Layout");
            ILayout layout = (ILayout)obj;
            instanceObj = layout;
            return Result.Success;
        }

        private static bool IsConversionSupported(TypeSchema fromType) => StringSchema.Type.IsAssignableFrom(fromType);

        private static Result TryConvertFrom(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result = Result.Fail("Unsupported");
            instance = null;
            if (StringSchema.Type.IsAssignableFrom(fromType))
            {
                result = LayoutSchema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            return result;
        }

        private static object CallTryParseStringLayout(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            ILayout parameter2 = (ILayout)parameters[1];
            object instanceObj1;
            return LayoutSchema.ConvertFromString(parameter1, out instanceObj1).Failed ? parameter2 : instanceObj1;
        }

        static LayoutSchema()
        {
            LayoutSchema.s_NameToLayoutMap.Add("anchor", new AnchorLayout());
            LayoutSchema.s_NameToLayoutMap.Add("default", DefaultLayout.Instance);
            LayoutSchema.s_NameToLayoutMap.Add("dock", new DockLayout());
            LayoutSchema.s_NameToLayoutMap.Add("grid", new GridLayout());
            LayoutSchema.s_NameToLayoutMap.Add("scale", new ScaleLayout());
            LayoutSchema.s_NameToLayoutMap.Add("popup", new PopupLayout());
            LayoutSchema.s_NameToLayoutMap.Add("stack", new StackLayout());
            LayoutSchema.s_NameToLayoutMap.Add("form", new AnchorLayout()
            {
                SizeToHorizontalChildren = false,
                SizeToVerticalChildren = false
            });
            LayoutSchema.s_NameToLayoutMap.Add("horizontalflow", new FlowLayout()
            {
                Orientation = Orientation.Horizontal
            });
            LayoutSchema.s_NameToLayoutMap.Add("verticalflow", new FlowLayout()
            {
                Orientation = Orientation.Vertical
            });
        }

        public static void Pass1Initialize() => LayoutSchema.Type = new UIXTypeSchema(132, "Layout", null, 153, typeof(ILayout), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema(132, "TryParse", new short[2]
            {
         208,
         132
            }, 132, new InvokeHandler(LayoutSchema.CallTryParseStringLayout), true);
            LayoutSchema.Type.Initialize(null, null, null, new MethodSchema[1]
            {
         uixMethodSchema
            }, null, null, new TypeConverterHandler(LayoutSchema.TryConvertFrom), new SupportsTypeConversionHandler(LayoutSchema.IsConversionSupported), null, null, null, null);
        }
    }
}
