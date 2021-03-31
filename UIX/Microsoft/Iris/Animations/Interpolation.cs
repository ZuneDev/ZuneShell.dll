// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.Interpolation
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Animations
{
    internal class Interpolation
    {
        public static readonly Interpolation Default = new Interpolation(InterpolationType.Linear);
        private InterpolationType _type;
        private float _weight = 1f;
        private float _bezierHandle1;
        private float _bezierHandle2 = 1f;
        private float _easePercent = 0.5f;

        public Interpolation()
          : this(InterpolationType.Linear)
        {
        }

        public Interpolation(InterpolationType type) => this._type = type;

        public InterpolationType Type
        {
            get => this._type;
            set => this._type = value;
        }

        public float Weight
        {
            get => this._weight;
            set => this._weight = value;
        }

        public float BezierHandle1
        {
            get => this._bezierHandle1;
            set => this._bezierHandle1 = value;
        }

        public float BezierHandle2
        {
            get => this._bezierHandle2;
            set => this._bezierHandle2 = value;
        }

        public float EasePercent
        {
            get => this._easePercent;
            set => this._easePercent = value;
        }

        public override string ToString()
        {
            string str;
            switch (this.Type)
            {
                case InterpolationType.SCurve:
                    str = "SCurve";
                    break;
                case InterpolationType.Exp:
                    str = "Exp";
                    break;
                case InterpolationType.Log:
                    str = "Log";
                    break;
                case InterpolationType.Sine:
                    str = "Sine";
                    break;
                case InterpolationType.Cosine:
                    str = "Cosine";
                    break;
                case InterpolationType.Bezier:
                    str = "Bezier, " + (object)this.BezierHandle1 + ", " + (object)this.BezierHandle2;
                    break;
                case InterpolationType.EaseIn:
                    str = "EaseIn, " + (object)this.Weight + ", " + (object)this.EasePercent;
                    break;
                case InterpolationType.EaseOut:
                    str = "EaseOut, " + (object)this.Weight + ", " + (object)this.EasePercent;
                    break;
                default:
                    str = "Linear";
                    break;
            }
            if ((double)this.Weight != 1.0)
                str = str + ", " + (object)this.Weight;
            return str;
        }

        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Interpolation)
            {
                Interpolation interpolation = (Interpolation)obj;
                flag = this._type == interpolation._type && (double)this._weight == (double)interpolation._weight && ((double)this._bezierHandle1 == (double)interpolation._bezierHandle1 && (double)this._bezierHandle2 == (double)interpolation._bezierHandle2) && (double)this._easePercent == (double)interpolation._easePercent;
            }
            return flag;
        }

        public override int GetHashCode() => this._type.GetHashCode() ^ this._weight.GetHashCode() ^ this._bezierHandle1.GetHashCode() ^ this._bezierHandle2.GetHashCode() ^ this._easePercent.GetHashCode();
    }
}
