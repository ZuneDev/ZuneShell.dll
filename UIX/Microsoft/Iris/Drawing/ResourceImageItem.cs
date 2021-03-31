// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.ResourceImageItem
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Render;
using Microsoft.Iris.Render.Extensions;

namespace Microsoft.Iris.Drawing
{
    internal class ResourceImageItem : ImageCacheItem
    {
        private string _source;
        private Resource _resource;
        private bool _acquireCalled;
        private ResourceAcquisitionCompleteHandler _resourceAcquisitionHandler;

        internal ResourceImageItem(
          IRenderSession renderSession,
          string source,
          Size maxSize,
          bool flippable,
          bool antialiasEdges)
          : base(renderSession, source, maxSize, flippable, antialiasEdges)
        {
            this._source = source;
            this._resource = ResourceManager.Instance.GetResource(this._source);
        }

        protected override void OnDispose()
        {
            if (this._resource != null)
            {
                this.FreeResource();
                this._resource = null;
            }
            base.OnDispose();
        }

        internal Resource Resource
        {
            get
            {
                this.EnsureResource();
                return this._resource;
            }
        }

        internal ImageStatus Status
        {
            get
            {
                switch (this._resource.Status)
                {
                    case ResourceStatus.NeedsAcquire:
                        return ImageStatus.PendingLoad;
                    case ResourceStatus.Acquiring:
                        return ImageStatus.Loading;
                    case ResourceStatus.Available:
                        return ImageStatus.Complete;
                    case ResourceStatus.Error:
                        return ImageStatus.Error;
                    default:
                        return ImageStatus.Error;
                }
            }
        }

        protected override bool EnsureBuffer()
        {
            if (this.EnsureResource())
            {
                if (this.IsResourceAvailable())
                    this.OnResourceLoadComplete(this._resource);
                else if (!this._acquireCalled)
                {
                    this._resourceAcquisitionHandler = new ResourceAcquisitionCompleteHandler(this.OnResourceLoadComplete);
                    this._resource.Acquire(this._resourceAcquisitionHandler);
                    this._acquireCalled = true;
                }
            }
            return false;
        }

        protected override void OnImageLoadComplete()
        {
            ImageStatus status = this.Status;
            if (this.LoadCompleteHandler == null)
                return;
            this.LoadCompleteHandler(this, status);
        }

        private bool IsResourceAvailable() => this._resource.Status == ResourceStatus.Available;

        public bool EnsureResource()
        {
            if (this._resource == null)
                this._resource = ResourceManager.Instance.GetResource(this._source);
            return this._resource != null;
        }

        private void OnResourceLoadComplete(Resource resource)
        {
            if (resource != this._resource)
                return;
            if (!IsSuccessfulResourceLoad(this._resource) || Application.IsShuttingDown)
            {
                this.OnImageLoadComplete();
                this.FreeResource();
            }
            else
            {
                this.SetBuffer(this._resource.Buffer, this._resource.Length);
                if (this._resource.Length <= 0U)
                    return;
                if (!this.ProcessBuffer())
                    this._resource.Status = ResourceStatus.Error;
                this.OnImageLoadComplete();
            }
        }

        private static bool IsSuccessfulResourceLoad(Resource resource) => resource.Status != ResourceStatus.Error;

        private void FreeResource()
        {
            if (this._acquireCalled)
            {
                this._resource.Free(this._resourceAcquisitionHandler);
                this._resourceAcquisitionHandler = null;
            }
            this._acquireCalled = false;
        }

        public override void StartLoad() => this.EnsureBuffer();

        public override string ToString() => this._source;

        public override void ReleaseImage()
        {
            this.LoadCompleteHandler = null;
            base.ReleaseImage();
        }

        public override void RemoveData()
        {
            this.FreeResource();
            base.RemoveData();
        }

        public event ContentLoadCompleteHandler LoadCompleteHandler;
    }
}
