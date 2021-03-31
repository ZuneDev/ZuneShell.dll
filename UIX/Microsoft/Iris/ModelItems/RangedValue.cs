// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ModelItems.RangedValue
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using System;

namespace Microsoft.Iris.ModelItems
{
    internal class RangedValue : NotifyObjectBase, IUIRangedValue, IUIValueRange, INotifyObject
    {
        private float _value;
        private float _min;
        private float _max;
        private float _step;

        public RangedValue(float min, float max, float step)
        {
            this._min = min;
            this._max = max;
            this._step = step;
        }

        public RangedValue()
          : this(float.MinValue, float.MaxValue, 1f)
        {
        }

        public float Value
        {
            get => this._value;
            set
            {
                value = Math.Max(value, this._min);
                value = Math.Min(value, this._max);
                if (_value == (double)value)
                    return;
                using (new RangedValue.PrevNextNotifier(this))
                {
                    this._value = value;
                    this.FireNotification(NotificationID.Value);
                    this.FireNotification(NotificationID.ObjectValue);
                }
            }
        }

        object IUIValueRange.ObjectValue => _value;

        public float MinValue
        {
            get => this._min;
            set
            {
                if (_min == (double)value)
                    return;
                using (new RangedValue.PrevNextNotifier(this))
                {
                    this._min = value;
                    this.FireNotification(NotificationID.MinValue);
                    this.FireNotification(NotificationID.Range);
                    this.Value = this.Value;
                }
            }
        }

        public float MaxValue
        {
            get => this._max;
            set
            {
                if (_max == (double)value)
                    return;
                using (new RangedValue.PrevNextNotifier(this))
                {
                    this._max = value;
                    this.FireNotification(NotificationID.MaxValue);
                    this.FireNotification(NotificationID.Range);
                    this.Value = this.Value;
                }
            }
        }

        public float Range => this._max - this._min;

        public float Step
        {
            get => this._step;
            set
            {
                if (_step == (double)value)
                    return;
                using (new RangedValue.PrevNextNotifier(this))
                {
                    this._step = value;
                    this.FireNotification(NotificationID.Step);
                }
            }
        }

        public bool HasPreviousValue => _step < 0.0 ? _value < (double)this._max : _value > (double)this._min;

        public bool HasNextValue => _step < 0.0 ? _value > (double)this._min : _value < (double)this._max;

        public void PreviousValue() => this.Value -= this.Step;

        public void NextValue() => this.Value += this.Step;

        private struct PrevNextNotifier : IDisposable
        {
            private bool _hadPrev;
            private bool _hadNext;
            private RangedValue _range;

            public PrevNextNotifier(RangedValue range)
            {
                this._range = range;
                this._hadPrev = range.HasPreviousValue;
                this._hadNext = range.HasNextValue;
            }

            public void Dispose()
            {
                if (this._range.HasPreviousValue != this._hadPrev)
                    this._range.FireNotification(NotificationID.HasPreviousValue);
                if (this._range.HasNextValue == this._hadNext)
                    return;
                this._range.FireNotification(NotificationID.HasNextValue);
            }
        }
    }
}
