// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Input.MouseButtonInfo
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;

namespace Microsoft.Iris.Input
{
    internal class MouseButtonInfo : MouseActionInfo
    {
        private static InputInfo.InfoType s_poolType = InputInfo.InfoType.MouseButton;
        private bool _doubleClick;
        private bool _state;

        static MouseButtonInfo() => InputInfo.SetPoolLimitMode(MouseButtonInfo.s_poolType, false);

        private MouseButtonInfo()
        {
        }

        public static MouseButtonInfo Create(
          IRawInputSite rawSource,
          IRawInputSite rawNatural,
          int x,
          int y,
          int screenX,
          int screenY,
          InputModifiers modifiers,
          MouseButtons button,
          bool state,
          uint messageID)
        {
            MouseButtonInfo mouseButtonInfo = (MouseButtonInfo)InputInfo.GetFromPool(MouseButtonInfo.s_poolType) ?? new MouseButtonInfo();
            mouseButtonInfo.Initialize(rawSource, rawNatural, x, y, screenX, screenY, modifiers, button, state, messageID);
            return mouseButtonInfo;
        }

        private void Initialize(
          IRawInputSite rawSource,
          IRawInputSite rawNatural,
          int x,
          int y,
          int screenX,
          int screenY,
          InputModifiers modifiers,
          MouseButtons button,
          bool state,
          uint messageID)
        {
            this._state = state;
            this._doubleClick = false;
            this.Initialize(rawSource, rawNatural, x, y, screenX, screenY, modifiers, MouseButtonInfo.EventTypeForMouseButton(state, button, modifiers), messageID, button, 0);
        }

        public override InputEventType EventType => this._doubleClick ? InputEventType.MouseDoubleClick : this._eventType;

        public override void UpdateTarget(ICookedInputSite target)
        {
            base.UpdateTarget(target);
            this._doubleClick = (this.Modifiers & InputModifiers.DoubleClick) != InputModifiers.None && target != null && target.AllowDoubleClicks;
        }

        private static InputEventType EventTypeForMouseButton(
          bool state,
          MouseButtons button,
          InputModifiers modifiers)
        {
            bool flag = (button & MouseButtons.Left) != MouseButtons.None;
            return !state ? (!flag ? InputEventType.MouseSecondaryUp : InputEventType.MousePrimaryUp) : (!flag ? InputEventType.MouseSecondaryDown : InputEventType.MousePrimaryDown);
        }

        public bool IsDown => this._state;

        protected override InputInfo.InfoType PoolType => MouseButtonInfo.s_poolType;

        public override string ToString() => InvariantString.Format("{0}({1}, Button={2})", this.GetType().Name, this._state ? "Down" : "Up", Button);
    }
}
