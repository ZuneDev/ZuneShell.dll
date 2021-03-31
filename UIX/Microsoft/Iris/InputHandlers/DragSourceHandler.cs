// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.InputHandlers.DragSourceHandler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.InputHandlers
{
    internal class DragSourceHandler : InputHandler
    {
        private object _value;
        private DropAction _allowedDropActions;
        private DropAction _currentDropAction;
        private bool _pendingDrag;
        private bool _dragCanceled;
        private int _initialX;
        private int _initialY;
        private CursorID _moveCursor;
        private CursorID _copyCursor;
        private CursorID _cancelCursor;

        internal DragSourceHandler()
        {
            this._moveCursor = CursorID.Move;
            this._copyCursor = CursorID.Copy;
            this._cancelCursor = CursorID.Cancel;
        }

        public override void OnZoneDetached()
        {
            if (this.Dragging)
                this.EndDrag(null, InputModifiers.None, DropAction.None);
            base.OnZoneDetached();
        }

        protected override void ConfigureInteractivity()
        {
            base.ConfigureInteractivity();
            if (!this.HandleDirect)
                return;
            this.UI.MouseInteractive = true;
        }

        public object Value
        {
            get => this._value;
            set
            {
                if (this._value == value)
                    return;
                this._value = value;
                this.FireNotification(NotificationID.Value);
            }
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
            }
        }

        public DropAction CurrentDropAction => this._currentDropAction;

        private void SetCurrentDropAction(DropAction value)
        {
            if (this._currentDropAction == value)
                return;
            this._currentDropAction = value;
            this.FireNotification(NotificationID.CurrentDropAction);
            this.UpdateCursor();
        }

        public bool Dragging => DragDropHelper.DraggingInternally && DragDropHelper.SourceHandler == this;

        public CursorID MoveCursor
        {
            get => this._moveCursor;
            set
            {
                if (this._moveCursor == value)
                    return;
                this._moveCursor = value;
                this.FireNotification(NotificationID.MoveCursor);
            }
        }

        public CursorID CopyCursor
        {
            get => this._copyCursor;
            set
            {
                if (this._copyCursor == value)
                    return;
                this._copyCursor = value;
                this.FireNotification(NotificationID.CopyCursor);
            }
        }

        public CursorID CancelCursor
        {
            get => this._cancelCursor;
            set
            {
                if (this._cancelCursor == value)
                    return;
                this._cancelCursor = value;
                this.FireNotification(NotificationID.CancelCursor);
            }
        }

        protected override void OnMousePrimaryDown(UIClass ui, MouseButtonInfo info)
        {
            this._pendingDrag = true;
            this._initialX = info.ScreenX;
            this._initialY = info.ScreenY;
            base.OnMousePrimaryDown(ui, info);
        }

        protected override void OnMouseMove(UIClass ui, MouseMoveInfo info)
        {
            if (this.Dragging)
            {
                DragDropHelper.Requery(info.NaturalHit, info.ScreenX, info.ScreenY, info.Modifiers);
                info.MarkHandled();
            }
            else if (this._pendingDrag)
            {
                if (Math.Abs(info.ScreenX - this._initialX) >= Win32Api.GetSystemMetrics(68) || Math.Abs(info.ScreenY - this._initialY) >= Win32Api.GetSystemMetrics(69))
                {
                    this._pendingDrag = false;
                    DragDropHelper.BeginDrag(this, info.Target, info.NaturalHit, 0, 0, info.Modifiers);
                    this.UI.SessionInput += new SessionInputHandler(this.OnSessionInput);
                    this.FireNotification(NotificationID.Dragging);
                    this.FireNotification(NotificationID.Started);
                    this.UpdateCursor();
                    info.MarkHandled();
                }
            }
            else if (this._dragCanceled)
                info.MarkHandled();
            base.OnMouseMove(ui, info);
        }

        protected override void OnMousePrimaryUp(UIClass sender, MouseButtonInfo info)
        {
            this._pendingDrag = false;
            if (this.Dragging)
            {
                this.EndDrag(info?.NaturalHit, info.Modifiers, this.CurrentDropAction);
                info.MarkHandled();
            }
            else if (this._dragCanceled)
            {
                this._dragCanceled = false;
                info.MarkHandled();
            }
            base.OnMousePrimaryUp(sender, info);
        }

        protected override void OnDragComplete(UIClass sender, DragDropInfo info)
        {
            DragDropHelper.OnDragComplete();
            info.MarkHandled();
            base.OnDragComplete(sender, info);
        }

        private void OnSessionInput(InputInfo originalEvent, EventRouteStages handledStage)
        {
            switch (originalEvent)
            {
                case KeyStateInfo keyStateInfo:
                    switch (keyStateInfo.Key)
                    {
                        case Keys.ControlKey:
                            InputModifiers modifiers = keyStateInfo.Modifiers;
                            if (keyStateInfo.Action == KeyAction.Down)
                                modifiers |= InputModifiers.ControlKey;
                            DragDropHelper.Requery(modifiers);
                            break;
                        case Keys.Escape:
                            this.EndDrag(null, keyStateInfo.Modifiers, DropAction.None);
                            this._dragCanceled = true;
                            break;
                    }
                    keyStateInfo.MarkHandled();
                    break;
                case MouseButtonInfo mouseButtonInfo:
                    if (mouseButtonInfo.Button == MouseButtons.Left)
                        break;
                    mouseButtonInfo.MarkHandled();
                    break;
            }
        }

        protected override void OnLoseKeyFocus(UIClass sender, KeyFocusInfo info)
        {
            this._pendingDrag = false;
            if (this.Dragging)
                this.EndDrag(null, DragDropHelper.Modifiers, DropAction.None);
            base.OnLoseKeyFocus(sender, info);
        }

        protected override void OnLoseMouseFocus(UIClass sender, MouseFocusInfo info)
        {
            this._pendingDrag = false;
            if (this.Dragging)
                this.EndDrag(null, DragDropHelper.Modifiers, DropAction.None);
            base.OnLoseMouseFocus(sender, info);
        }

        private void EndDrag(IRawInputSite target, InputModifiers modifiers, DropAction action)
        {
            this.UI.SessionInput -= new SessionInputHandler(this.OnSessionInput);
            DragDropHelper.EndDrag(target, modifiers, action);
        }

        internal void OnEndDrag(DropAction action)
        {
            this.FireNotification(NotificationID.Dragging);
            switch (action)
            {
                case DropAction.None:
                    this.FireNotification(NotificationID.Canceled);
                    break;
                case DropAction.Copy:
                    this.FireNotification(NotificationID.Copied);
                    break;
                case DropAction.Move:
                    this.FireNotification(NotificationID.Moved);
                    break;
            }
            this.UpdateCursor();
        }

        internal void UpdateCurrentAction()
        {
            DropAction dropAction = DragDropHelper.AllowedDropActions & this.AllowedDropActions;
            if ((dropAction & DropAction.All) == DropAction.All)
            {
                if ((DragDropHelper.Modifiers & InputModifiers.ControlKey) == InputModifiers.ControlKey)
                    this.SetCurrentDropAction(DropAction.Copy);
                else
                    this.SetCurrentDropAction(DropAction.Move);
            }
            else
                this.SetCurrentDropAction(dropAction);
        }

        internal override CursorID GetCursor()
        {
            if (this.Dragging)
            {
                switch (this.CurrentDropAction)
                {
                    case DropAction.None:
                        return this.CancelCursor;
                    case DropAction.Copy:
                        return this.CopyCursor;
                    case DropAction.Move:
                        return this.MoveCursor;
                }
            }
            return CursorID.NotSpecified;
        }
    }
}
