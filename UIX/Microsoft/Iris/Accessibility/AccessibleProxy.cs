// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Accessibility.AccessibleProxy
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Accessibility;
using Microsoft.Iris.Navigation;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;

namespace Microsoft.Iris.Accessibility
{
    [ComVisible(true)]
    [SuppressUnmanagedCodeSecurity]
    public class AccessibleProxy : Microsoft.Iris.OS.CLR.StandardOleMarshalObject, IAccessible, IEnumVARIANT
    {
        internal static readonly Guid IID_IAccessible = new Guid("618736E0-3C3D-11CF-810C-00AA00389B71");
        private UIClass _ui;
        private Accessible _data;
        private IEnumVARIANT _children;
        private int _proxyID = -1;
        private static int s_proxyIDAllocator = 0;
        private static Map<int, AccessibleProxy> s_proxyFromID = new Map<int, AccessibleProxy>();
        private static DeferredHandler s_notifyEventHandler = new DeferredHandler(AccessibleProxy.NotifyEvent);
        private static bool s_accessibilityActive;

        internal AccessibleProxy(UIClass ui, Accessible data)
        {
            this._ui = ui;
            this._data = data;
            this._data.Attach(this);
            this._children = new AccessibleChildren(this);
        }

        internal UIClass UI => this._ui;

        internal void Detach()
        {
            this._data.Detach();
            this._ui = null;
            if (this._proxyID == -1)
                return;
            AccessibleProxy.s_proxyFromID.Remove(this._proxyID);
        }

        internal static bool AccessibilityActive
        {
            get => AccessibleProxy.s_accessibilityActive;
            set => AccessibleProxy.s_accessibilityActive = true;
        }

        internal virtual IAccessible Parent => this._ui.Parent != null ? _ui.Parent.AccessibleProxy : null;

        internal int ChildCount => this._ui.Children.Count;

        internal virtual string Name
        {
            get => this._data.Name;
            set => this._data.Name = value;
        }

        internal string Value
        {
            get => this._data.Value;
            set => this._data.Value = value;
        }

        internal string Description => this._data.Description;

        internal AccRole Role => this._data.Role;

        internal AccStates State
        {
            get
            {
                AccStates accStates = AccStates.None;
                if (this._ui.DirectKeyFocus)
                    accStates |= AccStates.Focused;
                if (this._ui.IsKeyFocusable())
                    accStates |= AccStates.Focusable;
                if (this._ui.DirectMouseFocus)
                    accStates |= AccStates.HotTracked;
                if (!this._ui.Visible)
                    accStates |= AccStates.Invisible;
                if (this._ui.Host != null && this._ui.Host.IsOffscreen)
                    accStates |= AccStates.OffScreen;
                if (this._data.HasPopup)
                    accStates |= AccStates.HasPopup;
                if (this._data.IsAnimated)
                    accStates |= AccStates.Animated;
                if (this._data.IsBusy)
                    accStates |= AccStates.Busy;
                if (this._data.IsChecked)
                    accStates |= AccStates.Checked;
                if (this._data.IsCollapsed)
                    accStates |= AccStates.Collapsed;
                if (this._data.IsDefault)
                    accStates |= AccStates.Default;
                if (this._data.IsExpanded)
                    accStates |= AccStates.Expanded;
                if (this._data.IsMarquee)
                    accStates |= AccStates.Marquee;
                if (this._data.IsMixed)
                    accStates |= AccStates.Mixed;
                if (this._data.IsMultiSelectable)
                    accStates |= AccStates.MultiSelectable;
                if (this._data.IsPressed)
                    accStates |= AccStates.Pressed;
                if (this._data.IsProtected)
                    accStates |= AccStates.Protected;
                if (this._data.IsSelectable)
                    accStates |= AccStates.Selectable;
                if (this._data.IsSelected)
                    accStates |= AccStates.Selected;
                if (this._data.IsTraversed)
                    accStates |= AccStates.Traversed;
                if (this._data.IsUnavailable)
                    accStates |= AccStates.Unavailable;
                return accStates;
            }
        }

        internal string Help => this._data.Help;

        internal int HelpTopic => this._data.HelpTopic;

        internal string KeyboardShortcut => this._data.KeyboardShortcut;

        internal bool HasFocus => this._ui.DirectKeyFocus;

        internal string DefaultAction => this._data.DefaultAction;

        internal Rectangle Location
        {
            get
            {
                Rectangle rectangle = Rectangle.Zero;
                Vector3 positionPxlVector;
                Vector3 sizePxlVector;
                if (this._ui.RootItem != null && ((INavigationSite)this._ui.RootItem).ComputeBounds(out positionPxlVector, out sizePxlVector))
                {
                    rectangle = new Rectangle((int)positionPxlVector.X, (int)positionPxlVector.Y, (int)sizePxlVector.X, (int)sizePxlVector.Y);
                    Point location = rectangle.Location;
                    UISession.Default.Form.ClientToScreen(ref location);
                    rectangle.Location = location;
                }
                return rectangle;
            }
        }

        internal virtual IAccessible Navigate(AccNavDirs navDir)
        {
            UIClass resultUI = null;
            switch (navDir)
            {
                case AccNavDirs.Up:
                    this._ui.FindNextFocusablePeer(Direction.North, RectangleF.Zero, out resultUI);
                    break;
                case AccNavDirs.Down:
                    this._ui.FindNextFocusablePeer(Direction.South, RectangleF.Zero, out resultUI);
                    break;
                case AccNavDirs.Left:
                    this._ui.FindNextFocusablePeer(Direction.West, RectangleF.Zero, out resultUI);
                    break;
                case AccNavDirs.Right:
                    this._ui.FindNextFocusablePeer(Direction.East, RectangleF.Zero, out resultUI);
                    break;
                case AccNavDirs.Next:
                    resultUI = (UIClass)this._ui.NextSibling;
                    break;
                case AccNavDirs.Previous:
                    resultUI = (UIClass)this._ui.PreviousSibling;
                    break;
                case AccNavDirs.FirstChild:
                    resultUI = (UIClass)this._ui.FirstChild;
                    break;
                case AccNavDirs.LastChild:
                    resultUI = (UIClass)this._ui.LastChild;
                    break;
            }
            return resultUI != null ? resultUI.AccessibleProxy : null;
        }

        internal void DoDefaultAction()
        {
            if (this._data.DefaultActionCommand == null)
                return;
            this._data.DefaultActionCommand.Invoke();
        }

        internal static void NotifyCreated(UIClass ui)
        {
            if (!AccessibleProxy.AccessibilityActive)
                return;
            ui.AccessibleProxy.QueueNotifyEvent(AccEvents.ObjectCreate);
        }

        internal static void NotifyDestroyed(UIClass ui)
        {
            if (!AccessibleProxy.AccessibilityActive)
                return;
            ui.AccessibleProxy.QueueNotifyEvent(AccEvents.ObjectDestroy);
        }

        internal static void NotifyTreeChanged(UIClass ui)
        {
            if (!AccessibleProxy.AccessibilityActive || !ui.Initialized)
                return;
            ui.AccessibleProxy.QueueNotifyEvent(AccEvents.ObjectReorder);
        }

        internal static void NotifyVisibilityChange(UIClass ui, bool visible)
        {
            if (!AccessibleProxy.AccessibilityActive || !ui.Initialized)
                return;
            ui.AccessibleProxy.QueueNotifyEvent(visible ? AccEvents.ObjectShow : AccEvents.ObjectHide);
        }

        internal static void NotifyFocus(UIClass ui)
        {
            if (!AccessibleProxy.AccessibilityActive || !ui.Initialized)
                return;
            ui.AccessibleProxy.QueueNotifyEvent(AccEvents.ObjectFocus);
        }

        internal void NotifyAccessiblePropertyChanged(AccessibleProperty property)
        {
            if (!this._ui.Initialized)
                return;
            AccEvents eventType = AccEvents.None;
            switch (property)
            {
                case AccessibleProperty.DefaultAction:
                    eventType = AccEvents.ObjectDefaultActionChange;
                    break;
                case AccessibleProperty.Description:
                    eventType = AccEvents.ObjectDescriptionChange;
                    break;
                case AccessibleProperty.HasPopup:
                case AccessibleProperty.IsAnimated:
                case AccessibleProperty.IsBusy:
                case AccessibleProperty.IsChecked:
                case AccessibleProperty.IsCollapsed:
                case AccessibleProperty.IsDefault:
                case AccessibleProperty.IsExpanded:
                case AccessibleProperty.IsMarquee:
                case AccessibleProperty.IsMixed:
                case AccessibleProperty.IsMultiSelectable:
                case AccessibleProperty.IsPressed:
                case AccessibleProperty.IsProtected:
                case AccessibleProperty.IsSelectable:
                case AccessibleProperty.IsTraversed:
                case AccessibleProperty.IsUnavailable:
                    eventType = AccEvents.ObjectStateChange;
                    break;
                case AccessibleProperty.Help:
                    eventType = AccEvents.ObjectHelpChange;
                    break;
                case AccessibleProperty.HelpTopic:
                    eventType = AccEvents.ObjectHelpChange;
                    break;
                case AccessibleProperty.IsSelected:
                    if (this._data.IsSelected)
                    {
                        eventType = AccEvents.ObjectSelection;
                        break;
                    }
                    break;
                case AccessibleProperty.KeyboardShortcut:
                    eventType = AccEvents.ObjectAcceleratorChange;
                    break;
                case AccessibleProperty.Name:
                    eventType = AccEvents.ObjectNameChange;
                    break;
                case AccessibleProperty.Value:
                    eventType = AccEvents.ObjectValueChange;
                    break;
            }
            if (eventType == AccEvents.None)
                return;
            this.QueueNotifyEvent(eventType);
        }

        private void QueueNotifyEvent(AccEvents eventType)
        {
            object obj = new object[2]
            {
         this,
         (int) eventType
            };
            DeferredCall.Post(DispatchPriority.AppEvent, AccessibleProxy.s_notifyEventHandler, obj);
        }

        private static void NotifyEvent(object payload)
        {
            object[] objArray = (object[])payload;
            AccessibleProxy accessibleProxy = (AccessibleProxy)objArray[0];
            AccEvents accEvents = (AccEvents)objArray[1];
            int proxyId = accessibleProxy.ProxyID;
            UISession.Default.Form.NotifyWinEvent((int)accEvents, proxyId, 0);
            if (accEvents != AccEvents.ObjectDestroy)
                return;
            accessibleProxy.Detach();
        }

        object IAccessible.accParent
        {
            get
            {
                this.VerifyProxyAccess();
                return Parent;
            }
        }

        object IAccessible.get_accChild(object varChild)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            return this;
        }

        int IAccessible.accChildCount
        {
            get
            {
                this.VerifyProxyAccess();
                return this.ChildCount;
            }
        }

        string IAccessible.get_accName(object varChild)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            return this.Name;
        }

        string IAccessible.get_accValue(object varChild)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            return this.Value;
        }

        string IAccessible.get_accDescription(object varChild)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            return this.Description;
        }

        object IAccessible.get_accRole(object varChild)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            return (int)this.Role;
        }

        object IAccessible.get_accState(object varChild)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            return (int)this.State;
        }

        string IAccessible.get_accHelp(object varChild)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            return this.Help;
        }

        int IAccessible.get_accHelpTopic(out string pszHelpFile, object varChild)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            pszHelpFile = null;
            return this.HelpTopic;
        }

        string IAccessible.get_accKeyboardShortcut(object varChild)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            return this.KeyboardShortcut;
        }

        object IAccessible.accFocus
        {
            get
            {
                this.VerifyProxyAccess();
                return this.HasFocus ? 0 : (object)null;
            }
        }

        object IAccessible.accSelection
        {
            get
            {
                this.VerifyProxyAccess();
                Marshal.ThrowExceptionForHR(-2147467263);
                return null;
            }
        }

        string IAccessible.get_accDefaultAction(object varChild)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            return this.DefaultAction;
        }

        void IAccessible.accSelect(int flagsSelect, object varChild)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            Marshal.ThrowExceptionForHR(-2147467263);
        }

        void IAccessible.accLocation(
          out int pxLeft,
          out int pyTop,
          out int pcxWidth,
          out int pcyHeight,
          object varChild)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            Rectangle location = this.Location;
            pxLeft = location.X;
            pyTop = location.Y;
            pcxWidth = location.Width;
            pcyHeight = location.Height;
        }

        object IAccessible.accNavigate(int navDir, object varStart)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varStart);
            return this.Navigate((AccNavDirs)navDir);
        }

        object IAccessible.accHitTest(int xLeft, int yTop)
        {
            this.VerifyProxyAccess();
            Marshal.ThrowExceptionForHR(-2147467263);
            return null;
        }

        void IAccessible.accDoDefaultAction(object varChild)
        {
            this.VerifyProxyAccess();
            this.DoDefaultAction();
        }

        void IAccessible.set_accName(object varChild, string pszName)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            this.Name = pszName;
        }

        void IAccessible.set_accValue(object varChild, string pszValue)
        {
            this.VerifyProxyAccess();
            this.VerifySelfChildID(varChild);
            this.Value = pszValue;
        }

        IEnumVARIANT IEnumVARIANT.Clone()
        {
            this.VerifyProxyAccess();
            return this._children.Clone();
        }

        int IEnumVARIANT.Next(int celt, object[] rgVar, IntPtr pceltFetched)
        {
            this.VerifyProxyAccess();
            return this._children.Next(celt, rgVar, pceltFetched);
        }

        int IEnumVARIANT.Reset()
        {
            this.VerifyProxyAccess();
            return this._children.Reset();
        }

        int IEnumVARIANT.Skip(int celt)
        {
            this.VerifyProxyAccess();
            return this._children.Skip(celt);
        }

        private void VerifyProxyAccess()
        {
            if (this._ui != null && UIDispatcher.IsUIThread)
                return;
            Marshal.ThrowExceptionForHR(-2147467259);
        }

        private void VerifySelfChildID(object varChild)
        {
            if (varChild is int && (AccChildID)varChild == AccChildID.Self)
                return;
            Marshal.ThrowExceptionForHR(-2147024809);
        }

        private int ProxyID
        {
            get
            {
                if (this._proxyID == -1)
                {
                    this._proxyID = ++AccessibleProxy.s_proxyIDAllocator;
                    AccessibleProxy.s_proxyFromID[this._proxyID] = this;
                }
                return this._proxyID;
            }
        }

        internal static AccessibleProxy AccessibleProxyFromID(int proxyID)
        {
            AccessibleProxy accessibleProxy;
            AccessibleProxy.s_proxyFromID.TryGetValue(proxyID, out accessibleProxy);
            return accessibleProxy;
        }

        [DllImport("oleacc.dll")]
        internal static extern IntPtr LresultFromObject(
          ref Guid riid,
          IntPtr wParam,
          [MarshalAs(UnmanagedType.Interface)] object accPtr);

        [DllImport("oleacc.dll")]
        internal static extern int CreateStdAccessibleObject(
          IntPtr hwnd,
          int objectID,
          [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
          [MarshalAs(UnmanagedType.Interface)] out object accPtr);
    }
}
