// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.Scroller
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Layout;
using Microsoft.Iris.Layouts;
using Microsoft.Iris.Markup;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.ViewItems
{
    internal class Scroller : Clip
    {
        private ScrollModel _model;

        public Scroller()
        {
            this.Layout = (ILayout)new ScrollingLayout(this.Orientation, 50);
            this.ScrollModel = new ScrollModel();
        }

        protected override void OnDispose()
        {
            this.ScrollModel = (ScrollModel)null;
            base.OnDispose();
        }

        public override void OnOrientationChanged()
        {
            this._model.ScrollOrientation = this.Orientation;
            ((ScrollingLayout)this.Layout).Orientation = this.Orientation;
            this.MarkLayoutInvalid();
        }

        public ScrollModel ScrollModel
        {
            get => this._model;
            set
            {
                if (this._model == value)
                    return;
                if (this._model != null)
                    this._model.DetachFromViewItem((ViewItem)this);
                this._model = value;
                if (this._model != null)
                {
                    this._model.AttachToViewItem((ViewItem)this);
                    this._model.ScrollOrientation = this.Orientation;
                }
                this.FireNotification(NotificationID.ScrollModel);
            }
        }

        public ScrollIntoViewDisposition ScrollIntoViewDisposition
        {
            get => this.ScrollModel.ScrollIntoViewDisposition;
            set => this.ScrollModel.ScrollIntoViewDisposition = value;
        }

        public int Prefetch
        {
            get => ((ScrollingLayout)this.Layout).Prefetch;
            set
            {
                if (this.Prefetch == value)
                    return;
                ((ScrollingLayout)this.Layout).Prefetch = value;
                this.MarkLayoutInvalid();
                this.FireNotification(NotificationID.Prefetch);
            }
        }
    }
}
