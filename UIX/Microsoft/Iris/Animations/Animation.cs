// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.Animation
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using System;
using System.Collections.Specialized;
using System.Text;

namespace Microsoft.Iris.Animations
{
    internal class Animation : AnimationTemplate, IAnimationProvider
    {
        private static readonly DataCookie s_rotationAxisProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_centerPointScaleProperty = DataCookie.ReserveSlot();
        private static readonly DataCookie s_centerPointOffsetProperty = DataCookie.ReserveSlot();
        private BitVector32 _bitsBitVector;
        private AnimationEventType _type;
        private DynamicData _dataMap;

        public Animation()
        {
            this._type = AnimationEventType.Idle;
            this._bitsBitVector = new BitVector32();
            this._dataMap = new DynamicData();
        }

        public override object Clone()
        {
            Animation animation = new Animation();
            this.CloneWorker(animation);
            return animation;
        }

        protected override void CloneWorker(AnimationTemplate rawAnimation)
        {
            base.CloneWorker(rawAnimation);
            Animation animation = (Animation)rawAnimation;
            animation.Type = this.Type;
            if (this.GetBit(Animation.Bits.CenterPointScale))
                animation.CenterPointPercent = this.CenterPointPercent;
            if (this.GetBit(Animation.Bits.RotationAxis))
                animation.RotationAxis = this.RotationAxis;
            animation.DisableMouseInput = this.DisableMouseInput;
        }

        private void PrepareToPlay(ref AnimationArgs args)
        {
            if (this.GetBit(Animation.Bits.CenterPointScale))
                args.ViewItem.VisualCenterPoint = this.CenterPointPercent;
            if (!this.GetBit(Animation.Bits.RotationAxis))
                return;
            Rotation visualRotation = args.ViewItem.VisualRotation;
            args.ViewItem.VisualRotation = new Rotation(visualRotation.AngleRadians, this.RotationAxis);
        }

        public AnimationEventType Type
        {
            get => this._type;
            set => this._type = value;
        }

        public Vector3 CenterPointPercent
        {
            get => !this.GetBit(Animation.Bits.CenterPointScale) ? Vector3.Zero : (Vector3)this.GetData(Animation.s_centerPointScaleProperty);
            set
            {
                if (!(this.CenterPointPercent != value))
                    return;
                this.SetData(Animation.s_centerPointScaleProperty, value);
                this.SetBit(Animation.Bits.CenterPointScale, true);
            }
        }

        public Vector3 RotationAxis
        {
            get => !this.GetBit(Animation.Bits.RotationAxis) ? Rotation.Default.Axis : (Vector3)this.GetData(Animation.s_rotationAxisProperty);
            set
            {
                if (!(this.RotationAxis != value))
                    return;
                this.SetData(Animation.s_rotationAxisProperty, value);
                this.SetBit(Animation.Bits.RotationAxis, true);
            }
        }

        public bool DisableMouseInput
        {
            get => this.GetBit(Animation.Bits.DisableMouseInput);
            set => this.SetBit(Animation.Bits.DisableMouseInput, value);
        }

        AnimationTemplate IAnimationProvider.Build(
          ref AnimationArgs args)
        {
            this.PrepareToPlay(ref args);
            return this;
        }

        public bool CanCache => true;

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{AnimationTemplate ID=");
            stringBuilder.Append(this.DebugID);
            stringBuilder.Append(", Loop=");
            stringBuilder.Append(this.Loop);
            stringBuilder.Append(", KeyframeCount=");
            stringBuilder.Append(this.Keyframes.Count);
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        private bool GetBit(Animation.Bits bit) => this._bitsBitVector[(int)bit];

        private void SetBit(Animation.Bits bit, bool value) => this._bitsBitVector[(int)bit] = value;

        private bool ChangeBit(Animation.Bits bit, bool value)
        {
            if (this._bitsBitVector[(int)bit] == value)
                return false;
            this._bitsBitVector[(int)bit] = value;
            return true;
        }

        protected object GetData(DataCookie cookie) => this._dataMap.GetData(cookie);

        protected void SetData(DataCookie cookie, object value) => this._dataMap.SetData(cookie, value);

        protected Delegate GetEventHandler(EventCookie cookie) => this._dataMap.GetEventHandler(cookie);

        protected void AddEventHandler(EventCookie cookie, Delegate handlerToAdd) => this._dataMap.AddEventHandler(cookie, handlerToAdd);

        protected void RemoveEventHandler(EventCookie cookie, Delegate handlerToRemove) => this._dataMap.RemoveEventHandler(cookie, handlerToRemove);

        protected void RemoveEventHandlers(EventCookie cookie) => this._dataMap.RemoveEventHandlers(cookie);

        private static uint GetKey(EventCookie cookie) => EventCookie.ToUInt32(cookie);

        private enum Bits
        {
            CenterPointScale = 1,
            RotationAxis = 2,
            DisableMouseInput = 4,
        }
    }
}
