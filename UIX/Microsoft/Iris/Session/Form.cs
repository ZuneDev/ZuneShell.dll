// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Session.Form
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.Input;
using Microsoft.Iris.Library;
using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.UI;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Iris.Session
{
    [SuppressUnmanagedCodeSecurity]
    internal class Form : IModalSession
    {
        private UISession m_session;
        private UIZone m_zone;
        private IRenderWindow m_renderWindow;
        private bool m_fCloseRequested;
        private bool m_fClosing;
        private Size m_sizeWindow;
        private bool _refreshFocusRequestedFlag;
        private readonly SimpleCallback _processRefreshFocus;
        private SmartMap m_mapShutdownHooks;
        private ushort m_nextShutdownHookId;

        public Form(UISession session)
        {
            this.m_session = session;
            this.m_renderWindow = session.GetRenderWindow();
            this.InternalWindow.LoadEvent += new LoadHandler(this.OnRenderWindowLoad);
            this.InternalWindow.ShowEvent += new ShowHandler(this.OnShow);
            this.InternalWindow.CloseRequestEvent += new CloseRequestHandler(this.OnRenderWindowCloseRequested);
            this.InternalWindow.CloseEvent += new CloseHandler(this.OnRenderWindowClose);
            this.InternalWindow.MouseIdleEvent += new MouseIdleHandler(this.OnRenderWindowMouseIdle);
            this.InternalWindow.SizeChangedEvent += new SizeChangedHandler(this.OnRenderWindowSizeChanged);
            this.InternalWindow.SysCommandEvent += new SysCommandHandler(this.OnRenderWindowSysCommand);
            this.InternalWindow.SetFocusEvent += new SetFocusHandler(this.OnRenderWindowSetFocus);
            this.InternalWindow.WindowStateChangedEvent += new WindowStateChangedHandler(this.OnWindowStateChanged);
            this.InternalWindow.LocationChangedEvent += new Microsoft.Iris.Render.LocationChangedHandler(this.OnLocationChanged);
            this.InternalWindow.MonitorChangedEvent += new MonitorChangedHandler(this.OnMonitorChanged);
            this.InternalWindow.ActivationChangeEvent += new ActivationChangeHandler(this.OnActivationChange);
            this.InternalWindow.SessionConnectEvent += new SessionConnectHandler(this.OnSessionConnect);
            this.InternalWindow.BackgroundColor = new ColorF(byte.MaxValue, 0, 0, 0);
            session.RegisterHost(this);
            this.m_mapShutdownHooks = new SmartMap();
            this.m_nextShutdownHookId = 1;
            this._processRefreshFocus = new SimpleCallback(this.ProcessRefreshFocus);
            DeferredCall.Post(DispatchPriority.Housekeeping, new SimpleCallback(this.InitializeWindow));
        }

        private void InitializeWindow() => this.InternalWindow.Initialize();

        private IRenderWindow InternalWindow => this.m_renderWindow;

        public FormPlacement FinalPlacement => this.InternalWindow.FinalPlacement;

        public FormPlacement InitialPlacement
        {
            set => this.InternalWindow.InitialPlacement = value;
        }

        public IntPtr __WindowHandle => this.InternalWindow.WindowHandle.h;

        public bool InvokeRequired => !UIDispatcher.IsUIThread;

        public UISession Session => this.m_session;

        public UIZone Zone => this.m_zone;

        internal void ClientToScreen(ref Point point)
        {
            Point point1 = new Point(point.X, point.Y);
            this.InternalWindow.ClientToScreen(ref point1);
            point.X = point1.X;
            point.Y = point1.Y;
        }

        internal IVisualContainer RootVisual => this.InternalWindow.VisualRoot;

        public Size ClientSize
        {
            get => new Size(this.InternalWindow.Width, this.InternalWindow.Height);
            set => this.InternalWindow.ClientSize = value;
        }

        public Size InitialClientSize
        {
            get
            {
                Size initialClientSize = this.InternalWindow.InitialClientSize;
                return new Size(initialClientSize.Width, initialClientSize.Height);
            }
            set => this.InternalWindow.InitialClientSize = value;
        }

        public Point Position
        {
            get => new Point(this.InternalWindow.Left, this.InternalWindow.Top);
            set => this.InternalWindow.Position = value;
        }

        public string Text
        {
            get => this.InternalWindow.Text;
            set => this.InternalWindow.Text = value;
        }

        public CursorID Cursor
        {
            get => this.InternalWindow.Cursor.CursorID;
            set => this.InternalWindow.Cursor = Input.Cursor.GetCursor(value);
        }

        public CursorID IdleCursor
        {
            get => this.InternalWindow.IdleCursor.CursorID;
            set => this.InternalWindow.IdleCursor = Input.Cursor.GetCursor(value);
        }

        public bool Visible
        {
            get => this.InternalWindow.Visible;
            set
            {
                if (this.m_zone != null)
                    this.m_zone.SetPhysicalVisible(value);
                this.InternalWindow.Visible = value;
            }
        }

        public bool ActivationState => this.InternalWindow.ActivationState;

        public void TakeFocus() => this.InternalWindow.TakeFocus();

        public Microsoft.Iris.WindowState WindowState
        {
            get => (Microsoft.Iris.WindowState)this.InternalWindow.WindowState;
            set => this.InternalWindow.WindowState = (Microsoft.Iris.Render.WindowState)value;
        }

        public FormStyleInfo Styles
        {
            get => this.InternalWindow.Styles;
            set => this.InternalWindow.Styles = value;
        }

        public bool IsClosing => this.m_fClosing;

        public IntPtr AppNotifyWindow
        {
            set => this.InternalWindow.AppNotifyWindow = new HWND(value);
        }

        public void BeginDynamicResize(Size sizeLargestExpectedPxl)
        {
        }

        public void EndDynamicResize()
        {
        }

        protected void SetWindowOptions(WindowOptions options, bool enable) => this.InternalWindow.SetWindowOptions(options, enable);

        protected void SetMouseIdleOptions(Size mouseIdleTolerance, uint mouseIdleDelay) => this.InternalWindow.SetMouseIdleOptions(mouseIdleTolerance, mouseIdleDelay);

        public bool IsDragInProgress
        {
            get => this.InternalWindow.IsDragInProgress;
            set => this.InternalWindow.IsDragInProgress = value;
        }

        internal void RefreshHitTarget() => this.InternalWindow.RefreshHitTarget();

        public event EventHandler NativeSetFocus;

        protected virtual void OnNativeSetFocus(object sender, Form.NativeSetFocusEventArgs args)
        {
            if (this.NativeSetFocus == null)
                return;
            this.NativeSetFocus(sender, args);
        }

        public bool SetDefaultKeyFocus()
        {
            bool flag = false;
            if (this.m_zone != null)
                flag = this.m_zone.OnInboundKeyNavigation(Direction.Next, RectangleF.Zero, true);
            return flag;
        }

        public void RequestClose(FormCloseReason nReason)
        {
            if (this.m_fClosing)
                return;
            if (this.m_fCloseRequested)
                return;
            try
            {
                this.m_fCloseRequested = true;
                this.OnCloseRequest(nReason);
            }
            finally
            {
                this.m_fCloseRequested = false;
            }
        }

        public void ForceClose() => this.ForceCloseWorker(FormCloseReason.ForcedClose);

        private void ForceCloseWorker(FormCloseReason nReason)
        {
            if (this.m_fClosing)
                return;
            this.m_fClosing = true;
            this.InternalWindow.Close(FormCloseReason.UserRequest);
        }

        private void OnRenderWindowClose()
        {
            this.m_fClosing = true;
            this.OnDestroy();
        }

        public void ForceMouseIdle(bool fIdle) => this.InternalWindow.ForceMouseIdle(fIdle);

        internal void SetCapture(IRawInputSite captureSite, bool state) => this.InternalWindow.SetCapture(captureSite, state);

        public void SetBackgroundColor(Color color) => this.InternalWindow.BackgroundColor = color.RenderConvert();

        public void SetIcon(string sModuleName, uint nResourceID, IconFlags nOptions) => this.InternalWindow.SetIcon(sModuleName, nResourceID, nOptions);

        public void SetShadowImages(bool fActiveEdges, UIImage[] images)
        {
            ShadowEdgePart[] edges = new ShadowEdgePart[4];
            for (int index = 0; index < 4; ++index)
            {
                string host;
                string identifier;
                Inset nineGrid;
                this.ShadowEdgeResourceFromImage(images[index], out host, out identifier, out nineGrid);
                edges[index].ModuleName = host;
                edges[index].ResourceName = identifier;
                edges[index].SplitPoints = nineGrid;
            }
            this.InternalWindow.SetEdgeImages(fActiveEdges, edges);
        }

        internal void ShadowEdgeResourceFromImage(
          UIImage uiimage,
          out string host,
          out string identifier,
          out Inset nineGrid)
        {
            host = null;
            identifier = null;
            nineGrid = new Inset();
            if (!(uiimage is UriImage uriImage))
                return;
            nineGrid = uriImage.NineGrid;
            string source = uriImage.Source;
            if (source == null)
                return;
            string hierarchicalPart;
            ResourceManager.ParseUri(source, out string _, out hierarchicalPart);
            DllResources.ParseResource(hierarchicalPart, out host, out identifier);
        }

        public bool EnableExternalDragDrop
        {
            get => this.InternalWindow.EnableExternalDragDrop;
            set => this.InternalWindow.EnableExternalDragDrop = value;
        }

        public void SetDragDropResult(uint nDragOverResult, uint nDragDropResult) => this.InternalWindow.SetDragDropResult(nDragOverResult, nDragDropResult);

        public void TemporarilyExitExclusiveMode() => this.InternalWindow.TemporarilyExitExclusiveMode();

        public void EnableShellShutdownHook(string hookName, EventHandler handler) => this.GetShutdownHookInfo(hookName, true).Handler += handler;

        protected virtual void OnLocationChanged(Point position)
        {
            Form.LocationChangedHandler locationChangedEvent = this.OnLocationChangedEvent;
            if (locationChangedEvent == null)
                return;
            Form.LocationChangedArgs args = new Form.LocationChangedArgs(position);
            locationChangedEvent(args);
        }

        public event Form.LocationChangedHandler OnLocationChangedEvent;

        protected virtual void OnSizeChanged()
        {
        }

        protected virtual void OnMonitorChanged()
        {
        }

        protected virtual void OnWindowStateChanged(bool fUnplanned)
        {
        }

        protected virtual void OnCloseRequest(FormCloseReason nReason) => this.ForceCloseWorker(nReason);

        protected virtual void OnDestroy()
        {
            if (this.m_renderWindow == null)
                return;
            this.InternalWindow.LoadEvent -= new LoadHandler(this.OnRenderWindowLoad);
            this.InternalWindow.ShowEvent -= new ShowHandler(this.OnShow);
            this.InternalWindow.CloseRequestEvent -= new CloseRequestHandler(this.OnRenderWindowCloseRequested);
            this.InternalWindow.CloseEvent -= new CloseHandler(this.OnRenderWindowClose);
            this.InternalWindow.MouseIdleEvent -= new MouseIdleHandler(this.OnRenderWindowMouseIdle);
            this.InternalWindow.SizeChangedEvent -= new SizeChangedHandler(this.OnRenderWindowSizeChanged);
            this.InternalWindow.SysCommandEvent -= new SysCommandHandler(this.OnRenderWindowSysCommand);
            this.InternalWindow.SetFocusEvent -= new SetFocusHandler(this.OnRenderWindowSetFocus);
            this.InternalWindow.WindowStateChangedEvent -= new WindowStateChangedHandler(this.OnWindowStateChanged);
            this.InternalWindow.LocationChangedEvent -= new Microsoft.Iris.Render.LocationChangedHandler(this.OnLocationChanged);
            this.InternalWindow.MonitorChangedEvent -= new MonitorChangedHandler(this.OnMonitorChanged);
            this.InternalWindow.ActivationChangeEvent -= new ActivationChangeHandler(this.OnActivationChange);
            this.InternalWindow.SessionConnectEvent -= new SessionConnectHandler(this.OnSessionConnect);
        }

        protected virtual void OnSysCommand(IntPtr uParam1, IntPtr uParam2)
        {
        }

        protected virtual void OnMouseIdle(bool fIdle)
        {
            if (this.m_session == null)
                return;
            UI.Environment.Instance.SetIsMouseActive(!fIdle);
        }

        internal virtual void OnShow(bool fShow, bool fFirstShow)
        {
        }

        public event EventHandler ActivationChange;

        protected virtual void OnActivationChange()
        {
            if (this.ActivationChange == null)
                return;
            this.ActivationChange(this, EventArgs.Empty);
        }

        protected virtual void OnLoad()
        {
        }

        private void OnSessionConnect(bool fIsConnected)
        {
            if (this.SessionConnect == null)
                return;
            this.SessionConnect(this, fIsConnected);
        }

        public event FormSessionConnectHandler SessionConnect;

        public IntPtr FireAccGetObject(int iFlags, int iObjectID) => this.OnAccGetObject(iFlags, iObjectID);

        protected virtual IntPtr OnAccGetObject(int iFlags, int iObjectID) => IntPtr.Zero;

        public void BringToTop() => this.InternalWindow.BringToTop();

        public void LockMouseActive(bool fLock) => this.InternalWindow.LockMouseActive(fLock);

        internal void ScreenToClient(ref Point point)
        {
            Point point1 = new Point(point.X, point.Y);
            this.InternalWindow.ScreenToClient(ref point1);
            point.X = point1.X;
            point.Y = point1.Y;
        }

        internal void NotifyWinEvent(int idEvent, int idObject, int idChild) => NotifyWinEvent(idEvent, this.__WindowHandle, idObject, idChild);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern void NotifyWinEvent(int idEvent, IntPtr hwnd, int idObject, int idChild);

        internal void AttachChildZone(UIZone newZone)
        {
            this.m_zone = newZone;
            this.m_zone.ResizeRootContainer(this.InitialClientSize);
            this.m_zone.SetPhysicalVisible(this.Visible);
        }

        private void OnRenderWindowSysCommand(IntPtr wParam, IntPtr lParam) => this.OnSysCommand(wParam, lParam);

        private void OnRenderWindowMouseIdle(bool fNewIdle) => this.OnMouseIdle(fNewIdle);

        private void OnRenderWindowCloseRequested() => this.RequestClose(FormCloseReason.RendererRequest);

        private void OnRenderWindowLoad()
        {
            if (this.m_fClosing)
                return;
            this.OnLoad();
        }

        private void OnRenderWindowSizeChanged()
        {
            Size clientSize = this.m_renderWindow.ClientSize;
            if (!(clientSize != this.m_sizeWindow))
                return;
            this.m_sizeWindow = clientSize;
            if (this.m_renderWindow.WindowState == Render.WindowState.Minimized)
                return;
            if (this.m_zone != null)
                this.m_zone.ResizeRootContainer(clientSize);
            this.OnSizeChanged();
            this.m_session.RequestUpdateView(true);
        }

        private void OnRenderWindowSetFocus(bool focused)
        {
            if (focused && !this._refreshFocusRequestedFlag)
            {
                DeferredCall.Post(DispatchPriority.Idle, this._processRefreshFocus);
                this._refreshFocusRequestedFlag = true;
            }
            this.m_session.InputManager.Keyboard.Reset();
        }

        private void ProcessRefreshFocus()
        {
            if (!this._refreshFocusRequestedFlag)
                return;
            this._refreshFocusRequestedFlag = false;
            if (!(this.m_session.InputManager.Queue.InstantaneousKeyFocus is IInputCustomFocus instantaneousKeyFocus))
                return;
            instantaneousKeyFocus.OverrideHostFocus();
        }

        bool IModalSession.IsModalAllowed => !this.IsClosing;

        private Form.ShutdownHookInfo GetShutdownHookInfo(string hookName, bool fCanAdd)
        {
            Form.ShutdownHookInfo shutdownHookInfo = new Form.ShutdownHookInfo(hookName);
            uint key;
            if (this.m_mapShutdownHooks.Lookup(shutdownHookInfo, out key))
                shutdownHookInfo = (Form.ShutdownHookInfo)this.m_mapShutdownHooks[key];
            else if (fCanAdd)
                this.m_mapShutdownHooks[this.m_nextShutdownHookId++] = shutdownHookInfo;
            else
                shutdownHookInfo = null;
            return shutdownHookInfo;
        }

        public bool IsPreProcessedInput => false;

        public class LocationChangedArgs : EventArgs
        {
            private Point m_position;

            public LocationChangedArgs(Point position) => this.m_position = position;

            public Point Position => this.m_position;
        }

        public delegate void LocationChangedHandler(Form.LocationChangedArgs args);

        public class NativeSetFocusEventArgs : EventArgs
        {
            private bool _focused;
            private IntPtr _hwndFocusChange;

            public NativeSetFocusEventArgs(bool focused, IntPtr hwndFocusChange)
            {
                this._focused = focused;
                this._hwndFocusChange = hwndFocusChange;
            }

            public bool Focused => this._focused;

            public IntPtr HwndFocusChange => this._hwndFocusChange;
        }

        private class ShutdownHookInfo
        {
            private string m_stHookId;

            public ShutdownHookInfo(string stHookId) => this.m_stHookId = stHookId;

            public event EventHandler Handler;

            public void OnHook(object sender, EventArgs args)
            {
                if (this.Handler == null)
                    return;
                this.Handler(sender, args);
            }

            public override bool Equals(object obj) => obj is Form.ShutdownHookInfo shutdownHookInfo && this.m_stHookId == shutdownHookInfo.m_stHookId;

            public override int GetHashCode() => this.m_stHookId.GetHashCode();
        }
    }
}
