// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.RenderAPI.Drawing.EdgeFade
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.RenderAPI.Drawing
{
    internal class EdgeFade
    {
        private IGradient _minFadeGradient;
        private IGradient _maxFadeGradient;
        private float _fadeSizeValue;
        private float _fadeAmountValue;
        private float _minOffsetValue;
        private float _maxOffsetValue;
        private Orientation _orientation;
        private Color _maskColor;

        public EdgeFade()
        {
            this._orientation = Orientation.Horizontal;
            this._maskColor = Color.FromArgb((int)byte.MaxValue, 0, 0, 0);
            this._fadeAmountValue = 1f;
        }

        public void Dispose() => this.DisposeGradients();

        private void DisposeGradients()
        {
            if (this._minFadeGradient != null)
            {
                this._minFadeGradient.UnregisterUsage((object)this);
                this._minFadeGradient = (IGradient)null;
            }
            if (this._maxFadeGradient == null)
                return;
            this._maxFadeGradient.UnregisterUsage((object)this);
            this._maxFadeGradient = (IGradient)null;
        }

        public float FadeSize
        {
            get => this._fadeSizeValue;
            set
            {
                if ((double)this._fadeSizeValue == (double)value)
                    return;
                this._fadeSizeValue = value;
                this.UpdateFades(true);
            }
        }

        public float FadeAmount
        {
            get => this._fadeAmountValue;
            set
            {
                if ((double)value < 0.0 || (double)value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value), (object)value, "FadeAmount must be between 0.0 and 1.0.");
                if ((double)this._fadeAmountValue == (double)value)
                    return;
                this._fadeAmountValue = value;
                this.UpdateFades(true);
            }
        }

        public float MinOffset
        {
            get => this._minOffsetValue;
            set
            {
                if ((double)this._minOffsetValue == (double)value)
                    return;
                this._minOffsetValue = value;
                this.UpdateFades(true);
            }
        }

        public float MaxOffset
        {
            get => this._maxOffsetValue;
            set
            {
                if ((double)this._maxOffsetValue == (double)value)
                    return;
                this._maxOffsetValue = value;
                this.UpdateFades(true);
            }
        }

        public Orientation Orientation
        {
            get => this._orientation;
            set
            {
                if (this._orientation == value)
                    return;
                this._orientation = value;
                this.UpdateFades(false);
            }
        }

        public Color ColorMask
        {
            get => this._maskColor;
            set
            {
                if (!(this._maskColor != value))
                    return;
                this._maskColor = value;
                this.UpdateFades(false);
            }
        }

        internal void ApplyGradients(
          IVisualContainer visContainer,
          IRenderSession renderSession,
          bool minFlag,
          bool maxFlag)
        {
            visContainer.RemoveAllGradients();
            this.CreateFades(renderSession);
            this.UpdateFades(true);
            if (minFlag && this._minFadeGradient != null)
                visContainer.AddGradient(this._minFadeGradient);
            if (!maxFlag || this._maxFadeGradient == null)
                return;
            visContainer.AddGradient(this._maxFadeGradient);
        }

        internal bool NeedFades => (double)this._fadeSizeValue != 0.0 && (double)this._fadeAmountValue != 0.0;

        private void CreateFades(IRenderSession renderSession)
        {
            if (!this.NeedFades)
                return;
            if (this._minFadeGradient == null)
            {
                this._minFadeGradient = renderSession.CreateGradient((object)this);
                this._minFadeGradient.Orientation = this._orientation;
                this._minFadeGradient.ColorMask = this._maskColor.RenderConvert();
            }
            if (this._maxFadeGradient != null)
                return;
            this._maxFadeGradient = renderSession.CreateGradient((object)this);
            this._maxFadeGradient.Orientation = this._orientation;
            this._maxFadeGradient.ColorMask = this._maskColor.RenderConvert();
        }

        private void UpdateFades(bool isOffsetChange)
        {
            if (!this.NeedFades)
            {
                this.DisposeGradients();
            }
            else
            {
                if (this._minFadeGradient == null)
                    return;
                this._minFadeGradient.Orientation = this._orientation;
                this._maxFadeGradient.Orientation = this._orientation;
                this._minFadeGradient.ColorMask = this._maskColor.RenderConvert();
                this._maxFadeGradient.ColorMask = this._maskColor.RenderConvert();
                if (!isOffsetChange)
                    return;
                this._minFadeGradient.Clear();
                this._maxFadeGradient.Clear();
                float flValue1 = 1f;
                float flValue2 = 1f - this._fadeAmountValue;
                float flPosition1;
                float flPosition2;
                if (!UISession.Default.IsRtl || this._orientation == Orientation.Vertical)
                {
                    flPosition1 = this._minOffsetValue;
                    flPosition2 = this._maxOffsetValue;
                }
                else
                {
                    flPosition1 = -this._maxOffsetValue;
                    flPosition2 = -this._minOffsetValue;
                }
                if ((double)this.FadeSize > 0.0)
                {
                    this._minFadeGradient.AddValue(flPosition1, flValue2, RelativeSpace.Min);
                    this._minFadeGradient.AddValue(flPosition1 + this.FadeSize, flValue1, RelativeSpace.Min);
                    this._maxFadeGradient.AddValue(flPosition2 - this.FadeSize, flValue1, RelativeSpace.Max);
                    this._maxFadeGradient.AddValue(flPosition2, flValue2, RelativeSpace.Max);
                }
                else
                {
                    this._minFadeGradient.AddValue(flPosition1 + this.FadeSize, flValue2, RelativeSpace.Min);
                    this._minFadeGradient.AddValue(flPosition1, flValue1, RelativeSpace.Min);
                    this._maxFadeGradient.AddValue(flPosition2, flValue1, RelativeSpace.Max);
                    this._maxFadeGradient.AddValue(flPosition2 - this.FadeSize, flValue2, RelativeSpace.Max);
                }
            }
        }
    }
}
