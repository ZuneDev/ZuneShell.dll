// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Input.KeyCharacterInfo
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;

namespace Microsoft.Iris.Input
{
    internal class KeyCharacterInfo : KeyActionInfo
    {
        private static InputInfo.InfoType s_poolType = InputInfo.InfoType.KeyCharacter;
        private char _character;

        static KeyCharacterInfo() => InputInfo.SetPoolLimitMode(KeyCharacterInfo.s_poolType, false);

        private KeyCharacterInfo()
        {
        }

        public static KeyCharacterInfo Create(
          KeyAction action,
          InputDeviceType deviceType,
          InputModifiers modifiers,
          uint repeatCount,
          char character,
          bool systemKey,
          uint nativeMessageID,
          int scanCode,
          ushort eventFlags)
        {
            KeyCharacterInfo keyCharacterInfo = (KeyCharacterInfo)InputInfo.GetFromPool(KeyCharacterInfo.s_poolType) ?? new KeyCharacterInfo();
            keyCharacterInfo.Initialize(action, deviceType, modifiers, repeatCount, character, systemKey, nativeMessageID, scanCode, eventFlags);
            return keyCharacterInfo;
        }

        private void Initialize(
          KeyAction action,
          InputDeviceType deviceType,
          InputModifiers modifiers,
          uint repeatCount,
          char character,
          bool systemKey,
          uint nativeMessageID,
          int scanCode,
          ushort eventFlags)
        {
            this._character = character;
            this.Initialize(action, InputEventType.KeyCharacter, deviceType, modifiers, repeatCount, systemKey, nativeMessageID, scanCode, eventFlags);
        }

        public char Character => this._character;

        protected override InputInfo.InfoType PoolType => KeyCharacterInfo.s_poolType;

        public override string ToString() => InvariantString.Format("{0}({1}, Key={2})", (object)this.GetType().Name, (object)this.Action, (object)this._character);
    }
}
