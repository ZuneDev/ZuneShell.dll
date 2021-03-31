// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layout.LayoutSlot
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;
using System.Text;

namespace Microsoft.Iris.Layout
{
    internal struct LayoutSlot
    {
        private Size _bounds;
        private Point _offset;
        private Rectangle _viewBounds;
        private Rectangle _peripheralViewBounds;

        public LayoutSlot(Size extent)
          : this(extent, Point.Zero, new Rectangle(extent), new Rectangle(extent))
        {
        }

        public LayoutSlot(
          Size extent,
          Point offset,
          Rectangle viewBounds,
          Rectangle viewPeripheralBounds)
        {
            this._bounds = extent;
            this._offset = offset;
            this._viewBounds = viewBounds;
            this._peripheralViewBounds = viewPeripheralBounds;
        }

        public Size Bounds
        {
            get => this._bounds;
            set => this._bounds = value;
        }

        public Point Offset
        {
            get => this._offset;
            set => this._offset = value;
        }

        public Rectangle View
        {
            get => this._viewBounds;
            set => this._viewBounds = value;
        }

        public Rectangle PeripheralView
        {
            get => this._peripheralViewBounds;
            set => this._peripheralViewBounds = value;
        }

        public void Deflate(Inset inset)
        {
            int num1 = inset.Left + inset.Right;
            int num2 = inset.Top + inset.Bottom;
            this._bounds.Width -= num1;
            if (this._bounds.Width < 0)
                this._bounds.Width = 0;
            this._bounds.Height -= num2;
            if (this._bounds.Height < 0)
                this._bounds.Height = 0;
            this._viewBounds.X -= inset.Left;
            this._viewBounds.Y -= inset.Top;
            this._peripheralViewBounds.X -= inset.Left;
            this._peripheralViewBounds.Y -= inset.Top;
        }

        public static bool operator ==(LayoutSlot left, LayoutSlot right) => left._bounds == right._bounds && left._offset == right._offset && left._viewBounds == right._viewBounds && left._peripheralViewBounds == right._peripheralViewBounds;

        public static bool operator !=(LayoutSlot left, LayoutSlot right) => !(left == right);

        public override bool Equals(object obj) => obj is LayoutSlot layoutSlot && this == layoutSlot;

        public override int GetHashCode() => this._bounds.GetHashCode() ^ this._offset.GetHashCode() ^ this._viewBounds.GetHashCode() ^ this._peripheralViewBounds.GetHashCode();

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(this.GetType().Name);
            stringBuilder.Append("(");
            stringBuilder.Append("Bounds=");
            stringBuilder.Append(_bounds);
            stringBuilder.Append(", Offset=");
            stringBuilder.Append(_offset);
            stringBuilder.Append(", View=");
            stringBuilder.Append(_viewBounds);
            if (this._peripheralViewBounds != this._viewBounds)
            {
                stringBuilder.Append(", Peripheral=");
                stringBuilder.Append(_peripheralViewBounds);
            }
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }
    }
}
