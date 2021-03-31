// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Drawing.Camera
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.Iris.Drawing
{
    internal class Camera : SharedDisposableObject, INotifyObject, IAnimatableOwner
    {
        private ICamera _camera;
        private Vector3 _vEyeNoSend;
        private Vector3 _vAtNoSend;
        private Vector3 _vUpNoSend;
        private float _flZnNoSend;
        private bool _hasEyeNoSend;
        private bool _hasAtNoSend;
        private bool _hasUpNoSend;
        private bool _hasZnNoSend;
        private Vector<ActiveSequence> _activeAnimations;
        private IAnimationProvider _eyeAnimation;
        private IAnimationProvider _atAnimation;
        private IAnimationProvider _upAnimation;
        private IAnimationProvider _znAnimation;
        private NotifyService _notifier = new NotifyService();
        private IList _listeners = new ArrayList();

        public Camera() => this._camera = UISession.Default.RenderSession.CreateCamera(this);

        protected override void OnDispose()
        {
            base.OnDispose();
            Vector<ActiveSequence> activeAnimations = this.GetActiveAnimations(false);
            if (activeAnimations != null)
            {
                foreach (DisposableObject disposableObject in activeAnimations)
                    disposableObject.Dispose(this);
            }
            this._camera.UnregisterUsage(this);
            this._camera = null;
        }

        public void AddListener(Listener listener) => this._notifier.AddListener(listener);

        protected void FireNotification(string id)
        {
            this._notifier.Fire(id);
            foreach (object listener in _listeners)
            {
                if (listener is ViewItem viewItem)
                    viewItem.MarkPaintInvalid();
            }
        }

        public override void RegisterUsage(object consumer)
        {
            base.RegisterUsage(consumer);
            this._listeners.Add(consumer);
        }

        public override void UnregisterUsage(object consumer)
        {
            this._listeners.Remove(consumer);
            base.UnregisterUsage(consumer);
        }

        public bool Perspective
        {
            get => this._camera.Perspective;
            set
            {
                if (this._camera.Perspective == value)
                    return;
                this._camera.Perspective = value;
                this.FireNotification(NotificationID.Perspective);
            }
        }

        public Vector3 Eye
        {
            get => this._hasEyeNoSend ? this._vEyeNoSend : this._camera.Eye;
            set
            {
                if (!(this._camera.Eye != value))
                    return;
                this.CameraEyeNoSend = value;
                if (this._eyeAnimation == null || !this.PlayAnimation(this._eyeAnimation, null))
                    this.CameraEye = value;
                this.FireNotification(NotificationID.Eye);
            }
        }

        private Vector3 CameraEyeNoSend
        {
            set
            {
                this._hasEyeNoSend = true;
                this._vEyeNoSend = value;
            }
        }

        internal Vector3 CameraEye
        {
            set
            {
                this._camera.Eye = value;
                this._hasEyeNoSend = false;
            }
        }

        public Vector3 At
        {
            get => this._hasAtNoSend ? this._vAtNoSend : this._camera.At;
            set
            {
                if (!(this._camera.At != value))
                    return;
                this.CameraAtNoSend = value;
                if (this._atAnimation == null || !this.PlayAnimation(this._atAnimation, null))
                    this.CameraAt = value;
                this.FireNotification(NotificationID.At);
            }
        }

        private Vector3 CameraAtNoSend
        {
            set
            {
                this._hasAtNoSend = true;
                this._vAtNoSend = value;
            }
        }

        internal Vector3 CameraAt
        {
            set
            {
                this._camera.At = value;
                this._hasAtNoSend = false;
            }
        }

        public Vector3 Up
        {
            get => this._hasUpNoSend ? this._vUpNoSend : this._camera.Up;
            set
            {
                if (!(this._camera.Up != value))
                    return;
                this.CameraUpNoSend = value;
                if (this._upAnimation == null || !this.PlayAnimation(this._upAnimation, null))
                    this.CameraUp = value;
                this.FireNotification(NotificationID.Up);
            }
        }

        private Vector3 CameraUpNoSend
        {
            set
            {
                this._hasUpNoSend = true;
                this._vUpNoSend = value;
            }
        }

        internal Vector3 CameraUp
        {
            set
            {
                this._camera.Up = value;
                this._hasUpNoSend = false;
            }
        }

        public float Zn
        {
            get => this._hasZnNoSend ? this._flZnNoSend : this._camera.Zn;
            set
            {
                if (_camera.Zn == (double)value)
                    return;
                this.CameraZnNoSend = value;
                if (this._znAnimation == null || !this.PlayAnimation(this._znAnimation, null))
                    this.CameraZn = value;
                this.FireNotification(NotificationID.Zn);
            }
        }

        private float CameraZnNoSend
        {
            set
            {
                this._hasZnNoSend = true;
                this._flZnNoSend = value;
            }
        }

        internal float CameraZn
        {
            set
            {
                this._camera.Zn = value;
                this._hasZnNoSend = false;
            }
        }

        internal ICamera APICamera => this._camera;

        IAnimatable IAnimatableOwner.AnimationTarget => _camera;

        public IAnimationProvider EyeAnimation
        {
            get => this._eyeAnimation;
            set
            {
                if (this._eyeAnimation == value)
                    return;
                this._eyeAnimation = value;
                this.FireNotification(NotificationID.EyeAnimation);
            }
        }

        public IAnimationProvider AtAnimation
        {
            get => this._atAnimation;
            set
            {
                if (this._atAnimation == value)
                    return;
                this._atAnimation = value;
                this.FireNotification(NotificationID.AtAnimation);
            }
        }

        public IAnimationProvider UpAnimation
        {
            get => this._upAnimation;
            set
            {
                if (this._upAnimation == value)
                    return;
                this._upAnimation = value;
                this.FireNotification(NotificationID.UpAnimation);
            }
        }

        public IAnimationProvider ZnAnimation
        {
            get => this._znAnimation;
            set
            {
                if (this._znAnimation == value)
                    return;
                this._znAnimation = value;
                this.FireNotification(NotificationID.ZnAnimation);
            }
        }

        public bool PlayAnimation(IAnimationProvider ab, AnimationHandle animationHandle)
        {
            AnimationArgs args = new AnimationArgs(this);
            return this.PlayAnimation(ab, ref args, UIClass.ShouldPlayAnimation(ab), animationHandle);
        }

        private bool PlayAnimation(
          IAnimationProvider ab,
          ref AnimationArgs args,
          bool shouldPlayAnimation,
          AnimationHandle animationHandle)
        {
            AnimationTemplate anim = this.BuildAnimation(ab, ref args);
            if (anim == null)
                return false;
            if (shouldPlayAnimation)
            {
                this.PlayAnimation(anim, ref args, null, animationHandle);
            }
            else
            {
                this.ApplyFinalAnimationState(anim, ref args);
                animationHandle?.FireCompleted();
            }
            return true;
        }

        private void PlayAnimation(
          AnimationTemplate anim,
          ref AnimationArgs args,
          EventHandler onCompleteHandler,
          AnimationHandle animationHandle)
        {
            ActiveSequence instance = anim.CreateInstance(APICamera, ref args);
            if (instance == null)
                return;
            instance.DeclareOwner(this);
            if (onCompleteHandler != null)
                instance.AnimationCompleted += onCompleteHandler;
            animationHandle?.AssociateWithAnimationInstance(instance);
            instance.AnimationCompleted += new EventHandler(this.OnAnimationComplete);
            this.GetActiveAnimations(true).Add(instance);
            this.OnAnimationListChanged();
            this.PlayAnimationWorker(instance);
            if (!(anim is Animation animation))
                return;
            int num = animation.DisableMouseInput ? 1 : 0;
        }

        private void PlayAnimationWorker(ActiveSequence newSequence)
        {
            this.StopOverlappingAnimations(newSequence);
            newSequence.Play();
        }

        private AnimationTemplate BuildAnimation(
          IAnimationProvider ab,
          ref AnimationArgs args)
        {
            return ab.Build(ref args);
        }

        private void StopOverlappingAnimations(ActiveSequence newSequence)
        {
            Vector<ActiveSequence> activeAnimations = this.GetActiveAnimations(false);
            if (activeAnimations == null)
                return;
            ActiveTransitions activeTransitions = newSequence.GetActiveTransitions();
            StopCommandSet stopCommand = null;
            foreach (ActiveSequence playingSequence in activeAnimations)
                this.StopAnimationIfOverlapping(playingSequence, newSequence, activeTransitions, ref stopCommand);
        }

        private bool StopAnimationIfOverlapping(
          ActiveSequence playingSequence,
          ActiveSequence newSequence,
          ActiveTransitions newTransitions,
          ref StopCommandSet stopCommand)
        {
            if (playingSequence.Target == newSequence.Target)
            {
                ActiveTransitions activeTransitions = playingSequence.GetActiveTransitions();
                if ((newTransitions & activeTransitions) != ActiveTransitions.None)
                {
                    if (stopCommand == null)
                    {
                        stopCommand = newSequence.GetStopCommandSet();
                        StopCommandSet stopCommandSet = stopCommand;
                    }
                    playingSequence.Stop(stopCommand);
                    return true;
                }
            }
            return false;
        }

        private void OnAnimationComplete(object sender, EventArgs args)
        {
            ActiveSequence activeSequence = sender as ActiveSequence;
            activeSequence.AnimationCompleted -= new EventHandler(this.OnAnimationComplete);
            Vector<ActiveSequence> activeAnimations = this.GetActiveAnimations(false);
            activeAnimations.Remove(activeSequence);
            this.OnAnimationListChanged();
            if (activeAnimations.Count == 0)
                this._activeAnimations = null;
            if (activeSequence.Template is Animation template)
            {
                int num = template.DisableMouseInput ? 1 : 0;
            }
            activeSequence.Dispose(this);
        }

        private void StopActiveAnimations()
        {
            if (this._activeAnimations == null)
                return;
            foreach (ActiveSequence activeAnimation in this.GetActiveAnimations(true))
                activeAnimation.Stop();
        }

        internal void ApplyFinalAnimationState(AnimationTemplate anim, ref AnimationArgs args)
        {
            BaseKeyframe[] baseKeyframeArray = new BaseKeyframe[20];
            foreach (BaseKeyframe keyframe in anim.Keyframes)
            {
                BaseKeyframe baseKeyframe = baseKeyframeArray[(uint)keyframe.Type];
                if (baseKeyframe == null || baseKeyframe.Time <= (double)keyframe.Time)
                    baseKeyframeArray[(uint)keyframe.Type] = keyframe;
            }
            foreach (BaseKeyframe baseKeyframe in baseKeyframeArray)
                baseKeyframe?.Apply(this, ref args);
        }

        private Vector<ActiveSequence> GetActiveAnimations(bool createIfNone) => this.GetAnimationSequence(ref this._activeAnimations, createIfNone);

        private Vector<ActiveSequence> GetAnimationSequence(
          ref Vector<ActiveSequence> currentAnimationsList,
          bool createIfNone)
        {
            Vector<ActiveSequence> vector = null;
            if (currentAnimationsList == null)
            {
                if (createIfNone)
                {
                    vector = new Vector<ActiveSequence>();
                    currentAnimationsList = vector;
                }
            }
            else
                vector = currentAnimationsList;
            return vector;
        }

        private void OnAnimationListChanged()
        {
        }

        private static bool DoesAnimationListContainAnimationType(
          Vector<ActiveSequence> animationList,
          ActiveTransitions type)
        {
            if (animationList != null)
            {
                foreach (ActiveSequence animation in animationList)
                {
                    if ((animation.GetActiveTransitions() & type) == type)
                        return true;
                }
            }
            return false;
        }

        [Conditional("DEBUG")]
        private void DEBUG_DumpAnimation(ActiveSequence aseq)
        {
            foreach (BaseKeyframe keyframe in aseq.Template.Keyframes)
                ;
        }
    }
}
