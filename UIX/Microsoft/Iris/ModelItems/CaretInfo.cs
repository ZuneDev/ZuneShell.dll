// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ModelItems.CaretInfo
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using Microsoft.Iris.Render;

namespace Microsoft.Iris.ModelItems
{
    internal class CaretInfo : NotifyObjectBase
    {
        private Point _position;
        private Size _suggestedSize;
        private int _idealWidth;
        private bool _visible;
        private bool _ignoreIdealWidth;

        public CaretInfo() => this._idealWidth = this.GetSystemCaretWidth();

        public int IdealWidth
        {
            get => this._idealWidth;
            set
            {
                if (this._idealWidth == value)
                    return;
                this._idealWidth = value;
                this.FireThreadSafeNotification(NotificationID.IdealWidth);
                this.UpdateSuggestedSize(this.SuggestedSize);
            }
        }

        public Point Position => this._position;

        public Size SuggestedSize => this._suggestedSize;

        public bool Visible => this._visible && this._position.X >= 0 && this._position.Y >= 0;

        internal bool IgnoreIdealWidth
        {
            get => this._ignoreIdealWidth;
            set => this._ignoreIdealWidth = value;
        }

        public void CreateCaret(Size size) => this.UpdateSuggestedSize(size);

        public void SetVisible(bool visible)
        {
            if (this._visible == visible)
                return;
            this._visible = visible;
            this.FireThreadSafeNotification(NotificationID.Visible);
        }

        public void SetCaretPosition(Point position)
        {
            if (this._position.X == position.X && this._position.Y == position.Y)
                return;
            bool visible = this.Visible;
            this._position = position;
            this.FireThreadSafeNotification(NotificationID.Position);
            if (this.Visible == visible)
                return;
            this.FireThreadSafeNotification(NotificationID.Visible);
        }

        private void UpdateSuggestedSize(Size newSize)
        {
            if (newSize.Width == 1 && this._idealWidth != 0 && !this.IgnoreIdealWidth)
                newSize.Width = this._idealWidth;
            if (!(newSize != this._suggestedSize))
                return;
            this._suggestedSize = newSize;
            this.FireThreadSafeNotification(NotificationID.SuggestedSize);
        }

        private int GetSystemCaretWidth() => Win32Api.GetCaretWidth();

        public float BlinkTime
        {
            get
            {
                int num = Win32Api.GetCaretBlinkTime();
                if (num < 0)
                    num = 0;
                return (float)num / 1000f;
            }
        }
    }
}
