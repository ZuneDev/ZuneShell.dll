// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.UriImage
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Render;
using Microsoft.Iris.Render.Extensions;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Drawing
{
    internal sealed class UriImage : UIImage
    {
        private ImageCacheKey _cacheKey;

        public UriImage()
          : base(Inset.Zero, Size.Zero, false, false)
        {
        }

        public UriImage(string source, Inset nineGrid, Size maximumSize, bool flippable)
          : this(source, nineGrid, maximumSize, flippable, false)
        {
        }

        public UriImage(
          string source,
          Inset nineGrid,
          Size maximumSize,
          bool flippable,
          bool antialiasEdges)
          : base(nineGrid, maximumSize, flippable, antialiasEdges)
        {
            this.Source = source;
        }

        private ResourceImageItem FetchOrBuildItem()
        {
            ResourceImageItem resourceImageItem = this.GetResourceFromCache();
            if (resourceImageItem == null)
            {
                resourceImageItem = new ResourceImageItem(UISession.Default.RenderSession, this.Source, UIImage.ClampSize(this._maximumSize), this.IsFlipped, this._antialiasEdges);
                resourceImageItem.LoadCompleteHandler += new ContentLoadCompleteHandler(this.OnLoadComplete);
                ScavengeImageCache.Instance.Add(this._cacheKey, (ImageCacheItem)resourceImageItem);
                this.SetStatus(resourceImageItem.Status);
                this._contentSize = new Size(0, 0);
            }
            else if (resourceImageItem.Resource.Status == ResourceStatus.NeedsAcquire || resourceImageItem.Resource.Status == ResourceStatus.Acquiring)
                resourceImageItem.LoadCompleteHandler += new ContentLoadCompleteHandler(this.OnLoadComplete);
            return resourceImageItem;
        }

        private ResourceImageItem GetResourceFromCache()
        {
            Size maxSize = UIImage.ClampSize(this._maximumSize);
            if (this._cacheKey == null)
                this._cacheKey = new ImageCacheKey(this.Source, maxSize, this.IsFlipped, this._antialiasEdges);
            return (ResourceImageItem)ScavengeImageCache.Instance.Lookup(this._cacheKey);
        }

        protected override void OnImageAttributeChanged() => this._cacheKey = (ImageCacheKey)null;

        private void OnLoadComplete(object owner, ImageStatus status)
        {
            ResourceImageItem resourceFromCache = this.GetResourceFromCache();
            if (resourceFromCache != null)
                resourceFromCache.LoadCompleteHandler -= new ContentLoadCompleteHandler(this.OnLoadComplete);
            this.SetStatus(status);
        }

        protected override ImageCacheItem GetCacheItem(out bool needAsyncLoad)
        {
            ResourceImageItem resourceImageItem;
            if (!string.IsNullOrEmpty(this.Source))
            {
                resourceImageItem = this.FetchOrBuildItem();
                if (resourceImageItem != null)
                {
                    needAsyncLoad = resourceImageItem.Resource.Status != ResourceStatus.Available;
                }
                else
                {
                    resourceImageItem = (ResourceImageItem)null;
                    needAsyncLoad = false;
                }
            }
            else
            {
                resourceImageItem = (ResourceImageItem)null;
                needAsyncLoad = false;
            }
            return (ImageCacheItem)resourceImageItem;
        }

        protected override void EnsureSizeMetrics()
        {
            ImageCacheItem cacheItem = this.GetCacheItem(out bool _);
            if (cacheItem == null)
                return;
            this._contentSize = cacheItem.ImageSize;
        }

        public static void RemoveCache(
          string source,
          Size maximumSize,
          bool flippable,
          bool antialiasEdges)
        {
            ScavengeImageCache.Instance.RemoveData(new ImageCacheKey(source, maximumSize, flippable, antialiasEdges));
        }
    }
}
