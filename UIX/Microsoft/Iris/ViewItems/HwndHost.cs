// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.HwndHost
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.ViewItems
{
    internal class HwndHost : ContentViewItem
    {
        private IHwndHostWindow _window;
        private Color _backgroundColor;
        private long _childWindowHwnd;
        private IntPtr _childHwndIntPtr;

        public HwndHost()
        {
            ((ITrackableUIElementEvents)this).UIChange += new EventHandler(this.OnUIChange);
            IRenderWindow renderWindow = UISession.Default.GetRenderWindow();
            this._window = renderWindow.CreateHwndHostWindow();
            this._window.OnHandleChanged += new EventHandler(this.OnHandleChanged);
            renderWindow.ForwardMessageEvent += new ForwardMessageHandler(this.OnForwardMessage);
            this._backgroundColor = Color.White;
            this._window.BackgroundColor = new ColorF(byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }

        protected override void OnDispose()
        {
            IRenderWindow renderWindow = UISession.Default.GetRenderWindow();
            if (renderWindow != null)
                renderWindow.ForwardMessageEvent -= new ForwardMessageHandler(this.OnForwardMessage);
            if (this._window != null)
            {
                this._window.OnHandleChanged -= new EventHandler(this.OnHandleChanged);
                this._window.Dispose();
                this._window = null;
            }
          ((ITrackableUIElementEvents)this).UIChange -= new EventHandler(this.OnUIChange);
            base.OnDispose();
        }

        public long Handle => this._window.Hwnd.ToInt64();

        public long ChildHandle
        {
            get => this._childWindowHwnd;
            set
            {
                if (value == this._childWindowHwnd)
                    return;
                this._childWindowHwnd = value;
                this._childHwndIntPtr = (IntPtr)this.ChildHandle;
            }
        }

        public override Color Background
        {
            get => this._backgroundColor;
            set
            {
                if (!(this._backgroundColor != value))
                    return;
                if (value.A != byte.MaxValue)
                {
                    ErrorManager.ReportError("HwndHost.Background must be a solid color");
                }
                else
                {
                    this._backgroundColor = value;
                    this._window.BackgroundColor = value.RenderConvert();
                    this.FireNotification(NotificationID.Background);
                }
            }
        }

        protected override bool HasContent() => true;

        private void OnHandleChanged(object sender, EventArgs args) => this.FireNotification(NotificationID.Handle);

        private unsafe void OnForwardMessage(uint message, IntPtr wParam, IntPtr lParam)
        {
            if (!(this._childHwndIntPtr != IntPtr.Zero))
                return;
            Win32Api.MSG msg;
            msg.message = message;
            msg.wParam = wParam;
            msg.lParam = lParam;
            Win32Api.MSG* msgPtr = &msg;
            Win32Api.SendMessage(this._childHwndIntPtr, 895U, IntPtr.Zero, (IntPtr)msgPtr);
        }

        private void OnUIChange(object sender, EventArgs args)
        {
            if (this._window == null || !this.IsZoned || !this.HasVisual)
                return;
            Vector3 parentOffsetPxlVector;
            Vector3 scaleVector;
            GetAccumulatedOffsetAndScale(this, null, out parentOffsetPxlVector, out scaleVector);
            Vector2 visualSize = this.VisualSize;
            this._window.ClientPosition = new Point((int)Math.Round(parentOffsetPxlVector.X), (int)Math.Round(parentOffsetPxlVector.Y));
            this._window.WindowSize = new Size(Math2.RoundUp(scaleVector.X * visualSize.X), Math2.RoundUp(scaleVector.Y * visualSize.Y));
            this._window.Visible = this.FullyVisible;
        }
    }
}
