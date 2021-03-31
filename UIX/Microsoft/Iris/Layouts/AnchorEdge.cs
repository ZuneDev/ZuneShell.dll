// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layouts.AnchorEdge
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Text;

namespace Microsoft.Iris.Layouts
{
    internal class AnchorEdge
    {
        private string _idName;
        private float _percentValue;
        private int _offsetValue;
        private float _maximumPercentValue;
        private int _maximumOffsetValue;
        private float _minimumPercentValue;
        private int _minimumOffsetValue;
        private bool _maximumSetFlag;
        private bool _minimumSetFlag;

        public AnchorEdge()
        {
        }

        public AnchorEdge(string id, float percent)
        {
            this.Id = id;
            this.Percent = percent;
        }

        public string Id
        {
            get => this._idName;
            set => this._idName = value;
        }

        public float Percent
        {
            get => this._percentValue;
            set => this._percentValue = value;
        }

        public int Offset
        {
            get => this._offsetValue;
            set => this._offsetValue = value;
        }

        public float MaximumPercent
        {
            get => this._maximumPercentValue;
            set
            {
                this._maximumPercentValue = value;
                this._maximumSetFlag = true;
            }
        }

        public int MaximumOffset
        {
            get => this._maximumOffsetValue;
            set
            {
                this._maximumOffsetValue = value;
                this._maximumSetFlag = true;
            }
        }

        public float MinimumPercent
        {
            get => this._minimumPercentValue;
            set
            {
                this._minimumPercentValue = value;
                this._minimumSetFlag = true;
            }
        }

        public int MinimumOffset
        {
            get => this._minimumOffsetValue;
            set
            {
                this._minimumOffsetValue = value;
                this._minimumSetFlag = true;
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            stringBuilder.Append(this._idName);
            stringBuilder.Append(", ");
            stringBuilder.Append(this._percentValue);
            if (this._offsetValue != 0)
            {
                stringBuilder.Append(", ");
                stringBuilder.Append(this._offsetValue);
            }
            if (_maximumPercentValue > 0.0)
            {
                stringBuilder.Append(", MaximumPercent=");
                stringBuilder.Append(this._maximumPercentValue);
                stringBuilder.Append(", MaximumOffset=");
                stringBuilder.Append(this._maximumOffsetValue);
            }
            if (_minimumPercentValue > 0.0)
            {
                stringBuilder.Append(", MinimumPercent=");
                stringBuilder.Append(this._minimumPercentValue);
                stringBuilder.Append(", MinimumOffset=");
                stringBuilder.Append(this._minimumOffsetValue);
            }
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

        public static bool operator ==(AnchorEdge lhs, AnchorEdge rhs)
        {
            if ((object)lhs == null && (object)rhs == null)
                return true;
            return (object)lhs != null && lhs.Equals(rhs);
        }

        public static bool operator !=(AnchorEdge lhs, AnchorEdge rhs) => !(lhs == rhs);

        public override bool Equals(object obj)
        {
            AnchorEdge anchorEdge = obj as AnchorEdge;
            return (object)anchorEdge != null && this.Id == anchorEdge.Id && (Percent == (double)anchorEdge.Percent && this.Offset == anchorEdge.Offset) && (this.MaximumSet == anchorEdge.MaximumSet && MaximumPercent == (double)anchorEdge.MaximumPercent && (this.MaximumOffset == anchorEdge.MaximumOffset && this.MinimumSet == anchorEdge.MinimumSet)) && MinimumPercent == (double)anchorEdge.MinimumPercent && this.MinimumOffset == anchorEdge.MinimumOffset;
        }

        public override int GetHashCode() => (this.Id != null ? this.Id.GetHashCode() : 0) ^ this.Percent.GetHashCode() ^ this.Offset << 8 ^ this.MaximumSet.GetHashCode() ^ this.MaximumPercent.GetHashCode() ^ this.MaximumOffset << 16 ^ this.MinimumSet.GetHashCode() ^ this.MinimumPercent.GetHashCode() ^ this.MinimumOffset << 24;

        internal bool MaximumSet => this._maximumSetFlag;

        internal bool MinimumSet => this._minimumSetFlag;
    }
}
