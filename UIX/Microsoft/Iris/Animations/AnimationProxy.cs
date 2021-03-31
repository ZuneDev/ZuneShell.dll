// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.AnimationProxy
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;

namespace Microsoft.Iris.Animations
{
    internal class AnimationProxy : DisposableObject
    {
        private ActiveSequence _activeSequence;
        private IKeyframeAnimation _animation;
        private int _keyframesValue;
        private bool _playingFlag;
        private AnimationType _type;
        private bool _dynamicFlag;
        private bool _doNotAutoReleaseFlag;
        private IAnimatable _animatableTarget;
        private RendererProperty _rendererProperty;
        private static DeferredHandler s_deferredCleanupWorker = new DeferredHandler(DeferredCleanupWorker);

        internal AnimationProxy(
          ActiveSequence activeSequence,
          IAnimatable animatableTarget,
          AnimationType createType,
          RendererProperty rendererProperty,
          int loopCount,
          StopCommand stopCmd)
        {
            UISession.Validate(activeSequence.Session);
            AnimationSystem.ValidateAnimationType(createType);
            this._activeSequence = activeSequence;
            this._type = createType;
            this._animatableTarget = animatableTarget;
            this._rendererProperty = rendererProperty;
            this._animation = this._activeSequence.Session.AnimationManager.BuildAnimation(this);
            this.CommonCreate(loopCount, stopCmd);
        }

        private void CommonCreate(int loopCount, StopCommand stopCmd)
        {
            this._animation.RepeatCount = loopCount;
            this._animation.AsyncNotifyEvent += new AsyncNotifyHandler(this.OnAsyncNotification);
            this._animation.AddStageEvent(AnimationStage.Complete, new AnimationEvent(_animation, "AsyncNotify", 1U));
            this._animation.AddStageEvent(AnimationStage.Reset, new AnimationEvent(_animation, "AsyncNotify", 2U));
            this.SetStopCommand(stopCmd);
            this._activeSequence.OnAttachChildAnimation(this);
        }

        protected override void OnDispose()
        {
            this.CleanupWorker(0.0f, false);
            base.OnDispose();
        }

        public UISession Session => this._activeSequence.Session;

        public AnimationType Type => this._type;

        public bool HasDynamicKeyframes => this._dynamicFlag;

        internal bool DoNotAutoRelease
        {
            get => this._doNotAutoReleaseFlag;
            set => this._doNotAutoReleaseFlag = value;
        }

        public void AddFloatKeyframe(BaseKeyframe keyframe, float value)
        {
            ++this._keyframesValue;
            AnimationInput animationInput1;
            if (keyframe.IsRelativeToObject)
            {
                this._dynamicFlag = true;
                animationInput1 = keyframe.RelativeTo.CreateAnimationInput(this._animatableTarget, this._rendererProperty.Property, this._rendererProperty.SourceMask);
                if (keyframe.Multiply && value != 1.0)
                {
                    AnimationInput animationInput2 = new ConstantAnimationInput(value);
                    animationInput1 *= animationInput2;
                }
                else if (!keyframe.Multiply && value != 0.0)
                {
                    AnimationInput animationInput2 = new ConstantAnimationInput(value);
                    animationInput1 += animationInput2;
                }
            }
            else
                animationInput1 = new ConstantAnimationInput(value);
            this._animation.AddKeyframe(new AnimationKeyframe(keyframe.Time, animationInput1, GenerateInterpolation(keyframe.Interpolation)));
        }

        public void AddVector2Keyframe(BaseKeyframe keyframe, Vector2 value)
        {
            ++this._keyframesValue;
            AnimationInput animationInput1;
            if (keyframe.IsRelativeToObject)
            {
                this._dynamicFlag = true;
                animationInput1 = keyframe.RelativeTo.CreateAnimationInput(this._animatableTarget, this._rendererProperty.Property, this._rendererProperty.SourceMask);
                if (keyframe.Multiply && value != Vector2.UnitVector)
                {
                    AnimationInput animationInput2 = new ConstantAnimationInput(value);
                    animationInput1 *= animationInput2;
                }
                else if (!keyframe.Multiply && value != Vector2.Zero)
                {
                    AnimationInput animationInput2 = new ConstantAnimationInput(value);
                    animationInput1 += animationInput2;
                }
            }
            else
                animationInput1 = new ConstantAnimationInput(value);
            this._animation.AddKeyframe(new AnimationKeyframe(keyframe.Time, animationInput1, GenerateInterpolation(keyframe.Interpolation)));
        }

        public void AddVector3Keyframe(BaseKeyframe keyframe, Vector3 value)
        {
            ++this._keyframesValue;
            AnimationInput animationInput1;
            if (keyframe.IsRelativeToObject)
            {
                this._dynamicFlag = true;
                animationInput1 = keyframe.RelativeTo.CreateAnimationInput(this._animatableTarget, this._rendererProperty.Property, this._rendererProperty.SourceMask);
                if (keyframe.Multiply && value != Vector3.UnitVector)
                {
                    AnimationInput animationInput2 = new ConstantAnimationInput(value);
                    animationInput1 *= animationInput2;
                }
                else if (!keyframe.Multiply && value != Vector3.Zero)
                {
                    AnimationInput animationInput2 = new ConstantAnimationInput(value);
                    animationInput1 += animationInput2;
                }
            }
            else
                animationInput1 = new ConstantAnimationInput(value);
            this._animation.AddKeyframe(new AnimationKeyframe(keyframe.Time, animationInput1, GenerateInterpolation(keyframe.Interpolation)));
        }

        public void AddVector4Keyframe(BaseKeyframe keyframe, Vector4 value)
        {
            ++this._keyframesValue;
            AnimationInput animationInput1;
            if (keyframe.IsRelativeToObject)
            {
                this._dynamicFlag = true;
                animationInput1 = keyframe.RelativeTo.CreateAnimationInput(this._animatableTarget, this._rendererProperty.Property, this._rendererProperty.SourceMask);
                if (keyframe.Multiply && value != Vector4.UnitVector)
                {
                    AnimationInput animationInput2 = new ConstantAnimationInput(value);
                    animationInput1 *= animationInput2;
                }
                else if (!keyframe.Multiply && value != Vector4.Zero)
                {
                    AnimationInput animationInput2 = new ConstantAnimationInput(value);
                    animationInput1 += animationInput2;
                }
            }
            else
                animationInput1 = new ConstantAnimationInput(value);
            this._animation.AddKeyframe(new AnimationKeyframe(keyframe.Time, animationInput1, GenerateInterpolation(keyframe.Interpolation)));
        }

        public void AddRotationKeyframe(BaseKeyframe keyframe, Rotation value)
        {
            if (keyframe.Type != AnimationType.Orientation)
            {
                this.AddVector4Keyframe(keyframe, new Vector4(value.Axis.X, value.Axis.Y, value.Axis.Z, value.AngleRadians));
            }
            else
            {
                ++this._keyframesValue;
                AnimationInput animationInput1;
                if (keyframe.IsRelativeToObject)
                {
                    this._dynamicFlag = true;
                    animationInput1 = keyframe.RelativeTo.CreateAnimationInput(this._animatableTarget, this._rendererProperty.Property, this._rendererProperty.SourceMask);
                    if (value != Rotation.Default)
                    {
                        AnimationInput animationInput2 = new ConstantAnimationInput(new Quaternion(value.Axis, value.AngleRadians));
                        animationInput1 *= animationInput2;
                    }
                }
                else
                    animationInput1 = new ConstantAnimationInput(new Quaternion(value.Axis, value.AngleRadians));
                AnimationInterpolation interpolation = GenerateInterpolation(keyframe.Interpolation);
                interpolation.UseSphericalCombination = true;
                this._animation.AddKeyframe(new AnimationKeyframe(keyframe.Time, animationInput1, interpolation));
            }
        }

        public void SetStopCommand(StopCommand stopCmd)
        {
            switch (stopCmd)
            {
                case StopCommand.LeaveCurrent:
                    this._animation.ResetBehavior = AnimationResetBehavior.LeaveCurrent;
                    break;
                case StopCommand.MoveToBegin:
                    this._animation.ResetBehavior = AnimationResetBehavior.SetInitialValue;
                    break;
                case StopCommand.MoveToEnd:
                    this._animation.ResetBehavior = AnimationResetBehavior.SetFinalValue;
                    break;
            }
            this._animation.AutoReset = true;
        }

        public bool ValidatePlayable()
        {
            if (this._keyframesValue >= 2)
                return true;
            ErrorManager.ReportError("Animations must have at least 2 keyframes to play");
            return false;
        }

        public void Play()
        {
            if (!this.ValidatePlayable())
                return;
            if (!this._activeSequence.Session.AnimationManager.CanPlayAnimationType(this._type))
            {
                this.Cleanup(0.0f, true);
            }
            else
            {
                if (this._playingFlag || this._animation == null || !this.Session.IsValid)
                    return;
                this._animation.AddTarget(this._animatableTarget, this._rendererProperty.Property, this._rendererProperty.TargetMask);
                this._animation.Play();
                this._playingFlag = true;
            }
        }

        public void Stop() => this.StopWorker(false, StopCommand.MoveToEnd);

        public void Stop(StopCommand stopCommand) => this.StopWorker(true, stopCommand);

        private void StopWorker(bool stopCommandFlag, StopCommand stopCommand)
        {
            if (!this._playingFlag)
                return;
            this._playingFlag = false;
            if (this._animation == null || !this.Session.IsValid)
                return;
            if (stopCommandFlag)
                this.SetStopCommand(stopCommand);
            this._animation.Reset();
            this._animation.RemoveAllTargets();
        }

        private void Cleanup(float progress, bool forceDeferFlag)
        {
            if (this._animation == null)
                return;
            if (UIDispatcher.IsUIThread && !forceDeferFlag)
            {
                this.CleanupWorker(progress, true);
            }
            else
            {
                if (!this.Session.IsValid)
                    return;
                DeferredCall.Post(DispatchPriority.Housekeeping, s_deferredCleanupWorker, this);
            }
        }

        private void CleanupWorker(float progress, bool withNotifications)
        {
            this._playingFlag = false;
            if (this._animation != null)
            {
                this._animation.AsyncNotifyEvent -= new AsyncNotifyHandler(this.OnAsyncNotification);
                this._animation.UnregisterUsage(this);
                this._animation = null;
            }
            this._animatableTarget = null;
            if (!withNotifications)
                return;
            this._activeSequence.OnDetachChildAnimation(this, progress);
        }

        private static void DeferredCleanupWorker(object args) => (args as AnimationProxy).CleanupWorker(0.0f, true);

        private static AnimationInterpolation GenerateInterpolation(
          Interpolation interpolation)
        {
            if (interpolation == null)
                return new LinearInterpolation();
            switch (interpolation.Type)
            {
                case InterpolationType.Linear:
                    return new LinearInterpolation();
                case InterpolationType.SCurve:
                    return new SCurveInterpolation(interpolation.Weight * 10f);
                case InterpolationType.Exp:
                    return interpolation.Weight > 0.0 ? new ExponentialInterpolation(interpolation.Weight * 10f) : (AnimationInterpolation)new LinearInterpolation();
                case InterpolationType.Log:
                    return interpolation.Weight > 0.0 ? new LogarithmicInterpolation(interpolation.Weight * 10f) : (AnimationInterpolation)new LinearInterpolation();
                case InterpolationType.Sine:
                    return new SineInterpolation();
                case InterpolationType.Cosine:
                    return new CosineInterpolation();
                case InterpolationType.Bezier:
                    return new BezierInterpolation(interpolation.BezierHandle1, interpolation.BezierHandle2);
                case InterpolationType.EaseIn:
                    return new EaseInInterpolation(interpolation.Weight * 10f, interpolation.EasePercent);
                case InterpolationType.EaseOut:
                    return new EaseOutInterpolation(interpolation.Weight * 10f, interpolation.EasePercent);
                default:
                    return new LinearInterpolation();
            }
        }

        private void OnAsyncNotification(int nCookie)
        {
            switch (nCookie)
            {
                case 1:
                    this.Cleanup(0.0f, false);
                    break;
                case 2:
                    this.Cleanup(0.0f, false);
                    break;
            }
        }

        private enum AsyncNotifications
        {
            OnComplete = 1,
            OnReset = 2,
        }
    }
}
