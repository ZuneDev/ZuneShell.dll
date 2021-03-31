// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.TextRunRenderer
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Layout;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Render;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.ViewItems
{
    internal class TextRunRenderer : ContentViewItem, ILayout
    {
        private TextRunData _data;
        private Color _color;
        private TextFlowRenderingHelper _renderingHelper;
        private PaintInvalidEventHandler _paintHandler;

        public TextRunRenderer()
        {
            this.Layout = (ILayout)this;
            this._renderingHelper = new TextFlowRenderingHelper();
            this._paintHandler = new PaintInvalidEventHandler(this.OnRunPaintInvalid);
        }

        protected override void OnDispose()
        {
            this.Data = (TextRunData)null;
            base.OnDispose();
        }

        public TextRunData Data
        {
            get => this._data;
            set
            {
                if (this._data == value)
                    return;
                if (this._data != null)
                {
                    this._data.PaintInvalid -= this._paintHandler;
                    this._data.Run.UnregisterUsage((object)this);
                }
                this._data = value;
                if (this._data != null)
                {
                    this._data.PaintInvalid += this._paintHandler;
                    this._data.Run.RegisterUsage((object)this);
                }
                this.FireNotification(NotificationID.Data);
            }
        }

        public Color Color
        {
            get => this._color;
            set
            {
                if (!(this._color != value))
                    return;
                this._color = value;
                this.MarkPaintInvalid();
                this.FireNotification(NotificationID.Color);
            }
        }

        public ItemAlignment DefaultChildAlignment => ItemAlignment.Default;

        private void OnRunPaintInvalid()
        {
            this._renderingHelper.InvalidateGradients();
            this.MarkPaintInvalid();
        }

        protected override bool HasContent() => this._data != null;

        protected override void CreateContent() => base.CreateContent();

        protected override void OnEffectChanged()
        {
            if (this._contents == null)
                return;
            this._contents.Effect = (IEffect)null;
        }

        protected override void OnPaint(bool visible)
        {
            base.OnPaint(visible);
            if (this._contents == null || this._data == null)
                return;
            TextRun run = this._data.Run;
            Text textViewItem = this._data.TextViewItem;
            if (!run.Visible)
                return;
            IImage imageForRun = Text.GetImageForRun(this.UISession, this._data.Run, this._color.A != (byte)0 ? this._color : this._data.Color);
            if (this._contents.Effect == null)
            {
                this._contents.Effect = EffectClass.CreateImageRenderEffectWithFallback(this.Effect, (object)this, (IImage)null);
                this._contents.Effect.UnregisterUsage((object)this);
            }
            EffectClass.SetDefaultEffectProperty(this.Effect, this._contents.Effect, imageForRun);
            this._contents.RelativeSize = true;
            this._contents.Size = Vector2.UnitVector;
        }

        Size ILayout.Measure(ILayoutNode layoutNode, Size constraint)
        {
            Size constraint1 = Size.Min(this._data.Size, constraint);
            DefaultLayout.Measure(layoutNode, constraint1);
            return constraint1;
        }

        void ILayout.Arrange(ILayoutNode layoutNode, LayoutSlot slot) => DefaultLayout.Arrange(layoutNode, slot);
    }
}
