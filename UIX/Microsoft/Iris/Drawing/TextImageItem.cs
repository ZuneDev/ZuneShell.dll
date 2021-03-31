// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.TextImageItem
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;
using Microsoft.Iris.Render.Extensions;
using Microsoft.Iris.RenderAPI.Drawing;

namespace Microsoft.Iris.Drawing
{
    internal class TextImageItem : ImageCacheItem
    {
        private TextRun _run;
        private Dib _dib;
        private string _samplingModeName;
        private Color _textColor;
        private bool _outlineFlag;

        internal TextImageItem(
          IRenderSession renderSession,
          TextRun run,
          string samplingModeName,
          bool outlineFlag,
          Color textColor)
          : base(renderSession, run.Content)
        {
            this._run = run;
            this._run.RegisterUsage((object)this);
            this._dib = (Dib)null;
            this._samplingModeName = samplingModeName;
            this._outlineFlag = outlineFlag;
            this._textColor = textColor;
        }

        protected override void OnDispose()
        {
            this._run.UnregisterUsage((object)this);
            this.ReleaseDib();
            base.OnDispose();
        }

        internal TextRun Run => this._run;

        protected override bool EnsureBuffer()
        {
            if (this._dib == null)
                this._dib = this._run.Rasterize(this._samplingModeName, this._textColor, this._outlineFlag);
            return true;
        }

        protected override bool DoImageLoad() => this.RenderImage.LoadContent(this._dib.ImageFormat, this._dib.ContentSize, this._dib.Stride, this._dib.Data);

        protected override void OnImageLoadComplete()
        {
            if (this.HasLoadsInProgress)
                return;
            this.ReleaseDib();
        }

        public override string ToString() => this._run.Content;

        private void ReleaseDib()
        {
            if (this._dib == null)
                return;
            this._dib.Dispose();
            this._dib = (Dib)null;
        }
    }
}
