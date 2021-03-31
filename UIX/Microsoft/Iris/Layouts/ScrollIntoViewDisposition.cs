// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layouts.ScrollIntoViewDisposition
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;

namespace Microsoft.Iris.Layouts
{
    internal class ScrollIntoViewDisposition
    {
        private int _beginPadding;
        private int _endPadding;
        private bool _locked;
        private float _lockedPosition;
        private float _lockedAlignment;
        private ContentPositioningPolicy _positioningPolicy;
        private bool _enabled;
        private RelativeEdge _relativeBeginPadding;
        private RelativeEdge _relativeEndPadding;

        public ScrollIntoViewDisposition()
          : this(0)
        {
        }

        public ScrollIntoViewDisposition(int paddingValue)
        {
            this.Reset();
            this._beginPadding = this._endPadding = paddingValue;
        }

        public void CopyFrom(ScrollIntoViewDisposition other)
        {
            this._beginPadding = other._beginPadding;
            this._endPadding = other._endPadding;
            this._lockedPosition = other._lockedPosition;
            this._lockedAlignment = other._lockedAlignment;
            this._locked = other._locked;
            this._enabled = other._enabled;
            this._positioningPolicy = other._positioningPolicy;
            this._relativeBeginPadding = other._relativeBeginPadding;
            this._relativeEndPadding = other._relativeEndPadding;
        }

        public void Reset()
        {
            this._beginPadding = this._endPadding = 0;
            this._lockedPosition = -1f;
            this._lockedAlignment = 0.5f;
            this._enabled = true;
            this._locked = false;
            this._positioningPolicy = ContentPositioningPolicy.RespectPaddingAndLocking;
            this._relativeBeginPadding = RelativeEdge.Near;
            this._relativeEndPadding = RelativeEdge.Far;
        }

        public bool IsDefault => this._beginPadding == 0 && this._endPadding == 0 && (this.BeginPaddingRelativeTo == RelativeEdge.Near && this.EndPaddingRelativeTo == RelativeEdge.Far) && !this.Locked;

        public int Padding
        {
            get => this.BeginPadding;
            set
            {
                this.EndPadding = value;
                this.BeginPadding = value;
            }
        }

        public int BeginPadding
        {
            get => this._beginPadding;
            set => this._beginPadding = value;
        }

        public int EndPadding
        {
            get => this._endPadding;
            set => this._endPadding = value;
        }

        public RelativeEdge BeginPaddingRelativeTo
        {
            get => this._relativeBeginPadding;
            set => this._relativeBeginPadding = value;
        }

        public RelativeEdge EndPaddingRelativeTo
        {
            get => this._relativeEndPadding;
            set => this._relativeEndPadding = value;
        }

        public bool Locked
        {
            get => this._locked;
            set => this._locked = value;
        }

        public float LockedPosition
        {
            get => this._lockedPosition;
            set => this._lockedPosition = value;
        }

        public float LockedAlignment
        {
            get => this._lockedAlignment;
            set => this._lockedAlignment = value;
        }

        public bool Enabled
        {
            get => this._enabled;
            set => this._enabled = value;
        }

        public ContentPositioningPolicy ContentPositioningBehavior
        {
            get => this._positioningPolicy;
            set => this._positioningPolicy = value;
        }

        public override string ToString()
        {
            string str1 = InvariantString.Format("{0}(", (object)this.GetType().Name);
            string str2;
            if (!this._enabled)
            {
                str2 = InvariantString.Format("{0}Disabled", (object)str1);
            }
            else
            {
                str2 = InvariantString.Format("{0}(BeginPadding={1}({2}), EndPadding={3}({4})", (object)str1, (object)this._beginPadding, (object)this._relativeBeginPadding, (object)this._endPadding, (object)this._relativeEndPadding);
                if (this.Locked)
                    str2 = InvariantString.Format("{0}, LockedPosition={1}, LockedAlignment={2}", (object)str2, (object)this._lockedPosition, (object)this._lockedAlignment);
            }
            return str2 + ")";
        }
    }
}
