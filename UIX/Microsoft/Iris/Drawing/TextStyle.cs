// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.TextStyle
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System;
using System.Collections.Specialized;
using System.Text;

namespace Microsoft.Iris.Drawing
{
    internal class TextStyle
    {
        private BitVector32 _flags;
        private string _fontFace;
        private float _fontHeightPts;
        private float _altFontHeightPts;
        private float _lineSpacing;
        private float _characterSpacing;
        private Color _textColor;
        private bool _fragment;

        public bool IsInitialized() => this._flags.Data != 0;

        public string FontFace
        {
            get => this._fontFace;
            set
            {
                this._flags[1] = !string.IsNullOrEmpty(value);
                this._fontFace = value;
            }
        }

        public float FontSize
        {
            get => this._fontHeightPts;
            set
            {
                this._flags[2] = true;
                this._fontHeightPts = value;
            }
        }

        public float AltFontSize
        {
            get => _altFontHeightPts == 0.0 ? this._fontHeightPts : this._altFontHeightPts;
            set
            {
                this._flags[512] = true;
                this._altFontHeightPts = value;
            }
        }

        public bool Bold
        {
            get => this._flags[65536];
            set
            {
                this._flags[4] = true;
                this._flags[65536] = value;
            }
        }

        public bool Italic
        {
            get => this._flags[131072];
            set
            {
                this._flags[8] = true;
                this._flags[131072] = value;
            }
        }

        public bool Underline
        {
            get => this._flags[262144];
            set
            {
                this._flags[16] = true;
                this._flags[262144] = value;
            }
        }

        public Color Color
        {
            get => this._textColor;
            set
            {
                this._flags[64] = true;
                this._textColor = value;
            }
        }

        public float LineSpacing
        {
            get => this._lineSpacing;
            set
            {
                this._flags[32] = true;
                this._lineSpacing = value;
            }
        }

        public bool EnableKerning
        {
            get => this._flags[524288];
            set
            {
                this._flags[128] = true;
                this._flags[524288] = value;
            }
        }

        public float CharacterSpacing
        {
            get => this._characterSpacing;
            set
            {
                this._flags[256] = true;
                this._characterSpacing = value;
            }
        }

        public bool Fragment
        {
            get => this._fragment;
            set => this._fragment = value;
        }

        public void Add(TextStyle additional)
        {
            this._fragment = additional._fragment;
            if (additional._flags[1])
                this.FontFace = additional.FontFace;
            if (additional._flags[2])
                this.FontSize = additional.FontSize;
            if (additional._flags[512])
                this.AltFontSize = additional._altFontHeightPts;
            if (additional._flags[4])
                this.Bold = additional.Bold;
            if (additional._flags[8])
                this.Italic = additional.Italic;
            if (additional._flags[16])
                this.Underline = additional.Underline;
            if (additional._flags[32])
                this.LineSpacing = additional.LineSpacing;
            if (additional._flags[128])
                this.EnableKerning = additional.EnableKerning;
            if (additional._flags[256])
                this.CharacterSpacing = additional.CharacterSpacing;
            if (!additional._flags[64])
                return;
            this.Color = additional.Color;
        }

        public bool HasColor => this._flags[64];

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{TextStyle");
            if (this._flags[1])
            {
                stringBuilder.Append(" Font = \"");
                stringBuilder.Append(this.FontFace);
                stringBuilder.Append("\"");
            }
            if (this._flags[2])
            {
                stringBuilder.Append(" Pt = ");
                stringBuilder.Append(this.FontSize);
            }
            if (this._flags[4])
            {
                stringBuilder.Append(" Bold = ");
                stringBuilder.Append(this.Bold);
            }
            if (this._flags[8])
            {
                stringBuilder.Append(" Italic = ");
                stringBuilder.Append(this.Italic);
            }
            if (this._flags[16])
            {
                stringBuilder.Append(" Underline = ");
                stringBuilder.Append(this.Underline);
            }
            if (this._flags[32])
            {
                stringBuilder.Append(" LineSpacing = ");
                stringBuilder.Append(this.LineSpacing);
            }
            if (this._flags[64])
            {
                stringBuilder.Append(" Color = ");
                stringBuilder.Append(Color);
            }
            stringBuilder.Append(" }");
            return stringBuilder.ToString();
        }

        [Flags]
        internal enum SetFlags
        {
            None = 0,
            FontFace = 1,
            FontHeight = 2,
            Bold = 4,
            Italic = 8,
            Underline = 16, // 0x00000010
            LineSpacing = 32, // 0x00000020
            TextColor = 64, // 0x00000040
            EnableKerning = 128, // 0x00000080
            CharacterSpacing = 256, // 0x00000100
            AltFontHeight = 512, // 0x00000200
            BoldValue = 65536, // 0x00010000
            ItalicValue = 131072, // 0x00020000
            UnderlineValue = 262144, // 0x00040000
            EnableKerningValue = 524288, // 0x00080000
        }

        internal struct MarshalledData
        {
            public int _flags;
            public unsafe char* _fontFace;
            public float _fontHeightPts;
            public float _altFontHeightPts;
            public float _lineSpacing;
            public float _characterSpacing;
            public Color _textColor;

            public unsafe MarshalledData(TextStyle from)
            {
                this._flags = from._flags.Data;
                this._fontHeightPts = from._fontHeightPts;
                this._altFontHeightPts = from._altFontHeightPts;
                this._lineSpacing = from._lineSpacing;
                this._characterSpacing = from._characterSpacing;
                this._textColor = from._textColor;
                this._fontFace = null;
            }
        }
    }
}
