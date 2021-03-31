// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Input.InputQueue
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Queues;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.Input
{
    internal class InputQueue : SimpleQueue
    {
        private QueueItem.Stack _filterStack;
        private InputManager _inputManager;
        private ICookedInputSite _currentKeyFocus;
        private ICookedInputSite _desiredKeyFocus;
        private KeyFocusReason _desiredKeyFocusReason;
        private ICookedInputSite _lastCompletedKeyFocus;
        private bool _revalidateKeyFocus;
        private uint _keyFocusRepairCount;
        private SimpleCallback _invalidKeyFocusCallback;
        private bool _mouseMoved;
        private IRawInputSite _rawMouseSite;
        private IRawInputSite _rawMouseNaturalSite;
        private Point _rawMousePos;
        private Point _rawScreenPos;
        private InputModifiers _rawMouseModifiers;
        private int _mouseWheelDelta;
        private bool _dragOver;
        private bool _dragging;
        private ICookedInputSite _dragSource;
        private IRawInputSite _rawDropTargetSite;
        private Point _rawDragPoint;
        private InputModifiers _rawDragModifiers;
        private object _dragData;
        private object _pendingDragData;
        private ICookedInputSite _appDropTarget;
        private IRawInputSite _appMouseFocusSite;
        private IRawInputSite _appNaturalTarget;
        private ICookedInputSite _appMouseFocusTarget;
        private ICookedInputSite _appMouseFocusCapture;
        private InputModifiers _appMouseFocusButtons;
        private ICookedInputSite _lastCompletedMouseFocus;
        private Point _appMousePos;
        private bool _revalidateMouseFocus;
        private SimpleQueue _inputIdleQueue;

        public InputQueue(InputManager manager)
        {
            this._inputManager = manager;
            this._filterStack = new QueueItem.Stack();
            this._rawMousePos = new Point();
            this._appMousePos = new Point();
            this._desiredKeyFocusReason = KeyFocusReason.Default;
            this._inputIdleQueue = new SimpleQueue();
            this._inputIdleQueue.Wake += new EventHandler(this.OnChildQueueWake);
            this._invalidKeyFocusCallback = new SimpleCallback(this.InvalidKeyFocusCallback);
        }

        public ICookedInputSite InstantaneousKeyFocus => this._currentKeyFocus;

        public ICookedInputSite PendingKeyFocus => this._desiredKeyFocus;

        public bool PendingKeyFocusIsDefault => this._desiredKeyFocusReason == KeyFocusReason.Default;

        public ICookedInputSite LastCompletedKeyFocus => this._lastCompletedKeyFocus;

        public ICookedInputSite CurrentMouseFocus => this._appMouseFocusTarget;

        public InputModifiers DragModifiers => this._rawDragModifiers;

        public void RequestKeyFocus(ICookedInputSite desired) => this.RequestKeyFocus(desired, KeyFocusReason.Other);

        public void RequestKeyFocus(ICookedInputSite desired, KeyFocusReason reason)
        {
            if (this._desiredKeyFocus != desired)
            {
                this._desiredKeyFocus = desired;
                this._desiredKeyFocusReason = reason;
            }
            else if (reason != KeyFocusReason.Default)
                this._desiredKeyFocusReason = reason;
            if (desired != null)
                this._inputManager.RequestHostKeyFocus(desired);
            if (this._desiredKeyFocus == this._currentKeyFocus)
                return;
            this.OnWake();
        }

        public void PrepareToShutDown()
        {
            this.RequestKeyFocus(null);
            this.RawMouseLeave();
        }

        public void RawKeyAction(KeyInfo info) => this.PostItem(this.GenerateGenericInput(info));

        public void RawMouseMove(
          IRawInputSite site,
          IRawInputSite naturalSite,
          InputModifiers modifiers,
          ref RawMouseData rawEventData)
        {
            this._rawMouseSite = site;
            this._rawMouseNaturalSite = naturalSite;
            this._rawMousePos.X = rawEventData._positionX;
            this._rawMousePos.Y = rawEventData._positionY;
            this._rawScreenPos.X = rawEventData._screenX;
            this._rawScreenPos.Y = rawEventData._screenY;
            this._rawMouseModifiers = modifiers;
            this.SetMouseMoved();
        }

        public void RawMouseLeave()
        {
            this.CancelMouseCapture(null);
            this._mouseWheelDelta = 0;
            this._rawMouseSite = null;
            this._rawMouseNaturalSite = null;
            this._rawMousePos.X = -1;
            this._rawMousePos.Y = -1;
            this._rawScreenPos.X = -1;
            this._rawScreenPos.Y = -1;
            this._rawMouseModifiers = InputModifiers.None;
            this.SetMouseMoved();
        }

        public void RawMouseButtonState(
          uint message,
          IRawInputSite site,
          IRawInputSite naturalSite,
          InputModifiers modifiers,
          MouseButtons button,
          bool state)
        {
            this.PostItem(this.GenerateMouseButton(site, naturalSite, this._rawMousePos.X, this._rawMousePos.Y, this._rawScreenPos.X, this._rawScreenPos.Y, modifiers, button, state, message));
        }

        public void RawMouseWheel(InputModifiers modifiers, ref RawMouseData rawEventData)
        {
            bool flag = this._mouseWheelDelta != 0;
            this._mouseWheelDelta += rawEventData._wheelDelta;
            this._rawMouseModifiers = modifiers;
            if (flag || this._mouseWheelDelta == 0)
                return;
            this.OnWake();
        }

        public void SimulateDragEnter(
          ICookedInputSite dragSource,
          IRawInputSite rawTargetSite,
          object data,
          int x,
          int y,
          InputModifiers modifiers)
        {
            this.SimulateDragDrop(dragSource, rawTargetSite, data, x, y, modifiers, DragOperation.Enter);
        }

        public void SimulateDragOver(InputModifiers modifiers) => this.SimulateDragDrop(this._dragSource, this._rawDropTargetSite, null, this._rawDragPoint.X, this._rawDragPoint.Y, modifiers, DragOperation.Over);

        public void SimulateDragOver(
          IRawInputSite rawTargetSite,
          int x,
          int y,
          InputModifiers modifiers)
        {
            this.SimulateDragDrop(this._dragSource, rawTargetSite, null, x, y, modifiers, DragOperation.Over);
        }

        public void SimulateDragEnd(
          IRawInputSite rawTargetSite,
          InputModifiers modifiers,
          DragOperation formOperation)
        {
            this.PushFilterStack(this.GenerateDragDrop(this._dragSource, null, this._rawDragPoint.X, this._rawDragPoint.Y, modifiers, DragOperation.DragComplete));
            this.SimulateDragDrop(this._dragSource, rawTargetSite, null, this._rawDragPoint.X, this._rawDragPoint.Y, modifiers, formOperation);
            this.OnWake();
            this._dragSource = null;
        }

        private void SimulateDragDrop(
          ICookedInputSite dragSource,
          IRawInputSite rawTargetSite,
          object data,
          int x,
          int y,
          InputModifiers modifiers,
          DragOperation formOperation)
        {
            this._dragSource = dragSource;
            InputItem dragDropItem = this.GenerateDragDropItem(dragSource, rawTargetSite, data, x, y, modifiers, formOperation);
            if (dragDropItem == null)
                return;
            this.PushFilterStack(dragDropItem);
            this.OnWake();
        }

        public void RawDragDrop(
          IRawInputSite rawSite,
          object data,
          int x,
          int y,
          InputModifiers modifiers,
          DragOperation formOperation,
          RawDragData rawEventData)
        {
            if (this._dragSource == null)
            {
                if (formOperation == DragOperation.Over && data == null && this._pendingDragData != null)
                {
                    data = this._pendingDragData;
                    formOperation = DragOperation.Enter;
                }
                this._pendingDragData = null;
                InputItem dragDropItem = this.GenerateDragDropItem(null, rawSite, data, x, y, modifiers, formOperation);
                if (dragDropItem == null)
                    return;
                this.PostItem(dragDropItem);
            }
            else if (formOperation == DragOperation.Enter)
            {
                this._pendingDragData = data;
            }
            else
            {
                if (formOperation != DragOperation.Leave)
                    return;
                this._pendingDragData = null;
            }
        }

        private InputItem GenerateDragDropItem(
          ICookedInputSite dragSource,
          IRawInputSite rawSite,
          object data,
          int x,
          int y,
          InputModifiers modifiers,
          DragOperation formOperation)
        {
            InputItem inputItem = null;
            IRawInputSite rawDropTargetSite = this._rawDropTargetSite;
            if (formOperation == DragOperation.Enter)
                this._dragData = data;
            this._rawDropTargetSite = rawSite;
            this._rawDragPoint = new Point(x, y);
            this._rawDragModifiers = modifiers;
            if (formOperation != DragOperation.Drop)
            {
                this._dragging = true;
                if (rawDropTargetSite == rawSite)
                {
                    if (!this._dragOver)
                    {
                        this._dragOver = true;
                        this.OnWake();
                    }
                }
                else
                    inputItem = this.GenerateDragDrop(null, rawSite, x, y, modifiers, DragOperation.Over);
            }
            else
            {
                inputItem = this.GenerateDragDrop(this._appDropTarget, rawSite, x, y, modifiers, DragOperation.Drop);
                this._appDropTarget = null;
            }
            if (formOperation == DragOperation.Drop || formOperation == DragOperation.Leave)
            {
                this._dragOver = false;
                this._dragging = false;
                this._rawDropTargetSite = null;
                this._rawDragPoint = new Point();
                this._rawDragModifiers = InputModifiers.None;
                if (formOperation == DragOperation.Leave)
                    this._dragData = null;
            }
            return inputItem;
        }

        public object GetDragDropValue()
        {
            object dragData = this._dragData;
            if (!this._dragging)
                this._dragData = null;
            return dragData;
        }

        public void RawInputIdleItem(QueueItem item) => this._inputIdleQueue.PostItem(item);

        public void RevalidateInputSiteUsage(ICookedInputSite target, bool recursiveFlag)
        {
            bool flag1 = false;
            if (target == null)
            {
                if (!recursiveFlag)
                {
                    this._revalidateKeyFocus = true;
                    this._revalidateMouseFocus = true;
                    this.SetMouseMoved();
                    return;
                }
                flag1 = true;
            }
            this.CancelMouseCapture(target);
            bool flag2 = false;
            if (!this._mouseMoved && this._appMouseFocusTarget != null && (flag1 || this._appMouseFocusTarget == target || recursiveFlag && this._inputManager.IsSiteInfluencedByPeer(this._appMouseFocusTarget, target)))
            {
                this._revalidateMouseFocus = true;
                this.SetMouseMoved();
                if (this._appMouseFocusTarget == this._desiredKeyFocus)
                    flag2 = true;
            }
            if (this._revalidateKeyFocus || !flag2 && (this._desiredKeyFocus == null || !flag1 && this._desiredKeyFocus != target && (!recursiveFlag || !this._inputManager.IsSiteInfluencedByPeer(this._desiredKeyFocus, target))))
                return;
            this._revalidateKeyFocus = true;
            this.OnWake();
        }

        public override QueueItem GetNextItem()
        {
            InputItem inputItem;
            QueueItem queueItem1 = null;
            while (true)
            {
                QueueItem queueItem2;
                do
                {
                    queueItem2 = this.PeekFilterStack();
                    if (queueItem2 != null)
                    {
                        queueItem1 = this.FilterItem(queueItem2);
                        if (queueItem1 == null)
                            this.PopFilterStack();
                        else
                            goto label_5;
                    }
                    else
                        goto label_8;
                }
                while (!(queueItem2 is InputItem));
                inputItem = (InputItem)queueItem2;
                inputItem.ReturnToPool();
                continue;
            label_5:
                if (queueItem1 != queueItem2)
                {
                    this.PushFilterStack(queueItem1);
                    queueItem1 = null;
                }
                else
                    break;
            }
            this.PopFilterStack();
        label_8:
            return queueItem1;
        }

        private void PushFilterStack(QueueItem item) => this._filterStack.Push(item);

        private QueueItem PopFilterStack() => this._filterStack.Pop();

        private QueueItem PeekFilterStack()
        {
            QueueItem queueItem = this._filterStack.Peek();
            if (queueItem == null)
            {
                queueItem = this.GetNextRawInputItem();
                if (queueItem != null)
                    this.PushFilterStack(queueItem);
            }
            return queueItem;
        }

        private QueueItem FilterItem(QueueItem item)
        {
            if (item is InputItem inputItem)
            {
                switch (inputItem.Info)
                {
                    case MouseActionInfo mouseActionInfo:
                        this.MapMouseInput(mouseActionInfo);
                        Point clientOffset = new Point(mouseActionInfo.X, mouseActionInfo.Y);
                        switch (mouseActionInfo)
                        {
                            case MouseMoveInfo _:
                                QueueItem queueItem1 = this.UpdateMouseFocus(mouseActionInfo.Target, mouseActionInfo.RawSource, clientOffset);
                                if (queueItem1 != null)
                                    return queueItem1;
                                if (this.IsAppMouseMove(mouseActionInfo))
                                {
                                    this.FinalizeMouseHit(inputItem, mouseActionInfo);
                                    break;
                                }
                                item = null;
                                break;
                            case MouseButtonInfo mouseButtonInfo:
                                if (this.IsAppMouseMove(mouseActionInfo))
                                {
                                    InputModifiers modifiers1 = mouseButtonInfo.Modifiers;
                                    InputModifiers modifiersForButtons = this.GetModifiersForButtons(mouseButtonInfo.Button);
                                    InputModifiers modifiers2 = !mouseButtonInfo.IsDown ? modifiers1 | modifiersForButtons : modifiers1 & ~modifiersForButtons;
                                    return this.GenerateMouseMove(mouseActionInfo.RawSource, mouseActionInfo.NaturalHit, mouseActionInfo.X, mouseActionInfo.Y, mouseButtonInfo.ScreenX, mouseButtonInfo.ScreenY, modifiers2);
                                }
                                this.FinalizeMouseHit(inputItem, mouseActionInfo);
                                this.UpdateMouseCapture(mouseActionInfo.RawSource, mouseActionInfo.Target, mouseButtonInfo.Modifiers);
                                this._appMouseFocusButtons = mouseButtonInfo.Modifiers & InputModifiers.AllButtons;
                                break;
                            case MouseWheelInfo _:
                                inputItem.UpdateInputSite(this._appMouseFocusTarget);
                                break;
                        }
                        break;
                    case MouseFocusInfo mouseFocusInfo:
                        if (!mouseFocusInfo.State && this._appMouseFocusButtons != InputModifiers.None)
                        {
                            QueueItem queueItem2 = this.UpdateOrphanedButtons(mouseFocusInfo.RawSource, mouseFocusInfo.X, mouseFocusInfo.Y);
                            if (queueItem2 != null)
                                return queueItem2;
                            break;
                        }
                        break;
                    case KeyActionInfo _:
                        QueueItem queueItem3 = this.UpdateKeyFocus();
                        if (queueItem3 != null)
                            return queueItem3;
                        inputItem.UpdateInputSite(this._currentKeyFocus);
                        break;
                    case DragDropInfo dragDropInfo:
                        if (dragDropInfo.Operation == DragOperation.Over)
                        {
                            InputItem inputItemB = this.UpdateDragOver(this._inputManager.HitTestInput(this._rawDropTargetSite, null));
                            if (inputItemB != null)
                                return inputItemB;
                            inputItemB.UpdateInputSite(this._appDropTarget);
                            break;
                        }
                        break;
                }
            }
            return item;
        }

        private InputModifiers GetModifiersForButtons(MouseButtons button)
        {
            InputModifiers inputModifiers = InputModifiers.None;
            if ((button & MouseButtons.Left) != MouseButtons.None)
                inputModifiers |= InputModifiers.LeftMouse;
            if ((button & MouseButtons.Middle) != MouseButtons.None)
                inputModifiers |= InputModifiers.MiddleMouse;
            if ((button & MouseButtons.Right) != MouseButtons.None)
                inputModifiers |= InputModifiers.RightMouse;
            if ((button & MouseButtons.XButton1) != MouseButtons.None)
                inputModifiers |= InputModifiers.XMouse1;
            if ((button & MouseButtons.XButton2) != MouseButtons.None)
                inputModifiers |= InputModifiers.XMouse2;
            return inputModifiers;
        }

        private QueueItem GetNextRawInputItem()
        {
            QueueItem queueItem;
            if ((queueItem = this.InjectRawInput_Prty()) == null && (queueItem = this.InjectRawInput_Norm()) == null)
                queueItem = this.InjectRawInput_Idle();
            return queueItem;
        }

        private QueueItem InjectRawInput_Prty() => this.UpdateKeyFocus();

        private QueueItem InjectRawInput_Norm() => base.GetNextItem();

        private QueueItem InjectRawInput_Idle()
        {
            if (this._mouseWheelDelta != 0)
            {
                QueueItem mouseWheel = this.GenerateMouseWheel(this._rawMouseSite, this._rawMouseNaturalSite, this._rawMousePos.X, this._rawMousePos.Y, this._rawScreenPos.X, this._rawScreenPos.Y, this._rawMouseModifiers, this._mouseWheelDelta);
                this._mouseWheelDelta = 0;
                return mouseWheel;
            }
            if (this._mouseMoved)
            {
                QueueItem mouseMove = this.GenerateMouseMove(this._rawMouseSite, this._rawMouseNaturalSite, this._rawMousePos.X, this._rawMousePos.Y, this._rawScreenPos.X, this._rawScreenPos.Y, this._rawMouseModifiers);
                this._mouseMoved = false;
                return mouseMove;
            }
            if (!this._dragOver)
                return this._inputIdleQueue.GetNextItem();
            InputItem dragDrop = this.GenerateDragDrop(null, this._rawDropTargetSite, this._rawDragPoint.X, this._rawDragPoint.Y, this._rawDragModifiers, DragOperation.Over);
            this._dragOver = false;
            return dragDrop;
        }

        private void OnChildQueueWake(object sender, EventArgs args) => this.OnWake();

        private void MapMouseInput(MouseActionInfo info) => this._inputManager.MapMouseInput(info, this._appMouseFocusCapture);

        private void FinalizeMouseHit(InputItem item, MouseActionInfo mouse)
        {
            item.UpdateInputSite(mouse.Target);
            this._appMousePos = new Point(mouse.X, mouse.Y);
            this._appNaturalTarget = mouse.NaturalHit;
        }

        private QueueItem CheckForInvalidKeyFocus()
        {
            QueueItem queueItem = null;
            if (!this.IsValidInputSite(this._desiredKeyFocus) || !this._inputManager.IsValidKeyFocusSite(this._desiredKeyFocus))
            {
                ++this._keyFocusRepairCount;
                this._revalidateKeyFocus = true;
                queueItem = this.GenerateInvalidKeyFocusCallback();
            }
            else
            {
                int focusRepairCount = (int)this._keyFocusRepairCount;
                this._keyFocusRepairCount = 0U;
                this._revalidateKeyFocus = false;
            }
            return queueItem;
        }

        private void InvalidKeyFocusCallback() => this._inputManager.RepairInvalidKeyFocus(this._keyFocusRepairCount);

        private QueueItem UpdateKeyFocus()
        {
            QueueItem queueItem = null;
            if (this._revalidateKeyFocus || this._currentKeyFocus != this._desiredKeyFocus)
            {
                queueItem = this.CheckForInvalidKeyFocus();
                if (queueItem == null && this._currentKeyFocus != this._desiredKeyFocus)
                {
                    if (this._currentKeyFocus != null)
                    {
                        if (!this.IsValidInputSite(this._currentKeyFocus))
                            this._currentKeyFocus = null;
                        queueItem = this.GenerateKeyFocus(this._currentKeyFocus, false, this._desiredKeyFocus, this._desiredKeyFocusReason);
                        this._currentKeyFocus = null;
                    }
                    if (queueItem == null && this._desiredKeyFocus != null)
                    {
                        this._currentKeyFocus = this._desiredKeyFocus;
                        if (!this.IsValidInputSite(this._lastCompletedKeyFocus))
                            this._lastCompletedKeyFocus = null;
                        queueItem = this.GenerateKeyFocus(this._currentKeyFocus, true, this._lastCompletedKeyFocus, this._desiredKeyFocusReason);
                    }
                    if (this._currentKeyFocus == this._desiredKeyFocus)
                        this._lastCompletedKeyFocus = this._currentKeyFocus;
                }
                if (queueItem == null && this._currentKeyFocus != null)
                    queueItem = this.GenerateKeyFocus(this._currentKeyFocus, true, this._lastCompletedKeyFocus, KeyFocusReason.Validation);
            }
            return queueItem;
        }

        private QueueItem UpdateMouseFocus(
          ICookedInputSite target,
          IRawInputSite site,
          Point clientOffset)
        {
            QueueItem queueItem = null;
            if (this._revalidateMouseFocus || this._appMouseFocusTarget != target)
            {
                this._revalidateMouseFocus = false;
                if (this._appMouseFocusTarget != target)
                {
                    if (this._appMouseFocusTarget != null)
                    {
                        if (!this.IsValidInputSite(this._appMouseFocusTarget))
                            this._appMouseFocusTarget = null;
                        queueItem = this.GenerateMouseFocus(this._appMouseFocusTarget, this._appMouseFocusSite, this._appMousePos.X, this._appMousePos.Y, false, target);
                        this._appMouseFocusSite = null;
                        this._appMouseFocusTarget = null;
                        this._appMousePos.X = -1;
                        this._appMousePos.Y = -1;
                    }
                    if (queueItem == null && target != null)
                    {
                        this._appMouseFocusSite = site;
                        this._appMouseFocusTarget = target;
                        if (!this.IsValidInputSite(this._lastCompletedMouseFocus))
                            this._lastCompletedMouseFocus = null;
                        queueItem = this.GenerateMouseFocus(this._appMouseFocusTarget, this._appMouseFocusSite, clientOffset.X, clientOffset.Y, true, this._lastCompletedMouseFocus);
                    }
                    if (this._appMouseFocusTarget == target)
                        this._lastCompletedMouseFocus = this._appMouseFocusTarget;
                }
                if (queueItem == null && this._appMouseFocusTarget != null)
                    queueItem = this.GenerateMouseFocus(this._appMouseFocusTarget, this._appMouseFocusSite, clientOffset.X, clientOffset.Y, true, this._lastCompletedMouseFocus);
            }
            return queueItem;
        }

        private void UpdateMouseCapture(
          IRawInputSite site,
          ICookedInputSite target,
          InputModifiers modifiers)
        {
            bool flag = (modifiers & InputModifiers.AllButtons) != InputModifiers.None;
            ICookedInputSite cookedInputSite = null;
            if (flag)
                cookedInputSite = target;
            if (this._appMouseFocusCapture == cookedInputSite)
                return;
            this._inputManager.RequestHostMouseCapture(site, cookedInputSite != null);
            this._appMouseFocusCapture = cookedInputSite;
            this.SetMouseMoved();
        }

        private QueueItem UpdateOrphanedButtons(IRawInputSite site, int x, int y)
        {
            MouseButtons button;
            uint message;
            if ((this._appMouseFocusButtons & InputModifiers.LeftMouse) != InputModifiers.None)
            {
                this._appMouseFocusButtons &= ~InputModifiers.LeftMouse;
                button = MouseButtons.Left;
                message = 514U;
            }
            else if ((this._appMouseFocusButtons & InputModifiers.MiddleMouse) != InputModifiers.None)
            {
                this._appMouseFocusButtons &= ~InputModifiers.MiddleMouse;
                button = MouseButtons.Middle;
                message = 520U;
            }
            else if ((this._appMouseFocusButtons & InputModifiers.RightMouse) != InputModifiers.None)
            {
                this._appMouseFocusButtons &= ~InputModifiers.RightMouse;
                button = MouseButtons.Right;
                message = 517U;
            }
            else if ((this._appMouseFocusButtons & InputModifiers.XMouse1) != InputModifiers.None)
            {
                this._appMouseFocusButtons &= ~InputModifiers.XMouse1;
                button = MouseButtons.XButton1;
                message = 524U;
            }
            else if ((this._appMouseFocusButtons & InputModifiers.XMouse2) != InputModifiers.None)
            {
                this._appMouseFocusButtons &= ~InputModifiers.XMouse2;
                button = MouseButtons.XButton2;
                message = 524U;
            }
            else
            {
                this._appMouseFocusButtons = InputModifiers.None;
                return null;
            }
            return this.GenerateMouseButton(site, null, x, y, this._rawScreenPos.X, this._rawScreenPos.Y, this._appMouseFocusButtons, button, false, message);
        }

        private InputItem UpdateDragOver(ICookedInputSite target)
        {
            if (target != this._appDropTarget)
            {
                if (this._appDropTarget != null)
                {
                    ICookedInputSite appDropTarget = this._appDropTarget;
                    this._appDropTarget = null;
                    return this.GenerateDragDrop(appDropTarget, null, this._rawDragPoint.X, this._rawDragPoint.Y, this._rawMouseModifiers, DragOperation.Leave);
                }
                if (target != null)
                {
                    this._appDropTarget = target;
                    return this.GenerateDragDrop(target, this._rawDropTargetSite, this._rawDragPoint.X, this._rawDragPoint.Y, this._rawDragModifiers, DragOperation.Enter);
                }
            }
            return null;
        }

        private bool IsAppMouseMove(MouseActionInfo mouseInfo) => mouseInfo.Target != this._appMouseFocusTarget || mouseInfo.NaturalHit != this._appNaturalTarget || mouseInfo.X != this._appMousePos.X || mouseInfo.Y != this._appMousePos.Y;

        private bool IsValidInputSite(ICookedInputSite target) => target == null || this._inputManager.IsValidCookedInputSite(target);

        private void SetMouseMoved()
        {
            if (this._mouseMoved)
                return;
            this._mouseMoved = true;
            this.OnWake();
        }

        private void CancelMouseCapture(ICookedInputSite cancelSite)
        {
            if (this._appMouseFocusCapture == null || cancelSite != this._appMouseFocusCapture && cancelSite != null)
                return;
            this.UpdateMouseCapture(this._appMouseFocusCapture.RawInputSource, null, InputModifiers.None);
        }

        private QueueItem GenerateGenericInput(InputInfo info) => InputItem.Create(this._inputManager, null, info);

        private QueueItem GenerateInvalidKeyFocusCallback() => DeferredCall.Create(this._invalidKeyFocusCallback);

        private QueueItem GenerateKeyFocus(
          ICookedInputSite target,
          bool focus,
          ICookedInputSite other,
          KeyFocusReason reason)
        {
            return InputItem.Create(this._inputManager, target, KeyFocusInfo.Create(focus, other, reason));
        }

        private InputItem GenerateMouseFocus(
          ICookedInputSite target,
          IRawInputSite site,
          int x,
          int y,
          bool focus,
          ICookedInputSite other)
        {
            return InputItem.Create(this._inputManager, target, MouseFocusInfo.Create(site, x, y, focus, other));
        }

        private InputItem GenerateMouseMove(
          IRawInputSite site,
          IRawInputSite naturalSite,
          int x,
          int y,
          int screenX,
          int screenY,
          InputModifiers modifiers)
        {
            return InputItem.Create(this._inputManager, null, MouseMoveInfo.Create(site, naturalSite, x, y, screenX, screenY, modifiers));
        }

        private InputItem GenerateMouseButton(
          IRawInputSite site,
          IRawInputSite naturalSite,
          int x,
          int y,
          int screenX,
          int screenY,
          InputModifiers modifiers,
          MouseButtons button,
          bool state,
          uint message)
        {
            return InputItem.Create(this._inputManager, null, MouseButtonInfo.Create(site, naturalSite, x, y, screenX, screenY, modifiers, button, state, message));
        }

        private InputItem GenerateMouseWheel(
          IRawInputSite site,
          IRawInputSite naturalSite,
          int x,
          int y,
          int screenX,
          int screenY,
          InputModifiers modifiers,
          int wheelDelta)
        {
            return InputItem.Create(this._inputManager, null, MouseWheelInfo.Create(site, naturalSite, x, y, screenX, screenY, modifiers, wheelDelta));
        }

        private InputItem GenerateDragDrop(
          ICookedInputSite cookedSite,
          IRawInputSite rawSite,
          int x,
          int y,
          InputModifiers modifiers,
          DragOperation operation)
        {
            InputInfo info = DragDropInfo.Create(rawSite, x, y, modifiers, operation);
            return InputItem.Create(this._inputManager, cookedSite, info);
        }
    }
}
