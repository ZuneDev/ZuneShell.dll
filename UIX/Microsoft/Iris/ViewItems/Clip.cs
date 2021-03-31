// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.Clip
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.ViewItems
{
    internal class Clip : ViewItem
    {
        private bool _showNear;
        private bool _showFar;
        private float _nearOffset;
        private float _farOffset;
        private float _nearPercent;
        private float _farPercent;
        private EdgeFade _edgefade;

        public Clip()
        {
            this._showNear = true;
            this._showFar = true;
            this._farPercent = 1f;
            this.ClipMouse = true;
            this._edgefade = new EdgeFade();
        }

        protected override void OnDispose()
        {
            this._edgefade.Dispose();
            this._edgefade = (EdgeFade)null;
            base.OnDispose();
        }

        public Orientation Orientation
        {
            get => this._edgefade.Orientation;
            set
            {
                if (this.Orientation == value)
                    return;
                this._edgefade.Orientation = value;
                this.OnOrientationChanged();
                this.FireNotification(NotificationID.Orientation);
            }
        }

        public virtual void OnOrientationChanged()
        {
        }

        public float FadeSize
        {
            get => this._edgefade.FadeSize;
            set
            {
                if ((double)this.FadeSize == (double)value)
                    return;
                this._edgefade.FadeSize = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.FadeSize);
            }
        }

        public float NearOffset
        {
            get => this._nearOffset;
            set
            {
                if ((double)this._nearOffset == (double)value)
                    return;
                this._nearOffset = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.NearOffset);
            }
        }

        public float FarOffset
        {
            get => this._farOffset;
            set
            {
                if ((double)this._farOffset == (double)value)
                    return;
                this._farOffset = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.FarOffset);
            }
        }

        public float NearPercent
        {
            get => this._nearPercent;
            set
            {
                if ((double)this._nearPercent == (double)value)
                    return;
                this._nearPercent = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.NearPercent);
            }
        }

        public float FarPercent
        {
            get => this._farPercent;
            set
            {
                if ((double)this._farPercent == (double)value)
                    return;
                this._farPercent = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.FarPercent);
            }
        }

        public bool ShowNear
        {
            get => this._showNear;
            set
            {
                if (this._showNear == value)
                    return;
                this._showNear = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.ShowNear);
            }
        }

        public bool ShowFar
        {
            get => this._showFar;
            set
            {
                if (this._showFar == value)
                    return;
                this._showFar = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.ShowFar);
            }
        }

        public Color ColorMask
        {
            get => this._edgefade.ColorMask;
            set
            {
                if (!(this._edgefade.ColorMask != value))
                    return;
                this._edgefade.ColorMask = value;
                this.FireNotification(NotificationID.ColorMask);
            }
        }

        public float FadeAmount
        {
            get => this._edgefade.FadeAmount;
            set
            {
                if ((double)this._edgefade.FadeAmount == (double)value)
                    return;
                this._edgefade.FadeAmount = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.FadeAmount);
                this.UpdateEdgeFade();
            }
        }

        protected override void OnPaint(bool visible)
        {
            base.OnPaint(visible);
            this.UpdateEdgeFade();
        }

        protected void UpdateEdgeFade()
        {
            IVisualContainer visualContainer = this.VisualContainer;
            if (visualContainer == null)
                return;
            float num = this.Orientation == Orientation.Horizontal ? this.VisualSize.X : this.VisualSize.Y;
            this._edgefade.MinOffset = num * this.NearPercent + this.NearOffset;
            this._edgefade.MaxOffset = num * (this.FarPercent - 1f) + this.FarOffset;
            this._edgefade.ApplyGradients(visualContainer, this.UISession.RenderSession, this._showNear, this._showFar);
        }

        public override void ClipAreaOfInterest(ref AreaOfInterest interest, Size usedSize)
        {
            if (interest.Id != AreaOfInterestID.Focus && interest.Id != AreaOfInterestID.FocusOverride)
                return;
            Rectangle displayRectangle = interest.DisplayRectangle;
            int fadeSize = (int)this.FadeSize;
            if (this.Orientation == Orientation.Horizontal)
            {
                int right = displayRectangle.Right;
                if (this.ShowNear)
                {
                    displayRectangle.X = Math.Max(displayRectangle.X, fadeSize);
                    displayRectangle.Width = right - displayRectangle.X;
                }
                if (this.ShowFar)
                    displayRectangle.Width = Math.Min(right, usedSize.Width - fadeSize) - displayRectangle.X;
            }
            else
            {
                int bottom = displayRectangle.Bottom;
                if (this.ShowNear)
                {
                    displayRectangle.Y = Math.Max(displayRectangle.Y, fadeSize);
                    displayRectangle.Height = bottom - displayRectangle.Y;
                }
                if (this.ShowFar)
                    displayRectangle.Height = Math.Min(bottom, usedSize.Height - fadeSize) - displayRectangle.Y;
            }
            if (displayRectangle.Width > 0 && displayRectangle.Height > 0)
                interest.DisplayRectangle = displayRectangle;
            else
                interest.DisplayRectangle = Rectangle.Zero;
        }
    }
}
