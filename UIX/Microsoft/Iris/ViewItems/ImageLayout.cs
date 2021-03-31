// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.ImageLayout
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.Render;
using System;

namespace Microsoft.Iris.ViewItems
{
    internal class ImageLayout : ILayout
    {
        private Size _sourceExtent;
        private Size _minimumSize;
        private bool _maintainAspectRatio;
        private bool _fill;

        public Size SourceSize
        {
            get => this._sourceExtent;
            set => this._sourceExtent = value;
        }

        public Size MinimumSize
        {
            get => this._minimumSize;
            set => this._minimumSize = value;
        }

        public bool MaintainAspectRatio
        {
            get => this._maintainAspectRatio;
            set => this._maintainAspectRatio = value;
        }

        public bool Fill
        {
            get => this._fill;
            set => this._fill = value;
        }

        public ItemAlignment DefaultChildAlignment => ItemAlignment.Default;

        Size ILayout.Measure(ILayoutNode layoutNode, Size constraint)
        {
            Size size = Size.Zero;
            if (this._fill)
                size = constraint;
            else if (this._sourceExtent != Size.Zero)
                size = Size.Min(Size.Max(this._minimumSize, this._sourceExtent), constraint);
            if (this._maintainAspectRatio && this._sourceExtent != Size.Zero && size != this._sourceExtent)
            {
                Size sz1 = SmallestFillingFit(this._sourceExtent, size);
                if (sz1.Height > constraint.Height || sz1.Width > constraint.Width)
                    sz1 = Size.LargestFit(this._sourceExtent, constraint);
                size = Size.Min(sz1, constraint);
            }
            DefaultLayout.Measure(layoutNode, size);
            return size;
        }

        public static Size SmallestFillingFit(Size source, Size bounds)
        {
            float num = source.Width / (float)source.Height;
            Size size = new Size(bounds.Width, (int)Math.Ceiling(bounds.Width / (double)num));
            if (size.Height < bounds.Height)
                size = new Size((int)Math.Ceiling(bounds.Height * (double)num), bounds.Height);
            return size;
        }

        void ILayout.Arrange(ILayoutNode layoutNode, LayoutSlot slot) => DefaultLayout.Arrange(layoutNode, slot);
    }
}
