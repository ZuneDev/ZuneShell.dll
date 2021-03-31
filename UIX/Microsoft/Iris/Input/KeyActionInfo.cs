// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Input.KeyActionInfo
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System;

namespace Microsoft.Iris.Input
{
    [Serializable]
    internal abstract class KeyActionInfo : KeyInfo
    {
        private KeyAction _action;
        private InputDeviceType _deviceType;
        private InputModifiers _modifiers;
        private uint _repeatCount;
        private bool _systemKey;
        private uint _nativeMessageID;
        private int _scanCode;
        private ushort _eventFlags;

        protected void Initialize(
          KeyAction action,
          InputEventType eventType,
          InputDeviceType deviceType,
          InputModifiers modifiers,
          uint repeatCount,
          bool systemKey,
          uint nativeMessageID,
          int scanCode,
          ushort eventFlags)
        {
            this._systemKey = systemKey;
            this._action = action;
            this._deviceType = deviceType;
            this._modifiers = modifiers;
            this._repeatCount = repeatCount;
            this._nativeMessageID = nativeMessageID;
            this._scanCode = scanCode;
            this._eventFlags = eventFlags;
            this.Initialize(eventType);
        }

        protected void Initialize(
          KeyAction action,
          InputEventType eventType,
          InputDeviceType deviceType)
        {
            this.Initialize(action, eventType, deviceType, InputModifiers.None, 1U, false, 0U, 0, 0);
        }

        public KeyAction Action => this._action;

        public InputDeviceType DeviceType => this._deviceType;

        public InputModifiers Modifiers => this._modifiers;

        public uint RepeatCount => this._repeatCount;

        public bool SystemKey => this._systemKey;

        public uint NativeMessageID => this._nativeMessageID;

        public int ScanCode => this._scanCode;

        public ushort KeyboardFlags => this._eventFlags;

        public bool IsRepeatOf(KeyActionInfo other) => other != null && this._action == other._action && (this._deviceType == other._deviceType && this._systemKey == other._systemKey) && this._modifiers == other._modifiers;
    }
}
