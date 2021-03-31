// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Layouts.MajorMinor
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using System;

namespace Microsoft.Iris.Layouts
{
    internal struct MajorMinor
    {
        private int major;
        private int minor;
        public static readonly MajorMinor Zero = new MajorMinor(0, 0);

        public MajorMinor(int major, int minor)
        {
            this.major = major;
            this.minor = minor;
        }

        public MajorMinor(Size size, Orientation o)
        {
            switch (o)
            {
                case Orientation.Horizontal:
                    this.major = size.Width;
                    this.minor = size.Height;
                    break;
                default:
                    this.major = size.Height;
                    this.minor = size.Width;
                    break;
            }
        }

        public SizeF ToSizeF(Orientation o)
        {
            switch (o)
            {
                case Orientation.Horizontal:
                    return new SizeF((float)this.major, (float)this.minor);
                default:
                    return new SizeF((float)this.minor, (float)this.major);
            }
        }

        public Size ToSize(Orientation o)
        {
            switch (o)
            {
                case Orientation.Horizontal:
                    return new Size(this.major, this.minor);
                default:
                    return new Size(this.minor, this.major);
            }
        }

        public Point ToPoint(Orientation o) => this.ToSize(o).ToPoint();

        public static MajorMinor Min(MajorMinor a, MajorMinor b) => new MajorMinor(Math.Min(a.Major, b.Major), Math.Min(a.Minor, b.Minor));

        public static MajorMinor Max(MajorMinor a, MajorMinor b) => new MajorMinor(Math.Max(a.Major, b.Major), Math.Max(a.Minor, b.Minor));

        public static MajorMinor Clamp(MajorMinor original, MajorMinor min, MajorMinor max) => new MajorMinor(Math2.Clamp(original.Major, min.Major, max.Major), Math2.Clamp(original.Minor, min.Minor, max.Minor));

        public static MajorMinor operator +(MajorMinor left, MajorMinor right) => new MajorMinor(left.Major + right.Major, left.Minor + right.Minor);

        public static MajorMinor operator -(MajorMinor left, MajorMinor right) => new MajorMinor(left.Major - right.Major, left.Minor - right.Minor);

        public static MajorMinor operator *(MajorMinor left, MajorMinor right) => new MajorMinor(left.Major * right.Major, left.Minor * right.Minor);

        public static MajorMinor operator /(MajorMinor left, MajorMinor right) => new MajorMinor(left.Major / right.Major, left.Minor / right.Minor);

        public static bool operator ==(MajorMinor left, MajorMinor right) => left.Major == right.Major && left.Minor == right.Minor;

        public static bool operator !=(MajorMinor left, MajorMinor right) => !(left == right);

        public override bool Equals(object obj) => obj is MajorMinor majorMinor && majorMinor == this;

        public override int GetHashCode() => this.major ^ this.minor;

        public MajorMinor Swap() => new MajorMinor(this.Minor, this.Major);

        public int Major
        {
            get => this.major;
            set => this.major = value;
        }

        public int Minor
        {
            get => this.minor;
            set => this.minor = value;
        }

        public bool IsEmpty => this.Major == 0 || this.Minor == 0;

        public override string ToString() => InvariantString.Format("(Major={0}, Minor={1})", (object)this.Major, (object)this.Minor);
    }
}
