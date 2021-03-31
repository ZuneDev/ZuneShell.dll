// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Input.MouseActionInfo
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Input
{
    internal abstract class MouseActionInfo : MouseInfo
    {
        private IRawInputSite _naturalHit;
        private ICookedInputSite _naturalTarget;
        private InputModifiers _modifiers;
        private int _screenX;
        private int _screenY;
        private int _wheelDelta;
        private uint _nativeMessageID;
        private MouseButtons _button;

        protected void Initialize(
          IRawInputSite rawSource,
          IRawInputSite rawNatural,
          int x,
          int y,
          int screenX,
          int screenY,
          InputModifiers modifiers,
          InputEventType eventType,
          uint messageID,
          MouseButtons button,
          int wheelDelta)
        {
            this._naturalHit = rawNatural;
            this._modifiers = modifiers;
            this._screenX = screenX;
            this._screenY = screenY;
            this._wheelDelta = wheelDelta;
            this._nativeMessageID = messageID;
            this._button = button;
            this.Initialize(rawSource, x, y, eventType);
        }

        protected override void Zombie()
        {
            base.Zombie();
            this._naturalHit = (IRawInputSite)null;
            this._naturalTarget = (ICookedInputSite)null;
        }

        public uint NativeMessageID => this._nativeMessageID;

        public IRawInputSite NaturalHit => this._naturalHit;

        public ICookedInputSite NaturalTarget => this._naturalTarget;

        public InputModifiers Modifiers => this._modifiers;

        public MouseButtons Button => this._button;

        public int ScreenX => this._screenX;

        public int ScreenY => this._screenY;

        public int WheelDelta => this._wheelDelta;

        public void SetMappedTargets(ICookedInputSite target, ICookedInputSite naturalTarget)
        {
            this.UpdateTarget(target);
            this._naturalTarget = naturalTarget;
        }
    }
}
