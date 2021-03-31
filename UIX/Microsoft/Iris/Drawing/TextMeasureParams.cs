// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.TextMeasureParams
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.Session;
using Microsoft.Iris.ViewItems;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Iris.Drawing
{
    internal struct TextMeasureParams
    {
        public TextMeasureParams.MarshalledData _data;
        public TextStyle _textStyle;
        public TextMeasureParams.FormattedRange[] _formattedRanges;
        public TextStyle.MarshalledData[] _formattedRangeStyles;
        private GCHandle[] _formattedRangeStyleFontFaces;

        public void Initialize()
        {
            if (!UISession.Default.IsRtl)
                return;
            this._data._flags |= TextMeasureParams.MeasureFlags.IsRtl;
        }

        public void Dispose()
        {
            if (this._formattedRangeStyleFontFaces == null)
                return;
            foreach (GCHandle rangeStyleFontFace in this._formattedRangeStyleFontFaces)
            {
                if (rangeStyleFontFace.IsAllocated)
                    rangeStyleFontFace.Free();
            }
            this._formattedRangeStyleFontFaces = (GCHandle[])null;
        }

        public unsafe void SetContent(char* content)
        {
            this._data._flags |= TextMeasureParams.MeasureFlags.Content;
            this._data._content = content;
        }

        public void SetFormat(LineAlignment lineAlignment, TextStyle style)
        {
            switch (lineAlignment)
            {
                case LineAlignment.Near:
                    this._data._alignment = (byte)1;
                    break;
                case LineAlignment.Center:
                    this._data._alignment = (byte)3;
                    break;
                case LineAlignment.Far:
                    this._data._alignment = (byte)2;
                    break;
            }
            this._textStyle = style;
        }

        public void TrimLeftSideBearing() => this._data._flags |= TextMeasureParams.MeasureFlags.TrimLeftSideBearing;

        public void SetEditMode(bool inEditMode)
        {
            if (inEditMode)
                this._data._flags |= TextMeasureParams.MeasureFlags.FormatOnly;
            else
                this._data._flags &= ~TextMeasureParams.MeasureFlags.FormatOnly;
        }

        public void SetScale(float scale) => this._data._scale = scale;

        public void SetWordWrap(bool wordWrap)
        {
            this._data._flags |= TextMeasureParams.MeasureFlags.WordWrap;
            if (wordWrap)
                this._data._flags |= TextMeasureParams.MeasureFlags.WordWrapValue;
            else
                this._data._flags &= ~TextMeasureParams.MeasureFlags.WordWrapValue;
        }

        public void SetPasswordChar(char passwordChar)
        {
            this._data._flags |= TextMeasureParams.MeasureFlags.PasswordMasked;
            this._data._passwordChar = passwordChar;
        }

        public void SetConstraint(SizeF constraint) => this._data._constraint = constraint;

        public void AllocateFormattedRanges(int formattedRangeCount, int formattedRangeStylesCount)
        {
            this._formattedRanges = new TextMeasureParams.FormattedRange[formattedRangeCount];
            this._formattedRangeStyles = new TextStyle.MarshalledData[formattedRangeStylesCount];
            this._formattedRangeStyleFontFaces = new GCHandle[formattedRangeStylesCount];
            this._data._formattedRangeCount = formattedRangeCount;
            this._data._formattedRangeStylesCount = formattedRangeStylesCount;
        }

        public TextMeasureParams.FormattedRange[] FormattedRanges => this._formattedRanges;

        public unsafe void SetFormattedRangeStyle(int index, TextStyle style)
        {
            this._formattedRangeStyles[index] = new TextStyle.MarshalledData(style);
            string fontFace = style.FontFace;
            if (fontFace == null)
                return;
            GCHandle gcHandle = GCHandle.Alloc((object)fontFace, GCHandleType.Pinned);
            this._formattedRangeStyles[index]._fontFace = (char*)gcHandle.AddrOfPinnedObject().ToPointer();
            this._formattedRangeStyleFontFaces[index] = gcHandle;
        }

        internal struct MarshalledData
        {
            public TextMeasureParams.MeasureFlags _flags;
            public byte _alignment;
            [MarshalAs(UnmanagedType.U2)]
            public char _passwordChar;
            public unsafe char* _content;
            public float _scale;
            public SizeF _constraint;
            public unsafe TextStyle.MarshalledData* _pTextStyle;
            public int _formattedRangeCount;
            public unsafe TextMeasureParams.FormattedRange* _pFormattedRanges;
            public int _formattedRangeStylesCount;
            public unsafe TextStyle.MarshalledData* _pFormattedRangeStyles;
        }

        [Flags]
        internal enum MeasureFlags : byte
        {
            None = 0,
            Content = 1,
            IsRtl = 2,
            WordWrap = 4,
            WordWrapValue = 8,
            PasswordMasked = 16, // 0x10
            TrimLeftSideBearing = 32, // 0x20
            FormatOnly = 64, // 0x40
        }

        internal struct FormattedRange
        {
            public int FirstCharacter;
            public int LastCharacter;
            public Color Color;
            public int StyleIndex;
        }
    }
}
