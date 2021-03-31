// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Input.MouseFocusInfo
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Input
{
    internal class MouseFocusInfo : MouseInfo
    {
        private static InputInfo.InfoType s_poolType = InputInfo.InfoType.MouseFocus;
        private bool _state;
        private ICookedInputSite _other;

        static MouseFocusInfo() => InputInfo.SetPoolLimitMode(MouseFocusInfo.s_poolType, true);

        private MouseFocusInfo()
        {
        }

        public static MouseFocusInfo Create(
          IRawInputSite rawSource,
          int x,
          int y,
          bool state,
          ICookedInputSite other)
        {
            MouseFocusInfo mouseFocusInfo = (MouseFocusInfo)InputInfo.GetFromPool(MouseFocusInfo.s_poolType) ?? new MouseFocusInfo();
            mouseFocusInfo.Initialize(rawSource, x, y, state, other);
            return mouseFocusInfo;
        }

        private void Initialize(
          IRawInputSite rawSource,
          int x,
          int y,
          bool state,
          ICookedInputSite other)
        {
            this._state = state;
            this._other = other;
            this.Initialize(rawSource, x, y, state ? InputEventType.GainMouseFocus : InputEventType.LoseMouseFocus);
        }

        protected override void Zombie()
        {
            base.Zombie();
            this._other = null;
        }

        public ICookedInputSite Other => this._other;

        public bool State => this._state;

        protected override InputInfo.InfoType PoolType => MouseFocusInfo.s_poolType;
    }
}
