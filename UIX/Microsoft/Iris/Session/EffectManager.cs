// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Session.EffectManager
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Render;
using System;

namespace Microsoft.Iris.Session
{
    internal class EffectManager : IDisposable
    {
        public const string ColorEffectProperty = "ColorElem.Color";
        public const string ImageEffectProperty = "ImageElem.Image";
        private const string s_colorElement = "ColorElem";
        private const string s_imageElement = "ImageElem";
        private IRenderSession _renderSession;
        private bool _fDisposed;
        private IEffectTemplate _effectTemplateColor;
        private IEffectTemplate _effectTemplateImage;
        private IEffectTemplate _effectTemplateImageWithColor;

        public EffectManager(IRenderSession renderSession) => this._renderSession = renderSession;

        public void Dispose() => this.Dispose(true);

        private void Dispose(bool fDisposing)
        {
            if (!this._fDisposed && fDisposing)
            {
                if (this._effectTemplateColor != null)
                {
                    this._effectTemplateColor.UnregisterUsage(this);
                    this._effectTemplateColor = null;
                }
                if (this._effectTemplateImage != null)
                {
                    this._effectTemplateImage.UnregisterUsage(this);
                    this._effectTemplateImage = null;
                }
                if (this._effectTemplateImageWithColor != null)
                {
                    this._effectTemplateImageWithColor.UnregisterUsage(this);
                    this._effectTemplateImageWithColor = null;
                }
                this._renderSession = null;
            }
            this._fDisposed = true;
        }

        public IEffectTemplate ColorEffectTemplate
        {
            get
            {
                if (this._effectTemplateColor == null)
                {
                    this._effectTemplateColor = this._renderSession.CreateEffectTemplate(this, "ColorEffect");
                    ColorElement colorElement = new ColorElement("ColorElem");
                    this._effectTemplateColor.AddEffectProperty("ColorElem.Color");
                    this._effectTemplateColor.Build(colorElement);
                }
                return this._effectTemplateColor;
            }
        }

        public IEffectTemplate ImageEffectTemplate
        {
            get
            {
                if (this._effectTemplateImage == null)
                {
                    this._effectTemplateImage = this._renderSession.CreateEffectTemplate(this, "ImageEffect");
                    ImageElement imageElement = new ImageElement("ImageElem", null);
                    this._effectTemplateImage.AddEffectProperty("ImageElem.Image");
                    this._effectTemplateImage.Build(imageElement);
                }
                return this._effectTemplateImage;
            }
        }

        public static IEffect CreateColorFillEffect(object oOwner, Color color)
        {
            IEffect instance = UISession.Default.EffectManager.ColorEffectTemplate.CreateInstance(oOwner);
            instance.SetProperty("ColorElem.Color", color.RenderConvert());
            return instance;
        }

        public static IEffect CreateBasicImageEffect(object oOwner, IImage renderImage)
        {
            IEffect instance = UISession.Default.EffectManager.ImageEffectTemplate.CreateInstance(oOwner);
            if (renderImage != null)
                instance.SetProperty("ImageElem.Image", renderImage);
            return instance;
        }
    }
}
