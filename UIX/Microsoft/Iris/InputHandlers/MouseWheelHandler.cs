// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.InputHandlers.MouseWheelHandler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Markup;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.InputHandlers
{
    internal class MouseWheelHandler : InputHandler
    {
        private bool _handle;

        public MouseWheelHandler() => this._handle = true;

        protected override void ConfigureInteractivity()
        {
            base.ConfigureInteractivity();
            if (!this.HandleDirect)
                return;
            this.UI.MouseInteractive = true;
            this.UI.KeyInteractive = true;
        }

        public bool Handle
        {
            get => this._handle;
            set
            {
                if (this._handle == value)
                    return;
                this._handle = value;
                this.FireNotification(NotificationID.Handle);
            }
        }

        protected override void OnMouseWheel(UIClass ui, MouseWheelInfo info)
        {
            if (info.WheelDelta > 0)
                this.FireNotification(NotificationID.UpInvoked);
            else
                this.FireNotification(NotificationID.DownInvoked);
            if (!this._handle)
                return;
            info.MarkHandled();
        }
    }
}
