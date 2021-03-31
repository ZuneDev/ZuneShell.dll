// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.BaseKeyframe
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Render;
using System;
using System.Text;

namespace Microsoft.Iris.Animations
{
    internal abstract class BaseKeyframe : ICloneable
    {
        private Interpolation _interpolation;
        private RelativeTo _relative;
        private float _timeValue;
        private static RendererProperty[] s_propertyMap = new RendererProperty[20];

        static BaseKeyframe()
        {
            BaseKeyframe.s_propertyMap[0] = new RendererProperty("Position");
            BaseKeyframe.s_propertyMap[1] = new RendererProperty("Size");
            BaseKeyframe.s_propertyMap[2] = new RendererProperty("Alpha");
            BaseKeyframe.s_propertyMap[3] = new RendererProperty("Scale");
            BaseKeyframe.s_propertyMap[4] = new RendererProperty("Rotation");
            BaseKeyframe.s_propertyMap[5] = new RendererProperty("Orientation");
            BaseKeyframe.s_propertyMap[6] = new RendererProperty("Position", "X", "X00");
            BaseKeyframe.s_propertyMap[7] = new RendererProperty("Position", "Y", "0X0");
            BaseKeyframe.s_propertyMap[8] = new RendererProperty("Size", "X", "X0");
            BaseKeyframe.s_propertyMap[9] = new RendererProperty("Size", "Y", "0X");
            BaseKeyframe.s_propertyMap[10] = new RendererProperty("Scale", "X", "X00");
            BaseKeyframe.s_propertyMap[11] = new RendererProperty("Scale", "Y", "0X0");
            BaseKeyframe.s_propertyMap[16] = new RendererProperty("CameraEye");
            BaseKeyframe.s_propertyMap[17] = new RendererProperty("CameraAt");
            BaseKeyframe.s_propertyMap[18] = new RendererProperty("CameraUp");
            BaseKeyframe.s_propertyMap[19] = new RendererProperty("CameraZn");
        }

        public BaseKeyframe()
          : this(0.0f)
        {
        }

        public BaseKeyframe(float timeValue)
        {
            this._timeValue = timeValue;
            this._relative = RelativeTo.Final;
        }

        public void AddtoAnimation(
          AnimationTemplate anim,
          ActiveSequence aseq,
          string property,
          ref AnimationArgs args,
          ref AnimationProxy animation)
        {
            if (animation == null)
                animation = this.CreateProxy(anim, aseq, property);
            this.PopulateAnimationWorker(aseq.Target, animation, ref args);
        }

        protected virtual AnimationProxy CreateProxy(
          AnimationTemplate anim,
          ActiveSequence aseq,
          string property)
        {
            StopCommand stopCommand = anim.GetStopCommand(this.Type);
            RendererProperty rendererProperty = property != null ? new RendererProperty(property) : BaseKeyframe.s_propertyMap[(int)this.Type];
            return new AnimationProxy(aseq, aseq.Target, this.Type, rendererProperty, anim.Loop, stopCommand);
        }

        public BaseKeyframe Clone() => (BaseKeyframe)this.MemberwiseClone();

        object ICloneable.Clone() => this.Clone();

        protected abstract void PopulateAnimationWorker(
          IAnimatable targetObject,
          AnimationProxy animation,
          ref AnimationArgs args);

        public float Time
        {
            get => this._timeValue;
            set => this._timeValue = value;
        }

        public RelativeTo RelativeTo
        {
            get => this._relative == null ? RelativeTo.Absolute : this._relative;
            set => this._relative = value;
        }

        public bool IsRelativeToObject => this.RelativeTo.IsRelativeToObject;

        public Interpolation Interpolation
        {
            get => this._interpolation;
            set => this._interpolation = value;
        }

        public abstract AnimationType Type { get; }

        public abstract object ObjectValue { get; }

        public abstract void Apply(IAnimatableOwner animationTarget, ref AnimationArgs args);

        public abstract void MagnifyValue(float magnifyValue);

        public virtual bool Multiply => false;

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<");
            stringBuilder.Append(this.GetType().Name);
            stringBuilder.Append(" Time=\"");
            stringBuilder.Append(this._timeValue);
            stringBuilder.Append("\"");
            if (this._interpolation != null)
            {
                stringBuilder.Append(" Interpolation=\"");
                stringBuilder.Append(this._interpolation.ToString());
                stringBuilder.Append("\"");
            }
            if (this._relative != RelativeTo.Absolute)
            {
                stringBuilder.Append(" RelativeTo=\"");
                stringBuilder.Append(_relative);
                stringBuilder.Append("\"");
            }
            stringBuilder.Append(" Value=\"");
            stringBuilder.Append(this.ObjectValue);
            stringBuilder.Append("\"");
            stringBuilder.Append("/>");
            return stringBuilder.ToString();
        }
    }
}
