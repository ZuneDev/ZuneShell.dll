// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.ReferenceAnimation
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;

namespace Microsoft.Iris.Animations
{
    internal abstract class ReferenceAnimation : IAnimationProvider
    {
        private IAnimationProvider _sourceAnimation;

        public IAnimationProvider Source
        {
            get => this._sourceAnimation;
            set
            {
                if (this._sourceAnimation == value)
                    return;
                this._sourceAnimation = value;
                this.OnSourceChanged();
            }
        }

        protected virtual void OnSourceChanged()
        {
        }

        public AnimationEventType Type => this._sourceAnimation != null ? this._sourceAnimation.Type : AnimationEventType.Idle;

        public AnimationTemplate Build(ref AnimationArgs args) => this.BuildWorker(ref args);

        protected virtual AnimationTemplate BuildWorker(ref AnimationArgs args) => this._sourceAnimation.Build(ref args);

        public override string ToString() => InvariantString.Format("{0}({1})", (object)this.GetType().Name, (object)this.Source);

        public virtual bool CanCache => this._sourceAnimation == null || this._sourceAnimation.CanCache;
    }
}
