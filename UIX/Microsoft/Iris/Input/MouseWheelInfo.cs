// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Input.MouseWheelInfo
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;

namespace Microsoft.Iris.Input
{
    internal class MouseWheelInfo : MouseActionInfo
    {
        public const int k_defaultDelta = 120;
        private static InputInfo.InfoType s_poolType = InputInfo.InfoType.MouseWheel;

        static MouseWheelInfo() => InputInfo.SetPoolLimitMode(MouseWheelInfo.s_poolType, true);

        private MouseWheelInfo()
        {
        }

        public static MouseWheelInfo Create(
          IRawInputSite rawSource,
          IRawInputSite rawNatural,
          int x,
          int y,
          int screenX,
          int screenY,
          InputModifiers modifiers,
          int delta)
        {
            MouseWheelInfo mouseWheelInfo = (MouseWheelInfo)InputInfo.GetFromPool(MouseWheelInfo.s_poolType) ?? new MouseWheelInfo();
            mouseWheelInfo.Initialize(rawSource, rawNatural, x, y, screenX, screenY, modifiers, InputEventType.MouseWheel, 522U, MouseButtons.None, delta);
            return mouseWheelInfo;
        }

        public override string ToString() => InvariantString.Format("{0}(Delta={1})", (object)this.GetType().Name, (object)this.WheelDelta);

        protected override InputInfo.InfoType PoolType => MouseWheelInfo.s_poolType;
    }
}
