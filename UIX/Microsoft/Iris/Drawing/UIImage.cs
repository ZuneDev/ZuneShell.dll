// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.UIImage
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Render;
using Microsoft.Iris.Render.Extensions;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Drawing
{
    internal abstract class UIImage : NotifyObjectBase
    {
        private static Size s_sizeMaximumSurface = new Size(-1, -1);
        protected string _source;
        private Inset _nineGrid;
        protected Size _maximumSize;
        protected bool _flippable;
        protected bool _antialiasEdges;
        private ImageStatus _status;
        protected Size _contentSize;

        public UIImage(Inset nineGrid, Size maximumSize, bool flippable, bool antialiasEdges)
        {
            this._nineGrid = nineGrid;
            this._maximumSize = maximumSize;
            this._flippable = flippable;
            this._antialiasEdges = antialiasEdges;
        }

        public void Load() => this.Load(false);

        private void Load(bool inDraw)
        {
            if (this._status != ImageStatus.PendingLoad)
                return;
            bool needAsyncLoad;
            ImageCacheItem cacheItem = this.GetCacheItem(out needAsyncLoad);
            if (cacheItem == null)
                this.SetStatus(ImageStatus.Error);
            else if (needAsyncLoad)
            {
                this.SetStatus(ImageStatus.Loading);
                cacheItem.StartLoad();
            }
            else
                this.SetStatus(ImageStatus.Complete);
        }

        private ImageCacheItem GetCacheItem() => this.GetCacheItem(out bool _);

        protected abstract ImageCacheItem GetCacheItem(out bool needAsyncLoad);

        protected abstract void EnsureSizeMetrics();

        protected void OnHeaderLoadComplete()
        {
            if (this.LoadComplete == null)
                return;
            this.LoadComplete(this, this.Status);
        }

        protected void SetStatus(ImageStatus status)
        {
            this._status = status;
            if ((this._status == ImageStatus.Error || this._status == ImageStatus.Complete) && this.LoadComplete != null)
                this.LoadComplete(this, this._status);
            this.FireNotification(NotificationID.Status);
        }

        internal IImage RenderImage => this.GetCacheItem()?.RenderImage;

        public ImageStatus Status => this._status;

        public string Source
        {
            get => this._source;
            set
            {
                if (!(this._source != value))
                    return;
                this._source = value;
                this.OnImageAttributeChanged();
            }
        }

        public Inset NineGrid
        {
            get => this._nineGrid;
            set
            {
                if (!(this._nineGrid != value))
                    return;
                this._nineGrid = value;
                this.OnImageAttributeChanged();
            }
        }

        public Size MaximumSize
        {
            get => this._maximumSize;
            set
            {
                if (!(this._maximumSize != value))
                    return;
                this._maximumSize = value;
                this.OnImageAttributeChanged();
            }
        }

        public bool Flippable
        {
            get => this._flippable;
            set
            {
                if (this._flippable == value)
                    return;
                this._flippable = value;
                this.OnImageAttributeChanged();
            }
        }

        public bool IsFlipped => this._flippable && UISession.Default.IsRtl;

        public bool AntialiasEdges
        {
            get => this._antialiasEdges;
            set
            {
                if (this._antialiasEdges == value)
                    return;
                this._antialiasEdges = value;
                this.OnImageAttributeChanged();
            }
        }

        internal Size Size
        {
            get
            {
                this.EnsureSizeMetrics();
                return this._contentSize;
            }
        }

        public int Width
        {
            get
            {
                this.EnsureSizeMetrics();
                return this._contentSize.Width;
            }
        }

        public int Height
        {
            get
            {
                this.EnsureSizeMetrics();
                return this._contentSize.Height;
            }
        }

        internal void AddUser(object user) => this.GetCacheItem()?.RegisterUsage(user);

        public void RemoveUser(object user) => this.GetCacheItem()?.UnregisterUsage(user);

        internal static Size MaximumSurfaceSize(UISession session)
        {
            if (s_sizeMaximumSurface.Width == -1)
                s_sizeMaximumSurface = session.RenderSession.GraphicsDevice.MaximumImageSize;
            return s_sizeMaximumSurface;
        }

        internal static Size ClampSize(Size maxSizeImage)
        {
            Size size = MaximumSurfaceSize(UISession.Default);
            if (maxSizeImage.Width == 0 || maxSizeImage.Width > size.Width)
                maxSizeImage.Width = size.Width;
            if (maxSizeImage.Height == 0 || maxSizeImage.Height > size.Height)
                maxSizeImage.Height = size.Height;
            return maxSizeImage;
        }

        protected virtual void OnImageAttributeChanged()
        {
        }

        public event ContentLoadCompleteHandler LoadComplete;
    }
}
