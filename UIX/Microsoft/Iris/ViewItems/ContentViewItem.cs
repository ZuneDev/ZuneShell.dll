// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.ContentViewItem
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.ViewItems
{
    internal abstract class ContentViewItem : ViewItem
    {
        protected ISprite _contents;

        protected override ISprite ContentVisual => this._contents != null ? this._contents : base.ContentVisual;

        protected abstract bool HasContent();

        protected override void DisposeAllContent()
        {
            base.DisposeAllContent();
            this.DisposeContent(true);
        }

        protected virtual void DisposeContent(bool removeFromTree)
        {
            if (this._contents == null)
                return;
            if (removeFromTree)
                this._contents.Remove();
            this._contents.UnregisterUsage((object)this);
            this._contents = (ISprite)null;
        }

        protected virtual void CreateContent()
        {
            ISprite contentVisual = this.ContentVisual;
            this._contents = UISession.Default.RenderSession.CreateSprite((object)this, (object)this);
            if (contentVisual != null)
                this.VisualContainer.AddChild((IVisual)this._contents, (IVisual)contentVisual, VisualOrder.Before);
            else
                this.VisualContainer.AddChild((IVisual)this._contents, (IVisual)null, VisualOrder.Last);
        }

        public override void OrphanVisuals(OrphanedVisualCollection orphans)
        {
            base.OrphanVisuals(orphans);
            this.DisposeContent(false);
        }

        protected override void OnPaint(bool visible)
        {
            base.OnPaint(visible);
            bool flag = visible && this.HasContent();
            if (flag && this._contents == null)
            {
                this.CreateContent();
            }
            else
            {
                if (flag || this._contents == null)
                    return;
                this.DisposeContent(true);
            }
        }
    }
}
