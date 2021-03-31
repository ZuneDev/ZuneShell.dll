// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.RichTextInfoKey
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.Render.Extensions;
using Microsoft.Iris.RenderAPI.Drawing;

namespace Microsoft.Iris.Drawing
{
    internal class RichTextInfoKey : ImageCacheKey
    {
        private const int FLAGS_IS_ITALIC = 1;
        private const int FLAGS_IS_UNDERLINE = 2;
        private const int FLAGS_IS_OUTLINE = 4;
        private const int FLAGS_IS_THICK_UNDERLINE = 8;
        private const int FLAGS_IS_DOTTED_UNDERLINE = 16;
        private string _samplingMode;
        private string _content;
        private SizeF _srcSizeF;
        private Size _naturalSize;
        private Point _rasterizedOffset;
        private int _fontFaceUniqueId;
        private int _fontSize;
        private int _fontWeight;
        private byte _rasterizerConfig;
        private Color _textColor;
        private int _flags;
        private int _hashCode;

        public RichTextInfoKey(TextRun run, string samplingMode, bool outline, Color textColor)
          : base(run.Content)
        {
            this._samplingMode = samplingMode;
            this._content = run.Content;
            this._srcSizeF = run.RenderBounds.Size;
            this._naturalSize = run.NaturalExtent;
            this._rasterizedOffset = run.RasterizedOffset;
            this._fontFaceUniqueId = run.FontFaceUniqueId;
            this._fontSize = run.FontSize;
            this._fontWeight = run.FontWeight;
            this._rasterizerConfig = run.RasterizerConfig;
            this._textColor = textColor;
            this._flags = 0;
            if (run.Italic)
                this._flags |= 1;
            if (outline)
                this._flags |= 4;
            switch (run.UnderlineStyle)
            {
                case NativeApi.UnderlineStyle.Solid:
                    this._flags |= 2;
                    break;
                case NativeApi.UnderlineStyle.Thick:
                    this._flags |= 8;
                    break;
                case NativeApi.UnderlineStyle.Dotted:
                case NativeApi.UnderlineStyle.Dash:
                case NativeApi.UnderlineStyle.DashDot:
                case NativeApi.UnderlineStyle.DashDotDot:
                    this._flags |= 16;
                    break;
            }
            this._hashCode = this._samplingMode.GetHashCode() ^ this._content.GetHashCode() ^ this._srcSizeF.GetHashCode() ^ this._naturalSize.GetHashCode() ^ this._rasterizedOffset.GetHashCode() ^ this._fontFaceUniqueId.GetHashCode() ^ this._fontSize ^ this._fontWeight ^ this._flags ^ _rasterizerConfig ^ this._textColor.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;
            return obj is RichTextInfoKey richTextInfoKey && this._hashCode == richTextInfoKey._hashCode && (this._fontFaceUniqueId == richTextInfoKey._fontFaceUniqueId && this._fontSize == richTextInfoKey._fontSize) && (this._fontWeight == richTextInfoKey._fontWeight && this._flags == richTextInfoKey._flags && (_rasterizerConfig == richTextInfoKey._rasterizerConfig && this._samplingMode.Equals(richTextInfoKey._samplingMode))) && (this._srcSizeF.Equals(richTextInfoKey._srcSizeF) && this._naturalSize.Equals(richTextInfoKey._naturalSize) && (this._rasterizedOffset.Equals(richTextInfoKey._rasterizedOffset) && this._textColor.Equals(richTextInfoKey._textColor))) && this._content.Equals(richTextInfoKey._content);
        }

        public override int GetHashCode() => this._hashCode;
    }
}
