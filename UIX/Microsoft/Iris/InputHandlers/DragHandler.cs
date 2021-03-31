// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.InputHandlers.DragHandler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Iris.InputHandlers
{
    internal class DragHandler : InputHandler
    {
        private CursorID _dragCursor;
        private BeginDragPolicy _beginDragPolicy;
        private bool _pendingDrag;
        private bool _inDrag;
        private bool _hasRelativeTo;
        private Vector2 _initialPosition;
        private Point _initialScreenPosition;
        private Vector2 _beginPosition;
        private Vector2 _endPosition;
        private Point _screenBeginPosition;
        private Point _screenEndPosition;
        private Size _lastKnownSize;
        private InputHandlerModifiers _activeModifiers;
        private bool _cancelOnEscape;
        private ViewItem _relativeTo;
        private RectangleF _contextBounds;
        private List<object> _addedContexts;
        private List<object> _removedContexts;

        public DragHandler() => this._beginDragPolicy = BeginDragPolicy.Down;

        public override void OnZoneDetached()
        {
            this.CancelDrag();
            base.OnZoneDetached();
        }

        protected override void ConfigureInteractivity()
        {
            base.ConfigureInteractivity();
            if (this.HandleDirect)
                this.UI.MouseInteractive = true;
            if (this._relativeTo != null)
                return;
            this.SetRelativeTo((ViewItem)null);
        }

        public BeginDragPolicy BeginDragPolicy
        {
            get => this._beginDragPolicy;
            set
            {
                if (this._beginDragPolicy == value)
                    return;
                this._beginDragPolicy = value;
                this.FireNotification(NotificationID.BeginDragPolicy);
            }
        }

        public bool Dragging => this._inDrag;

        private void SetDragging(bool value)
        {
            if (this._inDrag == value)
                return;
            this._inDrag = value;
            this.FireNotification(NotificationID.Dragging);
        }

        public Vector2 BeginPosition => this.NormalizeCoordinates(this._beginPosition);

        private void SetBeginPosition(Vector2 relativeCoordinate)
        {
            if (!(this._beginPosition != relativeCoordinate))
                return;
            this._beginPosition = relativeCoordinate;
            this.FireNotification(NotificationID.BeginPosition);
            this.FireNotification(NotificationID.RelativeDragSize);
        }

        public Vector2 EndPosition => this.NormalizeCoordinates(this._endPosition);

        private void SetEndPosition(Vector2 relativeCoordinate)
        {
            if (!(this._endPosition != relativeCoordinate))
                return;
            this._endPosition = relativeCoordinate;
            this.FireNotification(NotificationID.EndPosition);
            this.FireNotification(NotificationID.RelativeDragSize);
        }

        public Size ScreenDragSize => (this.ScreenEndPosition - this.ScreenBeginPosition).ToSize();

        public Vector2 LocalDragSize
        {
            get
            {
                Size screenDragSize = this.ScreenDragSize;
                Vector3 vector3 = this._relativeTo != null ? this._relativeTo.ComputeEffectiveScale() : Vector3.UnitVector;
                return new Vector2((float)screenDragSize.Width / vector3.X, (float)screenDragSize.Height / vector3.Y);
            }
        }

        public Vector2 RelativeDragSize => this.EndPosition - this.BeginPosition;

        public InputHandlerModifiers ActiveModifiers => this._activeModifiers;

        private void SetActiveModifiers(InputHandlerModifiers value)
        {
            if (this._activeModifiers == value)
                return;
            this._activeModifiers = value;
            this.FireNotification(NotificationID.ActiveModifiers);
        }

        public CursorID DragCursor
        {
            get => this._dragCursor;
            set
            {
                if (this._dragCursor == value)
                    return;
                this._dragCursor = value;
                this.FireNotification(NotificationID.DragCursor);
            }
        }

        private Point ScreenBeginPosition
        {
            get => this._screenBeginPosition;
            set
            {
                if (!(this._screenBeginPosition != value))
                    return;
                this._screenBeginPosition = value;
                this.FireNotification(NotificationID.ScreenDragSize);
                this.FireNotification(NotificationID.LocalDragSize);
            }
        }

        private Point ScreenEndPosition
        {
            get => this._screenEndPosition;
            set
            {
                if (!(this._screenEndPosition != value))
                    return;
                this._screenEndPosition = value;
                this.FireNotification(NotificationID.ScreenDragSize);
                this.FireNotification(NotificationID.LocalDragSize);
            }
        }

        public bool CancelOnEscape
        {
            get => this._cancelOnEscape;
            set
            {
                if (this._cancelOnEscape == value)
                    return;
                if (this.Dragging)
                    this.HookSessionInput(value);
                this._cancelOnEscape = value;
                this.FireNotification(NotificationID.CancelOnEscape);
            }
        }

        public ViewItem RelativeTo
        {
            get => !this._hasRelativeTo ? (ViewItem)null : this._relativeTo;
            set
            {
                bool hasRelativeTo = this._hasRelativeTo;
                this._hasRelativeTo = value != null;
                ViewItem relativeTo = this._relativeTo;
                this.SetRelativeTo(value);
                if (hasRelativeTo == this._hasRelativeTo && relativeTo == this._relativeTo)
                    return;
                this.FireNotification(NotificationID.RelativeTo);
            }
        }

        private void SetRelativeTo(ViewItem relativeTo)
        {
            if (this._relativeTo == relativeTo && relativeTo != null)
                return;
            LayoutCompleteEventHandler completeEventHandler = new LayoutCompleteEventHandler(this.OnRelativeToLayoutComplete);
            if (this._relativeTo != null)
                this._relativeTo.LayoutComplete -= completeEventHandler;
            this._relativeTo = relativeTo;
            if (this._relativeTo == null && this.UI != null)
                this._relativeTo = this.UI.RootItem;
            if (this._relativeTo == null)
                return;
            this._relativeTo.LayoutComplete += completeEventHandler;
        }

        public void ResetDragOrigin()
        {
            this.SetBeginPosition(this._endPosition);
            this.ScreenBeginPosition = this.ScreenEndPosition;
        }

        private void BeginDrag(
          Vector2 relativeBegin,
          Vector2 relativeEnd,
          Point screenBegin,
          Point screenEnd,
          InputHandlerModifiers modifiers)
        {
            this.SetDragging(true);
            this._pendingDrag = false;
            this._contextBounds = RectangleF.Zero;
            if (this.CancelOnEscape)
                this.HookSessionInput(true);
            this.SetBeginPosition(relativeBegin);
            this.SetEndPosition(relativeEnd);
            this.ScreenBeginPosition = screenBegin;
            this.ScreenEndPosition = screenEnd;
            this.SetActiveModifiers(modifiers);
            this.FireNotification(NotificationID.Started);
            this.UpdateCursor();
        }

        private void EndDrag(bool completed)
        {
            if (this.Dragging)
            {
                if (this.CancelOnEscape)
                    this.HookSessionInput(false);
                this.SetDragging(false);
                if (completed)
                    this.FireNotification(NotificationID.Ended);
                else
                    this.FireNotification(NotificationID.Canceled);
                this.UpdateCursor();
            }
            this._pendingDrag = false;
        }

        private void InDrag(Vector2 uiPoint, Point screenPoint, InputHandlerModifiers modifiers)
        {
            if (screenPoint != this.ScreenEndPosition)
            {
                this.SetEndPosition(this.TransformToRelative(uiPoint));
                this.ScreenEndPosition = screenPoint;
            }
            this.SetActiveModifiers(modifiers);
        }

        protected override void OnMousePrimaryDown(UIClass ui, MouseButtonInfo info)
        {
            if (!this.Dragging)
            {
                Vector2 relative = this.TransformToRelative(this.TransformToUI(new Point(info.X, info.Y), (UIClass)info.Target));
                Point point = new Point(info.ScreenX, info.ScreenY);
                if (this.BeginDragPolicy == BeginDragPolicy.Down)
                {
                    this.BeginDrag(relative, relative, point, point, InputHandler.GetModifiers(info.Modifiers));
                    info.MarkHandled();
                }
                else
                {
                    this._pendingDrag = true;
                    this._initialPosition = relative;
                    this._initialScreenPosition = point;
                }
            }
            base.OnMousePrimaryDown(ui, info);
        }

        protected override void OnMousePrimaryUp(UIClass ui, MouseButtonInfo info)
        {
            bool dragging = this.Dragging;
            if (dragging || this._pendingDrag)
            {
                this.EndDrag(true);
                if (dragging)
                    info.MarkHandled();
            }
            base.OnMousePrimaryUp(ui, info);
        }

        protected override void OnMouseMove(UIClass ui, MouseMoveInfo info)
        {
            if ((info.Modifiers & InputModifiers.LeftMouse) == InputModifiers.None)
                this.EndDrag(true);
            else if (this.Dragging || this._pendingDrag)
            {
                Vector2 ui1 = this.TransformToUI(new Point(info.X, info.Y), (UIClass)info.Target);
                Point point = new Point(info.ScreenX, info.ScreenY);
                if (this.Dragging)
                {
                    this.InDrag(ui1, point, InputHandler.GetModifiers(info.Modifiers));
                    info.MarkHandled();
                }
                else if (Math.Abs(point.X - this._initialScreenPosition.X) >= Win32Api.GetSystemMetrics(68) || Math.Abs(point.Y - this._initialScreenPosition.Y) >= Win32Api.GetSystemMetrics(69))
                {
                    this.BeginDrag(this._initialPosition, this.TransformToRelative(ui1), this._initialScreenPosition, point, InputHandler.GetModifiers(info.Modifiers));
                    info.MarkHandled();
                }
            }
            base.OnMouseMove(ui, info);
        }

        protected override void OnLoseMouseFocus(UIClass ui, MouseFocusInfo info)
        {
            this.CancelDrag();
            base.OnLoseMouseFocus(ui, info);
        }

        private void OnRelativeToLayoutComplete(object sender) => this.SetLastLayoutSize(this._relativeTo.LayoutSize);

        protected void SetLastLayoutSize(Size size)
        {
            Vector2 beginPosition = this.BeginPosition;
            Vector2 endPosition = this.EndPosition;
            Vector2 relativeDragSize = this.RelativeDragSize;
            this._lastKnownSize = size;
            if (!this.Dragging)
                return;
            Point client = this._relativeTo.ScreenToClient(this._screenEndPosition);
            this.SetEndPosition(new Vector2((float)client.X, (float)client.Y));
            if (beginPosition != this.BeginPosition)
                this.FireNotification(NotificationID.BeginPosition);
            if (endPosition != this.EndPosition)
                this.FireNotification(NotificationID.EndPosition);
            if (!(relativeDragSize != this.RelativeDragSize))
                return;
            this.FireNotification(NotificationID.RelativeDragSize);
        }

        private void HookSessionInput(bool hook)
        {
            if (hook)
                this.UI.SessionInput += new SessionInputHandler(this.OnSessionInput);
            else
                this.UI.SessionInput -= new SessionInputHandler(this.OnSessionInput);
        }

        private void OnSessionInput(InputInfo originalEvent, EventRouteStages stageHandled)
        {
            if (!(originalEvent is KeyStateInfo keyStateInfo))
                return;
            if (keyStateInfo.Key == Keys.Escape)
                this.CancelDrag();
            keyStateInfo.MarkHandled();
        }

        private Vector2 TransformToRelative(Vector2 uiPoint)
        {
            if (this._relativeTo != null)
            {
                RectangleF rectangleF = this._relativeTo.TransformFromAncestor(this.UI.RootItem, new RectangleF(uiPoint.X, uiPoint.Y, 0.0f, 0.0f));
                uiPoint.X = rectangleF.X;
                uiPoint.Y = rectangleF.Y;
            }
            return uiPoint;
        }

        private RectangleF TransformFromRelative(RectangleF relativeBounds)
        {
            RectangleF rectangleF = relativeBounds;
            if (this._relativeTo != null)
                rectangleF = this._relativeTo.TransformToAncestor(this.UI.RootItem, relativeBounds);
            return rectangleF;
        }

        private Vector2 TransformToUI(Point uiPoint, UIClass reference)
        {
            float x = (float)uiPoint.X;
            float y = (float)uiPoint.Y;
            if (reference != this.UI)
            {
                RectangleF rect = new RectangleF(x, y, 0.0f, 0.0f);
                RectangleF ancestor = reference.RootItem.TransformToAncestor(this.UI.RootItem, rect);
                x = ancestor.X;
                y = ancestor.Y;
            }
            return new Vector2(x, y);
        }

        private Vector2 NormalizeCoordinates(Vector2 pt) => this._lastKnownSize.Width == 0 || this._lastKnownSize.Height == 0 ? Vector2.Zero : new Vector2(pt.X / (float)this._lastKnownSize.Width, pt.Y / (float)this._lastKnownSize.Height);

        internal override CursorID GetCursor() => this._dragCursor != CursorID.NotSpecified && this.Dragging ? this._dragCursor : CursorID.NotSpecified;

        public void CancelDrag() => this.EndDrag(false);

        private void GetDragBounds(out RectangleF relativeBounds, out RectangleF uiBounds)
        {
            relativeBounds = new RectangleF(Math.Min(this._beginPosition.X, this._endPosition.X), Math.Min(this._beginPosition.Y, this._endPosition.Y), Math.Abs(this._beginPosition.X - this._endPosition.X), Math.Abs(this._beginPosition.Y - this._endPosition.Y));
            uiBounds = this.TransformFromRelative(relativeBounds);
        }

        public IList GetEventContexts()
        {
            IList added = (IList)new List<object>();
            RectangleF uiBounds;
            this.GetDragBounds(out RectangleF _, out uiBounds);
            DragHandler.GetEventContexts(this.UI, added, (IList)null, RectangleF.Zero, uiBounds);
            return added;
        }

        public IList GetAddedEventContexts()
        {
            this.UpdateEventContexts();
            return (IList)this._addedContexts;
        }

        public IList GetRemovedEventContexts()
        {
            this.UpdateEventContexts();
            return (IList)this._removedContexts;
        }

        private void UpdateEventContexts()
        {
            RectangleF relativeBounds;
            RectangleF uiBounds;
            this.GetDragBounds(out relativeBounds, out uiBounds);
            if (!(relativeBounds != this._contextBounds))
                return;
            this._addedContexts = new List<object>();
            this._removedContexts = new List<object>();
            DragHandler.GetEventContexts(this.UI, (IList)this._addedContexts, (IList)this._removedContexts, this.TransformFromRelative(this._contextBounds), uiBounds);
            this._contextBounds = relativeBounds;
        }

        private static void GetEventContexts(
          UIClass node,
          IList added,
          IList removed,
          RectangleF oldBounds,
          RectangleF newBounds)
        {
            ViewItem rootItem = node.RootItem;
            if (rootItem == null)
                return;
            object eventContext = node.GetEventContext();
            if (eventContext != null && rootItem.Parent != null)
            {
                RectangleF rectangleF = new RectangleF(Point.Zero, rootItem.Parent.LayoutSize);
                bool flag1 = rectangleF.IntersectsWith(oldBounds);
                bool flag2 = rectangleF.IntersectsWith(newBounds);
                if (flag2 && !flag1)
                    added.Add(eventContext);
                else if (removed != null && flag1 && !flag2)
                    removed.Add(eventContext);
            }
            for (UIClass node1 = (UIClass)node.FirstChild; node1 != null; node1 = (UIClass)node1.NextSibling)
            {
                if (node1.RootItem != null)
                {
                    RectangleF oldBounds1 = node1.RootItem.Parent.TransformFromAncestor(rootItem.Parent, oldBounds);
                    RectangleF newBounds1 = node1.RootItem.Parent.TransformFromAncestor(rootItem.Parent, newBounds);
                    DragHandler.GetEventContexts(node1, added, removed, oldBounds1, newBounds1);
                }
            }
        }
    }
}
