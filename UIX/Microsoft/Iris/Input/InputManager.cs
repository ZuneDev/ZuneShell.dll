// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Input.InputManager
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Debug;
using Microsoft.Iris.Library;
using Microsoft.Iris.OS;
using Microsoft.Iris.Queues;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.Input
{
    internal class InputManager : IRawInputCallbacks
    {
        private InputModifiers _HACK_sysModifiers;
        private UISession _session;
        private InputQueue _inputQueue;
        private int _inputSuspendCount;
        private readonly SimpleCallback _resumeInput;
        private KeyboardDevice _keyboardDevice;
        private MouseDevice _mouseDevice;
        private HidDevice _remoteDevice;
        private DateTime _lastInputTime;
        private bool _keyFocusCanBeNull;
        private bool _ignoreHungKeyFocus;
        private ICookedInputSite _ignoreHungKeyFocusTarget;
        private bool _inputDisabled;
        private KeyCoalesceFilter _keyCoalescePolicy;
        private KeyStateInfo _currentCoalesceKeyEvent;
        private bool _currentCoalesceUndelivered;
        private Point _physicalMouseOffset;
        private bool _refreshHitTargetPending;
        private readonly SimpleCallback _refreshHitTargetHandler;
        private UIZone _mouseFocusZone;
        private UIZone _keyFocusZone;
        private static readonly DeferredHandler s_deliverCoalescedKey = new DeferredHandler(DeliverCoalescedKey);

        internal InputManager(UISession session)
        {
            this._session = session;
            this._keyFocusCanBeNull = true;
            this._lastInputTime = DateTime.MinValue;
            this._inputQueue = new InputQueue(this);
            this._resumeInput = new SimpleCallback(this.ResumeInput);
            this._refreshHitTargetHandler = new SimpleCallback(this.RefreshHitTargetHandler);
        }

        internal void ConnectToRenderer() => this.Session.RenderSession.InputSystem.RegisterRawInputCallbacks(this);

        internal void PrepareToShutDown()
        {
            this.KeyCoalescePolicy = null;
            this.EndKeyCoalesce();
            this.InvalidKeyFocus = null;
            this._inputDisabled = true;
            this.Session.RenderSession.InputSystem.UnregisterRawInputCallbacks();
            this._inputQueue.PrepareToShutDown();
        }

        internal UISession Session => this._session;

        internal InputQueue Queue => this._inputQueue;

        internal InputModifiers Modifiers => this.Keyboard.KeyboardModifiers | this.Mouse.MouseModifiers;

        internal InputModifiers DragModifiers => this._inputQueue.DragModifiers;

        internal KeyboardDevice Keyboard
        {
            get
            {
                if (this._keyboardDevice == null)
                    this._keyboardDevice = new KeyboardDevice(this);
                return this._keyboardDevice;
            }
        }

        internal MouseDevice Mouse
        {
            get
            {
                if (this._mouseDevice == null)
                    this._mouseDevice = new MouseDevice(this);
                return this._mouseDevice;
            }
        }

        internal HidDevice Remote
        {
            get
            {
                if (this._remoteDevice == null)
                    this._remoteDevice = new HidDevice(this);
                return this._remoteDevice;
            }
        }

        public DateTime LastInputTime => this._lastInputTime;

        public bool InputEnabled => !this._inputDisabled;

        public bool KeyFocusCanBeNull
        {
            get => this._inputDisabled || this._keyFocusCanBeNull;
            set
            {
                if (this._keyFocusCanBeNull == value)
                    return;
                this._keyFocusCanBeNull = value;
                if (value)
                    return;
                this._inputQueue.RevalidateInputSiteUsage(null, false);
            }
        }

        public ICookedInputSite RawKeyFocus => this._inputQueue.PendingKeyFocus;

        public ICookedInputSite RawInstantaneousKeyFocus => this._inputQueue.InstantaneousKeyFocus;

        public bool RawKeyFocusIsDefault => this._inputQueue.PendingKeyFocusIsDefault;

        public KeyCoalesceFilter KeyCoalescePolicy
        {
            get => this._keyCoalescePolicy;
            set => this._keyCoalescePolicy = value;
        }

        public Point MostRecentPhysicalMousePos
        {
            get => this._physicalMouseOffset;
            set
            {
                if (!(this._physicalMouseOffset != value))
                    return;
                this._physicalMouseOffset = value;
                if (this.MousePositionChanged == null)
                    return;
                this.MousePositionChanged(this, EventArgs.Empty);
            }
        }

        public event InvalidKeyFocusHandler InvalidKeyFocus;

        public event EventHandler MousePositionChanged;

        public event InputNotificationHandler PreviewInput;

        public event InputNotificationHandler HandledInput;

        public event InputNotificationHandler UnhandledInput;

        public void SuspendInputUntil(DispatchPriority unlockPriority)
        {
            DeferredCall.Post(unlockPriority, this._resumeInput);
            this.Session.Dispatcher.BlockInputQueue(true);
            ++this._inputSuspendCount;
        }

        private void ResumeInput()
        {
            if (this._inputSuspendCount <= 0)
                return;
            --this._inputSuspendCount;
            if (this._inputSuspendCount != 0)
                return;
            this.Session.Dispatcher.BlockInputQueue(false);
        }

        internal void UpdateLastInputTime() => this._lastInputTime = DateTime.UtcNow;

        public InputHandlerFlags InputHandlerMask => InputHandlerFlags.All;

        public void HandleRawKeyboardInput(
          uint message,
          InputModifiers modifiers,
          ref RawKeyboardData args)
        {
            KeyInfo info = this.Keyboard.OnRawInput(message, modifiers, ref args);
            if (info != null)
                this._inputQueue.RawKeyAction(info);
            this.UpdateLastInputTime();
        }

        public void HandleRawMouseInput(uint message, InputModifiers modifiers, ref RawMouseData args)
        {
            this.Mouse.OnRawInput(message, modifiers, ref args);
            this.UpdateLastInputTime();
        }

        public void HandleRawHidInput(ref RawHidData args)
        {
            KeyActionInfo keyActionInfo = this.Remote.OnRawInput(HIDCommandMapping.Find(args._commandCode, args._usagePage), ref args);
            if (keyActionInfo != null)
                this._inputQueue.RawKeyAction(keyActionInfo);
            this.UpdateLastInputTime();
        }

        public void HandleAppCommand(ref RawHidData args)
        {
            KeyActionInfo keyActionInfo = this.Remote.OnRawInput(AppCommandMapping.Find(args._commandCode), ref args);
            if (keyActionInfo != null)
                this._inputQueue.RawKeyAction(keyActionInfo);
            this.UpdateLastInputTime();
        }

        public void HandleRawDragInput(uint message, InputModifiers modifiers, ref RawDragData args)
        {
            using (DataObject dataObject = new DataObject(args._pDataStream))
            {
                object data = null;
                if (message == 0U)
                    data = dataObject.GetExternalData();
                this.Mouse.OnRawInput(message, modifiers, ref args, data);
            }
            this.UpdateLastInputTime();
        }

        internal void RevalidateInputSiteUsage(
          ICookedInputSite target,
          bool recursiveFlag,
          bool knownDisabledFlag)
        {
            if (this._ignoreHungKeyFocus && !knownDisabledFlag)
            {
                this.StopIgnoringHungKeyFocus();
                target = null;
                recursiveFlag = false;
            }
            this._inputQueue.RevalidateInputSiteUsage(target, recursiveFlag);
            if (this._refreshHitTargetPending)
                return;
            this._refreshHitTargetPending = true;
            DeferredCall.Post(DispatchPriority.RenderSync, this._refreshHitTargetHandler);
        }

        private void RefreshHitTargetHandler()
        {
            this._refreshHitTargetPending = false;
            this.Session.Form.RefreshHitTarget();
        }

        public bool IsPendingKeyFocusValid() => this.IsValidKeyFocusWorker(this._inputQueue.PendingKeyFocus);

        public void SimulateDragEnter(
          ICookedInputSite dragSource,
          IRawInputSite rawTargetSite,
          object data,
          int x,
          int y,
          InputModifiers modifiers)
        {
            this._inputQueue.SimulateDragEnter(dragSource, rawTargetSite, data, x, y, modifiers);
        }

        public void SimulateDragOver(InputModifiers modifiers) => this._inputQueue.SimulateDragOver(modifiers);

        public void SimulateDragOver(
          IRawInputSite rawTargetSite,
          int x,
          int y,
          InputModifiers modifiers)
        {
            this._inputQueue.SimulateDragOver(rawTargetSite, x, y, modifiers);
        }

        public void SimulateDragEnd(
          IRawInputSite rawTargetSite,
          InputModifiers modifiers,
          DragOperation formOperation)
        {
            this._inputQueue.SimulateDragEnd(rawTargetSite, modifiers, formOperation);
        }

        public object GetDragDropValue() => this._inputQueue.GetDragDropValue();

        internal bool FilterKeyboardEvent(KeyStateInfo info)
        {
            switch (info.Action)
            {
                case KeyAction.Up:
                    this.EndKeyCoalesce();
                    break;
                case KeyAction.Down:
                    if (info.RepeatCount == 2U)
                    {
                        this.BeginKeyCoalesce(info);
                        break;
                    }
                    if (info.RepeatCount > 2U && this.CoalesceRepeatedKey(info))
                        return false;
                    this.EndKeyCoalesce();
                    break;
            }
            return true;
        }

        private bool BeginKeyCoalesce(KeyStateInfo info)
        {
            this.EndKeyCoalesce();
            if (this._keyCoalescePolicy == null || !this._keyCoalescePolicy(info.Key))
                return false;
            this.SetCoalesceKeyEvent(info.MakeRepeatableCopy());
            return true;
        }

        private void SetCoalesceKeyEvent(KeyStateInfo info)
        {
            if (this._currentCoalesceKeyEvent != null)
                this._currentCoalesceKeyEvent.Unlock();
            this._currentCoalesceKeyEvent = info;
            if (this._currentCoalesceKeyEvent == null)
                return;
            this._currentCoalesceKeyEvent.Lock();
        }

        private bool CoalesceRepeatedKey(KeyStateInfo info)
        {
            if (!info.IsRepeatOf(this._currentCoalesceKeyEvent))
                return false;
            this.SetCoalesceKeyEvent(info);
            if (!this._currentCoalesceUndelivered)
            {
                this._currentCoalesceUndelivered = true;
                this._inputQueue.RawInputIdleItem(DeferredCall.Create(s_deliverCoalescedKey, this));
            }
            return true;
        }

        private void EndKeyCoalesce()
        {
            this.SetCoalesceKeyEvent(null);
            this._currentCoalesceUndelivered = false;
        }

        private static void DeliverCoalescedKey(object args)
        {
            InputManager inputManager = (InputManager)args;
            if (!inputManager._currentCoalesceUndelivered)
                return;
            inputManager._currentCoalesceUndelivered = false;
            ICookedInputSite instantaneousKeyFocus = inputManager._inputQueue.InstantaneousKeyFocus;
            inputManager.DeliverInputWorker(instantaneousKeyFocus, inputManager._currentCoalesceKeyEvent, EventRouteStages.All);
            inputManager.SuspendInputUntil(DispatchPriority.Idle);
        }

        public void MapMouseInput(MouseActionInfo info, ICookedInputSite captureSite)
        {
            ICookedInputSite target = this.HitTestInput(info.RawSource, captureSite);
            if (!this.IsValidCookedInputSite(target))
                target = null;
            IRawInputSite naturalHit = info.NaturalHit;
            ICookedInputSite naturalTarget = null;
            if (naturalHit != null)
                naturalTarget = naturalHit != info.RawSource ? (naturalHit is ITreeNode treeNode ? treeNode.Zone?.MapInput(naturalHit, null) : null) : target;
            info.SetMappedTargets(target, naturalTarget);
        }

        internal ICookedInputSite HitTestInput(
          IRawInputSite rawSource,
          ICookedInputSite targetRelative)
        {
            ICookedInputSite cookedInputSite = null;
            if (!this._inputDisabled)
            {
                ITreeNode treeNode = null;
                if (targetRelative != null)
                    treeNode = targetRelative as ITreeNode;
                else if (rawSource != null)
                    treeNode = rawSource.OwnerData as ITreeNode;
                if (treeNode != null)
                {
                    UIZone zone = treeNode.Zone;
                    if (zone != null)
                        cookedInputSite = zone.MapInput(rawSource, targetRelative);
                }
            }
            return cookedInputSite;
        }

        internal bool IsValidCookedInputSite(ICookedInputSite target) => target == null || (target as ITreeNode).Zone != null;

        internal bool IsSiteInfluencedByPeer(ICookedInputSite target, ICookedInputSite peer)
        {
            if (!(peer is ITreeNode potentialParent))
                return false;
            UIZone zone = potentialParent.Zone;
            return zone != null && target is ITreeNode potentialChild && potentialChild.Zone == zone && zone.IsChildADescendant(potentialParent, potentialChild);
        }

        internal bool IsValidKeyFocusSite(ICookedInputSite target)
        {
            bool flag = this.IsValidKeyFocusWorker(target);
            if (this._ignoreHungKeyFocus)
            {
                if (flag)
                    this.StopIgnoringHungKeyFocus();
                else if (target == this._ignoreHungKeyFocusTarget)
                    flag = true;
                else
                    this.StopIgnoringHungKeyFocus();
            }
            return flag;
        }

        private bool IsValidKeyFocusWorker(ICookedInputSite candidate)
        {
            bool flag = this.KeyFocusCanBeNull;
            if (candidate is ITreeNode child)
            {
                UIZone zone = child.Zone;
                if (zone != null)
                    flag = zone.IsChildKeyFocusable(child);
            }
            return flag;
        }

        internal void RepairInvalidKeyFocus(uint attemptCount)
        {
            if (this._inputQueue.PendingKeyFocus is ITreeNode pendingKeyFocus && pendingKeyFocus.Zone == null)
            {
                this._inputQueue.RequestKeyFocus(null, KeyFocusReason.Default);
                if (this._keyFocusCanBeNull)
                    return;
            }
            if (attemptCount <= 1U)
            {
                this.SuspendInputUntil(DispatchPriority.LayoutSync);
            }
            else
            {
                if (this.InvalidKeyFocus != null)
                    this.InvalidKeyFocus(this._inputQueue.LastCompletedKeyFocus);
                ICookedInputSite pendingKeyFocusB = this._inputQueue.PendingKeyFocus;
                if (this.IsValidKeyFocusWorker(pendingKeyFocusB))
                    return;
                if (this._keyFocusCanBeNull)
                {
                    this._inputQueue.RequestKeyFocus(null, KeyFocusReason.Default);
                }
                else
                {
                    this._ignoreHungKeyFocus = true;
                    this._ignoreHungKeyFocusTarget = pendingKeyFocusB;
                }
            }
        }

        private void StopIgnoringHungKeyFocus()
        {
            this._ignoreHungKeyFocus = false;
            this._ignoreHungKeyFocusTarget = null;
        }

        internal void RequestHostKeyFocus(ICookedInputSite target)
        {
            if (!this._session.IsValid || target is IInputCustomFocus inputCustomFocus && inputCustomFocus.OverrideHostFocus() || this._session.Form == null)
                return;
            this._session.Form.TakeFocus();
        }

        internal void RequestHostMouseCapture(IRawInputSite rawSource, bool state)
        {
            if (this._session.Form == null)
                return;
            this._session.Form.SetCapture(rawSource, state);
        }

        internal void DeliverInput(ICookedInputSite target, InputInfo info)
        {
            if (info is KeyStateInfo info1 && !this.FilterKeyboardEvent(info1))
                return;
            this.DeliverInputWorker(target, info, EventRouteStages.All);
        }

        public void ForwardInput(ICookedInputSite target, InputInfo info) => this.DeliverInputWorker(target, info, EventRouteStages.Direct);

        private void DeliverInputWorker(
          ICookedInputSite target,
          InputInfo info,
          EventRouteStages stages)
        {
            if (this._inputDisabled)
                return;
            InputManager.ZoneDeliveryInfo inputZoneRouting = this.ComputeInputZoneRouting(target, info);
            if (this.CheckFocus(target, ref inputZoneRouting, info) && inputZoneRouting.zone != null)
            {
                byte traceLevelForEvent = Trace.GetTraceLevelForEvent(info);
                EventRouteStages stage = EventRouteStages.None;
                if (this.PreviewInput != null && (stages & EventRouteStages.Preview) != EventRouteStages.None)
                {
                    this.PreviewInput(this, new InputNotificationEventArgs(info, target, stage));
                    if (info.Handled)
                        stage = EventRouteStages.Preview;
                }
                if (!info.Handled && (stages & EventRouteStages.Routed) != EventRouteStages.None)
                {
                    int num = (int)this.DeliverInputStageWorker(info, EventRouteStages.Routed, ref inputZoneRouting, traceLevelForEvent);
                    if (info.Handled)
                        stage = EventRouteStages.Routed;
                }
                if (!info.Handled && (stages & EventRouteStages.Direct) != EventRouteStages.None)
                {
                    int num = (int)this.DeliverInputStageWorker(info, EventRouteStages.Direct, ref inputZoneRouting, traceLevelForEvent);
                    if (info.Handled)
                        stage = EventRouteStages.Direct;
                }
                if (!info.Handled && (stages & EventRouteStages.Bubbled) != EventRouteStages.None)
                {
                    int num = (int)this.DeliverInputStageWorker(info, EventRouteStages.Bubbled, ref inputZoneRouting, traceLevelForEvent);
                    if (info.Handled)
                        stage = EventRouteStages.Bubbled;
                }
                if (!info.Handled && (stages & EventRouteStages.Unhandled) != EventRouteStages.None)
                {
                    int num = (int)this.DeliverInputStageWorker(info, EventRouteStages.Unhandled, ref inputZoneRouting, traceLevelForEvent);
                    if (info.Handled)
                        stage = EventRouteStages.Unhandled;
                }
                InputNotificationHandler notificationHandler = !info.Handled ? this.UnhandledInput : this.HandledInput;
                if (notificationHandler != null)
                    notificationHandler(this, new InputNotificationEventArgs(info, target, stage));
            }
            if (inputZoneRouting.zone == null)
                return;
            inputZoneRouting.zone.RecycleInputDeliveryData(inputZoneRouting.param);
        }

        private InputDeliveryStatus DeliverInputStageWorker(
          InputInfo input,
          EventRouteStages stage,
          ref InputManager.ZoneDeliveryInfo deliveryInfo,
          byte traceLevel)
        {
            InputDeliveryStatus inputDeliveryStatus = InputDeliveryStatus.Normal;
            if (deliveryInfo.param != null)
            {
                bool handled = input.Handled;
                inputDeliveryStatus = deliveryInfo.zone.DeliverInput(deliveryInfo.param, stage);
                int num1 = input.Handled ? 1 : 0;
                int num2 = handled ? 1 : 0;
            }
            deliveryInfo.stagesDelivered |= stage;
            return inputDeliveryStatus;
        }

        private InputManager.ZoneDeliveryInfo ComputeInputZoneRouting(
          ICookedInputSite finalTarget,
          InputInfo info)
        {
            ITreeNode endpoint = null;
            UIZone uiZone = null;
            if (finalTarget is ITreeNode)
            {
                endpoint = (ITreeNode)finalTarget;
                uiZone = endpoint.Zone;
            }
            if (uiZone == null)
                return new InputManager.ZoneDeliveryInfo();
            return new InputManager.ZoneDeliveryInfo()
            {
                zone = uiZone,
                param = uiZone.PrepareInputForDelivery(endpoint, finalTarget, info)
            };
        }

        private bool CheckFocus(
          ICookedInputSite target,
          ref InputManager.ZoneDeliveryInfo deliveryInfo,
          InputInfo info)
        {
            bool flag = true;
            switch (info)
            {
                case MouseFocusInfo mouseFocusInfo:
                    if (mouseFocusInfo.State || mouseFocusInfo.Other == null)
                    {
                        InputManager.ZoneDeliveryInfo newFocusInfo = new InputManager.ZoneDeliveryInfo();
                        if (mouseFocusInfo.State)
                            newFocusInfo = deliveryInfo;
                        ProcessFocusUpdates(InputDeviceType.Mouse, ref this._mouseFocusZone, newFocusInfo, target as ITreeNode);
                        this._session.RootZone.UpdateCursor(null);
                    }
                    if (mouseFocusInfo.State && target == mouseFocusInfo.Other)
                        flag = false;
                    return flag;
                case KeyFocusInfo keyFocusInfo:
                    if (keyFocusInfo.State || keyFocusInfo.Other == null)
                    {
                        InputManager.ZoneDeliveryInfo newFocusInfo = new InputManager.ZoneDeliveryInfo();
                        if (keyFocusInfo.State)
                            newFocusInfo = deliveryInfo;
                        ProcessFocusUpdates(InputDeviceType.Keyboard, ref this._keyFocusZone, newFocusInfo, target as ITreeNode);
                    }
                    if (keyFocusInfo.State && target == keyFocusInfo.Other)
                        flag = false;
                    return flag;
                default:
                    return flag;
            }
        }

        private static void ProcessFocusUpdates(
          InputDeviceType focusType,
          ref UIZone refCurrentFocusZone,
          InputManager.ZoneDeliveryInfo newFocusInfo,
          ITreeNode actualFocus)
        {
            UIZone zone = refCurrentFocusZone;
            if (zone == null && newFocusInfo.zone == null)
                return;
            refCurrentFocusZone = newFocusInfo.zone;
            if (refCurrentFocusZone == null)
                UpdateZoneFocusStates(focusType, zone, null, false, null);
            if (newFocusInfo.zone == null)
                return;
            UpdateZoneFocusStates(focusType, newFocusInfo.zone, newFocusInfo.param, true, actualFocus);
        }

        private static void UpdateZoneFocusStates(
          InputDeviceType focusType,
          UIZone zone,
          object param,
          bool deepFocusFlag,
          ITreeNode actualFocus)
        {
            ITreeNode directFocusChild = null;
            object obj = null;
            if (deepFocusFlag)
            {
                if (actualFocus != null && actualFocus.Zone == zone)
                    directFocusChild = actualFocus;
                obj = param;
            }
            zone.UpdateInputFocusStates(focusType, deepFocusFlag, directFocusChild, obj);
        }

        internal InputModifiers HACK_SystemModifiers => this._HACK_sysModifiers;

        internal void HACK_UpdateSystemModifiers(InputModifiers modifiers) => this._HACK_sysModifiers = modifiers;

        private struct ZoneDeliveryInfo
        {
            public UIZone zone;
            public object param;
            public EventRouteStages stagesDelivered;
        }
    }
}
