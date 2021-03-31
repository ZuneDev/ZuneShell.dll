// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.BaseFloatKeyframe
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;

namespace Microsoft.Iris.Animations
{
    internal abstract class BaseFloatKeyframe : BaseKeyframe
    {
        private float _value;

        protected override void PopulateAnimationWorker(
          IAnimatable targetObject,
          AnimationProxy animation,
          ref AnimationArgs args)
        {
            float effectiveValue = this.GetEffectiveValue(targetObject, this._value, ref args);
            animation.AddFloatKeyframe((BaseKeyframe)this, effectiveValue);
        }

        public float Value
        {
            get => this._value;
            set => this._value = value;
        }

        public override object ObjectValue => (object)this.Value;

        public virtual float GetEffectiveValue(
          IAnimatable targetObject,
          float baseValue,
          ref AnimationArgs args)
        {
            return baseValue;
        }

        public override void Apply(IAnimatableOwner animationTarget, ref AnimationArgs args)
        {
            float effectiveValue = this.GetEffectiveValue(animationTarget.AnimationTarget, this._value, ref args);
            this.Apply(animationTarget, effectiveValue);
        }

        public abstract void Apply(IAnimatableOwner animationTarget, float value);

        public override void MagnifyValue(float magnifyValue) => this.Value *= magnifyValue;
    }
}
