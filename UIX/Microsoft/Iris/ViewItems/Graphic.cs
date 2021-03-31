// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.Graphic
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.ViewItems
{
    internal class Graphic : ContentViewItem
    {
        private const string s_effectElementName = "BaseImage";
        private const string s_effectPropertyName = "BaseImage.Image";
        private static readonly DataCookie s_pendingLoadCompleteHandlerProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_acquiringImageProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_errorImageProperty = DataCookie.ReserveSlot();
        private static string s_AcquiringDefaultImageUri = "res://UIX!AcquiringImage.png";
        private static string s_ErrorDefaultImageUri = "res://UIX!ErrorImage.png";
        private static UIImage s_AcquiringDefaultImage;
        private static UIImage s_ErrorDefaultImage;
        private static object s_NullImage = new object();
        private UIImage _contentImage;
        private UIImage _preloadImage;
        private StripAlignment _horizontalAlignment;
        private StripAlignment _verticalAlignment;
        private StretchingPolicy _stretchMode;

        public Graphic()
        {
            this._horizontalAlignment = StripAlignment.Near;
            this._verticalAlignment = StripAlignment.Near;
            this._stretchMode = StretchingPolicy.Fill;
            this.SizingPolicy = SizingPolicy.SizeToContent;
        }

        public static void EnsureFallbackImages()
        {
            if (s_AcquiringDefaultImage != null)
                return;
            s_AcquiringDefaultImage = new UriImage(s_AcquiringDefaultImageUri, new Inset(3, 3, 53, 53), Size.Zero, false);
            s_AcquiringDefaultImage.Load();
            s_ErrorDefaultImage = new UriImage(s_ErrorDefaultImageUri, new Inset(3, 3, 53, 53), Size.Zero, false);
            s_ErrorDefaultImage.Load();
        }

        protected override void OnDispose()
        {
            this.ReleaseInUseImage(this._contentImage, null);
            this.ReleaseInUseImage(this._preloadImage, null);
            this.ReleaseInUseImage(this.AcquiringImage, s_AcquiringDefaultImage);
            this.ReleaseInUseImage(this.ErrorImage, s_ErrorDefaultImage);
            this._preloadImage = null;
            this._contentImage = null;
            this.AsyncLoadCompleteHandler = null;
            base.OnDispose();
        }

        protected override void OnEffectChanged()
        {
            if (this._contents == null)
                return;
            this._contents.Effect = null;
        }

        public UIImage Content
        {
            get => this._contentImage;
            set
            {
                if (this._contentImage == value)
                    return;
                if (this._contentImage != null)
                {
                    this.RemoveAsyncLoadCompleteHandler(this._contentImage);
                    this._contentImage.RemoveUser(this);
                }
                this._contentImage = value;
                if (this._contentImage != null)
                {
                    this._contentImage.AddUser(this);
                    this._contentImage.Load();
                    if (this._contentImage.Status == ImageStatus.Loading || this._contentImage.Status == ImageStatus.PendingLoad)
                        this.AttachAsyncLoadCompleteHandler(this._contentImage);
                }
                this.FireNotification(NotificationID.Content);
                this.NotifyContentChange();
            }
        }

        public UIImage AcquiringImage
        {
            get => this.GetStatusImage(s_acquiringImageProperty, s_AcquiringDefaultImage);
            set => this.SetStatusImage(value, NotificationID.AcquiringImage, s_acquiringImageProperty, s_AcquiringDefaultImage);
        }

        public UIImage ErrorImage
        {
            get => this.GetStatusImage(s_errorImageProperty, s_ErrorDefaultImage);
            set => this.SetStatusImage(value, NotificationID.ErrorImage, s_errorImageProperty, s_ErrorDefaultImage);
        }

        public UIImage PreloadContent
        {
            get => this._preloadImage;
            set
            {
                if (this._preloadImage != null)
                    this._preloadImage.RemoveUser(this);
                this._preloadImage = value;
                if (this._preloadImage == null)
                    return;
                this._preloadImage.AddUser(this);
                this._preloadImage.Load();
            }
        }

        private UIImage GetStatusImage(DataCookie cookie, UIImage defaultImage)
        {
            object data = this.GetData(cookie);
            return data != null ? (data != s_NullImage ? (UIImage)data : null) : defaultImage;
        }

        private void SetStatusImage(
          UIImage value,
          string name,
          DataCookie cookie,
          UIImage defaultImage)
        {
            UIImage statusImage = this.GetStatusImage(cookie, defaultImage);
            if (value == statusImage)
                return;
            object obj;
            if (value != null)
            {
                obj = value;
                value.Load();
                if (value.Status == ImageStatus.Loading || value.Status == ImageStatus.PendingLoad)
                    this.AttachAsyncLoadCompleteHandler(value);
                value.AddUser(this);
            }
            else
                obj = s_NullImage;
            this.ReleaseInUseImage(statusImage, defaultImage);
            this.SetData(cookie, obj);
            this.MarkPaintInvalid();
            this.FireNotification(name);
        }

        private void OnAsyncLoadComplete(object image, ImageStatus status)
        {
            this.RemoveAsyncLoadCompleteHandler((UIImage)image);
            this.NotifyContentChange();
        }

        private void AttachAsyncLoadCompleteHandler(UIImage image)
        {
            ContentLoadCompleteHandler loadCompleteHandler = this.AsyncLoadCompleteHandler;
            if (loadCompleteHandler == null)
            {
                loadCompleteHandler = new ContentLoadCompleteHandler(this.OnAsyncLoadComplete);
                this.AsyncLoadCompleteHandler = loadCompleteHandler;
            }
            image.LoadComplete += loadCompleteHandler;
        }

        private void RemoveAsyncLoadCompleteHandler(UIImage image)
        {
            if (image == null)
                return;
            image.LoadComplete -= this.AsyncLoadCompleteHandler;
        }

        private void ReleaseInUseImage(UIImage image, UIImage defaultImage)
        {
            if (image != null && image != defaultImage && (image.Status == ImageStatus.Loading || image.Status == ImageStatus.PendingLoad))
                this.RemoveAsyncLoadCompleteHandler(image);
            if (image == null || image == defaultImage)
                return;
            image.RemoveUser(this);
        }

        private void NotifyContentChange()
        {
            this.ForceContentChange();
            if (this.Layout is ImageLayout layout && this.UpdateSourceOnLayout(layout))
                this.MarkLayoutInvalid();
            this.MarkPaintInvalid();
        }

        protected override void OnMinimumSizeChanged()
        {
            if (!(this.Layout is ImageLayout layout))
                return;
            layout.MinimumSize = this.MinimumSize;
        }

        public SizingPolicy SizingPolicy
        {
            get
            {
                if (!(this.Layout is ImageLayout layout))
                    return SizingPolicy.SizeToChildren;
                return layout.Fill ? SizingPolicy.SizeToConstraint : SizingPolicy.SizeToContent;
            }
            set
            {
                if (value == this.SizingPolicy)
                    return;
                if (value == SizingPolicy.SizeToChildren)
                {
                    this.Layout = DefaultLayout.Instance;
                }
                else
                {
                    ImageLayout imageLayout = new ImageLayout();
                    this.UpdateSourceOnLayout(imageLayout);
                    this.UpdateMaintainAspectRatioOnLayout(imageLayout);
                    imageLayout.MinimumSize = this.MinimumSize;
                    imageLayout.Fill = value == SizingPolicy.SizeToConstraint;
                    this.Layout = imageLayout;
                }
                this.FireNotification(NotificationID.SizingPolicy);
            }
        }

        public StretchingPolicy StretchingPolicy
        {
            get => this._stretchMode;
            set
            {
                if (this._stretchMode == value)
                    return;
                this._stretchMode = value;
                if (this.UpdateMaintainAspectRatioOnLayout(this.Layout as ImageLayout))
                    this.MarkLayoutInvalid();
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.StretchingPolicy);
            }
        }

        private bool UpdateMaintainAspectRatioOnLayout(ImageLayout imageLayout)
        {
            bool flag1 = false;
            if (imageLayout != null)
            {
                bool flag2 = this._stretchMode == StretchingPolicy.Uniform;
                if (imageLayout.MaintainAspectRatio != flag2)
                {
                    imageLayout.MaintainAspectRatio = flag2;
                    flag1 = true;
                }
            }
            return flag1;
        }

        private bool UpdateSourceOnLayout(ImageLayout layout)
        {
            bool flag = false;
            Size size = Size.Zero;
            if (this._contentImage != null)
            {
                UIImage activeImage = this.GetActiveImage();
                if (activeImage != null)
                    size = activeImage.Size;
            }
            if (layout.SourceSize != size)
            {
                layout.SourceSize = size;
                flag = true;
            }
            return flag;
        }

        public StripAlignment HorizontalAlignment
        {
            get => this._horizontalAlignment;
            set
            {
                if (this._horizontalAlignment == value)
                    return;
                this._horizontalAlignment = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.HorizontalAlignment);
            }
        }

        public StripAlignment VerticalAlignment
        {
            get => this._verticalAlignment;
            set
            {
                if (this._verticalAlignment == value)
                    return;
                this._verticalAlignment = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.VerticalAlignment);
            }
        }

        private ContentLoadCompleteHandler AsyncLoadCompleteHandler
        {
            get => (ContentLoadCompleteHandler)this.GetData(s_pendingLoadCompleteHandlerProperty);
            set => this.SetData(s_pendingLoadCompleteHandlerProperty, value);
        }

        public void CommitPreload() => this.Content = this.PreloadContent;

        protected override bool HasContent() => this._contentImage != null;

        protected override void OnPaint(bool visible)
        {
            base.OnPaint(visible);
            if (this._contents == null)
                return;
            if (this._contents.Effect == null)
            {
                this._contents.Effect = EffectClass.CreateImageRenderEffectWithFallback(this.Effect, this, null);
                this._contents.Effect.UnregisterUsage(this);
            }
            this.UpdateEffectContents();
            this.UpdateCoordinateMaps();
        }

        private void UpdateEffectContents() => EffectClass.SetDefaultEffectProperty(this.Effect, this._contents.Effect, this.GetActiveImage()?.RenderImage);

        internal void UpdateCoordinateMaps()
        {
            UIImage activeImage = this.GetActiveImage();
            if (activeImage == null)
                return;
            if (this._stretchMode == StretchingPolicy.Fill && activeImage.NineGrid != Inset.Zero)
            {
                this._contents.RelativeSize = true;
                this._contents.Size = Vector2.UnitVector;
                this._contents.SetNineGrid(activeImage.NineGrid.Left, activeImage.NineGrid.Top, activeImage.NineGrid.Right, activeImage.NineGrid.Bottom);
            }
            else
            {
                Vector2 visualSize = this.VisualSize;
                Size layoutSize = new Size((int)visualSize.X, (int)visualSize.Y);
                Size size = activeImage.Size;
                RectangleF source;
                RectangleF destination;
                this.CalculatePaintRectangles(size, layoutSize, out source, out destination);
                switch (size.IsEmpty ? 1 : (int)this._stretchMode)
                {
                    case 0:
                    case 2:
                    case 3:
                        RectangleF rectangleF = new RectangleF(Point.Zero, size);
                        CoordMap coordMap = null;
                        if (source != rectangleF)
                        {
                            float flValue1 = source.Left / size.Width;
                            float flValue2 = source.Right / size.Width;
                            float flValue3 = source.Top / size.Height;
                            float flValue4 = source.Bottom / size.Height;
                            coordMap = new CoordMap();
                            coordMap.AddValue(0.0f, flValue1, Orientation.Horizontal);
                            coordMap.AddValue(0.0f, flValue3, Orientation.Vertical);
                            coordMap.AddValue(1f, flValue2, Orientation.Horizontal);
                            coordMap.AddValue(1f, flValue4, Orientation.Vertical);
                        }
                        this._contents.RelativeSize = false;
                        this._contents.Position = new Vector3(destination.Left, destination.Top, 0.0f);
                        this._contents.Size = new Vector2(destination.Width, destination.Height);
                        this._contents.SetCoordMap(0, coordMap);
                        break;
                    case 1:
                        this._contents.RelativeSize = true;
                        this._contents.Size = Vector2.UnitVector;
                        this._contents.SetCoordMap(0, null);
                        break;
                }
            }
        }

        private UIImage GetActiveImage() => this._contentImage.Status != ImageStatus.Complete ? (this._contentImage.Status == ImageStatus.Loading || this._contentImage.Status == ImageStatus.PendingLoad ? this.AcquiringImage : this.ErrorImage) : this._contentImage;

        private void CalculatePaintRectangles(
          Size originalSourceSize,
          Size layoutSize,
          out RectangleF source,
          out RectangleF destination)
        {
            if (this._stretchMode == StretchingPolicy.Fill)
            {
                source = RectangleF.Zero;
                destination = RectangleF.Zero;
            }
            else
            {
                Size size1 = originalSourceSize;
                Size size2 = layoutSize;
                if (size1.IsZero || size2.IsZero)
                {
                    source = RectangleF.Zero;
                    destination = RectangleF.Zero;
                }
                else
                {
                    switch (this._stretchMode)
                    {
                        case StretchingPolicy.None:
                            size1 = Size.Min(size1, size2);
                            size2 = size1;
                            break;
                        case StretchingPolicy.Uniform:
                            size2 = Size.LargestFit(size1, size2);
                            break;
                        case StretchingPolicy.UniformToFill:
                            size1 = Size.LargestFit(size2, size1);
                            break;
                    }
                    float dimensionOffset1 = this.CalculateDimensionOffset(size1.Width, originalSourceSize.Width, this._horizontalAlignment);
                    float dimensionOffset2 = this.CalculateDimensionOffset(size2.Width, layoutSize.Width, this._horizontalAlignment);
                    float dimensionOffset3 = this.CalculateDimensionOffset(size1.Height, originalSourceSize.Height, this._verticalAlignment);
                    float dimensionOffset4 = this.CalculateDimensionOffset(size2.Height, layoutSize.Height, this._verticalAlignment);
                    source = new RectangleF(dimensionOffset1, dimensionOffset3, size1.Width, size1.Height);
                    destination = new RectangleF(dimensionOffset2, dimensionOffset4, size2.Width, size2.Height);
                }
            }
        }

        private float CalculateDimensionOffset(
          float actualSize,
          float availableSize,
          StripAlignment alignment)
        {
            float num = availableSize - actualSize;
            switch (alignment)
            {
                case StripAlignment.Near:
                    return 0.0f;
                case StripAlignment.Center:
                    return num / 2f;
                case StripAlignment.Far:
                    return num;
                default:
                    return 0.0f;
            }
        }
    }
}
