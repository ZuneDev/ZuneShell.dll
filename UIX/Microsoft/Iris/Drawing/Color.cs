// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.Color
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;
using System;
using System.Text;

namespace Microsoft.Iris.Drawing
{
    internal struct Color
    {
        private const int ARGBAlphaShift = 24;
        private const int ARGBRedShift = 16;
        private const int ARGBGreenShift = 8;
        private const int ARGBBlueShift = 0;
        private readonly uint value;

        public Color(int red, int green, int blue) => this = Color.FromArgb((int)byte.MaxValue, red, green, blue);

        public Color(float red, float green, float blue) => this = Color.FromArgb(1f, red, green, blue);

        public Color(int alpha, int red, int green, int blue) => this = Color.FromArgb(alpha, red, green, blue);

        public Color(float alpha, float red, float green, float blue) => this = Color.FromArgb(alpha, red, green, blue);

        internal Color(uint value) => this.value = value;

        public byte R
        {
            get => (byte)(this.Value >> 16 & (uint)byte.MaxValue);
            set => this = Color.FromArgb((int)this.A, (int)value, (int)this.G, (int)this.B);
        }

        public byte G
        {
            get => (byte)(this.Value >> 8 & (uint)byte.MaxValue);
            set => this = Color.FromArgb((int)this.A, (int)this.R, (int)value, (int)this.B);
        }

        public byte B
        {
            get => (byte)(this.Value & (uint)byte.MaxValue);
            set => this = Color.FromArgb((int)this.A, (int)this.R, (int)this.G, (int)value);
        }

        public byte A
        {
            get => (byte)(this.Value >> 24 & (uint)byte.MaxValue);
            set => this = Color.FromArgb((int)value, (int)this.R, (int)this.G, (int)this.B);
        }

        internal void GetArgb(out float a, out float r, out float g, out float b)
        {
            a = (float)this.A / (float)byte.MaxValue;
            r = (float)this.R / (float)byte.MaxValue;
            g = (float)this.G / (float)byte.MaxValue;
            b = (float)this.B / (float)byte.MaxValue;
        }

        internal uint Value => this.value;

        private static void CheckByte(int value, string name)
        {
        }

        private static int ChannelFromFloat(float value) => (int)((double)value * (double)byte.MaxValue);

        private static uint MakeArgb(byte alpha, byte red, byte green, byte blue) => (uint)((int)red << 16 | (int)green << 8 | (int)blue | (int)alpha << 24);

        internal static Color FromArgb(uint argb) => new Color(argb);

        internal static Color FromArgb(int alpha, int red, int green, int blue)
        {
            Color.CheckByte(alpha, nameof(alpha));
            Color.CheckByte(red, nameof(red));
            Color.CheckByte(green, nameof(green));
            Color.CheckByte(blue, nameof(blue));
            return new Color(Color.MakeArgb((byte)alpha, (byte)red, (byte)green, (byte)blue));
        }

        internal static Color FromArgb(float alpha, float red, float green, float blue) => Color.FromArgb(Color.ChannelFromFloat(alpha), Color.ChannelFromFloat(red), Color.ChannelFromFloat(green), Color.ChannelFromFloat(blue));

        internal static Color FromArgb(int alpha, Color baseColor)
        {
            Color.CheckByte(alpha, nameof(alpha));
            return new Color(Color.MakeArgb((byte)alpha, baseColor.R, baseColor.G, baseColor.B));
        }

        internal static Color FromArgb(int red, int green, int blue) => Color.FromArgb((int)byte.MaxValue, red, green, blue);

        internal float GetValue()
        {
            float num1 = (float)this.R / (float)byte.MaxValue;
            float num2 = (float)this.G / (float)byte.MaxValue;
            float num3 = (float)this.B / (float)byte.MaxValue;
            float num4 = num1;
            float num5 = num1;
            if ((double)num2 > (double)num4)
                num4 = num2;
            if ((double)num3 > (double)num4)
                num4 = num3;
            if ((double)num2 < (double)num5)
                num5 = num2;
            if ((double)num3 < (double)num5)
                num5 = num3;
            return (float)(((double)num4 + (double)num5) / 2.0);
        }

        internal float GetHue()
        {
            if ((int)this.R == (int)this.G && (int)this.G == (int)this.B)
                return 0.0f;
            float num1 = (float)this.R / (float)byte.MaxValue;
            float num2 = (float)this.G / (float)byte.MaxValue;
            float num3 = (float)this.B / (float)byte.MaxValue;
            float num4 = 0.0f;
            float num5 = num1;
            float num6 = num1;
            if ((double)num2 > (double)num5)
                num5 = num2;
            if ((double)num3 > (double)num5)
                num5 = num3;
            if ((double)num2 < (double)num6)
                num6 = num2;
            if ((double)num3 < (double)num6)
                num6 = num3;
            float num7 = num5 - num6;
            if ((double)num1 == (double)num5)
                num4 = (num2 - num3) / num7;
            else if ((double)num2 == (double)num5)
                num4 = (float)(2.0 + ((double)num3 - (double)num1) / (double)num7);
            else if ((double)num3 == (double)num5)
                num4 = (float)(4.0 + ((double)num1 - (double)num2) / (double)num7);
            float num8 = num4 * 60f;
            if ((double)num8 < 0.0)
                num8 += 360f;
            return num8;
        }

        internal float GetSaturation()
        {
            float num1 = (float)this.R / (float)byte.MaxValue;
            float num2 = (float)this.G / (float)byte.MaxValue;
            float num3 = (float)this.B / (float)byte.MaxValue;
            float num4 = 0.0f;
            float num5 = num1;
            float num6 = num1;
            if ((double)num2 > (double)num5)
                num5 = num2;
            if ((double)num3 > (double)num5)
                num5 = num3;
            if ((double)num2 < (double)num6)
                num6 = num2;
            if ((double)num3 < (double)num6)
                num6 = num3;
            if ((double)num5 != (double)num6)
                num4 = ((double)num5 + (double)num6) / 2.0 > 0.5 ? (float)(((double)num5 - (double)num6) / (2.0 - (double)num5 - (double)num6)) : (float)(((double)num5 - (double)num6) / ((double)num5 + (double)num6));
            return num4;
        }

        internal static Color FromHSV(int nAlpha, float flHue, float flSaturation, float flValue)
        {
            if ((double)flSaturation == 0.0)
                return new Color(nAlpha, (int)((double)flValue * (double)byte.MaxValue), (int)((double)flValue * (double)byte.MaxValue), (int)((double)flValue * (double)byte.MaxValue));
            float num1 = flHue / 60f;
            int num2 = (int)Math.Floor((double)num1) % 6;
            float num3 = num1 - (float)num2;
            float num4 = flValue * (1f - flSaturation);
            float num5 = flValue * (float)(1.0 - (double)num3 * (double)flSaturation);
            float num6 = flValue * (float)(1.0 - (1.0 - (double)num3) * (double)flSaturation);
            switch (num2)
            {
                case 0:
                    return new Color(nAlpha, (int)((double)flValue * (double)byte.MaxValue), (int)((double)num6 * (double)byte.MaxValue), (int)((double)num4 * (double)byte.MaxValue));
                case 1:
                    return new Color(nAlpha, (int)((double)num5 * (double)byte.MaxValue), (int)((double)flValue * (double)byte.MaxValue), (int)((double)num4 * (double)byte.MaxValue));
                case 2:
                    return new Color(nAlpha, (int)((double)num4 * (double)byte.MaxValue), (int)((double)flValue * (double)byte.MaxValue), (int)((double)num6 * (double)byte.MaxValue));
                case 3:
                    return new Color(nAlpha, (int)((double)num4 * (double)byte.MaxValue), (int)((double)num5 * (double)byte.MaxValue), (int)((double)flValue * (double)byte.MaxValue));
                case 4:
                    return new Color(nAlpha, (int)((double)num6 * (double)byte.MaxValue), (int)((double)num4 * (double)byte.MaxValue), (int)((double)flValue * (double)byte.MaxValue));
                case 5:
                    return new Color(nAlpha, (int)((double)flValue * (double)byte.MaxValue), (int)((double)num4 * (double)byte.MaxValue), (int)((double)num5 * (double)byte.MaxValue));
                default:
                    return new Color(nAlpha, 0, 0, 0);
            }
        }

        internal int ToArgb() => (int)this.Value;

        internal ColorF RenderConvert() => new ColorF((int)this.A, (int)this.R, (int)this.G, (int)this.B);

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(32);
            stringBuilder.Append("A=");
            stringBuilder.Append(this.A);
            stringBuilder.Append(", R=");
            stringBuilder.Append(this.R);
            stringBuilder.Append(", G=");
            stringBuilder.Append(this.G);
            stringBuilder.Append(", B=");
            stringBuilder.Append(this.B);
            return stringBuilder.ToString();
        }

        public static bool operator ==(Color left, Color right) => (int)left.value == (int)right.value;

        public static bool operator !=(Color left, Color right) => !(left == right);

        public override bool Equals(object obj) => obj is Color color && (int)this.value == (int)color.value;

        public bool Equals(Color right) => (int)this.value == (int)right.value;

        public override int GetHashCode() => this.value.GetHashCode();

        internal static Color Transparent => new Color(0U);

        internal static Color Black => new Color(4278190080U);

        internal static Color White => new Color(uint.MaxValue);
    }
}
