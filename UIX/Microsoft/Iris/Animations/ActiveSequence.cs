// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Animations.ActiveSequence
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using System;
using System.Text;

namespace Microsoft.Iris.Animations
{
    internal class ActiveSequence : DisposableObject
    {
        private UISession _session;
        private IAnimatable _animatableTarget;
        private Vector<AnimationProxy> _ready;
        private Vector<AnimationProxy> _playing;
        private float _lastProgress;
        private AnimationTemplate _template;
        private int _playingCount;

        internal ActiveSequence(
          AnimationTemplate template_,
          IAnimatable animatableTarget,
          UISession session)
        {
            this._animatableTarget = animatableTarget;
            this._session = session;
            this._template = template_;
            this._ready = new Vector<AnimationProxy>();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            Vector<AnimationProxy> animationCollection = this.GetAnimationCollection();
            if (animationCollection != null)
            {
                foreach (DisposableObject disposableObject in animationCollection)
                    disposableObject.Dispose(this);
                this.FireComplete(false);
            }
            this._session = null;
            this._animatableTarget = null;
            this._template = null;
            this._ready = null;
        }

        public UISession Session => this._session;

        internal IAnimatable Target => this._animatableTarget;

        public AnimationTemplate Template => this._template;

        public bool Playing => this._playing != null;

        internal Vector<AnimationProxy> PendingProxies => this._ready;

        public void Play()
        {
            Vector<AnimationProxy> ready = this._ready;
            this._ready = null;
            if (ready.Count > 0)
            {
                foreach (AnimationProxy animationProxy in ready)
                    animationProxy.Play();
                this._playing = ready;
                this.FireStart();
            }
            else
            {
                this.FireStart();
                this.FireComplete(true);
            }
        }

        public void Stop() => this.Stop(null);

        public void Stop(StopCommandSet stopSetCommand)
        {
            if (UIDispatcher.IsUIThread)
            {
                this.StopWorker(stopSetCommand);
            }
            else
            {
                try
                {
                    if (!this.Session.IsValid)
                        return;
                    DeferredCall.Post(DispatchPriority.High, new DeferredHandler(this.DeferredStop), stopSetCommand);
                }
                catch (InvalidOperationException ex)
                {
                }
            }
        }

        private void StopWorker(StopCommandSet stopSetCommand)
        {
            if (this._playing == null)
                return;
            foreach (AnimationProxy animationProxy in this._playing)
            {
                if (stopSetCommand != null)
                    animationProxy.Stop(stopSetCommand[animationProxy.Type]);
                else
                    animationProxy.Stop();
            }
        }

        private void DeferredStop(object args) => this.StopWorker((StopCommandSet)args);

        public bool ValidatePlayable()
        {
            foreach (AnimationProxy animationProxy in this._ready)
            {
                if (!animationProxy.ValidatePlayable())
                    return false;
            }
            return true;
        }

        internal void OnAttachChildAnimation(AnimationProxy child)
        {
            child.DeclareOwner(this);
            this._ready.Add(child);
        }

        private Vector<AnimationProxy> GetAnimationCollection() => this._ready ?? this._playing;

        internal void OnDetachChildAnimation(AnimationProxy child, float progress)
        {
            Vector<AnimationProxy> animationCollection = this.GetAnimationCollection();
            if (animationCollection == null)
                return;
            animationCollection.Remove(child);
            child.Dispose(this);
            if (_lastProgress < (double)progress)
                this._lastProgress = progress;
            if (animationCollection != this._playing || animationCollection.Count != 0)
                return;
            this.FireComplete(true);
        }

        private void FireStart() => this.OnStart();

        private void FireComplete(bool notify)
        {
            if (this._playing != null)
                this._playing = null;
            this.OnStop(this._lastProgress, notify);
        }

        public StopCommandSet GetStopCommandSet()
        {
            StopCommandSet stopCommandSet = null;
            foreach (AnimationProxy animation in this.GetAnimationCollection())
            {
                if (animation.HasDynamicKeyframes)
                {
                    if (stopCommandSet == null)
                        stopCommandSet = new StopCommandSet(StopCommand.MoveToEnd);
                    stopCommandSet[animation.Type] = StopCommand.LeaveCurrent;
                }
            }
            return stopCommandSet;
        }

        public ActiveTransitions GetActiveTransitions()
        {
            Vector<AnimationProxy> animationCollection = this.GetAnimationCollection();
            ActiveTransitions activeTransitions = ActiveTransitions.None;
            foreach (AnimationProxy animationProxy in animationCollection)
            {
                ActiveTransitions activeTransition = ActiveSequence.ConvertToActiveTransition(animationProxy.Type);
                activeTransitions |= activeTransition;
            }
            return activeTransitions;
        }

        public static ActiveTransitions ConvertToActiveTransition(AnimationType type)
        {
            switch (type)
            {
                case AnimationType.Position:
                    return ActiveTransitions.Move;
                case AnimationType.Size:
                    return ActiveTransitions.Size;
                case AnimationType.Alpha:
                    return ActiveTransitions.Alpha;
                case AnimationType.Scale:
                    return ActiveTransitions.Scale;
                case AnimationType.Rotate:
                    return ActiveTransitions.Rotate;
                case AnimationType.Orientation:
                    return ActiveTransitions.Orientation;
                case AnimationType.PositionX:
                    return ActiveTransitions.PositionX;
                case AnimationType.PositionY:
                    return ActiveTransitions.PositionY;
                case AnimationType.SizeX:
                    return ActiveTransitions.SizeX;
                case AnimationType.SizeY:
                    return ActiveTransitions.SizeY;
                case AnimationType.ScaleX:
                    return ActiveTransitions.ScaleX;
                case AnimationType.ScaleY:
                    return ActiveTransitions.ScaleY;
                case AnimationType.Float:
                case AnimationType.Vector2:
                case AnimationType.Vector3:
                    return ActiveTransitions.Effect;
                case AnimationType.CameraEye:
                    return ActiveTransitions.CameraEye;
                case AnimationType.CameraAt:
                    return ActiveTransitions.CameraAt;
                case AnimationType.CameraUp:
                    return ActiveTransitions.CameraUp;
                case AnimationType.CameraZn:
                    return ActiveTransitions.CameraZn;
                default:
                    return ActiveTransitions.None;
            }
        }

        public static ActiveTransitions ConvertToActiveTransition(
          AnimationEventType type)
        {
            switch (type)
            {
                case AnimationEventType.Move:
                    return ActiveTransitions.Move;
                case AnimationEventType.Size:
                    return ActiveTransitions.Size;
                case AnimationEventType.Scale:
                    return ActiveTransitions.Scale;
                case AnimationEventType.Rotate:
                    return ActiveTransitions.Rotate | ActiveTransitions.Orientation;
                case AnimationEventType.Alpha:
                    return ActiveTransitions.Alpha;
                default:
                    return ActiveTransitions.None;
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{ActiveSequence Template=\"");
            if (this._template.DebugID != null)
            {
                stringBuilder.Append(this._template.DebugID);
            }
            else
            {
                Animation template = this._template as Animation;
                if (template != null)
                    stringBuilder.Append(template.Type);
                else
                    stringBuilder.Append("<Unknown>");
            }
            stringBuilder.Append("\", Target=");
            stringBuilder.Append(this._animatableTarget.GetType().Name);
            if (this._animatableTarget is IVisual)
            {
                stringBuilder.Append(", IVisual=");
                stringBuilder.Append(((IVisual)this._animatableTarget).DebugID);
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        public event EventHandler AnimationStarted;

        public event EventHandler AnimationCompleted;

        public event EventHandler AfterAnimationCompleted;

        internal void OnStart()
        {
            ++this._playingCount;
            if (this._playingCount != 1 || this.AnimationStarted == null)
                return;
            this.AnimationStarted(this, EventArgs.Empty);
        }

        internal void OnStop(float progress, bool notify)
        {
            --this._playingCount;
            if (notify && this._playingCount == 0)
            {
                EventArgs e = new AnimationCompleteArgs(progress);
                if (this.AnimationCompleted != null)
                    this.AnimationCompleted(this, e);
                if (this.AfterAnimationCompleted == null)
                    return;
                this.AfterAnimationCompleted(this, e);
            }
            else
            {
                int playingCount = this._playingCount;
            }
        }
    }
}
