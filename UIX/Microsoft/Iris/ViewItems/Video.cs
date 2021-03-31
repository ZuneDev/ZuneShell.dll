// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.Video
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Drawing;
using Microsoft.Iris.RenderAPI.VideoPlayback;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.ViewItems
{
    internal class Video : ContentViewItem, IUIVideoPortal, ITrackableUIElement
    {
        private const int c_spriteBorderCount = 2;
        private Color _fillLetterbox;
        private IUIVideoStream _videoStream;
        private ISprite[] _letterBoxSprites;
        private EventHandler _portalChangeEvent;

        public Video()
        {
            this._fillLetterbox = Color.Black;
            ((ITrackableUIElementEvents)this).UIChange += new EventHandler(this.OnUIChange);
        }

        protected override void OnDispose()
        {
            if (this._videoStream != null)
                this._videoStream.RevokePortal(this);
            this._videoStream = null;
            ((ITrackableUIElementEvents)this).UIChange -= new EventHandler(this.OnUIChange);
            base.OnDispose();
        }

        public IUIVideoStream VideoStream
        {
            get => this._videoStream;
            set
            {
                if (this._videoStream == value)
                    return;
                this.ForceContentChange();
                if (this._videoStream != null)
                    this._videoStream.RevokePortal(this);
                this._videoStream = value;
                if (this._videoStream != null)
                    this._videoStream.RegisterPortal(this);
                this.FireNotification(NotificationID.VideoStream);
            }
        }

        public Color LetterboxColor
        {
            get => this._fillLetterbox;
            set
            {
                if (!(value != this.LetterboxColor))
                    return;
                this._fillLetterbox = value;
                if (this._letterBoxSprites != null)
                    this._letterBoxSprites[0].Effect.SetProperty("ColorElem.Color", this._fillLetterbox.RenderConvert());
                this.FireNotification(NotificationID.LetterboxColor);
            }
        }

        protected override bool HasContent() => this._videoStream != null;

        protected override void CreateContent()
        {
            base.CreateContent();
            if (!UISession.Default.RenderSession.GraphicsDevice.IsVideoComposited)
                return;
            IRenderSession renderSession = UISession.Default.RenderSession;
            VideoElement videoElement = new VideoElement("VideoElement", null);
            IEffectTemplate effectTemplate = renderSession.CreateEffectTemplate(this, nameof(Video));
            effectTemplate.Build(videoElement);
            IEffect instance = effectTemplate.CreateInstance(this);
            this._contents.Effect = instance;
            instance.UnregisterUsage(this);
            effectTemplate.UnregisterUsage(this);
            this._contents.Effect.SetProperty("VideoElement.Video", (this._videoStream as Microsoft.Iris.VideoStream).RenderStream);
            this.CreateBorderSprites(renderSession);
        }

        protected override void DisposeContent(bool removeFromTree)
        {
            base.DisposeContent(removeFromTree);
            if (this._letterBoxSprites == null)
                return;
            foreach (ISprite letterBoxSprite in this._letterBoxSprites)
            {
                if (removeFromTree)
                    letterBoxSprite.Remove();
                letterBoxSprite.UnregisterUsage(this);
            }
            this._letterBoxSprites = null;
        }

        protected override void OnPaint(bool visible)
        {
            base.OnPaint(visible);
            if (this._contents == null || !UISession.Default.RenderSession.GraphicsDevice.IsVideoComposited)
                return;
            BasicVideoPresentation presentation = this._videoStream.GetPresentation(this);
            this._contents.Position = new Vector3(presentation.DisplayedDestination.Left, presentation.DisplayedDestination.Top, 0.0f);
            this._contents.Size = new Vector2(presentation.DisplayedDestination.Size.Width, presentation.DisplayedDestination.Size.Height);
            BasicVideoGeometry geometry = presentation.GetGeometry();
            for (int index = 0; index < 2; ++index)
            {
                if (index < geometry.arrcfBorders.Length)
                {
                    RectangleF arrcfBorder = geometry.arrcfBorders[index];
                    this._letterBoxSprites[index].Visible = true;
                    this._letterBoxSprites[index].Position = new Vector3(arrcfBorder.Left, arrcfBorder.Top, 0.0f);
                    this._letterBoxSprites[index].Size = new Vector2(arrcfBorder.Width, arrcfBorder.Height);
                }
                else
                    this._letterBoxSprites[index].Visible = false;
            }
        }

        private void CreateBorderSprites(IRenderSession renderSession)
        {
            this._letterBoxSprites = new ISprite[2];
            IEffect colorFillEffect = EffectManager.CreateColorFillEffect(this, this.LetterboxColor);
            for (int index = 0; index < 2; ++index)
            {
                ISprite sprite = renderSession.CreateSprite(this, this);
                sprite.Effect = colorFillEffect;
                this.VisualContainer.AddChild(sprite, ContentVisual, VisualOrder.Before);
                this._letterBoxSprites[index] = sprite;
            }
            colorFillEffect.UnregisterUsage(this);
        }

        Rectangle IUIVideoPortal.LogicalContentRect => this.HasVisual ? new Rectangle(0.0f, 0.0f, this.VisualSize.X, this.VisualSize.Y) : Rectangle.Zero;

        void IUIVideoPortal.OnStreamChange(bool formatChangedFlag)
        {
            if (formatChangedFlag)
                this.MarkLayoutInvalid();
            this.MarkPaintInvalid();
        }

        void IUIVideoPortal.OnRevokeStream()
        {
            this.MarkLayoutInvalid();
            this.MarkPaintInvalid();
        }

        event EventHandler IUIVideoPortal.PortalChange
        {
            add => this._portalChangeEvent += value;
            remove => this._portalChangeEvent -= value;
        }

        private void OnUIChange(object sender, EventArgs args)
        {
            if (this._portalChangeEvent == null)
                return;
            this._portalChangeEvent(sender, args);
        }
    }
}
