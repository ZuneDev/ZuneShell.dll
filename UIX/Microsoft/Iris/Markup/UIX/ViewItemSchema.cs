// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIX.ViewItemSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Library;
using Microsoft.Iris.Navigation;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;
using System.Collections;

namespace Microsoft.Iris.Markup.UIX
{
    internal static class ViewItemSchema
    {
        public static UIXTypeSchema Type;

        private static object GetAlpha(object instanceObj) => (object)((ViewItem)instanceObj).Alpha;

        private static void SetAlpha(ref object instanceObj, object valueObj)
        {
            ViewItem viewItem = (ViewItem)instanceObj;
            float num = (float)valueObj;
            Result result = SingleSchema.Validate0to1(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                viewItem.Alpha = num;
        }

        private static object GetAnimations(object instanceObj) => (object)ViewItemSchema.ListProxy.GetAnimation((ViewItem)instanceObj);

        private static object GetCenterPointPercent(object instanceObj) => (object)((ViewItem)instanceObj).CenterPointPercent;

        private static void SetCenterPointPercent(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).CenterPointPercent = (Vector3)valueObj;

        private static object GetDebugOutline(object instanceObj) => (object)((ViewItem)instanceObj).DebugOutline;

        private static void SetDebugOutline(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).DebugOutline = (Color)valueObj;

        private static object GetFocusOrder(object instanceObj) => (object)((ViewItem)instanceObj).FocusOrder;

        private static void SetFocusOrder(ref object instanceObj, object valueObj)
        {
            ViewItem viewItem = (ViewItem)instanceObj;
            int num = (int)valueObj;
            Result result = Int32Schema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                viewItem.FocusOrder = num;
        }

        private static object GetAlignment(object instanceObj) => (object)((ViewItem)instanceObj).Alignment;

        private static void SetAlignment(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Alignment = (ItemAlignment)valueObj;

        private static object GetChildAlignment(object instanceObj) => (object)((ViewItem)instanceObj).ChildAlignment;

        private static void SetChildAlignment(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).ChildAlignment = (ItemAlignment)valueObj;

        private static object GetLayout(object instanceObj) => (object)((ViewItem)instanceObj).Layout;

        private static void SetLayout(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Layout = (ILayout)valueObj;

        private static void SetLayoutInput(ref object instanceObj, object valueObj)
        {
            ViewItem viewItem = (ViewItem)instanceObj;
            ILayoutInput layoutInput = (ILayoutInput)valueObj;
            if (layoutInput == null)
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"LayoutInput");
            else
                viewItem.LayoutInput = layoutInput;
        }

        private static object GetLayoutOutput(object instanceObj) => (object)((ViewItem)instanceObj).LayoutOutput;

        private static object GetMargins(object instanceObj) => (object)((ViewItem)instanceObj).Margins;

        private static void SetMargins(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Margins = (Inset)valueObj;

        private static object GetMaximumSize(object instanceObj) => ((ViewItem)instanceObj).MaximumSizeObject;

        private static void SetMaximumSize(ref object instanceObj, object valueObj)
        {
            ViewItem viewItem = (ViewItem)instanceObj;
            Result result = SizeSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                viewItem.MaximumSizeObject = valueObj;
        }

        private static object GetMinimumSize(object instanceObj) => ((ViewItem)instanceObj).MinimumSizeObject;

        private static void SetMinimumSize(ref object instanceObj, object valueObj)
        {
            ViewItem viewItem = (ViewItem)instanceObj;
            Result result = SizeSchema.ValidateNotNegative(valueObj);
            if (result.Failed)
                ErrorManager.ReportError(result.Error);
            else
                viewItem.MinimumSizeObject = valueObj;
        }

        private static object GetMouseInteractive(object instanceObj) => BooleanBoxes.Box(((ViewItem)instanceObj).MouseInteractive);

        private static void SetMouseInteractive(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).MouseInteractive = (bool)valueObj;

        private static object GetName(object instanceObj) => (object)((ViewItem)instanceObj).Name;

        private static void SetName(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Name = (string)valueObj;

        private static object GetNavigation(object instanceObj) => (object)((ViewItem)instanceObj).Navigation;

        private static void SetNavigation(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Navigation = (NavigationPolicies)valueObj;

        private static object GetPadding(object instanceObj) => (object)((ViewItem)instanceObj).Padding;

        private static void SetPadding(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Padding = (Inset)valueObj;

        private static object GetRotation(object instanceObj) => (object)((ViewItem)instanceObj).Rotation;

        private static void SetRotation(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Rotation = (Rotation)valueObj;

        private static object GetScale(object instanceObj) => (object)((ViewItem)instanceObj).Scale;

        private static void SetScale(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Scale = (Vector3)valueObj;

        private static object GetSharedSize(object instanceObj) => (object)((ViewItem)instanceObj).SharedSize;

        private static void SetSharedSize(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).SharedSize = (SharedSize)valueObj;

        private static object GetSharedSizePolicy(object instanceObj) => (object)((ViewItem)instanceObj).SharedSizePolicy;

        private static void SetSharedSizePolicy(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).SharedSizePolicy = (SharedSizePolicy)valueObj;

        private static object GetVisible(object instanceObj) => BooleanBoxes.Box(((ViewItem)instanceObj).Visible);

        private static void SetVisible(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Visible = (bool)valueObj;

        private static object GetBackground(object instanceObj) => (object)((ViewItem)instanceObj).Background;

        private static void SetBackground(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Background = (Color)valueObj;

        private static object GetCamera(object instanceObj) => (object)((ViewItem)instanceObj).Camera;

        private static void SetCamera(ref object instanceObj, object valueObj) => ((ViewItem)instanceObj).Camera = (Camera)valueObj;

        private static object CallAttachAnimationIAnimation(object instanceObj, object[] parameters)
        {
            ViewItem viewItem = (ViewItem)instanceObj;
            IAnimationProvider parameter = (IAnimationProvider)parameters[0];
            if (parameter == null)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"animation");
                return (object)null;
            }
            viewItem.AttachAnimation(parameter);
            return (object)null;
        }

        private static object CallAttachAnimationIAnimationAnimationHandle(
          object instanceObj,
          object[] parameters)
        {
            ViewItem viewItem = (ViewItem)instanceObj;
            IAnimationProvider parameter1 = (IAnimationProvider)parameters[0];
            AnimationHandle parameter2 = (AnimationHandle)parameters[1];
            if (parameter1 == null)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"animation");
                return (object)null;
            }
            if (parameter2 == null)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"handle");
                return (object)null;
            }
            viewItem.AttachAnimation(parameter1, parameter2);
            return (object)null;
        }

        private static object CallDetachAnimationAnimationEventType(
          object instanceObj,
          object[] parameters)
        {
            ((ViewItem)instanceObj).DetachAnimation((AnimationEventType)parameters[0]);
            return (object)null;
        }

        private static object CallPlayAnimationIAnimation(object instanceObj, object[] parameters)
        {
            ViewItem viewItem = (ViewItem)instanceObj;
            IAnimationProvider parameter = (IAnimationProvider)parameters[0];
            if (parameter == null)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"animation");
                return (object)null;
            }
            viewItem.PlayAnimation(parameter, (AnimationHandle)null);
            return (object)null;
        }

        private static object CallPlayAnimationIAnimationAnimationHandle(
          object instanceObj,
          object[] parameters)
        {
            ViewItem viewItem = (ViewItem)instanceObj;
            IAnimationProvider parameter1 = (IAnimationProvider)parameters[0];
            AnimationHandle parameter2 = (AnimationHandle)parameters[1];
            if (parameter1 == null)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"animation");
                return (object)null;
            }
            if (parameter2 == null)
            {
                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"handle");
                return (object)null;
            }
            viewItem.PlayAnimation(parameter1, parameter2);
            return (object)null;
        }

        private static object CallPlayAnimationAnimationEventType(
          object instanceObj,
          object[] parameters)
        {
            ((ViewItem)instanceObj).PlayAnimation((AnimationEventType)parameters[0]);
            return (object)null;
        }

        private static object CallForceContentChange(object instanceObj, object[] parameters)
        {
            ((ViewItem)instanceObj).ForceContentChange();
            return (object)null;
        }

        private static object CallSnapshotPosition(object instanceObj, object[] parameters) => (object)((ViewItem)instanceObj).SnapshotPosition();

        private static object CallNavigateInto(object instanceObj, object[] parameters)
        {
            ((ViewItem)instanceObj).NavigateInto();
            return (object)null;
        }

        private static object CallNavigateIntoBoolean(object instanceObj, object[] parameters)
        {
            ((ViewItem)instanceObj).NavigateInto((bool)parameters[0]);
            return (object)null;
        }

        private static object CallScrollIntoView(object instanceObj, object[] parameters)
        {
            ((ViewItem)instanceObj).ScrollIntoView();
            return (object)null;
        }

        public static void Pass1Initialize() => ViewItemSchema.Type = new UIXTypeSchema((short)239, "ViewItem", (string)null, (short)-1, typeof(ViewItem), UIXTypeFlags.Disposable);

        public static void Pass2Initialize()
        {
            UIXPropertySchema uixPropertySchema1 = new UIXPropertySchema((short)239, "Alpha", (short)194, (short)-1, ExpressionRestriction.None, false, SingleSchema.Validate0to1, true, new GetValueHandler(ViewItemSchema.GetAlpha), new SetValueHandler(ViewItemSchema.SetAlpha), false);
            UIXPropertySchema uixPropertySchema2 = new UIXPropertySchema((short)239, "Animations", (short)138, (short)104, ExpressionRestriction.NoAccess, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetAnimations), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema3 = new UIXPropertySchema((short)239, "CenterPointPercent", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetCenterPointPercent), new SetValueHandler(ViewItemSchema.SetCenterPointPercent), false);
            UIXPropertySchema uixPropertySchema4 = new UIXPropertySchema((short)239, "DebugOutline", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetDebugOutline), new SetValueHandler(ViewItemSchema.SetDebugOutline), false);
            UIXPropertySchema uixPropertySchema5 = new UIXPropertySchema((short)239, "FocusOrder", (short)115, (short)-1, ExpressionRestriction.None, false, Int32Schema.ValidateNotNegative, true, new GetValueHandler(ViewItemSchema.GetFocusOrder), new SetValueHandler(ViewItemSchema.SetFocusOrder), false);
            UIXPropertySchema uixPropertySchema6 = new UIXPropertySchema((short)239, "Alignment", (short)sbyte.MaxValue, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetAlignment), new SetValueHandler(ViewItemSchema.SetAlignment), false);
            UIXPropertySchema uixPropertySchema7 = new UIXPropertySchema((short)239, "ChildAlignment", (short)sbyte.MaxValue, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetChildAlignment), new SetValueHandler(ViewItemSchema.SetChildAlignment), false);
            UIXPropertySchema uixPropertySchema8 = new UIXPropertySchema((short)239, "Layout", (short)132, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetLayout), new SetValueHandler(ViewItemSchema.SetLayout), false);
            UIXPropertySchema uixPropertySchema9 = new UIXPropertySchema((short)239, "LayoutInput", (short)133, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, (GetValueHandler)null, new SetValueHandler(ViewItemSchema.SetLayoutInput), false);
            UIXPropertySchema uixPropertySchema10 = new UIXPropertySchema((short)239, "LayoutOutput", (short)134, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, false, new GetValueHandler(ViewItemSchema.GetLayoutOutput), (SetValueHandler)null, false);
            UIXPropertySchema uixPropertySchema11 = new UIXPropertySchema((short)239, "Margins", (short)114, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetMargins), new SetValueHandler(ViewItemSchema.SetMargins), false);
            UIXPropertySchema uixPropertySchema12 = new UIXPropertySchema((short)239, "MaximumSize", (short)195, (short)-1, ExpressionRestriction.None, false, SizeSchema.ValidateNotNegative, true, new GetValueHandler(ViewItemSchema.GetMaximumSize), new SetValueHandler(ViewItemSchema.SetMaximumSize), false);
            UIXPropertySchema uixPropertySchema13 = new UIXPropertySchema((short)239, "MinimumSize", (short)195, (short)-1, ExpressionRestriction.None, false, SizeSchema.ValidateNotNegative, true, new GetValueHandler(ViewItemSchema.GetMinimumSize), new SetValueHandler(ViewItemSchema.SetMinimumSize), false);
            UIXPropertySchema uixPropertySchema14 = new UIXPropertySchema((short)239, "MouseInteractive", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetMouseInteractive), new SetValueHandler(ViewItemSchema.SetMouseInteractive), false);
            UIXPropertySchema uixPropertySchema15 = new UIXPropertySchema((short)239, "Name", (short)208, (short)-1, ExpressionRestriction.ReadOnly, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetName), new SetValueHandler(ViewItemSchema.SetName), false);
            UIXPropertySchema uixPropertySchema16 = new UIXPropertySchema((short)239, "Navigation", (short)151, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetNavigation), new SetValueHandler(ViewItemSchema.SetNavigation), false);
            UIXPropertySchema uixPropertySchema17 = new UIXPropertySchema((short)239, "Padding", (short)114, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetPadding), new SetValueHandler(ViewItemSchema.SetPadding), false);
            UIXPropertySchema uixPropertySchema18 = new UIXPropertySchema((short)239, "Rotation", (short)176, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetRotation), new SetValueHandler(ViewItemSchema.SetRotation), false);
            UIXPropertySchema uixPropertySchema19 = new UIXPropertySchema((short)239, "Scale", (short)234, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetScale), new SetValueHandler(ViewItemSchema.SetScale), false);
            UIXPropertySchema uixPropertySchema20 = new UIXPropertySchema((short)239, "SharedSize", (short)190, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetSharedSize), new SetValueHandler(ViewItemSchema.SetSharedSize), false);
            UIXPropertySchema uixPropertySchema21 = new UIXPropertySchema((short)239, "SharedSizePolicy", (short)191, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetSharedSizePolicy), new SetValueHandler(ViewItemSchema.SetSharedSizePolicy), false);
            UIXPropertySchema uixPropertySchema22 = new UIXPropertySchema((short)239, "Visible", (short)15, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetVisible), new SetValueHandler(ViewItemSchema.SetVisible), false);
            UIXPropertySchema uixPropertySchema23 = new UIXPropertySchema((short)239, "Background", (short)35, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetBackground), new SetValueHandler(ViewItemSchema.SetBackground), false);
            UIXPropertySchema uixPropertySchema24 = new UIXPropertySchema((short)239, "Camera", (short)21, (short)-1, ExpressionRestriction.None, false, (RangeValidator)null, true, new GetValueHandler(ViewItemSchema.GetCamera), new SetValueHandler(ViewItemSchema.SetCamera), false);
            UIXMethodSchema uixMethodSchema1 = new UIXMethodSchema((short)239, "AttachAnimation", new short[1]
            {
        (short) 104
            }, (short)240, new InvokeHandler(ViewItemSchema.CallAttachAnimationIAnimation), false);
            UIXMethodSchema uixMethodSchema2 = new UIXMethodSchema((short)239, "AttachAnimation", new short[2]
            {
        (short) 104,
        (short) 11
            }, (short)240, new InvokeHandler(ViewItemSchema.CallAttachAnimationIAnimationAnimationHandle), false);
            UIXMethodSchema uixMethodSchema3 = new UIXMethodSchema((short)239, "DetachAnimation", new short[1]
            {
        (short) 10
            }, (short)240, new InvokeHandler(ViewItemSchema.CallDetachAnimationAnimationEventType), false);
            UIXMethodSchema uixMethodSchema4 = new UIXMethodSchema((short)239, "PlayAnimation", new short[1]
            {
        (short) 104
            }, (short)240, new InvokeHandler(ViewItemSchema.CallPlayAnimationIAnimation), false);
            UIXMethodSchema uixMethodSchema5 = new UIXMethodSchema((short)239, "PlayAnimation", new short[2]
            {
        (short) 104,
        (short) 11
            }, (short)240, new InvokeHandler(ViewItemSchema.CallPlayAnimationIAnimationAnimationHandle), false);
            UIXMethodSchema uixMethodSchema6 = new UIXMethodSchema((short)239, "PlayAnimation", new short[1]
            {
        (short) 10
            }, (short)240, new InvokeHandler(ViewItemSchema.CallPlayAnimationAnimationEventType), false);
            UIXMethodSchema uixMethodSchema7 = new UIXMethodSchema((short)239, "ForceContentChange", (short[])null, (short)240, new InvokeHandler(ViewItemSchema.CallForceContentChange), false);
            UIXMethodSchema uixMethodSchema8 = new UIXMethodSchema((short)239, "SnapshotPosition", (short[])null, (short)171, new InvokeHandler(ViewItemSchema.CallSnapshotPosition), false);
            UIXMethodSchema uixMethodSchema9 = new UIXMethodSchema((short)239, "NavigateInto", (short[])null, (short)240, new InvokeHandler(ViewItemSchema.CallNavigateInto), false);
            UIXMethodSchema uixMethodSchema10 = new UIXMethodSchema((short)239, "NavigateInto", new short[1]
            {
        (short) 15
            }, (short)240, new InvokeHandler(ViewItemSchema.CallNavigateIntoBoolean), false);
            UIXMethodSchema uixMethodSchema11 = new UIXMethodSchema((short)239, "ScrollIntoView", (short[])null, (short)240, new InvokeHandler(ViewItemSchema.CallScrollIntoView), false);
            ViewItemSchema.Type.Initialize((DefaultConstructHandler)null, (ConstructorSchema[])null, new PropertySchema[24]
            {
        (PropertySchema) uixPropertySchema6,
        (PropertySchema) uixPropertySchema1,
        (PropertySchema) uixPropertySchema2,
        (PropertySchema) uixPropertySchema23,
        (PropertySchema) uixPropertySchema24,
        (PropertySchema) uixPropertySchema3,
        (PropertySchema) uixPropertySchema7,
        (PropertySchema) uixPropertySchema4,
        (PropertySchema) uixPropertySchema5,
        (PropertySchema) uixPropertySchema8,
        (PropertySchema) uixPropertySchema9,
        (PropertySchema) uixPropertySchema10,
        (PropertySchema) uixPropertySchema11,
        (PropertySchema) uixPropertySchema12,
        (PropertySchema) uixPropertySchema13,
        (PropertySchema) uixPropertySchema14,
        (PropertySchema) uixPropertySchema15,
        (PropertySchema) uixPropertySchema16,
        (PropertySchema) uixPropertySchema17,
        (PropertySchema) uixPropertySchema18,
        (PropertySchema) uixPropertySchema19,
        (PropertySchema) uixPropertySchema20,
        (PropertySchema) uixPropertySchema21,
        (PropertySchema) uixPropertySchema22
            }, new MethodSchema[11]
            {
        (MethodSchema) uixMethodSchema1,
        (MethodSchema) uixMethodSchema2,
        (MethodSchema) uixMethodSchema3,
        (MethodSchema) uixMethodSchema4,
        (MethodSchema) uixMethodSchema5,
        (MethodSchema) uixMethodSchema6,
        (MethodSchema) uixMethodSchema7,
        (MethodSchema) uixMethodSchema8,
        (MethodSchema) uixMethodSchema9,
        (MethodSchema) uixMethodSchema10,
        (MethodSchema) uixMethodSchema11
            }, (EventSchema[])null, (FindCanonicalInstanceHandler)null, (TypeConverterHandler)null, (SupportsTypeConversionHandler)null, (EncodeBinaryHandler)null, (DecodeBinaryHandler)null, (PerformOperationHandler)null, (SupportsOperationHandler)null);
        }

        internal class ListProxy : IList, ICollection, IEnumerable
        {
            private ViewItem _subject;
            private ViewItemSchema.ListProxyMode _mode;
            private static ViewItemSchema.ListProxy s_shared = new ViewItemSchema.ListProxy();

            public static IList GetChildren(ViewItem subject)
            {
                ViewItemSchema.ListProxy.s_shared.SetSubject(subject, ViewItemSchema.ListProxyMode.Children);
                return (IList)ViewItemSchema.ListProxy.s_shared;
            }

            public static IList GetAnimation(ViewItem subject)
            {
                ViewItemSchema.ListProxy.s_shared.SetSubject(subject, ViewItemSchema.ListProxyMode.Animation);
                return (IList)ViewItemSchema.ListProxy.s_shared;
            }

            public int Add(object value)
            {
                switch (this._mode)
                {
                    case ViewItemSchema.ListProxyMode.Children:
                        this._subject.Children.Add((Microsoft.Iris.Library.TreeNode)value);
                        break;
                    case ViewItemSchema.ListProxyMode.Animation:
                        if (value != null)
                        {
                            this._subject.AttachAnimation((IAnimationProvider)value);
                            break;
                        }
                        break;
                }
                this._subject = (ViewItem)null;
                return 0;
            }

            public void Clear() => throw new NotImplementedException();

            public bool Contains(object value) => throw new NotImplementedException();

            public int IndexOf(object value) => throw new NotImplementedException();

            public void Insert(int index, object value) => throw new NotImplementedException();

            public void Remove(object value) => throw new NotImplementedException();

            public void RemoveAt(int index) => throw new NotImplementedException();

            public bool IsFixedSize => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public object this[int index]
            {
                get => throw new NotImplementedException();
                set => throw new NotImplementedException();
            }

            public void CopyTo(Array array, int index) => throw new NotImplementedException();

            public int Count => throw new NotImplementedException();

            public bool IsSynchronized => throw new NotImplementedException();

            public object SyncRoot => throw new NotImplementedException();

            public IEnumerator GetEnumerator() => throw new NotImplementedException();

            private void SetSubject(ViewItem subject, ViewItemSchema.ListProxyMode mode)
            {
                this._subject = subject;
                this._mode = mode;
            }

            private ListProxy()
            {
            }
        }

        internal enum ListProxyMode
        {
            Children,
            Animation,
        }
    }
}
