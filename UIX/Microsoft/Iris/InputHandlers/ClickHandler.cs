// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.InputHandlers.ClickHandler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Markup;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.OS;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.InputHandlers
{
    internal class ClickHandler : ModifierInputHandler
    {
        private static int s_defaultRepeatDelay = Win32Api.GetDefaultKeyDelay();
        private static int s_defaultRepeatRate = Win32Api.GetDefaultKeyRepeat();
        private ClickCount _clickCount;
        private ClickType _clickType;
        private int _repeatDelay;
        private int _repeatRate;
        private IUICommand _command;
        private WeakReference _eventContext;
        private ClickType _clickTypeInProgress;
        private DispatcherTimer _repeatTimer;
        private bool _clickValidPosition;
        private bool _handleEscape;
        private bool _handle;
        private bool _repeat;

        public ClickHandler()
        {
            this._clickType = ClickType.Key | ClickType.GamePad | ClickType.LeftMouse;
            this._clickCount = ClickCount.Single;
            this._repeat = true;
            this._repeatDelay = ClickHandler.DefaultRepeatDelay;
            this._repeatRate = ClickHandler.DefaultRepeatRate;
            this._handle = true;
        }

        protected override void OnDispose()
        {
            if (this._repeatTimer != null)
                this._repeatTimer.Enabled = false;
            base.OnDispose();
        }

        protected override void ConfigureInteractivity()
        {
            base.ConfigureInteractivity();
            if (!this.HandleDirect)
                return;
            if (this.ShouldHandleEvent(ClickType.Mouse))
                this.UI.MouseInteractive = true;
            if (!this.ShouldHandleEvent(ClickType.Key | ClickType.GamePad))
                return;
            this.UI.KeyInteractive = true;
        }

        public ClickType ClickType
        {
            get => this._clickType;
            set
            {
                if (this._clickType == value)
                    return;
                this._clickType = value;
                this.FireNotification(NotificationID.ClickType);
            }
        }

        public ClickCount ClickCount
        {
            get => this._clickCount;
            set
            {
                if (this._clickCount == value)
                    return;
                this._clickCount = value;
                this.FireNotification(NotificationID.ClickCount);
            }
        }

        public bool Repeat
        {
            get => this._repeat;
            set
            {
                if (this._repeat == value)
                    return;
                this._repeat = value;
                this.FireNotification(NotificationID.Repeat);
            }
        }

        public int RepeatDelay
        {
            get => this._repeatDelay;
            set
            {
                if (this._repeatDelay == value)
                    return;
                this._repeatDelay = value;
                this.FireNotification(NotificationID.RepeatDelay);
            }
        }

        public int RepeatRate
        {
            get => this._repeatRate;
            set
            {
                if (this._repeatRate == value)
                    return;
                this._repeatRate = value;
                this.FireNotification(NotificationID.RepeatRate);
            }
        }

        public bool Clicking => this._clickTypeInProgress != ClickType.None && this._clickValidPosition;

        public object EventContext => this.CheckEventContext(ref this._eventContext);

        public IUICommand Command
        {
            get => this._command;
            set
            {
                if (this._command == value)
                    return;
                this._command = value;
                this.FireNotification(NotificationID.Command);
            }
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

        private void InvokeCommand()
        {
            this.FireNotification(NotificationID.Invoked);
            if (this._command == null)
                return;
            this._command.Invoke();
        }

        private void BeginClick(ClickType clickType, bool validPosition)
        {
            if (this._clickTypeInProgress == ClickType.None)
            {
                this._clickTypeInProgress = clickType;
                this._clickValidPosition = validPosition;
                if (validPosition)
                    this.FireNotification(NotificationID.Clicking);
            }
            else if (this._clickTypeInProgress != clickType)
                this.CancelClick(ClickType.Any);
            this.SetEventContext(null, ref this._eventContext, NotificationID.EventContext);
        }

        private void EndClick(ICookedInputSite clickTarget, ClickType clickType)
        {
            if (this._clickTypeInProgress == ClickType.None)
                return;
            this.SetEventContext(clickTarget, ref this._eventContext, NotificationID.EventContext);
            if (this._clickTypeInProgress == clickType && this._clickValidPosition)
                this.InvokeCommand();
            this.CancelClick(ClickType.Any);
        }

        private void UpdateClickValidPosition(bool validPosition)
        {
            bool clicking = this.Clicking;
            this._clickValidPosition = validPosition;
            if (this.Clicking == clicking)
                return;
            this.FireNotification(NotificationID.Clicking);
        }

        private void CancelClick(ClickType clickType)
        {
            if (this._clickTypeInProgress != ClickType.None && Microsoft.Iris.Library.Bits.TestAllFlags((uint)clickType, (uint)this._clickTypeInProgress))
            {
                bool clicking = this.Clicking;
                this._clickTypeInProgress = ClickType.None;
                if (this.Clicking != clicking)
                    this.FireNotification(NotificationID.Clicking);
            }
            if (this._repeatTimer == null)
                return;
            this._repeatTimer.Enabled = false;
        }

        private bool ShouldHandleEvent(
          ClickType type,
          InputHandlerModifiers modifiers,
          ClickCount count)
        {
            return this.ShouldHandleEvent(type) && this._clickCount == count && this.ShouldHandleEvent(modifiers);
        }

        private bool ShouldHandleEvent(ClickType type) => Microsoft.Iris.Library.Bits.TestAnyFlags((uint)this._clickType, (uint)type);

        private bool OnClickEvent(
          ICookedInputSite clickTarget,
          ClickType type,
          InputHandlerTransition transition,
          InputHandlerModifiers modifiers)
        {
            return this.OnClickEvent(clickTarget, type, transition, modifiers, ClickCount.Single);
        }

        private bool OnClickEvent(
          ICookedInputSite clickTarget,
          ClickType type,
          InputHandlerTransition transition,
          InputHandlerModifiers modifiers,
          ClickCount count)
        {
            if (!this.ShouldHandleEvent(type, modifiers, count))
                return false;
            if (transition == InputHandlerTransition.Up)
            {
                if (this.HandlerTransition != InputHandlerTransition.Down)
                    this.EndClick(clickTarget, type);
                if (this._repeatTimer != null)
                    this._repeatTimer.Enabled = false;
            }
            else
            {
                this.BeginClick(type, true);
                if (this.HandlerTransition == InputHandlerTransition.Down)
                {
                    this.EndClick(clickTarget, type);
                    this.StartRepeat(new ClickHandler.ClickInfo()
                    {
                        type = type,
                        transition = transition,
                        modifiers = modifiers,
                        count = count,
                        target = clickTarget
                    });
                }
            }
            return true;
        }

        private ClickType GetClickType(MouseButtons buttons)
        {
            ClickType clickType = ClickType.None;
            if ((buttons & MouseButtons.Left) != MouseButtons.None)
                clickType |= ClickType.LeftMouse;
            if ((buttons & MouseButtons.Right) != MouseButtons.None)
                clickType |= ClickType.RightMouse;
            if ((buttons & MouseButtons.Middle) != MouseButtons.None)
                clickType |= ClickType.MiddleMouse;
            return clickType;
        }

        private void StartRepeat(ClickHandler.ClickInfo clickInfo)
        {
            if (!this.Repeat)
                return;
            if (this._repeatTimer == null)
            {
                this._repeatTimer = new DispatcherTimer();
                this._repeatTimer.Tick += new EventHandler(this.SimulateClickEvent);
                this._repeatTimer.AutoRepeat = true;
            }
            this._repeatTimer.Interval = this.RepeatDelay;
            this._repeatTimer.UserData = clickInfo;
            this._repeatTimer.Enabled = true;
        }

        private void SimulateClickEvent(object sender, EventArgs args)
        {
            bool flag = false;
            ClickHandler.ClickInfo userData = (ClickHandler.ClickInfo)this._repeatTimer.UserData;
            if (this.Enabled && this.Repeat && this._clickValidPosition && (userData.target == null || userData.target.IsValid))
            {
                this.OnClickEvent(userData.target, userData.type, userData.transition, userData.modifiers, userData.count);
                flag = true;
            }
            if (flag)
            {
                this._repeatTimer.Enabled = false;
                this._repeatTimer = null;
                this.StartRepeat(userData);
                this._repeatTimer.Interval = this.RepeatRate;
            }
            else
            {
                this._repeatTimer.Enabled = false;
                this._repeatTimer = null;
            }
        }

        private static int DefaultRepeatDelay => ClickHandler.s_defaultRepeatDelay;

        private static int DefaultRepeatRate => ClickHandler.s_defaultRepeatRate;

        protected override void OnMousePrimaryDown(UIClass ui, MouseButtonInfo info)
        {
            if (!this.OnClickEvent(info.Target, this.GetClickType(info.Button), InputHandlerTransition.Down, InputHandler.GetModifiers(info.Modifiers)) || !this._handle)
                return;
            info.MarkHandled();
        }

        protected override void OnMouseSecondaryDown(UIClass ui, MouseButtonInfo info)
        {
            if (!this.OnClickEvent(info.Target, this.GetClickType(info.Button), InputHandlerTransition.Down, InputHandler.GetModifiers(info.Modifiers)) || !this._handle)
                return;
            info.MarkHandled();
        }

        protected override void OnMousePrimaryUp(UIClass ui, MouseButtonInfo info)
        {
            if (!this.OnClickEvent(info.Target, this.GetClickType(info.Button), InputHandlerTransition.Up, InputHandler.GetModifiers(info.Modifiers)) || !this._handle)
                return;
            info.MarkHandled();
        }

        protected override void OnMouseSecondaryUp(UIClass ui, MouseButtonInfo info)
        {
            if (!this.OnClickEvent(info.Target, this.GetClickType(info.Button), InputHandlerTransition.Up, InputHandler.GetModifiers(info.Modifiers)) || !this._handle)
                return;
            info.MarkHandled();
        }

        protected override void OnMouseDoubleClick(UIClass ui, MouseButtonInfo info)
        {
            ClickType clickType = this.GetClickType(info.Button);
            if (!this.OnClickEvent(info.Target, clickType, InputHandlerTransition.Down, InputHandler.GetModifiers(info.Modifiers), ClickCount.Double))
                return;
            this.OnClickEvent(info.Target, clickType, InputHandlerTransition.Up, InputHandler.GetModifiers(info.Modifiers), ClickCount.Double);
            if (!this._handle)
                return;
            info.MarkHandled();
        }

        protected override void OnMouseMove(UIClass ui, MouseMoveInfo info)
        {
            if (!this.ShouldHandleEvent(ClickType.Mouse))
                return;
            this.UpdateClickValidPosition(this.UI.HasDescendant(info.NaturalTarget as UIClass));
        }

        protected override void OnLoseMouseFocus(UIClass ui, MouseFocusInfo info)
        {
            if (!this.ShouldHandleEvent(ClickType.Mouse))
                return;
            this.CancelClick(ClickType.Mouse);
        }

        protected override void OnKeyDown(UIClass ui, KeyStateInfo info)
        {
            if (info.RepeatCount > 1U)
                return;
            switch (info.Key)
            {
                case Keys.Enter:
                    if (!this.OnClickEvent(info.Target, ClickType.EnterKey, InputHandlerTransition.Down, InputHandler.GetModifiers(info.Modifiers)) || !this._handle)
                        break;
                    info.MarkHandled();
                    break;
                case Keys.Escape:
                    if (!this.Clicking)
                        break;
                    this.CancelClick(ClickType.Any);
                    if (this._handle)
                        info.MarkHandled();
                    this._handleEscape = true;
                    break;
                case Keys.Space:
                    if (!this.OnClickEvent(info.Target, ClickType.SpaceKey, InputHandlerTransition.Down, InputHandler.GetModifiers(info.Modifiers)) || !this._handle)
                        break;
                    info.MarkHandled();
                    break;
                case Keys.GamePadA:
                    if (!this.OnClickEvent(info.Target, ClickType.GamePadA, InputHandlerTransition.Down, InputHandler.GetModifiers(info.Modifiers)) || !this._handle)
                        break;
                    info.MarkHandled();
                    break;
                case Keys.GamePadStart:
                    if (!this.OnClickEvent(info.Target, ClickType.GamePadStart, InputHandlerTransition.Down, InputHandler.GetModifiers(info.Modifiers)) || !this._handle)
                        break;
                    info.MarkHandled();
                    break;
            }
        }

        protected override void OnKeyUp(UIClass ui, KeyStateInfo info)
        {
            switch (info.Key)
            {
                case Keys.Enter:
                    if (!this.OnClickEvent(info.Target, ClickType.EnterKey, InputHandlerTransition.Up, InputHandler.GetModifiers(info.Modifiers)) || !this._handle)
                        break;
                    info.MarkHandled();
                    break;
                case Keys.Escape:
                    if (!this._handleEscape)
                        break;
                    if (this._handle)
                        info.MarkHandled();
                    this._handleEscape = false;
                    break;
                case Keys.Space:
                    if (!this.OnClickEvent(info.Target, ClickType.SpaceKey, InputHandlerTransition.Up, InputHandler.GetModifiers(info.Modifiers)) || !this._handle)
                        break;
                    info.MarkHandled();
                    break;
                case Keys.GamePadA:
                    if (!this.OnClickEvent(info.Target, ClickType.GamePadA, InputHandlerTransition.Up, InputHandler.GetModifiers(info.Modifiers)) || !this._handle)
                        break;
                    info.MarkHandled();
                    break;
                case Keys.GamePadStart:
                    if (!this.OnClickEvent(info.Target, ClickType.GamePadStart, InputHandlerTransition.Up, InputHandler.GetModifiers(info.Modifiers)) || !this._handle)
                        break;
                    info.MarkHandled();
                    break;
            }
        }

        protected override void OnKeyCharacter(UIClass ui, KeyCharacterInfo info)
        {
            switch (info.Character)
            {
                case '\r':
                    if (!this.ShouldHandleEvent(ClickType.EnterKey, InputHandler.GetModifiers(info.Modifiers), ClickCount.Single) || !this._handle)
                        break;
                    info.MarkHandled();
                    break;
                case '\x001B':
                    if (!this._handleEscape || !this._handle)
                        break;
                    info.MarkHandled();
                    break;
                case ' ':
                    if (!this.ShouldHandleEvent(ClickType.SpaceKey, InputHandler.GetModifiers(info.Modifiers), ClickCount.Single) || !this._handle)
                        break;
                    info.MarkHandled();
                    break;
            }
        }

        protected override void OnLoseKeyFocus(UIClass ui, KeyFocusInfo info)
        {
            if (this.ShouldHandleEvent(ClickType.SpaceKey))
                this.CancelClick(ClickType.SpaceKey);
            if (this.ShouldHandleEvent(ClickType.EnterKey))
                this.CancelClick(ClickType.EnterKey);
            if (this.ShouldHandleEvent(ClickType.GamePadA))
                this.CancelClick(ClickType.GamePadA);
            if (this.ShouldHandleEvent(ClickType.GamePadStart))
                this.CancelClick(ClickType.GamePadStart);
            this._handleEscape = false;
        }

        private struct ClickInfo
        {
            public ClickType type;
            public InputHandlerTransition transition;
            public InputHandlerModifiers modifiers;
            public ClickCount count;
            public ICookedInputSite target;
        }
    }
}
