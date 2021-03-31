// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.InputHandlers.KeyHandler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;
using System.Collections;

namespace Microsoft.Iris.InputHandlers
{
    internal class KeyHandler : ModifierInputHandler
    {
        private ArrayList _invokedKeys;
        private KeyHandlerKey _key;
        private IUICommand _command;
        private bool _pressing;
        private bool _handle;
        private bool _stopRoute;
        private bool _repeat;
        private WeakReference _eventContext;

        public KeyHandler()
        {
            this._handle = true;
            this.HandlerTransition = InputHandlerTransition.Down;
            this._key = KeyHandlerKey.None;
            this._repeat = true;
        }

        protected override void ConfigureInteractivity()
        {
            base.ConfigureInteractivity();
            if (!this.HandleDirect)
                return;
            this.UI.KeyInteractive = true;
        }

        public bool Pressing => this._pressing;

        public KeyHandlerKey Key
        {
            get => this._key;
            set
            {
                if (this._key == value)
                    return;
                this._key = value;
                this.FireNotification(NotificationID.Key);
            }
        }

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

        public bool StopRoute
        {
            get => this._stopRoute;
            set
            {
                this._stopRoute = value;
                this.FireNotification(NotificationID.StopRoute);
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

        public object EventContext => this.CheckEventContext(ref this._eventContext);

        public bool TrackInvokedKeys
        {
            get => this._invokedKeys != null;
            set
            {
                if (this.TrackInvokedKeys == value)
                    return;
                this._invokedKeys = !value ? (ArrayList)null : new ArrayList();
                this.FireNotification(NotificationID.TrackInvokedKeys);
            }
        }

        public void GetInvokedKeys(IList copyTo)
        {
            if (this.TrackInvokedKeys)
            {
                foreach (object invokedKey in this._invokedKeys)
                    copyTo.Add(invokedKey);
                this._invokedKeys.Clear();
            }
            else
                ErrorManager.ReportError("KeyHandler needs to be marked TrackInvokedKeys=\"true\" in order to call GetInvokedKeys()");
        }

        protected override void OnKeyDown(UIClass ui, KeyStateInfo info)
        {
            Keys key = info.Key;
            InputHandlerModifiers modifiers = InputHandler.GetModifiers(info.Modifiers);
            if (key != (Keys)this._key)
                KeyHandler.TranslateKey(ref key, ref modifiers);
            if (!this.KeyMatches(key) || !this.ShouldHandleEvent(modifiers))
                return;
            bool flag;
            if (!this._pressing)
            {
                this._pressing = true;
                this.FireNotification(NotificationID.Pressing);
                flag = true;
            }
            else
                flag = this._repeat;
            if (flag)
            {
                if (this.HandlerTransition == InputHandlerTransition.Down)
                    this.InvokeCommand(key);
                if (this._handle)
                    info.MarkHandled();
            }
            if (this._stopRoute)
                info.TruncateRoute();
            this.SetEventContext(info.Target, ref this._eventContext, NotificationID.EventContext);
        }

        protected override void OnKeyUp(UIClass ui, KeyStateInfo info)
        {
            Keys key = info.Key;
            InputHandlerModifiers modifiers = InputHandler.GetModifiers(info.Modifiers);
            if (key != (Keys)this._key)
                KeyHandler.TranslateKey(ref key, ref modifiers);
            if (!this.KeyMatches(key) || !this.ShouldHandleEvent(modifiers))
                return;
            this._pressing = false;
            this.FireNotification(NotificationID.Pressing);
            if (this.HandlerTransition == InputHandlerTransition.Up)
                this.InvokeCommand(key);
            if (this._handle)
                info.MarkHandled();
            if (this._stopRoute)
                info.TruncateRoute();
            this.SetEventContext(info.Target, ref this._eventContext, NotificationID.EventContext);
        }

        protected override void OnLoseKeyFocus(UIClass ui, KeyFocusInfo info)
        {
            if (!this._pressing)
                return;
            this._pressing = false;
            this.FireNotification(NotificationID.Pressing);
        }

        private bool KeyMatches(Keys candidate)
        {
            if (this._key >= KeyHandlerKey.None)
                return candidate == (Keys)this._key;
            return this._key == KeyHandlerKey.Any && candidate != Keys.None;
        }

        public static void TranslateKey(ref Keys key, ref InputHandlerModifiers modifiers) => KeyHandler.TranslateKey(ref key, ref modifiers, Orientation.Horizontal);

        public static void TranslateKey(
          ref Keys key,
          ref InputHandlerModifiers modifiers,
          Orientation orientation)
        {
            InputHandlerModifiers handlerModifiers = modifiers;
            modifiers = InputHandlerModifiers.None;
            switch (key)
            {
                case Keys.GamePadA:
                case Keys.GamePadStart:
                    key = Keys.Enter;
                    break;
                case Keys.GamePadB:
                case Keys.GamePadBack:
                    key = Keys.Back;
                    break;
                case Keys.GamePadRShoulder:
                    key = Keys.Tab;
                    break;
                case Keys.GamePadLShoulder:
                    key = Keys.Tab;
                    modifiers = InputHandlerModifiers.Shift;
                    break;
                case Keys.GamePadLTrigger:
                    key = Keys.PageUp;
                    break;
                case Keys.GamePadRTrigger:
                    key = Keys.Next;
                    break;
                case Keys.GamePadDPadUp:
                case Keys.GamePadLThumbUp:
                    key = Keys.Up;
                    break;
                case Keys.GamePadDPadDown:
                case Keys.GamePadLThumbDown:
                    key = Keys.Down;
                    break;
                case Keys.GamePadDPadLeft:
                case Keys.GamePadLThumbLeft:
                    key = Keys.Left;
                    break;
                case Keys.GamePadDPadRight:
                case Keys.GamePadLThumbRight:
                    key = Keys.Right;
                    break;
                case Keys.GamePadLThumbUpLeft:
                    key = orientation == Orientation.Vertical ? Keys.Up : Keys.Left;
                    break;
                case Keys.GamePadLThumbUpRight:
                    key = orientation == Orientation.Vertical ? Keys.Up : Keys.Right;
                    break;
                case Keys.GamePadLThumbDownRight:
                    key = orientation == Orientation.Vertical ? Keys.Down : Keys.Right;
                    break;
                case Keys.GamePadLThumbDownLeft:
                    key = orientation == Orientation.Vertical ? Keys.Down : Keys.Left;
                    break;
                default:
                    modifiers = handlerModifiers;
                    break;
            }
        }

        private void InvokeCommand(Keys key)
        {
            this.FireNotification(NotificationID.Invoked);
            if (this._command != null)
                this._command.Invoke();
            if (!this.TrackInvokedKeys)
                return;
            this._invokedKeys.Add((object)(KeyHandlerKey)key);
        }

        public override string ToString() => InvariantString.Format("{0}({1})", (object)this.GetType().Name, (object)this._key);
    }
}
