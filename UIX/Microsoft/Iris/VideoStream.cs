// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.VideoStream
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.RenderAPI.VideoPlayback;
using Microsoft.Iris.Session;
using System;
using System.Collections;

namespace Microsoft.Iris
{
    public sealed class VideoStream : IUIVideoStream
    {
        private ArrayList _clients;
        private VideoPresentationBuilder _presentationBuilder;
        private IVideoStream _renderStream;
        private bool _disposed;
        private bool _deferredInvalidate;
        private Rectangle _srcVideo;
        private Size _srcAspect;
        private float _contentOverscanPer;
        private WindowPosition _largestPosition;
        private WindowSize _largestSize;
        private bool _isRendering;

        public VideoStream()
        {
            UIDispatcher.VerifyOnApplicationThread();
            this._clients = new ArrayList();
            if (UISession.Default.RenderSession.GraphicsDevice.IsVideoComposited)
            {
                this._renderStream = UISession.Default.RenderSession.CreateVideoStream((object)this);
                this._renderStream.InvalidateContentEvent += new InvalidateContentHandler(this.OnRenderVideoStreamChange);
            }
            this._presentationBuilder = new VideoPresentationBuilder();
            this._presentationBuilder.DisplayMode = VideoDisplayMode.FullInScene;
            this._presentationBuilder.ContentOverscanMode = VideoOverscanMode.InvalidContent;
            this._largestPosition = new WindowPosition();
            this._largestSize = new WindowSize();
            this._isRendering = false;
        }

        public void Dispose()
        {
            UIDispatcher.VerifyOnApplicationThread();
            if (this._disposed)
                throw new InvalidOperationException("The stream has already been disposed");
            this.Dispose(true);
            this._disposed = true;
        }

        private void Dispose(bool inDisposeFlag)
        {
            if (!inDisposeFlag)
                return;
            if (this._renderStream != null)
            {
                this._renderStream.InvalidateContentEvent -= new InvalidateContentHandler(this.OnRenderVideoStreamChange);
                this._renderStream.UnregisterUsage((object)this);
                this._renderStream = (IVideoStream)null;
            }
            this._presentationBuilder = (VideoPresentationBuilder)null;
            this._clients.Clear();
        }

        public int StreamID
        {
            get
            {
                UIDispatcher.VerifyOnApplicationThread();
                return this._renderStream.StreamID;
            }
        }

        public float ContentOverscanPercent
        {
            get
            {
                UIDispatcher.VerifyOnApplicationThread();
                return this._contentOverscanPer;
            }
            set
            {
                UIDispatcher.VerifyOnApplicationThread();
                if ((double)value < 0.0 || (double)value > 0.5)
                    throw new ArgumentException("Valdid range for content overscan is [0, .5]");
                if (Math2.WithinEpsilon(this._contentOverscanPer, value))
                    return;
                this._contentOverscanPer = value;
                this.Invalidate(true);
            }
        }

        public int ContentWidth
        {
            get
            {
                UIDispatcher.VerifyOnApplicationThread();
                return this._srcVideo.Width;
            }
            set
            {
                UIDispatcher.VerifyOnApplicationThread();
                if (value < 0)
                    throw new ArgumentException("ContentWidth cannot be negative");
                if (this._srcVideo.Width == value)
                    return;
                this._srcVideo.Width = value;
                this.Invalidate(true);
            }
        }

        public int ContentHeight
        {
            get
            {
                UIDispatcher.VerifyOnApplicationThread();
                return this._srcVideo.Height;
            }
            set
            {
                UIDispatcher.VerifyOnApplicationThread();
                if (value < 0)
                    throw new ArgumentException("ContentHeight cannot be negative");
                if (this._srcVideo.Height == value)
                    return;
                this._srcVideo.Height = value;
                this.Invalidate(true);
            }
        }

        public int ContentAspectWidth
        {
            get
            {
                UIDispatcher.VerifyOnApplicationThread();
                return this._srcAspect.Width;
            }
            set
            {
                UIDispatcher.VerifyOnApplicationThread();
                if (value < 0)
                    throw new ArgumentException("ContentAspectWidth cannot be negative");
                if (this._srcAspect.Width == value)
                    return;
                this._srcAspect.Width = value;
                this.Invalidate(true);
            }
        }

        public int ContentAspectHeight
        {
            get
            {
                UIDispatcher.VerifyOnApplicationThread();
                return this._srcAspect.Height;
            }
            set
            {
                UIDispatcher.VerifyOnApplicationThread();
                if (value < 0)
                    throw new ArgumentException("ContentAspectHeight cannot be negative");
                if (this._srcAspect.Height == value)
                    return;
                this._srcAspect.Height = value;
                this.Invalidate(true);
            }
        }

        public WindowPosition DisplayPosition => this._largestPosition;

        public WindowSize DisplaySize => this._largestSize;

        public bool DisplayVisibility => this._isRendering;

        internal IVideoStream RenderStream => this._renderStream;

        void IUIVideoStream.RegisterPortal(IUIVideoPortal portal)
        {
            portal.PortalChange += new EventHandler(this.OnVideoClientChange);
            this._clients.Add((object)portal);
            portal.OnStreamChange(true);
            this.Invalidate(false);
        }

        void IUIVideoStream.RevokePortal(IUIVideoPortal portal)
        {
            portal.PortalChange -= new EventHandler(this.OnVideoClientChange);
            if (this._clients.Contains((object)portal))
            {
                this._clients.Remove((object)portal);
            }
            else
            {
                int num = this._disposed ? 1 : 0;
            }
            portal.OnRevokeStream();
            this.Invalidate(false);
        }

        BasicVideoPresentation IUIVideoStream.GetPresentation(
          IUIVideoPortal portal)
        {
            BasicVideoPresentation videoPresentation = (BasicVideoPresentation)null;
            if (!this._disposed)
            {
                this._presentationBuilder.CompleteDestination = RectangleF.FromRectangle(portal.LogicalContentRect);
                this._presentationBuilder.DestinationAspectRatio = new SizeF((float)portal.LogicalContentRect.Width, (float)portal.LogicalContentRect.Height);
                videoPresentation = this._presentationBuilder.BuildPresentation();
            }
            return videoPresentation;
        }

        private void OnVideoClientChange(object sender, EventArgs args)
        {
            this.Invalidate(false);
            Rectangle rectangle1 = new Rectangle();
            int num1 = 0;
            foreach (IUIVideoPortal client in this._clients)
            {
                if (client.IsUIVisible)
                {
                    Rectangle rectangle2 = client.EstimatePosition((IZoneDisplayChild)null);
                    int num2 = rectangle2.Width * rectangle2.Height;
                    if (num2 > num1)
                    {
                        num1 = num2;
                        rectangle1 = rectangle2;
                    }
                }
            }
            this._largestPosition = new WindowPosition(rectangle1.Left, rectangle1.Top);
            this._largestSize = new WindowSize(rectangle1.Width, rectangle1.Height);
            this.Invalidate(false);
        }

        private void OnRenderVideoStreamChange()
        {
            this._srcAspect.Height = this._renderStream.ContentAspectHeight;
            this._srcAspect.Width = this._renderStream.ContentAspectWidth;
            this._srcVideo.Height = this._renderStream.ContentHeight;
            this._srcVideo.Width = this._renderStream.ContentWidth;
            this.Invalidate(true);
        }

        public event EventHandler DisplayDetailsChanged;

        private void Invalidate(bool streamChange)
        {
            if (this._deferredInvalidate)
                return;
            DeferredCall.Post(DispatchPriority.AppEvent, new DeferredHandler(this.InvalidateWorker), (object)streamChange);
            this._deferredInvalidate = true;
        }

        private void InvalidateWorker(object param)
        {
            bool fFormatChanged = (bool)param;
            bool flag = this._srcAspect.Width > 0 && this._srcAspect.Height > 0 && this._srcVideo.Width >= 0 && this._srcVideo.Height >= 0;
            if (fFormatChanged && flag)
            {
                this._presentationBuilder.SourceDimensions = new SizeF((float)this._srcVideo.Width, (float)this._srcVideo.Height);
                this._presentationBuilder.ContentAspectRatio = new SizeF((float)this._srcAspect.Width, (float)this._srcAspect.Height);
                this._presentationBuilder.ContentOverscanFactor = this._contentOverscanPer * 100f;
            }
            this._isRendering = false;
            foreach (IUIVideoPortal client in this._clients)
            {
                client.OnStreamChange(fFormatChanged);
                if (client.IsUIVisible)
                    this._isRendering = true;
            }
            this._deferredInvalidate = false;
            this.FireDisplayDetailsChanged();
        }

        private void FireDisplayDetailsChanged()
        {
            if (this.DisplayDetailsChanged == null)
                return;
            this.DisplayDetailsChanged((object)this, EventArgs.Empty);
        }
    }
}
