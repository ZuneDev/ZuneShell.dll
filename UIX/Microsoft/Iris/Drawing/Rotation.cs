// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.Rotation
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;
using System.Text;

namespace Microsoft.Iris.Drawing
{
    internal struct Rotation
    {
        private Vector3 _axis;
        private float _angleRad;
        public static readonly Rotation Default = new Rotation(0.0f, new Vector3(0.0f, 0.0f, 1f));

        public Rotation(float angleRadians)
          : this(angleRadians, new Vector3(0.0f, 0.0f, 1f))
        {
        }

        public Rotation(float angleRadians, Vector3 axis)
        {
            this._axis = axis;
            this._angleRad = angleRadians;
        }

        public Vector3 Axis
        {
            get => this._axis;
            set => this._axis = value;
        }

        public float AngleRadians
        {
            get => this._angleRad;
            set => this._angleRad = value;
        }

        public int AngleDegrees
        {
            get => (int)(_angleRad * 180.0 / 3.14159274101257);
            set => this._angleRad = (float)(value * 3.14159274101257 / 180.0);
        }

        public override bool Equals(object obj) => obj is Rotation rotation && this == rotation;

        public static bool operator ==(Rotation left, Rotation right) => left.Axis == right.Axis && left.AngleRadians == (double)right.AngleRadians;

        public static bool operator !=(Rotation left, Rotation right) => left.Axis != right.Axis || left.AngleRadians != (double)right.AngleRadians;

        public override int GetHashCode() => this.Axis.GetHashCode() ^ this.AngleRadians.GetHashCode();

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(128);
            stringBuilder.Append("(Axis=");
            stringBuilder.Append(Axis);
            stringBuilder.Append(", Angle=");
            stringBuilder.Append(this.AngleRadians);
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }
    }
}
