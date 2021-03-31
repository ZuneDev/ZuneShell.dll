// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.InputHandlers.ScrollingHandler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Markup;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Render;
using Microsoft.Iris.UI;
using Microsoft.Iris.ViewItems;
using System;

namespace Microsoft.Iris.InputHandlers
{
    internal class ScrollingHandler : InputHandler
    {
        private ScrollModel _model;
        private bool _handleDirectionalKeysFlag;
        private bool _handlePageKeysFlag;
        private bool _handleHomeEndKeysFlag;
        private bool _handlePageCommandsFlag;
        private bool _handleMouseWheelFlag;
        private bool _useFocusBehavior;
        private Keys _currentCampingKey;
        private int _cumulativeMouseWheelDelta;

        public ScrollingHandler()
        {
            this._handleDirectionalKeysFlag = true;
            this._handlePageKeysFlag = true;
            this._handlePageCommandsFlag = true;
            this._handleHomeEndKeysFlag = true;
            this._handleMouseWheelFlag = true;
            this._useFocusBehavior = true;
            this.HandlerStage = InputHandlerStage.Direct | InputHandlerStage.Bubbled;
        }

        protected override void ConfigureInteractivity()
        {
            base.ConfigureInteractivity();
            if (!this.HandleDirect)
                return;
            if (this._handleDirectionalKeysFlag || this._handlePageKeysFlag || (this._handleHomeEndKeysFlag || this._handlePageCommandsFlag))
                this.UI.KeyInteractive = true;
            if (!this._handleMouseWheelFlag)
                return;
            this.UI.MouseInteractive = true;
        }

        public bool HandleDirectionalKeys
        {
            get => this._handleDirectionalKeysFlag;
            set
            {
                if (this._handleDirectionalKeysFlag == value)
                    return;
                this._handleDirectionalKeysFlag = value;
                this.FireNotification(NotificationID.HandleDirectionalKeys);
            }
        }

        public bool HandlePageKeys
        {
            get => this._handlePageKeysFlag;
            set
            {
                if (this._handlePageKeysFlag == value)
                    return;
                this._handlePageKeysFlag = value;
                this.FireNotification(NotificationID.HandlePageKeys);
            }
        }

        public bool HandleHomeEndKeys
        {
            get => this._handleHomeEndKeysFlag;
            set
            {
                if (this._handleHomeEndKeysFlag == value)
                    return;
                this._handleHomeEndKeysFlag = value;
                this.FireNotification(NotificationID.HandleHomeEndKeys);
            }
        }

        public bool HandlePageCommands
        {
            get => this._handlePageCommandsFlag;
            set
            {
                if (this._handlePageCommandsFlag == value)
                    return;
                this._handlePageCommandsFlag = value;
                this.FireNotification(NotificationID.HandlePageCommands);
            }
        }

        public bool HandleMouseWheel
        {
            get => this._handleMouseWheelFlag;
            set
            {
                if (this._handleMouseWheelFlag == value)
                    return;
                this._handleMouseWheelFlag = value;
                this.FireNotification(NotificationID.HandleMouseWheel);
            }
        }

        public ScrollModel ScrollModel
        {
            get => this._model;
            set
            {
                if (this._model == value)
                    return;
                this._model = value;
                this.FireNotification(NotificationID.ScrollModel);
            }
        }

        public bool UseFocusBehavior
        {
            get => this._useFocusBehavior;
            set
            {
                if (this._useFocusBehavior == value)
                    return;
                this._useFocusBehavior = value;
                this.FireNotification(NotificationID.UseFocusBehavior);
            }
        }

        private bool ValidScrollModel => this._model != null && this._model.Enabled;

        protected override void OnGainKeyFocus(UIClass sender, KeyFocusInfo info)
        {
            if (!this.ValidScrollModel || !this._useFocusBehavior)
                return;
            this._model.NotifyFocusChange(info.Target as UIClass);
        }

        protected override void OnKeyDown(UIClass ui, KeyStateInfo info)
        {
            if (!this.ValidScrollModel)
                return;
            if (info.Key != this._currentCampingKey)
                this.EndCamp();
            Keys key = info.Key;
            InputHandlerModifiers modifiers = GetModifiers(info.Modifiers);
            KeyHandler.TranslateKey(ref key, ref modifiers, this.Orientation);
            switch (this.ShouldHandleKey(key))
            {
                case HandleKeyPolicy.None:
                    return;
                case HandleKeyPolicy.Up:
                    this._model.ScrollUp(this._useFocusBehavior);
                    break;
                case HandleKeyPolicy.Down:
                    this._model.ScrollDown(this._useFocusBehavior);
                    break;
                case HandleKeyPolicy.PageUp:
                    this._model.PageUp(this._useFocusBehavior);
                    break;
                case HandleKeyPolicy.PageDown:
                    this._model.PageDown(this._useFocusBehavior);
                    break;
                case HandleKeyPolicy.Home:
                    this._model.Home(this._useFocusBehavior);
                    break;
                case HandleKeyPolicy.End:
                    this._model.End(this._useFocusBehavior);
                    break;
            }
            this.BeginCamp(info.Key);
            info.MarkHandled();
        }

        private void BeginCamp(Keys key)
        {
            if (!this.IsCamping)
                this._model.BeginCamp();
            this._currentCampingKey = key;
        }

        private void EndCamp()
        {
            if (!this.IsCamping)
                return;
            this._model.EndCamp();
            this._currentCampingKey = Keys.None;
        }

        private bool IsCamping => this._currentCampingKey != Keys.None;

        protected override void OnKeyUp(UIClass ui, KeyStateInfo info)
        {
            if (!this.ValidScrollModel)
                return;
            if (info.Key == this._currentCampingKey)
                this.EndCamp();
            Keys key = info.Key;
            InputHandlerModifiers modifiers = GetModifiers(info.Modifiers);
            KeyHandler.TranslateKey(ref key, ref modifiers, this.Orientation);
            if (this.ShouldHandleKey(key) == HandleKeyPolicy.None)
                return;
            info.MarkHandled();
        }

        protected override void OnLoseDeepKeyFocus() => this.EndCamp();

        protected override void OnCommandDown(UIClass ui, KeyCommandInfo info)
        {
            if (!this.ValidScrollModel || info.Action != KeyAction.Down)
                return;
            switch (info.Command)
            {
                case CommandCode.ChannelUp:
                    if (!this._handlePageCommandsFlag)
                        break;
                    this._model.PageUp();
                    info.MarkHandled();
                    break;
                case CommandCode.ChannelDown:
                    if (!this._handlePageCommandsFlag)
                        break;
                    this._model.PageDown();
                    info.MarkHandled();
                    break;
            }
        }

        protected override void OnCommandUp(UIClass ui, KeyCommandInfo info)
        {
            switch (info.Command)
            {
                case CommandCode.ChannelUp:
                case CommandCode.ChannelDown:
                    if (!this._handlePageCommandsFlag)
                        break;
                    info.MarkHandled();
                    break;
            }
        }

        protected override void OnMouseWheel(UIClass ui, MouseWheelInfo info)
        {
            if (!this._handleMouseWheelFlag || !this.ValidScrollModel || this.InputHasKeyModifiers(info))
                return;
            this._cumulativeMouseWheelDelta += -info.WheelDelta;
            if (Math.Abs(this._cumulativeMouseWheelDelta) >= 120)
            {
                if (this._cumulativeMouseWheelDelta > 0)
                    this._model.ScrollDown(this._cumulativeMouseWheelDelta / 120);
                else
                    this._model.ScrollUp(Math.Abs(this._cumulativeMouseWheelDelta / 120));
                this._cumulativeMouseWheelDelta %= 120;
            }
            info.MarkHandled();
        }

        private ScrollingHandler.HandleKeyPolicy ShouldHandleKey(Keys key)
        {
            ScrollingHandler.HandleKeyPolicy handleKeyPolicy = HandleKeyPolicy.None;
            switch (key)
            {
                case Keys.PageUp:
                    if (this._handlePageKeysFlag)
                    {
                        handleKeyPolicy = HandleKeyPolicy.PageUp;
                        break;
                    }
                    break;
                case Keys.Next:
                    if (this._handlePageKeysFlag)
                    {
                        handleKeyPolicy = HandleKeyPolicy.PageDown;
                        break;
                    }
                    break;
                case Keys.End:
                    if (this._handleHomeEndKeysFlag)
                    {
                        handleKeyPolicy = HandleKeyPolicy.End;
                        break;
                    }
                    break;
                case Keys.Home:
                    if (this._handleHomeEndKeysFlag)
                    {
                        handleKeyPolicy = HandleKeyPolicy.Home;
                        break;
                    }
                    break;
                case Keys.Left:
                    if (this._handleDirectionalKeysFlag && this.Orientation == Orientation.Horizontal)
                    {
                        handleKeyPolicy = this.UI.Zone.Session.IsRtl ? HandleKeyPolicy.Down : HandleKeyPolicy.Up;
                        break;
                    }
                    break;
                case Keys.Up:
                    if (this._handleDirectionalKeysFlag && this.Orientation == Orientation.Vertical)
                    {
                        handleKeyPolicy = HandleKeyPolicy.Up;
                        break;
                    }
                    break;
                case Keys.Right:
                    if (this._handleDirectionalKeysFlag && this.Orientation == Orientation.Horizontal)
                    {
                        handleKeyPolicy = this.UI.Zone.Session.IsRtl ? HandleKeyPolicy.Up : HandleKeyPolicy.Down;
                        break;
                    }
                    break;
                case Keys.Down:
                    if (this._handleDirectionalKeysFlag && this.Orientation == Orientation.Vertical)
                    {
                        handleKeyPolicy = HandleKeyPolicy.Down;
                        break;
                    }
                    break;
            }
            return handleKeyPolicy;
        }

        private Orientation Orientation
        {
            get
            {
                Orientation orientation = Orientation.Horizontal;
                if (this._model.TargetViewItem is Scroller targetViewItem)
                    orientation = targetViewItem.Orientation;
                return orientation;
            }
        }

        private bool InputHasKeyModifiers(InputInfo info)
        {
            InputModifiers inputModifiers = InputModifiers.None;
            switch (info)
            {
                case KeyStateInfo _:
                    inputModifiers = ((KeyActionInfo)info).Modifiers;
                    break;
                case MouseActionInfo _:
                    inputModifiers = ((MouseActionInfo)info).Modifiers;
                    break;
            }
            return (inputModifiers & InputModifiers.AllKeys) != InputModifiers.None;
        }

        private enum HandleKeyPolicy
        {
            None,
            Up,
            Down,
            PageUp,
            PageDown,
            Home,
            End,
        }
    }
}
