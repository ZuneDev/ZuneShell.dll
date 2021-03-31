// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.RawImageItem
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;
using Microsoft.Iris.Render.Extensions;
using System;

namespace Microsoft.Iris.Drawing
{
    internal class RawImageItem : ImageCacheItem
    {
        private int _stride;
        private SurfaceFormat _format;
        private object _oKeepAlive;

        internal RawImageItem(
          IRenderSession renderSession,
          RawImage rawImage,
          string source,
          IntPtr data,
          uint length,
          Size imageSize,
          int stride,
          SurfaceFormat format,
          Size maxSize,
          bool flippable,
          bool antialiasEdges)
          : base(renderSession, source, maxSize, flippable, antialiasEdges)
        {
            this._oKeepAlive = rawImage;
            this.SetSize(imageSize);
            this.SetBuffer(data, length);
            this._stride = stride;
            this._format = format;
        }

        protected override void OnDispose()
        {
            this._oKeepAlive = null;
            this.m_buffer = IntPtr.Zero;
            base.OnDispose();
        }

        protected override bool DoImageLoad() => ImageLoader.FromRaw(this.RenderImage, this.m_buffer, (int)this.m_length, this.m_size, this._stride, this._format, this.m_req.MaximumSize, this.m_req.Flippable, this.m_req.AntialiasEdges, this.m_req.BorderWidth, this.m_req.BorderColor, out this.m_info);
    }
}
