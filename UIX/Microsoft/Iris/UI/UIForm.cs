// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.UI.UIForm
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Accessibility;
using Microsoft.Iris.Accessibility;
using Microsoft.Iris.Input;
using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI;
using Microsoft.Iris.Session;
using Microsoft.Iris.ViewItems;
using System;

namespace Microsoft.Iris.UI
{
    internal class UIForm : Form, INotifyObject
    {
        private DeferredHandler _initialLoadComplete;
        private string _initialSource;
        private Vector<UIPropertyRecord> _initialProperties;
        private NotifyService _notifier = new NotifyService();
        private NativeApi.NotifyWindowCallback _notificationCallback;
        private bool _showWindowFrame;
        private bool _showShadow;
        private bool _startCentered;
        private bool _startInWorkArea;
        private bool _respectStartupSettings;
        private bool _alwaysOnTop;
        private bool _showInTaskbar;
        private bool _preventInterruption;
        private MaximizeMode _maximizeMode;
        private bool _mouseIsIdle;
        private int _mouseIdleTimeout;
        private bool _hideMouseOnIdle;

        public UIForm(UISession session)
          : base(session)
        {
            session.InputManager.KeyFocusCanBeNull = true;
            session.InputManager.InvalidKeyFocus += new InvalidKeyFocusHandler(this.OnInvalidKeyFocus);
            this._showWindowFrame = true;
            this._alwaysOnTop = false;
            this._showInTaskbar = true;
            this._showShadow = false;
            this._startCentered = false;
            this._startInWorkArea = false;
            this._preventInterruption = false;
            this.UpdateStyles();
            this.SetWindowOptions(WindowOptions.FreeformResize, true);
            this._notificationCallback = new NativeApi.NotifyWindowCallback(this.OnNotifyCallback);
            IntPtr handle;
            RendererApi.IFC(NativeApi.SpCreateNotifyWindow(out handle, this._notificationCallback));
            this.AppNotifyWindow = handle;
        }

        private void OnInitialize()
        {
            UIZone newZone = new UIZone(this);
            newZone.DeclareOwner(this);
            this.AttachChildZone(newZone);
            newZone.RootViewItem.RequestSource(this._initialSource, this._initialProperties);
        }

        public string Caption
        {
            get => this.Text;
            set
            {
                if (!(value != this.Text))
                    return;
                this.Text = value;
                this.FireNotification(NotificationID.Caption);
            }
        }

        public bool ShowWindowFrame
        {
            get => this._showWindowFrame;
            set
            {
                if (this._showWindowFrame == value)
                    return;
                this._showWindowFrame = value;
                this.UpdateStyles();
                this.FireNotification(NotificationID.ShowWindowFrame);
            }
        }

        public bool ShowShadow
        {
            get => this._showShadow;
            set
            {
                if (this._showShadow == value)
                    return;
                this._showShadow = value;
                this.SetWindowOptions(WindowOptions.ShowFormShadow, value);
            }
        }

        public bool PreventInterruption
        {
            get => this._preventInterruption;
            set
            {
                if (this._preventInterruption == value)
                    return;
                this._preventInterruption = value;
                this.SetWindowOptions(WindowOptions.PreventInterruption, value);
            }
        }

        public MaximizeMode MaximizeMode
        {
            get => this._maximizeMode;
            set
            {
                if (this._maximizeMode == value)
                    return;
                this._maximizeMode = value;
                if (value == MaximizeMode.FullScreen)
                    this.SetWindowOptions(WindowOptions.MaximizeFullScreen, true);
                else
                    this.SetWindowOptions(WindowOptions.MaximizeFullScreen, false);
                this.FireNotification(NotificationID.MaximizeMode);
            }
        }

        public bool StartCentered
        {
            get => this._startCentered;
            set
            {
                if (this._startCentered == value)
                    return;
                this._startCentered = value;
                this.SetWindowOptions(WindowOptions.StartCentered, value);
            }
        }

        public bool StartInWorkArea
        {
            get => this._startInWorkArea;
            set
            {
                if (this._startInWorkArea == value)
                    return;
                this._startInWorkArea = value;
                this.SetWindowOptions(WindowOptions.StartInWorkArea, value);
            }
        }

        public bool RespectsStartupSettings
        {
            get => this._respectStartupSettings;
            set
            {
                if (this._respectStartupSettings == value)
                    return;
                this._respectStartupSettings = value;
                this.SetWindowOptions(WindowOptions.RespectStartupSettings, value);
            }
        }

        public bool AlwaysOnTop
        {
            get => this._alwaysOnTop;
            set
            {
                if (this._alwaysOnTop == value)
                    return;
                this._alwaysOnTop = value;
                this.UpdateStyles();
                this.FireNotification(NotificationID.AlwaysOnTop);
            }
        }

        public bool ShowInTaskbar
        {
            get => this._showInTaskbar;
            set
            {
                if (this._showInTaskbar == value)
                    return;
                this._showInTaskbar = value;
                this.UpdateStyles();
                this.FireNotification(NotificationID.ShowInTaskbar);
            }
        }

        public int MouseIdleTimeout
        {
            get => this._mouseIdleTimeout;
            set
            {
                this._mouseIdleTimeout = value;
                if (value != 0)
                {
                    this.SetMouseIdleOptions(new Size(2, 2), (uint)value);
                    this.SetWindowOptions(WindowOptions.TrackMouseIdle, true);
                }
                else
                    this.SetWindowOptions(WindowOptions.TrackMouseIdle, false);
                this.FireNotification(NotificationID.MouseIdleTimeout);
            }
        }

        public bool HideMouseOnIdle
        {
            get => this._hideMouseOnIdle;
            set
            {
                if (this._hideMouseOnIdle == value)
                    return;
                this._hideMouseOnIdle = value;
                this.SetWindowOptions(WindowOptions.MouseleaveOnIdle, value);
                if (value)
                    this.IdleCursor = CursorID.None;
                else
                    this.IdleCursor = CursorID.NotSpecified;
                this.FireNotification(NotificationID.HideMouseOnIdle);
            }
        }

        public void RequestLoad(string source, Vector<UIPropertyRecord> properties)
        {
            if (this.Zone == null)
            {
                this._initialSource = source;
                this._initialProperties = properties;
            }
            else
                this.Zone.RootViewItem.RequestSource(source, properties);
        }

        public SavedKeyFocus SaveKeyFocus() => this.Zone != null && this.Zone.RootUI != null ? new SavedKeyFocus(this.Zone.RootUI.SaveKeyFocus()) : null;

        public void RestoreKeyFocus(SavedKeyFocus state)
        {
            if (state == null)
                return;
            DeferredCall.Post(DispatchPriority.LayoutSync, new DeferredHandler(this.DeferredRestoreKeyFocus), state.Payload);
        }

        private void DeferredRestoreKeyFocus(object cookie)
        {
            if (this.Zone == null || this.Zone.RootUI == null)
                return;
            this.Zone.RootUI.RestoreKeyFocus(cookie);
        }

        private void UpdateStyles()
        {
            FormStyleInfo formStyleInfo = new FormStyleInfo();
            uint num1 = 100663296;
            uint num2 = !this.ShowInTaskbar ? 128U : 262144U;
            if (this.AlwaysOnTop)
                num2 |= 8U;
            formStyleInfo.uStyleFullscreen = num1;
            formStyleInfo.uExStyleFullscreen = num2;
            uint num3 = num1 | 720896U;
            uint num4 = !this.ShowWindowFrame ? num3 | 2147483648U : num3 | 12845056U;
            formStyleInfo.uStyleRestored = num4;
            formStyleInfo.uExStyleRestored = num2;
            formStyleInfo.uStyleMinimized = num4;
            formStyleInfo.uExStyleMinimized = num2;
            formStyleInfo.uStyleMaximized = num4;
            formStyleInfo.uExStyleMaximized = num2;
            this.Styles = formStyleInfo;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Graphic.EnsureFallbackImages();
            this.OnInitialize();
            this.Visible = true;
        }

        internal override void OnShow(bool fShow, bool fFirstShow)
        {
            base.OnShow(fShow, fFirstShow);
            if (!fShow || !fFirstShow)
                return;
            this.Session.InputManager.KeyFocusCanBeNull = false;
            if (this._initialLoadComplete == null)
                return;
            DeferredCall.Post(DispatchPriority.Idle, new SimpleCallback(this.DeliverIntialLoadCompleteCallback));
        }

        protected override void OnActivationChange()
        {
            this.FireNotification(NotificationID.Active);
            base.OnActivationChange();
        }

        protected override void OnWindowStateChanged(bool fUnplanned) => this.FireNotification(NotificationID.WindowState);

        protected override void OnLocationChanged(Point position) => this.FireNotification(NotificationID.Position);

        protected override void OnSizeChanged() => this.FireNotification(NotificationID.ClientSize);

        public bool MouseIsIdle => this._mouseIsIdle;

        protected override void OnMouseIdle(bool value)
        {
            if (this._mouseIsIdle != value)
            {
                this._mouseIsIdle = value;
                this.FireNotification(NotificationID.MouseActive);
            }
            base.OnMouseIdle(value);
        }

        private void DeliverIntialLoadCompleteCallback()
        {
            DeferredCall.Post(DispatchPriority.Idle, this._initialLoadComplete, null);
            this._initialLoadComplete = null;
        }

        public void SetInitialLoadCompleteCallback(DeferredHandler callback) => this._initialLoadComplete = callback;

        public void Close() => this.RequestClose(FormCloseReason.UserRequest);

        protected override void OnCloseRequest(FormCloseReason nReason)
        {
            bool block = false;
            if (this.CloseRequested != null)
                this.CloseRequested(nReason, ref block);
            if (block)
                return;
            base.OnCloseRequest(nReason);
        }

        public event FormCloseRequestedHandler CloseRequested;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.Zone.Dispose(this);
            this.Session.InputManager.InvalidKeyFocus -= new InvalidKeyFocusHandler(this.OnInvalidKeyFocus);
            this.Session.Dispatcher.StopCurrentMessageLoop();
            NativeApi.SpDestroyNotifyWindow();
        }

        private void OnInvalidKeyFocus(ICookedInputSite lastKnownFocused)
        {
            if (lastKnownFocused is UIClass uiClass)
            {
                UIClass focusableAncestor = uiClass.FindKeyFocusableAncestor();
                if (focusableAncestor != null)
                {
                    focusableAncestor.RequestKeyFocus();
                    return;
                }
            }
            this.SetDefaultKeyFocus();
        }

        protected override IntPtr OnAccGetObject(int wparam, int lparam)
        {
            AccessibleProxy.AccessibilityActive = true;
            AccObjectID accObjectId = (AccObjectID)lparam;
            object accPtr1 = null;
            if (accObjectId == AccObjectID.Client)
            {
                accPtr1 = Zone.RootUI.AccessibleProxy;
                RootAccessibleProxy rootAccessibleProxy = (RootAccessibleProxy)accPtr1;
                if (rootAccessibleProxy.ClientBridge == null)
                {
                    object accPtr2;
                    AccessibleProxy.CreateStdAccessibleObject(this.__WindowHandle, -4, AccessibleProxy.IID_IAccessible, out accPtr2);
                    rootAccessibleProxy.AttachClientBridge((IAccessible)accPtr2);
                }
            }
            else if (accObjectId > AccObjectID.Window)
                accPtr1 = AccessibleProxy.AccessibleProxyFromID((int)accObjectId);
            if (accPtr1 == null)
                return IntPtr.Zero;
            Guid iidIaccessible = AccessibleProxy.IID_IAccessible;
            return AccessibleProxy.LresultFromObject(ref iidIaccessible, (IntPtr)wparam, accPtr1);
        }

        private IntPtr OnNotifyCallback(
          NativeApi.NotificationType notification,
          int param1,
          int param2)
        {
            return notification == NativeApi.NotificationType.GetObject ? this.OnAccGetObject(param1, param2) : new IntPtr(0);
        }

        private void FireNotification(string id)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(id);
            this._notifier.Fire(id);
        }

        public event FormPropertyChangedHandler PropertyChanged;

        void INotifyObject.AddListener(Listener listener) => this._notifier.AddListener(listener);
    }
}
