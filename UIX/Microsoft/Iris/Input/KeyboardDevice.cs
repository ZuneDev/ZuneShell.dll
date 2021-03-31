// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Input.KeyboardDevice
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;

namespace Microsoft.Iris.Input
{
    internal class KeyboardDevice : InputDevice
    {
        internal const int KeyDown = 0;
        internal const int KeyUp = 1;
        internal const int KeyChar = 2;
        internal const int SysKeyDown = 3;
        internal const int SysKeyUp = 4;
        internal const int SysChar = 5;
        private const int k_initialArrayLength = 4;
        private ExpandableArray _keyStates;
        private InputModifiers _keyModifiers;
        private bool _modifiersDirty;

        public KeyboardDevice(InputManager manager)
          : base(manager)
        {
            this._keyStates = new ExpandableArray(4);
            this.Reset();
        }

        public InputModifiers KeyboardModifiers
        {
            get
            {
                if (this._modifiersDirty)
                    this.UpdateModifierState();
                return this._keyModifiers & InputModifiers.AllKeys;
            }
        }

        public bool Alt => (this.KeyboardModifiers & InputModifiers.AltKey) != InputModifiers.None;

        public bool Ctrl => (this.KeyboardModifiers & InputModifiers.ControlKey) != InputModifiers.None;

        public bool Shift => (this.KeyboardModifiers & InputModifiers.ShiftKey) != InputModifiers.None;

        public bool Win => (this.KeyboardModifiers & InputModifiers.WindowsKey) != InputModifiers.None;

        internal KeyInfo OnRawInput(
          uint message,
          InputModifiers modifiers,
          ref RawKeyboardData args)
        {
            this.Manager.HACK_UpdateSystemModifiers(modifiers);
            return message == 2U || message == 5U ? this.OnRawKeyCharacter(message, ref args) : this.OnRawKeyState(message, modifiers, ref args);
        }

        internal KeyInfo OnRawKeyCharacter(uint message, ref RawKeyboardData args) => (KeyInfo)KeyCharacterInfo.Create(KeyAction.Character, args._deviceType, this.Manager.Modifiers, args._repCount, (char)args._virtualKey, message == 5U, message, args._scanCode, args._flags);

        internal KeyInfo OnRawKeyState(
          uint message,
          InputModifiers rawModifiers,
          ref RawKeyboardData args)
        {
            KeyStateInfo keyStateInfo = (KeyStateInfo)null;
            bool systemKey = false;
            KeyAction action;
            switch (message)
            {
                case 0:
                    action = KeyAction.Down;
                    break;
                case 1:
                    action = KeyAction.Up;
                    break;
                case 3:
                    systemKey = true;
                    goto case 0;
                case 4:
                    systemKey = true;
                    goto case 1;
                default:
                    return (KeyInfo)null;
            }
            if (this.TrackKey(action, args._virtualKey, args._scanCode))
            {
                InputModifiers modifiers = this.Manager.Modifiers & ~this.MapKeyToModifier(args._virtualKey);
                keyStateInfo = KeyStateInfo.Create(action, args._deviceType, modifiers, args._repCount, args._virtualKey, systemKey, message, args._scanCode, args._flags);
            }
            return (KeyInfo)keyStateInfo;
        }

        public bool IsKeyDown(Keys key)
        {
            foreach (KeyboardDevice.KeyState keyState in this._keyStates)
            {
                if (keyState.VKey == key && keyState.IsDown)
                    return true;
            }
            return false;
        }

        public bool IsKeyHandled(Keys key)
        {
            foreach (KeyboardDevice.KeyState keyState in this._keyStates)
            {
                if (keyState.VKey == key)
                    return keyState.IsHandled;
            }
            return false;
        }

        internal void MarkKeyHandled(Keys key)
        {
            foreach (KeyboardDevice.KeyState keyState in this._keyStates)
            {
                if (keyState.VKey == key && keyState.IsDown)
                {
                    keyState.MarkHandled();
                    break;
                }
            }
        }

        public void Reset()
        {
            this._keyStates.Clear();
            this._modifiersDirty = true;
            this._keyModifiers = InputModifiers.None;
        }

        private bool TrackKey(KeyAction action, Keys vkey, int scanCode)
        {
            bool flag = false;
            if (vkey == Keys.None)
                return false;
            if (this.IsModifier(vkey))
                this.MarkModifiersInvalid();
            switch (action)
            {
                case KeyAction.Up:
                    flag = this.TrackKeyUp(vkey, scanCode);
                    break;
                case KeyAction.Down:
                    flag = this.TrackKeyDown(vkey, scanCode);
                    break;
            }
            return flag;
        }

        private bool TrackKeyDown(Keys vkey, int scanCode)
        {
            KeyboardDevice.KeyState keyState1 = (KeyboardDevice.KeyState)null;
            for (int index = 0; index < this._keyStates.Length; ++index)
            {
                KeyboardDevice.KeyState keyState2 = (KeyboardDevice.KeyState)this._keyStates[index];
                if (keyState2 != null)
                {
                    if (keyState2.IsDown)
                    {
                        if (keyState2.VKey == vkey && keyState2.ScanCode == scanCode)
                        {
                            keyState1 = keyState2;
                            break;
                        }
                    }
                    else
                    {
                        this._keyStates[index] = (object)null;
                        keyState2.Dispose();
                    }
                }
            }
            if (keyState1 == null)
            {
                KeyboardDevice.KeyState keyState2 = new KeyboardDevice.KeyState(vkey, scanCode);
                this._keyStates.Add((object)keyState2);
                keyState2.IsDown = true;
            }
            return true;
        }

        private bool TrackKeyUp(Keys vkey, int scanCode)
        {
            KeyboardDevice.KeyState keyState1 = (KeyboardDevice.KeyState)null;
            bool flag = false;
            foreach (KeyboardDevice.KeyState keyState2 in this._keyStates)
            {
                if (keyState2.VKey == vkey && keyState2.ScanCode == scanCode)
                {
                    keyState1 = keyState2;
                    break;
                }
            }
            if (keyState1 != null)
            {
                flag = keyState1.IsDown;
                keyState1.IsDown = false;
            }
            int num = flag ? 1 : 0;
            return flag;
        }

        private bool IsModifier(Keys vkey) => this.MapKeyToModifier(vkey) != InputModifiers.None;

        private void MarkModifiersInvalid() => this._modifiersDirty = true;

        private InputModifiers MapKeyToModifier(Keys vkey)
        {
            switch (vkey)
            {
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                    return InputModifiers.ShiftKey;
                case Keys.ControlKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                    return InputModifiers.ControlKey;
                case Keys.Menu:
                case Keys.LMenu:
                case Keys.RMenu:
                    return InputModifiers.AltKey;
                case Keys.LWin:
                case Keys.RWin:
                    return InputModifiers.WindowsKey;
                default:
                    return InputModifiers.None;
            }
        }

        private void UpdateModifierState()
        {
            if (!this._modifiersDirty)
                return;
            this._keyModifiers = InputModifiers.None;
            foreach (KeyboardDevice.KeyState keyState in this._keyStates)
            {
                if (keyState.IsDown)
                    this._keyModifiers |= this.MapKeyToModifier(keyState.VKey);
            }
            this._modifiersDirty = false;
        }

        private class KeyState
        {
            private Keys _vkey;
            private int _scanCode;
            private bool _down;
            private bool _handled;

            public KeyState(Keys vkey, int scanCode)
            {
                this._vkey = vkey;
                this._scanCode = scanCode;
                this._down = false;
                this._handled = false;
            }

            public void Dispose() => this.Dispose(true);

            protected void Dispose(bool inDispose)
            {
                if (!inDispose)
                    return;
                this._vkey = Keys.None;
                this._scanCode = -1;
            }

            public Keys VKey => this._vkey;

            public int ScanCode => this._scanCode;

            public bool IsDown
            {
                get => this._down;
                set => this._down = value;
            }

            public bool IsHandled => this._handled;

            public void MarkHandled() => this._handled = true;
        }
    }
}
