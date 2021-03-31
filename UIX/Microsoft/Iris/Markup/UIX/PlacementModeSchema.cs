// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.PlacementModeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layouts;
using Microsoft.Iris.Library;
using System.Collections;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class PlacementModeSchema
    {
        public static UIXTypeSchema Type;

        private static object GetPopupPositions(object instanceObj) => (object)new ArrayList((ICollection)((PlacementMode)instanceObj).PopupPositions);

        private static void SetPopupPositions(ref object instanceObj, object valueObj)
        {
            PlacementMode placementMode = (PlacementMode)instanceObj;
            IList list = (IList)valueObj;
            PopupPosition[] popupPositionArray = new PopupPosition[list.Count];
            for (int index = 0; index < list.Count; ++index)
                popupPositionArray[index] = (PopupPosition)list[index];
            placementMode.PopupPositions = popupPositionArray;
        }

        private static object GetMouseTarget(object instanceObj) => (object)((PlacementMode)instanceObj).MouseTarget;

        private static void SetMouseTarget(ref object instanceObj, object valueObj) => ((PlacementMode)instanceObj).MouseTarget = (MouseTarget)valueObj;

        private static object Construct() => (object)new PlacementMode();

        private static Result ConvertFromString(object valueObj, out object instanceObj)
        {
            string str = (string)valueObj;
            instanceObj = (object)null;
            PlacementMode instance = PlacementModeSchema.StringToInstance(str);
            if (instance == null)
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)str, (object)"PlacementMode");
            instanceObj = (object)instance;
            return Result.Success;
        }

        private static object FindCanonicalInstance(string name) => (object)PlacementModeSchema.StringToInstance(name);

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
                result = PlacementModeSchema.ConvertFromString(from, out instance);
                if (!result.Failed)
                    return result;
            }
            return result;
        }

        private static object CallTryParseStringPlacementMode(object instanceObj, object[] parameters)
        {
            string parameter1 = (string)parameters[0];
            PlacementMode parameter2 = (PlacementMode)parameters[1];
            object instanceObj1;
            return PlacementModeSchema.ConvertFromString((object)parameter1, out instanceObj1).Failed ? (object)parameter2 : instanceObj1;
        }

        private static PlacementMode StringToInstance(string value)
        {
            if (value == "Origin")
                return PlacementMode.Origin;
            if (value == "Left")
                return PlacementMode.Left;
            if (value == "Right")
                return PlacementMode.Right;
            if (value == "Top")
                return PlacementMode.Top;
            if (value == "Bottom")
                return PlacementMode.Bottom;
            if (value == "Center")
                return PlacementMode.Center;
            if (value == "MouseOrigin")
                return PlacementMode.MouseOrigin;
            if (value == "MouseBottom")
                return PlacementMode.MouseBottom;
            if (value == "FollowMouseOrigin")
                return PlacementMode.FollowMouseOrigin;
            return value == "FollowMouseBottom" ? PlacementMode.FollowMouseBottom : (PlacementMode)null;
        }

        public static void Pass1Initialize() => PlacementModeSchema.Type = new UIXTypeSchema((short)157, "PlacementMode", (string)null, (short)153, typeof(PlacementMode), UIXTypeFlags.Immutable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)157, "PopupPositions", (short)138, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PlacementModeSchema.GetPopupPositions), new SetValueHandler(PlacementModeSchema.SetPopupPositions), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)157, "MouseTarget", (short)149, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(PlacementModeSchema.GetMouseTarget), new SetValueHandler(PlacementModeSchema.SetMouseTarget), false);
            UIXMethodSchema uixMethodSchema = new UIXMethodSchema((short)157, "TryParse", new short[2]
            {
        (short) 208,
        (short) 157
            }, (short)157, new InvokeHandler(PlacementModeSchema.CallTryParseStringPlacementMode), true);
            PlacementModeSchema.Type.Initialize(new DefaultConstructHandler(PlacementModeSchema.Construct), (ConstructorSchema[])null, new PropertySchema[2]
            {
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema1
            }, new MethodSchema[1]
            {
        (MethodSchema) uixMethodSchema
            }, (EventSchema[])null, new FindCanonicalInstanceHandler(PlacementModeSchema.FindCanonicalInstance), new TypeConverterHandler(PlacementModeSchema.TryConvertFrom), new SupportsTypeConversionHandler(PlacementModeSchema.IsConversionSupported), (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }
    }
}
