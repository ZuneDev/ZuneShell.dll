// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.TransformByAttributeAnimation
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.Animations
{
    internal class TransformByAttributeAnimation : TransformAnimation
    {
        private TransformAttribute _attrib;
        private float _maxTimeScaleValue;
        private float _maxTimeOffsetValue;
        private float _maxMagnitudeValue;
        private float _overrideValue;
        private bool _haveOverrideFlag;
        private ValueTransformer _valueTransformer;

        public TransformByAttributeAnimation()
        {
            this.Delay = 0.0f;
            this.Magnitude = 0.0f;
            this.TimeScale = 0.0f;
            this.Attribute = TransformAttribute.Index;
        }

        public TransformAttribute Attribute
        {
            get => this._attrib;
            set => this._attrib = value;
        }

        public float MaxTimeScale
        {
            get => this._maxTimeScaleValue;
            set
            {
                if (_maxTimeScaleValue == (double)value)
                    return;
                this._maxTimeScaleValue = value;
                this.ClearCache();
            }
        }

        public float MaxDelay
        {
            get => this._maxTimeOffsetValue;
            set
            {
                if (_maxTimeOffsetValue == (double)value)
                    return;
                this._maxTimeOffsetValue = value;
                this.ClearCache();
            }
        }

        public float MaxMagnitude
        {
            get => this._maxMagnitudeValue;
            set
            {
                if (_maxMagnitudeValue == (double)value)
                    return;
                this._maxMagnitudeValue = value;
                this.ClearCache();
            }
        }

        public float Override
        {
            get => this._overrideValue;
            set
            {
                if (this._haveOverrideFlag && _overrideValue == (double)value)
                    return;
                this._overrideValue = value;
                this._haveOverrideFlag = true;
                this.ClearCache();
            }
        }

        public ValueTransformer ValueTransformer
        {
            get => this._valueTransformer;
            set
            {
                if (this._valueTransformer == value)
                    return;
                this._valueTransformer = value;
                this.ClearCache();
            }
        }

        protected override float GetTimeScale(ref AnimationArgs args)
        {
            float timeScale = base.GetTimeScale(ref args);
            if (timeScale == 0.0)
                return 1f;
            float val1 = 1f + this.GetValue(ref args) * timeScale;
            if (_maxTimeScaleValue != 0.0)
                val1 = Math.Min(val1, this._maxTimeScaleValue);
            if (val1 < 0.0)
                val1 = 0.0f;
            return val1;
        }

        protected override float GetDelayTime(ref AnimationArgs args)
        {
            float delayTime = base.GetDelayTime(ref args);
            if (delayTime == 0.0)
                return 0.0f;
            float val1 = this.GetValue(ref args) * delayTime;
            if (_maxTimeOffsetValue != 0.0)
                val1 = Math.Min(val1, this._maxTimeOffsetValue);
            if (val1 < 0.0)
                val1 = 0.0f;
            return val1;
        }

        protected override float GetMagnitude(ref AnimationArgs args)
        {
            float magnitude = base.GetMagnitude(ref args);
            if (magnitude == 0.0)
                return 1f;
            float val1 = 1f + this.GetValue(ref args) * magnitude;
            if (_maxMagnitudeValue != 0.0)
                val1 = Math.Min(val1, this._maxMagnitudeValue);
            return val1;
        }

        private float GetValue(ref AnimationArgs args)
        {
            float num = this.GetValueWorker(ref args);
            if (this._valueTransformer != null)
                num = this._valueTransformer.Transform(num);
            return num;
        }

        protected virtual float GetValueWorker(ref AnimationArgs args)
        {
            if (this._haveOverrideFlag)
                return this._overrideValue;
            switch (this._attrib)
            {
                case TransformAttribute.Index:
                    return this.GetIndex(ref args);
                case TransformAttribute.Width:
                    return args.NewSize.X;
                case TransformAttribute.Height:
                    return args.NewSize.Y;
                case TransformAttribute.X:
                    return args.NewPosition.X;
                case TransformAttribute.Y:
                    return args.NewPosition.Y;
                default:
                    return 0.0f;
            }
        }

        private int GetIndex(ref AnimationArgs args)
        {
            int num = 0;
            ViewItem viewItem = args.ViewItem;
            if (viewItem != null && viewItem.Parent != null)
                num = viewItem.Parent.Children.IndexOf(viewItem);
            return num;
        }

        public override bool CanCache => false;
    }
}
