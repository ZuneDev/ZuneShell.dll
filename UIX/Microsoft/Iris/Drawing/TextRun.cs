// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.TextRun
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using System;

namespace Microsoft.Iris.Drawing
{
    internal sealed class TextRun : SharedDisposableObject
    {
        private const int CFE_LINK = 32;
        private IntPtr _hGlyphRunInfo;
        private IntPtr _hRasterizeRunPacket;
        private Rectangle _layoutBounds;
        private RectangleF _renderBounds;
        private Point _offsetPoint;
        private int _naturalX;
        private int _naturalY;
        private int _rasterizeX;
        private int _rasterizeY;
        private byte _rasterizerConfig;
        private byte _bits;
        private string _content;
        private Color _runColor;
        private Color _overrideColor;
        private Color _highlightColor;
        private int _fontFaceUniqueId;
        public int _lfHeight;
        public int _lfWeight;
        private Size _naturalRunExtent;
        private int _lineNumber;
        private int _ascenderInset;
        private int _baselineInset;
        private Rectangle _underlineBounds;
        public ISprite TextSprite;
        public ISprite HighlightSprite;

        private unsafe TextRun(
          IntPtr hGlyphRunInfo,
          NativeApi.RasterizeRunPacket* runPacketPtr,
          string content)
        {
            this._hGlyphRunInfo = hGlyphRunInfo;
            this._hRasterizeRunPacket = new IntPtr((void*)runPacketPtr);
            this._layoutBounds = runPacketPtr->rcLayoutBounds;
            this._renderBounds = runPacketPtr->rcfRenderBounds;
            this._naturalX = runPacketPtr->naturalX;
            this._naturalY = runPacketPtr->naturalY;
            this._rasterizeX = runPacketPtr->rasterizeX;
            this._rasterizeY = runPacketPtr->rasterizeY;
            this._rasterizerConfig = runPacketPtr->AAConfig;
            this._runColor = runPacketPtr->clrText;
            this._overrideColor = Color.Transparent;
            this._highlightColor = runPacketPtr->clrBackground;
            this._fontFaceUniqueId = runPacketPtr->fontFaceUniqueId;
            this._lfHeight = runPacketPtr->lf.lfHeight;
            this._lfWeight = runPacketPtr->lf.lfWeight;
            this.SetBit(TextRun.Bits.Italic, runPacketPtr->lf.lfItalic != (byte)0);
            this.SetBit(TextRun.Bits.Underline, runPacketPtr->lf.lfUnderline != (byte)0);
            this.SetBit(TextRun.Bits.Link, (runPacketPtr->dwEffects & 32) != 0);
            this._underlineBounds = runPacketPtr->rcUnderlineBounds;
            this._lineNumber = runPacketPtr->nLineNumber;
            this._naturalRunExtent = runPacketPtr->sizeNatural;
            this._ascenderInset = runPacketPtr->ascenderInset;
            this._baselineInset = runPacketPtr->baselineInset;
            this._content = content;
        }

        protected override void OnDispose()
        {
            NativeApi.SpRichTextDestroyGlyphRunInfo(this._hGlyphRunInfo);
            base.OnDispose();
        }

        public string Content => this._content;

        public RectangleF RenderBounds => this._renderBounds;

        public Rectangle LayoutBounds => this._layoutBounds;

        public Size NaturalExtent => this._naturalRunExtent;

        public Color RunColor => this._runColor;

        public Color OverrideColor
        {
            get => this._overrideColor;
            set => this._overrideColor = value;
        }

        public Color Color => this._overrideColor != Color.Transparent ? this._overrideColor : this._runColor;

        public bool Highlighted => this._highlightColor.A != (byte)0;

        public Color HighlightColor => this._highlightColor;

        public int FontFaceUniqueId => this._fontFaceUniqueId;

        public int FontSize => this._lfHeight;

        public int FontWeight => this._lfWeight;

        public bool Italic => this.GetBit(TextRun.Bits.Italic);

        public bool Underline => this.GetBit(TextRun.Bits.Underline);

        public Point RasterizedOffset => new Point(this._rasterizeX - this._naturalX, this._rasterizeY - this._naturalY);

        public int AscenderInset => this._ascenderInset;

        public int BaselineInset => this._baselineInset;

        public int Line => this._lineNumber;

        public bool Visible
        {
            get => this.GetBit(TextRun.Bits.Visible);
            set => this.SetBit(TextRun.Bits.Visible, value);
        }

        public bool IsFragment
        {
            get => this.GetBit(TextRun.Bits.Fragment);
            set => this.SetBit(TextRun.Bits.Fragment, value);
        }

        public Point Position => new Point(this._layoutBounds.X, this._layoutBounds.Y);

        public Size Size => new Size(this._layoutBounds.Width, this._layoutBounds.Height);

        public byte RasterizerConfig => this._rasterizerConfig;

        public bool Link => this.GetBit(TextRun.Bits.Link);

        public unsafe NativeApi.UnderlineStyle UnderlineStyle
        {
            get => this._hRasterizeRunPacket == IntPtr.Zero ? NativeApi.UnderlineStyle.None : ((NativeApi.RasterizeRunPacket*)(void*)this._hRasterizeRunPacket)->usUnderlineStyle;
            set
            {
                if (!(this._hRasterizeRunPacket != IntPtr.Zero))
                    return;
                ((NativeApi.RasterizeRunPacket*)(void*)this._hRasterizeRunPacket)->usUnderlineStyle = value;
            }
        }

        public Rectangle UnderlineBounds => this._underlineBounds;

        internal void ApplyOffset(Point offsetPoint)
        {
            if (!(this._offsetPoint != offsetPoint))
                return;
            this._renderBounds.X -= (float)this._offsetPoint.X;
            this._renderBounds.Y -= (float)this._offsetPoint.Y;
            this._layoutBounds.X -= this._offsetPoint.X;
            this._layoutBounds.Y -= this._offsetPoint.Y;
            this._renderBounds.X += (float)offsetPoint.X;
            this._renderBounds.Y += (float)offsetPoint.Y;
            this._layoutBounds.X += offsetPoint.X;
            this._layoutBounds.Y += offsetPoint.Y;
            this._offsetPoint = offsetPoint;
        }

        internal Dib Rasterize(string samplingMode, Color textColor, bool outlineFlag)
        {
            bool shadowMode = false;
            if (samplingMode == "sdw")
                shadowMode = true;
            return RichText.Rasterize(this._hGlyphRunInfo, outlineFlag, textColor, shadowMode);
        }

        internal static unsafe TextRun FromRunPacket(
          IntPtr hGlyphRunInfo,
          NativeApi.RasterizeRunPacket* runPacketPtr,
          string content)
        {
            return new TextRun(hGlyphRunInfo, runPacketPtr, content);
        }

        public void RemoveSprites(IVisualContainer container)
        {
            if (this.TextSprite != null)
            {
                container.RemoveChild((IVisual)this.TextSprite);
                this.TextSprite = (ISprite)null;
            }
            if (this.HighlightSprite == null)
                return;
            container.RemoveChild((IVisual)this.HighlightSprite);
            this.HighlightSprite = (ISprite)null;
        }

        private bool GetBit(TextRun.Bits lookupBit) => ((TextRun.Bits)this._bits & lookupBit) != (TextRun.Bits)0;

        private void SetBit(TextRun.Bits changeBit, bool value) => this._bits = value ? (byte)((TextRun.Bits)this._bits | changeBit) : (byte)((TextRun.Bits)this._bits & ~changeBit);

        private enum Bits : byte
        {
            Visible = 1,
            Fragment = 2,
            Italic = 4,
            Underline = 8,
            Link = 16, // 0x10
        }
    }
}
