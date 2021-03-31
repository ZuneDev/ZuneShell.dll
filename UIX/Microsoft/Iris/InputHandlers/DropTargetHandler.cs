// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.InputHandlers.DropTargetHandler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.InputHandlers
{
    internal class DropTargetHandler : InputHandler
    {
        private bool _dragging;
        private DropAction _allowedDropActions;
        private WeakReference _eventContext;

        internal DropTargetHandler() => UISession.Default.Form.EnableExternalDragDrop = true;

        protected override void ConfigureInteractivity()
        {
            base.ConfigureInteractivity();
            if (!this.HandleDirect)
                return;
            this.UI.MouseInteractive = true;
        }

        public DropAction AllowedDropActions
        {
            get => this._allowedDropActions;
            set
            {
                if (this._allowedDropActions == value)
                    return;
                this._allowedDropActions = value;
                this.FireNotification(NotificationID.AllowedDropActions);
                if (!this.Dragging)
                    return;
                DragDropHelper.OnAllowedDropActionsChanged();
            }
        }

        public bool Dragging => this._dragging;

        public object EventContext => this.CheckEventContext(ref this._eventContext);

        protected override void OnDragEnter(UIClass ui, DragDropInfo info)
        {
            this._dragging = true;
            DragDropHelper.TargetHandler = this;
            this.FireNotification(NotificationID.Dragging);
            this.FireNotification(NotificationID.DragEnter);
            this.SetEventContext(info.Target, ref this._eventContext, NotificationID.EventContext);
            info.MarkHandled();
            base.OnDragEnter(ui, info);
        }

        protected override void OnDragOver(UIClass ui, DragDropInfo info)
        {
            if (this.Dragging)
            {
                this.FireNotification(NotificationID.DragOver);
                info.MarkHandled();
            }
            base.OnDragOver(ui, info);
        }

        protected override void OnDragLeave(UIClass ui, DragDropInfo info)
        {
            if (this.Dragging)
            {
                this.EndDrag(NotificationID.DragLeave);
                info.MarkHandled();
                this.SetEventContext((ICookedInputSite)null, ref this._eventContext, NotificationID.EventContext);
            }
            base.OnDragLeave(ui, info);
        }

        protected override void OnDropped(UIClass ui, DragDropInfo info)
        {
            if (this.Dragging)
            {
                this.EndDrag(NotificationID.Dropped);
                info.MarkHandled();
            }
            base.OnDropped(ui, info);
        }

        private void EndDrag(string eventName)
        {
            this._dragging = false;
            this.FireNotification(NotificationID.Dragging);
            this.FireNotification(eventName);
            DragDropHelper.TargetHandler = (DropTargetHandler)null;
        }

        public object GetValue() => DragDropHelper.GetValue();
    }
}
