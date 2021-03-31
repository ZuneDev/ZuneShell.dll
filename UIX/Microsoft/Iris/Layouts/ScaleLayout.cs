// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layouts.ScaleLayout
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using System;

namespace Microsoft.Iris.Layouts
{
    internal class ScaleLayout : ILayout
    {
        private Vector2 _minimumScale;
        private Vector2 _maximumScale;
        private bool _maintainAspectRatio;

        public ScaleLayout()
        {
            this._minimumScale = new Vector2(0.0f, 0.0f);
            this._maximumScale = new Vector2(1f, 1f);
            this._maintainAspectRatio = true;
        }

        public Vector2 MinimumScale
        {
            get => this._minimumScale;
            set => this._minimumScale = value;
        }

        public Vector2 MaximumScale
        {
            get => this._maximumScale;
            set => this._maximumScale = value;
        }

        public bool MaintainAspectRatio
        {
            get => this._maintainAspectRatio;
            set => this._maintainAspectRatio = value;
        }

        public ItemAlignment DefaultChildAlignment => ItemAlignment.Default;

        Size ILayout.Measure(ILayoutNode layoutNode, Size constraint)
        {
            Size zero = Size.Zero;
            Size constraint1 = new Size(this.UnscaleConstraint(constraint.Width, this.MinimumScale.X), this.UnscaleConstraint(constraint.Height, this.MinimumScale.Y));
            Size size = DefaultLayout.Measure(layoutNode, constraint1);
            SizeF sizeF = new SizeF(1f, 1f);
            if (!size.IsZero)
                sizeF = new SizeF((float)constraint.Width / (float)size.Width, (float)constraint.Height / (float)size.Height);
            if (this._maintainAspectRatio)
            {
                float num = Math.Min(sizeF.Width, sizeF.Height);
                sizeF = new SizeF(num, num);
            }
            sizeF.Width = Math.Max(sizeF.Width, this.MinimumScale.X);
            sizeF.Height = Math.Max(sizeF.Height, this.MinimumScale.Y);
            if ((double)this.MaximumScale.X != 0.0)
                sizeF.Width = Math.Min(sizeF.Width, this.MaximumScale.X);
            if ((double)this.MaximumScale.Y != 0.0)
                sizeF.Height = Math.Min(sizeF.Height, this.MaximumScale.Y);
            layoutNode.MeasureData = (object)sizeF;
            size.Width = (int)Math.Round((double)size.Width * (double)sizeF.Width);
            size.Height = (int)Math.Round((double)size.Height * (double)sizeF.Height);
            return size;
        }

        private int UnscaleConstraint(int constraint, float minScale) => (double)minScale == 0.0 ? 16777215 : Math.Min((int)Math.Round((double)constraint / (double)minScale), 16777215);

        void ILayout.Arrange(ILayoutNode layoutNode, LayoutSlot slot)
        {
            if (layoutNode.LayoutChildrenCount <= 0)
                return;
            SizeF measureData = (SizeF)layoutNode.MeasureData;
            Vector3 scale = new Vector3(measureData.Width, measureData.Height, 1f);
            slot.View = ScaleLayout.ScaleView(slot.View, measureData);
            slot.PeripheralView = ScaleLayout.ScaleView(slot.PeripheralView, measureData);
            foreach (ILayoutNode layoutChild in layoutNode.LayoutChildren)
                layoutChild.Arrange(slot, new Rectangle(Point.Zero, layoutChild.DesiredSize), scale, Rotation.Default);
        }

        private static Rectangle ScaleView(Rectangle view, SizeF scale)
        {
            view.X = (int)Math.Round((double)view.X / (double)scale.Width);
            view.Y = (int)Math.Round((double)view.Y / (double)scale.Height);
            view.Width = (int)Math.Round((double)view.Width / (double)scale.Width);
            view.Height = (int)Math.Round((double)view.Height / (double)scale.Height);
            return view;
        }
    }
}
