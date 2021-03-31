// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.Font
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Text;

namespace Microsoft.Iris.Drawing
{
    internal sealed class Font
    {
        private string _fontName;
        private float _fontHeight;
        private float _altFontHeight;
        private FontStyles _fontStyle;

        public Font()
          : this("Arial", 12f, 0.0f, FontStyles.None)
        {
        }

        public Font(string fontName)
          : this(fontName, 12f, 0.0f, FontStyles.None)
        {
        }

        public Font(string fontName, float fontHeight)
          : this(fontName, fontHeight, 0.0f, FontStyles.None)
        {
        }

        public Font(string fontName, float fontHeight, float altFontHeight)
          : this(fontName, fontHeight, altFontHeight, FontStyles.None)
        {
        }

        public Font(string fontName, float fontHeight, FontStyles fontStyles)
          : this(fontName, fontHeight, 0.0f, fontStyles)
        {
        }

        public Font(string fontName, float fontHeight, float altFontHeight, FontStyles fontStyles)
        {
            this._fontName = fontName;
            this._fontHeight = fontHeight;
            this._altFontHeight = altFontHeight;
            this._fontStyle = fontStyles;
        }

        public float FontSize
        {
            get => this._fontHeight;
            set => this._fontHeight = value;
        }

        public float AltFontSize
        {
            get => _altFontHeight == 0.0 ? this._fontHeight : this._altFontHeight;
            set => this._altFontHeight = value;
        }

        public FontStyles FontStyle
        {
            get => this._fontStyle;
            set => this._fontStyle = value;
        }

        public string FontName
        {
            get => this._fontName;
            set => this._fontName = value;
        }

        public override bool Equals(object obj) => obj is Font font && this._fontName == font._fontName && (_fontHeight == (double)font._fontHeight && _altFontHeight == (double)font._altFontHeight) && this._fontStyle == font._fontStyle;

        public override int GetHashCode() => this._fontName.GetHashCode() ^ this._fontHeight.GetHashCode() ^ this._altFontHeight.GetHashCode() ^ this._fontStyle.GetHashCode();

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Font \"");
            stringBuilder.Append(this._fontName);
            stringBuilder.Append("\" ");
            stringBuilder.Append(this._fontHeight);
            stringBuilder.Append("pt ");
            stringBuilder.Append(_fontStyle);
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }
}
