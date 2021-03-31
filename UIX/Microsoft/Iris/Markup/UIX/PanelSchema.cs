// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.PanelSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.UI;
using Microsoft.Iris.ViewItems;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class PanelSchema
    {
        public static UIXTypeSchema Type;

        private static object GetChildren(object instanceObj) => (object)ViewItemSchema.ListProxy.GetChildren((ViewItem)instanceObj);

        private static object Construct() => (object)new Panel();

        public static void Pass1Initialize() => PanelSchema.Type = new UIXTypeSchema((short)156, "Panel", (string)null, (short)239, typeof(Panel), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema = new UIXPropertySchema((short)156, "Children", (short)138, (short)239, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(PanelSchema.GetChildren), (SetValueHandler)null, false);
            PanelSchema.Type.Initialize(new DefaultConstructHandler(PanelSchema.Construct), (ConstructorSchema[])null, new PropertySchema[1]
            {
        (PropertySchema) uixPropertySchema
            }, (MethodSchema[])null, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
