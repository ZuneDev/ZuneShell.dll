// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.RawImage
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.Render.Extensions;
using Microsoft.Iris.RenderAPI;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.Drawing
{
    internal class RawImage : UIImage
    {
        private string _uniqueID;
        private Size _imageSize;
        private int _stride;
        private SurfaceFormat _format;
        private IntPtr _data;
        private uint _length;
        private RawImageItemKey _cacheItemKey;

        public RawImage(
          string uniqueID,
          Size imageSize,
          int stride,
          SurfaceFormat format,
          IntPtr data,
          bool takeOwnership,
          Inset nineGrid,
          Size maximumSize,
          bool flippable,
          bool antialiasEdges)
          : base(nineGrid, maximumSize, flippable, antialiasEdges)
        {
            this.Source = uniqueID;
            this._uniqueID = uniqueID;
            this._imageSize = imageSize;
            this._stride = stride;
            this._format = format;
            this._contentSize = imageSize;
            int cbCopy = this._imageSize.Height * Math.Abs(stride);
            this._length = (uint)cbCopy;
            if (!takeOwnership)
            {
                this._data = NativeApi.MemAlloc(this._length, false);
                Memory.Copy(this._data, data, cbCopy);
            }
            else
                this._data = data;
        }

        ~RawImage()
        {
            RawImage.FreeBuffer(this._data);
            this._data = IntPtr.Zero;
        }

        internal static void FreeBuffer(IntPtr data)
        {
            if (!(data != IntPtr.Zero))
                return;
            NativeApi.MemFree(data);
        }

        protected override ImageCacheItem GetCacheItem(out bool needAsyncLoad)
        {
            ImageCache instance = (ImageCache)ScavengeImageCache.Instance;
            ImageCacheItem imageCacheItem = (ImageCacheItem)null;
            string str = this.GetHashCode().ToString();
            if (this._cacheItemKey == null)
                this._cacheItemKey = new RawImageItemKey(str);
            else
                imageCacheItem = instance.Lookup((ImageCacheKey)this._cacheItemKey);
            if (imageCacheItem == null)
            {
                Size maxSize = UIImage.ClampSize(this._maximumSize);
                imageCacheItem = (ImageCacheItem)new RawImageItem(UISession.Default.RenderSession, this, str, this._data, this._length, this._imageSize, this._stride, this._format, maxSize, this.IsFlipped, this._antialiasEdges);
                instance.Add((ImageCacheKey)this._cacheItemKey, imageCacheItem);
            }
            needAsyncLoad = false;
            return imageCacheItem;
        }

        protected override void EnsureSizeMetrics()
        {
        }
    }
}
