// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layouts.PopupPosition
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Layouts
{
    internal struct PopupPosition
    {
        private InterestPoint _popup;
        private InterestPoint _target;
        private FlipDirection _flipDirection;

        public PopupPosition(InterestPoint target, InterestPoint popup, FlipDirection flipDirection)
        {
            this._target = target;
            this._popup = popup;
            this._flipDirection = flipDirection;
        }

        public InterestPoint Target
        {
            get => this._target;
            set => this._target = value;
        }

        public InterestPoint Popup
        {
            get => this._popup;
            set => this._popup = value;
        }

        public FlipDirection Flipped
        {
            get => this._flipDirection;
            set => this._flipDirection = value;
        }
    }
}
